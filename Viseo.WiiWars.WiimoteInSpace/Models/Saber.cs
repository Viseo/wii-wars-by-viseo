namespace Viseo.WiiWars.WiimoteInSpace.Models
{
    public class Saber : NotifierBase
    {
        public enum SaberColor
        {
            Blue,
            Green,
            Red,
            Violet
        }

        public int Id { get; set; }

        private SaberColor _color;

        public SaberColor Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        private bool _isOn;

        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                _isOn = value;
                OnPropertyChanged();
            }
        }
        
    }
}
