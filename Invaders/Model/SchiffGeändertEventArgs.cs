using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders.Model
{
    class SchiffGeändertEventArgs
    {
        public Schiff SchiffAktualisiert { get; private set; }
        public bool Tod { get; private set; }
        public SchiffGeändertEventArgs(Schiff schiff, bool tod)
        {
            SchiffAktualisiert = schiff;
            Tod = tod;
        }
    }
}
