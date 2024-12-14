using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopFoodCourt.Views
{
    public partial class VIewReservation : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public VIewReservation()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Parse("2024-04-03");
        }

        private void VIewReservation_Load(object sender, EventArgs e)
        {
            foreach (Table tab in db.Tables.ToList())
            {
                var dataTable = db.Reservations.Any(f => f.TableID == tab.ID && f.ReservationDate == DbFunctions.TruncateTime(dateTimePicker1.Value));

                TableUC tableUC = new TableUC()
                {
                    LabelText = $"{tab.ID}",
                    ImageTable = dataTable ? DesktopFoodCourt.Properties.Resources.table_reserved : DesktopFoodCourt.Properties.Resources.table_free,
                };

                var dataReserv = db.Reservations.FirstOrDefault(f => f.TableID == tab.ID && f.ReservationDate == DbFunctions.TruncateTime(dateTimePicker1.Value));

                tableUC.ev += (s, a) =>
                {
                    textBox1.Text = dataReserv?.CustomerFirstName;
                    textBox2.Text = dataReserv?.CustomerLastName;
                    textBox3.Text = dataReserv?.CustomerEmail;
                    textBox4.Text = dataReserv?.CustomerPhoneNumber;

                    if (dataReserv != null)
                    {
                        reservationDetailBindingSource.DataSource = db.ReservationDetails.Where(f => f.ReservationID == dataReserv.ID).ToList();
                    }
                    else
                    {
                        reservationDetailBindingSource.Clear();
                    }

                };

                flowLayoutPanel1.Controls.Add(tableUC);
            }
        }

        private void reservationDetailDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (reservationDetailDataGridView.Rows[e.RowIndex].DataBoundItem is ReservationDetail reservation)
            {
                if (e.ColumnIndex == menCol.Index) e.Value = reservation.Menu.Name;
                if (e.ColumnIndex == priceCol.Index) e.Value = reservation.Menu.Price.ToString("C2", new CultureInfo("id-ID"));
                if (e.ColumnIndex == subCol.Index)
                {
                    var countTotal = reservation.Menu.Price * reservation.Qty;

                    e.Value = countTotal.ToString("C2", new CultureInfo("id-ID"));
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (Table tab in db.Tables.ToList())
            {
                var dataTable = db.Reservations.Any(f => f.TableID == tab.ID && f.ReservationDate == DbFunctions.TruncateTime(dateTimePicker1.Value));

                TableUC tableUC = new TableUC()
                {
                    LabelText = $"{tab.ID}",
                    ImageTable = dataTable ? DesktopFoodCourt.Properties.Resources.table_reserved : DesktopFoodCourt.Properties.Resources.table_free,
                };

                var dataReserv = db.Reservations.FirstOrDefault(f => f.TableID == tab.ID && f.ReservationDate == DbFunctions.TruncateTime(dateTimePicker1.Value));

                tableUC.ev += (s, a) =>
                {
                    textBox1.Text = dataReserv?.CustomerFirstName;
                    textBox2.Text = dataReserv?.CustomerLastName;
                    textBox3.Text = dataReserv?.CustomerEmail;
                    textBox4.Text = dataReserv?.CustomerPhoneNumber;

                    if (dataReserv != null)
                    {
                        reservationDetailBindingSource.DataSource = db.ReservationDetails.Where(f => f.ReservationID == dataReserv.ID).ToList();
                    }
                    else
                    {
                        reservationDetailBindingSource.Clear();
                    }

                };

                flowLayoutPanel1.Controls.Add(tableUC);
            }
        }

        private void VIewReservation_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Program.AppContex(new AdminMain());

        }
    }
}
