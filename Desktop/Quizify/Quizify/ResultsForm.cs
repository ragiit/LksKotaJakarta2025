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
    public partial class ResultsForm : Form
    {
        public ResultsForm()
        {
            InitializeComponent();
            InitCombobox();
        }

        private void InitCombobox()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                cbSubject.ValueMember = "ID";
                cbSubject.DisplayMember = "Name";
                cbSubject.DataSource = db.Subjects.Select(a => new { a.ID, a.Name }).ToList();
            }
        }

        private void LoadData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    if (Helper.user.Role == '1')
                    {
                        dataGridView2.DataSource = db.Participants.Where(a => a.Date.Date == dateTimePicker1.Value.Date && a.SubjectID == cbSubject.SelectedValue.ToInt32()).Select(a => new
                        {
                            a.ID,
                            User = a.User.FullName,
                            a.SubjectID,
                            TimeTaken = TimeSpan.FromMinutes(a.TimeTaken),
                            Answered = a.ParticipantAnswers.Count(),
                            Unanswered = a.Subject.Questions.Count() - a.ParticipantAnswers.Count(),
                            Grade = $"{a.ParticipantAnswers.Count(x => x.Answer == x.Question.CorrectAnswer) * 100 / a.Subject.Questions.Count()}",
                        }).AsQueryable();

                        tableLayoutPanel1.Visible = true;
                        dataGridView2.Visible = true;
                        dataGridView1.Visible = false;
                        dataGridView2.Columns["ID"].Visible = false;
                        dataGridView2.Columns["SubjectID"].Visible = false;
                        dataGridView2.ColumnHeadersVisible = !(dataGridView2.RowCount <= 0);
                        return;
                    }

                    dataGridView1.DataSource = db.Participants.Where(a => a.User.ID == Helper.user.ID && a.Date.Date == DateTime.Now.Date).Select(a => new
                    {
                        a.ID,
                        a.SubjectID,
                        Subject = a.Subject.Name,
                        TimeTaken = TimeSpan.FromMinutes(a.TimeTaken),
                        a.Date,
                        Answered = a.ParticipantAnswers.Count(),
                        Unanswered = a.Subject.Questions.Count() - a.ParticipantAnswers.Count(),
                        Grade = $"{a.ParticipantAnswers.Count(x => x.Answer == x.Question.CorrectAnswer) * 100 / a.Subject.Questions.Count()}",
                    }).AsQueryable();


                    dataGridView1.Columns["ID"].Visible = false;
                    dataGridView1.Columns["SubjectID"].Visible = false;
                    dataGridView1.ColumnHeadersVisible = !(dataGridView1.RowCount <= 0);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void cbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    r.Cells["Grade"].Style.BackColor = Color.Green;
                    r.Cells["Grade"].Style.ForeColor = Color.White;
                    r.Cells["Grade"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    if (r.Cells["Grade"].Value.ToInt32() < 75)
                    {
                        r.Cells["Grade"].Style.BackColor = Color.Salmon;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow r in dataGridView2.Rows)
                {
                    r.Cells["Grade"].Style.BackColor = Color.Green;
                    r.Cells["Grade"].Style.ForeColor = Color.White;
                    r.Cells["Grade"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    if (r.Cells["Grade"].Value.ToInt32() < 75)
                    {
                        r.Cells["Grade"].Style.BackColor = Color.Salmon;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }
    }
}
