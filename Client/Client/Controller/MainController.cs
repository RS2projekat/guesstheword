using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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


        private ObservableCollection<CustomPoint> coordinates = new ObservableCollection<CustomPoint>();
        private bool clicked = false;
        private Canvas previousCanvas;

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
            View.Canvas.MouseDown += CanvasOnMouseDown;
            View.Canvas.MouseMove += CanvasOnMouseMove;
            View.Canvas.MouseUp += CanvasOnMouseUp;
            View.ButtonClear.Click += ButtonClearOnClick;
            View.ButtonUndo.Click += ButtonUndoOnClick;
        }

        private void ButtonUndoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (previousCanvas != null)
            {
                int i = View.Canvas.Children.IndexOf(previousCanvas);
                if (i > 0)
                {
                    View.Canvas.Children.RemoveAt(i);
                    previousCanvas = (Canvas)View.Canvas.Children[i - 1];
                }
                else if (i == 0)
                {
                    View.Canvas.Children.Clear();
                    previousCanvas = null;
                }


            }
        }

        private void ButtonClearOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            coordinates.Clear();
            View.Canvas.Children.Clear();
        }

        private void CanvasOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Canvas obj = new Canvas();
            for (int i = 0; i < coordinates.Count - 5; i += 5)
            {

                Line line = new Line();
                line.Stroke = Brushes.Red;
                line.X1 = coordinates[i].X;
                line.X2 = coordinates[i + 5].X;
                line.Y1 = coordinates[i].Y;
                line.Y2 = coordinates[i + 5].Y;
                line.StrokeThickness = 2;
                obj.Children.Add(line);
                previousCanvas = obj;
            }
            View.Canvas.Children.Add(obj);
            coordinates.Clear();
        }

        private void CanvasOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && clicked)
            {
                // MessageBox.Show(string.Format("Move. Mouse position x: {0} y: {1}", e.GetPosition(this).X, e.GetPosition(this).Y));
                coordinates.Add(new CustomPoint(Convert.ToInt32(e.GetPosition(View.Canvas).X), Convert.ToInt32(e.GetPosition(View.Canvas).Y)));

            }
        }

        private void CanvasOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            clicked = true;
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
