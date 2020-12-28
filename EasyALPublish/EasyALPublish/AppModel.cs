using EasyALPublish.Extension;
using EasyALPublish.Misc;
using EasyALPublish.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyALPublish
{
    public class AppModel : INotifyPropertyChanged
    {
        private static AppModel instance;

        public static AppModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new AppModel();
                return instance;
            }
            set { instance = value; }
        }

        private List<BCVersion> bcVersions = new List<BCVersion>()
        {
            new BCVersion("BC V13", "130"),
            new BCVersion("BC V14", "140"),
            new BCVersion("BC V15", "150"),
            new BCVersion("BC V16", "160"),
            new BCVersion("BC V17", "170"),
            new BCVersion("BC V18", "180")
        };

        public List<BCVersion> BCVersions
        {
            get { return bcVersions; }
            set { bcVersions = value; }
        }

        private ObservableCollection<Company> companies = new ObservableCollection<Company>();

        public ObservableCollection<Company> Companies
        {
            get { return companies; }
            set
            {
                companies = value;
                NotifyPropertyChanged();
            }
        }

        private Company currCompany;

        public Company CurrCompany
        {
            get { return currCompany; }
            set
            {
                currCompany = value;
                NotifyPropertyChanged();
            }
        }

        private PublishConfig currConfig;

        public PublishConfig CurrConfig
        {
            get { return currConfig; }
            set
            {
                currConfig = value;
                NotifyPropertyChanged();
            }
        }

        private ExtensionMgt extensionMgt = new ExtensionMgt();

        public ExtensionMgt ExtensionMgt
        {
            get { return extensionMgt; }
            set
            {
                extensionMgt = value;
                NotifyPropertyChanged();
            }
        }

        public IPersistenceDataManagement PeristenceMgt { get; } = new PersistenceJsonManagement();

        public AppModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            PersistentData data = PeristenceMgt.Load();
            Companies = new ObservableCollection<Company>(data.Companies);
        }

        public void SaveData()
        {
            PersistentData data = new PersistentData();
            data.Companies = Companies.ToList();
            PeristenceMgt.Save(data);
        }

        #region Notify Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
