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

namespace DesktopHovRailKiosk
{
    public partial class Form3 : Form
    {
        HovRailKioskEntities db = new HovRailKioskEntities();
        int tabIndex = 0;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Form1().Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (scheduleDataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Please select the schedule!");
                return;
            }

            if (scheduleBindingSource.Current is Schedule schedule)
            {
                var checkAny = db.Ticket.Where(f => f.scheduleID == schedule.scheduleID && f.departureTime == dateTimePicker1.Value).ToList();
                var checkTrain = schedule.Train.capacity;

                if (checkAny.Count >= checkTrain)
                {
                    MessageBox.Show("Seat not available!");
                    return;
                }

                if (DateTime.Now > schedule.departureTime || DateTime.Now.AddDays(3) < schedule.departureTime)
                {
                    MessageBox.Show("Bookings must be made at least one day in advance and up to three days before departure");
                    return;
                }

                Session.sc = schedule;
            }

            

            if (tabControl1.SelectedIndex < tabControl1.TabCount - 1)
            {
                tabIndex += 1;
                tabControl1.SelectedIndex = tabIndex;
            }

            if (tabIndex == 1)
            {
                int limitSeat = 1;
                int selectedSeat = 0;

                flowLayoutPanel1.Controls.Clear();
                for (int i = 0; i < Session.sc.Train.capacity; i++)
                {
                    Button button = new Button()
                    {
                        Width = 64,
                        Height = 64,
                        Text = (i + 1).ToString(),
                        Tag = i + 1
                    };

                    if (!string.IsNullOrEmpty(Session.seat))
                    {
                        if (button.Text == Session.seat)
                        {
                            button.BackColor = Color.Yellow;
                            selectedSeat = 1;
                            button.Tag = -1;
                        }
                    }

                    int convertTag = Convert.ToInt32(button.Tag);

                    button.Click += (s, a) =>
                    {
                        var senderTag = s as Button;

                        if (db.Ticket.Any(f => DbFunctions.TruncateTime(f.departureTime) == dateTimePicker1.Value.Date && f.scheduleID == Session.sc.scheduleID && f.seatNumber == convertTag))
                        {
                            MessageBox.Show("The seat is not empty!");
                            return;
                        }

                        if (selectedSeat == 0)
                        {
                            senderTag.BackColor = Color.Yellow;
                            selectedSeat = 1;
                            senderTag.Tag = -1;
                            Session.seat = senderTag.Text;
                            return;
                        }

                        if (selectedSeat == 1 && Convert.ToInt32(senderTag.Tag) == -1)
                        {
                            senderTag.BackColor = default;
                            senderTag.Tag = 0;
                            selectedSeat = 0;
                        }
                    };

                    if (db.Ticket.Any(f => DbFunctions.TruncateTime(f.departureTime) == dateTimePicker1.Value.Date && f.scheduleID == Session.sc.scheduleID && f.seatNumber == convertTag))
                    {
                        button.BackColor = Color.Red;
                    }

                    flowLayoutPanel1.Controls.Add(button);
                }
            }

            if (button3.Text == "Submit")
            {
                if (MessageBox.Show("Are you sure to booking this ticket?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    decimal travelHour = 0;

                    foreach (RouteDetail r in db.RouteDetail.Where(f => f.routeID == Session.sc.routeID).ToList())
                    {
                        travelHour += r.travelHour;
                    }

                    var price = Session.sc.Route.fixedPrice + (travelHour * Session.sc.Route.pricePerHour);
                    var sampaiStasiun = Session.sc.departureTime.AddHours((double)travelHour);

                    Ticket ticket = new Ticket()
                    {
                        scheduleID = Session.sc.scheduleID,
                        departureStationID = Session.sc.Route.departureStationID,
                        departureTime = Session.sc.departureTime,
                        arrivalStationID = Session.sc.Route.arrivalStationID,
                        arrivalTime = sampaiStasiun,
                        seatNumber = int.Parse(flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(f => f.BackColor == Color.Yellow).Text),
                        passengerName = textBox1.Text,
                        price = price,
                        createdAt = DateTime.Now,
                    };

                    try
                    {
                        db.Ticket.Add(ticket);
                        db.SaveChanges();
                    }
                    catch
                    {
                        MessageBox.Show("Failed to booking ticket!");
                        return;
                    }

                    MessageBox.Show("Success booking ticket!");
                    new Form1().Show();
                    Close();

                    return;
                }
            }

            if (tabIndex == 2)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Please fill passenger name!");
                    tabIndex = 1; 
                    tabControl1.SelectedIndex = tabIndex;
                    return;
                }

