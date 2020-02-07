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
using System.Windows.Shapes;

namespace ClientApp.addWindows
{
    /// <summary>
    /// Interaction logic for AddJokeWindow.xaml
    /// </summary>
    public partial class AddJokeWindow : Window
    {
        /// <summary>
        /// Current logged user in application.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }
        public AddJokeWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        /// <summary>
        /// Cancel adding new joke to server.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Get data from controls and if everything is correct send adding joke to db http request.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            List<Joke> jokes = new List<Joke>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (!CheckUniqueJokeContent(jokes, ContentTextBox.Text)) { MessageBox.Show("Dowcip o podanej treści już istnieje!"); return; }
            if (!CheckNewJokeTitle(TitleTextBox.Text.Trim())) { MessageBox.Show("Pusty tytuł"); return; }
            if (!CheckNewJokeContent(ContentTextBox.Text.Trim())) { MessageBox.Show("Pusta zawartośc"); return; }

            Joke joke = new Joke
            {
                Title = TitleTextBox.Text,
                Content = ContentTextBox.Text,
                CreatedDate = DateTime.Now,
                AuthorFK = this.CurrentLoggedInUser.Id
            };

            var json = JsonConvert.SerializeObject(joke, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            MessageBox.Show(json);
            try
            {
                HttpContent content = new StringContent(json);
                
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PostAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes", content);
                response.EnsureSuccessStatusCode();
                this.Close();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Check if joke's title is not empty.
        /// </summary>
        /// <param name="title">Joke's title.</param>
        /// <returns>True if title is not empty.</returns>
        public bool CheckNewJokeTitle(string title)
        {
            if (title.Equals(""))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Check if joke's content is not empty.
        /// </summary>
        /// <param name="title">Joke's content.</param>
        /// <returns>True if content is not empty.</returns>
        public bool CheckNewJokeContent(string content)
        {
            if (content.Equals("")) 
                return false;
            else 
                return true;
        }


        /// <summary>
        /// Check if joke's content is unique in jokes list.
        /// </summary>
        /// <param name="jokes">List of jokes.</param>
        /// <param name="content">Joke's content.</param>
        /// <returns>True if content is unique.</returns>
        public bool CheckUniqueJokeContent(List<Joke> jokes, string content)
        {
            if (jokes.Where(u => u.Content.Equals(content)).Any())
                return false;
            else
                return true;
        }
    }
}
