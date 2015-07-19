using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
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
            Server.StartServer(HandleSuccessLabel, MessageList, PacketReceivedCallback);
        }

        private void PacketReceivedCallback(Packet core, Socket client)
        {
            int command = core.GetCommand();

            switch (command)
            {
                case Command.LOGIN: // login

                    string nickName = core.GetString("nickname");
                    /*  if (ClientSockets.First(x => x.NickName == nickName) != null) TODO
                      {
                          var returnCore = new Packet();
                          returnCore.AddCommand(Command.USER_NICKNAME_IN_USE);
                      }
                      else*/
                    {
                        ClientSocket clientSocket = new ClientSocket(client);
                        clientSocket.NickName = nickName;
                        Model.Server.ClientSockets.Add(clientSocket);
                        
                        // Send response that user have been successfully connected
                        core = new Packet();
                        core.AddCommand(Command.USER_SUCCESSFULL_LOGIN);
                        Model.Server.SendData(client, core);

                        // Send notification to other users that new client has been connected so list can be refreshed
                        core = Packet.Instance;
                        core.AddCommand(Command.NEW_USER);
                        XElement usersElement = core.AddDataElement("users");

                        foreach (ClientSocket socket in Model.Server.ClientSockets)
                        {
                            XElement user = new XElement("user");
                            user.Value = socket.NickName;
                            usersElement.Add(user);
                        }

                        Model.Server.SendNotificationToAll(core);
                    }
                    break;
            }
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