using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Server.Model
{
    public class Server
    {
        public static Socket ServerListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public delegate void ServerMessageLinqHandler(Packet core);
        public delegate void ServerMessageErrorLinqHandler(Packet errorCore, Exception error);

        public static List<ClientSocket> ClientSockets = new List<ClientSocket>();

        private static AsyncObservableCollection<LogMessage> _messageList;
        private int _numberOfPendingClients = 10;
        private static Byte[] _buffer = new byte[1024];


        public bool HasClients()
        {
            return ClientSockets.Count > 0;
        }

        public void StartServer(Action successCallback, AsyncObservableCollection<LogMessage> messageList)
        {
            _messageList = messageList;
            _messageList.Add(new LogMessage("Server started listening"));
            ServerListener.Bind(new IPEndPoint(IPAddress.Any, 27015));
            ServerListener.Listen(_numberOfPendingClients);
            successCallback();
            ServerListener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult result)
        {
            Socket socket = ServerListener.EndAccept(result);
            _messageList.Add(new LogMessage("Client connected "));
            ClientSockets.Add(new ClientSocket(socket));
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

                string text = "Message received: " + Encoding.ASCII.GetString(dataBuffer);
                _messageList.Add(new LogMessage(text));
   
                SendData(socket, "Server je odgovorio");
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
           
        }

        //todo
        public static void ExchangePacketAsync(Packet core, ServerMessageLinqHandler successHandler, ServerMessageErrorLinqHandler errorHandler)
        {
            
        }

        private static void SendData(Socket client, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnBeginSend), client);
        }

        private static void OnBeginSend(IAsyncResult result)
        {
            Socket socket = (Socket) result.AsyncState;
            socket.EndSend(result);
        }
    }
}