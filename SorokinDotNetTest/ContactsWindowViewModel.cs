using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1.Data;
using SorokinDotNetTest.Commands; // Для команд

namespace SorokinDotNetTest.ViewModels
{
    public class ContactsWindowViewModel : BaseViewModel // BaseViewModel должен реализовывать INotifyPropertyChanged
    {
        private UserCredential _credential;
        private IList<Person> _peopleList;
        private string _searchText;
        private Person _selectedContact;
        private IList<Person> _originalPeopleList;
        public ICommand CreateContactCommand { get; }
        public ICommand EditContactCommand { get; }
        public ICommand CloseCommand { get; }
        public IList<Person> PeopleList
        {
            get => _peopleList;
            set
            {
                _peopleList = value;
                OnPropertyChanged(nameof(PeopleList));
            }
        }
        public Person SelectedContact  // Свойство для хранения выбранного контакта
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged(nameof(SelectedContact));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                SearchContacts(); // Автоматический поиск при изменении текста
            }
        }

        public ContactsWindowViewModel(UserCredential credential)
        {
            _credential = credential ?? throw new ArgumentNullException(nameof(credential));

            // Загружаем контакты при инициализации
            LoadContactsAsync();

            CreateContactCommand = new RelayCommand(CreateContact);
            EditContactCommand = new RelayCommand<Person>(EditContact);
            CloseCommand = new RelayCommand(CloseWindow);
        }

        public async Task LoadContactsAsync()
        {
            try
            {
                var people = await GetPeopleListAsync();
                if (people == null || !people.Any())
                {
                    MessageBox.Show("Контакты не найдены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Сохраняем оригинальный список
                    _originalPeopleList = people;
                    // Сортируем и показываем контакты
                    PeopleList = _originalPeopleList
                        .OrderBy(x => x.Names?.FirstOrDefault()?.FamilyName)
                        .ThenBy(x => x.Names?.FirstOrDefault()?.GivenName)
                        .ToList();
                    MessageBox.Show($"Загружено {PeopleList.Count} контактов.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки контактов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<IList<Person>> GetPeopleListAsync()
        {
            try
            {
                var serviceClass = new ServiceClass();
                var service = serviceClass.Credential(_credential);

                var peopleRequest = service.People.Connections.List("people/me");
                peopleRequest.PersonFields = "names,emailAddresses,phoneNumbers,organizations,photos,birthdays,biographies";
                var response = await peopleRequest.ExecuteAsync();
                var connections = response.Connections;
                return connections;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении контактов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void SearchContacts()
        {
            if (_originalPeopleList == null || !_originalPeopleList.Any())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Если поле поиска пустое, показываем полный оригинальный список, отсортированный по алфавиту
                PeopleList = _originalPeopleList
                    .OrderBy(x => x.Names?.FirstOrDefault()?.FamilyName)
                    .ThenBy(x => x.Names?.FirstOrDefault()?.GivenName)
                    .ToList();
            }
            else
            {
                // Выполняем поиск по оригинальному списку, и сортируем результат по алфавиту
                PeopleList = _originalPeopleList.Where(x =>
                    (x.Names?.FirstOrDefault()?.FamilyName?.ToLower().Contains(SearchText.ToLower()) ?? false) ||
                    (x.Names?.FirstOrDefault()?.GivenName?.ToLower().Contains(SearchText.ToLower()) ?? false))
                    .OrderBy(x => x.Names?.FirstOrDefault()?.FamilyName)
                    .ThenBy(x => x.Names?.FirstOrDefault()?.GivenName)
                    .ToList();
            }

            // Уведомляем интерфейс об изменении списка
            OnPropertyChanged(nameof(PeopleList));
        }

        private void CreateContact()
        {
            var createContactWindow = new CreateContact(_credential, null, "Create");
            var currentWindow = Application.Current.Windows.OfType<ContactsWindow>().FirstOrDefault();
            currentWindow?.Close();
            createContactWindow.Show();
        }

        private void EditContact(Person contact)
        {
            if (contact == null) return;
            var editContactViewModel = new CreateContactViewModel(_credential, contact, "Edit");

            // Открываем окно для редактирования
            var editContactWindow = new CreateContact(_credential, contact, "Edit")
            {
                DataContext = editContactViewModel
            };
            var currentWindow = Application.Current.Windows.OfType<ContactsWindow>().FirstOrDefault();
            currentWindow?.Close();
            editContactWindow.Show();
        }

        private void CloseWindow()
        {
            if (Directory.Exists("token.json"))
            {
                Directory.Delete("token.json", true);
            }
            var mainWindow = new AuthorizationViewModel();
            var autorizationWindow = new MainWindow();
            var currentWindow = Application.Current.Windows.OfType<ContactsWindow>().FirstOrDefault();
            currentWindow?.Close();
            autorizationWindow.Show();
        }
    }
}
