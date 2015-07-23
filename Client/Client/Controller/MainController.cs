using System.ComponentModel;
using System.Linq;
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
        private AsyncObservableCollection<ClientModel> _users;
        private AsyncObservableCollection<string> _messages;

        public AsyncObservableCollection<ClientModel> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public AsyncObservableCollection<string> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }
        public MainController(MainWindow window)
        {
            View = window;
            View.Show();

            Users = new AsyncObservableCollection<ClientModel>();
            Messages = new AsyncObservableCollection<string>();

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
                case Command.REFRESH_USER_LIST:
                    XElement dataElement = core.DataNode;
                    Users.Clear();
                    foreach (XElement userElement in dataElement.Element("users").Elements("user"))
                    {
                        ClientModel temp = new ClientModel();
                        if (Users.Where(x => x.NickName == userElement.Value).Count() != 0)
                            continue;
                        temp.NickName = userElement.Value;
                        Users.Add(temp);
                    }


                    break;

                case Command.MESSAGE:
                    _messages.Add(core.GetString("nick_name") + ": " + core.GetString("message"));

                    break;
            }
        }

        private void DoBinding()
        {
            View.UsersList.ItemsSource = Users;
            View.Messages.ItemsSource = Messages;
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

            Packet core = new Packet();
            core.AddCommand(Command.MESSAGE);
            core.AddData("nick_name", Model.NickName);
            core.AddData("message", message);

            Connection.Send(core);
            View.TextBoxChat.Document.Blocks.Clear();
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Packet core = new Packet();
            core.AddCommand(Command.LOGOUT_REQUEST);
            core.AddData("username", Model.NickName);
            Connection.Send(core);

            if (Connection.ClientSocket.Connected)
            {
                Connection.Disconnect(CloseWindow);
            }
        }

        private void CloseWindow()
        {
            View.Dispatcher.Invoke(() =>
            {
                View.Close();
            });
        }

       

        public static ClientModel Model = new ClientModel();
        
        public static MainWindow View { get; set; }




        
    }
}
