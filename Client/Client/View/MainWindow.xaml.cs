using System;
using System.Windows;
using Client.Controller;

namespace Client.View
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
                new MainController(this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK);
            }
           
        }
    }
}
