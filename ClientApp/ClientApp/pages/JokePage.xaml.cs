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
    /// Interaction logic for JokePage.xaml
    /// </summary>
    public partial class JokePage : Page
    {
        readonly Joke joke;
        User author;
        List<Comment> comments;
        readonly Page previousPage;
        public User CurrentLoggedInUser { get; set; }
        public JokePage(Joke joke, Page previousPage)
        {
            this.joke = joke;
            this.previousPage = previousPage; 
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
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/jokes/" + joke.Id + "/comments");
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
    }
}
