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
    public partial class Form5 : Form
    {
        QuizinAjaEntities db = new QuizinAjaEntities();

        public Form5()
        {
            InitializeComponent();
        }

        void GetData()
        {
            try
            {
                var selectedIdQuiz = (int)comboBox1.SelectedValue;

                if (db.Participant.Where(f => f.QuizID == selectedIdQuiz).Count() > 0)
                {
                    var avgTimeTaken = db.Participant.Where(f => f.QuizID == selectedIdQuiz).Average(f => f.TimeTaken);
                    var convertToTimeSpan = TimeSpan.FromSeconds(avgTimeTaken);
                    label3.Text = $"Average Time Taken : {convertToTimeSpan.Hours:D2}:{convertToTimeSpan.Minutes:D2}:{convertToTimeSpan.Seconds:D2}";

                    var totalParticipant = db.Participant.Count(f => f.QuizID == selectedIdQuiz);
                    label5.Text = $"Total Participant : {totalParticipant} participant(s)";

                    decimal correctAnswer = db.ParticipantAnswer.Count(f => f.Answer == f.Question.CorrectAnswer && f.Question.QuizID == selectedIdQuiz);
                    decimal totalQuestion = db.Question.Count(f => f.QuizID == selectedIdQuiz) * totalParticipant;
                    var sum = (correctAnswer / totalQuestion) * decimal.Parse("1.00");
                    label4.Text = $"Average Correct Percentage : {Convert.ToInt32(sum)}%";

                    participantBindingSource.DataSource = db.Participant.Where(f => f.QuizID == selectedIdQuiz).ToList();
                }
                else
                {
                    label4.Text = $"Average Correct Percentage : 0%";
                    label3.Text = $"Average Time Taken : 00:00:00";
                    label5.Text = $"Total Participant : 0 participant(s)";
                    participantBindingSource.Clear();
                }
            }
            catch { }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                List<Quiz> q = db.Quiz.ToList();
                quizBindingSource.DataSource = q;

                GetData();
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void participantDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (participantDataGridView.Rows[e.RowIndex].DataBoundItem is Participant participant)
                {
                    if (e.ColumnIndex == timetakenCol.Index)
                    {
                        TimeSpan time = TimeSpan.FromSeconds(participant.TimeTaken);
                        e.Value = $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
                    }
                    if (e.ColumnIndex == correctpercentageCol.Index)
                    {
                        decimal correctAnswer = db.ParticipantAnswer.Count(f => f.Answer == f.Question.CorrectAnswer && f.Question.QuizID == participant.QuizID && f.ParticipantID == participant.ID);
                        decimal totalQuestion = db.Question.Count(f => f.QuizID == participant.QuizID);
                        decimal sum = (correctAnswer / totalQuestion) * decimal.Parse("1.00");

                        e.Value = $"{Convert.ToInt32(sum)}%";
                    }
                }
            }
            catch { }
        }
    }
}
