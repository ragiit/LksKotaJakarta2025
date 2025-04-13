using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Quizify
{
    public partial class EditAddQuestionForm : Form
    {
        private int selectedId, selectedSubject;
        private string imageQuestion, imageA, imageB, imageC, imageD;

        private void pictC_Click(object sender, EventArgs e)
        {
            ChooseImageOpt(pictC);
        }

        private void pictA_Click(object sender, EventArgs e)
        {
            ChooseImageOpt(pictA);
        }

        private void pictB_Click(object sender, EventArgs e)
        {
            ChooseImageOpt(pictB);
        }

        private void pictD_Click(object sender, EventArgs e)
        {
            ChooseImageOpt(pictD);
        }

        public EditAddQuestionForm(int selectedId, int selectedSubject)
        {
            InitializeComponent();
            this.selectedId = selectedId;
            this.selectedSubject = selectedSubject;

            if (selectedId != 0)
            {
                this.Text = "Edit";

                try
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        var question = db.Questions.FirstOrDefault(a => a.ID == selectedId);
                        var textQuestion = question.Question1.Split(',');

                        tbQuestion.Text = textQuestion.Length <= 1 ? textQuestion[0] : string.Join(",", textQuestion.Take(textQuestion.Length - 1));

                        var image = textQuestion.LastOrDefault()?.Trim();
                        if (IsImage(image))
                        {
                            imageA = $"{Helper.basePath}{image}";
                            pictQuestion.Image = Image.FromFile($"{image}".GetImage());
                        }

                        if (IsImage(question.OptionA))
                        {
                            checkBox1.Checked = true;
                            imageA = $"{Helper.basePath}{question.OptionA}";
                            imageB = $"{Helper.basePath}{question.OptionB}";
                            imageC = $"{Helper.basePath}{question.OptionC}";
                            imageD = $"{Helper.basePath}{question.OptionD}";
                            pictA.Image = Image.FromFile($"{question.OptionA}".GetImage());
                            pictB.Image = Image.FromFile($"{question.OptionB}".GetImage());
                            pictC.Image = Image.FromFile($"{question.OptionC}".GetImage());
                            pictD.Image = Image.FromFile($"{question.OptionD}".GetImage());
                        }
                        else
                        {
                            tbA.Text = question.OptionA;
                            tbB.Text = question.OptionB;
                            tbC.Text = question.OptionC;
                            tbD.Text = question.OptionD;
                        }

                        var correct = "";
                        if (question.CorrectAnswer == question.OptionA) correct = "A";
                        if (question.CorrectAnswer == question.OptionB) correct = "B";
                        if (question.CorrectAnswer == question.OptionC) correct = "C";
                        if (question.CorrectAnswer == question.OptionD) correct = "D";
                        GetCorrectAnswer(correct);
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ShowMessageError();
                }
            }
        }

        private bool IsImage(string filePath)
        {
            string extension = Path.GetExtension(filePath)?.ToLower();

            if (string.IsNullOrWhiteSpace(extension)) return false;

            return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string GetCorrectAnswer()
        {
            if (checkBox1.Checked)
            {
                if (rbApict.Checked) return imageA;
                else if (rbBpict.Checked) return imageB;
                else if (rbCpict.Checked) return imageC;
                else if (rbDpict.Checked) return imageD;
                else return null;
            }

            if (rbAtext.Checked) return tbA.Text;
            else if (rbBtext.Checked) return tbB.Text;
            else if (rbCtext.Checked) return tbC.Text;
            else if (rbDtext.Checked) return tbD.Text;
            else return null;
        }

        private void pictQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Files|*.png;*.jpg;*.jpeg" })
                {
                    if (ofd.ShowDialog() != DialogResult.OK) return;
                    imageQuestion = ofd.FileName;
                    pictQuestion.Image = Image.FromFile(imageQuestion);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbQuestion.Text))
                {
                    "Question can't be empty.".ShowMessage();
                    return;
                }

                if (!checkBox1.Checked && this.CheckIsEmpty())
                {
                    "All field must be filled.".ShowMessage();
                    return;
                }

                if (checkBox1.Checked && (string.IsNullOrWhiteSpace(imageA) || string.IsNullOrWhiteSpace(imageB) || string.IsNullOrWhiteSpace(imageC) || string.IsNullOrWhiteSpace(imageD)))
                {
                    "Option cannot null.".ShowMessage();
                    return;
                }

                if (string.IsNullOrWhiteSpace(GetCorrectAnswer()))
                {
                    "Please choose the correct answer for the option.".ShowMessage();
                    return;
                }

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var pictQuestion = $"{Guid.NewGuid().ToString().Split('-')[0]}{Path.GetExtension(imageQuestion)}";
                    var pictA = $"{Guid.NewGuid().ToString().Split('-')[0]}{Path.GetExtension(imageA)}";
                    var pictB = $"{Guid.NewGuid().ToString().Split('-')[0]}{Path.GetExtension(imageB)}";
                    var pictC = $"{Guid.NewGuid().ToString().Split('-')[0]}{Path.GetExtension(imageC)}";
                    var pictD = $"{Guid.NewGuid().ToString().Split('-')[0]}{Path.GetExtension(imageD)}";

                    string correct = null;
                    if (checkBox1.Checked)
                    {
                        if (rbApict.Checked) correct = pictA;
                        else if (rbBpict.Checked) correct = pictB;
                        else if (rbCpict.Checked) correct = pictC;
                        else if (rbDpict.Checked) correct = pictD;
                    }

                    if (selectedId == 0)
                    {
                        db.Questions.InsertOnSubmit(new Question
                        {
                            SubjectID = selectedSubject,
                            Question1 = imageQuestion == null ? tbQuestion.Text : $"{tbQuestion.Text},{pictQuestion}",
                            OptionA = checkBox1.Checked ? pictA : tbA.Text,
                            OptionB = checkBox1.Checked ? pictB : tbB.Text,
                            OptionC = checkBox1.Checked ? pictC : tbC.Text,
                            OptionD = checkBox1.Checked ? pictD : tbD.Text,
                            CorrectAnswer = checkBox1.Checked ? correct : GetCorrectAnswer(),
                        });
                        db.SubmitChanges();
                        "Successfully add new question.".ShowMessage();
                    }
                    else
                    {
                        var question = db.Questions.FirstOrDefault(a => a.ID == selectedId);

                        if (question != null)
                        {
                            question.Question1 = imageQuestion == null ? tbQuestion.Text : $"{tbQuestion.Text},{pictQuestion}";
                            question.OptionA = checkBox1.Checked ? pictA : tbA.Text;
                            question.OptionB = checkBox1.Checked ? pictB : tbB.Text;
                            question.OptionC = checkBox1.Checked ? pictC : tbC.Text;
                            question.OptionD = checkBox1.Checked ? pictD : tbD.Text;
                            question.CorrectAnswer = checkBox1.Checked ? correct : GetCorrectAnswer();

                            db.SubmitChanges();
                            "Successfully update the question.".ShowMessage();
                        }
                    }

                    if (imageQuestion != null) SaveBitmap(imageQuestion, pictQuestion);
                    if (checkBox1.Checked)
                    {
                        SaveBitmap(imageA, pictA);
                        SaveBitmap(imageB, pictB);
                        SaveBitmap(imageC, pictC);
                        SaveBitmap(imageD, pictD);
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        void SaveBitmap(string imagePath, string filename)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Helper.basePath));
            using (var img = Image.FromFile(imagePath))
            {
                img.Save(Helper.basePath + filename, img.RawFormat);
            }
        }

        private void GetCorrectAnswer(string text)
        {
            if (checkBox1.Checked)
            {
                if (text == "A") rbApict.Checked = true;
                if (text == "B") rbBpict.Checked = true;
                if (text == "C") rbCpict.Checked = true;
                if (text == "D") rbDpict.Checked = true;
                return;
            }

            if (text == "A") rbAtext.Checked = true;
            if (text == "B") rbBtext.Checked = true;
            if (text == "C") rbCtext.Checked = true;
            if (text == "D") rbDtext.Checked = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panelImage.Visible = checkBox1.Checked;
        }

        private void ChooseImageOpt(PictureBox opt)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Files|*.png;*.jpg;*.jpeg" })
                {
                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    foreach (Control c in panelImage.Controls.OfType<PictureBox>())
                    {
                        if (c == opt)
                        {
                            opt.Image = Image.FromFile(ofd.FileName);

                            if (opt.Name == "pictA") imageA = ofd.FileName;
                            if (opt.Name == "pictB") imageB = ofd.FileName;
                            if (opt.Name == "pictC") imageC = ofd.FileName;
                            if (opt.Name == "pictD") imageD = ofd.FileName;
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
