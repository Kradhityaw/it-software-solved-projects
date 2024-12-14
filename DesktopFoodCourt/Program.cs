using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopFoodCourt.Views;

namespace DesktopFoodCourt
{
    internal static class Program
    {
        public class AppContex : ApplicationContext
        {
            public AppContex(Form form)
            {
                form.FormClosed += (send, args) =>
                {
                    int countForm = Application.OpenForms.Cast<Form>().Count(f => f.TopLevel);

                    if (countForm == 0)
                    {
                        Application.Exit();
                        ExitThread();
                    }
                };

                form.Show();
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
            Application.Run(new AppContex(new Login()));
        }
    }
}
