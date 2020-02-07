using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using ClientApp.models;
using System.Configuration;
using ClientApp.pages;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoginPage startPage = new LoginPage();
            ContentFrame.Navigate(startPage);
        }

        /// <summary>
        /// Allow LoginWindow move  with dragginf mouse.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
