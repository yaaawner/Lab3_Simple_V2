using System.Numerics;
using System.ComponentModel;

namespace ClassLibrary
{
    abstract class V2Data : INotifyPropertyChanged
    {
        private string info = "";
        private double freq = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
            }
        }

        public string Info { 
            get { return info; }
            set 
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }

        public double Freq
        {
            get { return freq; }
            set
            {
                freq = value;
                OnPropertyChanged("Freq");
            }
        }

        public V2Data(string info, double freq)
        {
            Info = info;
            Freq = freq;
        }

        public V2Data()
        {
            Info = "info";
            Freq = 100;
        }

        public abstract Complex[] NearAverage(float eps);
        public abstract string ToLongString();
        public abstract string ToLongString(string format);

        public override string ToString()
        {
            return "Info: " + Info + " Frequency: " + Freq.ToString();
        }

    }
}
