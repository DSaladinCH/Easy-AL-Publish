using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyALPublish.Resources
{
    public partial class CanMinimize : ResourceDictionary
    {
        public CanMinimize()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.Close();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = WindowState.Minimized;
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
