namespace UI
{
    partial class OnlineMealReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnlineMealReports));
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.rbDaily = new System.Windows.Forms.RadioButton();
            this.rbPeriodical = new System.Windows.Forms.RadioButton();
            this.gbPeriod = new System.Windows.Forms.GroupBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.tbFromTime = new System.Windows.Forms.TextBox();
            this.tbToTime = new System.Windows.Forms.TextBox();
            this.gbFilter.SuspendLayout();
            this.gbPeriod.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnOUTree);
            this.gbFilter.Controls.Add(this.cbOU);
            this.gbFilter.Controls.Add(this.btnWUTree);
            this.gbFilter.Controls.Add(this.cbWU);
            this.gbFilter.Controls.Add(this.rbOU);
            this.gbFilter.Controls.Add(this.rbWU);
            this.gbFilter.Location = new System.Drawing.Point(15, 30);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(437, 87);
            this.gbFilter.TabIndex = 2;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnOUTree
            // 
            this.btnOUTree.Enabled = false;
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(395, 51);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 5;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // cbOU
            // 
            this.cbOU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOU.Enabled = false;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(172, 51);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(217, 21);
            this.cbOU.TabIndex = 4;
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(395, 16);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(172, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(217, 21);
            this.cbWU.TabIndex = 1;
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(6, 51);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(117, 17);
            this.rbOU.TabIndex = 3;
            this.rbOU.Text = "Organizational units";
            this.rbOU.UseVisualStyleBackColor = true;
            this.rbOU.CheckedChanged += new System.EventHandler(this.rbOU_CheckedChanged);
            // 
            // rbWU
            // 
            this.rbWU.AutoSize = true;
            this.rbWU.Checked = true;
            this.rbWU.Location = new System.Drawing.Point(6, 19);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(90, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "Working units";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(25, 30);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 13);
            this.lblFrom.TabIndex = 14;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(72, 24);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(100, 20);
            this.dtpFrom.TabIndex = 15;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(25, 66);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 13);
            this.lblTo.TabIndex = 17;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(72, 62);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(100, 20);
            this.dtpTo.TabIndex = 18;
            // 
            // rbDaily
            // 
            this.rbDaily.AutoSize = true;
            this.rbDaily.Checked = true;
            this.rbDaily.Location = new System.Drawing.Point(21, 143);
            this.rbDaily.Name = "rbDaily";
            this.rbDaily.Size = new System.Drawing.Size(48, 17);
            this.rbDaily.TabIndex = 19;
            this.rbDaily.TabStop = true;
            this.rbDaily.Text = "Daily";
            this.rbDaily.UseVisualStyleBackColor = true;
            this.rbDaily.CheckedChanged += new System.EventHandler(this.rbDaily_CheckedChanged);
            // 
            // rbPeriodical
            // 
            this.rbPeriodical.AutoSize = true;
            this.rbPeriodical.Location = new System.Drawing.Point(21, 180);
            this.rbPeriodical.Name = "rbPeriodical";
            this.rbPeriodical.Size = new System.Drawing.Size(71, 17);
            this.rbPeriodical.TabIndex = 20;
            this.rbPeriodical.Text = "Periodical";
            this.rbPeriodical.UseVisualStyleBackColor = true;
            this.rbPeriodical.CheckedChanged += new System.EventHandler(this.rbDaily_CheckedChanged);
            // 
            // gbPeriod
            // 
            this.gbPeriod.Controls.Add(this.tbToTime);
            this.gbPeriod.Controls.Add(this.tbFromTime);
            this.gbPeriod.Controls.Add(this.lblFrom);
            this.gbPeriod.Controls.Add(this.dtpFrom);
            this.gbPeriod.Controls.Add(this.dtpTo);
            this.gbPeriod.Controls.Add(this.lblTo);
            this.gbPeriod.Enabled = false;
            this.gbPeriod.Location = new System.Drawing.Point(184, 180);
            this.gbPeriod.Name = "gbPeriod";
            this.gbPeriod.Size = new System.Drawing.Size(268, 102);
            this.gbPeriod.TabIndex = 21;
            this.gbPeriod.TabStop = false;
            this.gbPeriod.Text = "Period";
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(184, 145);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(40, 13);
            this.lblDate.TabIndex = 22;
            this.lblDate.Text = "Date:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd.MM.yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(256, 143);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(100, 20);
            this.dtpDate.TabIndex = 23;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(330, 310);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(102, 30);
            this.btnGenerate.TabIndex = 24;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // tbFromTime
            // 
            this.tbFromTime.Location = new System.Drawing.Point(181, 24);
            this.tbFromTime.Name = "tbFromTime";
            this.tbFromTime.Size = new System.Drawing.Size(67, 20);
            this.tbFromTime.TabIndex = 19;
            // 
            // tbToTime
            // 
            this.tbToTime.Location = new System.Drawing.Point(181, 62);
            this.tbToTime.Name = "tbToTime";
            this.tbToTime.Size = new System.Drawing.Size(67, 20);
            this.tbToTime.TabIndex = 20;
            // 
            // OnlineMealReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.gbPeriod);
            this.Controls.Add(this.rbPeriodical);
            this.Controls.Add(this.rbDaily);
            this.Controls.Add(this.gbFilter);
            this.Name = "OnlineMealReports";
            this.Size = new System.Drawing.Size(514, 375);
            this.Load += new System.EventHandler(this.OnlineMealReports_Load);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.gbPeriod.ResumeLayout(false);
            this.gbPeriod.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.RadioButton rbDaily;
        private System.Windows.Forms.RadioButton rbPeriodical;
        private System.Windows.Forms.GroupBox gbPeriod;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox tbToTime;
        private System.Windows.Forms.TextBox tbFromTime;
    }
}
