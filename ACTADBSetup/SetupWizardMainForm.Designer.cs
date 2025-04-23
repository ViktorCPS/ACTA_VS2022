namespace ACTADBSetup
{
    partial class SetupWizardMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizardMainForm));
            this.gbDatabase = new System.Windows.Forms.GroupBox();
            this.rbMySQL = new System.Windows.Forms.RadioButton();
            this.rbMSSQLServer = new System.Windows.Forms.RadioButton();
            this.txtHostName = new System.Windows.Forms.TextBox();
            this.lblHostName = new System.Windows.Forms.Label();
            this.lblDBAPassword = new System.Windows.Forms.Label();
            this.txtDBAPassword = new System.Windows.Forms.TextBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblHostPort = new System.Windows.Forms.Label();
            this.txtHostPort = new System.Windows.Forms.TextBox();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.cbOverwriteExistingDatabase = new System.Windows.Forms.CheckBox();
            this.lblOverwriteExistingDatabase = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.gbDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDatabase
            // 
            this.gbDatabase.Controls.Add(this.rbMySQL);
            this.gbDatabase.Controls.Add(this.rbMSSQLServer);
            this.gbDatabase.Location = new System.Drawing.Point(26, 37);
            this.gbDatabase.Name = "gbDatabase";
            this.gbDatabase.Size = new System.Drawing.Size(135, 77);
            this.gbDatabase.TabIndex = 0;
            this.gbDatabase.TabStop = false;
            this.gbDatabase.Text = "Database";
            // 
            // rbMySQL
            // 
            this.rbMySQL.AutoSize = true;
            this.rbMySQL.Location = new System.Drawing.Point(16, 48);
            this.rbMySQL.Name = "rbMySQL";
            this.rbMySQL.Size = new System.Drawing.Size(60, 17);
            this.rbMySQL.TabIndex = 1;
            this.rbMySQL.Text = "MySQL";
            this.rbMySQL.UseVisualStyleBackColor = true;
            // 
            // rbMSSQLServer
            // 
            this.rbMSSQLServer.AutoSize = true;
            this.rbMSSQLServer.Checked = true;
            this.rbMSSQLServer.Location = new System.Drawing.Point(16, 20);
            this.rbMSSQLServer.Name = "rbMSSQLServer";
            this.rbMSSQLServer.Size = new System.Drawing.Size(65, 17);
            this.rbMSSQLServer.TabIndex = 0;
            this.rbMSSQLServer.TabStop = true;
            this.rbMSSQLServer.Text = "MS SQL";
            this.rbMSSQLServer.UseVisualStyleBackColor = true;
            this.rbMSSQLServer.CheckedChanged += new System.EventHandler(this.rbDatabaseType_CheckedChanged);
            // 
            // txtHostName
            // 
            this.txtHostName.Location = new System.Drawing.Point(244, 138);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(219, 20);
            this.txtHostName.TabIndex = 1;
            this.txtHostName.Text = "(local)";
            // 
            // lblHostName
            // 
            this.lblHostName.AutoSize = true;
            this.lblHostName.Location = new System.Drawing.Point(23, 141);
            this.lblHostName.Name = "lblHostName";
            this.lblHostName.Size = new System.Drawing.Size(216, 13);
            this.lblHostName.TabIndex = 6;
            this.lblHostName.Text = "Database server (default or named instance)";
            // 
            // lblDBAPassword
            // 
            this.lblDBAPassword.AutoSize = true;
            this.lblDBAPassword.Location = new System.Drawing.Point(22, 208);
            this.lblDBAPassword.Name = "lblDBAPassword";
            this.lblDBAPassword.Size = new System.Drawing.Size(115, 13);
            this.lblDBAPassword.TabIndex = 8;
            this.lblDBAPassword.Text = "Database sa password";
            // 
            // txtDBAPassword
            // 
            this.txtDBAPassword.Location = new System.Drawing.Point(244, 204);
            this.txtDBAPassword.Name = "txtDBAPassword";
            this.txtDBAPassword.PasswordChar = '*';
            this.txtDBAPassword.Size = new System.Drawing.Size(219, 20);
            this.txtDBAPassword.TabIndex = 3;
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(199, 308);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(95, 23);
            this.btnInstall.TabIndex = 4;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(368, 308);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(95, 23);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblHostPort
            // 
            this.lblHostPort.AutoSize = true;
            this.lblHostPort.Enabled = false;
            this.lblHostPort.Location = new System.Drawing.Point(23, 175);
            this.lblHostPort.Name = "lblHostPort";
            this.lblHostPort.Size = new System.Drawing.Size(26, 13);
            this.lblHostPort.TabIndex = 7;
            this.lblHostPort.Text = "Port";
            // 
            // txtHostPort
            // 
            this.txtHostPort.Enabled = false;
            this.txtHostPort.Location = new System.Drawing.Point(244, 171);
            this.txtHostPort.MaxLength = 5;
            this.txtHostPort.Name = "txtHostPort";
            this.txtHostPort.Size = new System.Drawing.Size(40, 20);
            this.txtHostPort.TabIndex = 2;
            this.txtHostPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAdvanced.Location = new System.Drawing.Point(376, 241);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(87, 23);
            this.btnAdvanced.TabIndex = 9;
            this.btnAdvanced.Text = "Advanced >>";
            this.btnAdvanced.UseVisualStyleBackColor = true;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // cbOverwriteExistingDatabase
            // 
            this.cbOverwriteExistingDatabase.AutoSize = true;
            this.cbOverwriteExistingDatabase.Location = new System.Drawing.Point(176, 246);
            this.cbOverwriteExistingDatabase.Name = "cbOverwriteExistingDatabase";
            this.cbOverwriteExistingDatabase.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbOverwriteExistingDatabase.Size = new System.Drawing.Size(15, 14);
            this.cbOverwriteExistingDatabase.TabIndex = 10;
            this.cbOverwriteExistingDatabase.UseVisualStyleBackColor = true;
            // 
            // lblOverwriteExistingDatabase
            // 
            this.lblOverwriteExistingDatabase.AutoSize = true;
            this.lblOverwriteExistingDatabase.Location = new System.Drawing.Point(22, 251);
            this.lblOverwriteExistingDatabase.Name = "lblOverwriteExistingDatabase";
            this.lblOverwriteExistingDatabase.Size = new System.Drawing.Size(137, 13);
            this.lblOverwriteExistingDatabase.TabIndex = 11;
            this.lblOverwriteExistingDatabase.Text = "Overwrite existing database";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(25, 308);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(95, 23);
            this.btnTest.TabIndex = 12;
            this.btnTest.Text = "Test connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // SetupWizardMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 357);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblOverwriteExistingDatabase);
            this.Controls.Add(this.cbOverwriteExistingDatabase);
            this.Controls.Add(this.btnAdvanced);
            this.Controls.Add(this.lblHostPort);
            this.Controls.Add(this.txtHostPort);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.lblDBAPassword);
            this.Controls.Add(this.txtDBAPassword);
            this.Controls.Add(this.lblHostName);
            this.Controls.Add(this.txtHostName);
            this.Controls.Add(this.gbDatabase);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SetupWizardMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ACTA DB System Setup";
            this.gbDatabase.ResumeLayout(false);
            this.gbDatabase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDatabase;
        private System.Windows.Forms.RadioButton rbMSSQLServer;
        private System.Windows.Forms.RadioButton rbMySQL;
        private System.Windows.Forms.TextBox txtHostName;
        private System.Windows.Forms.Label lblHostName;
        private System.Windows.Forms.Label lblDBAPassword;
        private System.Windows.Forms.TextBox txtDBAPassword;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblHostPort;
        private System.Windows.Forms.TextBox txtHostPort;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.CheckBox cbOverwriteExistingDatabase;
        private System.Windows.Forms.Label lblOverwriteExistingDatabase;
        private System.Windows.Forms.Button btnTest;
    }
}

