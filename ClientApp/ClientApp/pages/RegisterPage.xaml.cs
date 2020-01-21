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

        readonly Page previousPage;
        public RegisterPage(Page previous)
        {
            this.previousPage = previous;
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

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
            if (EmailTextBox.Text.Trim().Equals("")) { MessageBox.Show("Nie podano adresu e-mail!"); return; }
            if (NicknameTextBox.Text.Trim().Equals("")) { MessageBox.Show("Nie podano nickname'u!"); return; }
            if (!IsValidEMail(EmailTextBox.Text)) { MessageBox.Show("Nie poprawny adres e-mail!"); return; }
            if (users.Where(u => u.Nickname.Equals(NicknameTextBox.Text)).Any()) { MessageBox.Show("Użytkownik o podanym loginie już istnieje!"); return; }
            if (users.Where(u => u.Email.Equals(EmailTextBox.Text)).Any()) { MessageBox.Show("Użytkownik o podanym adresie e-mail już istnieje!"); return; }
            if (NameTextBox.Text.Trim().Equals("")) { MessageBox.Show("Nie podano imienia!"); return; }
            if (SurnameTextBox.Text.Trim().Equals("")) { MessageBox.Show("Nie podano nazwiska!"); return; }
            if (PasswordField.Password.Trim().Equals("")) { MessageBox.Show("Nie podano hasła!"); return; }
            if (RepeatPasswordField.Password.Trim().Equals("")) { MessageBox.Show("Nie podano powtórzonego hasła!"); return; }
            if (!PasswordField.Password.Equals(RepeatPasswordField.Password)) { MessageBox.Show("Podane hasła różnią się!"); return; }
            

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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(previousPage);
        }

        private bool IsValidEMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
