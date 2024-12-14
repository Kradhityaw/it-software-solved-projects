using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesktopFoodCourt.Views
{
    public partial class ManageMember : Form
    {
        private readonly EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public ManageMember()
        {
            InitializeComponent();
            this.FormClosed += (send, a) =>
            {
                new Program.AppContex(new AdminMain());
            };
        }

        private void EnableInputs()
        {
            firstNameTextBox.Enabled = true;
            lastNameTextBox.Enabled = true;
            emailTextBox.Enabled = true;
            passwordTextBox.Enabled = true;
            phoneNumberTextBox.Enabled = true;

            button4.Enabled = true;
            button5.Enabled = true;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void DisableInputs()
        {
            firstNameTextBox.Enabled = false;
            lastNameTextBox.Enabled = false;
            emailTextBox.Enabled = false;
            passwordTextBox.Enabled = false;
            phoneNumberTextBox.Enabled = false;

            button4.Enabled = false;
            button5.Enabled = false;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            IQueryable<User> search = db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(textBox1.Text)) search = db.Users.Where(f => f.FirstName.Contains(textBox1.Text) || f.LastName.Contains(textBox1.Text) || f.Email.Contains(textBox1.Text));

            userBindingSource.DataSource = search.ToList();
            userDataGridView.ClearSelection();
        }

        private void ManageMember_Load(object sender, EventArgs e)
        {
            userBindingSource.DataSource = db.Users.ToList();
            bindingSource1.AddNew();
            userDataGridView.ClearSelection();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Check if empty
            if (string.IsNullOrEmpty(firstNameTextBox.Text) || string.IsNullOrEmpty(lastNameTextBox.Text) || string.IsNullOrEmpty(emailTextBox.Text) || string.IsNullOrEmpty(phoneNumberTextBox.Text) || string.IsNullOrEmpty(passwordTextBox.Text))
            {
                Alerts.Error("All inputs must be filled!");
                return;
            }

            // Email Correct format
            if (!new EmailAddressAttribute().IsValid(emailTextBox.Text))
            {
                Alerts.Error("Your email is invalid!");
                return;
            }

            // Must be 10 - 15 Digits
            if (phoneNumberTextBox.Text.Length < 10 || phoneNumberTextBox.Text.Length > 15)
            {
                Alerts.Error("Invalid phone number!");
                return;
            }

            // Password must be more than 8 char
            if (passwordTextBox.Text.Length < 8)
            {
                Alerts.Error("Password must more than 8 character!");
                return;
            }

            if (bindingSource1.Current is User user)
            {
                // Unique email
                if (db.Users.Any(f => f.Email == emailTextBox.Text) && user.ID == 0)
                {
                    Alerts.Error("Your email is already exist!");
                    return;
                }

                user.RoleID = 2;
                user.DateJoined = DateTime.Now;

                db.Users.AddOrUpdate(user);
                db.SaveChanges();

                OnLoad(EventArgs.Empty);
                DisableInputs();
            }
        }

        private void phoneNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void userDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (userDataGridView.Rows[e.RowIndex].DataBoundItem is User user)
            {
                if (e.ColumnIndex == membersinceCol.Index)
                {
                    var dateJoined = user.DateJoined.Year;
                    var dateNow = DateTime.Now.Year;
                    var since = dateNow - dateJoined;

                    e.Value = $"{user.DateJoined.ToString("dd/MM/yyyy")} ({since} year(s))";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnableInputs();
            OnLoad(EventArgs.Empty);
        }

        private void userDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (userDataGridView.Rows[e.RowIndex].DataBoundItem is User user)
                {
                    var getData = db.Users.AsNoTracking().FirstOrDefault(f => f.ID == user.ID);
                    bindingSource1.DataSource = getData;
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (userDataGridView.SelectedRows.Count == 0)
            {
                Alerts.Error("Please select a user!");
                return;
            }
            EnableInputs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (userDataGridView.SelectedRows.Count == 0)
            {
                Alerts.Error("Please select a user!");
                return;
            }

            if (Alerts.Confirmation("Are you sure deleted this user?") == DialogResult.Yes)
            {
                if (bindingSource1.Current is User user)
                {
                    var dataDelete = db.Users.FirstOrDefault(f => f.ID == user.ID);

                    db.Users.Remove(dataDelete);
                    db.SaveChanges();

                    OnLoad(EventArgs.Empty);
                }
            }
            else
            {
                OnLoad(EventArgs.Empty);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OnLoad(EventArgs.Empty);
            DisableInputs();
        }
    }
}
