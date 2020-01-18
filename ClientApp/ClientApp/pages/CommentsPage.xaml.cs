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
    /// Interaction logic for CommentsPage.xaml
    /// </summary>
    public partial class CommentsPage : Page
    {

        public User CurrentLoggedInUser { get; set; }
        public CommentsPage()
        {
            InitializeComponent();
            Reload();
        }

        async void Reload()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/comments");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var comments = JsonConvert.DeserializeObject<List<Comment>>(responseBody);
                foreach (Comment c in comments)
                {
                    if (count % 3 == 0)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, true, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, true, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
                    count++;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CommentButton_Click(object sender, RoutedEventArgs args, Comment c)
        {
            CommentPage commentPage = new CommentPage(c, this);
            NavigationService.Navigate(commentPage);
        }
    }
}
