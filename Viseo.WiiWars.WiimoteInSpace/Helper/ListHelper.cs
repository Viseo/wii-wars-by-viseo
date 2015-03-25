using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viseo.WiiWars.WiimoteInSpace.Helper
{
    public static class ListHelper
    {
        public static void RotateLeft<T>(this IList<T> list)
        {
            var value = list[0];
            list.RemoveAt(0);
            list.Insert(list.Count, value);
        }

        public static void RotateRight<T>(this IList<T> list)
        {
            var value = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            list.Insert(0, value);
        }
    }
}
