using ClientApp.models;
using ClientApp.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Current logged user in application.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }
        public MainWindow(User user)
        {
            this.CurrentLoggedInUser = user;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            StartPage startPage = new StartPage(this.CurrentLoggedInUser);
            ContentFrame.Navigate(startPage);
        }

        /// <summary>
        /// Logging our from application.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Open all users page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            UsersPage usersPage = new UsersPage()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser
            };
            ContentFrame.Navigate(usersPage);
        }

        /// <summary>
        /// Open home page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            StartPage startPage = new StartPage(this.CurrentLoggedInUser);
            ContentFrame.Navigate(startPage);
        }

        /// <summary>
        /// Open setting page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage(this.CurrentLoggedInUser);
            ContentFrame.Navigate(settingsPage);
        }

        /// <summary>
        /// Open all jokes page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void JokesButton_Click(object sender, RoutedEventArgs e)
        {
            JokesPage jokesPage = new JokesPage()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser
            };
            ContentFrame.Navigate(jokesPage);
        }

        /// <summary>
        /// Open all comments page.
        /// </summary>
        /// <param name="sender">The control/object that raised the event</param>
        /// <param name="e">Event Data</param>
        private void CommentsButton_Click(object sender, RoutedEventArgs e)
        {
            CommentsPage commentsPage = new CommentsPage()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser
            };
            ContentFrame.Navigate(commentsPage);
        }
    }
}
