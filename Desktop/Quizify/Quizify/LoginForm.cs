using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quizify
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
         }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tbPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckIsEmpty())
                {
                    "All field must be filled.".ShowMessage();
                    return;
                }

                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    var user = db.Users.FirstOrDefault(a => a.Email == tbEmailExamCard.Text && a.Password == tbPassword.Text);

                    if (user == null)
                    {
                        "Your data invalid, please try again.".ShowMessage();
                        return;
                    }

                    if (user.IsActive == false)
                    {
                        "Your account is not active, please contact admin.".ShowMessage();
                        return;
                    }

                    Helper.user = user;
                    Hide();
                    new MainForm(false).Show();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ShowMessageError();
            }
        }

        private void lblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new RegistrationForm().ShowDialog();
        }
    }
}
