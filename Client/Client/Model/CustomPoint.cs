using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class CustomPoint : ModelBase
    {
        private int _x, _y;
        public CustomPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("X"));
                }
            }
        }

        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y;
        }

        public int Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Y"));
                }
            }
        }

    }
}
