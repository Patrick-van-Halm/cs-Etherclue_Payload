using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Etherclue
{
    static class Program
    {
        public static string cyphertext = "JudydbtUHuex8gak3uT45EFvTbwR2pP7";
#if DEBUG
        public static bool debug = true;
#else
        public static bool debug = false;
#endif
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EtherclueMain());
        }
    }
}
