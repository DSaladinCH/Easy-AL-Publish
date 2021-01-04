using EasyALPublish.Extension;
using EasyALPublish.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace EasyALPublish.PopUp
{
    /// <summary>
    /// Interaction logic for PublishConfig.xaml
    /// </summary>
    public partial class Config : Window, INotifyPropertyChanged
    {
        private bool openDropDown = true;

        private PublishConfig publishConfig;
        public PublishConfig PublishConfig
        {
            get { return publishConfig; }
            set
            {
                publishConfig = value;
                NotifyPropertyChanged();
            }
        }

        public bool CloseOK { get; set; } = false;

        private bool isCreateMode;
        public bool IsCreateMode
        {
            get { return isCreateMode; }
            set
            {
                isCreateMode = value;
                NotifyPropertyChanged();
            }
        }

        public Config(bool topMost = false) : this(new PublishConfig("New Config", null, "", ""), true, topMost)
        {
            InitializeComponent();
        }

        public Config(PublishConfig extension, bool createMode = true, bool topMost = false)
        {
            InitializeComponent();
            DataContext = this;
            PublishConfig = extension;
            IsCreateMode = createMode;
            if (IsCreateMode)
                Title = "Create a new Config";
            else
                Title = "Edit Config";
            Topmost = topMost;
            cmb_configVersion.ItemsSource = AppModel.Instance.BCVersions;
            //cmb_configVersion.SelectedItem = AppModel.Instance.AppOptions.BCVersions.First(v => v.FolderVersion == extension.Version.FolderVersion);

            tbx_configName.Focus();
            tbx_configName.Dispatcher.BeginInvoke(new Action(() =>
            {
                tbx_configName.SelectAll();
            }));
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
            openDropDown = true;
        }

        private void cmb_configVersion_GotFocus(object sender, RoutedEventArgs e)
        {
            if (openDropDown)
            {
                openDropDown = false;
                ((ComboBox)sender).IsDropDownOpen = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            CloseOK = Convert.ToBoolean(((Button)sender).Tag);
            if (!CloseOK)
            {
                this.Close();
                return;
            }

            if (tbx_configName.Text.Length < 2)
                return;

            if (tbx_instanceName.Text.Length < 2)
                return;

            if (cmb_configVersion.SelectedIndex == -1)
                return;

            if (tbx_extensionsPath.Text.Length < 2)
                return;

            this.Close();
        }

        private void cmb_configVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(((BCVersion)((ComboBox)sender).SelectedItem).Name);
            Debug.WriteLine(PublishConfig.Version.Name);

            tbx_extensionsPath.Focus();
        }
    }
}
