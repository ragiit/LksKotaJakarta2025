using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Quizify
{
    public partial class OptionControl : UserControl
    {
        private string imageA, imageB, imageC, imageD;
        public OptionControl()
        {
            InitializeComponent();

            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var question = Helper.model;
                    var textQuestion = question.Question.Split(',');

                    lblNumber.Text = $"Question #{question.No}";
                    lblQuestion.Text = textQuestion.Length <= 1 ? textQuestion[0] : string.Join(",", textQuestion.Take(textQuestion.Length - 1));

                    var image = textQuestion.LastOrDefault()?.Trim();
                    if (IsImage(image))
                    {
                        pictQuestion.Image = Image.FromFile($"{image}".GetImage());
                    }

                    if (IsImage(question.OptionA))
                    {
                        pictA.Visible = true;
                        pictB.Visible = true;
                        pictC.Visible = true;
                        pictD.Visible = true;
                        imageA = question.OptionA;
                        imageB = question.OptionB;
                        imageC = question.OptionC;
                        imageD = question.OptionD;
                        pictA.Image = Image.FromFile($"{question.OptionA}".GetImage());
                        pictB.Image = Image.FromFile($"{question.OptionB}".GetImage());
                        pictC.Image = Image.FromFile($"{question.OptionC}".GetImage());
                        pictD.Image = Image.FromFile($"{question.OptionD}".GetImage());
                    }
                    else
                    {
                        lblA.Text = question.OptionA;
                        lblB.Text = question.OptionB;
                        lblC.Text = question.OptionC;
                        lblD.Text = question.OptionD;
                    }   

                    if (question.Answer != null)
                    {
                        if (question.Answer == question.OptionA) rbA.Checked = true;
                        if (question.Answer == question.OptionB) rbB.Checked = true;
                        if (question.Answer == question.OptionC) rbC.Checked = true;
                        if (question.Answer == question.OptionD) rbD.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private bool IsImage(string filePath)
        {
            string extension = Path.GetExtension(filePath)?.ToLower();

            if (string.IsNullOrWhiteSpace(extension)) return false;

            return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            var question = Helper.models.FirstOrDefault(a => a.ID == Helper.model.ID);

            if (IsImage(question.OptionA))
            {
                if (rb.Text == "A.") question.Answer = imageA;
                if (rb.Text == "B.") question.Answer = imageB;
                if (rb.Text == "C.") question.Answer = imageC;
                if (rb.Text == "D.") question.Answer = imageD;
                return;
            }

            if (rb.Text == "A.") question.Answer = lblA.Text;
            if (rb.Text == "B.") question.Answer = lblB.Text;
            if (rb.Text == "C.") question.Answer = lblC.Text;
            if (rb.Text == "D.") question.Answer = lblD.Text;
        }
    }
}
