using DSaladin.DynamicsBC;
using EasyALPublish.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyALPublish.Extension
{
    public class ExtensionMgt
    {
        public void UpdateCurrVersions(ObservableCollection<BCExtension> extensions)
        {
            extensions.RunForEach(async e => e = await GetAppCurrVersion(AppModel.Instance.CurrConfig.InstanceName, e));
        }

        public void UpdateNewVersions(ObservableCollection<BCExtension> extensions)
        {
            extensions.RunForEach(e => e.NewVersion = GetAppNewVersion(AppModel.Instance.CurrConfig.ExtensionsPath, e.Name, e.Publisher));
        }

        public void UninstallExtensions(ObservableCollection<BCExtension> extensions)
        {
            extensions.RunForEach(e => Uninstall(AppModel.Instance.CurrConfig.InstanceName, e.Name, e.CurrVersion));
        }

        public void UnpublishExtensions(ObservableCollection<BCExtension> extensions)
        {
            extensions.RunForEach(e => Unpublish(AppModel.Instance.CurrConfig.InstanceName, e.Name, e.CurrVersion));
        }

        public void PublishAndInstallExtensions(ObservableCollection<BCExtension> extensions)
        {
            extensions.RunForEach(e => PublishAndInstallNew(AppModel.Instance.CurrConfig, e));
        }

        private async Task<BCExtension> GetAppCurrVersion(string instanceName, BCExtension extension)
        {
            NAVAppInfo appInfo = Commands.GetNAVAppInfo(instanceName, extension.Name);
            extension.CurrVersion = appInfo.Version;
            extension.Status = (ExtensionStatus)appInfo.Status;
            return extension;
        }

        private string GetAppNewVersion(string extensionsPath, string appName, string publisher)
        {
            List<Version> versions = new List<Version>();
            List<string> versionsStr = new List<string>();
            string startFileName = string.Format("{0}_{1}_*", publisher, appName);
            List<string> files = Directory.GetFiles(extensionsPath, startFileName).ToList();
            foreach (string file in files)
            {
                Match match = Regex.Match(Path.GetFileName(file), @"(.+[^_])_(.+[^_])_([0-9\.]+).app");
                if (match == null || match.Groups.Count != 4)
                    continue;
                versions.Add(new Version(match.Groups[3].Value));
                versionsStr.Add(match.Groups[3].Value);
            }

            versions.Sort();

            if (versions.Count == 0)
                return "";

            return versionsStr.First(v => new Version(v) == versions.Last());
        }

        private bool Uninstall(string instanceName, string appName, string version)
        {
            return Commands.UninstallExtension(instanceName, appName, version);
        }

        private bool Unpublish(string instanceName, string appName, string version)
        {
            return Commands.UnpublishExtension(instanceName, appName, version);
        }

        private bool PublishAndInstallNew(PublishConfig config, BCExtension extension)
        {
            if (!Commands.UninstallExtension(config.InstanceName, extension.Name, extension.CurrVersion))
                return false;

            if (!Commands.UnpublishExtension(config.InstanceName, extension.Name, extension.CurrVersion))
                return false;

            if (!Commands.Publish(config.InstanceName, config.GetExtensionPath(extension)))
                return false;

            if (!Commands.Sync(config.InstanceName, extension.Name, extension.NewVersion))
                return false;

            if (new Version(extension.NewVersion) > new Version(extension.CurrVersion))
                if (!Commands.DataUpgrade(config.InstanceName, extension.Name, extension.NewVersion))
                    return false;

            if (!Commands.Install(config.InstanceName, extension.Name, extension.NewVersion))
                return false;

            return true;
        }
    }
}
