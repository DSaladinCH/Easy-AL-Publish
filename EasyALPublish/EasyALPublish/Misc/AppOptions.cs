using EasyALPublish.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish.Misc
{
    //[DebuggerDisplay("")]
    public class AppOptions : INotifyPropertyChanged
    {
        private int currTheme = 0;

        public int CurrTheme
        {
            get { return currTheme; }
            set
            {
                currTheme = value;
                NotifyPropertyChanged();
            }
        }

        public AppOptions()
        {

        }

        public void Instantiate()
        {
        }

        #region Notify Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
