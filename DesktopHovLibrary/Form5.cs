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

namespace DesktopHovLibrary
{
    public partial class Form5 : Form
    {
        HovLibraryEntities db = new HovLibraryEntities();
        Book _book;

        public Form5(Book book)
        {
            InitializeComponent();
            _book = book;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            locationBindingSource.DataSource = db.Location.Where(f => f.deleted_at == null).ToList();
            bookDetailsBindingSource.DataSource = db.BookDetails.Where(f => f.deleted_at == null && f.book_id == _book.id).ToList();

            var getLastId = db.BookDetails.OrderByDescending(f => f.id).Where(f => f.deleted_at == null).FirstOrDefault().id.ToString("D4");
            textBox2.Text = $"{getLastId}.{_book.id:D4}.{(int)comboBox1.SelectedValue:D2}.{DateTime.Now.Year}";
        }

        private void bookDetailsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (bookDetailsDataGridView.Rows[e.RowIndex].DataBoundItem is BookDetails details)
            {
                if (e.ColumnIndex == locationCol.Index) e.Value = details.Location.name;
                if (e.ColumnIndex == statusCol.Index)
                {
                    if (db.Borrowing.Any(f => f.bookdetails_id == details.id && f.return_date == null)) e.Value = "Unavailable"; else e.Value = "Available";
                }
                if (e.ColumnIndex == deleteCol.Index) e.Value = "Delete";
            }
        }

        private void bookDetailsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bookDetailsDataGridView.Rows[e.RowIndex].DataBoundItem is BookDetails details)
            {
                if (e.ColumnIndex == deleteCol.Index)
                {
                    if (db.Borrowing.Any(f => f.bookdetails_id == details.id && f.return_date == null))
                    {
                        Alert.Error("Only available book can be deleted!");
                    }
                    else
                    {
                        if (Alert.Confirm("Are you sure to delete this book?") == DialogResult.Yes)
                        {
                            var find = db.BookDetails.Find(details.id);
                            find.deleted_at = DateTime.Now;

                            db.BookDetails.AddOrUpdate(find);
                            db.SaveChanges();

                            OnLoad(EventArgs.Empty);
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var getLastId = db.BookDetails.OrderByDescending(f => f.id).Where(f => f.deleted_at == null).FirstOrDefault().id.ToString("D4");
            textBox2.Text = $"{getLastId}.{_book.id:D4}.{(int)comboBox1.SelectedValue:D2}.{DateTime.Now.Year}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var newBookDetails = new BookDetails()
            {
                book_id = _book.id,
                location_id = (int)comboBox1.SelectedValue,
                code = textBox2.Text,
                created_at = DateTime.Now,
            };

            db.BookDetails.Add(newBookDetails);
            db.SaveChanges();

            Alert.Success("Success add detail book!");
            DialogResult = DialogResult.OK;
        }
    }
}
