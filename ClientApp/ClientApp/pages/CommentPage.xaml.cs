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
    /// Interaction logic for CommentPage.xaml
    /// </summary>
    ///
    public partial class CommentPage : Page
    {
        /// <summary>
        /// Keep information abouit previous page.
        /// </summary>
        readonly Page previousPage;
        /// <summary>
        /// Current comment.
        /// </summary>
        readonly Comment comment;
        /// <summary>
        /// Comment's user.
        /// </summary>
        User author;
        /// <summary>
        /// Comment's joke.
        /// </summary>
        Joke joke;
        /// <summary>
        /// Current logged user in application.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }
        public CommentPage()
        {
            InitializeComponent();
        }
        public CommentPage(Comment comment, Page previousPage)
        {
            this.comment = comment;
            this.previousPage = previousPage;
            InitializeComponent();
            DateLabel.Text = comment.CreatedDate.ToString();
            ContentLabel.Text = comment.Content;
            FindUser();
            FindJoke();
        }

        /// <summary>
        /// Send finding joke http request and comlpetes the controls.
        /// </summary>
        async private void FindJoke()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes/" + comment.JokeFK);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                this.joke = JsonConvert.DeserializeObject<Joke>(responseBody);
                JokeLabel.Text = this.joke.Title;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Send finding user http reuqest and completes the control.
        /// </summary>
        async private void FindUser()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + comment.AuthorFK);
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

        /// <summary>
        /// Navigate do previous page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(previousPage);
        }
    }
}
