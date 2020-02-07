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

namespace ClientApp.editWindows
{
    /// <summary>
    /// Interaction logic for JokeEditWindow.xaml
    /// </summary>
    public partial class JokeEditWindow : Window
    {
        /// <summary>
        /// Current logged user in application.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }

        /// <summary>
        /// Current joke.
        /// </summary>
        public Joke CurrentJoke { get; set; }
        public JokeEditWindow(User user, Joke joke)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            CurrentLoggedInUser = user;
            CurrentJoke = joke;
            InitializeComponent();
            TitleTextBox.Text = CurrentJoke.Title;
            ContentTextBox.Text = CurrentJoke.Content;
            CreatedDatePicker.SelectedDate = CurrentJoke.CreatedDate;
        }

        public JokeEditWindow() { }

        /// <summary>
        /// Save changes in joke when everythins is correct typed in.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {

            HttpClient client = new HttpClient();
            List<Joke> jokes = new List<Joke>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (jokes.Where(u => u.Content.Equals(ContentTextBox.Text)).Any() && !ContentTextBox.Text.Equals(CurrentJoke.Content)) { MessageBox.Show("Dowcip o podanej treści już istnieje!"); return; }
            if (CreatedDatePicker.SelectedDate > DateTime.Now){ MessageBox.Show("Wybierz wcześniejszą datę!"); return; }
            if(ContentTextBox.Text.Trim().Equals("")) { MessageBox.Show("Brak treści docipu!"); return; }
            if (TitleTextBox.Text.Trim().Equals("")) { MessageBox.Show("Brak tytułu docipu!"); return; }

            CurrentJoke.Title = TitleTextBox.Text;
            CurrentJoke.Content = ContentTextBox.Text;
            CurrentJoke.CreatedDate = (DateTime)CreatedDatePicker.SelectedDate;

            var json = JsonConvert.SerializeObject(CurrentJoke);
            try
            {
                HttpContent content = new StringContent(json);

                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PutAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes/" + CurrentJoke.Id, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Dane zostały zmienione!");
            this.Close();
        }

        /// <summary>
        /// Cancel editing current joke.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Send deleting current joke http request to server.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(ConfigurationManager.AppSettings["ServerURL"] + "jokes/" + CurrentJoke.Id);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Dowcip został usunięty!");
            this.Close();
        }
    }
}
