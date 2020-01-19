using ClientApp.models;
using ClientApp.pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public User CurrentLoggedInUser { get; set; }
        public SettingsPage(User user)
        {
            this.CurrentLoggedInUser = user;
            InitializeComponent();
            NameTextBox.Text = this.CurrentLoggedInUser.Name;
            SurnameTextBox.Text = this.CurrentLoggedInUser.Surname;
            EMailTextBox.Text = this.CurrentLoggedInUser.Email;
            NicknameTextBox.Text = this.CurrentLoggedInUser.Nickname;
            OldPasswordBox.Password = "";
            NewPasswordBox.Password = "";
            ReapeatPasswordBox.Password = "";
            GetComments();
            GetJokes();
            OldPasswordBox.PasswordChanged += PasswordChangedHandler;
            NewPasswordBox.IsEnabled = false;
            ReapeatPasswordBox.IsEnabled = false;
        }

        private async void SaveChangeButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            List<User> users = new List<User>();
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/users/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (NameTextBox.Text.Trim().Equals("")) { MessageBox.Show("Puste pole z imieniem!"); return; }
            if (SurnameTextBox.Text.Trim().Equals("")) { MessageBox.Show("Puste pole z nazwiskiem!"); return; }
            if (NicknameTextBox.Text.Trim().Equals("")) { MessageBox.Show("Puste pole z nickname!"); return; }
            if (EMailTextBox.Text.Trim().Equals("")) { MessageBox.Show("Puste pole z e-mail'em!"); return; }
            if( users.Where(u => u.Nickname.Equals(NicknameTextBox.Text)).Any() && !NicknameTextBox.Text.Equals(CurrentLoggedInUser.Nickname) ) { MessageBox.Show("Nickname zajęty"); return; }
            if (users.Where(u => u.Email.Equals(EMailTextBox.Text)).Any() && !EMailTextBox.Text.Equals(CurrentLoggedInUser.Email)) { MessageBox.Show("Email zajęty"); return; }
            if (!OldPasswordBox.Password.Trim().Equals(""))
            {
                if (!BCrypt.Net.BCrypt.Verify(OldPasswordBox.Password, CurrentLoggedInUser.Password)) { MessageBox.Show("Podane hasło jest nieprawidłowe!"); return; }
                else
                {
                    if (!NewPasswordBox.Password.Equals(ReapeatPasswordBox.Password)) { MessageBox.Show("Podane hasła różnią się!"); return; }
                    else if (NewPasswordBox.Password.Trim().Equals("") || ReapeatPasswordBox.Password.Trim().Equals("")) { MessageBox.Show("Podane nowe  hasło jest puste!"); return; }
                    else
                    {
                        CurrentLoggedInUser.Password = BCrypt.Net.BCrypt.HashPassword(NewPasswordBox.Password);
                    }
                }
            }

            CurrentLoggedInUser.Name = NameTextBox.Text;
            CurrentLoggedInUser.Surname = SurnameTextBox.Text;
            CurrentLoggedInUser.Nickname = NicknameTextBox.Text;
            CurrentLoggedInUser.Email = EMailTextBox.Text;

            var json = JsonConvert.SerializeObject(CurrentLoggedInUser);
            try
            {
                HttpContent content = new StringContent(json);

                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44377/api/users/" + CurrentLoggedInUser.Id, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Dane zostały zmienione" + NewPasswordBox.Password);
        }

        async private void GetComments()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/users/" + CurrentLoggedInUser.Id + "/comments");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var comments = JsonConvert.DeserializeObject<List<Comment>>(responseBody);
                foreach (Comment c in comments)
                {
                    if (count % 2 == 0)
                        CommentsStackPanel.Children.Add(Utils.CreateCommentContentGrid(c, true, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
                    else
                        CommentsStackPanel.Children.Add(Utils.CreateCommentContentGrid(c, false, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
                    count++;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async private void GetJokes()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/users/" + CurrentLoggedInUser.Id + "/jokes");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
                foreach (Joke j in jokes)
                {
                    if (count % 2 == 0)
                        JokesStackPanel.Children.Add(Utils.CreateJokeContentGrid(j, true, new RoutedEventHandler((s, e) => JokeButton_Click(s, e, j))));
                    else
                        JokesStackPanel.Children.Add(Utils.CreateJokeContentGrid(j, false, new RoutedEventHandler((s, e) => JokeButton_Click(s, e, j))));
                    count++;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void JokeButton_Click(object sender, RoutedEventArgs e, Joke j)
        {
            MessageBox.Show("Not implemented");
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e, Comment c)
        {
            MessageBox.Show("Not implemented");
        }

        void PasswordChangedHandler(Object sender, RoutedEventArgs args)
        {
            if (BCrypt.Net.BCrypt.Verify(OldPasswordBox.Password, CurrentLoggedInUser.Password)){
                NewPasswordBox.IsEnabled = true;
                ReapeatPasswordBox.IsEnabled = true;
                OldPasswordBox.Background = new SolidColorBrush(Color.FromArgb(40, 0, 255, 0));
            }
            else { 
                OldPasswordBox.Background = new SolidColorBrush(Color.FromArgb(40, 255, 0, 0));
                NewPasswordBox.IsEnabled = true;
                ReapeatPasswordBox.IsEnabled = true;
            }
        }


    }
}
