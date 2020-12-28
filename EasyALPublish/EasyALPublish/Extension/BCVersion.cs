using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Extension
{
    [DebuggerDisplay("{Name}")]
    public class BCVersion
    {
        public string Name { get; set; }
        public string FolderVersion { get; set; }

        public BCVersion()
        {
        }

        public BCVersion(string name, string folderVersion)
        {
            Name = name;
            FolderVersion = folderVersion;
        }
    }
}
