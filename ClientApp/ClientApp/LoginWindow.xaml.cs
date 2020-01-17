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

namespace ClientApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow newWindow1 = new MainWindow();
            newWindow1.Show();
            this.Close();
            LoginButton.IsEnabled = false;
            LoginButton.Content = "Loading...";
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/users/nickname/" + nicknameField.Text);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                User account = JsonConvert.DeserializeObject<User>(responseBody);

                Console.WriteLine(account.Password);
                // james@example.com
                if (BCrypt.Net.BCrypt.Verify(passwordField.Password, account.Password))
                {
                    MainWindow newWindow = new MainWindow();
                    newWindow.Show();
                    this.Close();
                }
                else
                    status.Content = "Wrong";


            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                MessageBoxResult result = MessageBox.Show("No Internet Connection");
                LoginButton.IsEnabled = true;
                LoginButton.Content = "Login";
            }
        }
    }
}
