using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopFoodCourt.Views
{
    public partial class TableUC : UserControl
    {
        public TableUC()
        {
            InitializeComponent();
            pictureBox1.Click += PBCLick;
        }

        public string LabelText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public Image ImageTable
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        public event EventHandler ev;

        public void PBCLick(object sender, EventArgs e)
        {
            ev?.Invoke(this, e);
        }
    }
}
