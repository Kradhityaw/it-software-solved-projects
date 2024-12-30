using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulasiDesktopW4
{
    public partial class Form3 : Form
    {
        QuizinAjaEntities db = new QuizinAjaEntities();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = Session.us.FullName;

            quizBindingSource.DataSource = db.Quiz.Where(f => f.UserID == Session.us.ID).ToList();
        }

        private void quizDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (quizDataGridView.Rows[e.RowIndex].DataBoundItem is Quiz quiz)
                {
                    if (e.ColumnIndex == deleteCol.Index) e.Value = "Delete";
                    if (e.ColumnIndex == numberofqusetionCol.Index) e.Value = db.Question.Count(f => f.QuizID == quiz.ID).ToString();
                }
            }
            catch { }
        }

        private void quizDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (quizDataGridView.Rows[e.RowIndex].DataBoundItem is Quiz quiz)
            {
                if (e.ColumnIndex == deleteCol.Index)
                {
                    Quiz quiz1 = db.Quiz.Find(quiz.ID);

                    if (Alerts.Confirm("Are you sure to delete this quiz?") == DialogResult.Yes)
                    {
                        db.Quiz.Remove(quiz1);
                        db.SaveChanges();

                        OnLoad(EventArgs.Empty);
                        Alerts.Success("Complete delete quiz!");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Disini emang agak error untung untungan bukanya
                Form5 f = new Form5();
                f.ShowDialog();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            Hide();
        }
    }
}
