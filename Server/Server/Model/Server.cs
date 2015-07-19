using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace Server.Model
{
    public class Server
    {
        public static Socket ServerListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public delegate void ServerMessageLinqHandler(Packet core);
        public delegate void ServerMessageErrorLinqHandler(Packet errorCore, Exception error);

        public static List<ClientSocket> ClientSockets = new List<ClientSocket>();

        private static Action<Packet, Socket> PackageReceivedCallback;
        private static AsyncObservableCollection<LogMessage> _messageList;
        private int _numberOfPendingClients = 10;
        private static Byte[] _buffer = new byte[1024];


        public bool HasClients()
        {
            return ClientSockets.Count > 0;
        }

        public void StartServer(Action successCallback, AsyncObservableCollection<LogMessage> messageList, Action<Packet, Socket> packageCallback)
        {
            _messageList = messageList;
            PackageReceivedCallback = packageCallback;
            _messageList.Add(new LogMessage("Server started listening"));
            ServerListener.Bind(new IPEndPoint(IPAddress.Any, 27015));
            ServerListener.Listen(_numberOfPendingClients);
            successCallback();
            ServerListener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult result)
        {
            Socket socket = ServerListener.EndAccept(result);
            _messageList.Add(new LogMessage("Client connected"));
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnBeginReceiveCallback), socket); 
            ServerListener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void OnBeginReceiveCallback(IAsyncResult result)
        {
           
            try
            {
                Socket socket = (Socket) result.AsyncState;
                int received  = socket.EndReceive(result);

                byte[] dataBuffer = new byte[received];
                Array.Copy(_buffer, dataBuffer, received);


                Packet core = new Packet();
                string dataBufferStr = ByteToString(dataBuffer);
   
                core.XmlDocument = XDocument.Parse(dataBufferStr, LoadOptions.None);

                PackageReceivedCallback(core, socket);


                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnBeginReceiveCallback), socket); 
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
           
        }

        public static void SendNotificationToAll(Packet core)
        {
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
            Byte[] byteMessage = new byte[message.Length + 1];

            for (int i = 0; i < message.Length; i++)
            {
                byteMessage[i] = Convert.ToByte(message[i]);
            }

            return byteMessage;
        }

        public static void SendData(Socket client, Packet core)
        {
            byte[] data = StringToByte(core.RawXml);
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