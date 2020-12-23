using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Misc
{
    public class PublishConfig : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string InstanceName { get; set; }

        private ObservableCollection<Extension> extensions = new ObservableCollection<Extension>();

        public ObservableCollection<Extension> Extensions
        {
            get { return extensions; }
            set { extensions = value; }
        }

        public PublishConfig(string name, ObservableCollection<Extension> extensions)
        {
            Name = name;
            Extensions = extensions;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
