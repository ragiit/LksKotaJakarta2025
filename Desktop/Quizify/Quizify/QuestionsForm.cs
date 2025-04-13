using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quizify
{
    public partial class QuestionsForm : Form
    {
        private Form f;
        public QuestionsForm(Form f)
        {
            InitializeComponent();
            this.f = f;

            if (Helper.user.Role == '2')
            {
                lblName.Text = "Question";
                flowLayoutPanel1.Visible = true;
                cbSubject.Visible = false;
                try
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var subjects = db.Subjects.ToList();

                        foreach (var i in subjects)
                        {
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

            InitCombobox();
            LoadData();
        }

        private void InitCombobox()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                cbSubject.ValueMember = "ID";
                cbSubject.DisplayMember = "Name";
                cbSubject.DataSource = db.Subjects.Select(a =>new { a.ID, a.Name }).ToList();
            }
        }

        private void LoadData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    dataGridView1.DataSource = db.Questions.Where(a => SqlMethods.Like(a.Subject.Name, $"%{cbSubject.Text}%")).Select(a => new
                    {
                        a.ID,
                        Subject = a.Subject.Name,
                        Question = a.Question1,
                        a.OptionA,
                        a.OptionB,
                        a.OptionC,
                        a.OptionD,
                        a.CorrectAnswer,
                    }).ToList();
                    dataGridView1.Columns["ID"].Visible = false;
                    dataGridView1.Columns["Subject"].Visible = false;
                    dataGridView1.Columns[EditGrid.Name].DisplayIndex = dataGridView1.ColumnCount - 1;
                    dataGridView1.Columns[DeleteGrid.Name].DisplayIndex = dataGridView1.ColumnCount - 1;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (new EditAddQuestionForm(0, cbSubject.SelectedValue.ToInt32()).ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    DataGridViewRow r = dataGridView1.Rows[e.RowIndex];

                    if (e.ColumnIndex == 0)
                    {
                        if (new EditAddQuestionForm(r.Cells["ID"].Value.ToInt32(), cbSubject.SelectedValue.ToInt32()).ShowDialog() == DialogResult.OK)
                        {
                            LoadData();
                        }
                    }

                    if (e.ColumnIndex == 1)
                    {
                        if (MessageBox.Show("Are you sure want to delete this question?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            using (DataClasses1DataContext db = new DataClasses1DataContext())
                            {
                                var question = db.Questions.FirstOrDefault(a => a.ID == r.Cells["ID"].Value.ToInt32());
                                db.Questions.DeleteOnSubmit(question);
                                db.SubmitChanges();
                                "Successfully delete.".ShowMessage();
                                LoadData();
                            }
                        }
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
