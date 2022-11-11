using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mankala
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BoardFactory boardFactory = new BoardFactory();
            Board test = boardFactory.CreateBoard(3, true, 4, new Player("f"), new Player("D"));
            test.printPockets();
            Application.Run(new Game());
        }
    }
}
