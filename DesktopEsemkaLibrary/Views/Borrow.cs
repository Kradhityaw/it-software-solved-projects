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

namespace DesktopEsemkaLibrary.Views
{
    public partial class Borrow : Form
    {
        EsemkaLibraryEntities db = new EsemkaLibraryEntities();

        public Borrow()
        {
            InitializeComponent();
        }

        private void Borrow_Load(object sender, EventArgs e)
        {
            bookBindingSource.DataSource = db.Books.Where(f =>
                f.deleted_at == null
            ).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bookBindingSource.DataSource = db.Books.Where(f =>
                f.deleted_at == null &&
                f.title.Contains(textBox1.Text)
            ).ToList();
        }

        private void bookDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (bookDataGridView.Rows[e.RowIndex].DataBoundItem is Book book)
            {
                
                if (e.ColumnIndex == genreCol.Index)
                {
                    var genres = "";

                    foreach (BookGenre item in db.BookGenres.Where(f => f.book_id == book.id && f.deleted_at == null).ToList())
                    {
                        genres += $"{item.Genre.name}, ";
                    }

                    e.Value = genres;
                }
                if (e.ColumnIndex == publishdateCol.Index) e.Value = book.publish_date?.ToString("dd MMMM yyyy");
                if (e.ColumnIndex == actionCol.Index)
                {
                    if (book.stock == 0)
                    {
                        bookDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        e.Value = "";
                    }
                    else e.Value = "Borrow";
                }
            }
        }

        private void bookDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bookDataGridView.Rows[e.RowIndex].DataBoundItem is Book book)
            {
                if (e.ColumnIndex == actionCol.Index)
                {
                    if (book.stock == 0)
                    {
                        return;
                    }
                    else
                    {
                        Borrowing borrow = new Borrowing()
                        {
                            book_id = book.id,
                            member_id = Session.mb.id,
                            borrow_date = DateTime.Now,
                            return_date = DateTime.Now.AddDays(7),
                            created_at = DateTime.Now
                        };

                        var dataBook = db.Books.AsNoTracking().FirstOrDefault(f => f.id == book.id);
                        dataBook.stock -= 1;

                        db.Books.AddOrUpdate(dataBook);
                        db.Borrowings.Add(borrow);
                        db.SaveChanges();

                        MessageBox.Show($"Success borrow {$"{book.title}"}\nDue date is 7 days from today.");

                        Close();
                    }
                }
            }
        }
    }
}
