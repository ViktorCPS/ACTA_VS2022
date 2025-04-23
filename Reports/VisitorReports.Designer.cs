namespace Reports
{
    partial class VisitorReports
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCurrentVisitors = new System.Windows.Forms.TabPage();
            this.rtbRemarks = new System.Windows.Forms.RichTextBox();
            this.lvVisitorsView = new System.Windows.Forms.ListView();
            this.tabPageVisitSearch = new System.Windows.Forms.TabPage();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnScanDocument = new System.Windows.Forms.Button();
            this.gbVisit = new System.Windows.Forms.GroupBox();
            this.cbVisitor = new System.Windows.Forms.ComboBox();
            this.lblVisitor = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cbVisitDescr = new System.Windows.Forms.ComboBox();
            this.lblVisitDescr = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.cbPerson = new System.Windows.Forms.ComboBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.cbCardNum = new System.Windows.Forms.ComboBox();
            this.lblCardNum = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDetails = new System.Windows.Forms.Button();
            this.rtbRemarksSearch = new System.Windows.Forms.RichTextBox();
            this.lvVisitorsViewSearch = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageCurrentVisitors.SuspendLayout();
            this.tabPageVisitSearch.SuspendLayout();
            this.gbVisit.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageCurrentVisitors);
            this.tabControl1.Controls.Add(this.tabPageVisitSearch);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(780, 590);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageCurrentVisitors
            // 
            this.tabPageCurrentVisitors.Controls.Add(this.rtbRemarks);
            this.tabPageCurrentVisitors.Controls.Add(this.lvVisitorsView);
            this.tabPageCurrentVisitors.Location = new System.Drawing.Point(4, 22);
            this.tabPageCurrentVisitors.Name = "tabPageCurrentVisitors";
            this.tabPageCurrentVisitors.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCurrentVisitors.Size = new System.Drawing.Size(772, 564);
            this.tabPageCurrentVisitors.TabIndex = 0;
            this.tabPageCurrentVisitors.Text = "Current visitors";
            this.tabPageCurrentVisitors.UseVisualStyleBackColor = true;
            // 
            // rtbRemarks
            // 
            this.rtbRemarks.Location = new System.Drawing.Point(8, 446);
            this.rtbRemarks.MaxLength = 132;
            this.rtbRemarks.Name = "rtbRemarks";
            this.rtbRemarks.ReadOnly = true;
            this.rtbRemarks.Size = new System.Drawing.Size(756, 57);
            this.rtbRemarks.TabIndex = 3;
            // 
            // lvVisitorsView
            // 
            this.lvVisitorsView.FullRowSelect = true;
            this.lvVisitorsView.GridLines = true;
            this.lvVisitorsView.HideSelection = false;
            this.lvVisitorsView.Location = new System.Drawing.Point(8, 50);
            this.lvVisitorsView.MultiSelect = false;
            this.lvVisitorsView.Name = "lvVisitorsView";
            this.lvVisitorsView.Size = new System.Drawing.Size(756, 369);
            this.lvVisitorsView.TabIndex = 2;
            this.lvVisitorsView.UseCompatibleStateImageBehavior = false;
            this.lvVisitorsView.View = System.Windows.Forms.View.Details;
            this.lvVisitorsView.SelectedIndexChanged += new System.EventHandler(this.lvVisitorsView_SelectedIndexChanged);
            this.lvVisitorsView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvVisitorsView_ColumnClick);
            // 
            // tabPageVisitSearch
            // 
            this.tabPageVisitSearch.Controls.Add(this.btnReport);
            this.tabPageVisitSearch.Controls.Add(this.btnScanDocument);
            this.tabPageVisitSearch.Controls.Add(this.gbVisit);
            this.tabPageVisitSearch.Controls.Add(this.btnDetails);
            this.tabPageVisitSearch.Controls.Add(this.rtbRemarksSearch);
            this.tabPageVisitSearch.Controls.Add(this.lvVisitorsViewSearch);
            this.tabPageVisitSearch.Location = new System.Drawing.Point(4, 22);
            this.tabPageVisitSearch.Name = "tabPageVisitSearch";
            this.tabPageVisitSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVisitSearch.Size = new System.Drawing.Size(772, 564);
            this.tabPageVisitSearch.TabIndex = 1;
            this.tabPageVisitSearch.Text = "Visit search";
            this.tabPageVisitSearch.UseVisualStyleBackColor = true;
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(401, 525);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 21;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnScanDocument
            // 
            this.btnScanDocument.Location = new System.Drawing.Point(504, 525);
            this.btnScanDocument.Name = "btnScanDocument";
            this.btnScanDocument.Size = new System.Drawing.Size(164, 23);
            this.btnScanDocument.TabIndex = 22;
            this.btnScanDocument.Text = "Scan document";
            this.btnScanDocument.UseVisualStyleBackColor = true;
            this.btnScanDocument.Click += new System.EventHandler(this.btnScanDocument_Click);
            // 
            // gbVisit
            // 
            this.gbVisit.Controls.Add(this.cbVisitor);
            this.gbVisit.Controls.Add(this.lblVisitor);
            this.gbVisit.Controls.Add(this.dtpToDate);
            this.gbVisit.Controls.Add(this.lblTo);
            this.gbVisit.Controls.Add(this.dtpFromDate);
            this.gbVisit.Controls.Add(this.lblFrom);
            this.gbVisit.Controls.Add(this.cbVisitDescr);
            this.gbVisit.Controls.Add(this.lblVisitDescr);
            this.gbVisit.Controls.Add(this.lblEmployee);
            this.gbVisit.Controls.Add(this.lblWU);
            this.gbVisit.Controls.Add(this.cbPerson);
            this.gbVisit.Controls.Add(this.cbWorkingUnit);
            this.gbVisit.Controls.Add(this.cbCardNum);
            this.gbVisit.Controls.Add(this.lblCardNum);
            this.gbVisit.Controls.Add(this.btnSearch);
            this.gbVisit.Location = new System.Drawing.Point(8, 8);
            this.gbVisit.Name = "gbVisit";
            this.gbVisit.Size = new System.Drawing.Size(756, 180);
            this.gbVisit.TabIndex = 3;
            this.gbVisit.TabStop = false;
            this.gbVisit.Text = "Visit search";
            // 
            // cbVisitor
            // 
            this.cbVisitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVisitor.Location = new System.Drawing.Point(496, 52);
            this.cbVisitor.Name = "cbVisitor";
            this.cbVisitor.Size = new System.Drawing.Size(226, 21);
            this.cbVisitor.TabIndex = 9;
            this.cbVisitor.MouseHover += new System.EventHandler(this.cbVisitor_MouseHover);
            this.cbVisitor.SelectedIndexChanged += new System.EventHandler(this.cbVisitor_SelectedIndexChanged);
            this.cbVisitor.MouseLeave += new System.EventHandler(this.cbVisitor_MouseLeave);
            // 
            // lblVisitor
            // 
            this.lblVisitor.Location = new System.Drawing.Point(390, 52);
            this.lblVisitor.Name = "lblVisitor";
            this.lblVisitor.Size = new System.Drawing.Size(100, 23);
            this.lblVisitor.TabIndex = 8;
            this.lblVisitor.Text = "Visitor:";
            this.lblVisitor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(496, 116);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 17;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(466, 115);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 16;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(144, 116);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 15;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(96, 113);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 14;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbVisitDescr
            // 
            this.cbVisitDescr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVisitDescr.Location = new System.Drawing.Point(496, 84);
            this.cbVisitDescr.Name = "cbVisitDescr";
            this.cbVisitDescr.Size = new System.Drawing.Size(226, 21);
            this.cbVisitDescr.TabIndex = 13;
            // 
            // lblVisitDescr
            // 
            this.lblVisitDescr.Location = new System.Drawing.Point(390, 84);
            this.lblVisitDescr.Name = "lblVisitDescr";
            this.lblVisitDescr.Size = new System.Drawing.Size(100, 23);
            this.lblVisitDescr.TabIndex = 12;
            this.lblVisitDescr.Text = "Visit purpose:";
            this.lblVisitDescr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(8, 84);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(128, 23);
            this.lblEmployee.TabIndex = 10;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(8, 52);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(128, 23);
            this.lblWU.TabIndex = 6;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPerson
            // 
            this.cbPerson.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbPerson.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPerson.Location = new System.Drawing.Point(144, 84);
            this.cbPerson.Name = "cbPerson";
            this.cbPerson.Size = new System.Drawing.Size(226, 21);
            this.cbPerson.TabIndex = 11;
            this.cbPerson.Leave += new System.EventHandler(cbPerson_Leave);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(144, 52);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(226, 21);
            this.cbWorkingUnit.TabIndex = 7;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // cbCardNum
            // 
            this.cbCardNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCardNum.Location = new System.Drawing.Point(144, 20);
            this.cbCardNum.Name = "cbCardNum";
            this.cbCardNum.Size = new System.Drawing.Size(226, 21);
            this.cbCardNum.TabIndex = 5;
            // 
            // lblCardNum
            // 
            this.lblCardNum.Location = new System.Drawing.Point(8, 20);
            this.lblCardNum.Name = "lblCardNum";
            this.lblCardNum.Size = new System.Drawing.Size(128, 23);
            this.lblCardNum.TabIndex = 4;
            this.lblCardNum.Text = "Assigned tag:";
            this.lblCardNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(675, 151);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point(689, 525);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(75, 23);
            this.btnDetails.TabIndex = 23;
            this.btnDetails.Text = "Details ...";
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // rtbRemarksSearch
            // 
            this.rtbRemarksSearch.Location = new System.Drawing.Point(8, 450);
            this.rtbRemarksSearch.MaxLength = 132;
            this.rtbRemarksSearch.Name = "rtbRemarksSearch";
            this.rtbRemarksSearch.ReadOnly = true;
            this.rtbRemarksSearch.Size = new System.Drawing.Size(756, 57);
            this.rtbRemarksSearch.TabIndex = 20;
            // 
            // lvVisitorsViewSearch
            // 
            this.lvVisitorsViewSearch.FullRowSelect = true;
            this.lvVisitorsViewSearch.GridLines = true;
            this.lvVisitorsViewSearch.HideSelection = false;
            this.lvVisitorsViewSearch.Location = new System.Drawing.Point(8, 200);
            this.lvVisitorsViewSearch.MultiSelect = false;
            this.lvVisitorsViewSearch.Name = "lvVisitorsViewSearch";
            this.lvVisitorsViewSearch.Size = new System.Drawing.Size(756, 232);
            this.lvVisitorsViewSearch.TabIndex = 19;
            this.lvVisitorsViewSearch.UseCompatibleStateImageBehavior = false;
            this.lvVisitorsViewSearch.View = System.Windows.Forms.View.Details;
            this.lvVisitorsViewSearch.SelectedIndexChanged += new System.EventHandler(this.lvVisitorsViewSearch_SelectedIndexChanged);
            this.lvVisitorsViewSearch.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvVisitorsViewSearch_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(713, 610);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // VisitorReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 632);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 670);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 670);
            this.Name = "VisitorReports";
            this.ShowInTaskbar = false;
            this.Text = "Visitors";
            this.Load += new System.EventHandler(this.VisitorReports_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VisitorReports_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tabPageCurrentVisitors.ResumeLayout(false);
            this.tabPageVisitSearch.ResumeLayout(false);
            this.gbVisit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void cbPerson_Leave(object sender, System.EventArgs e)
        {
            if (cbPerson.SelectedIndex == -1) {
                cbPerson.SelectedIndex = 0;
            }
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageCurrentVisitors;
        private System.Windows.Forms.TabPage tabPageVisitSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvVisitorsView;
        private System.Windows.Forms.RichTextBox rtbRemarks;
        private System.Windows.Forms.RichTextBox rtbRemarksSearch;
        private System.Windows.Forms.ListView lvVisitorsViewSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.GroupBox gbVisit;
        private System.Windows.Forms.ComboBox cbCardNum;
        private System.Windows.Forms.Label lblCardNum;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ComboBox cbPerson;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.ComboBox cbVisitDescr;
        private System.Windows.Forms.Label lblVisitDescr;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.ComboBox cbVisitor;
        private System.Windows.Forms.Label lblVisitor;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnScanDocument;
        private System.Windows.Forms.Button btnReport;
    }
}