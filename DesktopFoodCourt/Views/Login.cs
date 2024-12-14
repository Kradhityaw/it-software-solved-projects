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
    public partial class Login : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public Login()
        {
            InitializeComponent();

            // Admin
            textBox1.Text = "dgannyt@squidoo.com";
            textBox2.Text = "dN1|qg!,xuZ";

            // Member
            //textBox1.Text = "lschwant0@europa.eu";
            //textBox2.Text = "uM1%g)Aq0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Find data in users table
            User loginData = db.Users.FirstOrDefault(f => f.Email == textBox1.Text && f.Password == textBox2.Text);

            if (loginData != null)
            {
                Session.us = loginData;

                if (loginData.RoleID == 1) new Program.AppContex(new AdminMain()); else new Program.AppContex(new MemberMain());

                Close();
            }
            else
            {
                Alerts.Error("Your email or password is incorrect!");
                return;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Program.AppContex(new Register());
            Close();
        }
    }
}
