using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopFoodCourt.Views
{
    public partial class History : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public History()
        {
            InitializeComponent();
        }

        private void History_Load(object sender, EventArgs e)
        {
            reservationBindingSource.DataSource = db.ReservationDetails.Where(f => f.Reservation.UserID == Session.us.ID).ToList().GroupBy(i => i.ReservationID).Select(f => f.First());
        }

        private void reservationDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (reservationDataGridView.Rows[e.RowIndex].DataBoundItem is ReservationDetail reservation)
            {
                if (e.ColumnIndex == tabCol.Index) e.Value = reservation.Reservation.Table.Name;
                if (e.ColumnIndex == totpriceCol.Index)
                {
                    double totPrice = 0;
                    foreach (var tot in db.ReservationDetails.Where(f => f.ReservationID == reservation.ReservationID).ToList())
                    {
                        totPrice += tot.Menu.Price * tot.Qty;
                    }

                    e.Value = totPrice.ToString("C2", new CultureInfo("id-ID"));
                }
                if (e.ColumnIndex == numCol.Index) e.Value = reservation.Reservation.NumberOfPeople;
                if (e.ColumnIndex == resdateCol.Index) e.Value = reservation.Reservation.ReservationDate;
            }
        }

        private void reservationDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (reservationDataGridView.Rows[e.RowIndex].DataBoundItem is ReservationDetail reservation)
            {
                bindingSource1.DataSource = db.ReservationDetails.Where(f => f.ReservationID == reservation.ReservationID).ToList();
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is ReservationDetail reservation)
            {
                if (e.ColumnIndex == menuCol.Index) e.Value = reservation.Menu.Name;
                if (e.ColumnIndex == priceCol.Index) e.Value = reservation.Menu.Price;
                if (e.ColumnIndex == subCol.Index)
                {
                    var sum = reservation.Menu.Price * reservation.Qty;

                    e.Value = sum.ToString("C2", new CultureInfo("id-ID"));
                }
            }
        }

        private void History_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Program.AppContex(new MemberMain());

        }
    }
}
