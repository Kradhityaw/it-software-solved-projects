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
    public partial class Form6 : Form
    {
        QuizinAjaEntities db = new QuizinAjaEntities();

        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                Alerts.Error("Please fill participant name!");
                return;
            }

            Quiz q = db.Quiz.FirstOrDefault(f => f.Code == textBox1.Text);

            if (q != null)
            {
                this.DialogResult = DialogResult.OK;
                Session.qz = q;
                Session.pname = textBox2.Text;


                new Form7().Show();
                Hide();
            }
            else
            {
                Alerts.Error("Quiz not found!");
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            textBox1.Text = "HIST101";
        }
    }
}
