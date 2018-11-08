using System.ComponentModel;
using System.Net.NetworkInformation;

namespace Switch.Model
{
    class Cam_zaznam : INotifyPropertyChanged
    {
        private int timerko;
        private int rozhranicko;
        public PhysicalAddress MAC { get; set; }
        public int rozhranie { get { return rozhranicko; } set{ rozhranicko = value; NotifyPropertyChanged("rozhranie"); } }
        public int timer { get {return timerko; } set { timerko = value; NotifyPropertyChanged("timer");} }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
        public Cam_zaznam(PhysicalAddress MAC, int rozhranie, int timer)
        {
            this.MAC = MAC;
            this.rozhranie = rozhranie;
            this.timer = timer;

        }        
    }
}
