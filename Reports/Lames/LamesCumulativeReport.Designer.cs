namespace Reports.Lames
{
    partial class LamesCumulativeReport
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
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.gbUnitFilter = new System.Windows.Forms.GroupBox();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.gbDateInterval = new System.Windows.Forms.GroupBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbUnitFilter.SuspendLayout();
            this.gbDateInterval.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(12, 126);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(334, 275);
            this.lvEmployees.TabIndex = 3;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(96, 100);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(250, 20);
            this.tbEmployee.TabIndex = 2;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // gbUnitFilter
            // 
            this.gbUnitFilter.Controls.Add(this.cbOU);
            this.gbUnitFilter.Controls.Add(this.cbWU);
            this.gbUnitFilter.Controls.Add(this.rbOU);
            this.gbUnitFilter.Controls.Add(this.rbWU);
            this.gbUnitFilter.Location = new System.Drawing.Point(12, 12);
            this.gbUnitFilter.Name = "gbUnitFilter";
            this.gbUnitFilter.Size = new System.Drawing.Size(334, 82);
            this.gbUnitFilter.TabIndex = 0;
            this.gbUnitFilter.TabStop = false;
            this.gbUnitFilter.Text = "Unit filter";
            // 
            // cbOU
            // 
            this.cbOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(69, 45);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(221, 21);
            this.cbOU.TabIndex = 4;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(69, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(221, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(20, 46);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(41, 17);
            this.rbOU.TabIndex = 3;
            this.rbOU.TabStop = true;
            this.rbOU.Text = "OU";
            this.rbOU.UseVisualStyleBackColor = true;
            this.rbOU.CheckedChanged += new System.EventHandler(this.rbOU_CheckedChanged);
            // 
            // rbWU
            // 
            this.rbWU.AutoSize = true;
            this.rbWU.Location = new System.Drawing.Point(20, 19);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(38, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "FS";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // gbDateInterval
            // 
            this.gbDateInterval.Controls.Add(this.dtpFrom);
            this.gbDateInterval.Controls.Add(this.lblFrom);
            this.gbDateInterval.Controls.Add(this.dtpTo);
            this.gbDateInterval.Controls.Add(this.lblTo);
            this.gbDateInterval.Location = new System.Drawing.Point(352, 321);
            this.gbDateInterval.Name = "gbDateInterval";
            this.gbDateInterval.Size = new System.Drawing.Size(173, 80);
            this.gbDateInterval.TabIndex = 4;
            this.gbDateInterval.TabStop = false;
            this.gbDateInterval.Text = "Date interval";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(48, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 16);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(48, 45);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(21, 42);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 23);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 427);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(114, 23);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(450, 427);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(12, 98);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(78, 23);
            this.lblEmployee.TabIndex = 1;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LamesCumulativeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 483);
            this.ControlBox = false;
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.gbDateInterval);
            this.Controls.Add(this.lvEmployees);
            this.Controls.Add(this.tbEmployee);
            this.Controls.Add(this.gbUnitFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LamesCumulativeReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lames Cumulative Report";
            this.Load += new System.EventHandler(this.LamesCumulativeReport_Load);
            this.gbUnitFilter.ResumeLayout(false);
            this.gbUnitFilter.PerformLayout();
            this.gbDateInterval.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.GroupBox gbUnitFilter;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.GroupBox gbDateInterval;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblEmployee;
    }
}