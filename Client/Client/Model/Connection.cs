using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Xml.Linq;
using Timer = System.Timers.Timer;

namespace Client.Model
{

    public static class Connection
    {
        private static Timer _aTimer;
        private static Action ConnectErrorCallback;
        private static Action DisconnectCallback;
        private static Byte[] _buffer = new byte[10000];
        private static Action<Packet> PacketReceivedCallback { get; set; }
        public static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // ---------------------------------------------------------------
        public static void Disconnect(Action successDisconect)
        {
            DisconnectCallback = successDisconect;
            try
            {
                //ClientSocket.Shutdown(SocketShutdown.Both);
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
            ClientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, OnBeginSend, ClientSocket);

        }

        // ---------------------------------------------------------------
        public static void Connect(Action successCallback, Action errorCallback, Action<Packet> packetCallback)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Loopback, 27015);

                // Create a TCP/IP socket.

                ClientSocket.BeginConnect(remoteEP, OnBeginConnect, ClientSocket);

                PacketReceivedCallback = packetCallback;
                
                _aTimer = new Timer(500);
                _aTimer.Enabled = true;
                _aTimer.Elapsed += OnTimedCheckConnection;
                successCallback();

            }

            catch (SocketException)
            {
                ConnectErrorCallback = errorCallback;
                _aTimer = new Timer(2000);
                _aTimer.Enabled = true;
                _aTimer.Elapsed += OnTimedErrorConnectionEvent;
            }

            
        }

        // ---------------------------------------------------------------
        private static void OnBeginConnect(IAsyncResult ar)
        {
            try
            {
                // Complete the connection.
                if (ClientSocket.Connected)
                {
                    ClientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, OnBeginReceive, ClientSocket);
                }
                

                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                // Signal that the connection has been made.;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // ---------------------------------------------------------------
        private static void OnBeginReceive(IAsyncResult result)
        {
            try
            {
                Socket client = (Socket)result.AsyncState;
                int bytesRead = client.EndReceive(result);
                string response = ByteToString(_buffer);

                Packet receivedPacket = new Packet();
                
                receivedPacket.XmlDocument = XDocument.Parse(response, LoadOptions.None);
                PacketReceivedCallback(receivedPacket);

                _buffer = new byte[10000];

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
                msg += "\n";
                byte[] data = Encoding.ASCII.GetBytes(msg);
                ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, OnBeginSend, ClientSocket);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // ---------------------------------------------------------------
        private static void OnBeginSend(IAsyncResult result)
        {
            try
            {
                Socket client = (Socket) result.AsyncState;
                client.EndSend(result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

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
                Socket client = (Socket) result.AsyncState;
                client.EndDisconnect(result);
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
            return Encoding.UTF8.GetBytes(message);
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