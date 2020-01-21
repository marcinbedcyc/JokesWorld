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
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public User CurrentLoggedInUser { get; set; }
        public StartPage(User user)
        {
            this.CurrentLoggedInUser = user;
            InitializeComponent();
            TitleLabel.Text = "WITAJ " + this.CurrentLoggedInUser.Nickname.ToUpper() + "!";
            RefreshButton_Click(null, null);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GetLastJokes();
            GetLastComments();
            GetMyLastComment();
            GetMyLastJoke();
        }

        private async void GetLastJokes()
        {
            try
            {
                JokesStackPanel.Children.Clear();
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes/last_ones");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
                foreach (Joke j in jokes)
                {
                    if (count % 4 == 3 || count % 4 == 0)
                        JokesStackPanel.Children.Add(Utils.CreateJokeContentGrid(j, true, new RoutedEventHandler((s, e2) => JokeButton_Click(s, e2, j))));
                    else
                        JokesStackPanel.Children.Add(Utils.CreateJokeContentGrid(j, false, new RoutedEventHandler((s, e2) => JokeButton_Click(s, e2, j))));
                    count++;
                }

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void GetLastComments()
        {
            try
            {
                CommentsStackPanel.Children.Clear();
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "comments/last_ones");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var comments = JsonConvert.DeserializeObject<List<Comment>>(responseBody);
                foreach (Comment c in comments)
                {
                    if (count % 4 == 3 || count % 4 == 0)
                        CommentsStackPanel.Children.Add(Utils.CreateCommentContentGrid(c, true, new RoutedEventHandler((s, e2) => CommentButton_Click(s, e2, c))));
                    else
                        CommentsStackPanel.Children.Add(Utils.CreateCommentContentGrid(c, false, new RoutedEventHandler((s, e2) => CommentButton_Click(s, e2, c))));
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
            JokePage jokePage = new JokePage(j, this, this.CurrentLoggedInUser);
            NavigationService.Navigate(jokePage);
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e, Comment c)
        {
            CommentPage commentPage = new CommentPage(c, this);
            NavigationService.Navigate(commentPage);
        }

        private async void GetMyLastComment()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + this.CurrentLoggedInUser.Id + "/last_comment");
                string responseBody = await response.Content.ReadAsStringAsync();
                var comment = JsonConvert.DeserializeObject<Comment>(responseBody);
                if (comment.Content is null)
                {
                    comment.Content = "Brak";
                    comment.CreatedDate = DateTime.Now;
                    MyCommentStackPanel.Children.Add(Utils.LackTextBox());
                }
                else
                    MyCommentStackPanel.Children.Add(Utils.CreateCommentContentGrid(comment, true, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, comment))));
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void GetMyLastJoke()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + this.CurrentLoggedInUser.Id + "/last_joke");
                string responseBody = await response.Content.ReadAsStringAsync();
                var joke = JsonConvert.DeserializeObject<Joke>(responseBody);
                if (joke.Content is null)
                {
                    joke.Content = "Brak";
                    joke.Title = "Brak";
                    joke.CreatedDate = DateTime.Now;
                    MyJokeStackPanel.Children.Add(Utils.LackTextBox());
                }
                else
                    MyJokeStackPanel.Children.Add(Utils.CreateJokeContentGrid(joke, true, new RoutedEventHandler((s, e) => JokeButton_Click(s, e, joke))));
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
