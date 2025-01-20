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
    public partial class Form5 : Form
    {
        MotorcycleRepairEntities db = new MotorcycleRepairEntities();

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            bindingSource1.AddNew();

            var getCurrentUserFirst = db.MotorcycleServices.OrderByDescending(f => f.ServiceCode).FirstOrDefault();
            var incrementId = Convert.ToInt32(getCurrentUserFirst.ServiceCode.Substring(2)) + 1;
            var newUserId = $"SR{incrementId:D3}";

            serviceCodeTextBox.Text = newUserId;
            motorcycleServicesDataGridView.DataSource = db.MotorcycleServices.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(serviceNameTextBox.Text))
            {
                Alert.Error("Plase fill all input fields!");
                return;
            }

            if (costNumericUpDown.Value < 1)
            {
                Alert.Error("Cost must be more than zero!");
                return;
            }

            if (bindingSource1.Current is MotorcycleServices services)
            {
                services.ServiceCode = serviceCodeTextBox.Text;

                db.MotorcycleServices.AddOrUpdate(services);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);

                serviceNameTextBox.ReadOnly = false;
                costNumericUpDown.ReadOnly = false;
            }
        }

        private void motorcycleServicesDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (motorcycleServicesDataGridView.Rows[e.RowIndex].DataBoundItem is MotorcycleServices products)
            {
                var data = db.MotorcycleServices.AsNoTracking().FirstOrDefault(f => f.ServiceCode == products.ServiceCode);

                bindingSource1.DataSource = data;
                serviceCodeTextBox.Text = data.ServiceCode;

                serviceNameTextBox.ReadOnly = true;
                costNumericUpDown.ReadOnly = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (motorcycleServicesDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to edit!");
                return;
            }

            serviceNameTextBox.ReadOnly = false;
            costNumericUpDown.ReadOnly = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (motorcycleServicesDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to delete!");
                return;
            }

            var data = db.MotorcycleServices.Find(serviceCodeTextBox.Text);

            if (Alert.Confirm($"Are you sure to delete '{data.ServiceName}'?") == DialogResult.Yes)
            {
                db.MotorcycleServices.Remove(data);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);

                serviceNameTextBox.ReadOnly = false;
                costNumericUpDown.ReadOnly = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OnLoad(EventArgs.Empty);

            serviceNameTextBox.ReadOnly = false;
            costNumericUpDown.ReadOnly = false;
        }
    }
}
