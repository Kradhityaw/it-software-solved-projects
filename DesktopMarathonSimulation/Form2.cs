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
    public partial class Form2 : Form
    {
        MarathonDBEntities db = new MarathonDBEntities();

        public Form2()
        {
            InitializeComponent();
            Text = $"Participants [{Session.events.EventName}]";
        }

        public BindingSource bs
        {
            get { return participantsBindingSource; }
            set { participantsBindingSource = value; }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            participantsBindingSource.DataSource = db.Participants.Where(f => f.EventID == Session.events.EventID).ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (participantsDataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Please select a participant!");
                return;
            }

            if (participantsBindingSource.Current is Participants participants)
            {
                if (MessageBox.Show("Are you sure to delete this participant?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var find = db.Participants.Find(participants.ParticipantID);

                    db.Participants.Remove(find);
                    db.SaveChanges();

                    OnLoad(EventArgs.Empty);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (participantsDataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Please select a participant!");
                return;
            }

            if (participantsBindingSource.Current is Participants participants)
            {
                // Bug Disini
                var edit = new Form3(participants);
                edit.ShowDialog();

                if (edit.DialogResult == DialogResult.OK)
                {
                    participantsBindingSource.Clear();
                    participantsBindingSource.DataSource = db.Participants.Where(f => f.EventID == Session.events.EventID).ToList();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var edit = new Form3();
            edit.ShowDialog();

            if (edit.DialogResult == DialogResult.OK)
            {
                participantsBindingSource.Clear();
                participantsBindingSource.DataSource = db.Participants.Where(f => f.EventID == Session.events.EventID).ToList();
            }
        }
    }
}
