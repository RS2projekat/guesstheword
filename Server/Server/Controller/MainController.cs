using System.Windows;
using System.Windows.Input;

namespace Server.Controller
{
    public class MainController
    {
        public MainWindow View { get; set; }

        private void AttachEvents()
        {
            View.TextBoxPort.PreviewTextInput += NumberValidationTextBox;
            View.ButtonServerStart.Click += ButtonServerStartOnClick;
        }

        // -----------------------------------------------------------------------------------
        private void ButtonServerStartOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            View.ButtonServerStart.Content = "Stop";
        }

        // -----------------------------------------------------------------------------------
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }
        }

        // -----------------------------------------------------------------------------------
        internal void Init(MainWindow main)
        {
            View = main;

            AttachEvents();
            DoBinding();
        }

        // -----------------------------------------------------------------------------------
        private void DoBinding()
        {
            
        }
    }
}