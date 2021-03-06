﻿using ClientApp.models;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        /// <summary>
        /// Current logged user in application.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }
        public UsersPage()
        {
            InitializeComponent();
            Reload();
        }

        /// <summary>
        /// Navigate to user's detail page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        /// <param name="user">User. Detail page will be opened about it.</param>
        private void UserButton_Click(object sender, RoutedEventArgs e, User user)
        {
            UserPage userPage = new UserPage(user, this, this.CurrentLoggedInUser);
            NavigationService.Navigate(userPage);
        }

        /// <summary>
        /// Refresh information on page.
        /// </summary>
        /// <seealso cref="UsersPage.Reload"/>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Reload();
        }

        /// <summary>
        /// Get text from SearchTextBox and send searching http reuqest. Result will fill controls.
        /// </summary>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ScrollContentWrapPanel.Children.RemoveRange(5, ScrollContentWrapPanel.Children.Count - 5);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/search/" + SearchTextBox.Text);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                foreach (User u in users)
                {
                    response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + u.Id + "/last_joke");
                    responseBody = await response.Content.ReadAsStringAsync();
                    var joke = JsonConvert.DeserializeObject<Joke>(responseBody);
                    string jokeContent;
                    if (joke.Content is null) jokeContent = "Brak";
                    else jokeContent = joke.Content;

                    response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + u.Id + "/last_comment");
                    responseBody = await response.Content.ReadAsStringAsync();
                    var comment = JsonConvert.DeserializeObject<Comment>(responseBody);
                    string commentContent;
                    if (comment.Content is null) commentContent = "Brak";
                    else commentContent = comment.Content;

                    if (count % 4 == 0 || count % 4 == 3)
                        ScrollContentWrapPanel.Children.Add(Utils.CreateUserContentGrid(u, commentContent, jokeContent, true, new RoutedEventHandler((s, arg) => UserButton_Click(s, arg, u))));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateUserContentGrid(u, commentContent, jokeContent, false, new RoutedEventHandler((s, arg) => UserButton_Click(s, arg, u))));
                    count++;
                }

            }
            catch (HttpRequestException ex)
            {
                ScrollContentWrapPanel.Children.RemoveRange(5, ScrollContentWrapPanel.Children.Count - 5);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Refresh information on page. Send http request to get all available users.
        /// </summary>
        async void Reload()
        {
            ScrollContentWrapPanel.Children.RemoveRange(5, ScrollContentWrapPanel.Children.Count - 5);
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                int count = 0;
                var users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                foreach (User u in users)
                {
                    response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + u.Id + "/last_joke");
                    responseBody = await response.Content.ReadAsStringAsync();
                    var joke = JsonConvert.DeserializeObject<Joke>(responseBody);
                    string jokeContent;
                    if (joke.Content is null) jokeContent = "Brak";
                    else jokeContent = joke.Content;

                    response = await client.GetAsync(ConfigurationManager.AppSettings["ServerURL"] + "users/" + u.Id + "/last_comment");
                    responseBody = await response.Content.ReadAsStringAsync();
                    var comment = JsonConvert.DeserializeObject<Comment>(responseBody);
                    string commentContent;
                    if (comment.Content is null) commentContent = "Brak";
                    else commentContent = comment.Content;

                    if (count % 4 == 0 || count % 4 == 3) 
                        ScrollContentWrapPanel.Children.Add(Utils.CreateUserContentGrid(u, commentContent, jokeContent, true, new RoutedEventHandler((s, e) => UserButton_Click(s, e, u))));
                    else
                        ScrollContentWrapPanel.Children.Add(Utils.CreateUserContentGrid(u, commentContent, jokeContent, false, new RoutedEventHandler((s, e) => UserButton_Click(s, e, u))));
                    count++;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Search action when enter is pressed when focus on SearchTextBox.
        /// </summary>
        /// <seealso cref="UsersPage.SearchButton_Click"/>
        /// <param name="sender">The control/object that raised the event.</param>
        /// <param name="e">Event Data.</param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(this, new RoutedEventArgs());
            }
        }
    }
}
