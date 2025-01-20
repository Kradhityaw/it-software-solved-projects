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
    public partial class Form3 : Form
    {
        MotorcycleRepairEntities db = new MotorcycleRepairEntities();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            bindingSource1.AddNew();

            var dateTimeNow = DateTime.Now.ToString("yy");
            var getCurrentUserFirst = db.Users.OrderByDescending(f => f.UserCode).FirstOrDefault();
            var currentUser = $"USR-{dateTimeNow}-{getCurrentUserFirst.UserCode.Substring(7)}";
            var getCurrentUser = db.Users.OrderByDescending(f => f.UserCode).FirstOrDefault(f => f.UserCode == currentUser)?.UserCode ?? $"USR-{dateTimeNow}-00";
            var incrementId = Convert.ToInt32(getCurrentUser.Substring(7)) + 1;
            var newUserId = $"USR-{dateTimeNow}-{incrementId:D2}";

            textBox1.Text = newUserId;
            usersBindingSource.DataSource = db.Users.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (userNameTextBox.Text.Length < 3 || userNameTextBox.Text.Length > 25)
            {
                Alert.Error("Name of the user minimum 3 characters and maximum 25 characters");
                return;
            }

            if (string.IsNullOrEmpty(userPasswordTextBox.Text))
            {
                Alert.Error("User password must be filled!");
                return;
            }

            if (bindingSource1.Current is Users users)
            {
                users.UserCode = textBox1.Text;

                db.Users.AddOrUpdate(users);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);
                userNameTextBox.ReadOnly = false;
                userPasswordTextBox.ReadOnly = false;
            }
        }

        private void usersDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (usersDataGridView.Rows[e.RowIndex].DataBoundItem is Users users)
            {
                var data = db.Users.AsNoTracking().FirstOrDefault(f => f.UserCode == users.UserCode);

                bindingSource1.DataSource = data;
                textBox1.Text = data.UserCode;

                userNameTextBox.ReadOnly = true;
                userPasswordTextBox.ReadOnly = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OnLoad(EventArgs.Empty);
            userNameTextBox.ReadOnly = false;
            userPasswordTextBox.ReadOnly = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to edit!");
                return;
            }

            userNameTextBox.ReadOnly = false;
            userPasswordTextBox.ReadOnly = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (usersDataGridView.SelectedRows.Count < 1)
            {
                Alert.Error("Plase select 1 row to delete!");
                return;
            }

            var searchUser = db.Users.Find(textBox1.Text);

            if (Alert.Confirm($"Are you sure to delete '{searchUser.UserName}'?") == DialogResult.Yes)
            {
                db.Users.Remove(searchUser);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);
                userNameTextBox.ReadOnly = false;
                userPasswordTextBox.ReadOnly = false;
            }
        }
    }
}
