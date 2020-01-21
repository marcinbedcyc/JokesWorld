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
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "comments");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var comments = JsonConvert.DeserializeObject<List<Comment>>(responseBody);
                foreach (Comment c in comments)
                {
                    if (count % 4 == 3 || count % 4 == 0)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, true, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, false, new RoutedEventHandler((s, e) => CommentButton_Click(s, e, c))));
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

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ScrollContentWrapPanel.Children.RemoveRange(4, ScrollContentWrapPanel.Children.Count - 4);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/comments/search/" + SearchTextBox.Text);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var comments = JsonConvert.DeserializeObject<List<Comment>>(responseBody);
                foreach (Comment c in comments)
                {
                    if (count % 4 == 3 || count % 4 == 0)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, true, new RoutedEventHandler((s, e2) => CommentButton_Click(s, e2, c))));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateCommentContentGrid(c, false, new RoutedEventHandler((s, e2) => CommentButton_Click(s, e2, c))));
                    count++;
                }

            }
            catch (HttpRequestException ex)
            {
                ScrollContentWrapPanel.Children.RemoveRange(4, ScrollContentWrapPanel.Children.Count - 4);
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(this, new RoutedEventArgs());
            }
        }
    }
}
