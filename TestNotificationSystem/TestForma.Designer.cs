namespace TestNotificationSystem
{
    partial class TestForma
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.dtPFromDate = new System.Windows.Forms.DateTimePicker();
            this.llbFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtPToDate = new System.Windows.Forms.DateTimePicker();
            this.gbEmail = new System.Windows.Forms.GroupBox();
            this.gbDate = new System.Windows.Forms.GroupBox();
            this.gbEmail.SuspendLayout();
            this.gbDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(147, 26);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(140, 20);
            this.txtEmail.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "E mail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(147, 57);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(140, 20);
            this.txtPassword.TabIndex = 4;
            // 
            // dtPFromDate
            // 
            this.dtPFromDate.CustomFormat = "yyyy-MM-dd";
            this.dtPFromDate.Location = new System.Drawing.Point(147, 19);
            this.dtPFromDate.Name = "dtPFromDate";
            this.dtPFromDate.Size = new System.Drawing.Size(140, 20);
            this.dtPFromDate.TabIndex = 6;
            // 
            // llbFromDate
            // 
            this.llbFromDate.AutoSize = true;
            this.llbFromDate.Location = new System.Drawing.Point(18, 25);
            this.llbFromDate.Name = "llbFromDate";
            this.llbFromDate.Size = new System.Drawing.Size(57, 13);
            this.llbFromDate.TabIndex = 7;
            this.llbFromDate.Text = "From date:";
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(18, 64);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(50, 13);
            this.lblToDate.TabIndex = 8;
            this.lblToDate.Text = "To date: ";
            // 
            // dtPToDate
            // 
            this.dtPToDate.Location = new System.Drawing.Point(147, 57);
            this.dtPToDate.Name = "dtPToDate";
            this.dtPToDate.Size = new System.Drawing.Size(140, 20);
            this.dtPToDate.TabIndex = 9;
            // 
            // gbEmail
            // 
            this.gbEmail.Controls.Add(this.label2);
            this.gbEmail.Controls.Add(this.txtPassword);
            this.gbEmail.Controls.Add(this.txtEmail);
            this.gbEmail.Controls.Add(this.label1);
            this.gbEmail.Location = new System.Drawing.Point(46, 27);
            this.gbEmail.Name = "gbEmail";
            this.gbEmail.Size = new System.Drawing.Size(305, 134);
            this.gbEmail.TabIndex = 13;
            this.gbEmail.TabStop = false;
            this.gbEmail.Text = "Enter sender eMail address";
            // 
            // gbDate
            // 
            this.gbDate.Controls.Add(this.dtPFromDate);
            this.gbDate.Controls.Add(this.lblToDate);
            this.gbDate.Controls.Add(this.dtPToDate);
            this.gbDate.Controls.Add(this.llbFromDate);
            this.gbDate.Location = new System.Drawing.Point(46, 198);
            this.gbDate.Name = "gbDate";
            this.gbDate.Size = new System.Drawing.Size(305, 100);
            this.gbDate.TabIndex = 14;
            this.gbDate.TabStop = false;
            this.gbDate.Text = "Choose date range";
            // 
            // TestForma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 356);
            this.Controls.Add(this.gbDate);
            this.Controls.Add(this.gbEmail);
            this.Name = "TestForma";
            this.Text = "Test notification";
            this.Load += new System.EventHandler(this.TestForma_Load);
            this.gbEmail.ResumeLayout(false);
            this.gbEmail.PerformLayout();
            this.gbDate.ResumeLayout(false);
            this.gbDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.DateTimePicker dtPFromDate;
        private System.Windows.Forms.Label llbFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtPToDate;
        private System.Windows.Forms.GroupBox gbEmail;
        private System.Windows.Forms.GroupBox gbDate;

    }
}

