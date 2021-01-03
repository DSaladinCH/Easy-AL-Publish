using EasyALPublish.Extension;
using EasyALPublish.Misc;
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

        public static string Input(string title, bool topMost)
        {
            PopUp.Input input = new PopUp.Input(title, topMost);
            input.ShowDialog();
            return input.InputText;
        }

        public static bool NewExtension(out BCExtension ex, bool topMost = false)
        {
            PopUp.Extension extension = new PopUp.Extension(topMost);
            extension.ShowDialog();
            ex = extension.BCExtension;
            if (!extension.CloseOK)
                ex = null;
            return extension.CloseOK;
        }

        public static bool EditExtension(ref BCExtension exRef, bool topMost = false)
        {
            BCExtension ex = (BCExtension)exRef.Clone();
            PopUp.Extension extension = new PopUp.Extension(ex, false, topMost);
            extension.ShowDialog();
            if (extension.CloseOK)
                exRef = extension.BCExtension;
            return extension.CloseOK;
        }

        public static bool NewConfig(out PublishConfig ex, bool topMost = false)
        {
            PopUp.Config extension = new PopUp.Config(topMost);
            extension.ShowDialog();
            ex = extension.PublishConfig;
            if (!extension.CloseOK)
                ex = null;
            return extension.CloseOK;
        }

        public static bool EditConfig(ref PublishConfig exRef, bool topMost = false)
        {
            PublishConfig ex = (PublishConfig)exRef.Clone();
            PopUp.Config extension = new PopUp.Config(ex, false, topMost);
            extension.ShowDialog();
            if (extension.CloseOK)
                exRef = extension.PublishConfig;
            return extension.CloseOK;
        }
    }
}
