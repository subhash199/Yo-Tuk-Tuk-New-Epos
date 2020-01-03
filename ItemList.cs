using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yo_Tuk_Tuk_Epos
{
   public class ItemList
    {
        public int qty;
        public string name;
        public decimal value;
        public string identifier;

        public ItemList(int pQty, string pName, decimal pValue, string id)
        {
            qty = pQty;
            name = pName;
            value = pValue;
            identifier = id;

        }
    }
}
