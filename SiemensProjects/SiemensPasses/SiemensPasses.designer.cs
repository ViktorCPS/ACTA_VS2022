namespace SiemensPasses
{
    partial class SiemensPasses
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
            this.lvPasses = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.lblRemark = new System.Windows.Forms.Label();
            this.tbRemarkSearch = new System.Windows.Forms.TextBox();
            this.cbCreated = new System.Windows.Forms.ComboBox();
            this.lblCreated = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.lblDirection = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAdd = new System.Windows.Forms.TabPage();
            this.gbRemark = new System.Windows.Forms.GroupBox();
            this.tbRemark = new System.Windows.Forms.TextBox();
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.gbEmployee = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFirstName = new System.Windows.Forms.ComboBox();
            this.lblID = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.cbID = new System.Windows.Forms.ComboBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.cbLastName = new System.Windows.Forms.ComboBox();
            this.btnAddOut = new System.Windows.Forms.Button();
            this.btnAddIN = new System.Windows.Forms.Button();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.gbFilter.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabAdd.SuspendLayout();
            this.gbRemark.SuspendLayout();
            this.gbTime.SuspendLayout();
            this.gbEmployee.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvPasses
            // 
            this.lvPasses.FullRowSelect = true;
            this.lvPasses.GridLines = true;
            this.lvPasses.Location = new System.Drawing.Point(15, 157);
            this.lvPasses.MultiSelect = false;
            this.lvPasses.Name = "lvPasses";
            this.lvPasses.ShowItemToolTips = true;
            this.lvPasses.Size = new System.Drawing.Size(690, 295);
            this.lvPasses.TabIndex = 3;
            this.lvPasses.UseCompatibleStateImageBehavior = false;
            this.lvPasses.View = System.Windows.Forms.View.Details;
            this.lvPasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPasses_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(658, 549);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(8, 22);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(70, 20);
            this.lblEmployee.TabIndex = 0;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.lblRemark);
            this.gbFilter.Controls.Add(this.tbRemarkSearch);
            this.gbFilter.Controls.Add(this.cbCreated);
            this.gbFilter.Controls.Add(this.lblCreated);
            this.gbFilter.Controls.Add(this.btnSearch);
            this.gbFilter.Controls.Add(this.dtpTo);
            this.gbFilter.Controls.Add(this.dtpFrom);
            this.gbFilter.Controls.Add(this.lblTo);
            this.gbFilter.Controls.Add(this.lblFrom);
            this.gbFilter.Controls.Add(this.cbDirection);
            this.gbFilter.Controls.Add(this.lblDirection);
            this.gbFilter.Controls.Add(this.cbEmployee);
            this.gbFilter.Controls.Add(this.lblEmployee);
            this.gbFilter.Location = new System.Drawing.Point(15, 6);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(687, 116);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Search criteria";
            // 
            // lblRemark
            // 
            this.lblRemark.Location = new System.Drawing.Point(310, 79);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(69, 13);
            this.lblRemark.TabIndex = 8;
            this.lblRemark.Text = "Remark:";
            this.lblRemark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRemarkSearch
            // 
            this.tbRemarkSearch.Location = new System.Drawing.Point(385, 76);
            this.tbRemarkSearch.MaxLength = 7;
            this.tbRemarkSearch.Name = "tbRemarkSearch";
            this.tbRemarkSearch.Size = new System.Drawing.Size(108, 20);
            this.tbRemarkSearch.TabIndex = 9;
            // 
            // cbCreated
            // 
            this.cbCreated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCreated.FormattingEnabled = true;
            this.cbCreated.Location = new System.Drawing.Point(84, 76);
            this.cbCreated.Name = "cbCreated";
            this.cbCreated.Size = new System.Drawing.Size(109, 21);
            this.cbCreated.TabIndex = 9;
            // 
            // lblCreated
            // 
            this.lblCreated.Location = new System.Drawing.Point(11, 76);
            this.lblCreated.Name = "lblCreated";
            this.lblCreated.Size = new System.Drawing.Size(67, 18);
            this.lblCreated.TabIndex = 8;
            this.lblCreated.Text = "Created:";
            this.lblCreated.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(606, 74);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy.";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(385, 49);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(108, 20);
            this.dtpTo.TabIndex = 7;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy.";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(385, 23);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(108, 20);
            this.dtpFrom.TabIndex = 5;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(344, 52);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(35, 13);
            this.lblTo.TabIndex = 6;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(344, 23);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(35, 18);
            this.lblFrom.TabIndex = 4;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Location = new System.Drawing.Point(84, 49);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(109, 21);
            this.cbDirection.TabIndex = 3;
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(11, 49);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(67, 18);
            this.lblDirection.TabIndex = 2;
            this.lblDirection.Text = "Direction:";
            this.lblDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.FormattingEnabled = true;
            this.cbEmployee.Location = new System.Drawing.Point(84, 23);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(208, 21);
            this.cbEmployee.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAdd);
            this.tabControl.Controls.Add(this.tabSearch);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(725, 531);
            this.tabControl.TabIndex = 0;
            // 
            // tabAdd
            // 
            this.tabAdd.Controls.Add(this.gbRemark);
            this.tabAdd.Controls.Add(this.gbTime);
            this.tabAdd.Controls.Add(this.gbEmployee);
            this.tabAdd.Controls.Add(this.btnAddOut);
            this.tabAdd.Controls.Add(this.btnAddIN);
            this.tabAdd.Location = new System.Drawing.Point(4, 22);
            this.tabAdd.Name = "tabAdd";
            this.tabAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdd.Size = new System.Drawing.Size(717, 505);
            this.tabAdd.TabIndex = 1;
            this.tabAdd.Text = "Add pass";
            this.tabAdd.UseVisualStyleBackColor = true;
            // 
            // gbRemark
            // 
            this.gbRemark.Controls.Add(this.tbRemark);
            this.gbRemark.Location = new System.Drawing.Point(218, 153);
            this.gbRemark.Name = "gbRemark";
            this.gbRemark.Size = new System.Drawing.Size(192, 76);
            this.gbRemark.TabIndex = 2;
            this.gbRemark.TabStop = false;
            this.gbRemark.Text = "Remark";
            // 
            // tbRemark
            // 
            this.tbRemark.Location = new System.Drawing.Point(12, 29);
            this.tbRemark.MaxLength = 7;
            this.tbRemark.Name = "tbRemark";
            this.tbRemark.Size = new System.Drawing.Size(150, 20);
            this.tbRemark.TabIndex = 0;
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.label4);
            this.gbTime.Controls.Add(this.dtpTime);
            this.gbTime.Location = new System.Drawing.Point(6, 147);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(192, 82);
            this.gbTime.TabIndex = 1;
            this.gbTime.TabStop = false;
            this.gbTime.Text = "Time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(165, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "*";
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "dd.MM.yyyy. HH:mm";
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(12, 29);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(150, 20);
            this.dtpTime.TabIndex = 0;
            // 
            // gbEmployee
            // 
            this.gbEmployee.Controls.Add(this.label3);
            this.gbEmployee.Controls.Add(this.label2);
            this.gbEmployee.Controls.Add(this.label1);
            this.gbEmployee.Controls.Add(this.cbFirstName);
            this.gbEmployee.Controls.Add(this.lblID);
            this.gbEmployee.Controls.Add(this.lblFirstName);
            this.gbEmployee.Controls.Add(this.tbLastName);
            this.gbEmployee.Controls.Add(this.lblLastName);
            this.gbEmployee.Controls.Add(this.tbFirstName);
            this.gbEmployee.Controls.Add(this.cbID);
            this.gbEmployee.Controls.Add(this.tbID);
            this.gbEmployee.Controls.Add(this.cbLastName);
            this.gbEmployee.Location = new System.Drawing.Point(6, 5);
            this.gbEmployee.Name = "gbEmployee";
            this.gbEmployee.Size = new System.Drawing.Size(688, 136);
            this.gbEmployee.TabIndex = 0;
            this.gbEmployee.TabStop = false;
            this.gbEmployee.Text = "Employee";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(592, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(394, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(194, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "*";
            // 
            // cbFirstName
            // 
            this.cbFirstName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbFirstName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbFirstName.FormattingEnabled = true;
            this.cbFirstName.Location = new System.Drawing.Point(212, 51);
            this.cbFirstName.Name = "cbFirstName";
            this.cbFirstName.Size = new System.Drawing.Size(180, 21);
            this.cbFirstName.TabIndex = 4;
            this.cbFirstName.SelectedIndexChanged += new System.EventHandler(this.cbFirstName_SelectedIndexChanged);
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(9, 26);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(21, 13);
            this.lblID.TabIndex = 0;
            this.lblID.Text = "ID:";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(209, 26);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(58, 13);
            this.lblFirstName.TabIndex = 1;
            this.lblFirstName.Text = "First name:";
            // 
            // tbLastName
            // 
            this.tbLastName.Enabled = false;
            this.tbLastName.Location = new System.Drawing.Point(410, 88);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(180, 20);
            this.tbLastName.TabIndex = 10;
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(407, 26);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(59, 13);
            this.lblLastName.TabIndex = 2;
            this.lblLastName.Text = "Last name:";
            // 
            // tbFirstName
            // 
            this.tbFirstName.Enabled = false;
            this.tbFirstName.Location = new System.Drawing.Point(212, 88);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(180, 20);
            this.tbFirstName.TabIndex = 8;
            // 
            // cbID
            // 
            this.cbID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbID.FormattingEnabled = true;
            this.cbID.Location = new System.Drawing.Point(12, 51);
            this.cbID.Name = "cbID";
            this.cbID.Size = new System.Drawing.Size(180, 21);
            this.cbID.TabIndex = 3;
            this.cbID.SelectedIndexChanged += new System.EventHandler(this.cbID_SelectedIndexChanged);
            // 
            // tbID
            // 
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(12, 88);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(180, 20);
            this.tbID.TabIndex = 6;
            // 
            // cbLastName
            // 
            this.cbLastName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbLastName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbLastName.FormattingEnabled = true;
            this.cbLastName.Location = new System.Drawing.Point(410, 51);
            this.cbLastName.Name = "cbLastName";
            this.cbLastName.Size = new System.Drawing.Size(180, 21);
            this.cbLastName.TabIndex = 5;
            this.cbLastName.SelectedIndexChanged += new System.EventHandler(this.cbLastName_SelectedIndexChanged);
            // 
            // btnAddOut
            // 
            this.btnAddOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddOut.Location = new System.Drawing.Point(514, 195);
            this.btnAddOut.Name = "btnAddOut";
            this.btnAddOut.Size = new System.Drawing.Size(180, 23);
            this.btnAddOut.TabIndex = 4;
            this.btnAddOut.Text = "Add OUT";
            this.btnAddOut.UseVisualStyleBackColor = true;
            this.btnAddOut.Click += new System.EventHandler(this.btnAddOut_Click);
            // 
            // btnAddIN
            // 
            this.btnAddIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddIN.Location = new System.Drawing.Point(514, 156);
            this.btnAddIN.Name = "btnAddIN";
            this.btnAddIN.Size = new System.Drawing.Size(180, 23);
            this.btnAddIN.TabIndex = 3;
            this.btnAddIN.Text = "Add IN";
            this.btnAddIN.UseVisualStyleBackColor = true;
            this.btnAddIN.Click += new System.EventHandler(this.btnAddIN_Click);
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.btnExport);
            this.tabSearch.Controls.Add(this.lblTotal);
            this.tabSearch.Controls.Add(this.btnNext);
            this.tabSearch.Controls.Add(this.btnPrev);
            this.tabSearch.Controls.Add(this.gbFilter);
            this.tabSearch.Controls.Add(this.lvPasses);
            this.tabSearch.Location = new System.Drawing.Point(4, 22);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabSearch.Size = new System.Drawing.Size(717, 505);
            this.tabSearch.TabIndex = 0;
            this.tabSearch.Text = "Search passes";
            this.tabSearch.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(15, 476);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(119, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(526, 455);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(176, 13);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(665, 128);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(37, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(622, 128);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(37, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.bntPrev_Click);
            // 
            // SiemensPasses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 584);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SiemensPasses";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Siemens Passes";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SiemensPasses_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SiemensPasses_FormClosing);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabAdd.ResumeLayout(false);
            this.gbRemark.ResumeLayout(false);
            this.gbRemark.PerformLayout();
            this.gbTime.ResumeLayout(false);
            this.gbTime.PerformLayout();
            this.gbEmployee.ResumeLayout(false);
            this.gbEmployee.PerformLayout();
            this.tabSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvPasses;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.TabPage tabAdd;
        private System.Windows.Forms.ComboBox cbID;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.ComboBox cbLastName;
        private System.Windows.Forms.ComboBox cbFirstName;
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.TextBox tbFirstName;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.GroupBox gbEmployee;
        private System.Windows.Forms.Button btnAddOut;
        private System.Windows.Forms.Button btnAddIN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.GroupBox gbTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ComboBox cbCreated;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.GroupBox gbRemark;
        private System.Windows.Forms.TextBox tbRemark;
        private System.Windows.Forms.Label lblRemark;
        private System.Windows.Forms.TextBox tbRemarkSearch;
    }
}