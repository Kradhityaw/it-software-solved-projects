using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopHovLibrary
{
    public partial class Form7 : Form
    {
        HovLibraryEntities db = new HovLibraryEntities();

        public Form7()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = db.Borrowing.AsQueryable();

            if (comboBox1.SelectedIndex == 0)
            {
                query = db.Borrowing.Where(f => f.borrow_date >= dateTimePicker1.Value && f.borrow_date <= dateTimePicker2.Value && f.return_date == null);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                var minusDay = DateTime.Now.AddDays(-7);
                query = db.Borrowing.Where(f => f.borrow_date >= dateTimePicker1.Value && f.borrow_date <= dateTimePicker2.Value && f.borrow_date <= minusDay && f.return_date == null);
            }
            else
            {
                query = db.Borrowing.Where(f => f.borrow_date >= dateTimePicker1.Value && f.borrow_date <= dateTimePicker2.Value && f.return_date != null);
            }

            borrowingBindingSource.DataSource = query.ToList();
        }

        private void borrowingDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (borrowingDataGridView.Rows[e.RowIndex].DataBoundItem is Borrowing borrowing)
            {
                if (e.ColumnIndex == membernameCol.Index) e.Value = borrowing.Member.name;
                if (e.ColumnIndex == booktitleCol.Index) e.Value = borrowing.BookDetails.Book.title;
                if (e.ColumnIndex == bookcodeCol.Index) e.Value = borrowing.BookDetails.code;
                if (e.ColumnIndex == returnCol.Index) e.Value = "Return";
            }
        }

        private void borrowingDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (borrowingDataGridView.Rows[e.RowIndex].DataBoundItem is Borrowing borrowing)
            {
                if (e.ColumnIndex == returnCol.Index)
                {
                    if (borrowing.return_date is null)
                    {
                        borrowing.return_date = DateTime.Now;
                        db.Borrowing.AddOrUpdate(borrowing);
                        db.SaveChanges();

                        var sumFine = (borrowing.borrow_date.Date - borrowing.return_date.Value.Date).Days * 1000;
                        borrowing.fine = Math.Abs(sumFine);

                        db.Borrowing.AddOrUpdate(borrowing);
                        db.SaveChanges();

                        Alert.Success($"Success return a book, your fine is {Math.Abs(sumFine).ToString("C2", new CultureInfo("id-ID"))}!");
                        OnLoad(EventArgs.Empty);
                    }
                }
            }
        }
    }
}
