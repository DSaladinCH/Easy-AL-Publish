using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyALPublish.Misc
{
    public class Extension : INotifyPropertyChanged
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

        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
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

        private ObservableCollection<Extension> dependencies = new ObservableCollection<Extension>();

        public ObservableCollection<Extension> Dependencies
        {
            get { return dependencies; }
            set
            {
                dependencies = value;
                NotifyPropertyChanged();
            }
        }

        public Extension(string name, string path, string currVersion, string newVersion)
        {
            Name = name;
            Path = path;
            CurrVersion = currVersion;
            NewVersion = newVersion;
            Dependencies = new ObservableCollection<Extension>();
        }

        public Extension(string name, string path, string currVersion, string newVersion, ObservableCollection<Extension> dependencies)
        {
            Name = name;
            Path = path;
            CurrVersion = currVersion;
            NewVersion = newVersion;
            Dependencies = dependencies;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
