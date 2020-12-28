using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Automation;
using EasyALPublish.Extension;

namespace EasyALPublish.Misc
{
    public static class PowerShell
    {
        private static System.Management.Automation.PowerShell ps { get; set; }

        public static void StartPS(BCVersion version)
        {
            ps = System.Management.Automation.PowerShell.Create();
            ps.AddCommand(string.Format(@"Import-Module 'C:\Program Files\Microsoft Dynamics 365 Business Central\{0}\Service\NavAdminTool.ps1'", version.FolderVersion));
        }

        public static void StopPS()
        {
            ps = null;
        }

        public static string GetNavAppInfo(string instanceName, string appName)
        {
            if (ps == null)
                return null;

            ps.AddCommand("Get-NAVAppInfo");
            ps.AddParameter("ServerInstance", instanceName);
            ps.AddParameter("Name", appName);
            List<PSObject> results = ps.Invoke().ToList();
            if (results == null || results.Count == 0)
                return null;
            return results[0].ToString();
        }
    }
}
