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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for JokesPage.xaml
    /// </summary>
    public partial class JokesPage : Page
    {
        public JokesPage()
        {
            InitializeComponent();
            Reload();
        }

        async void Reload()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:44377/api/jokes");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
                foreach (Joke j in jokes)
                {
                    if (count % 3 == 0)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j.Title, j.CreatedDate.ToString(), j.Content, true));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j.Title, j.CreatedDate.ToString(), j.Content, false));
                    count++;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for(int i=0; i < ScrollContentWrapPanel.Children.Count; i++)
                {
                    if (i >= 4) 
                        ScrollContentWrapPanel.Children.RemoveAt(i);
                }
                HttpClient client = new HttpClient();
                HttpResponseMessage response =  await client.GetAsync("https://localhost:44377/api/jokes/search/" + SearchTextBox.Text);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
                foreach (Joke j in jokes)
                {
                    if (count % 3 == 0)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j.Title, j.CreatedDate.ToString(), j.Content, true));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j.Title, j.CreatedDate.ToString(), j.Content, false));
                    count++;
                }
               
            }
            catch (HttpRequestException ex)
            {
                for (int i = 0; i < ScrollContentWrapPanel.Children.Count; i++)
                {
                    if (i >= 4)
                        ScrollContentWrapPanel.Children.RemoveAt(i);
                }
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
