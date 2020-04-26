using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace _04.ReliableApp
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
            // Third improve
            // As in previous 03.ReliableApp, we use Task or Task<T>
            // But, what happen is main task throws an exception. 
            // Note: Test it in Release mode NOT in Debug mode

            // We want to reproduce the 02.ReliableApp pattern but using Task
            RssButton.IsEnabled = false;
            BusyIndicator.Visibility = Visibility.Visible;

            // First way

            //var task = Task.Run(() =>
            //    {
            //        var webClient = new WebClient();
            //        // Enable just to check how exception works
            //        // throw new UnauthorizedAccessException();        // If you run the app, nobody notes this exception. But on ContinueWith, take a look IsFaulted
            //        return webClient.DownloadString("http://rss.elmundo.es/rss/");
            //    }
            //);

            //task.ContinueWith((t) =>
            //{
            //    if (t.IsFaulted)        // True when an exception occurs in task
            //    {
            //        Dispatcher.Invoke(() =>
            //            {
            //                RssText.Text = "Error getting RSS";
            //            }
            //        );
            //    }
            //    else
            //    {
            //        Dispatcher.Invoke(() =>
            //            {
            //                RssText.Text = t.Result;
            //            }
            //        );
            //    }
            //    Dispatcher.Invoke(() =>
            //        {
            //            BusyIndicator.Visibility = Visibility.Hidden;
            //            RssButton.IsEnabled = true;
            //        }
            //    );
            //}
            //);


            // Second way
            // Comment first way
            // We use task.ConfigureAwait allows configure where you want to run the "Continuation", that is the end of a task
            // When is set to true, Task automatically backs to original context, in our case, it is the main ui thread
            // Then we don't need to use Dispatch

            var task = Task.Run(() =>
                {
                    var webClient = new WebClient();
                    // Enable just to check how exception works
                    // throw new UnauthorizedAccessException();        // If you run the app, nobody notes this exception. But on ContinueWith, take a look IsFaulted
                    return webClient.DownloadString("http://rss.elmundo.es/rss/");
                }
            );

            task.ConfigureAwait(true)
                .GetAwaiter()
                .OnCompleted(() =>
                    {
                        // Note, we don't use dispatcher
                        RssText.Text = task.Result;
                        BusyIndicator.Visibility = Visibility.Hidden;
                        RssButton.IsEnabled = true;
                    });
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
