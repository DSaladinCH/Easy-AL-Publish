using EasyALPublish.Extension;
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
    [DebuggerDisplay("{Name} - Instance: {InstanceName} - Extensions: {Extensions.Count}")]
    public class PublishConfig : INotifyPropertyChanged, ICloneable
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

        private BCVersion version;
        public BCVersion Version
        {
            get { return version; }
            set
            {
                version = value;
                NotifyPropertyChanged();
            }
        }

        private string instanceName;
        public string InstanceName
        {
            get { return instanceName; }
            set
            {
                instanceName = value;
                NotifyPropertyChanged();
            }
        }

        private string extensionsPath;
        public string ExtensionsPath
        {
            get { return extensionsPath; }
            set
            {
                extensionsPath = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<BCExtension> extensions = new ObservableCollection<BCExtension>();

        public ObservableCollection<BCExtension> Extensions
        {
            get { return extensions; }
            set
            {
                extensions = value;
                NotifyPropertyChanged();
            }
        }

        public PublishConfig()
        {
        }
        public PublishConfig(string name, BCVersion version, string instanceName, string extensionsPath)
        {
            Name = name;
            Extensions = new ObservableCollection<BCExtension>();
            Version = version;
            InstanceName = instanceName;
            ExtensionsPath = extensionsPath;
        }

        public PublishConfig(string name, BCVersion version, string instanceName, string extensionsPath, ObservableCollection<BCExtension> extensions)
        {
            Name = name;
            Extensions = extensions;
            Version = version;
            InstanceName = instanceName;
            ExtensionsPath = extensionsPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public object Clone()
        {
            //return JsonSerializer.Deserialize<BCExtension>(JsonSerializer.Serialize(this));
            return new PublishConfig()
            {
                Name = this.Name,
                Version = this.Version,
                InstanceName = this.InstanceName,
                ExtensionsPath = this.ExtensionsPath,
                Extensions = this.Extensions
            };
        }
    }
}
