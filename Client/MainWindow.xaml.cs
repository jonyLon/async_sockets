using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StreetLookupClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnGetStreetsClick(object sender, RoutedEventArgs e)
        {
            resultTextBlock.Text = await GetStreetsAsync(zipCodeTextBox.Text);
        }

        private Task<string> GetStreetsAsync(string zipCode)
        {
            return Task.Run(() =>
            {
                using (var client = new TcpClient("localhost", 8000))
                {
                    var stream = client.GetStream();
                    byte[] data = Encoding.ASCII.GetBytes(zipCode);

                    stream.Write(data, 0, data.Length);

                    byte[] response = new byte[1024];
                    int bytesRead = stream.Read(response, 0, response.Length);
                    return Encoding.ASCII.GetString(response, 0, bytesRead);
                }
            });
        }
    }
}