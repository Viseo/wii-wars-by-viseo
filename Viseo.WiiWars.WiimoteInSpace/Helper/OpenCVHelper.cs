using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using WiimoteLib;

namespace Viseo.WiiWars.WiimoteInSpace.Helper
{
	public static class OpenCVHelper
	{
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Point3f is part of Opencv library.")]
        public static Point3f ToPoint3f(this Point3D value)
		{
			return new Point3f(Convert.ToSingle(value.X), Convert.ToSingle(value.Y), Convert.ToSingle(value.Z));
		}

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Point3f is part of Opencv library.")]
        public static IEnumerable<Point3f> ToPoint3f(this IEnumerable<Point3D> values)
		{
			return values.Select(v => v.ToPoint3f());
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Point2f is part of Opencv library.")]
        public static Point2f ToPoint2f(this WiimoteLib.Point value)
		{
			return new Point2f(value.X, value.Y);
		}

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Point2f is part of Opencv library.")]
        public static IEnumerable<Point2f> ToPoint2f(this IEnumerable<WiimoteLib.Point> values)
		{
			return values.Select(v => v.ToPoint2f());
		}

        public static Matrix3D ToMatrix3D(this Mat value)
        {
            var mi = new MatOfDouble(value).GetIndexer();

            //return new Matrix3D(mi[0, 0], mi[0, 1], mi[0, 2], mi[0, 3],
            //                    mi[1, 0], mi[1, 1], mi[1, 2], mi[1, 3],
            //                    mi[2, 0], mi[2, 1], mi[2, 2], mi[2, 3],
            //                    mi[3, 0], mi[3, 1], mi[3, 2], mi[3, 3]);

            return new Matrix3D(mi[0, 0], mi[1, 0], mi[2, 0], mi[3, 0],
                                mi[0, 1], mi[1, 1], mi[2, 1], mi[3, 1],
                                mi[0, 2], mi[1, 2], mi[2, 2], mi[3, 2],
                                mi[0, 3], mi[1, 3], mi[2, 3], mi[3, 3]);
        }
    }
}
