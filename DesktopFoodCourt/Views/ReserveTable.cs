using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesktopFoodCourt.Views
{
    public partial class ReserveTable : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public ReserveTable()
        {
            InitializeComponent();
            reservationDateDateTimePicker.Value = DateTime.Parse("2024-04-03");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                groupBox1.Enabled = false;
                customerEmailTextBox.Text = Session.us.Email;
                customerFirstNameTextBox.Text = Session.us.FirstName;
                customerLastNameTextBox.Text = Session.us.LastName;
                customerPhoneNumberTextBox.Text = Session.us.PhoneNumber;
            }
            else
            {
                groupBox1.Enabled = true;
            }
        }

        private void ReserveTable_Load(object sender, EventArgs e)
        {
            //reservationBindingSource.AddNew();

            customerEmailTextBox.Text = Session.us.Email;
            customerFirstNameTextBox.Text = Session.us.FirstName;
            customerLastNameTextBox.Text = Session.us.LastName;
            customerPhoneNumberTextBox.Text = Session.us.PhoneNumber;

            // N + 1 Query
            //foreach (Reservation item in db.Reservations.Where(f => f.ReservationDate == reservationDateDateTimePicker.Value).ToList())
            //{
            //    var table = db.Tables.Where(f => f.ID != item.TableID).ToList();
            //    tableBindingSource.DataSource = table;
            //}

            var reservedTable = db.Reservations.Where(f => f.ReservationDate == reservationDateDateTimePicker.Value).Select(r => r.TableID).ToList();
            var availableTable = db.Tables.Where(f => !reservedTable.Contains(f.ID)).ToList();

            tableBindingSource.DataSource = availableTable;

            menuBindingSource.DataSource = db.Menus.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                // Email Correct format
                if (!new EmailAddressAttribute().IsValid(customerEmailTextBox.Text))
                {
                    Alerts.Error("Your email is invalid!");
                    return;
                }

                // Must be 10 - 15 Digits
                if (customerPhoneNumberTextBox.Text.Length < 10 || customerPhoneNumberTextBox.Text.Length > 15)
                {
                    Alerts.Error("Invalid phone number!");
                    return;
                }

                if (string.IsNullOrEmpty(customerFirstNameTextBox.Text) || string.IsNullOrEmpty(customerLastNameTextBox.Text) || string.IsNullOrEmpty(customerPhoneNumberTextBox.Text) || string.IsNullOrEmpty(customerEmailTextBox.Text))
                {
                    Alerts.Error("All inputs must be filled!");
                    return;
                }
            }

            if (numberOfPeopleNumericUpDown.Value < 1 || numberOfPeopleNumericUpDown.Value > 4)
            {
                Alerts.Error("People must be more than 1 and not more than 4");
                return;
            }

            Reservation reservation = new Reservation
            {
                UserID = Session.us.ID,
                CustomerFirstName = customerFirstNameTextBox.Text,
                CustomerLastName = customerLastNameTextBox.Text,
                CustomerEmail = customerEmailTextBox.Text,
                CustomerPhoneNumber = customerPhoneNumberTextBox.Text,
                NumberOfPeople = (int)numberOfPeopleNumericUpDown.Value,
                Table = comboBox1.SelectedItem as Table,
                ReservationDate = reservationDateDateTimePicker.Value
            };

            db.Reservations.Add(reservation);
            db.SaveChanges();

            List<ReservationDetail> res = new List<ReservationDetail>();

            foreach (ReservationDetail resdet in reservationDetailBindingSource.List)
            {
                resdet.ReservationID = reservation.ID;
                res.Add(resdet);
            }

            db.ReservationDetails.AddRange(res);
            db.SaveChanges();

            Alerts.Success("Reservation success!");

            new Program.AppContex(new MemberMain());
            Close();
        }

        private void customerPhoneNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 1)
            {
                Alerts.Error("Quantity cannot 0!");
                return;
            }

            ReservationDetail resdet = new ReservationDetail()
            {
                Menu = comboBox2.SelectedItem as Menu,
                Qty = (int)numericUpDown1.Value,
            };

            foreach (ReservationDetail res in reservationDetailBindingSource.List)
            {
                if (res.Menu.ID == resdet.Menu.ID)
                {
                    resdet.Qty += res.Qty;

                    reservationDetailBindingSource.Remove(res);

                    reservationDetailBindingSource.Add(resdet);
                    return;
                }
            }

            reservationDetailBindingSource.Add(resdet);
        }

        private void reservationDetailDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (reservationDetailDataGridView.Rows[e.RowIndex].DataBoundItem is ReservationDetail reservation)
                {
                    if (e.ColumnIndex == menuCol.Index) e.Value = reservation?.Menu?.Name;
                    if (e.ColumnIndex == priceCol.Index) e.Value = reservation?.Menu?.Price.ToString("C2", new CultureInfo("id-ID"));
                    if (e.ColumnIndex == subCol.Index)
                    {
                        var totalPrice = reservation?.Menu?.Price * reservation?.Qty;

                        e.Value = totalPrice?.ToString("C2", new CultureInfo("id-ID"));
                    }
                    if (e.ColumnIndex == actCol.Index) e.Value = "Remove";
                }
            }
            catch { }
        }

        private void reservationDetailDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (reservationDetailDataGridView.Rows[e.RowIndex].DataBoundItem is ReservationDetail reservation)
            {
                if (e.ColumnIndex == actCol.Index)
                {
                    reservationDetailBindingSource.Remove(reservation);
                }
            }
        }

        private void reservationDetailBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            double price = 0;

            foreach (ReservationDetail reservation in reservationDetailBindingSource.List)
            {
                price += reservation.Menu.Price * reservation.Qty;
            }

            label8.Text = price.ToString("C2", new CultureInfo("id-ID"));
            label10.Text = (price + Convert.ToDouble(50000)).ToString("C2", new CultureInfo("id-ID"));
        }

        private void reservationDateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            var reservedTable = db.Reservations.Where(f => f.ReservationDate == reservationDateDateTimePicker.Value).Select(r => r.TableID).ToList();
            var availableTable = db.Tables.Where(f => !reservedTable.Contains(f.ID)).ToList();

            tableBindingSource.DataSource = availableTable;
        }

        private void ReserveTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Program.AppContex(new MemberMain());

        }
    }
}
