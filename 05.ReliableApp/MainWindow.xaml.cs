using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace _05.ReliableApp
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

        private async void RssButton_Click(object sender, RoutedEventArgs e)
        {
            // More improves, we introduce Async and await 
            // First we add async to the method, only when we test "First way", if we test "Second way" remove async from method
            // Async allows to compiler to run async code in this method
            // Async method NOT make the method async

            // First way

            // We want to reproduce the 02.ReliableApp pattern but using Task, async and await
            //RssButton.IsEnabled = false;
            //BusyIndicator.Visibility = Visibility.Visible;

            //// These 2 lines are the same, but second use sugar syntax
            ////var result = await Task.Run<string>(() =>
            //var result = await Task.Run(() =>
            //    {
            //        var webClient = new WebClient();
            //        return webClient.DownloadString("http://rss.elmundo.es/rss/");
            //    }
            //);

            //// The await in previous line, wait till task finish. But the task are executing in another thread
            //// Don't need to call dispatcher because we're in the ui thread 
            //RssText.Text = result;
            //RssButton.IsEnabled = true;
            //BusyIndicator.Visibility = Visibility.Hidden;


            // Second way
            // Comment previous
            // And make a new method, i will work as usual
            // GetFeedAsync();

            // Third way. Try to get the exception
            // Comment previous code
            // And make a new method
            // GetFeedWithExceptionAsync();
            //try
            //{
            //    GetFeedWithExceptionAsync();
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //Four way. Try to get the exception
            //Comment previous code
            //And make a new method
            //GetFeedWithExceptionFixedAsync();
            //try
            //{
            //    GetFeedWithExceptionFixedAsync();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            // Comment previous code
            // Dont forget to add async to this 
            try
            {
                await GetFeedWithExceptionFixedBetterAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task GetFeedWithExceptionFixedBetterAsync()
        {
            // The method who call this method needs to be async and the call needs to use await
            // This way, you will get the exception
            throw new UnauthorizedAccessException();

            RssButton.IsEnabled = false;
            BusyIndicator.Visibility = Visibility.Visible;

            var result = await Task.Run(() =>
            {
                // Again, we test an exception inside the task
                // But even, GetFeedWithExceptionAsync is using try/catch, the program will crash
                throw new UnauthorizedAccessException();

                var webClient = new WebClient();
                return webClient.DownloadString("http://rss.elmundo.es/rss/");
            }
            );

            // The await in previous line, wait till task finish. But the task are executing in another thread
            // Don't need to call dispatcher because we're in the ui thread 
            RssText.Text = result;
            RssButton.IsEnabled = true;
            BusyIndicator.Visibility = Visibility.Hidden;
        }

        private async Task GetFeedWithExceptionFixedAsync()
        {
            // Even if we throw an exception like this, the call don't catch the exception. This is due a async keyword
            // To fix it, we need return a Task. The app, will not crash, but exception is not call
            // The best way is add async to the method who cal this method and add await. Take a look GetFeedWithExceptionFixedBetterAsync
            throw new UnauthorizedAccessException();

            try
            {
                RssButton.IsEnabled = false;
                BusyIndicator.Visibility = Visibility.Visible;

                var result = await Task.Run(() =>
                {
                    // Again, we test an exception inside the task
                    // But even, GetFeedWithExceptionAsync is using try/catch, the program will crash
                    throw new UnauthorizedAccessException();

                    var webClient = new WebClient();
                    return webClient.DownloadString("http://rss.elmundo.es/rss/");
                }
                );

                // The await in previous line, wait till task finish. But the task are executing in another thread
                // Don't need to call dispatcher because we're in the ui thread 
                RssText.Text = result;
                RssButton.IsEnabled = true;
                BusyIndicator.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async void GetFeedWithExceptionAsync()
        {
            RssButton.IsEnabled = false;
            BusyIndicator.Visibility = Visibility.Visible;

            var result = await Task.Run(() =>
            {
                // Again, we test an exception inside the task
                // But even, GetFeedWithExceptionAsync is using try/catch, the program will crash
                throw new UnauthorizedAccessException();

                var webClient = new WebClient();
                return webClient.DownloadString("http://rss.elmundo.es/rss/");
            }
            );

            // The await in previous line, wait till task finish. But the task are executing in another thread
            // Don't need to call dispatcher because we're in the ui thread 
            RssText.Text = result;
            RssButton.IsEnabled = true;
            BusyIndicator.Visibility = Visibility.Hidden;
        }

        private async void GetFeedAsync()
        {
            RssButton.IsEnabled = false;
            BusyIndicator.Visibility = Visibility.Visible;

            // These 2 lines are the same, but second use sugar syntax
            //var result = await Task.Run<string>(() =>
            var result = await Task.Run(() =>
                {
                    // Again, we test an exception inside the task
                    throw new UnauthorizedAccessException();

                    var webClient = new WebClient();
                    return webClient.DownloadString("http://rss.elmundo.es/rss/");
                }
            );

            // The await in previous line, wait till task finish. But the task are executing in another thread
            // Don't need to call dispatcher because we're in the ui thread 
            RssText.Text = result;
            RssButton.IsEnabled = true;
            BusyIndicator.Visibility = Visibility.Hidden;
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
