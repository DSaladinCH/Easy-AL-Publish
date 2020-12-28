using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyALPublish.Resources
{
    public partial class NoResize : ResourceDictionary
    {
        public NoResize()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.Close();
        }

        private void OnResize(object sender, RoutedEventArgs e)
        {
            var window = (Window)(sender);

            if (window == null) return;


            if (window.WindowState == WindowState.Maximized)
            {
                window.BorderThickness = new Thickness(8);
                window.Margin = new Thickness(8);
            }
            else
            {
                window.BorderThickness = new Thickness(0);
            }
        }
    }
}
