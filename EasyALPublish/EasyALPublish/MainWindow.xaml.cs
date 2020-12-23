using EasyALPublish.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyALPublish
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = DataModel.Instance;
        }

        private void btn_pin_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (Topmost)
            {
                btn_pin.Visibility = Visibility.Collapsed;
                btn_unpin.Visibility = Visibility.Visible;
            }
            else
            {
                btn_pin.Visibility = Visibility.Visible;
                btn_unpin.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            PowerShell ps = PowerShell.Create();
            Task.Run(async () =>
            {
                await Task.Delay(100);
                DataModel.Instance.CurrConfig.Extensions[0].CurrVersion = GetAppCurrVersion(ps, DataModel.Instance.CurrConfig.Extensions[0].Name);
                await Task.Delay(200);
                DataModel.Instance.CurrConfig.Extensions[0].NewVersion = "16.0.0.1";

                await Task.Delay(300);
                DataModel.Instance.CurrConfig.Extensions[0].Dependencies[0].CurrVersion = "16.0.0.45";
                await Task.Delay(150);
                DataModel.Instance.CurrConfig.Extensions[0].Dependencies[0].NewVersion = "16.0.0.46";

                await Task.Delay(50);
                DataModel.Instance.CurrConfig.Extensions[0].Dependencies[1].CurrVersion = "16.0.0.10";
                await Task.Delay(500);
                DataModel.Instance.CurrConfig.Extensions[0].Dependencies[1].NewVersion = "16.0.0.12";
            });
        }

        private string GetAppCurrVersion(PowerShell ps, string instanceName, string appName)
        {
            ps.AddCommand(string.Format("Get-NAVAppInfo -ServerInstance {0} -Name {1}"));
            string appInfo = File.ReadAllText(@"C:\Users\domin\Desktop\GetNavAppInfoTemp.txt");
            List<Match> matches = Regex.Matches(appInfo, @"([A-Za-z0-9 ]+)\:(.+)").ToList();
            Match version = matches.FirstOrDefault(m => m.Groups[1].ToString().Trim() == "Version");
            if (version == null)
                return "";
            return version.Groups[2].ToString().Trim();
        }

        private void cmb_company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataModel.Instance.CurrCompany = (Company)cmb_company.SelectedItem;
            cmb_config.SelectedIndex = 0;
            if (DataModel.Instance.CurrCompany.Configs.Count != 0)
                DataModel.Instance.CurrConfig = DataModel.Instance.CurrCompany.Configs[0];
        }

        private void cmb_config_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataModel.Instance.CurrConfig = (PublishConfig)cmb_config.SelectedItem;
            if (DataModel.Instance.CurrConfig == null)
                return;
        }

        private void btn_addDependency_Click(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void btn_config_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_addExtension_Click(object sender, RoutedEventArgs e)
        {
            DataModel.Instance.CurrConfig.Extensions.Add(new Extension(PopUpMgt.Input("New Extension"), "", "", ""));
        }
    }
}
