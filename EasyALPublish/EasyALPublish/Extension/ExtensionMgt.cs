using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyALPublish.Extension
{
    public class ExtensionMgt
    {
        public async Task UpdateCurrVersions(ObservableCollection<BCExtension> extensions)
        {
            foreach (BCExtension item in extensions)
            {
                if (item.Dependencies.Count > 0)
                    UpdateCurrVersions(item.Dependencies);
                item.CurrVersion = await GetAppCurrVersion(AppModel.Instance.CurrConfig.InstanceName, item.Name);
            }
        }

        private async Task<string> GetAppCurrVersion(string instanceName, string appName)
        {
            //string appInfo = PowerShell.GetNavAppInfo(instanceName, appName);
            string appInfo = File.ReadAllText(@"C:\Users\domin\Desktop\GetNavAppInfoTemp.txt");
            List<Match> matches = Regex.Matches(appInfo, @"([A-Za-z0-9 ]+)\:(.+)").ToList();
            Match version = matches.FirstOrDefault(m => m.Groups[1].ToString().Trim() == "Version");
            if (version == null)
                return "";
            return version.Groups[2].ToString().Trim();
        }
    }
}
