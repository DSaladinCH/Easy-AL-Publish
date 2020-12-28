using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : Window, INotifyPropertyChanged
    {
        private string inputText;

        public string InputText
        {
            get { return inputText; }
            set
            {
                inputText = value;
                NotifyPropertyChanged();
            }
        }
        public bool CloseOK { get; set; } = false;

        public Input(string title, string startText = "")
        {
            InitializeComponent();
            DataContext = this;
            Title = title;
            InputText = startText;
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

            if (tbx_input.Text.Length < 1)
                return;

            this.Close();
        }
    }
}
