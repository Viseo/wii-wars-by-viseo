using System.Collections.Generic;
using System.Linq;

namespace Viseo.WiiWars.WiimoteInSpace.Helper
{
    public class DoubleAverager
    {
        private int _depth;
        private Queue<double> _queue;

        public DoubleAverager(int depth)
        {
            _depth = depth;
            _queue = new Queue<double>(depth);
        }

        public double Update(double item)
        {
            _queue.Enqueue(item);
            if (_queue.Count > _depth)
                _queue.Dequeue();

            return _queue.Average(q => q);
        }
    }
}
