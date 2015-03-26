using System.Windows.Media.Media3D;

namespace Viseo.WiiWars.WiimoteInSpace.ViewModel
{
    public interface IMainWindowViewModel
    {
        Model3D IRBeacon { get; set; }
        Model3D Model { get; set; }
        double RotX { get; set; }
        double RotY { get; set; }
        double RotZ { get; set; }
        double TranslateX { get; set; }
        double TranslateY { get; set; }
        double TranslateZ { get; set; }
    }
}