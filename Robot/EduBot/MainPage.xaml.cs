using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EduBot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        RobotControl robotControl = new RobotControl();
        StreamSocketListener socketListener;

        public MainPage()
        {
            this.InitializeComponent();
            CreateServer();
            buttonUpdate.Click += ButtonUpdate_Click;
            updateLog();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            updateLog();
        }

        public async void CreateServer()
        {
            try
            {
                //Skapar StreamSocketListener för att börja lyssna på TCP-anslutningar på port 1337.
                socketListener = new StreamSocketListener();
                socketListener.ConnectionReceived += SocketListener_ConnectionReceived;
                await socketListener.BindServiceNameAsync("1337");
            }
            catch
            {
                robotControl.Log("Error while creating server!");
                //Handle exception.
            }
        }

        /// <summary>
        /// Läser inkommande sträng och kör kommandon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void SocketListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            //Read line from the remote client.
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(inStream);
            string request = await reader.ReadLineAsync();

            robotControl.Run(request);
            updateLog();
        }

        /// <summary>
        /// Uppdaterar logg
        /// </summary>
        async void updateLog()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.textBoxLog.Text = "Log:\n";
                string[] log = robotControl.GetLog();
                foreach (string message in log)
                {
                    this.textBoxLog.Text += "\n" + message;
                }
            });
        }
    }
}
