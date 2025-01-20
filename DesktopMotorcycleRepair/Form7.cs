using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopMotorcycleRepair
{
    public partial class Form7 : Form
    {
        MotorcycleRepairEntities db = new MotorcycleRepairEntities();

        public Form7()
        {
            InitializeComponent();
        }

        private void ClearForm()
        {
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;

            motorcycleServicesBindingSource.Clear();
            productsBindingSource.Clear();

            textBox11.Text = string.Empty;
            textBox10.Text = string.Empty;
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            var getCurrentTransaction = db.TransactionService.OrderByDescending(f => f.TransactionNumber).FirstOrDefault();
            var format = $"T{(getCurrentTransaction != null ? getCurrentTransaction.TransactionNumber.Substring(1,3) : "000")}{DateTime.Now:MM}{DateTime.Now:yyyy}";
            var find = db.TransactionService.OrderByDescending(f => f.TransactionNumber).FirstOrDefault(f => f.TransactionNumber == format)?.TransactionNumber ?? format;
            var convert = Convert.ToInt32(find.Substring(1, 3)) + 1;
            var realizeId = $"T{convert:D3}{DateTime.Now:MM}{DateTime.Now:yyyy}";
            var dateTransaction = DateTime.Now.ToString("dd MMM yyyy");

            textBox1.Text = realizeId;
            textBox2.Text = dateTransaction;

            mechanicsBindingSource.DataSource = db.Mechanics.ToList();
        }

        private void motorcycleServicesDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (motorcycleServicesDataGridView.Rows[e.RowIndex].DataBoundItem is MotorcycleServices services)
                {
                    if (e.ColumnIndex == codeCol.Index)
                    {
                        var value = motorcycleServicesDataGridView.Rows[e.RowIndex].Cells[codeCol.Index].Value.ToString();
                        var data = db.MotorcycleServices.FirstOrDefault(f => f.ServiceCode == value);
                        if (data != null)
                        {
                            motorcycleServicesBindingSource.RemoveCurrent();
                            motorcycleServicesBindingSource.Add(data);

                            textBox5.Text = motorcycleServicesBindingSource.List.Count.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Data tidak ditemukan");
                        }
                    }
                }
            }
            catch { }
        }

        private void motorcycleServicesBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            var price = 0;

            for (int i = 0; i < motorcycleServicesBindingSource.List.Count; i++)
            {
                price += Convert.ToInt32(motorcycleServicesDataGridView.Rows[i].Cells[costCol.Index].Value);
            }

            textBox6.Text = price.ToString();
            textBox9.Text = (Convert.ToInt32(textBox6.Text) + Convert.ToInt32(textBox7.Text)).ToString();
        }

        private void motorcycleServicesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (motorcycleServicesDataGridView.Rows[e.RowIndex].DataBoundItem is MotorcycleServices services)
            {
                if (e.ColumnIndex == codeCol.Index)
                {
                    motorcycleServicesBindingSource.Remove(services);
                }
            }
        }

        private void motorcycleServicesDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            textBox5.Text = motorcycleServicesBindingSource.List.Count.ToString();
        }

        private void productsDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (productsDataGridView.Rows[e.RowIndex].DataBoundItem is Products products)
                {
                    if (e.ColumnIndex == productCodeCol.Index)
                    {
                        var value = productsDataGridView.Rows[e.RowIndex].Cells[productCodeCol.Index].Value.ToString();
                        var data = db.Products.FirstOrDefault(f => f.ProductCode == value);
                        if (data != null)
                        {
                            productsBindingSource.RemoveCurrent();
                            productsBindingSource.Add(data);

                            productsDataGridView.Rows[e.RowIndex].Cells[amountCol.Index].Value = 1;
                            productsDataGridView.Rows[e.RowIndex].Cells[totalCol.Index].Value = data.Price;
                            textBox8.Text = productsBindingSource.List.Count.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Data tidak ditemukan");
                        }
                    }
                    if (e.ColumnIndex == amountCol.Index)
                    {
                        var amount = Convert.ToInt32(productsDataGridView.Rows[e.RowIndex].Cells[amountCol.Index].Value);

                        productsDataGridView.Rows[e.RowIndex].Cells[totalCol.Index].Value = products.Price * amount;
                    }
                }
            }
            catch { }
        }

        private void productsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (productsDataGridView.Rows[e.RowIndex].DataBoundItem is Products products)
            {
                if (e.ColumnIndex == productCodeCol.Index)
                {
                    productsBindingSource.Remove(products);
                }
            }
        }

        private void productsBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            var price = 0;

            for (int i = 0; i < productsBindingSource.List.Count; i++)
            {
                price += Convert.ToInt32(productsDataGridView.Rows[i].Cells[totalCol.Index].Value);
            }

            textBox7.Text = price.ToString();
            textBox9.Text = (Convert.ToInt32(textBox6.Text) + Convert.ToInt32(textBox7.Text)).ToString();
        }

        private void productsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            textBox8.Text = productsBindingSource.List.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var check = string.IsNullOrEmpty(textBox10.Text) ? 0 : Convert.ToInt32(textBox10.Text);

            if (check < Convert.ToInt32(textBox9.Text))
            {
                Alert.Error("The Paid textbox is typed by the user whose value cannot be less than the Total Charge");
                return;
            }

            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || motorcycleServicesDataGridView.RowCount < 1 || productsDataGridView.RowCount < 1)
            {
                Alert.Error("Please fill all input field!");
                return;
            }

            TransactionService transactionService = new TransactionService()
            {
                TransactionNumber = textBox1.Text,
                TransactionDate = DateTime.Now,
                PoliceRegistrationNumber = textBox3.Text,
                Damage = textBox4.Text,
                TotalServiceCost = Convert.ToInt32(textBox6.Text),
                TotalProductPrice = Convert.ToInt32(textBox7.Text),
                TotalCharge = Convert.ToInt32(textBox9.Text),
                ChangeMoney = Convert.ToInt32(textBox11.Text),
                Paid = Convert.ToInt32(textBox10.Text),
                UserCode = Session.usr.UserCode,
                MechanicCode = (string)comboBox1.SelectedValue,
            };

            db.TransactionService.Add(transactionService);
            db.SaveChanges();

            foreach (MotorcycleServices item in motorcycleServicesBindingSource.List)
            {
                DetailService detailService = new DetailService()
                {
                    TransactionNumber = textBox1.Text,
                    ServiceCode = item.ServiceCode,
                    Cost = item.Cost,
                };

                db.DetailService.Add(detailService);
                db.SaveChanges();
            }

            for (int i = 0; i < productsBindingSource.List.Count; i++)
            {
                DetailProduct detail = new DetailProduct()
                {
                    TransactionNumber = textBox1.Text,
                    ProductCode = Convert.ToString(productsDataGridView.Rows[i].Cells[productCodeCol.Index].Value),
                    Price = Convert.ToInt32(productsDataGridView.Rows[i].Cells[productPriceCol.Index].Value),
                };

                detail.Amount = Convert.ToInt32(productsDataGridView.Rows[i].Cells[amountCol.Index].Value);
                detail.Total = Convert.ToInt32(productsDataGridView.Rows[i].Cells[totalCol.Index].Value);

                db.DetailProduct.Add(detail);
                db.SaveChanges();
            }

            Alert.Success("Complete Transaction");
            OnLoad(EventArgs.Empty);
            ClearForm();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            var get = (string.IsNullOrEmpty(textBox10.Text) ? 0 : Convert.ToInt32(textBox10.Text));
            textBox11.Text = (get - Convert.ToInt32(textBox9.Text)).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
