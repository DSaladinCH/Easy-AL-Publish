using DSaladin.DynamicsBC;
using EasyALPublish.Misc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyALPublish
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private Company currCompany;

        public Settings(bool topMost = false)
        {
            InitializeComponent();
            DataContext = AppModel.Instance;
            Topmost = topMost;

            Closing += (s, e) =>
            {
                AppModel.Instance.SaveData();
            };
        }

        private void lst_companies_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((ListViewItem)sender).DataContext == null)
                return;
            currCompany = (Company)((ListViewItem)sender).DataContext;
            lst_configs.ItemsSource = ((Company)((ListViewItem)sender).DataContext).Configs;
        }

        private void btn_addCompany_Click(object sender, RoutedEventArgs e)
        {
            string company = PopUpMgt.Input("Create Company");
            if (string.IsNullOrEmpty(company))
                return;
            AppModel.Instance.Companies.Add(new Company(company));
        }

        private void btn_deleteCompany_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete Company", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                AppModel.Instance.Companies.Remove((Company)((Button)sender).DataContext);
        }

        private void btn_addConfig_Click(object sender, RoutedEventArgs e)
        {
            if (currCompany == null)
                return;

            if (!PopUpMgt.NewConfig(out PublishConfig publishConfig, Topmost))
                return;
            currCompany.Configs.Add(publishConfig);
        }

        private void btn_editConfig_Click(object sender, RoutedEventArgs e)
        {
            PublishConfig publishConfig = (PublishConfig)((Button)sender).DataContext;
            if (!PopUpMgt.EditConfig(ref publishConfig, Topmost))
                return;
            currCompany.Configs.Update((PublishConfig)((Button)sender).DataContext, publishConfig);
        }

        private void btn_uploadLicense_Click(object sender, RoutedEventArgs e)
        {
            PublishConfig publishConfig = (PublishConfig)((Button)sender).DataContext;
            Commands.Init(publishConfig.Version.FolderVersion, true);
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select License File";
            dialog.Filter = "License File (*.flf)|*.flf";
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == false || dialogResult == null)
                return;

            bool restart = PopUpMgt.Confirm("Restart Instance", "Would you like to restart the Server now?", Topmost);
            Commands.UploadLicense(publishConfig.InstanceName, dialog.FileName, restart);
            PopUpMgt.Message("Uploaded", "License File successfully uploaded", Topmost);
        }

        private void btn_deleteConfig_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete Config", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                currCompany.Configs.Remove((PublishConfig)((Button)sender).DataContext);
            }
        }

        private void CloseWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
