using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quizify
{
    public partial class DashboardForm : Form
    {
        private int selectedUser;
        private Form f;
        public DashboardForm(Form f)
        {
            InitializeComponent();
            timer1.Start();
            this.f = f;

            if (Helper.user.Role == '2')
            {
                lblName.Text = $"Hey {Helper.user.FullName},";
                panelAdmin.Visible = false;
                flowLayoutPanel1.Visible = true;
                LoadDataStudent();
                return;
            }

            LoadDataAdmin();
        }

        private void LoadDataStudent()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var subjects = db.Subjects.ToList();
                    var participant = db.Participants.Where(a => a.UserID == Helper.user.ID && a.Date.Date == DateTime.Now.Date).ToList();

                    foreach (var i in subjects)
                    {
                        if (participant.Any(a => a.SubjectID == i.ID)) continue;
                        QuestionControl c = new QuestionControl(i.ID, f);
                        flowLayoutPanel1.Controls.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void LoadDataAdmin()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    dataGridView1.DataSource = db.Participants.Where(a => a.Date.Date == DateTime.Now.Date).Select(a => new
                    {
                        User = a.User.FullName,
                        Subject = a.Subject.Name,
                        a.Date,
                        TimeTaken = a.TimeTaken >= 60 ? $"{a.TimeTaken} minutes" : $"{a.TimeTaken} minute",
                        Answered = a.ParticipantAnswers.Count(),
                        Unanswered = a.Subject.Questions.Count() - a.ParticipantAnswers.Count()
                    }).OrderBy(a => a.Date).ToList();
                    dataGridView1.ColumnHeadersVisible = !(dataGridView1.RowCount <= 0);

                    dataGridView2.DataSource = db.Users.Where(a => a.Role != '1').Select(a => new
                    {
                        a.ID,
                        User = a.FullName,
                        a.Gender,
                        a.BirthDate,
                        a.IsActive
                    }).AsQueryable();
                    dataGridView2.Columns["ID"].Visible = false;
                    dataGridView2.ColumnHeadersVisible = !(dataGridView2.RowCount <= 0);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToString("dd-MMMM-yyyy HH:mm:ss");
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    r.Cells["Answered"].Style.BackColor = Color.ForestGreen;
                    r.Cells["Unanswered"].Style.BackColor = Color.Salmon;
                    r.Cells["Answered"].Style.ForeColor = Color.White;
                    r.Cells["Unanswered"].Style.ForeColor = Color.White;
                    r.Cells["Answered"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    r.Cells["Unanswered"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.Columns["Answered"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.Columns["Unanswered"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow r in dataGridView2.Rows)
                {
                    if (r.Cells["IsActive"].Value.ToInt32() == 0)
                        r.DefaultCellStyle.BackColor = Color.Salmon;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    DataGridViewRow r = dataGridView2.Rows[e.RowIndex];
                    selectedUser = r.Cells["ID"].Value.ToInt32();
                    btnIsActive.Enabled = true;
                    btnIsActive.Text = r.Cells["IsActive"].Value.ToInt32() == 0 ? "IsActive" : "InActive";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void btnIsActive_Click(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var user = db.Users.FirstOrDefault(a => a.ID == selectedUser);
                    user.IsActive = btnIsActive.Text == "IsActive" ? true : false;
                    db.SubmitChanges();
                    LoadDataAdmin();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }
    }
}
