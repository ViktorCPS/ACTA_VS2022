namespace Reports.Vlatacom
{
    partial class WholeDayAbsenceAnnualReport
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
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitName = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.chbShowRetired = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.dtpYear = new System.Windows.Forms.DateTimePicker();
            this.lblYear = new System.Windows.Forms.Label();
            this.gbWorkingUnit.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.lblEmployee);
            this.gbWorkingUnit.Controls.Add(this.cbEmployee);
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.gbWorkingUnit.Location = new System.Drawing.Point(12, 12);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(343, 108);
            this.gbWorkingUnit.TabIndex = 8;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working units";
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(20, 72);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(98, 74);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(230, 21);
            this.cbEmployee.TabIndex = 5;
            this.cbEmployee.Tag = "FILTERABLE";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(98, 44);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(82, 24);
            this.chbHierarhicly.TabIndex = 2;
            this.chbHierarhicly.Tag = "FILTERABLE";
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(98, 16);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(230, 21);
            this.cbWorkingUnit.TabIndex = 1;
            this.cbWorkingUnit.Tag = "FILTERABLE";
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(44, 16);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 0;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 197);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 13;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(361, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(149, 91);
            this.gbFilter.TabIndex = 9;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 1;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(137, 21);
            this.cbFilter.TabIndex = 0;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // chbShowRetired
            // 
            this.chbShowRetired.AutoSize = true;
            this.chbShowRetired.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbShowRetired.Location = new System.Drawing.Point(12, 126);
            this.chbShowRetired.Name = "chbShowRetired";
            this.chbShowRetired.Size = new System.Drawing.Size(85, 17);
            this.chbShowRetired.TabIndex = 10;
            this.chbShowRetired.Tag = "FILTERABLE";
            this.chbShowRetired.Text = "Show retired";
            this.chbShowRetired.UseVisualStyleBackColor = true;
            this.chbShowRetired.CheckedChanged += new System.EventHandler(this.chbShowRetired_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(406, 197);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dtpYear
            // 
            this.dtpYear.CustomFormat = "yyyy";
            this.dtpYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpYear.Location = new System.Drawing.Point(110, 157);
            this.dtpYear.Name = "dtpYear";
            this.dtpYear.ShowUpDown = true;
            this.dtpYear.Size = new System.Drawing.Size(120, 20);
            this.dtpYear.TabIndex = 12;
            this.dtpYear.Tag = "FILTERABLE";
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(12, 154);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(92, 23);
            this.lblYear.TabIndex = 11;
            this.lblYear.Text = "Year:";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WholeDayAbsenceAnnualReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 246);
            this.ControlBox = false;
            this.Controls.Add(this.dtpYear);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.gbWorkingUnit);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.chbShowRetired);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "WholeDayAbsenceAnnualReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WholeDayAbsenceAnnualReport";
            this.Load += new System.EventHandler(this.WholeDayAbsenceAnnualReport_Load);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbWorkingUnit;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnitName;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.CheckBox chbShowRetired;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DateTimePicker dtpYear;
        private System.Windows.Forms.Label lblYear;
    }
}