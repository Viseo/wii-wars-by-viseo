using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Viseo.WiiWars.WiimoteInSpace.ViewModel
{
    public class DesignMainWindowViewModel : IMainWindowViewModel
    {
        public Model3D IRBeacon { get; set; }
        public Model3D Model { get; set; }
        public double RotX { get; set; }
        public double RotY { get; set; }
        public double RotZ { get; set; }
        public double TranslateX { get; set; }
        public double TranslateY { get; set; }
        public double TranslateZ { get; set; }
    }
}
