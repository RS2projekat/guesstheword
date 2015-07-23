using System;
using System.Collections.ObjectModel;
using System.Linq;
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
                Server.StopServer(() =>
                {
                    View.LabelServerStatus.Background = Brushes.Red;
                    View.LabelServerStatus.Content = "Server stopped...";

                    View.ButtonServerStart.Content = "Start";
                });

              
            }

        }

      

        public void StartServer()
        {
            Server.StartServer(HandleSuccessLabel, MessageList, PacketReceivedCallback);
        }

        private void PacketReceivedCallback(Packet core, Socket client)
        {
            int command = core.GetCommand();
            string nickName = "";
            Packet returnedCore;
            switch (command)
            {
                case Command.LOGIN: // login

                    nickName = core.GetString("nickname");
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

                        MessageList.Add(new LogMessage("Client (" + nickName + ") connected"));
                        // Send response that user have been successfully connected
                        RefreshClientsUsersList();
                    }
                    break;
                case Command.LOGOUT_REQUEST:
                    var xElement = core.DataNode.Element("username");
                    if (xElement != null) nickName = xElement.Value;
                    if (string.IsNullOrWhiteSpace(nickName))
                        return;
                    if (Model.Server.ClientSockets.Count(x => x.NickName == nickName) > 0)
                    {
                        int id =
                            Model.Server.ClientSockets.IndexOf(
                                Model.Server.ClientSockets.First(x => x.NickName == nickName));
                        Model.Server.ClientSockets.RemoveAt(id);
                        MessageList.Add(new LogMessage("Client (" + nickName + ") disconnected"));
                        RefreshClientsUsersList();
                    }
                    break;
            }
        }

        public void RefreshClientsUsersList()
        {
            Packet returnedCore = Model.Server.GetConnectedClients();
            Model.Server.SendNotificationToAll(returnedCore);
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