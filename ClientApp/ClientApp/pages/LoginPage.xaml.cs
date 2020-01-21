using ClientApp.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace ClientApp.pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            LoginButton.Content = "LOADING...";
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/nickname/" + LoginTextBox.Text);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                User account = JsonConvert.DeserializeObject<User>(responseBody);
                if (BCrypt.Net.BCrypt.Verify(PasswordField.Password, account.Password))
                {
                    MainWindow newWindow = new MainWindow(account);
                    newWindow.Show();
                    Application.Current.MainWindow.Close();
                }
                else { 
                    LoginButton.IsEnabled = true;
                    LoginButton.Content = "LOGIN";
                    MessageBox.Show("Podane dane są nieprawidłowe");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                LoginButton.IsEnabled = true;
                LoginButton.Content = "LOGIN";
                }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage startPage = new RegisterPage(this);
            NavigationService.Navigate(startPage);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
