using System.Windows;
using Server.Controller;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainController main = new MainController();
            main.Init(this);
        }

       
    }
}
