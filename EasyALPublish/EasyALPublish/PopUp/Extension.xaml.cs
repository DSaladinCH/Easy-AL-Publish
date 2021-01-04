using EasyALPublish.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EasyALPublish.PopUp
{
    /// <summary>
    /// Interaction logic for Extension.xaml
    /// </summary>
    public partial class Extension : Window, INotifyPropertyChanged
    {
        private BCExtension bcExtension;
        public BCExtension BCExtension
        {
            get { return bcExtension; }
            set
            {
                bcExtension = value;
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


        public Extension(bool topMost = false) : this(new BCExtension("New Extension", "", "1.0.0.0", ""), true, topMost)
        {
            InitializeComponent();
        }

        public Extension(BCExtension extension, bool createMode = true, bool topMost = false)
        {
            InitializeComponent();
            DataContext = this;
            BCExtension = extension;
            IsCreateMode = createMode;
            if (IsCreateMode)
                Title = "Create a new Extension";
            else
                Title = "Edit Extension";
            Topmost = topMost;
            tbx_extensionName.Focus();
            tbx_extensionName.Dispatcher.BeginInvoke(new Action(() =>
            {
                tbx_extensionName.SelectAll();
            }));
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
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

            if (tbx_extensionName.Text.Length < 2)
                return;

            if (tbx_extensionPublisher.Text.Length < 2)
                return;

            this.Close();
        }
    }
}
