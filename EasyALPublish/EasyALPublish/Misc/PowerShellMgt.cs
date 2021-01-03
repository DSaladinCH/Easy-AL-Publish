using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using EasyALPublish.Extension;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Diagnostics;

namespace EasyALPublish.Misc
{
    public static class PowerShellMgt
    {
        private static PowerShell ps { get; set; }

        public static bool StartPS(BCVersion version)
        {
            DSaladin.DynamicsBC.Commands.Init(version.FolderVersion);
            return true;
            using (PowerShellProcessInstance pspi = new PowerShellProcessInstance())
            {
                //string psfn = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                string psfn = pspi.Process.StartInfo.FileName;
                psfn = psfn.ToLowerInvariant().Replace("\\syswow64\\", "\\sysnative\\");
                pspi.Process.StartInfo.FileName = psfn;
                Runspace r = RunspaceFactory.CreateOutOfProcessRunspace(null, pspi);
                //Runspace r = RunspaceFactory.CreateRunspace();
                r.Open();
                ps = PowerShell.Create();
                ps.Runspace = r;
                ps.AddScript("[Environment]::Is64BitProcess");
                foreach (var item in ps.Invoke())
                {
                    Debug.WriteLine("Result: " + item.ToString());
                }

                ps.AddScript("Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted");
                foreach (var item in ps.Invoke())
                {
                    Debug.WriteLine("Result: " + item.ToString());
                }
                ps.Commands.Clear();
                ps.AddCommand("Import-Module");
                ps.AddArgument(string.Format(@"C:\Program Files\Microsoft Dynamics 365 Business Central\{0}\Service\NavAdminTool.ps1", version.FolderVersion));
                ps.AddParameter("WarningAction", "SilentlyContinue");
                ps.Invoke();
                if (ps.HadErrors)
                {
                    foreach (var item in ps.Streams.Error)
                    {
                        Debug.WriteLine("Error: " + item.Exception);
                    }
                }
                ps.Commands.Clear();
                return true;
            }
            //ps.Invoke();
        }

        public static void StopPS()
        {
            ps = null;
        }

        public static string GetNavAppInfo(string instanceName, string appName)
        {
            if (ps == null)
                return null;

            ps.AddScript(string.Format("Get-NAVAppInfo -ServerInstance {0} -Name {1} | Out-String", instanceName, appName));
            List<PSObject> results = ps.Invoke().ToList();
            if (ps.HadErrors)
            {
                PSDataCollection<ErrorRecord> errors = ps.Streams.Error;
                foreach (var item in errors)
                {
                    Debug.WriteLine(item.Exception);
                }
            }
            if (results == null || results.Count == 0)
                return null;
            return results[0].ToString();
        }

        public static string UninstallExtension(string instanceName, string appName, string version)
        {
            if (ps == null)
                return null;

            ps.AddCommand("Uninstall-NAVApp");
            ps.AddParameter("ServerInstance", instanceName);
            ps.AddParameter("Name", appName);
            ps.AddParameter("Version", version);
            ps.AddParameter("Force");

            List<PSObject> results = ps.Invoke().ToList();
            if (results == null || results.Count == 0)
                return null;
            return results[0].ToString();
        }

        public static string UnpublishExtension(string instanceName, string appName, string version)
        {
            if (ps == null)
                return null;

            ps.AddCommand("Unpublish-NAVApp");
            ps.AddParameter("ServerInstance", instanceName);
            ps.AddParameter("Name", appName);
            ps.AddParameter("Version", version);

            List<PSObject> results = ps.Invoke().ToList();
            if (results == null || results.Count == 0)
                return null;
            return results[0].ToString();
        }
    }
}
