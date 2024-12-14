using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopFoodCourt.Views
{
    public partial class ManageMenu : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();
        public ManageMenu()
        {
            InitializeComponent();
            priceNumericUpDown.Maximum = Decimal.MaxValue;
        }

        private void EnableInputs()
        {
            descriptionTextBox.Enabled = true;
            nameTextBox.Enabled = true;
            comboBox1.Enabled = true;
            priceNumericUpDown.Enabled = true;

            button4.Enabled = true;
            button5.Enabled = true;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void DisableInputs()
        {
            descriptionTextBox.Enabled = false;
            nameTextBox.Enabled = false;
            comboBox1.Enabled = false;
            priceNumericUpDown.Enabled = false;

            button4.Enabled = false;
            button5.Enabled = false;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void ManageMenu_Load(object sender, EventArgs e)
        {
            categoryBindingSource.DataSource = db.Categories.ToList();
            menuBindingSource.DataSource = db.Menus.ToList();
            bindingSource1.AddNew();
            menuDataGridView.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableInputs();
            OnLoad(EventArgs.Empty);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OnLoad(EventArgs.Empty);
            DisableInputs();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (priceNumericUpDown.Value < 1)
            {
                Alerts.Error("Minimal price is 1!");
                return;
            }

            if (string.IsNullOrEmpty(nameTextBox.Text) || string.IsNullOrEmpty(descriptionTextBox.Text))
            {
                Alerts.Error("All inputs must be filled!");
                return;
            }

            if (bindingSource1.Current is Menu menu)
            {
                menu.CategoryID = (int)comboBox1.SelectedValue;

                db.Menus.AddOrUpdate(menu);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);
                DisableInputs();
            }
        }

        private void menuDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (menuDataGridView.Rows[e.RowIndex].DataBoundItem is Menu menu)
            {
                if (e.ColumnIndex == catCol.Index) e.Value = menu.Category.Name;
                if (e.ColumnIndex == priceCol.Index) e.Value = menu.Price.ToString("C2", new CultureInfo("id-ID"));
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            var menuSearch = db.Menus.AsQueryable();

            if (!string.IsNullOrEmpty(textBox1.Text)) menuSearch = db.Menus.Where(f => f.Name.Contains(textBox1.Text) || f.Category.Name.Contains(textBox1.Text));

            menuBindingSource.DataSource = menuSearch.ToList();
            menuDataGridView.ClearSelection();
        }

        private void menuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (menuDataGridView.Rows[e.RowIndex].DataBoundItem is Menu menu)
                {
                    var getMenu = db.Menus.AsNoTracking().FirstOrDefault(f => f.ID == menu.ID);

                    bindingSource1.DataSource = getMenu;
                    comboBox1.SelectedValue = getMenu.CategoryID;
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (menuDataGridView.SelectedRows.Count == 0)
            {
                Alerts.Error("Please select a menu!");
                return;
            }
            EnableInputs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (menuDataGridView.SelectedRows.Count == 0)
            {
                Alerts.Error("Please select a menu!");
                return;
            }

            if (Alerts.Confirmation("Are you sure deleted this menu?") == DialogResult.Yes)
            {
                if (bindingSource1.Current is Menu menu)
                {
                    try
                    {
                        var data = db.Menus.FirstOrDefault(f => f.ID == menu.ID);

                        db.Menus.Remove(data);
                        db.SaveChanges();

                        OnLoad(EventArgs.Empty);
                    }
                    catch
                    {
                        Alerts.Error("Cannot deleted this menu!");
                    }
                }
            }
            else
            {
                OnLoad(EventArgs.Empty);
            }
        }

        private void ManageMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Program.AppContex(new AdminMain());
        }
    }
}
