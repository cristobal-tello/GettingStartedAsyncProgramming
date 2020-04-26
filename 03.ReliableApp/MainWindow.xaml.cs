using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace _03.ReliableApp
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
            // Second improve
            // We use Task or Task<T>
            // Task represents an async operation and don't return any value
            // Task<T> represents an async operation that will return a T value

            // The Action when Task finish is call "Continuation". In fact, on 02.ReliableApp, the delegate is an "Continuation"

            // We want to reproduce the 02.ReliableApp pattern but using Task
            RssButton.IsEnabled = false;
            BusyIndicator.Visibility = Visibility.Visible;

            var task = Task.Run(() => 
                    {
                        var webClient = new WebClient();
                        return webClient.DownloadString("http://rss.elmundo.es/rss/");
                    }
            );

            task.ContinueWith((t) =>
                {
                    // Next 2 lines will crash!! We cannot use this object in a different thread!!!
                    //BusyIndicator.Visibility = Visibility.Hidden;
                    //RssButton.IsEnabled = true;

                    // Use Dispatcher instead
                    Dispatcher.Invoke(() =>
                        {
                            RssText.Text = t.Result;
                            BusyIndicator.Visibility = Visibility.Hidden;
                            RssButton.IsEnabled = true;
                        }
                    );
                }
            );
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
