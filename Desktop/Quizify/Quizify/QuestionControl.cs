using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quizify
{
    public partial class QuestionControl : UserControl
    {
        private int selectedId;
        private Form f;

        public QuestionControl(int selectedId, Form f)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            this.f = f;

            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var subject = db.Subjects.FirstOrDefault(a => a.ID == selectedId);
                    lblName.Text = subject.Name;
                    lblTime.Text = $"Time: {subject.Time} min";
                    lblNumberOfQuestion.Text = $"Number of Question: {subject.Questions.Count()}";

                    var participant = db.Participants.Where(a => a.UserID == Helper.user.ID && a.Date.Date == DateTime.Now.Date).ToList();

                    if (participant.Any(a => a.SubjectID == subject.ID))
                    {
                        btn.Text = "Finish";
                        btn.BackColor = Color.ForestGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (btn.Text == "Finish")
            {
                "You have finished this question.".ShowMessage();
                return;
            }

            if (MessageBox.Show($"You want to start {lblName.Text}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new QuestionSessionForm(selectedId).Show();
                f.Close();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }
    }
}