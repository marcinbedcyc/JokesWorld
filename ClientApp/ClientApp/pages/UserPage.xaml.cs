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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    ///
    
    public partial class UserPage : Page
    {
        /// <summary>
        /// Keep information abouit previous page.
        /// </summary>
        readonly Page previousPage;
        /// <summary>
        /// Current user.
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Current logged user in application.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }
        public UserPage(User user, Page previousPage, User loggedUser)
        {
            this.User = user;
            this.previousPage = previousPage;
            this.CurrentLoggedInUser = loggedUser;
            InitializeComponent();
            NameTextBlock.Text = this.User.Name;
            SurnameTextBlock.Text = this.User.Surname;
            NicknameTextBlock.Text = this.User.Nickname;
            EMailTextBlock.Text = this.User.Email;
            GetComments();
            GetJokes();
        }

        /// <summary>
        /// Navigate to previous page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(previousPage);
        }

        /// <summary>
        /// Send http request to get user's comments and put information in controls.
        /// </summary>
        async private void GetComments()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + this.User.Id + "/comments");
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
        /// Send http request to get user's jokes and put information in controls.
        /// </summary>
        async private void GetJokes()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + this.User.Id + "/jokes");
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
        /// Navigate to joke's detail page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        /// <param name="joke">Joke. Detail page will be opened about it.</param>
        private void JokeButton_Click(object sender, RoutedEventArgs e, Joke joke)
        {
            JokePage jokePage = new JokePage(joke, this, this.CurrentLoggedInUser);
            NavigationService.Navigate(jokePage);
        }

        /// <summary>
        /// Navigate to comment's detail page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        /// <param name="comment">Comment. Detail page will be opened about it.</param>
        private void CommentButton_Click(object sender, RoutedEventArgs e, Comment comment)
        {
            CommentPage commentPage = new CommentPage(comment, this);
            NavigationService.Navigate(commentPage);
        }
    }
}
