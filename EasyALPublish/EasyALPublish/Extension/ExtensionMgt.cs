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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
            extensions.RunForEach(e => e.NewVersion = GetAppNewVersion(AppModel.Instance.CurrConfig.ExtensionsPath, e));
        }

        public void Uninstall(ObservableCollection<BCExtension> extensions, Window window = null, ProgressBar progressBar = null)
        {
            extensions.RunForEach(e =>
            {
                if (e.Status == ExtensionStatus.Installed)
                {
                    if (Uninstall(AppModel.Instance.CurrConfig.InstanceName, e))
                    {
                        e.Status = ExtensionStatus.Published;
                        if (progressBar != null)
                            window.Dispatcher.Invoke(() => progressBar.Value += 1);
                    }
                }
            });
        }

        public void Unpublish(ObservableCollection<BCExtension> extensions, Window window = null, ProgressBar progressBar = null)
        {
            extensions.RunForEach(e =>
            {
                if (e.Status == ExtensionStatus.Published)
                {
                    if (Unpublish(AppModel.Instance.CurrConfig.InstanceName, e))
                    {
                        e.Status = ExtensionStatus.None;
                        if (progressBar != null)
                            window.Dispatcher.Invoke(() => progressBar.Value += 1);
                    }
                }
            });
        }

        public void PublishAndInstall(ObservableCollection<BCExtension> extensions, Window window = null, ProgressBar progressBar = null)
        {
            extensions.RunForEach(e =>
            {
                if (Publish(AppModel.Instance.CurrConfig, e))
                {
                    e.Status = ExtensionStatus.Published;
                    if (progressBar != null)
                        window.Dispatcher.Invoke(() => progressBar.Value += 1);
                }

                if (Install(AppModel.Instance.CurrConfig, e))
                {
                    e.Status = ExtensionStatus.Installed;
                    if (progressBar != null)
                        window.Dispatcher.Invoke(() => progressBar.Value += 1);
                }
            });
        }

        public void ResetStatus(ObservableCollection<BCExtension> extensions)
        {
            extensions.RunForEach(e => e.Status = ExtensionStatus.None);
        }

        private async Task<BCExtension> GetAppCurrVersion(string instanceName, BCExtension extension)
        {
            NAVAppInfo appInfo = Commands.GetNAVAppInfo(instanceName, extension.Name);
            if (appInfo == null)
            {
                extension.CurrVersion = "";
                extension.Status = ExtensionStatus.None;
                return extension;
            }

            extension.CurrVersion = appInfo.Version;
            extension.Status = (ExtensionStatus)appInfo.Status;
            return extension;
        }

        private string GetAppNewVersion(string extensionsPath, BCExtension extension)
        {
            List<Version> versions = new List<Version>();
            List<string> versionsStr = new List<string>();
            string startFileName = string.Format("{0}_{1}_*", extension.Publisher, extension.Name);
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

        private bool Uninstall(string instanceName, BCExtension extension)
        {
            return Commands.UninstallExtension(instanceName, extension.Name, extension.CurrVersion);
        }

        private bool Unpublish(string instanceName, BCExtension extension)
        {
            return Commands.UnpublishExtension(instanceName, extension.Name, extension.CurrVersion);
        }

        private bool Publish(PublishConfig config, BCExtension extension)
        {
            if (!Commands.Publish(config.InstanceName, config.GetExtensionPath(extension)))
                return false;

            if (!Commands.Sync(config.InstanceName, extension.Name, extension.NewVersion))
                return false;

            if (extension.CurrVersion == "" || new Version(extension.NewVersion) > new Version(extension.CurrVersion))
                if (!Commands.DataUpgrade(config.InstanceName, extension.Name, extension.NewVersion))
                    return false;

            return true;
        }

        private bool Install(PublishConfig config, BCExtension extension)
        {
            if (!Commands.Install(config.InstanceName, extension.Name, extension.NewVersion))
                return false;

            return true;
        }
    }
}
