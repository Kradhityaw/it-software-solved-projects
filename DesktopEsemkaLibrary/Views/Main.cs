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

namespace DesktopEsemkaLibrary.Views
{
    public partial class Main : Form
    {
        EsemkaLibraryEntities db = new EsemkaLibraryEntities();

        public Main()
        {
            InitializeComponent();
            textBox1.Text = "Irwin Acheson";

            timer1.Start();

            timer1.Tick += (send, a) =>
            {
                label3.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            };

            Load += (send, a) =>
            {
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Member memberData = db.Members.FirstOrDefault(f => f.name == textBox1.Text);

            if (memberData != null)
            {
                Session.mb = memberData;
                borrowingBindingSource.DataSource = db.Borrowings.Where(f => f.member_id == memberData.id && f.deleted_at == null).ToList();
                if (db.Borrowings.Where(f => f.member_id == Session.mb.id && f.deleted_at == null).Count() < 3) button2.Enabled = true; else button2.Enabled = false;
            }
            else
            {
                MessageBox.Show("Member Not Found!", "Notification");
            }
        }

        private void borrowingDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (borrowingDataGridView.Rows[e.RowIndex].DataBoundItem is Borrowing borrowing)
            {
                var overdue = (DateTime.Now.Date - borrowing?.return_date) ?? TimeSpan.Zero;
                var totalDays = overdue.TotalDays;

                if (DateTime.Now.Date > borrowing?.return_date) borrowingDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                if (totalDays == 0) borrowingDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                if (e.ColumnIndex == titleCol.Index) e.Value = borrowing.Book.title;
                if (e.ColumnIndex == borrowdateCol.Index) e.Value = borrowing.borrow_date.ToString("dd MMMM yyyy");
                if (e.ColumnIndex == duedateCol.Index) e.Value = borrowing.return_date?.ToString("dd MMMM yyyy");
                if (e.ColumnIndex == overdueCol.Index) if (totalDays > 0) e.Value = totalDays; else e.Value = "0";
                if (e.ColumnIndex == actionCol.Index) e.Value = "Return";
            }
        }

        private void borrowingDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (borrowingDataGridView.Rows[e.RowIndex].DataBoundItem is Borrowing borrowing)
            {
                var overdue = (DateTime.Now.Date - borrowing?.return_date) ?? TimeSpan.Zero;
                var totalDays = overdue.TotalDays;

                if (e.ColumnIndex == actionCol.Index)
                {
                    var data = db.Borrowings.AsNoTracking().FirstOrDefault(f => f.id == borrowing.id);
                    data.fine = Convert.ToDecimal(totalDays * 2000);
                    data.deleted_at = DateTime.Now;

                    var book = db.Books.AsNoTracking().FirstOrDefault(f => f.id == borrowing.book_id);
                    book.stock++;

                    db.Books.AddOrUpdate(book);
                    db.Borrowings.AddOrUpdate(data);
                    db.SaveChanges();

                    if (totalDays > 0) MessageBox.Show($"Success return {$"{borrowing.Book.title}"}\nMember needs to pay fine: {(totalDays * 2000).ToString("C2", new CultureInfo("id-ID"))}.", "Notification");

                    borrowingBindingSource.Clear();
                    borrowingBindingSource.DataSource = db.Borrowings.Where(f => f.member_id == Session.mb.id && f.deleted_at == null).ToList();
                    if (db.Borrowings.Where(f => f.member_id == Session.mb.id && f.deleted_at == null).Count() < 3) button2.Enabled = true; else button2.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Borrow().ShowDialog();
            borrowingBindingSource.Clear();
            borrowingBindingSource.DataSource = db.Borrowings.Where(f => f.member_id == Session.mb.id && f.deleted_at == null).ToList();
            if (db.Borrowings.Where(f => f.member_id == Session.mb.id && f.deleted_at == null).Count() < 3) button2.Enabled = true; else button2.Enabled = false;
        }
    }
}
