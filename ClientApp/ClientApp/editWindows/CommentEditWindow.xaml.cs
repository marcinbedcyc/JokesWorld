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

namespace ClientApp.editWindows
{
    /// <summary>
    /// Interaction logic for CommentEditWindow.xaml
    /// </summary>
    public partial class CommentEditWindow : Window
    {
        public User CurrentLoggedInUser { get; set; }
        public Comment CurrentComment { get; set; }
        public CommentEditWindow(User user, Comment comment)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            CurrentLoggedInUser = user;
            CurrentComment = comment;
            InitializeComponent();
            ContentTextBox.Text = CurrentComment.Content;
            CreatedDatePicker.SelectedDate = CurrentComment.CreatedDate;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreatedDatePicker.SelectedDate > DateTime.Now) { MessageBox.Show("Wybierz wcześniejszą datę!"); return; }
            if (ContentTextBox.Text.Trim().Equals("")) { MessageBox.Show("Brak treści docipu!"); return; }

            CurrentComment.Content = ContentTextBox.Text;
            CurrentComment.CreatedDate = (DateTime)CreatedDatePicker.SelectedDate;

            var json = JsonConvert.SerializeObject(CurrentComment);
            try
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(json);

                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44377/api/comments/" + CurrentComment.Id, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Dane zostały zmienione!");
            this.Close();
        }
    }
}
