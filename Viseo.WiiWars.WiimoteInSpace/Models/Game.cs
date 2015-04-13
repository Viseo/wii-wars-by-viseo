using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viseo.WiiWars.WiimoteInSpace.Models
{
    public class Game : NotifierBase
    {
        private List<Saber> _sabers;

        public List<Saber> Sabers
        {
            get { return _sabers; }
            set
            {
                _sabers = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }

        private bool _isGameStarted;

        public bool IsGameStarted
        {
            get { return _isGameStarted; }
            set
            {
                _isGameStarted = value;
                OnPropertyChanged();
            }

        }


    }
}
