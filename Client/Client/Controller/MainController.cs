using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using Client.Model;
using Client.View;

namespace Client.Controller
{
    public class MainController
    {
        private LoginController _login;
        private AsyncObservableCollection<ClientModel> Users;
        public MainController(MainWindow window)
        {
            View = window;
            View.Show();

            Users = new AsyncObservableCollection<ClientModel>();

            _login = new LoginController(Model, PacketReceivedCallback);
            var successLogin = _login.View.ShowDialog();
            if (successLogin.HasValue && !successLogin.Value)
            {
                MessageBox.Show("Connection failed, closing program");
                View.Close();
            }

            AttachEvents();
            DoBinding();

        }

        public void PacketReceivedCallback(Packet core)
        {
             
            int command = core.GetCommand();

            switch (command)
            {
                case Command.USER_SUCCESSFULL_LOGIN:
                    _login.View.Dispatcher.Invoke(() =>
                    {
                        _login.View.DialogResult = true;
                    });
                   
                    break;
                case Command.NEW_USER:
                    XElement dataElement = core.DataNode;
                    Users.Clear();
                    foreach (XElement userElement in dataElement.Element("users").Elements("user"))
                    {
                        ClientModel temp = new ClientModel();
                        temp.NickName = userElement.Value;
                        Users.Add(temp);
                    }


                    break;
            }
        }

        private void DoBinding()
        {
            View.UsersList.ItemsSource = Users;
        }

        private void AttachEvents()
        {
            View.Closing += ViewOnClosing;
            View.ButtonSendChat.Click += ButtonSendChatOnClick;
        }

        private void ButtonSendChatOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            string message =
                new TextRange(View.TextBoxChat.Document.ContentStart, View.TextBoxChat.Document.ContentEnd).Text;
            if (string.IsNullOrWhiteSpace(message))
                return;

            Connection.Send(message);



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
