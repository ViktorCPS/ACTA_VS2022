namespace UI
{
    partial class SystemMessageAdd
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
            this.cbRole = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.gbDateInterval = new System.Windows.Forms.GroupBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.gbMessage = new System.Windows.Forms.GroupBox();
            this.tbMessageEN = new System.Windows.Forms.TextBox();
            this.tbMessageSR = new System.Windows.Forms.TextBox();
            this.lblMessageSR = new System.Windows.Forms.Label();
            this.lblMessageEN = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbDateInterval.SuspendLayout();
            this.gbMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbRole
            // 
            this.cbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRole.FormattingEnabled = true;
            this.cbRole.Location = new System.Drawing.Point(83, 56);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(231, 21);
            this.cbRole.TabIndex = 3;
            // 
            // lblRole
            // 
            this.lblRole.Location = new System.Drawing.Point(11, 54);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(66, 23);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Role:";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCompany
            // 
            this.cbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(83, 30);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(231, 21);
            this.cbCompany.TabIndex = 1;
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(8, 28);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(69, 23);
            this.lblCompany.TabIndex = 0;
            this.lblCompany.Text = "Company:";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbDateInterval
            // 
            this.gbDateInterval.Controls.Add(this.dtpFrom);
            this.gbDateInterval.Controls.Add(this.lblFrom);
            this.gbDateInterval.Controls.Add(this.dtpTo);
            this.gbDateInterval.Controls.Add(this.lblTo);
            this.gbDateInterval.Location = new System.Drawing.Point(328, 10);
            this.gbDateInterval.Name = "gbDateInterval";
            this.gbDateInterval.Size = new System.Drawing.Size(178, 80);
            this.gbDateInterval.TabIndex = 4;
            this.gbDateInterval.TabStop = false;
            this.gbDateInterval.Text = "Date interval";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(48, 21);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(122, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(48, 47);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(122, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(21, 44);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 23);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbMessage
            // 
            this.gbMessage.Controls.Add(this.tbMessageEN);
            this.gbMessage.Controls.Add(this.tbMessageSR);
            this.gbMessage.Controls.Add(this.lblMessageSR);
            this.gbMessage.Controls.Add(this.lblMessageEN);
            this.gbMessage.Location = new System.Drawing.Point(14, 96);
            this.gbMessage.Name = "gbMessage";
            this.gbMessage.Size = new System.Drawing.Size(492, 257);
            this.gbMessage.TabIndex = 5;
            this.gbMessage.TabStop = false;
            this.gbMessage.Text = "Messages";
            // 
            // tbMessageEN
            // 
            this.tbMessageEN.Location = new System.Drawing.Point(52, 136);
            this.tbMessageEN.MaxLength = 4000;
            this.tbMessageEN.Multiline = true;
            this.tbMessageEN.Name = "tbMessageEN";
            this.tbMessageEN.Size = new System.Drawing.Size(432, 110);
            this.tbMessageEN.TabIndex = 3;
            // 
            // tbMessageSR
            // 
            this.tbMessageSR.Location = new System.Drawing.Point(52, 20);
            this.tbMessageSR.MaxLength = 4000;
            this.tbMessageSR.Multiline = true;
            this.tbMessageSR.Name = "tbMessageSR";
            this.tbMessageSR.Size = new System.Drawing.Size(432, 110);
            this.tbMessageSR.TabIndex = 1;
            // 
            // lblMessageSR
            // 
            this.lblMessageSR.Location = new System.Drawing.Point(9, 18);
            this.lblMessageSR.Name = "lblMessageSR";
            this.lblMessageSR.Size = new System.Drawing.Size(37, 23);
            this.lblMessageSR.TabIndex = 0;
            this.lblMessageSR.Text = "SR:";
            this.lblMessageSR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMessageEN
            // 
            this.lblMessageEN.Location = new System.Drawing.Point(9, 134);
            this.lblMessageEN.Name = "lblMessageEN";
            this.lblMessageEN.Size = new System.Drawing.Size(37, 23);
            this.lblMessageEN.TabIndex = 2;
            this.lblMessageEN.Text = "EN:";
            this.lblMessageEN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(431, 369);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 369);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(11, 369);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SystemMessageAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 415);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMessage);
            this.Controls.Add(this.gbDateInterval);
            this.Controls.Add(this.cbRole);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cbCompany);
            this.Controls.Add(this.lblCompany);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SystemMessageAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Message Add";
            this.Load += new System.EventHandler(this.SystemMessageAdd_Load);
            this.gbDateInterval.ResumeLayout(false);
            this.gbMessage.ResumeLayout(false);
            this.gbMessage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRole;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.GroupBox gbDateInterval;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.GroupBox gbMessage;
        private System.Windows.Forms.TextBox tbMessageEN;
        private System.Windows.Forms.TextBox tbMessageSR;
        private System.Windows.Forms.Label lblMessageSR;
        private System.Windows.Forms.Label lblMessageEN;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSave;
    }
}