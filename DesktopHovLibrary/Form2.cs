using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopHovLibrary
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void LoadMDI(Form form)
        {
            this.ActiveMdiChild?.Close();
            form.MdiParent = this;
            form.Show();
        }

        private void memberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMDI(new Form3());
        }

        private void bookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMDI(new Form4());
        }

        private void newBorrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMDI(new Form6());
        }

        private void allBorrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMDI(new Form7());
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            Close();
        }
    }
}
