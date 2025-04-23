namespace Reports.UNIPROM
{
    partial class IOPairsReport
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.cbPersone = new System.Windows.Forms.ComboBox();
            this.cbVechicle = new System.Windows.Forms.ComboBox();
            this.lblDirection = new System.Windows.Forms.Label();
            this.lblPersone = new System.Windows.Forms.Label();
            this.lblVechicle = new System.Windows.Forms.Label();
            this.gbDate = new System.Windows.Forms.GroupBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lvPasses = new System.Windows.Forms.ListView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbGroupByVechicle = new System.Windows.Forms.GroupBox();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.gbSearch.SuspendLayout();
            this.gbDate.SuspendLayout();
            this.gbTime.SuspendLayout();
            this.gbGroupByVechicle.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.cbDirection);
            this.gbSearch.Controls.Add(this.cbPersone);
            this.gbSearch.Controls.Add(this.cbVechicle);
            this.gbSearch.Controls.Add(this.lblDirection);
            this.gbSearch.Controls.Add(this.lblPersone);
            this.gbSearch.Controls.Add(this.lblVechicle);
            this.gbSearch.Controls.Add(this.gbDate);
            this.gbSearch.Controls.Add(this.gbTime);
            this.gbSearch.Location = new System.Drawing.Point(12, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(775, 172);
            this.gbSearch.TabIndex = 5;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Search";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(659, 132);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 23);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Location = new System.Drawing.Point(129, 96);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(109, 21);
            this.cbDirection.TabIndex = 16;
            // 
            // cbPersone
            // 
            this.cbPersone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPersone.Location = new System.Drawing.Point(129, 61);
            this.cbPersone.Name = "cbPersone";
            this.cbPersone.Size = new System.Drawing.Size(248, 21);
            this.cbPersone.TabIndex = 15;
            // 
            // cbVechicle
            // 
            this.cbVechicle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVechicle.Location = new System.Drawing.Point(129, 24);
            this.cbVechicle.Name = "cbVechicle";
            this.cbVechicle.Size = new System.Drawing.Size(248, 21);
            this.cbVechicle.TabIndex = 14;
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(30, 94);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(93, 23);
            this.lblDirection.TabIndex = 13;
            this.lblDirection.Text = "Direction:";
            this.lblDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPersone
            // 
            this.lblPersone.Location = new System.Drawing.Point(30, 59);
            this.lblPersone.Name = "lblPersone";
            this.lblPersone.Size = new System.Drawing.Size(93, 23);
            this.lblPersone.TabIndex = 12;
            this.lblPersone.Text = "Persone:";
            this.lblPersone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVechicle
            // 
            this.lblVechicle.Location = new System.Drawing.Point(30, 22);
            this.lblVechicle.Name = "lblVechicle";
            this.lblVechicle.Size = new System.Drawing.Size(93, 23);
            this.lblVechicle.TabIndex = 11;
            this.lblVechicle.Text = "Vechicle:";
            this.lblVechicle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbDate
            // 
            this.gbDate.Controls.Add(this.dtpFromDate);
            this.gbDate.Controls.Add(this.lblFrom);
            this.gbDate.Controls.Add(this.dtpToDate);
            this.gbDate.Controls.Add(this.lblTo);
            this.gbDate.Location = new System.Drawing.Point(413, 12);
            this.gbDate.Name = "gbDate";
            this.gbDate.Size = new System.Drawing.Size(200, 70);
            this.gbDate.TabIndex = 10;
            this.gbDate.TabStop = false;
            this.gbDate.Text = "Date";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(74, 14);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(28, 14);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(74, 40);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(44, 40);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.dtTo);
            this.gbTime.Controls.Add(this.lblEnd);
            this.gbTime.Controls.Add(this.lblStart);
            this.gbTime.Controls.Add(this.dtFrom);
            this.gbTime.Location = new System.Drawing.Point(413, 88);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(200, 67);
            this.gbTime.TabIndex = 9;
            this.gbTime.TabStop = false;
            this.gbTime.Text = "Time";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "HH:mm";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(74, 37);
            this.dtTo.Name = "dtTo";
            this.dtTo.ShowUpDown = true;
            this.dtTo.Size = new System.Drawing.Size(56, 20);
            this.dtTo.TabIndex = 4;
            this.dtTo.Value = new System.DateTime(2008, 3, 5, 23, 59, 0, 0);
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point(25, 37);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(40, 23);
            this.lblEnd.TabIndex = 3;
            this.lblEnd.Text = "End:";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point(25, 14);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(40, 23);
            this.lblStart.TabIndex = 1;
            this.lblStart.Text = "Start:";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "HH:mm";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(74, 14);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.ShowUpDown = true;
            this.dtFrom.Size = new System.Drawing.Size(56, 20);
            this.dtFrom.TabIndex = 2;
            this.dtFrom.Value = new System.DateTime(2008, 3, 5, 0, 0, 0, 0);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(683, 472);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(100, 33);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(120, 23);
            this.btnGenerate.TabIndex = 17;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lvPasses
            // 
            this.lvPasses.FullRowSelect = true;
            this.lvPasses.GridLines = true;
            this.lvPasses.HideSelection = false;
            this.lvPasses.Location = new System.Drawing.Point(12, 201);
            this.lvPasses.Name = "lvPasses";
            this.lvPasses.ShowItemToolTips = true;
            this.lvPasses.Size = new System.Drawing.Size(775, 232);
            this.lvPasses.TabIndex = 19;
            this.lvPasses.UseCompatibleStateImageBehavior = false;
            this.lvPasses.View = System.Windows.Forms.View.Details;
            this.lvPasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPasses_ColumnClick);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(637, 436);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbGroupByVechicle
            // 
            this.gbGroupByVechicle.Controls.Add(this.rbNo);
            this.gbGroupByVechicle.Controls.Add(this.rbYes);
            this.gbGroupByVechicle.Controls.Add(this.btnGenerate);
            this.gbGroupByVechicle.Location = new System.Drawing.Point(280, 439);
            this.gbGroupByVechicle.Name = "gbGroupByVechicle";
            this.gbGroupByVechicle.Size = new System.Drawing.Size(237, 68);
            this.gbGroupByVechicle.TabIndex = 21;
            this.gbGroupByVechicle.TabStop = false;
            this.gbGroupByVechicle.Text = "Group by vechicle";
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Checked = true;
            this.rbNo.Location = new System.Drawing.Point(10, 43);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(39, 17);
            this.rbNo.TabIndex = 19;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "No";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Location = new System.Drawing.Point(10, 20);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(43, 17);
            this.rbYes.TabIndex = 18;
            this.rbYes.Text = "Yes";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // IOPairsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 512);
            this.ControlBox = false;
            this.Controls.Add(this.gbGroupByVechicle);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lvPasses);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "IOPairsReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "IOPairsReport";
            this.Load += new System.EventHandler(this.IOPairsReport_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbDate.ResumeLayout(false);
            this.gbTime.ResumeLayout(false);
            this.gbGroupByVechicle.ResumeLayout(false);
            this.gbGroupByVechicle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox gbTime;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label lblVechicle;
        private System.Windows.Forms.GroupBox gbDate;
        private System.Windows.Forms.Label lblPersone;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.ComboBox cbPersone;
        private System.Windows.Forms.ComboBox cbVechicle;
        private System.Windows.Forms.ListView lvPasses;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox gbGroupByVechicle;
        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbYes;
    }
}