using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Viseo.WiiWars.WiimoteInSpace.Helper
{
    public class Matrix3DAverager
    {
        Matrix3D _matrix;
        DoubleAverager _m11, _m12, _m13, _m14,
                       _m21, _m22, _m23, _m24,
                       _m31, _m32, _m33, _m34,
                       _m41, _m42, _m43, _m44;

        public Matrix3DAverager(int depth)
        {
            _m11 = new DoubleAverager(depth);
            _m12 = new DoubleAverager(depth);
            _m13 = new DoubleAverager(depth);
            _m14 = new DoubleAverager(depth);
            _m21 = new DoubleAverager(depth);
            _m22 = new DoubleAverager(depth);
            _m23 = new DoubleAverager(depth);
            _m24 = new DoubleAverager(depth);
            _m31 = new DoubleAverager(depth);
            _m32 = new DoubleAverager(depth);
            _m33 = new DoubleAverager(depth);
            _m34 = new DoubleAverager(depth);
            _m41 = new DoubleAverager(depth);
            _m42 = new DoubleAverager(depth);
            _m43 = new DoubleAverager(depth);
            _m44 = new DoubleAverager(depth);
        }

        public Matrix3D Update(double m11, double m12, double m13, double m14,
                               double m21, double m22, double m23, double m24,
                               double m31, double m32, double m33, double m34,
                               double m41, double m42, double m43, double m44)
        {
            _matrix.M11 = _m11.Update(m11);
            _matrix.M12 = _m12.Update(m12);
            _matrix.M13 = _m13.Update(m13);
            _matrix.M14 = _m14.Update(m14);
            _matrix.M21 = _m21.Update(m21);
            _matrix.M22 = _m22.Update(m22);
            _matrix.M23 = _m23.Update(m23);
            _matrix.M24 = _m24.Update(m24);
            _matrix.M31 = _m31.Update(m31);
            _matrix.M32 = _m32.Update(m32);
            _matrix.M33 = _m33.Update(m33);
            _matrix.M34 = _m34.Update(m34);
            _matrix.OffsetX = _m41.Update(m41);
            _matrix.OffsetY = _m42.Update(m42);
            _matrix.OffsetZ = _m43.Update(m43);
            _matrix.M44 = _m44.Update(m44);
            return _matrix;
        }

        public Matrix3D Update(Matrix3D matrix)
        {
            _matrix.M11 = _m11.Update(matrix.M11);
            _matrix.M12 = _m12.Update(matrix.M12);
            _matrix.M13 = _m13.Update(matrix.M13);
            _matrix.M14 = _m14.Update(matrix.M14);
            _matrix.M21 = _m21.Update(matrix.M21);
            _matrix.M22 = _m22.Update(matrix.M22);
            _matrix.M23 = _m23.Update(matrix.M23);
            _matrix.M24 = _m24.Update(matrix.M24);
            _matrix.M31 = _m31.Update(matrix.M31);
            _matrix.M32 = _m32.Update(matrix.M32);
            _matrix.M33 = _m33.Update(matrix.M33);
            _matrix.M34 = _m34.Update(matrix.M34);
            _matrix.OffsetX = _m41.Update(matrix.OffsetX);
            _matrix.OffsetY = _m42.Update(matrix.OffsetY);
            _matrix.OffsetZ = _m43.Update(matrix.OffsetZ);
            _matrix.M44 = _m44.Update(matrix.M44);

            return _matrix;
        }
    }
}
