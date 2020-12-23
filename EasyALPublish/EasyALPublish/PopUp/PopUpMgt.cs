using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish
{
    public static class PopUpMgt
    {
        public static string Input(string title, string startText = "")
        {
            PopUp.Input input = new PopUp.Input(title, startText);
            input.ShowDialog();
            return input.InputText;
        }
    }
}
