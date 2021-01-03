using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Misc
{
    //[DebuggerDisplay("")]
    public class Theme
    {
        public string Name { get; set; }
        public string Identifier { get; set; }

        public Theme()
        {
        }

        public Theme(string name, string identifier)
        {
            Name = name;
            Identifier = identifier;
        }
    }
}
