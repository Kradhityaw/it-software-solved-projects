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
    public partial class Form4 : Form
    {
        QuizinAjaEntities db = new QuizinAjaEntities();

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (TextBox t in groupBox1.Controls.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(t.Text))
                {
                    Alerts.Error("Please fill all inputs!");
                    return;
                }
            }

            if (groupBox1.Controls.OfType<RadioButton>().Count(f => f.Checked) == 0)
            {
                Alerts.Error("Plase select a correct answer!");
                return;
            }

            string correctAnswer = "";

            if (radioButton1.Checked) correctAnswer = textBox5.Text;
            else if (radioButton2.Checked) correctAnswer = textBox6.Text;
            else if (radioButton3.Checked) correctAnswer = textBox7.Text;
            else if (radioButton4.Checked) correctAnswer = textBox8.Text;

            Question question = new Question()
            {
                Question1 = textBox4.Text,
                OptionA = textBox5.Text,
                OptionB = textBox6.Text,
                OptionC = textBox7.Text,
                OptionD = textBox8.Text,
                CorrectAnswer = correctAnswer
            };

            questionBindingSource.Add(question);
            groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(f => f.Checked == true).Checked = false;

            foreach (TextBox t in groupBox1.Controls.OfType<TextBox>())
            {
                t.Text = string.Empty;
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
        }

        private void questionDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (questionDataGridView.Rows[e.RowIndex].DataBoundItem is Question question)
                {
                    if (e.ColumnIndex == noCol.Index) e.Value = (e.RowIndex + 1).ToString();
                    if (e.ColumnIndex == deleteCol.Index) e.Value = "Delete";
                }
            }
            catch { }
        }

        private void questionDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (questionDataGridView.Rows[e.RowIndex].DataBoundItem is Question question)
            {
                if (e.ColumnIndex == deleteCol.Index) questionBindingSource.Remove(question);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (char c in textBox2.Text.ToCharArray())
            {
                if (!char.IsControl(c) && !char.IsLetterOrDigit(c))
                {
                    Alerts.Error("Quiz code only uppercase character and number");
                    return;
                }
            }

            if (db.Quiz.Any(f => f.Code == textBox2.Text))
            {
                Alerts.Error("Quiz code already exist!");
                return;
            }

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                Alerts.Error("Please fill all inputs!");
                return;
            }

            if (questionBindingSource.List.Count <= 0)
            {
                Alerts.Error("Quiz must be have at least 1 question!");
                return;
            }

            Quiz quiz = new Quiz()
            {
                Code = textBox2.Text,
                CreatedAt = DateTime.Now,
                Description = textBox3.Text,
                Name = textBox1.Text,
                UserID = Session.us.ID,
            };

            db.Quiz.Add(quiz);
            db.SaveChanges();

            List<Question> q = questionBindingSource.List.Cast<Question>().ToList();
            q.ForEach(a => a.QuizID = quiz.ID);

            db.Question.AddRange(q);
            db.SaveChanges();

            Alerts.Success("Success crate a new Quiz!");

            new Form3().Show();
            Hide();
        }
    }
}
