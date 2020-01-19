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
        }

        private void SaveChangeButton_Click(object sender, RoutedEventArgs e)
        {

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


    }
}
