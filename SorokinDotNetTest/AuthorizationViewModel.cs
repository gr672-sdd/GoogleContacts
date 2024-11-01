using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Util.Store;
using SorokinDotNetTest.Commands;

namespace SorokinDotNetTest.ViewModels
{
    public class AuthorizationViewModel : BaseViewModel
    {
        private CancellationTokenSource _cancellationTokenSource;

        public RelayCommand AuthorizeCommand { get; }

        private string _idClient;
        public string IdClient
        {
            get => _idClient;
            set
            {
                if (_idClient != value)
                {
                    _idClient = value;
                    OnPropertyChanged(nameof(IdClient));
                }
            }
        }

        private string _secretClient;
        public string SecretClient
        {
            get => _secretClient;
            set
            {
                if (_secretClient != value)
                {
                    _secretClient = value;
                    OnPropertyChanged(nameof(SecretClient));
                }
            }
        }

        public AuthorizationViewModel()
        {
            AuthorizeCommand = new RelayCommand(async () => await AuthorizeAsync());
        }

        private async Task AuthorizeAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var credential = await AuthorizationAsync(_cancellationTokenSource.Token);
                if (credential != null)
                {
                    // Если авторизация прошла успешно
                    OpenContactsWindow(credential);
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Авторизация была отменена.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Сброс для следующей попытки авторизации
                _cancellationTokenSource = null;
            }
        }

        // Функция авторизации через Google API с использованием client_secret.json или вручную введенных данных
        public async Task<UserCredential> AuthorizationAsync(CancellationToken token)
        {
            try
            {
                ClientSecrets clientSecrets;

                // Проверка на наличие данных для ручной инициализации
                if (string.IsNullOrWhiteSpace(_idClient) || string.IsNullOrWhiteSpace(_secretClient))
                {
                    // Если данные отсутствуют, проверяем наличие client_secret.json
                    if (!File.Exists("client_secret.json"))
                    {
                        MessageBox.Show("Файл client_secret.json не найден, и данные клиента не введены. Пожалуйста, укажите данные клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }

                    // Загружаем данные из client_secret.json
                    using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                    {
                        clientSecrets = GoogleClientSecrets.FromStream(stream).Secrets;
                    }
                }
                else
                {
                    // Инициализация clientSecrets вручную с введенными данными
                    clientSecrets = new ClientSecrets
                    {
                        ClientId = _idClient,
                        ClientSecret = _secretClient
                    };
                }

                var credPath = "token.json";
                // Выполнение авторизации
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    new[] { PeopleServiceService.Scope.Contacts, PeopleServiceService.Scope.ContactsReadonly },
                    "user",
                    token,
                    new FileDataStore(credPath, true));

                return credential;
            }
            catch (TaskCanceledException)
            {
                throw new OperationCanceledException("Авторизация была отменена пользователем или системой.");
            }
            catch (Exception ex)
            {
                // Удаление токена
                if (Directory.Exists("token.json"))
                {
                    Directory.Delete("token.json", true);
                }
                throw new Exception($"Ошибка во время авторизации: {ex.Message}");
            }
        }

        private void OpenContactsWindow(UserCredential credential)
        {
            var contactsWindow = new ContactsWindow
            {
                DataContext = new ContactsWindowViewModel(credential)
            };

            contactsWindow.Show();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
        }
    }
}
