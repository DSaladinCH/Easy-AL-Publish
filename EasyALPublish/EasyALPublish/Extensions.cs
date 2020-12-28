using EasyALPublish.Extension;
using EasyALPublish.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish
{
    //[DebuggerDisplay("")]
    public static class Extensions
    {
        public static bool Create(this ObservableCollection<Company> companies, Company value)
        {
            if (companies.FirstOrDefault(c => c.Name == value.Name) != null)
                return false;

            companies.Add(value);
            return true;
        }

        public static bool CreateConfig(this Company company, PublishConfig value)
        {
            if (company.Configs.FirstOrDefault(c => c.Name == value.Name) != null)
                return false;

            company.Configs.Add(value);
            return true;
        }

        public static bool Update(this ObservableCollection<PublishConfig> list, PublishConfig oldValue, PublishConfig newValue)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == oldValue)
                {
                    list[i] = newValue;
                    return true;
                }
            }

            return false;
        }

        public static bool CreateExtension(this PublishConfig config, ObservableCollection<BCExtension> parentList, BCExtension value)
        {
            if (ExtensionExists(config.Extensions, value))
                return false;

            parentList.Add(value);
            return true;
        }

        private static bool ExtensionExists(ObservableCollection<BCExtension> parentList, BCExtension value)
        {
            for (int i = 0; i < parentList.Count; i++)
            {
                if (parentList[i] == value)
                    return true;

                if (ExtensionExists(parentList[i].Dependencies, value))
                    return true;
            }

            return false;
        }

        public static bool Update(this ObservableCollection<BCExtension> list, BCExtension oldValue, BCExtension newValue)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == oldValue)
                {
                    list[i] = newValue;
                    return true;
                }
                if (list[i].Dependencies.Update(oldValue, newValue))
                    return true;
            }

            return false;
        }

        public static bool Delete(this ObservableCollection<BCExtension> list, BCExtension value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    list.RemoveAt(i);
                    return true;
                }
                if (list[i].Dependencies.Delete(value))
                    return true;
            }

            return false;
        }
    }
}
