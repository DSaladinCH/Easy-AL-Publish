using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Misc
{
    public class Company : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public List<PublishConfig> Configs { get; set; } = new List<PublishConfig>();

        public Company(string name, List<PublishConfig> configs)
        {
            Name = name;
            Configs = configs;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
