using System;
using System.Windows;
using Server.Controller;

namespace Server.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                MainController main = new MainController();
                main.Init(this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK);
            }
        }

       
    }
}
