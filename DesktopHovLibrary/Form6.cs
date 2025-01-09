using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopHovLibrary
{
    public partial class Form6 : Form
    {
        HovLibraryEntities db = new HovLibraryEntities();

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            var bookDetail = db.BookDetails.Where(f => f.deleted_at == null).Select(f => f.Book.title).ToList();
            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(bookDetail.ToArray());

            textBox1.AutoCompleteCustomSource = autoComplete;

            var memberDetail = db.Member.Where(f => f.deleted_at == null).Select(f => f.name).ToList();
            var memberAutoComplete = new AutoCompleteStringCollection();
            memberAutoComplete.AddRange(memberDetail.ToArray());

            textBox2.AutoCompleteCustomSource = memberAutoComplete;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!db.Member.Any(f => f.name == textBox2.Text))
            {
                Alert.Error("Member not found!");
                return;
            }

            var getMember = db.Member.FirstOrDefault(f => f.name == textBox2.Text);

            for (int i = 0; i < bookDetailsDataGridView.RowCount; i++)
            {
                if (Convert.ToBoolean(bookDetailsDataGridView.Rows[i].Cells[actionCol.Index]?.Value ?? false) == true)
                {
                    if (bookDetailsDataGridView.Rows[i].DataBoundItem is BookDetails book)
                    {
                        if (db.Borrowing.Any(f => f.return_date == null && f.deleted_at == null && f.bookdetails_id == book.id))
                        {
                            Alert.Error("This book is unavailable!");
                            return;
                        }

                        var borrow = new Borrowing()
                        {
                            member_id = getMember.id,
                            bookdetails_id = book.id,
                            borrow_date = DateTime.Now,
                            return_date = null,
                            fine = null,
                            created_at = DateTime.Now,
                        };

                        db.Borrowing.Add(borrow);
                        db.SaveChanges();
                    }
                }
            }

            Alert.Success("Success to add borrowing!");
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            bookDetailsBindingSource.Clear();
            OnLoad(EventArgs.Empty);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var search = db.BookDetails.AsNoTracking().Where(f => f.Book.title == textBox1.Text).ToList();
                bookDetailsBindingSource.DataSource = search;
            }
        }

        private void bookDetailsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (bookDetailsDataGridView.Rows[e.RowIndex].DataBoundItem is BookDetails book)
            {
                if (e.ColumnIndex == locationCol.Index) e.Value = book.Location.name;
                if (e.ColumnIndex == statusCol.Index)
                {
                    if (db.Borrowing.Any(f => f.bookdetails_id == book.id && f.return_date == null)) e.Value = "Unavailable"; else e.Value = "Available";
                }
            }
        }

        private void bookDetailsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bookDetailsDataGridView.Rows[e.RowIndex].DataBoundItem is BookDetails book)
            {
                if (e.ColumnIndex == actionCol.Index)
                {
                    bookDetailsDataGridView.Rows[e.RowIndex].Cells[actionCol.Index].Value = Convert.ToBoolean(bookDetailsDataGridView.Rows[e.RowIndex].Cells[actionCol.Index]?.Value ?? false) == false ? true : false;
                }
            }
        }
    }
}
