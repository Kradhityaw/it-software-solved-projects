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
    public partial class Form4 : Form
    {
        MarathonDBEntities db = new MarathonDBEntities();
        List<Timer> timers = new List<Timer>();
        int totalFinish = 0;
        bool toggle = false;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            Text = $"Simulation [{Session.events.EventName}]";
            List<int> allTime = new List<int>();

            foreach (Participants item in db.Participants.Where(f => f.EventID == Session.events.EventID).ToList())
            {
                var time = (Session.events.DistanceKm * 1000) / item.Speed;
                //var search = db.Participants.OrderBy(f => f.Speed).FirstOrDefault();
                //var searchTime = (Session.events.DistanceKm * 1000) / search.Speed;

                allTime.Add((int)time);

                ProgressBar progressBar = new ProgressBar()
                {
                    Width = flowLayoutPanel1.Width - 20,
                    Height = 20,
                    Margin = new Padding(0, 0, 0, 20),
                    Maximum = (int)time,
                };

                Label label = new Label()
                {
                    Text = $"{item.Name}",
                };

                Timer timer = new Timer()
                {
                    Interval = 500,
                };

                timer.Tick += (send, a) =>
                {
                    var val = progressBar.Value;

                    if (val + 50 > progressBar.Maximum)
                    {
                        progressBar.Value = progressBar.Maximum;

                        if (progressBar.Maximum == allTime.Max())
                        {
                            timers.ForEach(t => t.Stop());
                            new Form5().ShowDialog();
                            return;
                        }

                        timer.Stop();
                        return;
                    }
                    progressBar.Value += 50;
                };

                timers.Add(timer);
                flowLayoutPanel1.Controls.Add(label);
                flowLayoutPanel1.Controls.Add(progressBar);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in timers)
            {
                item.Start();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            Hide();
        }
    }
}
