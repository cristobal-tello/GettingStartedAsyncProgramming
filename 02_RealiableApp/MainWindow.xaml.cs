using System;
using System.Net;
using System.Windows;

namespace _02_ReliableApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int counter;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            counter = 0;
        }

        private void RssButton_Click(object sender, RoutedEventArgs e)
        {
            // First improve
            // We use DownloadStringAsync and DownloadStringCompleted delegate
            // Also, we add an spin icon and disable RssButton when we click on it
            // After complete, we restore the button again
            
            // When you click RSS button, NOW the app will be a little bit more (not much!!!) responsive
            // Even you will note it seems the feed is received faster than previous unreliable app
            BusyIndicator.Visibility = Visibility.Visible;
            RssButton.IsEnabled = false;

            var client = new WebClient();

            BusyIndicator.Visibility = Visibility.Visible;

            // Use Async method and delegate
            client.DownloadStringAsync(new Uri("http://rss.elmundo.es/rss/"));
            client.DownloadStringCompleted += DownloadStringCompleted;

            // This line is no longer need it and it will move into delegate
            //  RssText.Text = data;
        }

        private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            RssText.Text = e.Result;
            BusyIndicator.Visibility = Visibility.Hidden;
            RssButton.IsEnabled = true;
        }

        private void CounterButton_Click(object sender, RoutedEventArgs e)
        {
            // When you try to press this button AFTER RssButton, you will note the counter is not increase
            // When Rss_Button finish, it seems Windows has a little click buffer and some events are raised

            // Both lines do the same

            //CounterText.Text = string.Format("Counter:{0}", counter);
            //counter++;

            CounterText.Text = $"Counter : {counter++}";
        }
    }
}