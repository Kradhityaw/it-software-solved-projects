using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopMarathonSimulation
{
    public partial class Form1 : Form
    {
        MarathonDBEntities db = new MarathonDBEntities();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dataEvent = db.Events.FirstOrDefault(f => f.EventID == (int)comboBox1.SelectedValue);

            if (dataEvent != null)
            {
                Session.events = dataEvent;

                new Form2().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Event not found!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dataEvent = db.Events.FirstOrDefault(f => f.EventID == (int)comboBox1.SelectedValue);

            if (dataEvent != null)
            {
                Session.events = dataEvent;

                new Form4().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Event not found!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            eventsBindingSource.DataSource = db.Events.ToList();
        }
    }
}
