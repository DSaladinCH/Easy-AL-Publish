using System;
using System.Collections.Generic;
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
        private Theme currTheme;

        public Theme CurrTheme
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
            if (currTheme == null)
                CurrTheme = AppModel.Instance.Themes[0];
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
