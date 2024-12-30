using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulasiDesktopW4
{
    public partial class Form2 : Form
    {
        QuizinAjaEntities db = new QuizinAjaEntities();

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (db.User.Any(f => f.Username == textBox1.Text))
            {
                Alerts.Error("Username already exist!");
                return;
            }

            if (textBox2.Text.Length < 4)
            {
                Alerts.Error("Password must be more than 4 character!");
                return;
            }

            if (textBox4.Text != textBox2.Text)
            {
                Alerts.Error("Your retype password not same with password!");
                return;
            }

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                Alerts.Error("All inputs must be filled!");
                return;
            }

            User user = new User()
            {
                Username = textBox1.Text,
                FullName = textBox3.Text,
                Password = textBox3.Text,
                DateOfBirth = dateTimePicker1.Value
            };

            db.User.Add(user);
            db.SaveChanges();

            Session.us = user;

            new Form3().Show();
            Hide();
        }
    }
}
