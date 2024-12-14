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

namespace DesktopFoodCourt.Views
{
    public partial class ManageMenuIngredients : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        private List<MenuIngredient> deletedList = new List<MenuIngredient>();

        public ManageMenuIngredients()
        {
            InitializeComponent();
        }

        private void EnableState()
        {
            groupBox1.Enabled = true;
        }

        private void DisableState()
        {
            groupBox1.Enabled = false;
        }

        private void ManageMenuIngredients_Load(object sender, EventArgs e)
        {
            menuBindingSource.DataSource = db.Menus.ToList();
            ingredientBindingSource.DataSource = db.Ingredients.ToList();
            unitBindingSource.DataSource = db.Units.ToList();
        }

        private void menuDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (menuDataGridView.Rows[e.RowIndex].DataBoundItem is Menu menu)
            {
                if (e.ColumnIndex == actCol.Index) e.Value = "Edit Ingredients";
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            var getMenu = db.Menus.AsQueryable();

            if (!string.IsNullOrEmpty(textBox1.Text)) getMenu = db.Menus.Where(f => f.Name.Contains(textBox1.Text));

            menuBindingSource.DataSource = getMenu.ToList();
        }

        private void menuDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (menuDataGridView.Rows[e.RowIndex].DataBoundItem is Menu menu)
            {
                if (e.ColumnIndex == actCol.Index)
                {
                    EnableState();
                    var getData = db.MenuIngredients.Where(f => f.MenuID == menu.ID).ToList();
                    menuIngredientBindingSource.DataSource = getData;
                }
            }
        }

        private void menuIngredientDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (menuIngredientDataGridView.Rows[e.RowIndex].DataBoundItem is MenuIngredient menu)
                {
                    if (e.ColumnIndex == ingCol.Index) e.Value = menu?.Ingredient?.Name;
                    if (e.ColumnIndex == untCol.Index) e.Value = menu?.Unit?.Name;
                    if (e.ColumnIndex == act2Col.Index) e.Value = "Remove";
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (MenuIngredient m in deletedList.ToList())
            {
                if (m.ID == 0)
                {
                    deletedList.Remove(m);
                }
            }

            db.MenuIngredients.RemoveRange(deletedList);

            foreach (MenuIngredient m in menuIngredientBindingSource.List)
            {
                db.MenuIngredients.AddOrUpdate(m);
            }

            db.SaveChanges();

            OnLoad(EventArgs.Empty);
            Alerts.Success("Seccess add data!");

            DisableState();
            menuIngredientBindingSource.Clear();
            deletedList = new List<MenuIngredient>();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OnLoad(EventArgs.Empty);
            menuIngredientBindingSource.Clear();
            DisableState();
            deletedList = new List<MenuIngredient>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 1)
            {
                Alerts.Error("quantity must be more than or equals to 1");
                return;
            }

            if (menuIngredientBindingSource.Current is MenuIngredient menuIngredient)
            {
                MenuIngredient addIngredient = new MenuIngredient
                {
                    Ingredient = comboBox1.SelectedItem as Ingredient,
                    Unit = comboBox2.SelectedItem as Unit,
                    Qty = (int)numericUpDown1.Value,
                    MenuID = 1
                };

                Ingredient checkIngredients = comboBox1.SelectedItem as Ingredient;

                foreach (MenuIngredient c in menuIngredientBindingSource.List)
                {
                    if (checkIngredients.ID == c.Ingredient.ID)
                    {
                        Alerts.Error("Menu can't have duplicate ingredients!");
                        return;
                    }
                }

                menuIngredientBindingSource.Add(addIngredient);
            }
        }

        private void menuIngredientDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (menuIngredientDataGridView.Rows[e.RowIndex].DataBoundItem is MenuIngredient menuin)
                {
                    if (e.ColumnIndex == act2Col.Index)
                    {
                        deletedList.Add(menuin);
                        menuIngredientBindingSource.Remove(menuin);
                    }
                }
            }
            catch { }
        }

        private void ManageMenuIngredients_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Program.AppContex(new AdminMain());
        }
    }
}
