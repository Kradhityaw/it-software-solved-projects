using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesktopFoodCourt.Views
{
    public partial class MemberMain : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public MemberMain()
        {
            InitializeComponent();
            timer1.Start();

            label1.Text = $"Welcome, {Session.us.FirstName} {Session.us.LastName}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = $"Current Time: {DateTime.Now:HH:mm:ss}";
        }

        private void MemberMain_Load(object sender, EventArgs e)
        {
            foreach (Table tab in db.Tables.ToList())
            {
                var dataTable = db.Reservations.Any(f => f.TableID == tab.ID && f.ReservationDate == DbFunctions.TruncateTime(DateTime.Now));

                TableUC tableUC = new TableUC()
                {
                    LabelText = $"{tab.ID}",
                    ImageTable = dataTable ? DesktopFoodCourt.Properties.Resources.table_reserved : DesktopFoodCourt.Properties.Resources.table_free,
                };

                flowLayoutPanel1.Controls.Add(tableUC);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new Login());
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new ReserveTable());
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Program.AppContex(new History());
            Close();
        }
    }
}
