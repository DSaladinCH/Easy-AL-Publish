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
            if (!Commands.IsInitialized())
                Commands.Init(AppModel.Instance.CurrConfig.Version.FolderVersion);
            //PowerShellMgt.StartPS(AppModel.Instance.CurrConfig.Version);
            //string instanceName = AppModel.Instance.CurrConfig.InstanceName;
            Debug.WriteLine("Start before");
            AppModel.Instance.ExtensionMgt.UpdateCurrVersions(AppModel.Instance.CurrConfig.Extensions);
            //AppModel.Instance.CurrConfig.NotifyPropertyChanged(nameof(AppModel.Instance.CurrConfig.Extensions));
            Debug.WriteLine("Start end");
            await Task.Delay(500);
            Debug.WriteLine("End before");
            AppModel.Instance.ExtensionMgt.UpdateNewVersions(AppModel.Instance.CurrConfig.Extensions);
            //await Task.Delay(3000);
            Debug.WriteLine("End after");
            //AppModel.Instance.ExtensionMgt.UninstallExtensions(AppModel.Instance.CurrConfig.Extensions);
            //AppModel.Instance.ExtensionMgt.UnpublishExtensions(AppModel.Instance.CurrConfig.Extensions);
            //AppModel.Instance.ExtensionMgt.PublishAndInstallExtensions(AppModel.Instance.CurrConfig.Extensions);
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
        }

        private void btn_addDependency_Click(object sender, RoutedEventArgs e)
        {
            BCExtension parentExtension = (BCExtension)((Button)sender).DataContext;
            if (!PopUpMgt.NewExtension(out BCExtension newExtension, Topmost))
                return;
            parentExtension.Dependencies.Add(newExtension);
        }

        private void btn_editExtension_Click(object sender, RoutedEventArgs e)
        {
            BCExtension extension = (BCExtension)((MenuItem)sender).DataContext;
            if (!PopUpMgt.EditExtension(ref extension, Topmost))
                return;
            AppModel.Instance.CurrConfig.Extensions.Update((BCExtension)((MenuItem)sender).DataContext, extension);
        }

        private void btn_deleteExtension_Click(object sender, RoutedEventArgs e)
        {
            AppModel.Instance.CurrConfig.Extensions.Delete((BCExtension)((MenuItem)sender).DataContext);
        }

        private void btn_addExtension_Click(object sender, RoutedEventArgs e)
        {
            if (!PopUpMgt.NewExtension(out BCExtension newExtension, Topmost))
                return;
            AppModel.Instance.CurrConfig.Extensions.Add(newExtension);
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
