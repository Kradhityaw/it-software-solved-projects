using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulasiDesktopW4
{
    public partial class Form7 : Form
    {
        QuizinAjaEntities db = new QuizinAjaEntities();
        private Stopwatch st = new Stopwatch();
        List<Quest> quests;
        int currentIndex = 0;

        public Form7()
        {
            InitializeComponent();
            st.Start();
            timer1.Start();
            label1.Text = Session.pname;
        }

        public class Quest
        {
            public int ID { get; set; }
            public string QuestionText { get; set; }
            public List<string> Options { get; set; }
            public string CorrectAnswer { get; set; }
            public string ParticipantAnswer { get; set; }
        }

        public List<Quest> GetQuestion(int QuizId)
        {
            var quest = new List<Quest>();

            foreach (Question item in db.Question.Where(f => f.QuizID == QuizId).ToList())
            {
                quest.Add(new Quest()
                {
                    ID = item.ID,
                    QuestionText = item.Question1,
                    Options = new List<string>()
                    {
                        item.OptionA,
                        item.OptionB,
                        item.OptionC,
                        item.OptionD
                    },
                    CorrectAnswer = item.CorrectAnswer
                });
            }

            return quest;
        }

        private void LoadQuest(int index)
        {
            var question = quests[index];

            if (question.ParticipantAnswer == null)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }

            label4.Text = question.QuestionText;
            radioButton1.Text = question.Options[0];
            radioButton2.Text = question.Options[1];
            radioButton3.Text = question.Options[2];
            radioButton4.Text = question.Options[3];

            if (question.ParticipantAnswer == question.Options[0])
            {
                radioButton1.Checked = true;
            }
            else if (question.ParticipantAnswer == question.Options[1])
            {
                radioButton2.Checked = true;
            }
            else if (question.ParticipantAnswer == question.Options[2])
            {
                radioButton3.Checked = true;
            }
            else if (question.ParticipantAnswer == question.Options[3])
            {
                radioButton4.Checked = true;
            }
        }

        private void GetAnswer(int index)
        {
            var question = quests[index];

            if (radioButton1.Checked)
            {
                question.ParticipantAnswer = radioButton1.Text;
            }
            else if (radioButton2.Checked)
            {
                question.ParticipantAnswer = radioButton2.Text;
            }
            else if (radioButton3.Checked)
            {
                question.ParticipantAnswer = radioButton3.Text;
            }
            else if (radioButton4.Checked)
            {
                question.ParticipantAnswer = radioButton4.Text;
            }

            if (question.ParticipantAnswer != null)
            {
                var btn = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(f => Convert.ToInt32(f.Tag) == index);
                btn.BackColor = Color.Green;
            }
        }

        private void LoadButton(int totalQuestion)
        {
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < totalQuestion; i++)
            {
                Button btn = new Button()
                {
                    Text = (i + 1).ToString(),
                    Width = 40,
                    Height = 40,
                    Tag = i
                };

                btn.Click += Btn_Click;
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            button1.Text = "Next";

            var button = sender as Button;
            var indexQuest = (int)button.Tag;
            GetAnswer(currentIndex);
            currentIndex = indexQuest;
            LoadQuest(currentIndex);

            if (currentIndex == 0)
            {
                button2.Visible = false;
            }

            if (currentIndex == quests.Count - 1)
            {
                button1.Text = "Finish";
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            button2.Visible = false;
            quests = GetQuestion(Session.qz.ID);
            LoadQuest(currentIndex);
            LoadButton(db.Question.Count(f => f.QuizID == Session.qz.ID));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            GetAnswer(currentIndex);

            if (currentIndex == quests.Count - 1)
            {
                foreach (Quest q in quests)
                {
                    if (q.ParticipantAnswer == null)
                    {
                        Alerts.Error("Please answer all question!");
                        return;
                    }
                }

                if (Alerts.Confirm("Are you sure submit your answer?") == DialogResult.Yes)
                {
                    Participant p = new Participant()
                    {
                        QuizID = Session.qz.ID,
                        ParticipantNickname = Session.pname,
                        ParticipationDate = DateTime.Now - st.Elapsed,
                        TimeTaken = st.Elapsed.Seconds
                    };

                    db.Participant.Add(p);
                    db.SaveChanges();

                    List<ParticipantAnswer> pa = new List<ParticipantAnswer>();

                    var getId = db.ParticipantAnswer.OrderByDescending(f => f.ID).FirstOrDefault().ID;

                    quests.ForEach(f =>
                    {
                        getId++;

                        pa.Add(new ParticipantAnswer()
                        {
                            ID = getId,
                            ParticipantID = p.ID,
                            Answer = f.ParticipantAnswer,
                            QuestionID = f.ID
                        });
                    });

                    db.ParticipantAnswer.AddRange(pa.ToList());

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        foreach (var eve in ex.EntityValidationErrors)
                        {
                            Console.WriteLine($"Entity: {eve.Entry.Entity.GetType().Name}, State: {eve.Entry.State}");
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                            }
                        }

                        MessageBox.Show("Validation error occurred. Check the console output for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    Alerts.Success("Complete submit answer!");
                    new Form1().Show();
                    Hide();
                }
            }

            if (currentIndex < quests.Count - 1)
            {
                currentIndex++;
                LoadQuest(currentIndex);
            }

            if (currentIndex == quests.Count - 1)
            {
                button1.Text = "Finish";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Text = "Next";

            if (currentIndex > 0)
            {
                GetAnswer(currentIndex);
                currentIndex--;
                LoadQuest(currentIndex);

                if (currentIndex == 0)
                {
                    button2.Visible = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = $"Time elapsed: {st.Elapsed.Hours:D2}:{st.Elapsed.Minutes:D2}:{st.Elapsed.Seconds:D2}";
        }

        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Form1().Show();
            Hide();
        }
    }
}
