using SorokinDotNetTest.ViewModels;
using System.Windows;

namespace SorokinDotNetTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AuthorizationViewModel(); // Привязываем DataContext
        }
    }
}
