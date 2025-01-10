using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopMarathonSimulation
{
    public partial class Form3 : Form
    {
        MarathonDBEntities db = new MarathonDBEntities();
        Participants _participants = null;

        public Form3(Participants participants = null)
        {
            InitializeComponent();
            _participants = participants;
            numericUpDown2.Increment = 0.1m;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (_participants != null)
            {
                Text = "Edit";
                textBox1.Text = _participants.Name;
                numericUpDown1.Value = (int)_participants.Age;
                numericUpDown2.Value = (decimal)_participants.Speed;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || numericUpDown1.Value < 1)
            {
                MessageBox.Show("Please fill with valid value!");
                return;
            }

            if (_participants != null)
            {
                var pt = db.Participants.FirstOrDefault(f => f.ParticipantID == _participants.ParticipantID);
                pt.Name = textBox1.Text;
                pt.Speed = numericUpDown2.Value;
                pt.Age = (int)numericUpDown1.Value;

                db.Participants.AddOrUpdate(pt);
                db.SaveChanges();
            }
            else
            {
                Participants p = new Participants()
                {
                    Name = textBox1.Text,
                    Age = (int)numericUpDown1.Value,
                    EventID = Session.events.EventID,
                    Speed = (decimal)numericUpDown2.Value,
                };

                db.Participants.Add(p);
                db.SaveChanges();
            }


            DialogResult = DialogResult.OK;
        }
    }
}
