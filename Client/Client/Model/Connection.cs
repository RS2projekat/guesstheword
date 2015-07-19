using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using System.Windows;
using System.Xml.Linq;

namespace Client.Model
{
    public static class Connection
    {
        public static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Timer _aTimer;
        private static Action ConnectErrorCallback;
        private static Action DisconnectCallback;
        private static Byte[] _buffer = new byte[1024];
        private static Action<Packet> PacketReceivedCallback;
        // ---------------------------------------------------------------
        public static void Disconnect(Action successDisconect)
        {
            DisconnectCallback = successDisconect;
            try
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.BeginDisconnect(false, OnBeginDisconnect, ClientSocket);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // ---------------------------------------------------------------
        public static bool IsConnected()
        {
            try
            {
                return !(ClientSocket.Poll(1, SelectMode.SelectRead) && ClientSocket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        // ---------------------------------------------------------------
        public static void Send(Packet core)
        {
            string message = core.RawXml;
            byte[] buffer = StringToByte(message);
            ClientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, OnBeginSend, null);

        }

        // ---------------------------------------------------------------
        public static void Connect(Action successCallback, Action errorCallback, Action<Packet> packetCallback)
        {
            try
            {
                ClientSocket.Connect(IPAddress.Loopback, 27015);
                successCallback();
                PacketReceivedCallback = packetCallback;
                _aTimer = new Timer(500);
                _aTimer.Enabled = true;
                _aTimer.Elapsed += OnTimedCheckConnection;
            }

            catch (SocketException)
            {
                ConnectErrorCallback = errorCallback;
                _aTimer = new Timer(2000);
                _aTimer.Enabled = true;
                _aTimer.Elapsed += OnTimedErrorConnectionEvent;
            }

            if (ClientSocket.Connected)
                ClientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, OnBeginReceive, null);
        }

        // ---------------------------------------------------------------
        private static void OnBeginReceive(IAsyncResult result)
        {
            try
            {
                int received = ClientSocket.EndReceive(result);
                byte[] dataBuffer = new byte[received];
                Array.Copy(_buffer, dataBuffer, received);

                Packet core = new Packet();
                string dataBufferStr = ByteToString(dataBuffer);
                core.XmlDocument = XDocument.Parse(dataBufferStr, LoadOptions.None);
                PacketReceivedCallback(core);

                ClientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None,
                    new AsyncCallback(OnBeginReceive), ClientSocket);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

       

        // ---------------------------------------------------------------
        public static void Send(string msg)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(msg);
                ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, OnBeginSend, null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // ---------------------------------------------------------------
        private static void OnBeginSend(IAsyncResult result)
        {
            ClientSocket.EndSend(result);
        }

        // ---------------------------------------------------------------
        private static void OnTimedCheckConnection(object sender, ElapsedEventArgs e)
        {
            if (!IsConnected())
            {
                _aTimer.Enabled = false;
                MessageBox.Show("Lost connection to server, please try loggin in again.");
                // todo show login window again
                /*Application.Current.MainWindow.Dispatcher.Invoke(() =>
                {
                    LoginController login = new LoginController(MainController.Model);
                    var successLogin = login.View.ShowDialog();
                    if (successLogin.HasValue && !successLogin.Value)
                    {
                        MessageBox.Show("Connection failed, closing program");
                        Application.Current.MainWindow.Close();
                    }
                });*/
            }
        }

        // ---------------------------------------------------------------
        private static void OnTimedErrorConnectionEvent(object sender, ElapsedEventArgs e)
        {
            _aTimer.Enabled = false;
            ConnectErrorCallback();
        }

        // ---------------------------------------------------------------
        private static void OnBeginDisconnect(IAsyncResult result)
        {
            try
            {
                Socket socket = (Socket) result.AsyncState;
                socket.EndDisconnect(result);
                DisconnectCallback();
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
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
    }
}