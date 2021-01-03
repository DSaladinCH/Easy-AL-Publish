using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyALPublish.Extension
{
    [DebuggerDisplay("{Publisher} - {Name} - Dependencies: {Dependencies.Count}")]
    public class BCExtension : INotifyPropertyChanged, ICloneable
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

        private string publisher;

        public string Publisher
        {
            get { return publisher; }
            set
            {
                publisher = value;
                NotifyPropertyChanged();
            }
        }

        private string currVersion;

        public string CurrVersion
        {
            get { return currVersion; }
            set
            {
                currVersion = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ShowVersions));
            }
        }

        private string newVersion;

        public string NewVersion
        {
            get { return newVersion; }
            set
            {
                newVersion = value;
                NotifyPropertyChanged();
            }
        }

        public Visibility ShowVersions
        {
            get
            {
                if (currVersion == "")
                    return Visibility.Hidden;
                return Visibility.Visible;
            }
        }

        public ExtensionStatus Status { get; set; }

        private ObservableCollection<BCExtension> dependencies = new ObservableCollection<BCExtension>();

        public ObservableCollection<BCExtension> Dependencies
        {
            get { return dependencies; }
            set
            {
                dependencies = value;
                NotifyPropertyChanged();
            }
        }

        public BCExtension()
        {
        }

        public BCExtension(string name, string publisher, string currVersion, string newVersion)
        {
            Name = name;
            Publisher = publisher;
            CurrVersion = currVersion;
            NewVersion = newVersion;
            Dependencies = new ObservableCollection<BCExtension>();
        }

        public BCExtension(string name, string publisher, string currVersion, string newVersion, ObservableCollection<BCExtension> dependencies)
        {
            Name = name;
            Publisher = publisher;
            CurrVersion = currVersion;
            NewVersion = newVersion;
            Dependencies = dependencies;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public object Clone()
        {
            //return JsonSerializer.Deserialize<BCExtension>(JsonSerializer.Serialize(this));
            return new BCExtension()
            {
                Name = this.Name,
                Publisher = this.Publisher,
                CurrVersion = this.CurrVersion,
                NewVersion = this.NewVersion,
                Dependencies = this.Dependencies
            };
        }
    }
}
