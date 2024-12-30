using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulasiDesktopW4
{
    internal class Alerts
    {
        public static DialogResult Success(string m)
        {
            return MessageBox.Show(m, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult Error(string m)
        {
            return MessageBox.Show(m, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Confirm(string m)
        {
            return MessageBox.Show(m, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
