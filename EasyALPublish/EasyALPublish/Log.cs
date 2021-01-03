using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyALPublish
{
    //[DebuggerDisplay("")]
    public static class Log
    {
        private static string filePath = @"C:\temp\easyalpublish.log";

        public static void LogText(string text)
        {
            File.AppendAllText(filePath, string.Format("\n[{0}] {1}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss ffff"), text));
        }
    }
}
