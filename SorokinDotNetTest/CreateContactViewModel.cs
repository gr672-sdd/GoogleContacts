using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1.Data;
using SorokinDotNetTest.Commands;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Xml.Linq;
namespace SorokinDotNetTest.ViewModels
{
    public class CreateContactViewModel : BaseViewModel
    {
        private UserCredential _credential;
        private string _etag;
        private string _actionType;
        private string _resourceName;
        private BitmapImage _contactImage;
        public ICommand SaveContactCommand { get; }
        public ICommand DeleteContactCommand { get; }
        public ICommand UploadImageCommand { get; }
        public ICommand CloseCommand { get; }
        public CreateContactViewModel(UserCredential credential, Person contact, string actionType)
        {
            _credential = credential;
            _etag = contact?.ETag;
            _actionType = actionType;
            _resourceName = contact?.ResourceName;
            // Команды для сохранения, удаления, обновления контакта.
            SaveContactCommand = new RelayCommand(SaveContact);
            DeleteContactCommand = new RelayCommand(DeleteContact);
            UploadImageCommand = new RelayCommand(UploadImage);
            // Команды для закрытия окна и загрузки фотографии
            CloseCommand = new RelayCommand(CloseWindow);
            UploadImageCommand = new RelayCommand(UploadImage);
            // Команды для взаимодействия с интерфейсом

            if (!string.IsNullOrEmpty(_resourceName) && _actionType == "Edit")
            {
                // Находим контакт по resourceName
                if (contact != null)
                {
                    // Инициализируем поля контакта
                    FirstName = contact.Names?.FirstOrDefault()?.GivenName ?? string.Empty;
                    LastName = contact.Names?.FirstOrDefault()?.FamilyName ?? string.Empty;
                    Company = contact.Organizations?.FirstOrDefault()?.Name ?? string.Empty;
                    Position = contact.Organizations?.FirstOrDefault()?.Title ?? string.Empty;
                    Email = contact.EmailAddresses?.FirstOrDefault()?.Value ?? string.Empty;
                    Phone = contact.PhoneNumbers?.FirstOrDefault()?.Value ?? string.Empty;
                    Note = contact.Biographies?.FirstOrDefault()?.Value ?? string.Empty;

                    // Если у контакта есть фото, загрузим его
                    if (contact.Photos != null && contact.Photos.Count > 0)
                    {
                        ContactImage = new BitmapImage(new Uri(contact.Photos[0].Url));
                    }
                }
                else
                {
                    MessageBox.Show("Контакт не найден для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Свойства контакта
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        private string _localImagePath;
        public BitmapImage ContactImage
        {
            get => _contactImage;
            set
            {
                _contactImage = value;
                OnPropertyChanged(nameof(ContactImage));
            }
        }

        private void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _localImagePath = openFileDialog.FileName;
                ContactImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        private async Task UploadContactPhoto(string resourceName, string photoPath)
        {
            try
            {
                var serviceClass = new ServiceClass();
                var service = serviceClass.Credential(_credential);

                // Чтение изображения и конвертация в байтовый массив
                byte[] photoBytes = File.ReadAllBytes(photoPath);

                var updatePhotoRequestBody = new Google.Apis.PeopleService.v1.Data.UpdateContactPhotoRequest
                {
                    PhotoBytes = Convert.ToBase64String(photoBytes)
                };

                var updatePhotoRequest = service.People.UpdateContactPhoto(updatePhotoRequestBody, resourceName);

                await updatePhotoRequest.ExecuteAsync();

                MessageBox.Show("Фотография успешно обновлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении фотографии: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            // Проверка формата электронной почты
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidPhone()
        {
            // Проверка международного номера телефона
            var phonePattern = @"^\+([1-9]{1}[0-9]{1,2})[-. (]?([0-9]{1,4})[-. )]?([0-9]{1,4})[-. ]?([0-9]{1,9})$";
            if (!Phone.StartsWith("+"))
            {
                Phone = "+" + Phone;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(Phone, phonePattern);
        }

        public async void SaveContact()
        {
            // Валидация телефона и почты
            if (!string.IsNullOrEmpty(Email))
            {
                if (!IsValidEmail(Email))
                {
                    MessageBox.Show("Некорректный формат электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                if (!IsValidPhone())
                {
                    MessageBox.Show("Некорректный формат телефонного номера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Создания или редактирования контакта
            try
            {
                if (_actionType == "Edit" && string.IsNullOrEmpty(_resourceName))
                {
                    MessageBox.Show("ResourceName не установлен для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var newContact = new Person
                {
                    Names = new List<Name>
                    {
                        new Name { GivenName = FirstName, FamilyName = LastName }
                    },
                    Organizations = new List<Organization>
                    {
                        new Organization { Name = Company, Title = Position }
                    },
                    EmailAddresses = new List<EmailAddress>
                    {
                        new EmailAddress { Value = Email }
                    },
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Value = Phone }
                    },
                    Biographies = new List<Biography>
                    {
                        new Biography { Value = Note }
                    },
                };
                var serviceClass = new ServiceClass();
                var service = serviceClass.Credential(_credential);
                if (_actionType == "Edit")
                {
                    if (!string.IsNullOrEmpty(_etag))
                    {
                        // Добавляем ETag для редактирования
                        newContact.ETag = _etag;
                    }
                    // Редактирование контакта
                    var updateRequest = service.People.UpdateContact(newContact, _resourceName);
                    updateRequest.UpdatePersonFields = "names,emailAddresses,phoneNumbers,organizations,biographies";
                    await updateRequest.ExecuteAsync();
                    MessageBox.Show("Контакт успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (!string.IsNullOrEmpty(_localImagePath))
                    {
                        await UploadContactPhoto(_resourceName, _localImagePath);
                    }
                }
                else
                {
                    // Создание контакта
                    var createContact = service.People.CreateContact(newContact);
                    createContact.PersonFields = "names,emailAddresses,phoneNumbers,organizations,biographies";
                    var createdContact = await createContact.ExecuteAsync(); // Получаем созданный контакт для работы с его ресурсом

                    MessageBox.Show("Контакт успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Загрузите фотографию после создания контакта
                    if (!string.IsNullOrEmpty(_localImagePath))
                    {
                        await UploadContactPhoto(createdContact.ResourceName, _localImagePath);
                    }
                }
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении контакта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void DeleteContact()
        {
            try
            {
                if (string.IsNullOrEmpty(_resourceName))
                {
                    MessageBox.Show("ResourceName не установлен для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Удаление контакта
                var serviceClass = new ServiceClass();
                var service = serviceClass.Credential(_credential);

                // Удаление контакта по resourceName
                await service.People.DeleteContact(_resourceName).ExecuteAsync();

                MessageBox.Show("Контакт успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении контакта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            CloseWindow();
        }
        private void CloseWindow()
        {
            var contactsWindow = new ContactsWindow
            {
                DataContext = new ContactsWindowViewModel(_credential)
            };
            var currentWindow = Application.Current.Windows.OfType<CreateContact>().FirstOrDefault();
            currentWindow?.Close();
            contactsWindow.Show();
        }
    }
}
