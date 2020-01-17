using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
            LoginWindow newWindow1 = new LoginWindow();
            newWindow1.Show();
            this.Close();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("pages\\UsersPage.xaml", UriKind.Relative);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("pages\\StartPage.xaml", UriKind.Relative);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("pages\\SettingsPage.xaml", UriKind.Relative);
        }

        private void JokesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("pages\\JokesPage.xaml", UriKind.Relative);
        }
    }
}
