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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesktopMotorcycleRepair
{
    public partial class Form4 : Form
    {
        MotorcycleRepairEntities db = new MotorcycleRepairEntities();

        public Form4()
        {
            InitializeComponent();
            priceNumericUpDown.Maximum = decimal.MaxValue;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            bindingSource1.AddNew();

            var getCurrentUserFirst = db.Products.OrderByDescending(f => f.ProductCode).FirstOrDefault();
            var incrementId = Convert.ToInt32(getCurrentUserFirst.ProductCode.Substring(2)) + 1;
            var newUserId = $"PR{incrementId:D3}";

            productCodeTextBox.Text = newUserId;
            productsBindingSource.DataSource = db.Products.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (priceNumericUpDown.Value < 1)
            {
                Alert.Error("Price must be more than zero!");
                return;
            }

            if (string.IsNullOrEmpty(productNameTextBox.Text))
            {
                Alert.Error("Plase fill all input fields!");
                return;
            }

            if (bindingSource1.Current is Products products)
            {
                products.ProductCode = productCodeTextBox.Text;

                db.Products.AddOrUpdate(products);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);

                productNameTextBox.ReadOnly = false;
                priceNumericUpDown.ReadOnly = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (productsDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to edit!");
                return;
            }

            productNameTextBox.ReadOnly = false;
            priceNumericUpDown.ReadOnly = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (productsDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to delete!");
                return;
            }

            var searchItem = db.Products.Find(productCodeTextBox.Text);

            if (Alert.Confirm($"Are you sure to delete '{searchItem.ProductName}'?") == DialogResult.Yes)
            {
                db.Products.Remove(searchItem);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);

                productNameTextBox.ReadOnly = false;
                priceNumericUpDown.ReadOnly = false;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OnLoad(EventArgs.Empty);
            productNameTextBox.ReadOnly = false;
            priceNumericUpDown.ReadOnly = false;
        }

        private void productsDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (productsDataGridView.Rows[e.RowIndex].DataBoundItem is Products products)
            {
                var data = db.Products.AsNoTracking().FirstOrDefault(f => f.ProductCode == products.ProductCode);

                bindingSource1.DataSource = data;
                productCodeTextBox.Text = data.ProductCode;

                productNameTextBox.ReadOnly = true;
                priceNumericUpDown.ReadOnly = true;
            }
        }
    }
}
