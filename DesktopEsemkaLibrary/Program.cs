using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopEsemkaLibrary.Views;

namespace DesktopEsemkaLibrary
{
    internal static class Program
    {
        public class OpenForm : ApplicationContext
        {
            public OpenForm(Form form)
            {
                form.FormClosed += (send, a) =>
                {
                    //var count = ;
                };
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
