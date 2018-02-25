using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;

namespace projectY
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string GetRegValue(string key,string dv)
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey("py");
            if (k != null)
                return k.GetValue(key, dv).ToString();
            else
                return dv;
        }

        public static int GetRegValue(string key,int i)
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey("py");
            if (k != null)
                return ((int)k.GetValue(key, i));
            else
                return i;
        }

        public static void SetRegValue(string key, string v)
        {
            if (Registry.CurrentUser.OpenSubKey("py") == null)
                Registry.CurrentUser.CreateSubKey("py");
            Registry.CurrentUser.OpenSubKey("py",true).SetValue(key, v);
        }

        public static void SetRegValue(string key, int v)
        {
            if (Registry.CurrentUser.OpenSubKey("py") == null)
                Registry.CurrentUser.CreateSubKey("py");
            Registry.CurrentUser.OpenSubKey("py",true).SetValue(key, v);
        }

    }
}
