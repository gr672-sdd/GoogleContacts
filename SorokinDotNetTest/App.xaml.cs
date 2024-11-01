using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using SorokinDotNetTest.ViewModels;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SorokinDotNetTest
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();

            // Регистрация UserCredential
            serviceCollection.AddSingleton<UserCredential>(provider =>
            {
                return AuthorizeUser().GetAwaiter().GetResult(); // Получаем UserCredential синхронно
            });

            // Регистрация ViewModel
            serviceCollection.AddTransient<ContactsWindowViewModel>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);
        }

        private async Task<UserCredential> AuthorizeUser()
        {
            string[] scopes = { "profile", "https://www.googleapis.com/auth/contacts" };
            var clientSecrets = new ClientSecrets
            {
                ClientId = "ваш_client_id",
                ClientSecret = "ваш_client_secret"
            };

            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                scopes,
                "user",
                CancellationToken.None);
        }
    }
}
