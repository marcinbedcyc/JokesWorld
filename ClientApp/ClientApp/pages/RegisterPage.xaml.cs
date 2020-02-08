using ClientApp.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {

        /// <summary>
        /// Keep information abouit previous page.
        /// </summary>
        readonly Page previousPage;
        public RegisterPage(Page previous)
        {
            this.previousPage = previous;
            InitializeComponent();
        }

        /// <summary>
        /// Exit application.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Send http request to add new user to db. Before sending checking is form is filled properly (every text input is filled in, check if nickname and
        /// e-mail are unique and check if password and repeated password are the same).
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            List<User> users = new List<User>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                UserFormUtils.CheckForm(NameTextBox.Text.Trim(), SurnameTextBox.Text.Trim(), EmailTextBox.Text.Trim(), NicknameTextBox.Text.Trim(), PasswordField.Password.Trim(), RepeatPasswordField.Password.Trim(), users);
            }
            catch (EmptyFormException) { MessageBox.Show("Pozostawiono puste pole!"); return; }
            catch (NicknameAlredyUsedException) { MessageBox.Show("Użytkownik o podanym loginie już istnieje!"); return; }
            catch (NotCorrectEmailException) { MessageBox.Show("Nie poprawny adres e-mail!"); return; }
            catch (EmailAlredyUsedException) { MessageBox.Show("Użytkownik o podanym adresie e-mail już istnieje!"); return; }
            catch (DiffrentPasswordsException) { MessageBox.Show("Podane hasła różnią się!"); return; }
            

            User user = new User
            {
                Name = NameTextBox.Text,
                Surname = SurnameTextBox.Text,
                Nickname = NicknameTextBox.Text,
                Password = BCrypt.Net.BCrypt.HashPassword(PasswordField.Password),
                Email = EmailTextBox.Text
            };

            var json = JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            MessageBox.Show(json);
            try
            {
                HttpContent content = new StringContent(json);
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PostAsync(ConfigurationManager.AppSettings["ServerURL"] + "users", content);
                response.EnsureSuccessStatusCode();
                NavigationService.Navigate(previousPage);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Navigate to login page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(previousPage);
        }
    }
}
