using ClientApp.models;
using ClientApp.pages;
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
using ClientApp.addWindows;
using System.Configuration;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for JokesPage.xaml
    /// </summary>
    public partial class JokesPage : Page
    { 
        public User CurrentLoggedInUser { get; set; }

        public JokesPage()
        {
            InitializeComponent();
            Reload();
        }

        async void Reload()
        {
            ScrollContentWrapPanel.Children.RemoveRange(6, ScrollContentWrapPanel.Children.Count - 6);
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
                foreach (Joke j in jokes)
                {
                    if (count % 4 == 0 || count % 4 == 3)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j, true, new RoutedEventHandler((s, e) => JokeButton_Click(s, e, j))));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j, false, new RoutedEventHandler((s, e) => JokeButton_Click(s, e, j))));
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
                ScrollContentWrapPanel.Children.RemoveRange(6, ScrollContentWrapPanel.Children.Count - 6);
                HttpClient client = new HttpClient();
                HttpResponseMessage response =  await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes/search/" + SearchTextBox.Text);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
                foreach (Joke j in jokes)
                {
                    if (count % 4 == 3 || count % 4 == 0)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j, true, new RoutedEventHandler((s, e2) => JokeButton_Click(s, e2, j))));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateJokeContentGrid(j, false, new RoutedEventHandler((s, e2) => JokeButton_Click(s, e2, j))));
                    count++;
                }
               
            }
            catch (HttpRequestException ex)
            {
                ScrollContentWrapPanel.Children.RemoveRange(5, ScrollContentWrapPanel.Children.Count - 5);
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddJokeWindow newWindow1 = new AddJokeWindow()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser,
            };
            newWindow1.Show();
        }

        private void JokeButton_Click(object sender, RoutedEventArgs e, Joke j)
        {
            JokePage jokePage = new JokePage(j, this, this.CurrentLoggedInUser);
            NavigationService.Navigate(jokePage);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Reload();
        }
    }
}
