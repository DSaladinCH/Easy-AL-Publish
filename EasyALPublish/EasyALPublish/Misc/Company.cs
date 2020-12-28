using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Misc
{
    [DebuggerDisplay("{Name} - Configs: {Configs.Count}")]
    public class Company : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<PublishConfig> configs;

        public ObservableCollection<PublishConfig> Configs
        {
            get { return configs; }
            set
            {
                configs = value;
                NotifyPropertyChanged();
            }
        }


        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
            Configs = new ObservableCollection<PublishConfig>();
        }

        public Company(string name, ObservableCollection<PublishConfig> configs)
        {
            Name = name;
            Configs = configs;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
