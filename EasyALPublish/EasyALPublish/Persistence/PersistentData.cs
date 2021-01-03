using EasyALPublish.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Persistence
{
    public class PersistentData
    {
        public AppOptions AppOptions { get; set; } = new AppOptions();
        public List<Company> Companies { get; set; } = new List<Company>();
    }
}
