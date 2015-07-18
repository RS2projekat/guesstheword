using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using System.Windows;
using Client.Controller;

namespace Client.Model
{
    public static class Connection
    {
        public static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Timer _aTimer;
        private static Action ConnectErrorCallback;
        private static Action DisconnectCallback;
        public static IAsyncResult OnEndDisconnect { get; set; }
        private static Byte[] _buffer = new byte[1024];

        // ---------------------------------------------------------------
        public static void Disconnect(Action successDisconect)
        {
            DisconnectCallback = successDisconect;
            try
            {
                ClientSocket.BeginDisconnect(false, OnBeginDisconnect, ClientSocket);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static bool IsConnected()
        {
            try
            {
                return !(ClientSocket.Poll(1, SelectMode.SelectRead) && ClientSocket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        // ---------------------------------------------------------------
        public static void Connect(Action successCallback, Action errorCallback)
        {
            try
            {
                ClientSocket.Connect(IPAddress.Loopback, 27015);
                successCallback();
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

            if (IsConnected())
                ClientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, OnBeginReceive, null);
        }

        private static void OnBeginReceive(IAsyncResult result)
        {
            int received = ClientSocket.EndReceive(result);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);
            string text = "Message received: " + Encoding.ASCII.GetString(dataBuffer);
            MessageBox.Show(text);
        }

        public static void Send(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, OnBeginSend, null);
        }

        private static void OnBeginSend(IAsyncResult result)
        {
            ClientSocket.EndSend(result);
        }

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
                socket.EndDisconnect(OnEndDisconnect);
                DisconnectCallback();
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}