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

namespace DesktopMotorcycleRepair
{
    public partial class Form6 : Form
    {
        MotorcycleRepairEntities db = new MotorcycleRepairEntities();

        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mechanicNameTextBox.Text))
            {
                Alert.Error("Please fill all input fields!");
                return;
            }

            if (bindingSource1.Current is Mechanics mechanics)
            {
                mechanics.MechanicCode = mechanicCodeTextBox.Text;

                db.Mechanics.AddOrUpdate(mechanics);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);
                mechanicNameTextBox.ReadOnly = false;
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            bindingSource1.AddNew();

            var getCurrentUserFirst = db.Mechanics.OrderByDescending(f => f.MechanicCode).FirstOrDefault();
            var incrementId = Convert.ToInt32(getCurrentUserFirst.MechanicCode.Substring(2)) + 1;
            var newUserId = $"MC{incrementId:D3}";

            mechanicCodeTextBox.Text = newUserId;
            mechanicsBindingSource.DataSource = db.Mechanics.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mechanicsDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to edit!");
                return;
            }

            mechanicNameTextBox.ReadOnly = false;
        }

        private void mechanicsDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (mechanicsDataGridView.Rows[e.RowIndex].DataBoundItem is Mechanics products)
            {
                var data = db.Mechanics.AsNoTracking().FirstOrDefault(f => f.MechanicCode == products.MechanicCode);

                bindingSource1.DataSource = data;
                mechanicCodeTextBox.Text = data.MechanicCode;

                mechanicNameTextBox.ReadOnly = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mechanicsDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to delete!");
                return;
            }

            var data = db.Mechanics.Find(mechanicCodeTextBox.Text);

            if (Alert.Confirm($"Are you sure to delete '{data.MechanicName}'?") == DialogResult.Yes)
            {
                db.Mechanics.Remove(data);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);

                mechanicNameTextBox.ReadOnly = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OnLoad(EventArgs.Empty);

            mechanicNameTextBox.ReadOnly = false;
        }
    }
}
