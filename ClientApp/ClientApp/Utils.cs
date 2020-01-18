using ClientApp.models;
using ClientApp.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    public static class Utils
    {

        public static Label LongEmptyLabel()
        {
            Label label = new Label()
            {
                Margin = new Thickness(0, 0, 3000, 0)
            };
            return label;
        }
        public static Grid CreateJokeContentGrid(Joke joke, bool isPink, RoutedEventHandler OnClick)
        {
            var jokeGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10)
            };
            jokeGrid.RowDefinitions.Add(new RowDefinition());
            jokeGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var jokeButton = new Button
            {
                Width = 350,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250)),
                Margin = new Thickness(10),
                Cursor = Cursors.Hand
            };
            jokeButton.Click += OnClick;

            var jokeRectangle = new Rectangle
            {
                Width = 350,
                RadiusX = 10,
                RadiusY = 10
            };
            jokeRectangle.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 222, G = 222, B = 222 },
                ShadowDepth = 1,
                BlurRadius = 20
            };
            if (isPink)
                jokeRectangle.Fill = new SolidColorBrush(Color.FromArgb(80, 177, 101, 240));
            else
                jokeRectangle.Fill = new SolidColorBrush(Color.FromArgb(80, 112, 172, 250));


            jokeGrid.Children.Add(jokeRectangle);

            var jokeStackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock titleTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = joke.Title,
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 18,
                Width = 330,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock dateTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = joke.CreatedDate.ToString(),
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 14,
                Width = 330,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock contentTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = joke.Content,
                Width = 330,
                Margin = new Thickness(10, 5, 10, 5),
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            jokeStackPanel.Children.Add(titleTextBlock);
            jokeStackPanel.Children.Add(dateTextBlock);
            jokeStackPanel.Children.Add(contentTextBlock);

            jokeButton.Content = jokeStackPanel;
            jokeGrid.Children.Add(jokeButton);
            return jokeGrid;
        }

        public static Grid CreateCommentContentGrid(Comment comment, bool isPink, RoutedEventHandler OnClick)
        {
            var jokeGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10)
            };
            jokeGrid.RowDefinitions.Add(new RowDefinition());
            jokeGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var jokeButton = new Button
            {
                Width = 350,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250)),
                Margin = new Thickness(10),
                Cursor = Cursors.Hand
            };
            jokeButton.Click += OnClick;

            var jokeRectangle = new Rectangle
            {
                Width = 350,
                RadiusX = 10,
                RadiusY = 10
            };
            jokeRectangle.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 222, G = 222, B = 222 },
                ShadowDepth = 1,
                BlurRadius = 20
            };
            if (isPink)
                jokeRectangle.Fill = new SolidColorBrush(Color.FromArgb(80, 177, 101, 240));
            else
                jokeRectangle.Fill = new SolidColorBrush(Color.FromArgb(80, 112, 172, 250));


            jokeGrid.Children.Add(jokeRectangle);

            var jokeStackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock dateTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = comment.CreatedDate.ToString(),
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 12,
                Width = 330,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock contentTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = comment.Content,
                Width = 330,
                FontSize = 14,
                Margin = new Thickness(10, 5, 10, 5),
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            jokeStackPanel.Children.Add(contentTextBlock);
            jokeStackPanel.Children.Add(dateTextBlock);

            jokeButton.Content = jokeStackPanel;
            jokeGrid.Children.Add(jokeButton);
            return jokeGrid;
        }
    }
}
