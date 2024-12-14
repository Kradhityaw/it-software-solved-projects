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
    public partial class AdminMain : Form
    {
        public AdminMain()
        {
            InitializeComponent();

            // Set user name in welcome
            label1.Text = $"Welcome, {Session.us.FirstName} {Session.us.LastName}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new ManageMember());
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new ManageMenu());
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new ManageMenuIngredients());
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new VIewReservation());
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new Login());
            Close();
        }
    }
}
