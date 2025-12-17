using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mikulásbácsihozta_PDD.Models
{
    internal class Ellenorzo
    {
        public static bool SzamEllenorzo(string bemenet)
        {
            int szam = 0;
            bool eredmeny = int.TryParse(bemenet, out szam);
            return eredmeny;
        }

        public static bool IdoEllenorzo(string bemenet)
        {
            double ido = 0;
            bool eredmeny = double.TryParse(bemenet, out ido);
            return eredmeny;
        }
    }
}
