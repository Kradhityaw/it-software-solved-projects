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
    public partial class Form1 : Form
    {
        MotorcycleRepairEntities db = new MotorcycleRepairEntities();

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "USR-20-01";
            textBox2.Text = "y7IGGjwH";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                Alert.Error("Please Fill All Input Fields!");
                return;
            }

            var data = db.Users.FirstOrDefault(f => f.UserCode == textBox1.Text);
            if (data == null)
            {
                Alert.Error("User Not Found!");
                return;
            }

            var checkPassword = db.Users.FirstOrDefault(f => f.UserCode == data.UserCode.ToString() && f.UserPassword == textBox2.Text);
            if (checkPassword == null)
            {
                Alert.Error("Incorrect Password!");
                return;
            }

            Session.usr = data;

            new Form2().Show();
            Hide();
        }
    }
}
