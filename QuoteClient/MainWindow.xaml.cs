using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace QuoteClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnGetQuote(object sender, RoutedEventArgs e)
        {
            const int bufferSize = 1024;
            Cursor currentCursor = this.Cursor;
            this.Cursor = Cursors.Wait;

            string serverName = Properties.Settings.Default.ServerName;
            int port = Properties.Settings.Default.PortNumber;

            var client = new TcpClient();
            NetworkStream stream = null;
            try
            {
                client.Connect(serverName, port);
                byte[] buffer = new byte[bufferSize];
                int received = stream.Read(buffer, 0, bufferSize);
                if(received <= 0) {
                    return;
                }
                textQuote.Text = Encoding.Unicode.GetString(buffer).Trim('\0');
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Error Quote of the day", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if(stream != null)
                {
                    stream.Close();
                }
                if(client.Connected)
                {
                    client.Close();
                }
                this.Cursor = currentCursor;
            }
        }
    }
}
