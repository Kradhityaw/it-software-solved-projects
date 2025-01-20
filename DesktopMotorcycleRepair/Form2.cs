using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopMotorcycleRepair
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void ShowForm(Form form)
        {
            this.ActiveMdiChild?.Close();
            form.MdiParent = this;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Form3());
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Form4());
        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Form5());
        }

        private void mechanicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Form6());
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Form7());
        }
    }
}
