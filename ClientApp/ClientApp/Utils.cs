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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Static class with WPF Utils.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Create long empty label which can be used as filling on page/window.
        /// </summary>
        /// <returns>Long empty label</returns>
        public static Label LongEmptyLabel()
        {
            Label label = new Label()
            {
                Margin = new Thickness(0, 0, 3000, 0)
            };
            return label;
        }
        
        /// <summary>
        /// Create box with rounded corner, nice looking with informataion about joke.
        /// </summary>
        /// <param name="joke">Joke object.</param>
        /// <param name="isPink">Set background color pink if true else set blue.</param>
        /// <param name="OnClick">RoutedEventHandler which defines action on click joke box.</param>
        /// <returns>Grid with information about joke.</returns>
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

        /// <summary>
        /// Create box with rounded corner, nice looking with informataion about comment.
        /// </summary>
        /// <param name="comment">Comment object.</param>
        /// <param name="isPink">Set background color pink if true else set blue.</param>
        /// <param name="OnClick">RoutedEventHandler which defines action on click comment box.</param>
        /// <returns>Grid with information about comment.</returns>
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

        /// <summary>
        /// Create box with rounded corner, nice looking with informataion about user.
        /// </summary>
        /// <param name="comment">User object.</param>
        /// <param name="isPink">Set background color pink if true else set blue.</param>
        /// <param name="OnClick">RoutedEventHandler which defines action on click user box.</param>
        /// <returns>Grid with information about user.</returns>
        public static Grid CreateUserContentGrid(User user, string comment, string joke, bool isPink, RoutedEventHandler OnClick)
        {
            var userGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 20)
            };
            userGrid.RowDefinitions.Add(new RowDefinition());
            userGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var userButton = new Button
            {
                Width = 350,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250)),
                BorderBrush = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250)),
                Margin = new Thickness(10),
                Cursor = Cursors.Hand
            };
            userButton.Click += OnClick;

            var userRectangle = new Rectangle
            {
                Width = 350,
                RadiusX = 10,
                RadiusY = 10
            };
            userRectangle.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 222, G = 222, B = 222 },
                ShadowDepth = 1,
                BlurRadius = 20
            };
            if (isPink)
                userRectangle.Fill = new SolidColorBrush(Color.FromArgb(80, 177, 101, 240));
            else
                userRectangle.Fill = new SolidColorBrush(Color.FromArgb(80, 112, 172, 250));


            userGrid.Children.Add(userRectangle);

            var userStackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock nicknameTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = "NICKNAME:   " + user.Nickname,
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 18,
                MaxWidth = 330,
                Width = 330,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock NameTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = user.Name + " " + user.Surname,
                MaxWidth = 330,
                Width = 330,
                FontSize = 16,
                Margin = new Thickness(10, 5, 10, 5),
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock EmailTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = "E-MAIL:   " + user.Email,
                MaxWidth = 330,
                Width = 330,
                FontSize = 16,
                Margin = new Thickness(10, 5, 10, 20),
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock LastCommentLabelTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = "OSTATNI KOMENTARZ:",
                MaxWidth = 300,
                Width = 300,
                FontSize = 14,
                Margin = new Thickness(10, 10, 10, 0),
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock LastCommentTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = comment,
                MaxWidth = 300,
                Width = 300,
                FontSize = 12,
                Margin = new Thickness(10),
                Padding = new Thickness(5),
                Background = new SolidColorBrush(Color.FromArgb(180, 255, 255, 255))
            };


            TextBlock LastJokeLabelTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = "OSTATNI DOWCIP:",
                Width = 300,
                MaxWidth = 300,
                FontSize = 14,
                Margin = new Thickness(10, 10, 10, 0),
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };

            TextBlock LastJokeTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = joke,
                Width = 300,
                MaxWidth = 300,
                FontSize = 12,
                Margin = new Thickness(10),
                Padding = new Thickness(5),
                Background = new SolidColorBrush(Color.FromArgb(180, 255, 255, 255))
            };

            userStackPanel.Children.Add(nicknameTextBlock);
            userStackPanel.Children.Add(NameTextBlock);
            userStackPanel.Children.Add(EmailTextBlock);
            userStackPanel.Children.Add(LastCommentLabelTextBlock);
            userStackPanel.Children.Add(LastCommentTextBlock);
            userStackPanel.Children.Add(LastJokeLabelTextBlock);
            userStackPanel.Children.Add(LastJokeTextBlock);

            userButton.Content = userStackPanel;
            userGrid.Children.Add(userButton);
            return userGrid;
        }

        /// <summary>
        /// Create TextBlock control with 'BRAK' text. Using when object doesn't exist in db.
        /// </summary>
        /// <returns>TextBlock with 'BRAK' text.</returns>
        public static TextBlock LackTextBox()
        {
            TextBlock lackTextBox = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = "BRAK",
                Margin = new Thickness(10, 10, 10, 10),
                FontSize = 25,
                MaxWidth = 330,
                Width = 330,
                Background = new SolidColorBrush(Color.FromArgb(0, 112, 172, 250))
            };
            return lackTextBox;
        }
    }
}
