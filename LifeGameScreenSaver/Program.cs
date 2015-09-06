using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeGameScreenSaver
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, Application.ProductName);
            if (mutex.WaitOne(0, false) == false) return;
            GC.KeepAlive(mutex);
            mutex.Close();
            if (args.Length > 0)
            {
                if (args[0].ToLower().Trim().Substring(0, 2).Equals("/s"))
                {
                    //Show
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    ShowScreenSaver();
                    Application.Run();
                }
                if (args[0].ToLower().Trim().Substring(0, 2).Equals("/c"))
                {
                    //Config
                }
                if (args[0].ToLower().Trim().Substring(0, 2).Equals("/p"))
                {
                    //Preview
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ShowScreenSaver();
                Application.Run();
            }
        }

        static void ShowScreenSaver()
        {
            foreach( Screen screen in Screen.AllScreens )
            {
                MainForm screensaver = new MainForm(screen.Bounds);
                screensaver.Show();
            }
        }
    }
}
