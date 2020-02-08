using ClientApp.editWindows;
using ClientApp.models;
using ClientApp.pages;
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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        /// <summary>
        /// Current logged user in application.
        /// </summary>
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

        /// <summary>
        /// Send http request to server to save user's data when everything is filled in corectly.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private async void SaveChangeButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            List<User> users = new List<User>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/");
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
                UserFormUtils.CheckForm(NameTextBox.Text.Trim(), SurnameTextBox.Text.Trim(), EMailTextBox.Text.Trim(), NicknameTextBox.Text.Trim(), "password", "password", users);
            }
            catch (EmptyFormException) { MessageBox.Show("Pozostawiono puste pole!"); return; }
            catch (NicknameAlredyUsedException) { MessageBox.Show("Użytkownik o podanym loginie już istnieje!"); return; }
            catch (NotCorrectEmailException) { MessageBox.Show("Nie poprawny adres e-mail!"); return; }
            catch (EmailAlredyUsedException) { MessageBox.Show("Użytkownik o podanym adresie e-mail już istnieje!"); return; }
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
                HttpResponseMessage response = await client.PutAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + CurrentLoggedInUser.Id, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Dane zostały zmienione" + NewPasswordBox.Password);
        }

        /// <summary>
        /// Send http request to server to get all current logged in user's comment.
        /// </summary>
        async private void GetComments()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + CurrentLoggedInUser.Id + "/comments");
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

        /// <summary>
        /// Send http request to server to get all current logged in user's joke.
        /// </summary>
        async private void GetJokes()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + CurrentLoggedInUser.Id + "/jokes");
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

        /// <summary>
        /// Open new editing window to change joke's data.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        /// <param name="joke">Joke. Editing window will contain info about it.</param>
        private void JokeButton_Click(object sender, RoutedEventArgs e, Joke joke)
        {

            JokeEditWindow jokeEditWindow = new JokeEditWindow(CurrentLoggedInUser, joke);
            jokeEditWindow.Closing += (s, args) => { Reload(); };
            jokeEditWindow.Show();
        }

        /// <summary>
        /// Open new editing window to change comment's data.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        /// <param name="comment">Comment. Editing window will contain info about it.</param>
        private void CommentButton_Click(object sender, RoutedEventArgs e, Comment comment)
        {
            CommentEditWindow commentEditWindow = new CommentEditWindow(CurrentLoggedInUser, comment);
            commentEditWindow.Closing += (s, args) => { Reload(); };
            commentEditWindow.Show();
        }

        /// <summary>
        /// Set enable/disable PasswordBoxes depends on type in correctly old one.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="args">Event Data.</param>
        private void PasswordChangedHandler(Object sender, RoutedEventArgs args)
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

        /// <summary>
        /// Reset information about user's jokes and comments.
        /// </summary>
        private void Reload()
        {
            CommentsStackPanel.Children.Clear();
            JokesStackPanel.Children.Clear();
            GetComments();
            GetJokes();
        }

        /// <summary>
        /// Reset TextBoxes and get refresh information on page.
        /// </summary>
        /// <seealso cref="SettingsPage.Reload"/>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = this.CurrentLoggedInUser.Name;
            SurnameTextBox.Text = this.CurrentLoggedInUser.Surname;
            EMailTextBox.Text = this.CurrentLoggedInUser.Email;
            NicknameTextBox.Text = this.CurrentLoggedInUser.Nickname;
            OldPasswordBox.Password = "";
            NewPasswordBox.Password = "";
            ReapeatPasswordBox.Password = "";
            Reload();
        }
    }
}
