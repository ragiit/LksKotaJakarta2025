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
    public partial class MainForm : Form
    {
        private bool sidebarOpen = false;
        private bool result = false;
        public MainForm(bool result)
        {
            InitializeComponent();
            this.result = result;
        }

        private void ChangeColorButton(Button btn)
        {
            foreach (Control c in panelSidebar.Controls)
            {
                if (c == btn)
                    c.BackColor = Color.Gray;
                else
                    c.BackColor = Color.FromArgb(85, 150, 210);
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            btnMenuLeft.Visible = !sidebarOpen;
            panelSidebar.Size = new Size(140, 464);
        }

        private void ChangeForm(Form f)
        {
            panelContainer.Controls.Clear();
            f.TopLevel = false;
            panelContainer.Controls.Add(f);
            f.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ChangeColorButton(btnDashboard);
            ChangeForm(new DashboardForm(this));
        }

        private void btnQuestion_Click(object sender, EventArgs e)
        {
            ChangeColorButton(btnQuestion);
            ChangeForm(new QuestionsForm(this));
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            ChangeColorButton(btnResult);
            ChangeForm(new ResultsForm());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to Logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Helper.user = null;
                Hide();
                new LoginForm().Show();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (result)
            {
                ChangeColorButton(btnResult);
                ChangeForm(new ResultsForm());
                return;
            }

            ChangeColorButton(btnDashboard);
            ChangeForm(new DashboardForm(this));
        }

        private void btnMenuLeft_Click(object sender, EventArgs e)
        {
            btnMenuLeft.Visible = sidebarOpen;
            panelSidebar.Size = new Size(50, 464);
        }
    }
}
