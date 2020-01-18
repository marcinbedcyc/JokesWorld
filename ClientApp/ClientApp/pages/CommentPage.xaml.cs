using ClientApp.models;
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

namespace ClientApp.pages
{
    /// <summary>
    /// Interaction logic for CommentPage.xaml
    /// </summary>
    ///
    public partial class CommentPage : Page
    {
        readonly Page previousPage;
        readonly Comment comment;
        User author;
        Joke joke;
        public User CurrentLoggedInUser { get; set; }
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

        async private void FindJoke()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/jokes/" + comment.JokeFK);
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

        async private void FindUser()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/users/" + comment.AuthorFK);
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

        public CommentPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(previousPage);
        }
    }
}
