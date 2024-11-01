using System;
using System.Collections.Generic;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1.Data;
using SorokinDotNetTest.ViewModels;

namespace SorokinDotNetTest
{
    public partial class CreateContact : Window
    {
        public CreateContact(UserCredential credential, Person contact, string actionType)
        {
            InitializeComponent();

            // Привязываем DataContext к соответствующей ViewModel
            DataContext = new CreateContactViewModel(credential, contact, actionType);
        }
    }
}
