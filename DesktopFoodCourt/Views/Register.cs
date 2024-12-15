using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopFoodCourt.Views
{
    public partial class Register : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new Login());
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Email Correct format
            if (!new EmailAddressAttribute().IsValid(textBox3.Text))
            {
                Alerts.Error("Your email is invalid!");
                return;
            }

            // All inputs filled
            foreach (TextBox data in groupBox1.Controls.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(data.Text))
                {
                    Alerts.Error("All inputs must be filled!");
                    return;
                }
            }

            // Unique email
            if (db.Users.Any(f => f.Email == textBox3.Text))
            {
                Alerts.Error("Your email is already exist!");
                return;
            }

            // Must be 10 - 15 Digits
            if (textBox4.Text.Length < 10 || textBox4.Text.Length > 15)
            {
                Alerts.Error("Invalid phone number!");
                return;
            }

            // Password must be more than 8 char
            if (textBox5.Text.Length < 8)
            {
                Alerts.Error("Password must more than 8 character!");
                return;
            }

            // Check confirm password
            if (textBox6.Text != textBox5.Text)
            {
                Alerts.Error("Confirm Password not same with your password!");
                return;
            }

            User user = new User
            {
                FirstName = textBox1.Text,
                LastName = textBox2.Text,
                Email = textBox3.Text,
                PhoneNumber = textBox4.Text,
                Password = textBox6.Text,
                DateJoined = DateTime.Now,
                RoleID = 2
            };

            db.Users.Add(user);
            db.SaveChanges();

            Session.us = user;
            new Program.AppContex(new MemberMain());
            Close();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }
    }
}
