using System.ComponentModel;

namespace Server.Model
{
    public class LogMessage : ModelBase
    {
        private string _message;

        public LogMessage(string msg)
        {
            _message = msg;
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Message"));
            }
        }
    }
}