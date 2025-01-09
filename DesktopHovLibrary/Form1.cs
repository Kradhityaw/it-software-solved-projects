using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopHovLibrary
{
    public partial class Form1 : Form
    {
        HovLibraryEntities db = new HovLibraryEntities();

        public Form1()
        {
            InitializeComponent();
        }

        private string HashPassword(string password)
        {
            var sha256 = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            return string.Concat(sha256.Select(b => b.ToString("x2")));
        }
            
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "ghuriche0@skyrock.com";
            textBox2.Text = "admin1";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var password = HashPassword(textBox2.Text);

            var employeeData = db.Employee.FirstOrDefault(f => f.email == textBox1.Text && f.password == password);

            if (employeeData != null)
            {
                Session.em = employeeData;

                new Form2().Show();
                Hide();
            }
            else
            {
                Alert.Error("Email or password is incorrect!");
            }
        }
    }
}
