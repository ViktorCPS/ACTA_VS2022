namespace Reports
{
    partial class TrespassReports
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
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cbEvent = new System.Windows.Forms.ComboBox();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblDirection = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEvent = new System.Windows.Forms.Label();
            this.lblReader = new System.Windows.Forms.Label();
            this.lblGate = new System.Windows.Forms.Label();
            this.cbReader = new System.Windows.Forms.ComboBox();
            this.cbGate = new System.Windows.Forms.ComboBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lvTrespasses = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.gbSearch.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.dtpToDate);
            this.gbSearch.Controls.Add(this.dtpFromDate);
            this.gbSearch.Controls.Add(this.lblFrom);
            this.gbSearch.Controls.Add(this.cbEvent);
            this.gbSearch.Controls.Add(this.cbDirection);
            this.gbSearch.Controls.Add(this.lblEmployee);
            this.gbSearch.Controls.Add(this.lblTo);
            this.gbSearch.Controls.Add(this.lblDirection);
            this.gbSearch.Controls.Add(this.cbEmployee);
            this.gbSearch.Controls.Add(this.lblEvent);
            this.gbSearch.Controls.Add(this.lblReader);
            this.gbSearch.Controls.Add(this.lblGate);
            this.gbSearch.Controls.Add(this.cbReader);
            this.gbSearch.Controls.Add(this.cbGate);
            this.gbSearch.Controls.Add(this.cbLocation);
            this.gbSearch.Controls.Add(this.lblLocation);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Location = new System.Drawing.Point(12, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(702, 180);
            this.gbSearch.TabIndex = 4;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Search";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(461, 117);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 22;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(461, 85);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 21;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(386, 82);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(69, 23);
            this.lblFrom.TabIndex = 20;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEvent
            // 
            this.cbEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEvent.Location = new System.Drawing.Point(461, 54);
            this.cbEvent.Name = "cbEvent";
            this.cbEvent.Size = new System.Drawing.Size(226, 21);
            this.cbEvent.TabIndex = 19;
            this.cbEvent.SelectedIndexChanged += new System.EventHandler(this.cbEvent_SelectedIndexChanged);
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Location = new System.Drawing.Point(110, 115);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(83, 21);
            this.cbDirection.TabIndex = 9;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(355, 20);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(100, 23);
            this.lblEmployee.TabIndex = 8;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(431, 116);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 16;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(18, 113);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(86, 23);
            this.lblDirection.TabIndex = 14;
            this.lblDirection.Text = "Direction:";
            this.lblDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(461, 22);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(226, 21);
            this.cbEmployee.TabIndex = 13;
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            this.cbEmployee.Leave += new System.EventHandler(cbEmployee_Leave);
            // 
            // lblEvent
            // 
            this.lblEvent.Location = new System.Drawing.Point(355, 50);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(100, 23);
            this.lblEvent.TabIndex = 12;
            this.lblEvent.Text = "Event:";
            this.lblEvent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReader
            // 
            this.lblReader.Location = new System.Drawing.Point(14, 84);
            this.lblReader.Name = "lblReader";
            this.lblReader.Size = new System.Drawing.Size(90, 23);
            this.lblReader.TabIndex = 10;
            this.lblReader.Text = "Reader:";
            this.lblReader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGate
            // 
            this.lblGate.Location = new System.Drawing.Point(14, 50);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(90, 23);
            this.lblGate.TabIndex = 6;
            this.lblGate.Text = "Gate:";
            this.lblGate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbReader
            // 
            this.cbReader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReader.Location = new System.Drawing.Point(110, 86);
            this.cbReader.Name = "cbReader";
            this.cbReader.Size = new System.Drawing.Size(226, 21);
            this.cbReader.TabIndex = 11;
            // 
            // cbGate
            // 
            this.cbGate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGate.Location = new System.Drawing.Point(110, 54);
            this.cbGate.Name = "cbGate";
            this.cbGate.Size = new System.Drawing.Size(226, 21);
            this.cbGate.TabIndex = 7;
            this.cbGate.SelectedIndexChanged += new System.EventHandler(this.cbGate_SelectedIndexChanged);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(110, 22);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(226, 21);
            this.cbLocation.TabIndex = 5;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(11, 20);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(93, 23);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(612, 151);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lvTrespasses
            // 
            this.lvTrespasses.FullRowSelect = true;
            this.lvTrespasses.GridLines = true;
            this.lvTrespasses.HideSelection = false;
            this.lvTrespasses.Location = new System.Drawing.Point(12, 207);
            this.lvTrespasses.MultiSelect = false;
            this.lvTrespasses.Name = "lvTrespasses";
            this.lvTrespasses.ShowItemToolTips = true;
            this.lvTrespasses.Size = new System.Drawing.Size(852, 281);
            this.lvTrespasses.TabIndex = 20;
            this.lvTrespasses.UseCompatibleStateImageBehavior = false;
            this.lvTrespasses.View = System.Windows.Forms.View.Details;
            this.lvTrespasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTrespasses_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(789, 521);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(12, 521);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 22;
            this.btnReport.Text = "Generate Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(829, 178);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 24;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(789, 178);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 23;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(712, 491);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 25;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(720, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 106);
            this.gbFilter.TabIndex = 34;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(27, 65);
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
            // TrespassReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 552);
            this.ControlBox = false;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvTrespasses);
            this.Controls.Add(this.gbSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "TrespassReports";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TrespassReports";
            this.Load += new System.EventHandler(this.TrespassReports_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TrespassReports_KeyUp);
            this.gbSearch.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void cbEmployee_Leave(object sender, System.EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
            }
        }

        #endregion

        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label lblEvent;
        private System.Windows.Forms.Label lblReader;
        private System.Windows.Forms.Label lblGate;
        private System.Windows.Forms.ComboBox cbReader;
        private System.Windows.Forms.ComboBox cbGate;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbEvent;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.ListView lvTrespasses;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;

    }
}