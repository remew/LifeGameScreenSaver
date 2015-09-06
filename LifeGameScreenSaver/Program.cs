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
                if (args[0].ToLower().Trim().Substring(0, 2).Equals("/p"))
                {
                    //Preview
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm(new IntPtr(long.Parse(args[1]))));
                }
                if (args[0].ToLower().Trim().Substring(0, 2).Equals("/c"))
                {
                    //Config
                    MessageBox.Show("オプションなし\nこのスクリーンセーバーには、設定できるオプションはありません。", "LifeGame ScreenSaver");
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ShowScreenSaver();
                Application.Run();
            }
            GC.KeepAlive(mutex);
            mutex.Close();
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
