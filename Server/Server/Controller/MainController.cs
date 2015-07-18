using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Server.Model;
using Server.View;

namespace Server.Controller
{

    public class MainController
    {
        public MainWindow View { get; set; }
        public AsyncObservableCollection<LogMessage> MessageList { get; set; }
        private Thread _serverListenThread;
        public Model.Server Server { get; set; }
        // ----------------------------------------------------------------------------------
        private void AttachEvents()
        {
            View.TextBoxPort.PreviewTextInput += NumberValidationTextBox;
            View.ButtonServerStart.Click += ButtonServerStartOnClick;
        }

        // -----------------------------------------------------------------------------------
        private void ButtonServerStartOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (View.ButtonServerStart.Content.ToString() == "Start")
            {
                View.ButtonServerStart.Content = "Stop";
                View.LabelServerStatus.Background = Brushes.Yellow;
                View.LabelServerStatus.Content = "Server starting...";
                Server = new Model.Server();
                _serverListenThread = new Thread(StartServer);
                _serverListenThread.Start();
            }
            else if (View.ButtonServerStart.Content.ToString() == "Stop")
            {
                if (Server.HasClients())
                {
                    var answer = MessageBox.Show("There are users connected, are you sure you want to stop server?",
                        "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (answer != MessageBoxResult.Yes)
                        return;
                }

                
                foreach (ClientSocket clientSocket in Model.Server.ClientSockets)
                {
                    try
                    {
                        clientSocket.Socket.Disconnect(false);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }

                View.LabelServerStatus.Background = Brushes.Red;
                View.LabelServerStatus.Content = "Server stopped...";

                View.ButtonServerStart.Content = "Start";
            }

        }

        public void StartServer()
        {
            Server.StartServer(HandleSuccessLabel, MessageList);
        }

        // -----------------------------------------------------------------------------------
        private void HandleSuccessLabel()
        {
            View.Dispatcher.Invoke(() =>
            {
                View.LabelServerStatus.Background = Brushes.Green;
                View.LabelServerStatus.Content = "Server is listening...";
            });

        }

        // -----------------------------------------------------------------------------------
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        // -----------------------------------------------------------------------------------
        internal void Init(MainWindow main)
        {
            View = main;

            MessageList = new AsyncObservableCollection<LogMessage>();
            AttachEvents();
            DoBinding();
        }

        // -----------------------------------------------------------------------------------
        private void DoBinding()
        {
            View.ListViewMessages.ItemsSource = MessageList;
        }
    }
}