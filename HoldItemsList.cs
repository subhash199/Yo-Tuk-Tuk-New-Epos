using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yo_Tuk_Tuk_Epos
{
    public class HoldItemsList
    {
        public int qty;
        public string name;

        public HoldItemsList( int pQty, string pName )
        {
            qty = pQty;
            name = pName;
        }
    }
}
