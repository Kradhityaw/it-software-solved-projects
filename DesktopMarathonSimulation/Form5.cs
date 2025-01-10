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
    public partial class Form5 : Form
    {
        MarathonDBEntities db = new MarathonDBEntities();
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Text = $"Race Result [{Session.events.EventName}]";

            int i = 0;
            foreach (Participants item in db.Participants.OrderByDescending(f => f.Speed).Where(f => f.EventID == Session.events.EventID).ToList())
            {
                i++;
                var time = (Session.events.DistanceKm * 1000) / item.Speed;
                var spanner = TimeSpan.FromSeconds((int)time);
                listView1.Items.Add($"{i}.{item.Name} - Time: {spanner.Hours:D2} hour, {spanner.Minutes:D2} min, {spanner.Seconds:D2} sec");
            }

            var dataChart = db.Participants.OrderByDescending(f => f.Speed).Where(f => f.EventID == Session.events.EventID)
                .Select(f => new
                {
                    Nama = f.Name,
                    Speed = (Session.events.DistanceKm * 1000) / f.Speed,
                }).ToList();

            var listColor = new[]
            {
                Color.DarkGray,
            };

            chart1.Series[0].Points.DataBind(dataChart, "Nama", "Speed", null);
            chart1.PaletteCustomColors = listColor;

            foreach (var item in chart1.Series)
            {
                item.Points[0].Color = Color.Gold;
                item.Points[1].Color = Color.Silver;
                item.Points[2].Color = Color.Brown;
            }
        }
    }
}
