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
using System.Windows.Shapes;

namespace ClientApp.addWindows
{
    /// <summary>
    /// Interaction logic for AddJokeWindow.xaml
    /// </summary>
    public partial class AddJokeWindow : Window
    {
        public User CurrentLoggedInUser { get; set; }
        public AddJokeWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Joke joke = new Joke
            {
                Title = TitleTextBox.Text,
                Content = ContentTextBox.Text,
                CreatedDate = DateTime.Now,
                AuthorFK = this.CurrentLoggedInUser.Id
            };

            if (TitleTextBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("Pusty tytuł");
                return;
            }

            if (TitleTextBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("Pusta zawartośc");
                return;
            }

            var json = JsonConvert.SerializeObject(joke, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            MessageBox.Show(json);
            try
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(json);
                
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44377/api/jokes", content);
                response.EnsureSuccessStatusCode();
                this.Close();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
