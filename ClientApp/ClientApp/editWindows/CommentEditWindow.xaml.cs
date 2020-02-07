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
    /// Interaction logic for CommentEditWindow.xaml
    /// </summary>
    public partial class CommentEditWindow : Window
    {
        /// <summary>
        /// Current logged user in application.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }

        /// <summary>
        /// Comment which is editing.
        /// </summary>
        public Comment CurrentComment { get; set; }

        public CommentEditWindow() { }
        public CommentEditWindow(User user, Comment comment)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            CurrentLoggedInUser = user;
            CurrentComment = comment;
            InitializeComponent();
            ContentTextBox.Text = CurrentComment.Content;
            CreatedDatePicker.SelectedDate = CurrentComment.CreatedDate;
        }

        /// <summary>
        /// Cancel editing comment.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Save changes in comment when everythins is correct typed in.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsPreviousDate(CreatedDatePicker.SelectedDate)) { MessageBox.Show("Wybierz wcześniejszą datę!"); return; }
            if (IsContentEmpty(ContentTextBox.Text.Trim())) { MessageBox.Show("Brak treści docipu!"); return; }

            CurrentComment.Content = ContentTextBox.Text;
            CurrentComment.CreatedDate = (DateTime)CreatedDatePicker.SelectedDate;

            var json = JsonConvert.SerializeObject(CurrentComment);
            try
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(json);

                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
                HttpResponseMessage response = await client.PutAsync(ConfigurationManager.AppSettings["ServerURL"] + "comments/" + CurrentComment.Id, content);
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
        /// Deleting current comment.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(ConfigurationManager.AppSettings["ServerURL"] + "comments/" + CurrentComment.Id);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Komentarz został usunięty!");
            this.Close();
        }

        /// <summary>
        /// Check if passing date is before now.
        /// </summary>
        /// <param name="date">Comment's date.</param>
        /// <returns>True if date was earlier than today.</returns>
        public bool IsPreviousDate(DateTime? date)
        {
            if (date is null || date > DateTime.Now)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Check content is empty.
        /// </summary>
        /// <param name="content">String value of comment's content.</param>
        /// <returns>True if conent is empty.</returns>
        public bool IsContentEmpty(string content)
        {
            if (content.Equals(""))
                return true;
            else
                return false;
        }
    }
}
