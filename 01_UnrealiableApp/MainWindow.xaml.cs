using System.Net;
using System.Threading;
using System.Windows;

namespace _01_UnreliableApp
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
            // When you click RSS button, you will note the app seems unresponsive
          
            var client = new WebClient();

            var data = client.DownloadString("http://rss.elmundo.es/rss/");
            Thread.Sleep(10000);

            RssText.Text = data;
        }

        private void CounterButton_Click(object sender, RoutedEventArgs e)
        {
            // When you try to press this button AFTER RssButton, you will note the counter is not increase
            // When Rss_Button finish, it seems Windows has a little click buffer and some events are raised
            
            // Both lines do the same

            //CounterText.Text = string.Format("Counter:{0}", counter);
            //counter++;

            CounterText.Text = $"Counter:{counter++}";
        }
    }
}
