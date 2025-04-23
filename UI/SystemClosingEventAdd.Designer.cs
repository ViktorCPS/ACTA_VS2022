namespace UI
{
    partial class SystemClosingEventAdd
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
            this.gbDateInterval = new System.Windows.Forms.GroupBox();
            this.chbNotDefined = new System.Windows.Forms.CheckBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.gbMessage = new System.Windows.Forms.GroupBox();
            this.btnSelectMsgEN = new System.Windows.Forms.Button();
            this.btnSelectMsgSR = new System.Windows.Forms.Button();
            this.tbMessageEN = new System.Windows.Forms.TextBox();
            this.tbMessageSR = new System.Windows.Forms.TextBox();
            this.lblMessageSR = new System.Windows.Forms.Label();
            this.lblMessageEN = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbDateInterval.SuspendLayout();
            this.gbMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDateInterval
            // 
            this.gbDateInterval.Controls.Add(this.chbNotDefined);
            this.gbDateInterval.Controls.Add(this.dtpFrom);
            this.gbDateInterval.Controls.Add(this.lblFrom);
            this.gbDateInterval.Controls.Add(this.dtpTo);
            this.gbDateInterval.Controls.Add(this.lblTo);
            this.gbDateInterval.Location = new System.Drawing.Point(12, 6);
            this.gbDateInterval.Name = "gbDateInterval";
            this.gbDateInterval.Size = new System.Drawing.Size(271, 80);
            this.gbDateInterval.TabIndex = 0;
            this.gbDateInterval.TabStop = false;
            this.gbDateInterval.Text = "Date interval";
            // 
            // chbNotDefined
            // 
            this.chbNotDefined.AutoSize = true;
            this.chbNotDefined.Location = new System.Drawing.Point(176, 48);
            this.chbNotDefined.Name = "chbNotDefined";
            this.chbNotDefined.Size = new System.Drawing.Size(81, 17);
            this.chbNotDefined.TabIndex = 4;
            this.chbNotDefined.Text = "Not defined";
            this.chbNotDefined.UseVisualStyleBackColor = true;
            this.chbNotDefined.CheckedChanged += new System.EventHandler(this.chbNotDefined_CheckedChanged);
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
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(289, 50);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(40, 23);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(335, 52);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(178, 21);
            this.cbType.TabIndex = 2;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // gbMessage
            // 
            this.gbMessage.Controls.Add(this.btnSelectMsgEN);
            this.gbMessage.Controls.Add(this.btnSelectMsgSR);
            this.gbMessage.Controls.Add(this.tbMessageEN);
            this.gbMessage.Controls.Add(this.tbMessageSR);
            this.gbMessage.Controls.Add(this.lblMessageSR);
            this.gbMessage.Controls.Add(this.lblMessageEN);
            this.gbMessage.Location = new System.Drawing.Point(12, 98);
            this.gbMessage.Name = "gbMessage";
            this.gbMessage.Size = new System.Drawing.Size(518, 257);
            this.gbMessage.TabIndex = 3;
            this.gbMessage.TabStop = false;
            this.gbMessage.Text = "Messages";
            // 
            // btnSelectMsgEN
            // 
            this.btnSelectMsgEN.Location = new System.Drawing.Point(463, 134);
            this.btnSelectMsgEN.Name = "btnSelectMsgEN";
            this.btnSelectMsgEN.Size = new System.Drawing.Size(38, 23);
            this.btnSelectMsgEN.TabIndex = 5;
            this.btnSelectMsgEN.Text = "...";
            this.btnSelectMsgEN.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectMsgEN.UseVisualStyleBackColor = true;
            this.btnSelectMsgEN.Click += new System.EventHandler(this.btnSelectMsgEN_Click);
            // 
            // btnSelectMsgSR
            // 
            this.btnSelectMsgSR.Location = new System.Drawing.Point(463, 18);
            this.btnSelectMsgSR.Name = "btnSelectMsgSR";
            this.btnSelectMsgSR.Size = new System.Drawing.Size(38, 23);
            this.btnSelectMsgSR.TabIndex = 2;
            this.btnSelectMsgSR.Text = "...";
            this.btnSelectMsgSR.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectMsgSR.UseVisualStyleBackColor = true;
            this.btnSelectMsgSR.Click += new System.EventHandler(this.btnSelectMsgSR_Click);
            // 
            // tbMessageEN
            // 
            this.tbMessageEN.Location = new System.Drawing.Point(52, 136);
            this.tbMessageEN.MaxLength = 500;
            this.tbMessageEN.Multiline = true;
            this.tbMessageEN.Name = "tbMessageEN";
            this.tbMessageEN.Size = new System.Drawing.Size(405, 110);
            this.tbMessageEN.TabIndex = 4;
            // 
            // tbMessageSR
            // 
            this.tbMessageSR.Location = new System.Drawing.Point(52, 20);
            this.tbMessageSR.MaxLength = 500;
            this.tbMessageSR.Multiline = true;
            this.tbMessageSR.Name = "tbMessageSR";
            this.tbMessageSR.Size = new System.Drawing.Size(405, 110);
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
            this.lblMessageEN.TabIndex = 3;
            this.lblMessageEN.Text = "EN:";
            this.lblMessageEN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 366);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 366);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(455, 366);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SystemClosingEventAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 410);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMessage);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.gbDateInterval);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SystemClosingEventAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Closing Event Add";
            this.Load += new System.EventHandler(this.SystemClosingEventAdd_Load);
            this.gbDateInterval.ResumeLayout(false);
            this.gbDateInterval.PerformLayout();
            this.gbMessage.ResumeLayout(false);
            this.gbMessage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDateInterval;
        private System.Windows.Forms.CheckBox chbNotDefined;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.GroupBox gbMessage;
        private System.Windows.Forms.Button btnSelectMsgSR;
        private System.Windows.Forms.TextBox tbMessageEN;
        private System.Windows.Forms.TextBox tbMessageSR;
        private System.Windows.Forms.Label lblMessageSR;
        private System.Windows.Forms.Label lblMessageEN;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectMsgEN;
    }
}