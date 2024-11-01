using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.PeopleService.v1;
using Google.Apis.Auth.OAuth2;
namespace SorokinDotNetTest
{
    class ServiceClass
    {
        // Создание сервиса
        public PeopleServiceService Credential(UserCredential credential)
        {
            var service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "My App",
            });
            return service;
        }
    }
}
