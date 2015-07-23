using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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


        private ObservableCollection<CustomPoint> CanvasCoordinates = new ObservableCollection<CustomPoint>();
        private bool clicked;
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
                case Command.SEND_COORDINATES:
                    ParseCoordinates(core);
                    break;
                case Command.CANVAS_CLEAR:
                        CanvasCoordinates.Clear();
                    View.Dispatcher.Invoke(() =>
                    {
                        View.Canvas.Children.Clear();
                    });
                        
                    break;
                case Command.CANVAS_UNDO:
                    View.Dispatcher.Invoke(() =>
                    {
                        if (previousCanvas != null)
                        {
                            int i = View.Canvas.Children.IndexOf(previousCanvas);
                            if (i > 0)
                            {
                                View.Canvas.Children.RemoveAt(i);
                                previousCanvas = (Canvas) View.Canvas.Children[i - 1];
                            }
                            else if (i == 0)
                            {
                                View.Canvas.Children.Clear();
                                previousCanvas = null;
                            }
                        }

                    });
                    break;
            }
        }

        private void ParseCoordinates(Packet core)
        {


            XElement coordinates = core.DataNode.Element("coordinates");

            foreach (XElement coordinate in coordinates.Elements())
            {
                int x, y;
                x = Convert.ToInt32(coordinate.Element("x").Value);
                y = Convert.ToInt32(coordinate.Element("y").Value);

                CanvasCoordinates.Add(new CustomPoint(x,y));
            }

            View.Dispatcher.Invoke(() =>
            {
                DrawLines();
            });

            CanvasCoordinates.Clear();
            
        }

        private void DrawLines()
        {
            Canvas obj = new Canvas();
            for (int i = 0; i < CanvasCoordinates.Count - 3; i += 3)
            {

                Line line = new Line();
                line.Stroke = Brushes.Red;
                line.X1 = CanvasCoordinates[i].X;
                line.X2 = CanvasCoordinates[i + 3].X;
                line.Y1 = CanvasCoordinates[i].Y;
                line.Y2 = CanvasCoordinates[i + 3].Y;
                line.StrokeThickness = 2;
                obj.Children.Add(line);
                previousCanvas = obj;
            }

            View.Canvas.Children.Add(obj);
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
            View.Canvas.MouseLeave += CanvasOnMouseLeave;
            View.ButtonClear.Click += ButtonClearOnClick;
            View.ButtonUndo.Click += ButtonUndoOnClick;
        }

        private void CanvasOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
           CanvasOnMouseUp(null, null);
        }

        private void ButtonUndoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Packet core = new Packet();
            core.AddCommand(Command.CANVAS_UNDO);
            core.AddData("nick_name", Model.NickName);
            Connection.Send(core);
        }

        private void ButtonClearOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Packet core = new Packet();
            core.AddCommand(Command.CANVAS_CLEAR);

            core.AddData("nick_name", Model.NickName);
            Connection.Send(core);
        }

        private void CanvasOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
           
            Connection.Send(CreatePacketFromCoordinates());

            CanvasCoordinates.Clear();
        }

        private Packet CreatePacketFromCoordinates()
        {
            Packet core = new Packet();
            core.AddCommand(Command.SEND_COORDINATES);
            XElement coordinatesNode = core.AddElement("data", "coordinates", string.Empty);
            foreach (CustomPoint coordinate in CanvasCoordinates)
            {    
                XElement coordinateNode = new XElement("coordinate",
                    new XElement("x", coordinate.X),
                    new XElement("y", coordinate.Y));
                coordinatesNode.Add(coordinateNode);
            }

            return core;
        }

        private void CanvasOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && clicked)
            {
                // MessageBox.Show(string.Format("Move. Mouse position x: {0} y: {1}", e.GetPosition(this).X, e.GetPosition(this).Y));
                CanvasCoordinates.Add(new CustomPoint(Convert.ToInt32(e.GetPosition(View.Canvas).X), Convert.ToInt32(e.GetPosition(View.Canvas).Y)));

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
