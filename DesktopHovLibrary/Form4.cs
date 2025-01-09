using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopHovLibrary
{
    public partial class Form4 : Form
    {
        HovLibraryEntities db = new HovLibraryEntities();
        IQueryable<Book> books;

        public Form4()
        {
            InitializeComponent();
            numericUpDown3.DecimalPlaces = 2;
            numericUpDown4.DecimalPlaces = 2;
            numericUpDown1.Maximum = decimal.MaxValue;
            numericUpDown2.Maximum = decimal.MaxValue;
            number_of_pagesNumericUpDown.Maximum = decimal.MaxValue;
            ratings_countNumericUpDown.Maximum = decimal.MaxValue;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (groupBox3.Controls.OfType<TextBox>().Any(f => string.IsNullOrEmpty(f .Text)))
            {
                Alert.Error("Please fill all inputs!");
                return;
            }

            if (groupBox3.Controls.OfType<NumericUpDown>().Any(f => f.Value < 1))
            {
                Alert.Error("Please fill all inputs!");
                return;
            }

            if (bindingSource1.Current is Book book)
            {
                book.language_id = (int)comboBox3.SelectedValue;
                book.publisher_id = (int)comboBox4.SelectedValue;

                db.Book.AddOrUpdate(book);
                db.SaveChanges();

                groupBox3.Enabled = false;
                OnLoad(EventArgs.Empty);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            bookBindingSource.DataSource = db.Book.Where(f => f.deleted_at == null).ToList();
            languageBindingSource.DataSource = db.Language.Where(f => f.deleted_at == null).ToList();
            languageBindingSource1.DataSource = db.Language.Where(f => f.deleted_at == null).ToList();
            publisherBindingSource.DataSource = db.Publisher.Where(f => f.deleted_at == null).ToList();
            bindingSource1.AddNew();
            comboBox1.SelectedIndex = 0;
        }

        private void bookDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (bookDataGridView.Rows[e.RowIndex].DataBoundItem is Book book)
            {
                if (e.ColumnIndex == editCol.Index) e.Value = "Edit";
                if (e.ColumnIndex == deleteCol.Index) e.Value = "Delete";
                if (e.ColumnIndex == booklistCol.Index) e.Value = "Show";
                if (e.ColumnIndex == languageCol.Index) e.Value = book.Language.long_text;
                if (e.ColumnIndex == publisherCol.Index) e.Value = book.Publisher.name;
                if (e.ColumnIndex == ratingcol.Index) e.Value = $"{book.average_rating} ({book.ratings_count})";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            books = db.Book.AsQueryable();

            if (comboBox1.SelectedIndex == 0)
            {
                books = db.Book.Where(f => f.title.Contains(textBox1.Text));
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                books = db.Book.Where(f => f.authors.Contains(textBox1.Text));
            }
            else
            {
                books = db.Book.Where(f => f.Publisher.name.Contains(textBox1.Text));
            }

            bookBindingSource.DataSource = books.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            books = db.Book.AsQueryable();

            books = db.Book.Where(f => f.language_id == (int)comboBox2.SelectedValue);

            if (dateTimePicker2.Value > dateTimePicker1.Value)
            {
                books = db.Book.Where(f => f.language_id == (int)comboBox2.SelectedValue && f.publication_date >= dateTimePicker1.Value && f.publication_date <= dateTimePicker2.Value);
            }

            if (dateTimePicker2.Value > dateTimePicker1.Value && numericUpDown2.Value > numericUpDown1.Value)
            {
                books = db.Book.Where(f => f.language_id == (int)comboBox2.SelectedValue && f.publication_date >= dateTimePicker1.Value && f.publication_date <= dateTimePicker2.Value && f.number_of_pages >= numericUpDown1.Value && f.number_of_pages <= numericUpDown2.Value);
            }

            if (dateTimePicker2.Value > dateTimePicker1.Value && numericUpDown2.Value > numericUpDown1.Value && numericUpDown3.Value > numericUpDown4.Value)
            {
                books = db.Book.Where(f => f.language_id == (int)comboBox2.SelectedValue && f.publication_date >= dateTimePicker1.Value && f.publication_date <= dateTimePicker2.Value && f.number_of_pages >= numericUpDown1.Value && f.number_of_pages <= numericUpDown2.Value && f.average_rating >= (double)numericUpDown4.Value && f.average_rating <= (double)numericUpDown3.Value);
            }

            bookBindingSource.DataSource = books.ToList();
        }

        private void bookDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bookDataGridView.Rows[e.RowIndex].DataBoundItem is Book book)
            {
                if (e.ColumnIndex == editCol.Index)
                {
                    var dataBook = db.Book.AsNoTracking().FirstOrDefault(f => f.id == book.id);
                    bindingSource1.DataSource = dataBook;
                    comboBox3.SelectedValue = book.language_id;
                    comboBox4.SelectedValue = book.publisher_id;

                    groupBox3.Enabled = true;
                }
                if (e.ColumnIndex == deleteCol.Index)
                {
                    if (Alert.Confirm("Are youy sure delete this book?") == DialogResult.Yes)
                    {
                        var find = db.Book.Find(book.id);
                        find.deleted_at = DateTime.Now;

                        db.Book.AddOrUpdate(find);
                        db.SaveChanges();

                        OnLoad(EventArgs.Empty);
                    }
                }
                if (e.ColumnIndex == booklistCol.Index)
                {
                    var form = new Form5(book);
                    form.ShowDialog();
                }
                //if (e.ColumnIndex == languageCol.Index) e.Value = book.Language.long_text;
                //if (e.ColumnIndex == publisherCol.Index) e.Value = book.Publisher.name;
                //if (e.ColumnIndex == ratingcol.Index) e.Value = $"{book.average_rating} ({book.ratings_count})";
            }
        }
    }
}
