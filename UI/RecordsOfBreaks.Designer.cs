namespace UI
{
    partial class RecordsOfBreaks
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
            this.gbRecordsOfBreaks = new System.Windows.Forms.GroupBox();
            this.cbGate = new System.Windows.Forms.ComboBox();
            this.lblGate = new System.Windows.Forms.Label();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.lvRecords = new System.Windows.Forms.ListView();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbRecordsOfBreaks.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRecordsOfBreaks
            // 
            this.gbRecordsOfBreaks.Controls.Add(this.cbGate);
            this.gbRecordsOfBreaks.Controls.Add(this.lblGate);
            this.gbRecordsOfBreaks.Controls.Add(this.chbHierarhicly);
            this.gbRecordsOfBreaks.Controls.Add(this.btnWUTree);
            this.gbRecordsOfBreaks.Controls.Add(this.cbEmployee);
            this.gbRecordsOfBreaks.Controls.Add(this.lblEmployee);
            this.gbRecordsOfBreaks.Controls.Add(this.cbWU);
            this.gbRecordsOfBreaks.Controls.Add(this.dtpTo);
            this.gbRecordsOfBreaks.Controls.Add(this.lblTo);
            this.gbRecordsOfBreaks.Controls.Add(this.btnSearch);
            this.gbRecordsOfBreaks.Controls.Add(this.dtpFrom);
            this.gbRecordsOfBreaks.Controls.Add(this.lblFrom);
            this.gbRecordsOfBreaks.Controls.Add(this.lblWU);
            this.gbRecordsOfBreaks.Location = new System.Drawing.Point(12, 12);
            this.gbRecordsOfBreaks.Name = "gbRecordsOfBreaks";
            this.gbRecordsOfBreaks.Size = new System.Drawing.Size(610, 198);
            this.gbRecordsOfBreaks.TabIndex = 2;
            this.gbRecordsOfBreaks.TabStop = false;
            this.gbRecordsOfBreaks.Tag = "FILTERABLE";
            this.gbRecordsOfBreaks.Text = "Record of Breaks";
            // 
            // cbGate
            // 
            this.cbGate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGate.Location = new System.Drawing.Point(432, 54);
            this.cbGate.Name = "cbGate";
            this.cbGate.Size = new System.Drawing.Size(160, 21);
            this.cbGate.TabIndex = 27;
            // 
            // lblGate
            // 
            this.lblGate.Location = new System.Drawing.Point(338, 53);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(88, 23);
            this.lblGate.TabIndex = 26;
            this.lblGate.Text = "Gate:";
            this.lblGate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(112, 51);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 23;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Location = new System.Drawing.Point(322, 22);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 20;
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(432, 26);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(160, 21);
            this.cbEmployee.TabIndex = 3;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(354, 24);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 2;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(112, 24);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(204, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(112, 119);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(160, 20);
            this.dtpTo.TabIndex = 13;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(34, 116);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(72, 23);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(482, 161);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpFrom.Location = new System.Drawing.Point(112, 93);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(160, 20);
            this.dtpFrom.TabIndex = 11;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(18, 92);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(88, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(16, 24);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(88, 23);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvRecords
            // 
            this.lvRecords.FullRowSelect = true;
            this.lvRecords.GridLines = true;
            this.lvRecords.HideSelection = false;
            this.lvRecords.Location = new System.Drawing.Point(12, 216);
            this.lvRecords.Name = "lvRecords";
            this.lvRecords.Size = new System.Drawing.Size(610, 216);
            this.lvRecords.TabIndex = 4;
            this.lvRecords.UseCompatibleStateImageBehavior = false;
            this.lvRecords.View = System.Windows.Forms.View.Details;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(12, 443);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(115, 23);
            this.btnReport.TabIndex = 11;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(507, 443);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // RecordsOfBreaks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 479);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvRecords);
            this.Controls.Add(this.gbRecordsOfBreaks);
            this.Name = "RecordsOfBreaks";
            this.Text = "RecordsOfBreaks";
            this.Load += new System.EventHandler(this.RecordsOfBreaks_Load);
            this.gbRecordsOfBreaks.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbRecordsOfBreaks;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ListView lvRecords;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cbGate;
        private System.Windows.Forms.Label lblGate;
    }
}