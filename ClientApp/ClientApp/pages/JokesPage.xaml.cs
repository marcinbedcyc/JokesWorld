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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for JokesPage.xaml
    /// </summary>
    public partial class JokesPage : Page
    {
        TextBlock label1;
        public JokesPage()
        {
            InitializeComponent();
            var myScrollViewer = new ScrollViewer();
            myScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            var myStackPanel = new WrapPanel();
            myStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            myStackPanel.VerticalAlignment = VerticalAlignment.Top;

            Rectangle myRectangle = new Rectangle();
            myRectangle.Fill = Brushes.Red;
            myRectangle.Width = 500;
            myRectangle.Height = 500;

            Rectangle myRectangle1 = new Rectangle();
            myRectangle1.Fill = Brushes.Green;
            myRectangle1.Width = 500;
            myRectangle1.Height = 500;

            Button button = new Button();
            var myStackPanel1 = new StackPanel();
            myStackPanel1.HorizontalAlignment = HorizontalAlignment.Left;
            myStackPanel1.VerticalAlignment = VerticalAlignment.Top;

            Button button1 = new Button();

            TextBlock label = new TextBlock();
            label.TextWrapping = TextWrapping.WrapWithOverflow;
            label.Text = "test";

            label1 = new TextBlock();
            label1.TextWrapping = TextWrapping.WrapWithOverflow;
            label1.Text = "Bardzo długi teskt tekstowy aby sprawdzi czy dobrze się wyświetla.Bardzo długi teskt tekstowy aby sprawdzi czy dobrze się wyświetla.Bardzo długi teskt tekstowy aby sprawdzi czy dobrze się wyświetla.Bardzo długi teskt tekstowy aby sprawdzi czy dobrze się wyświetla.Bardzo długi teskt tekstowy aby sprawdzi czy dobrze się wyświetla.";

            myStackPanel1.Children.Add(label);
            myStackPanel1.Children.Add(label1);

            button.Content = myStackPanel1;
            button.Width = 350;
            button.Margin = new Thickness(10, 10, 10, 10);

            button1.Content = myStackPanel1;
            button1.Width = 350;
            button1.Margin = new Thickness(10, 10, 10, 10);

            myStackPanel.Children.Add(myRectangle);
            myStackPanel.Children.Add(myRectangle1);
            myStackPanel.Children.Add(button);
            myStackPanel.Children.Add(button1);

            myScrollViewer.Content = myStackPanel;
            this.Content = myScrollViewer;
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

                var jokes = JsonConvert.DeserializeObject<List<Joke>>(responseBody);
                foreach (Joke j in jokes)
                    label1.Text += j;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }
    }
}
