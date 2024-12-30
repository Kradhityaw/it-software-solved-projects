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
    public partial class Form1 : Form
    {
        QuizinAjaEntities db = new QuizinAjaEntities();

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "mahdi";
            textBox2.Text = "1234";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = db.User.FirstOrDefault(f => f.Username == textBox1.Text && f.Password == textBox2.Text);

            if (user != null)
            {
                Session.us = user;

                new Form3().Show();
                Hide();
            }
            else
            {
                Alerts.Error("Email or password is incorrect!");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) textBox2.UseSystemPasswordChar = false; else textBox2.UseSystemPasswordChar = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Form2().Show();
            Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form6 f = new Form6();
            f.ShowDialog();
            
            if (f.DialogResult == DialogResult.OK)
            {
                Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }
    }
}
