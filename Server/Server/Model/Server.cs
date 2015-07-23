using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Linq;

namespace Server.Model
{
    public class Server
    {
        public static Socket ServerListener { get; set; }
        public delegate void ServerMessageLinqHandler(Packet core);
        public delegate void ServerMessageErrorLinqHandler(Packet errorCore, Exception error);

        public static List<ClientSocket> ClientSockets = new List<ClientSocket>();

        public static bool IsListening { get; set; }
        private static Action<Packet, Socket> PackageReceivedCallback;
        private static AsyncObservableCollection<LogMessage> _messageList;
        private int _numberOfPendingClients = 10;
        private static Byte[] _buffer = new byte[10000];


        public bool HasClients()
        {
            return ClientSockets.Count > 0;
        }

        public void StopServer(Action returnMethod)
        {
            if (HasClients())
            {
                var answer = MessageBox.Show("There are users connected, are you sure you want to stop server?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (answer != MessageBoxResult.Yes)
                    return;
            }

            foreach (ClientSocket clientSocket in ClientSockets)
            {
                try
                {
                    clientSocket.Socket.BeginDisconnect(false, OnBeginDisconnect, clientSocket);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            ClientSockets.Clear();
            IsListening = false;
            ServerListener.Close();
            returnMethod();
        }

        private void OnBeginDisconnect(IAsyncResult ar)
        {
            if (IsListening == false)
                return;
            Socket socket = (Socket) ar.AsyncState;
            socket.EndDisconnect(ar);
        }

        public void StartServer(Action successCallback, AsyncObservableCollection<LogMessage> messageList, Action<Packet, Socket> packageCallback)
        {
            ServerListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerListener.Bind(new IPEndPoint(IPAddress.Any, 27015));
            ServerListener.Listen(_numberOfPendingClients);
            _messageList = messageList;
            PackageReceivedCallback = packageCallback;
            _messageList.Add(new LogMessage("Server started listening"));
            IsListening = true;
            successCallback();

            ServerListener.BeginAccept(new AsyncCallback(AcceptCallback), ServerListener);

        }

        private static void AcceptCallback(IAsyncResult result)
        {
            if (IsListening == false) 
                return;

            Socket connectedClient = ServerListener.EndAccept(result);
            connectedClient.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnBeginReceiveCallback), connectedClient); 
            ServerListener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void OnBeginReceiveCallback(IAsyncResult result)
        {
           
            try
            {
                SocketError errorCode;
                Socket socket = (Socket)result.AsyncState;
                int received = socket.EndReceive(result, out errorCode);
                if (errorCode != SocketError.Success)
                {
                    received = 0;

                    foreach (ClientSocket clientSocket in ClientSockets)
                    {
                        if (!clientSocket.Connected)
                        {
                            ClientSockets.Remove(clientSocket);
                            _messageList.Add(new LogMessage("Client(" + clientSocket.NickName + ") disconnected."));
                            SendNotificationToAll(GetConnectedClients());
                            return;
                        }
                    }
                }
                

                byte[] dataBuffer = new byte[received];
                Array.Copy(_buffer, dataBuffer, received);

                Packet core = new Packet();
                string dataBufferStr = ByteToString(dataBuffer);
                if (String.IsNullOrWhiteSpace(dataBufferStr))
                    return;
                core.XmlDocument = XDocument.Parse(dataBufferStr, LoadOptions.None);

                PackageReceivedCallback(core, socket);


                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnBeginReceiveCallback), socket); 
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
           
        }

        public static Packet GetConnectedClients()
        {
            if (ClientSockets.Count < 1)
                return null;
            Packet core = Packet.Instance;
            core.AddCommand(Command.REFRESH_USER_LIST);
            XElement usersElement = core.AddDataElement("users");

            foreach (ClientSocket socket in Server.ClientSockets)
            {
                XElement user = new XElement("user");
                user.Value = socket.NickName;
                usersElement.Add(user);
            }
            return core;
        }

        public static void SendNotificationToAll(Packet core)
        {
            if (core == null)
                return;
            foreach (ClientSocket clientSocket in ClientSockets)
            {
                if (clientSocket.Socket.Connected)
                    SendData(clientSocket.Socket, core);
            }
        }

        //todo
        public static void ExchangePacketAsync(Packet core, ServerMessageLinqHandler successHandler, ServerMessageErrorLinqHandler errorHandler)
        {
            
        }

        // Method that converts byte to string
        public static string ByteToString(byte[] message)
        {
            StringBuilder str = new StringBuilder(message.Length);
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == ('\r') || message[i] == '\n' || message[i] == '\0')
                    continue;

                str.Append(Convert.ToChar(message[i]));
            }

            return str.ToString();
        }

        // Method that converts string message to byte
        private static Byte[] StringToByte(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        public static void SendData(Socket client, Packet core)
        {
            string msg = core.RawXml;
            byte[] data = StringToByte(msg);
            try
            {
                client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnBeginSend), client);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static void OnBeginSend(IAsyncResult result)
        {
            Socket socket = (Socket) result.AsyncState;
            socket.EndSend(result);
        }
    }
}