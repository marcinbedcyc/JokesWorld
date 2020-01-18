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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User CurrentLoggedInUser { get; set; }
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TitleBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            UsersPage usersPage = new UsersPage()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser
            };
            ContentFrame.Navigate(usersPage);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            StartPage startPage = new StartPage()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser
            };
            ContentFrame.Navigate(startPage);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser
            };
            ContentFrame.Navigate(settingsPage);
        }

        private void JokesButton_Click(object sender, RoutedEventArgs e)
        {
            JokesPage jokesPage = new JokesPage()
            {
                CurrentLoggedInUser = this.CurrentLoggedInUser
            };
            ContentFrame.Navigate(jokesPage);
        }

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
