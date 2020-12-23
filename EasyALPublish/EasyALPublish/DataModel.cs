using EasyALPublish.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish
{
    public class DataModel : INotifyPropertyChanged
    {
        private static DataModel instance;

        public static DataModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataModel();
                return instance;
            }
            set { instance = value; }
        }

        private ObservableCollection<Company> companies = new ObservableCollection<Company>(new List<Company>()
        {
            new Company("ELFO", new List<PublishConfig>()
            {
                new PublishConfig("Test", new ObservableCollection<Extension>(){
                    new Extension("base", "", "", "", new ObservableCollection<Extension>()
                    {
                        new Extension("sub 1", "", "", "", null),
                        new Extension("sub 2", "", "", "", null)
                    })
                }),
                new PublishConfig("Prod", new ObservableCollection<Extension>(){
                    new Extension("base", "", "", "", new ObservableCollection<Extension>()
                    {
                        new Extension("sub 1", "", "", "", null),
                        new Extension("sub 2", "", "", "", null)
                    })
                })
            }),
            new Company("Tsiag", new List<PublishConfig>()
            {
                new PublishConfig("Test", new ObservableCollection<Extension>(){
                    new Extension("base", "", "", "", new ObservableCollection<Extension>()
                    {
                        new Extension("sub 1", "", "", "", null),
                        new Extension("sub 2", "", "", "", null)
                    }),
                    new Extension("something", "", "", "", new ObservableCollection<Extension>()
                    {
                        new Extension("idk", "", "", "", null),
                        new Extension("anything", "", "", "", null)
                    })
                }),
                new PublishConfig("Qual", new ObservableCollection<Extension>(){
                    new Extension("base", "", "", "", new ObservableCollection<Extension>()
                    {
                        new Extension("sub 1", "", "", "", null),
                        new Extension("sub 2", "", "", "", null)
                    })
                }),
                new PublishConfig("Prod", new ObservableCollection<Extension>(){
                    new Extension("base", "", "", "", new ObservableCollection<Extension>()
                    {
                        new Extension("sub 1", "", "", "", null),
                        new Extension("sub 2", "", "", "", null)
                    })
                })
            })
        });

        public ObservableCollection<Company> Companies
        {
            get { return companies; }
            set { companies = value; }
        }

        private Company currCompany;

        public Company CurrCompany
        {
            get { return currCompany; }
            set
            {
                currCompany = value;
                NotifyPropertyChanged(nameof(CurrCompany));
            }
        }

        private PublishConfig currConfig;

        public PublishConfig CurrConfig
        {
            get { return currConfig; }
            set
            {
                currConfig = value;
                NotifyPropertyChanged(nameof(CurrConfig));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
