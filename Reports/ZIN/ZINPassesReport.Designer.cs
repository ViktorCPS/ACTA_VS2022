namespace Reports.ZIN
{
    partial class ZINPassesReport
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
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.gbEmplGroup = new System.Windows.Forms.GroupBox();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.rbCategory = new System.Windows.Forms.RadioButton();
            this.rbWorkingUnit = new System.Windows.Forms.RadioButton();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lvPasses = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnReport = new System.Windows.Forms.Button();
            this.gbSearch.SuspendLayout();
            this.gbEmplGroup.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.gbEmplGroup);
            this.gbSearch.Controls.Add(this.dtpTo);
            this.gbSearch.Controls.Add(this.lblTo);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.dtpFrom);
            this.gbSearch.Controls.Add(this.lblFrom);
            this.gbSearch.Location = new System.Drawing.Point(12, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(610, 164);
            this.gbSearch.TabIndex = 2;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Search";
            // 
            // gbEmplGroup
            // 
            this.gbEmplGroup.Controls.Add(this.cbCategory);
            this.gbEmplGroup.Controls.Add(this.rbCategory);
            this.gbEmplGroup.Controls.Add(this.rbWorkingUnit);
            this.gbEmplGroup.Controls.Add(this.chbHierarhicly);
            this.gbEmplGroup.Controls.Add(this.cbWU);
            this.gbEmplGroup.Location = new System.Drawing.Point(17, 19);
            this.gbEmplGroup.Name = "gbEmplGroup";
            this.gbEmplGroup.Size = new System.Drawing.Size(494, 82);
            this.gbEmplGroup.TabIndex = 24;
            this.gbEmplGroup.TabStop = false;
            this.gbEmplGroup.Text = "Group of employees";
            // 
            // cbCategory
            // 
            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.Enabled = false;
            this.cbCategory.Location = new System.Drawing.Point(134, 51);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(204, 21);
            this.cbCategory.TabIndex = 34;
            // 
            // rbCategory
            // 
            this.rbCategory.Location = new System.Drawing.Point(6, 48);
            this.rbCategory.Name = "rbCategory";
            this.rbCategory.Size = new System.Drawing.Size(122, 24);
            this.rbCategory.TabIndex = 25;
            this.rbCategory.TabStop = true;
            this.rbCategory.Text = "Category:";
            this.rbCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbCategory.UseVisualStyleBackColor = true;
            this.rbCategory.CheckedChanged += new System.EventHandler(this.rbCategory_CheckedChanged);
            // 
            // rbWorkingUnit
            // 
            this.rbWorkingUnit.Checked = true;
            this.rbWorkingUnit.Location = new System.Drawing.Point(6, 16);
            this.rbWorkingUnit.Name = "rbWorkingUnit";
            this.rbWorkingUnit.Size = new System.Drawing.Size(122, 24);
            this.rbWorkingUnit.TabIndex = 24;
            this.rbWorkingUnit.TabStop = true;
            this.rbWorkingUnit.Text = "Working unit:";
            this.rbWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbWorkingUnit.UseVisualStyleBackColor = true;
            this.rbWorkingUnit.CheckedChanged += new System.EventHandler(this.rbWorkingUnit_CheckedChanged);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(390, 16);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(98, 24);
            this.chbHierarhicly.TabIndex = 23;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(134, 19);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(204, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(151, 132);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(160, 20);
            this.dtpTo.TabIndex = 13;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(73, 131);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(72, 23);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(484, 130);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpFrom.Location = new System.Drawing.Point(151, 106);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(160, 20);
            this.dtpFrom.TabIndex = 11;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(57, 106);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(88, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvPasses
            // 
            this.lvPasses.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lvPasses.FullRowSelect = true;
            this.lvPasses.GridLines = true;
            this.lvPasses.HideSelection = false;
            this.lvPasses.Location = new System.Drawing.Point(12, 182);
            this.lvPasses.Name = "lvPasses";
            this.lvPasses.Size = new System.Drawing.Size(759, 216);
            this.lvPasses.TabIndex = 4;
            this.lvPasses.UseCompatibleStateImageBehavior = false;
            this.lvPasses.View = System.Windows.Forms.View.Details;
            this.lvPasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPasses_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(696, 426);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(619, 399);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(739, 153);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 16;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(699, 153);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 15;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(634, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 91);
            this.gbFilter.TabIndex = 29;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
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
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(12, 426);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 30;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // ZINPassesReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 455);
            this.ControlBox = false;
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvPasses);
            this.Controls.Add(this.gbSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ZINPassesReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ZINPassesReport";
            this.Load += new System.EventHandler(this.ZINPassesReport_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbEmplGroup.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.ListView lvPasses;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.GroupBox gbEmplGroup;
        private System.Windows.Forms.RadioButton rbCategory;
        private System.Windows.Forms.RadioButton rbWorkingUnit;
        private System.Windows.Forms.ComboBox cbCategory;
    }
}