namespace Reports.ZIN
{
    partial class ZINPresencePreview
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
            this.btnReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbDate = new System.Windows.Forms.GroupBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHrs = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpHoursFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpHoursTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lvWU = new System.Windows.Forms.ListView();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbPresenceInterval = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMinPresence = new System.Windows.Forms.Label();
            this.nudPresenceInterval = new System.Windows.Forms.NumericUpDown();
            this.gbDate.SuspendLayout();
            this.gbTime.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbPresenceInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPresenceInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(9, 449);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 31;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(469, 449);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 32;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbDate
            // 
            this.gbDate.Controls.Add(this.lblDate);
            this.gbDate.Controls.Add(this.dtpDateFrom);
            this.gbDate.Location = new System.Drawing.Point(12, 12);
            this.gbDate.Name = "gbDate";
            this.gbDate.Size = new System.Drawing.Size(389, 73);
            this.gbDate.TabIndex = 44;
            this.gbDate.TabStop = false;
            this.gbDate.Tag = "FILTERABLE";
            this.gbDate.Text = "Date";
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(11, 19);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(61, 23);
            this.lblDate.TabIndex = 46;
            this.lblDate.Text = "Date:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(81, 19);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(126, 20);
            this.dtpDateFrom.TabIndex = 47;
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.label1);
            this.gbTime.Controls.Add(this.lblHrs);
            this.gbTime.Controls.Add(this.lblFrom);
            this.gbTime.Controls.Add(this.dtpHoursFrom);
            this.gbTime.Controls.Add(this.dtpHoursTo);
            this.gbTime.Controls.Add(this.lblTo);
            this.gbTime.Location = new System.Drawing.Point(12, 91);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(389, 73);
            this.gbTime.TabIndex = 45;
            this.gbTime.TabStop = false;
            this.gbTime.Tag = "FILTERABLE";
            this.gbTime.Text = "Time interval";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(147, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 23);
            this.label1.TabIndex = 49;
            this.label1.Text = "-";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHrs
            // 
            this.lblHrs.Location = new System.Drawing.Point(261, 28);
            this.lblHrs.Name = "lblHrs";
            this.lblHrs.Size = new System.Drawing.Size(28, 23);
            this.lblHrs.TabIndex = 48;
            this.lblHrs.Text = "Hrs";
            this.lblHrs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(11, 28);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(64, 23);
            this.lblFrom.TabIndex = 47;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpHoursFrom
            // 
            this.dtpHoursFrom.CustomFormat = "HH:mm";
            this.dtpHoursFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursFrom.Location = new System.Drawing.Point(81, 29);
            this.dtpHoursFrom.Name = "dtpHoursFrom";
            this.dtpHoursFrom.ShowUpDown = true;
            this.dtpHoursFrom.Size = new System.Drawing.Size(53, 20);
            this.dtpHoursFrom.TabIndex = 46;
            // 
            // dtpHoursTo
            // 
            this.dtpHoursTo.CustomFormat = "HH:mm";
            this.dtpHoursTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursTo.Location = new System.Drawing.Point(196, 28);
            this.dtpHoursTo.Name = "dtpHoursTo";
            this.dtpHoursTo.ShowUpDown = true;
            this.dtpHoursTo.Size = new System.Drawing.Size(59, 20);
            this.dtpHoursTo.TabIndex = 45;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(164, 26);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(26, 23);
            this.lblTo.TabIndex = 44;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvWU
            // 
            this.lvWU.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvWU.FullRowSelect = true;
            this.lvWU.GridLines = true;
            this.lvWU.HideSelection = false;
            this.lvWU.Location = new System.Drawing.Point(12, 170);
            this.lvWU.Name = "lvWU";
            this.lvWU.Size = new System.Drawing.Size(389, 169);
            this.lvWU.TabIndex = 46;
            this.lvWU.Tag = "FILTERABLE";
            this.lvWU.UseCompatibleStateImageBehavior = false;
            this.lvWU.View = System.Windows.Forms.View.Details;
            this.lvWU.SelectedIndexChanged += new System.EventHandler(this.lvWU_SelectedIndexChanged);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(407, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 47;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(27, 66);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(5, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // gbPresenceInterval
            // 
            this.gbPresenceInterval.Controls.Add(this.nudPresenceInterval);
            this.gbPresenceInterval.Controls.Add(this.label3);
            this.gbPresenceInterval.Controls.Add(this.lblMinPresence);
            this.gbPresenceInterval.Location = new System.Drawing.Point(12, 345);
            this.gbPresenceInterval.Name = "gbPresenceInterval";
            this.gbPresenceInterval.Size = new System.Drawing.Size(389, 73);
            this.gbPresenceInterval.TabIndex = 48;
            this.gbPresenceInterval.TabStop = false;
            this.gbPresenceInterval.Tag = "FILTERABLE";
            this.gbPresenceInterval.Text = "Presence interval";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(151, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 23);
            this.label3.TabIndex = 48;
            this.label3.Text = "Hrs";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMinPresence
            // 
            this.lblMinPresence.Location = new System.Drawing.Point(6, 28);
            this.lblMinPresence.Name = "lblMinPresence";
            this.lblMinPresence.Size = new System.Drawing.Size(66, 23);
            this.lblMinPresence.TabIndex = 47;
            this.lblMinPresence.Text = "More than:";
            this.lblMinPresence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudPresenceInterval
            // 
            this.nudPresenceInterval.Location = new System.Drawing.Point(78, 31);
            this.nudPresenceInterval.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudPresenceInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPresenceInterval.Name = "nudPresenceInterval";
            this.nudPresenceInterval.Size = new System.Drawing.Size(67, 20);
            this.nudPresenceInterval.TabIndex = 49;
            this.nudPresenceInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ZINPresencePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 482);
            this.ControlBox = false;
            this.Controls.Add(this.gbPresenceInterval);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lvWU);
            this.Controls.Add(this.gbTime);
            this.Controls.Add(this.gbDate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ZINPresencePreview";
            this.ShowInTaskbar = false;
            this.Text = "ZINStatisticPreview";
            this.Load += new System.EventHandler(this.ZINPresencePreview_Load);
            this.gbDate.ResumeLayout(false);
            this.gbTime.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbPresenceInterval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudPresenceInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbDate;
        private System.Windows.Forms.GroupBox gbTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHrs;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpHoursFrom;
        private System.Windows.Forms.DateTimePicker dtpHoursTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.ListView lvWU;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.GroupBox gbPresenceInterval;
        private System.Windows.Forms.NumericUpDown nudPresenceInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMinPresence;
    }
}