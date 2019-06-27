using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Invaders.Model
{
    abstract class Schiff
    {
        public Point Ort { get; protected set; }
        public Size Größe { get; private set; }
        public Rect Fläche { get { return new Windows.Foundation.Rect(Ort, Größe); }}
        public Schiff(Point ort, Size größe)
        {
            Ort = ort;
            Größe = größe;
        }


        public abstract void Bewegen(Richtung richtung);
    }
}
