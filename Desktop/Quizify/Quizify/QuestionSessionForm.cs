using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Quizify
{
    public partial class QuestionSessionForm : Form
    {
        private int selectedId, totalSeconds, currentQuestion, timeTaken;
        private bool isFinish;

        public QuestionSessionForm(int selectedId)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            LoadData();
            LoadButton();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (totalSeconds > 0)
            {
                totalSeconds--;
                timeTaken++;

                int hours = totalSeconds / 3600;
                int minutes = (totalSeconds % 3600) / 60;
                int seconds = totalSeconds % 60;

                lblCountdown.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            }
            else
            {
                ((Timer)sender).Stop();
                lblCountdown.Text = "00:00:00";
                SaveData();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentQuestion >= 1)
            {
                currentQuestion--;
                InitQuestion(currentQuestion);
                QuestionAnswer();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (btnNext.Text == "Next")
            {
                currentQuestion++;
                InitQuestion(currentQuestion);
                QuestionAnswer();
                return;
            }

            if (Helper.models.Any(a => a.Answer == null))
            {
                "Please completed the question first.".ShowMessage();
                return;
            }

            isFinish = true;
            SaveData();
        }

        private void SaveData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var participant = new Participant
                    {
                        UserID = Helper.user.ID,
                        SubjectID = selectedId,
                        Date = DateTime.Now,
                        TimeTaken = timeTaken / 60,
                    };
                    db.Participants.InsertOnSubmit(participant);
                    db.SubmitChanges();

                    foreach (var i in Helper.models)
                    {
                        if (i.Answer == null) continue;

                        db.ParticipantAnswers.InsertOnSubmit(new ParticipantAnswer
                        {
                            ParticipantID = participant.ID,
                            QuestionID = i.ID,
                            Answer = i.Answer,
                        });
                    }

                    db.SubmitChanges();
                    "Congratulation you have to finished.".ShowMessage();
                    Close();
                    new MainForm(true).Show();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void InitQuestion(int No)
        {
            try
            {
                btnPrevious.Visible = No > Helper.models[0].No;
                btnNext.Text = No == Helper.models.Count() ? "Finish" : "Next";

                panelQuestion.Controls.Clear();
                Helper.model = Helper.models.FirstOrDefault(a => a.No == currentQuestion);
                OptionControl optionForm = new OptionControl();
                panelQuestion.Controls.Add(optionForm);
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            if (gbButton.Visible)
            {
                gbButton.Visible = false;
                return;
            }

            gbButton.Visible = true;
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            currentQuestion = btn.Text.ToInt32();
            InitQuestion(currentQuestion);
            QuestionAnswer();
        }

        private void QuestionSessionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isFinish) return;
            e.Cancel = true;
            "This form cannot be closed. Please complete the question first!".ShowMessage();
        }

        private void LoadButton()
        {
            try
            {
                panelButton.Controls.Clear();
                for (int i = 1; i <= Helper.models.Count; i++)
                {
                    Button btn = new Button
                    {
                        AutoSize = true,
                        Size = new Size(43, 39),
                        Text = i.ToString(),
                        Name = $"ID{i}",
                        BackColor = Color.LightGray,
                    };

                    if (i % 5 == 0)
                    {
                        panelButton.SetFlowBreak(btn, true);
                    }

                    btn.Click += btn_Clicked;
                    panelButton.Controls.Add(btn);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void QuestionAnswer()
        {
            var models = Helper.models.Where(x => x.Answer != null).Select(x => x.No).ToList();
            panelButton.Controls.OfType<Button>().Where(a => models.Contains(a.Text.ToInt32())).ToList().ForEach(a =>
            {
                if (models.Contains(a.Text.ToInt32()))
                    a.BackColor = Color.ForestGreen;
            });
        }

        private void LoadData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var subject = db.Subjects.FirstOrDefault(a => a.ID == selectedId);
                    lblName.Text = subject.Name;
                    lblCountdown.Text = $"{TimeSpan.FromMinutes(subject.Time)}";
                    totalSeconds = subject.Time * 60;

                    var question = db.Questions.Where(a => a.SubjectID == subject.ID).ToList();
                    int no = 0;
                    foreach (var i in question)
                    {
                        Helper.models.Add(new Helper.QuestionModel
                        {
                            ID = i.ID,
                            No = ++no,
                            Question = i.Question1,
                            OptionA = i.OptionA,
                            OptionB = i.OptionB,
                            OptionC = i.OptionC,
                            OptionD = i.OptionD,
                        });
                    }

                    currentQuestion = Helper.models[0].No;
                    InitQuestion(currentQuestion);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }
    }
}
