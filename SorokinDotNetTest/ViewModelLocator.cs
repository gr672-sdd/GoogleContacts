using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.Extensions.DependencyInjection;
using SorokinDotNetTest.ViewModels;
using System;
using System.Collections.Generic;

namespace SorokinDotNetTest
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceProvider = new ServiceCollection()
                 .AddSingleton<ContactsWindowViewModel>()
                 .BuildServiceProvider();
        }

        public static ServiceProvider ServiceProvider { get; private set; }

        // Свойство для доступа к ContactsWindowViewModel
        public static ContactsWindowViewModel GetContactsWindowViewModel(UserCredential credential)
        {
            return new ContactsWindowViewModel(credential);
        }

    }
}
