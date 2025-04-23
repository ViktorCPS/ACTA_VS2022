namespace Reports
{
    partial class RoutesReports
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbCriteria = new System.Windows.Forms.GroupBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.cbRouteName = new System.Windows.Forms.ComboBox();
            this.cbEmpl = new System.Windows.Forms.ComboBox();
            this.lblToDate = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblRouteName = new System.Windows.Forms.Label();
            this.lblEmpl = new System.Windows.Forms.Label();
            this.rbSyntetic = new System.Windows.Forms.RadioButton();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.rbAnalytic = new System.Windows.Forms.RadioButton();
            this.lblRepType = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbCriteria.SuspendLayout();
            this.gbReport.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(378, 363);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 363);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(134, 23);
            this.btnGenerate.TabIndex = 17;
            this.btnGenerate.Text = "Generate report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbCriteria
            // 
            this.gbCriteria.Controls.Add(this.cbStatus);
            this.gbCriteria.Controls.Add(this.lblStatus);
            this.gbCriteria.Controls.Add(this.cbWU);
            this.gbCriteria.Controls.Add(this.lblWU);
            this.gbCriteria.Controls.Add(this.dtpToDate);
            this.gbCriteria.Controls.Add(this.dtpFromDate);
            this.gbCriteria.Controls.Add(this.cbRouteName);
            this.gbCriteria.Controls.Add(this.cbEmpl);
            this.gbCriteria.Controls.Add(this.lblToDate);
            this.gbCriteria.Controls.Add(this.lblFromDate);
            this.gbCriteria.Controls.Add(this.lblRouteName);
            this.gbCriteria.Controls.Add(this.lblEmpl);
            this.gbCriteria.Location = new System.Drawing.Point(12, 12);
            this.gbCriteria.Name = "gbCriteria";
            this.gbCriteria.Size = new System.Drawing.Size(298, 231);
            this.gbCriteria.TabIndex = 0;
            this.gbCriteria.TabStop = false;
            this.gbCriteria.Tag = "FILTERABLE";
            this.gbCriteria.Text = "Criteria";
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(103, 123);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(157, 21);
            this.cbStatus.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(20, 124);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(81, 17);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(102, 23);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(158, 21);
            this.cbWU.TabIndex = 2;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(6, 24);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(95, 17);
            this.lblWU.TabIndex = 1;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(102, 198);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(125, 20);
            this.dtpToDate.TabIndex = 12;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(102, 165);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(125, 20);
            this.dtpFromDate.TabIndex = 10;
            // 
            // cbRouteName
            // 
            this.cbRouteName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRouteName.FormattingEnabled = true;
            this.cbRouteName.Location = new System.Drawing.Point(103, 89);
            this.cbRouteName.Name = "cbRouteName";
            this.cbRouteName.Size = new System.Drawing.Size(157, 21);
            this.cbRouteName.TabIndex = 6;
            // 
            // cbEmpl
            // 
            this.cbEmpl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmpl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmpl.FormattingEnabled = true;
            this.cbEmpl.Location = new System.Drawing.Point(102, 56);
            this.cbEmpl.Name = "cbEmpl";
            this.cbEmpl.Size = new System.Drawing.Size(158, 21);
            this.cbEmpl.TabIndex = 4;
            this.cbEmpl.Leave += new System.EventHandler(cbEmpl_Leave);
            // 
            // lblToDate
            // 
            this.lblToDate.Location = new System.Drawing.Point(55, 200);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(41, 17);
            this.lblToDate.TabIndex = 11;
            this.lblToDate.Text = "To:";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Location = new System.Drawing.Point(52, 167);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(44, 17);
            this.lblFromDate.TabIndex = 9;
            this.lblFromDate.Text = "From:";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRouteName
            // 
            this.lblRouteName.Location = new System.Drawing.Point(20, 90);
            this.lblRouteName.Name = "lblRouteName";
            this.lblRouteName.Size = new System.Drawing.Size(81, 17);
            this.lblRouteName.TabIndex = 5;
            this.lblRouteName.Text = "Route:";
            this.lblRouteName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmpl
            // 
            this.lblEmpl.Location = new System.Drawing.Point(20, 57);
            this.lblEmpl.Name = "lblEmpl";
            this.lblEmpl.Size = new System.Drawing.Size(81, 17);
            this.lblEmpl.TabIndex = 3;
            this.lblEmpl.Text = "Employee:";
            this.lblEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbSyntetic
            // 
            this.rbSyntetic.AutoSize = true;
            this.rbSyntetic.Location = new System.Drawing.Point(186, 23);
            this.rbSyntetic.Name = "rbSyntetic";
            this.rbSyntetic.Size = new System.Drawing.Size(63, 17);
            this.rbSyntetic.TabIndex = 16;
            this.rbSyntetic.TabStop = true;
            this.rbSyntetic.Text = "Syntetic";
            this.rbSyntetic.UseVisualStyleBackColor = true;
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.rbSyntetic);
            this.gbReport.Controls.Add(this.rbAnalytic);
            this.gbReport.Controls.Add(this.lblRepType);
            this.gbReport.Location = new System.Drawing.Point(12, 259);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(441, 66);
            this.gbReport.TabIndex = 13;
            this.gbReport.TabStop = false;
            this.gbReport.Tag = "FILTERABLE";
            this.gbReport.Text = "Report";
            // 
            // rbAnalytic
            // 
            this.rbAnalytic.AutoSize = true;
            this.rbAnalytic.Location = new System.Drawing.Point(103, 23);
            this.rbAnalytic.Name = "rbAnalytic";
            this.rbAnalytic.Size = new System.Drawing.Size(62, 17);
            this.rbAnalytic.TabIndex = 15;
            this.rbAnalytic.TabStop = true;
            this.rbAnalytic.Text = "Analytic";
            this.rbAnalytic.UseVisualStyleBackColor = true;
            // 
            // lblRepType
            // 
            this.lblRepType.Location = new System.Drawing.Point(7, 22);
            this.lblRepType.Name = "lblRepType";
            this.lblRepType.Size = new System.Drawing.Size(84, 17);
            this.lblRepType.TabIndex = 14;
            this.lblRepType.Text = "Report type:";
            this.lblRepType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(316, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 33;
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
            // RoutesReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 398);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.gbCriteria);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RoutesReports";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "RoutesReports";
            this.Load += new System.EventHandler(this.RoutesReports_Load);
            this.gbCriteria.ResumeLayout(false);
            this.gbReport.ResumeLayout(false);
            this.gbReport.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void cbEmpl_Leave(object sender, System.EventArgs e)
        {
            if (cbEmpl.SelectedIndex == -1) {
                cbEmpl.SelectedIndex = 0;
            }
        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox gbCriteria;
        private System.Windows.Forms.RadioButton rbSyntetic;
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.RadioButton rbAnalytic;
        private System.Windows.Forms.Label lblRepType;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.ComboBox cbRouteName;
        private System.Windows.Forms.ComboBox cbEmpl;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblRouteName;
        private System.Windows.Forms.Label lblEmpl;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
    }
}