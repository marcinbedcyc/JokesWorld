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

        /// <summary>
        /// Make Http request to server to get user object. Check password's correctness and logging to application. When something goes wrong showy messagebos with information.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
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

        /// <summary>
        /// Open Page with Registration form
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage startPage = new RegisterPage(this);
            NavigationService.Navigate(startPage);
        }

        /// <summary>
        /// Exit Application
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Logging into the application
        /// </summary>
        /// <seealso cref="LoginPage.LoginButton_Click"/>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(this, new RoutedEventArgs());
            }
        }
    }
}
