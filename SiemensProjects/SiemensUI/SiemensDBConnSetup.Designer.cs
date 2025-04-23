namespace SiemensUI
{
    partial class SiemensDBConnSetup
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
            this.lblHost = new System.Windows.Forms.Label();
            this.lblLoginName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tbDataBase = new System.Windows.Forms.TextBox();
            this.tbLoginName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblTableName = new System.Windows.Forms.Label();
            this.tbTableName = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.lbUser = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHost
            // 
            this.lblHost.Location = new System.Drawing.Point(12, 20);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(69, 23);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host:";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLoginName
            // 
            this.lblLoginName.Location = new System.Drawing.Point(12, 124);
            this.lblLoginName.Name = "lblLoginName";
            this.lblLoginName.Size = new System.Drawing.Size(69, 23);
            this.lblLoginName.TabIndex = 1;
            this.lblLoginName.Text = "Login name:";
            this.lblLoginName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(12, 176);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(69, 23);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(85, 22);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(178, 20);
            this.tbHost.TabIndex = 3;
            // 
            // tbDataBase
            // 
            this.tbDataBase.Location = new System.Drawing.Point(87, 74);
            this.tbDataBase.Name = "tbDataBase";
            this.tbDataBase.Size = new System.Drawing.Size(178, 20);
            this.tbDataBase.TabIndex = 4;
            // 
            // tbLoginName
            // 
            this.tbLoginName.Location = new System.Drawing.Point(87, 126);
            this.tbLoginName.Name = "tbLoginName";
            this.tbLoginName.Size = new System.Drawing.Size(178, 20);
            this.tbLoginName.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(125, 324);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(238, 324);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(87, 179);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(178, 20);
            this.tbPassword.TabIndex = 9;
            // 
            // lblDatabase
            // 
            this.lblDatabase.Location = new System.Drawing.Point(12, 72);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(69, 23);
            this.lblDatabase.TabIndex = 10;
            this.lblDatabase.Text = "Database:";
            this.lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 324);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(95, 23);
            this.btnTest.TabIndex = 12;
            this.btnTest.Text = "Test connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblTableName
            // 
            this.lblTableName.Location = new System.Drawing.Point(12, 229);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(69, 23);
            this.lblTableName.TabIndex = 13;
            this.lblTableName.Text = "Table name:";
            this.lblTableName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTableName
            // 
            this.tbTableName.Location = new System.Drawing.Point(87, 231);
            this.tbTableName.Name = "tbTableName";
            this.tbTableName.Size = new System.Drawing.Size(178, 20);
            this.tbTableName.TabIndex = 14;
            // 
            // tbUserName
            // 
            this.tbUserName.BackColor = System.Drawing.SystemColors.Control;
            this.tbUserName.Location = new System.Drawing.Point(87, 275);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(178, 20);
            this.tbUserName.TabIndex = 16;
            // 
            // lbUser
            // 
            this.lbUser.Location = new System.Drawing.Point(12, 273);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(69, 23);
            this.lbUser.TabIndex = 15;
            this.lbUser.Text = "User:";
            this.lbUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SiemensDBConnSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 361);
            this.ControlBox = false;
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.lbUser);
            this.Controls.Add(this.tbTableName);
            this.Controls.Add(this.lblTableName);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbLoginName);
            this.Controls.Add(this.tbDataBase);
            this.Controls.Add(this.tbHost);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblLoginName);
            this.Controls.Add(this.lblHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SiemensDBConnSetup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Asco DB connection data:";
            this.Load += new System.EventHandler(this.SiemensDBConnSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblLoginName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.TextBox tbDataBase;
        private System.Windows.Forms.TextBox tbLoginName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.TextBox tbTableName;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label lbUser;
    }
}