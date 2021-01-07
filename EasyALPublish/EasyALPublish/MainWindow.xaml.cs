using DSaladin.DynamicsBC;
using EasyALPublish.Extension;
using EasyALPublish.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            DataContext = AppModel.Instance;
            AppModel.Instance.AppOptions.Instantiate();
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

        private async void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (AppModel.Instance.CurrConfig == null || AppModel.Instance.CurrConfig.Extensions.Count == 0)
                return;

            int extensionsCount = AppModel.Instance.CurrConfig.Extensions.CountAll();
            if (!PopUpMgt.Confirm("Uninstall and re-install", 
                string.Format("Are you sure you want to uninstall and unpublish {0} extensions you have set up and re-publish and re-install them with the latest version?", extensionsCount), 
                Topmost))
                return;

            pgr_progress.Minimum = 0;
            pgr_progress.Value = 0;
            pgr_progress.Maximum = extensionsCount * 4 + 2;

            await Task.Run(() =>
            {
                Debug.WriteLine("Getting current Versions");
                AppModel.Instance.ExtensionMgt.UpdateCurrVersions(AppModel.Instance.CurrConfig.Extensions);
                Dispatcher.Invoke(() => pgr_progress.Value += 1);
                Debug.WriteLine("Got current Versions");
                Debug.WriteLine("Getting new Versions");
                AppModel.Instance.ExtensionMgt.UpdateNewVersions(AppModel.Instance.CurrConfig.Extensions);
                Dispatcher.Invoke(() => pgr_progress.Value += 1);
                Debug.WriteLine("Got new Versions");
                int count = 0;
                foreach (var item in AppModel.Instance.CurrConfig.Extensions)
                {
                    if (item.Status == Extension.ExtensionStatus.Installed)
                        count += 4;
                    else if (item.Status == Extension.ExtensionStatus.Published)
                        count += 3;
                    else
                        count += 2;
                }
                Debug.WriteLine("Uninstalling Extensions");
                AppModel.Instance.ExtensionMgt.Uninstall(AppModel.Instance.CurrConfig.Extensions, this, pgr_progress);
                Debug.WriteLine("Uninstalled Extensions");
                Debug.WriteLine("Unpublishing Extensions");
                AppModel.Instance.ExtensionMgt.Unpublish(AppModel.Instance.CurrConfig.Extensions, this, pgr_progress);
                Debug.WriteLine("Unpublished Extensions");
                Debug.WriteLine("Publishing and Installing Extensions");
                AppModel.Instance.ExtensionMgt.PublishAndInstall(AppModel.Instance.CurrConfig.Extensions, this, pgr_progress);
                Debug.WriteLine("Published and Installed Extensions");

                Dispatcher.Invoke(() => PopUpMgt.Message("Completion", "All extension have been successfully reinstalled", Topmost));
            });
        }

        private void cmb_company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppModel.Instance.CurrCompany = (Company)cmb_company.SelectedItem;
            cmb_config.SelectedIndex = 0;
            if (AppModel.Instance.CurrCompany.Configs.Count != 0)
                AppModel.Instance.CurrConfig = AppModel.Instance.CurrCompany.Configs[0];
        }

        private void cmb_config_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppModel.Instance.CurrConfig = (PublishConfig)cmb_config.SelectedItem;
            if (AppModel.Instance.CurrConfig == null)
                return;

            if (string.IsNullOrEmpty(AppModel.Instance.CurrConfig.ComputerName))
                Commands.Init(AppModel.Instance.CurrConfig.Version.FolderVersion, false);
            else
                Commands.Init(AppModel.Instance.CurrConfig.ComputerName, AppModel.Instance.CurrConfig.Version.FolderVersion, false);
        }

        private void btn_addDependency_Click(object sender, RoutedEventArgs e)
        {
            BCExtension parentExtension = (BCExtension)((Button)sender).DataContext;
            if (!PopUpMgt.NewExtension(out BCExtension newExtension, Topmost))
                return;
            parentExtension.Dependencies.Add(newExtension);
            AppModel.Instance.SaveData();
        }

        private void btn_editExtension_Click(object sender, RoutedEventArgs e)
        {
            BCExtension extension = (BCExtension)((MenuItem)sender).DataContext;
            if (!PopUpMgt.EditExtension(ref extension, Topmost))
                return;
            AppModel.Instance.CurrConfig.Extensions.Update((BCExtension)((MenuItem)sender).DataContext, extension);
            AppModel.Instance.SaveData();
        }

        private void btn_deleteExtension_Click(object sender, RoutedEventArgs e)
        {
            AppModel.Instance.CurrConfig.Extensions.Delete((BCExtension)((MenuItem)sender).DataContext);
            AppModel.Instance.SaveData();
        }

        private void btn_addExtension_Click(object sender, RoutedEventArgs e)
        {
            if (!PopUpMgt.NewExtension(out BCExtension newExtension, Topmost))
                return;
            AppModel.Instance.CurrConfig.Extensions.Add(newExtension);
            AppModel.Instance.SaveData();
        }

        private void btn_config_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(Topmost);
            settings.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppModel.Instance.SaveData();
        }

        private void trv_extensions_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                //here you would probably want to include code that is called by your
                //mouse down event handler.
                e.Handled = true;
            }
        }
    }
}
