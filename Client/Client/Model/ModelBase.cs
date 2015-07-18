using System.ComponentModel;

namespace Client.Model
{
    public abstract class ModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, eventArgs);
        }
    }
}
