using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yo_Tuk_Tuk_Epos
{
    class userId
    {
        private string name;
        private int id;
        public userId(string _name, int _id)
        {
            name = _name;
            id = _id;
            StreamWriter users = new StreamWriter("users.txt");
            users.Write(name + " " + id);
        }     
    }
}
