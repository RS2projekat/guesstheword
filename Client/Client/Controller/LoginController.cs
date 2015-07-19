using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Client.Model;
using Client.View;

namespace Client.Controller
{
    public class LoginController
    {
        private int attemps;
        private static Action<Packet> PacketReceivedCallback;
        private Thread ConnectionThread;
        public ClientModel Model { get; set; }
        public LoginView View { get; set; }

        public LoginController(ClientModel model, Action<Packet> packetCallback)
        {
            View = new LoginView();
            PacketReceivedCallback = packetCallback;
            Model = model;
            AttachEvents();
            DoBinding();
        }

        // ----------------------------------------------------------------------------------------
        private void DoBinding()
        {
            View.TextBoxNickname.SetBinding(TextBox.TextProperty, new Binding("NickName")
            {
                Source = Model,
                Mode = BindingMode.TwoWay
            });

        }

        private void AttachEvents()
        {
            View.ButtonLogin.Click += ButtonLoginOnClick;
            View.ButtonRegister.Click += ButtonRegisterOnClick;
            View.ButtonPlayAsGuest.Click += ButtonPlayAsGuestOnClick;
            
        }

        private void ButtonPlayAsGuestOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            View.Validation.Visibility = Visibility.Hidden;
            View.TextBoxNickname.BorderBrush = Brushes.White;
            View.TextBoxNickname.BorderThickness = new Thickness(1);
            if (string.IsNullOrWhiteSpace(Model.NickName))
            {
                View.Validation.Visibility = Visibility.Visible;
                View.TextBoxNickname.BorderBrush = Brushes.Blue;
                View.TextBoxNickname.BorderThickness = new Thickness(3);
                return;
            }
            View.LabelStatus.Content = "Connecting...";
            DoConnection();
        }

        private void ButtonRegisterOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            //todo
        }

        private void ButtonLoginOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (string.IsNullOrWhiteSpace(Model.NickName))
            {
                
            }
            View.LabelStatus.Content = "Connecting...";
            DoConnection();
           
        }

        private void DoConnection()
        {
            if (attemps == 5)
            {
                View.Dispatcher.Invoke(() =>
                {
                    View.LabelStatus.Content = "Connection failed, check server address or port";
                    View.DialogResult = false;
                });
            }
            
            attemps++;
            ConnectionThread = new Thread(TryConnect);
            ConnectionThread.Start();
            
        }

        private void TryConnect()
        {
            Connection.Connect(ConnectionSuccessCallback, ConnectionErrorCallback, PacketReceivedCallback);
        }

        private void ConnectionErrorCallback()
        {
            View.Dispatcher.Invoke(() =>
            {
                View.LabelStatus.Content = string.Format("Connection failed, connection attempts: {0}", attemps);
            });

            DoConnection();
        }

        private void ConnectionSuccessCallback()
        {
            View.Dispatcher.Invoke(() =>
            {
                View.LabelStatus.Content = "Connected";
            });

            // After we connected we need to login with nickname/password
            Model.Login();
        }
    }
}
