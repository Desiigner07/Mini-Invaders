using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Invaders.Model
{
    class SternGeändertEventArgs : EventArgs
    {
        public Point Point { get; private set; }
        public bool Verschwunden { get; private set; }

        public SternGeändertEventArgs(Point point, bool verschwunden)
        {
            Point = point;
            Verschwunden = verschwunden;
        }
    }
}
