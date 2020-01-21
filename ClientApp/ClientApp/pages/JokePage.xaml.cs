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
    /// Interaction logic for JokePage.xaml
    /// </summary>
    public partial class JokePage : Page
    {
        readonly Joke joke;
        User author;
        List<Comment> comments;
        readonly Page previousPage;
        public User CurrentLoggedInUser { get; set; }
        public JokePage(Joke joke, Page previousPage, User user)
        {
            this.joke = joke;
            this.previousPage = previousPage;
            this.CurrentLoggedInUser = user;
            InitializeComponent();
            TitleLabel.Text = joke.Title.ToUpper();
            DateLabel.Text = joke.CreatedDate.ToString();
            ContentLabel.Text = joke.Content;
            FindUser();
            FindComments();

        }
        public JokePage()
        {
            InitializeComponent();
        }

        async private  void FindUser()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + joke.AuthorFK);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                this.author = JsonConvert.DeserializeObject<User>(responseBody);
                AuthorLabel.Text = this.author.Nickname;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async private void FindComments()
        {
            try
            {
                ScrollContentWrapPanel.Children.RemoveRange(17, ScrollContentWrapPanel.Children.Count - 17);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes/" + joke.Id + "/comments");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                comments = JsonConvert.DeserializeObject<List<Comment>>(responseBody);
                if(this.author != null)
                    AuthorLabel.Text = this.author.Nickname;
                foreach (Comment c in comments)
                {
                    if (count % 3 == 0)
                    {
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, true, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
                        ScrollContentWrapPanel.Children.Add(Utils.LongEmptyLabel());
                    }
                    else
                    {
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, false, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
                        ScrollContentWrapPanel.Children.Add(Utils.LongEmptyLabel());
                    }
                    count++;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e, Comment c)
        {
            CommentPage commentPage = new CommentPage(c, this);
            NavigationService.Navigate(commentPage);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(previousPage);
        }

        private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddCommentTextBox.Text.Trim().Equals("")) { MessageBox.Show("Komentarz nie może być pusty!"); return; }
            try
            {
                HttpClient client = new HttpClient();

                Comment comment = new Comment()
                {
                    CreatedDate = DateTime.Now,
                    AuthorFK = this.CurrentLoggedInUser.Id,
                    JokeFK = this.joke.Id,
                    Content = AddCommentTextBox.Text
                };
                var json = JsonConvert.SerializeObject(comment, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                HttpContent content = new StringContent(json);

                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PostAsync(ConfigurationManager.AppSettings["ServerURL"] + "comments", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            AddCommentTextBox.Text = "";
            FindComments();
        }
    }
}
