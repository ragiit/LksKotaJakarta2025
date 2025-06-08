namespace Quizify
{
    partial class QuestionControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn = new System.Windows.Forms.Button();
            this.lblNumberOfQuestion = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn);
            this.groupBox1.Controls.Add(this.lblNumberOfQuestion);
            this.groupBox1.Controls.Add(this.lblTime);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 153);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btn
            // 
            this.btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(150)))), ((int)(((byte)(210)))));
            this.btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn.ForeColor = System.Drawing.Color.White;
            this.btn.Location = new System.Drawing.Point(23, 113);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(214, 23);
            this.btn.TabIndex = 27;
            this.btn.Text = "Start";
            this.btn.UseVisualStyleBackColor = false;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // lblNumberOfQuestion
            // 
            this.lblNumberOfQuestion.AutoSize = true;
            this.lblNumberOfQuestion.Location = new System.Drawing.Point(20, 83);
            this.lblNumberOfQuestion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNumberOfQuestion.Name = "lblNumberOfQuestion";
            this.lblNumberOfQuestion.Size = new System.Drawing.Size(179, 17);
            this.lblNumberOfQuestion.TabIndex = 26;
            this.lblNumberOfQuestion.Text = "Number of Question: 45";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(20, 60);
            this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 17);
            this.lblTime.TabIndex = 25;
            this.lblTime.Text = "Time: 60 min";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(19, 16);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(115, 29);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "Science";
            // 
            // QuestionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "QuestionControl";
            this.Size = new System.Drawing.Size(257, 153);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Label lblNumberOfQuestion;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblName;
    }
}
