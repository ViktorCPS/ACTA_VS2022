namespace Reports {
    partial class OpenPairsReportByEmployees {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMonth = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.lblOrgUnit = new System.Windows.Forms.Label();
            this.cbOrgUnits = new System.Windows.Forms.ComboBox();
            this.cbWorkingUnits = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Location = new System.Drawing.Point(60, 83);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(56, 13);
            this.lblEmployee.TabIndex = 30;
            this.lblEmployee.Text = "Employee:";
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(201, 75);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(263, 21);
            this.cbEmployee.TabIndex = 29;
            this.cbEmployee.Tag = "NOTFILTERABLE";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(13, 217);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 23);
            this.btnGenerate.TabIndex = 27;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(539, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(60, 104);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(56, 20);
            this.lblMonth.TabIndex = 25;
            this.lblMonth.Text = "Month:";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.chbHierarhicly);
            this.gbFilter.Controls.Add(this.dtpMonth);
            this.gbFilter.Controls.Add(this.lblOrgUnit);
            this.gbFilter.Controls.Add(this.cbOrgUnits);
            this.gbFilter.Controls.Add(this.lblMonth);
            this.gbFilter.Controls.Add(this.cbWorkingUnits);
            this.gbFilter.Controls.Add(this.lblWorkingUnit);
            this.gbFilter.Controls.Add(this.cbEmployee);
            this.gbFilter.Controls.Add(this.lblEmployee);
            this.gbFilter.Location = new System.Drawing.Point(13, 13);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(601, 161);
            this.gbFilter.TabIndex = 31;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filters";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(470, 51);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 36;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // dtpMonth
            // 
            this.dtpMonth.CustomFormat = "MM/yyyy";
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(201, 102);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpMonth.Size = new System.Drawing.Size(97, 20);
            this.dtpMonth.TabIndex = 35;
            // 
            // lblOrgUnit
            // 
            this.lblOrgUnit.AutoSize = true;
            this.lblOrgUnit.Location = new System.Drawing.Point(60, 27);
            this.lblOrgUnit.Name = "lblOrgUnit";
            this.lblOrgUnit.Size = new System.Drawing.Size(99, 13);
            this.lblOrgUnit.TabIndex = 34;
            this.lblOrgUnit.Text = "Organizational Unit:";
            // 
            // cbOrgUnits
            // 
            this.cbOrgUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrgUnits.Location = new System.Drawing.Point(201, 19);
            this.cbOrgUnits.Name = "cbOrgUnits";
            this.cbOrgUnits.Size = new System.Drawing.Size(263, 21);
            this.cbOrgUnits.TabIndex = 33;
            this.cbOrgUnits.Tag = "NOTFILTERABLE";
            this.cbOrgUnits.SelectedIndexChanged += new System.EventHandler(this.cbOrgUnits_SelectedIndexChanged);
            // 
            // cbWorkingUnits
            // 
            this.cbWorkingUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnits.Location = new System.Drawing.Point(201, 48);
            this.cbWorkingUnits.Name = "cbWorkingUnits";
            this.cbWorkingUnits.Size = new System.Drawing.Size(263, 21);
            this.cbWorkingUnits.TabIndex = 31;
            this.cbWorkingUnits.Tag = "NOTFILTERABLE";
            this.cbWorkingUnits.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnits_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.AutoSize = true;
            this.lblWorkingUnit.Location = new System.Drawing.Point(60, 56);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(72, 13);
            this.lblWorkingUnit.TabIndex = 32;
            this.lblWorkingUnit.Text = "Working Unit:";
            // 
            // OpenPairsReportByEmployees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 257);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.Name = "OpenPairsReportByEmployees";
            this.Text = "OpenPairsReportByEmployees";
            this.Load += new System.EventHandler(this.OpenPairsReportByEmployees_Load);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label lblOrgUnit;
        private System.Windows.Forms.ComboBox cbOrgUnits;
        private System.Windows.Forms.ComboBox cbWorkingUnits;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.DateTimePicker dtpMonth;
        private System.Windows.Forms.CheckBox chbHierarhicly;
    }
}