                if (!flowLayoutPanel1.Controls.OfType<Button>().Any(f => f.BackColor == Color.Yellow))
                {
                    MessageBox.Show("Please select seat!");
                    tabIndex = 1;
                    tabControl1.SelectedIndex = tabIndex;
                    return;
                }

                button3.Text = "Submit";

                decimal travelHour = 0;

                foreach (RouteDetail r in db.RouteDetail.Where(f => f.routeID == Session.sc.routeID).ToList())
                {
                    travelHour += r.travelHour;
                }

                var price = Session.sc.Route.fixedPrice + (travelHour * Session.sc.Route.pricePerHour);
                var sampaiStasiun = Session.sc.departureTime.AddHours((double)travelHour);

                label7.Text  = $"Schedule Date     : {dateTimePicker1.Value:dd MMMM yyyy HH:mm:ss}";
                label8.Text  = $"Route Name        : {Session.sc.Route.routeName}";
                label9.Text  = $"Departure Station : {Session.sc.Route.Station1.stationName}";
                label10.Text = $"Departure Time    : {Session.sc.departureTime}";
                label11.Text = $"Passenger Name    : {textBox1.Text}";
                label12.Text = $"Price             : {price}";
                label13.Text = $"Train Name        : {Session.sc.Train.trainName}";
                label14.Text = $"Arrival Station   : {Session.sc.Route.Station.stationName}";
                label15.Text = $"Arrival Time      : {sampaiStasiun}";
                label16.Text = $"Seat Number       : {flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(f => f.BackColor == Color.Yellow).Text}";
            }

            if (tabIndex == 0)
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = tabIndex;

            stationBindingSource.DataSource = db.Station.ToList();
            stationBindingSource1.DataSource = db.Station.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabIndex -= 1;
            tabControl1.SelectedIndex = tabIndex;

            if (tabIndex == 0)
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }

            button3.Text = "Next";
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabIndex == 0)
            {
                tabControl1.SelectedIndex = tabIndex;
            }
            else if (tabIndex == 1)
            {
                tabControl1.SelectedIndex = tabIndex;
            }
            else if (tabIndex == 2)
            {
                tabControl1.SelectedIndex = tabIndex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to cancel booking ticket?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new Form1().Show();
                Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = db.Schedule.Where(f => DbFunctions.TruncateTime(f.departureTime) == dateTimePicker1.Value.Date && f.Route.departureStationID == (int)comboBox1.SelectedValue && f.Route.arrivalStationID == (int)comboBox2.SelectedValue).ToList();
            if (query.Count == 0) MessageBox.Show("Schedule Not Available!");
            scheduleBindingSource.DataSource = query;
        }

        private void scheduleDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (scheduleDataGridView.Rows[e.RowIndex].DataBoundItem is Schedule schedule)
            {
                if (e.ColumnIndex == trainCol.Index) e.Value = schedule.Train.trainName;
                if (e.ColumnIndex == routeCol.Index) e.Value = schedule.Route.routeName;
                if (e.ColumnIndex == arrivalTimeCol.Index)
                {
                    decimal t = 0;

                    foreach (RouteDetail r in db.RouteDetail.Where(f => f.routeID == schedule.routeID).ToList())
                    {
                        t += r.travelHour;
                    }

                    e.Value = schedule.departureTime.AddHours(Convert.ToDouble(t));
                }
                if (e.ColumnIndex == priceCol.Index)
                {
                    var fixedPrice = schedule.Route.fixedPrice;
                    var pricePerhour = schedule.Route.pricePerHour;
                    decimal t = 0;

                    foreach (RouteDetail r in db.RouteDetail.Where(f => f.routeID == schedule.routeID).ToList())
                    {
                        t += r.travelHour;
                    }

                    var price = fixedPrice + (t * pricePerhour);
                    e.Value = price;
                }
                if (e.ColumnIndex == availableSeatCol.Index)
                {
                    var checkAny = db.Ticket.Where(f => f.scheduleID == schedule.scheduleID && DbFunctions.TruncateTime(f.departureTime) == dateTimePicker1.Value.Date).ToList();
                    var checkTrain = schedule.Train.capacity;

                    if (checkAny.Count >= checkTrain)
                    {
                        scheduleDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    }

                    e.Value = (checkTrain - checkAny.Count).ToString();
                }
            }
        }
    }
}
