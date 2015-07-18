using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Client.Model;
using Client.View;

namespace Client.Controller
{
    public class LoginController
    {
        private int attemps;
        private Thread ConnectionThread;
        public ClientModel Model { get; set; }
        public LoginView View { get; set; }

        public LoginController(ClientModel model)
        {
            View = new LoginView();
            Model = model;
            AttachEvents();
            DoBinding();
        }

        // ----------------------------------------------------------------------------------------
        private void DoBinding()
        {
            View.TextBoxUsername.SetBinding(TextBox.TextProperty, new Binding("UserName")
            {
                Source = this,
                Mode = BindingMode.TwoWay
            });

        }

        private void AttachEvents()
        {
            View.ButtonLogin.Click += ButtonLoginOnClick;
            View.ButtonRegister.Click += ButtonRegisterOnClick;
            
        }

        private void ButtonRegisterOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            //todo
        }

        private void ButtonLoginOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            //todo validation
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
            Connection.Connect(ConnectionSuccessCallback, ConnectionErrorCallback);
            Connection.Send("Poruka je poslata.");
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
                View.DialogResult = true;
            });
            
        }
    }
}
