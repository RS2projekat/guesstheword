using System;
using System.CodeDom;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using Client.Model;

namespace Client.Controller
{
    public class MainController
    {
        public MainController(MainWindow window)
        {
            View = window;
            View.Show();

            LoginController login = new LoginController(Model);
            var successLogin = login.View.ShowDialog();
            if (successLogin.HasValue && !successLogin.Value)
            {
                MessageBox.Show("Connection failed, closing program");
                View.Close();
            }

            AttachEvents();


        }

        private void AttachEvents()
        {
            View.Closing += ViewOnClosing;
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (Connection.ClientSocket.Connected)
            {
                Connection.Disconnect(CloseWindow);
            }
        }

        private void CloseWindow()
        {
            View.Close();
        }

       

        public static ClientModel Model = new ClientModel();
        
        public static MainWindow View { get; set; }




        
    }
}
