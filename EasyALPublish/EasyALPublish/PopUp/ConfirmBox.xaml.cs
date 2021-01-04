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

namespace EasyALPublish.PopUp
{
    /// <summary>
    /// Interaction logic for ConfirmBox.xaml
    /// </summary>
    public partial class ConfirmBox : Window
    {
        public bool CloseOK { get; set; } = false;

        public ConfirmBox(string title, string message, bool topMost = false)
        {
            InitializeComponent();
            Topmost = topMost;
            DataContext = this;
            Title = title;
            tbl_message.Text = message;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseOK = Convert.ToBoolean(((Button)sender).Tag);
            Close();
        }
    }
}
