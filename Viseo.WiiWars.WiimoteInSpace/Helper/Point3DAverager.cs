using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Viseo.WiiWars.WiimoteInSpace.Helper
{
    public class Point3DAverager
    {
        Point3D _point;
        DoubleAverager _x, _y, _z;

        public Point3DAverager(int depth)
        {
            _x = new DoubleAverager(depth);
            _y = new DoubleAverager(depth);
            _z = new DoubleAverager(depth);
        }

        public Point3D Update(double x, double y, double z)
        {
            _point.X = _x.Update(x);
            _point.Y = _y.Update(y);
            _point.Z = _z.Update(z);

            return _point;
        }

    }
}
