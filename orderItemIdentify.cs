﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yo_Tuk_Tuk_Epos
{
   public class orderItemIdentify
    {
        public string section;
        public string name;

        public orderItemIdentify(string pSection, string pName)
        {
            section = pSection;
            name = pName;
        }
    }
}
