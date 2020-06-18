using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_SQL
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static int T_EOF = 0;
        public static int T_ERROR = 1;
        public static int T_WS = 3;

        public static int T_INT = 10;
        public static int T_STRING = 11;
        public static int T_FLOAT = 12;
        public static int T_DATE = 13;
    }
}
