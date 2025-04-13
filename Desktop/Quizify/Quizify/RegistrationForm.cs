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
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckIsEmpty() || (!rbMale.Checked && !rbFemale.Checked))
                {
                    "All field must be filled.".ShowMessage();
                    return;
                }

                if (!Helper.rgEmail.IsMatch(tbEmail.Text))
                {
                    "Email invalid.".ShowMessage();
                    return;
                }

                if (tbPassword.Text != tbRetrypePassword.Text)
                {
                    "Password & Retype Password must be same.".ShowMessage();
                    return;
                }

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var check = db.Users.FirstOrDefault(a => a.Email == tbEmail.Text);

                    if (check != null)
                    {
                        "Email or Exam number is already exist.".ShowMessage();
                        return;
                    }

                    db.Users.InsertOnSubmit(new User
                    {
                        FullName = tbFullName.Text,
                        Email = tbEmail.Text,
                        Role = '2',
                        Gender = rbMale.Checked ? '1' : '2',
                        BirthDate = Convert.ToDateTime(tbBirthDate.Text),
                        Password = tbRetrypePassword.Text,
                        IsActive = false
                    });
                    db.SubmitChanges();
                    "Registration successfully, admin will activate your account soon.".ShowMessage();
                    Close();
                    new LoginForm().Show();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }
    }
}
