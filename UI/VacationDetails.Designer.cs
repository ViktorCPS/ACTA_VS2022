namespace UI
{
    partial class VacationDetails
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
            this.gbEmployeeData = new System.Windows.Forms.GroupBox();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.tbWorkingUnit = new System.Windows.Forms.TextBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.gbSummaryData = new System.Windows.Forms.GroupBox();
            this.lblForCurrentYear = new System.Windows.Forms.Label();
            this.tbLeft = new System.Windows.Forms.TextBox();
            this.tbUsed = new System.Windows.Forms.TextBox();
            this.tbTransmited = new System.Windows.Forms.TextBox();
            this.tbApproved = new System.Windows.Forms.TextBox();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblUsed = new System.Windows.Forms.Label();
            this.lblTransmited = new System.Windows.Forms.Label();
            this.lblApproved = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.gbUsingPlan = new System.Windows.Forms.GroupBox();
            this.tbValidTo = new System.Windows.Forms.TextBox();
            this.lblValidTo = new System.Windows.Forms.Label();
            this.lblUsingPlan = new System.Windows.Forms.Label();
            this.lvUsingPlan = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbUsingDetails = new System.Windows.Forms.GroupBox();
            this.lvUsingDetails = new System.Windows.Forms.ListView();
            this.btnPrintRequest = new System.Windows.Forms.Button();
            this.gbEmployeeData.SuspendLayout();
            this.gbSummaryData.SuspendLayout();
            this.gbUsingPlan.SuspendLayout();
            this.gbUsingDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEmployeeData
            // 
            this.gbEmployeeData.Controls.Add(this.tbEmployee);
            this.gbEmployeeData.Controls.Add(this.tbWorkingUnit);
            this.gbEmployeeData.Controls.Add(this.lblEmployee);
            this.gbEmployeeData.Controls.Add(this.lblWorkingUnit);
            this.gbEmployeeData.Location = new System.Drawing.Point(32, 22);
            this.gbEmployeeData.Name = "gbEmployeeData";
            this.gbEmployeeData.Size = new System.Drawing.Size(505, 100);
            this.gbEmployeeData.TabIndex = 0;
            this.gbEmployeeData.TabStop = false;
            this.gbEmployeeData.Text = "Employee data";
            // 
            // tbEmployee
            // 
            this.tbEmployee.Enabled = false;
            this.tbEmployee.Location = new System.Drawing.Point(145, 25);
            this.tbEmployee.MaxLength = 50;
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(287, 20);
            this.tbEmployee.TabIndex = 9;
            // 
            // tbWorkingUnit
            // 
            this.tbWorkingUnit.Enabled = false;
            this.tbWorkingUnit.Location = new System.Drawing.Point(145, 57);
            this.tbWorkingUnit.MaxLength = 50;
            this.tbWorkingUnit.Name = "tbWorkingUnit";
            this.tbWorkingUnit.Size = new System.Drawing.Size(287, 20);
            this.tbWorkingUnit.TabIndex = 11;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(33, 25);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(100, 23);
            this.lblEmployee.TabIndex = 8;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(33, 57);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(100, 23);
            this.lblWorkingUnit.TabIndex = 10;
            this.lblWorkingUnit.Text = "Working unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbSummaryData
            // 
            this.gbSummaryData.Controls.Add(this.lblForCurrentYear);
            this.gbSummaryData.Controls.Add(this.tbLeft);
            this.gbSummaryData.Controls.Add(this.tbUsed);
            this.gbSummaryData.Controls.Add(this.tbTransmited);
            this.gbSummaryData.Controls.Add(this.tbApproved);
            this.gbSummaryData.Controls.Add(this.tbYear);
            this.gbSummaryData.Controls.Add(this.lblLeft);
            this.gbSummaryData.Controls.Add(this.lblUsed);
            this.gbSummaryData.Controls.Add(this.lblTransmited);
            this.gbSummaryData.Controls.Add(this.lblApproved);
            this.gbSummaryData.Controls.Add(this.lblYear);
            this.gbSummaryData.Location = new System.Drawing.Point(32, 138);
            this.gbSummaryData.Name = "gbSummaryData";
            this.gbSummaryData.Size = new System.Drawing.Size(505, 90);
            this.gbSummaryData.TabIndex = 1;
            this.gbSummaryData.TabStop = false;
            this.gbSummaryData.Text = "Summary data";
            // 
            // lblForCurrentYear
            // 
            this.lblForCurrentYear.Location = new System.Drawing.Point(6, 64);
            this.lblForCurrentYear.Name = "lblForCurrentYear";
            this.lblForCurrentYear.Size = new System.Drawing.Size(284, 23);
            this.lblForCurrentYear.TabIndex = 24;
            this.lblForCurrentYear.Text = "*For current year only";
            this.lblForCurrentYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbLeft
            // 
            this.tbLeft.Enabled = false;
            this.tbLeft.Location = new System.Drawing.Point(421, 42);
            this.tbLeft.MaxLength = 50;
            this.tbLeft.Name = "tbLeft";
            this.tbLeft.Size = new System.Drawing.Size(75, 20);
            this.tbLeft.TabIndex = 18;
            this.tbLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbUsed
            // 
            this.tbUsed.Enabled = false;
            this.tbUsed.Location = new System.Drawing.Point(318, 42);
            this.tbUsed.MaxLength = 50;
            this.tbUsed.Name = "tbUsed";
            this.tbUsed.Size = new System.Drawing.Size(75, 20);
            this.tbUsed.TabIndex = 17;
            this.tbUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbTransmited
            // 
            this.tbTransmited.Enabled = false;
            this.tbTransmited.Location = new System.Drawing.Point(215, 42);
            this.tbTransmited.MaxLength = 50;
            this.tbTransmited.Name = "tbTransmited";
            this.tbTransmited.Size = new System.Drawing.Size(75, 20);
            this.tbTransmited.TabIndex = 16;
            this.tbTransmited.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbApproved
            // 
            this.tbApproved.Enabled = false;
            this.tbApproved.Location = new System.Drawing.Point(112, 42);
            this.tbApproved.MaxLength = 50;
            this.tbApproved.Name = "tbApproved";
            this.tbApproved.Size = new System.Drawing.Size(75, 20);
            this.tbApproved.TabIndex = 15;
            this.tbApproved.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbYear
            // 
            this.tbYear.Enabled = false;
            this.tbYear.Location = new System.Drawing.Point(9, 42);
            this.tbYear.MaxLength = 50;
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(75, 20);
            this.tbYear.TabIndex = 14;
            this.tbYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLeft
            // 
            this.lblLeft.Location = new System.Drawing.Point(421, 16);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(75, 23);
            this.lblLeft.TabIndex = 13;
            this.lblLeft.Text = "Left";
            this.lblLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUsed
            // 
            this.lblUsed.Location = new System.Drawing.Point(318, 16);
            this.lblUsed.Name = "lblUsed";
            this.lblUsed.Size = new System.Drawing.Size(75, 23);
            this.lblUsed.TabIndex = 12;
            this.lblUsed.Text = "Used";
            this.lblUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTransmited
            // 
            this.lblTransmited.Location = new System.Drawing.Point(215, 16);
            this.lblTransmited.Name = "lblTransmited";
            this.lblTransmited.Size = new System.Drawing.Size(75, 23);
            this.lblTransmited.TabIndex = 11;
            this.lblTransmited.Text = "Transmited";
            this.lblTransmited.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblApproved
            // 
            this.lblApproved.Location = new System.Drawing.Point(112, 16);
            this.lblApproved.Name = "lblApproved";
            this.lblApproved.Size = new System.Drawing.Size(75, 23);
            this.lblApproved.TabIndex = 10;
            this.lblApproved.Text = "Approved";
            this.lblApproved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(9, 16);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(75, 23);
            this.lblYear.TabIndex = 9;
            this.lblYear.Text = "Year";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbUsingPlan
            // 
            this.gbUsingPlan.Controls.Add(this.tbValidTo);
            this.gbUsingPlan.Controls.Add(this.lblValidTo);
            this.gbUsingPlan.Controls.Add(this.lblUsingPlan);
            this.gbUsingPlan.Controls.Add(this.lvUsingPlan);
            this.gbUsingPlan.Location = new System.Drawing.Point(32, 234);
            this.gbUsingPlan.Name = "gbUsingPlan";
            this.gbUsingPlan.Size = new System.Drawing.Size(505, 202);
            this.gbUsingPlan.TabIndex = 2;
            this.gbUsingPlan.TabStop = false;
            this.gbUsingPlan.Text = "Using plan*";
            // 
            // tbValidTo
            // 
            this.tbValidTo.Enabled = false;
            this.tbValidTo.Location = new System.Drawing.Point(145, 17);
            this.tbValidTo.MaxLength = 50;
            this.tbValidTo.Name = "tbValidTo";
            this.tbValidTo.Size = new System.Drawing.Size(287, 20);
            this.tbValidTo.TabIndex = 27;
            // 
            // lblValidTo
            // 
            this.lblValidTo.Location = new System.Drawing.Point(33, 15);
            this.lblValidTo.Name = "lblValidTo";
            this.lblValidTo.Size = new System.Drawing.Size(100, 23);
            this.lblValidTo.TabIndex = 26;
            this.lblValidTo.Text = "Valid to:";
            this.lblValidTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUsingPlan
            // 
            this.lblUsingPlan.Location = new System.Drawing.Point(9, 143);
            this.lblUsingPlan.Name = "lblUsingPlan";
            this.lblUsingPlan.Size = new System.Drawing.Size(487, 41);
            this.lblUsingPlan.TabIndex = 25;
            this.lblUsingPlan.Text = "*Using plan";
            this.lblUsingPlan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvUsingPlan
            // 
            this.lvUsingPlan.FullRowSelect = true;
            this.lvUsingPlan.GridLines = true;
            this.lvUsingPlan.Location = new System.Drawing.Point(9, 43);
            this.lvUsingPlan.Name = "lvUsingPlan";
            this.lvUsingPlan.Size = new System.Drawing.Size(484, 97);
            this.lvUsingPlan.TabIndex = 0;
            this.lvUsingPlan.UseCompatibleStateImageBehavior = false;
            this.lvUsingPlan.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(462, 622);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbUsingDetails
            // 
            this.gbUsingDetails.Controls.Add(this.lvUsingDetails);
            this.gbUsingDetails.Location = new System.Drawing.Point(32, 442);
            this.gbUsingDetails.Name = "gbUsingDetails";
            this.gbUsingDetails.Size = new System.Drawing.Size(505, 174);
            this.gbUsingDetails.TabIndex = 13;
            this.gbUsingDetails.TabStop = false;
            this.gbUsingDetails.Text = "Using details";
            // 
            // lvUsingDetails
            // 
            this.lvUsingDetails.FullRowSelect = true;
            this.lvUsingDetails.GridLines = true;
            this.lvUsingDetails.Location = new System.Drawing.Point(12, 19);
            this.lvUsingDetails.Name = "lvUsingDetails";
            this.lvUsingDetails.Size = new System.Drawing.Size(484, 149);
            this.lvUsingDetails.TabIndex = 0;
            this.lvUsingDetails.UseCompatibleStateImageBehavior = false;
            this.lvUsingDetails.View = System.Windows.Forms.View.Details;
            this.lvUsingDetails.SelectedIndexChanged += new System.EventHandler(this.lvUsingDetails_SelectedIndexChanged);
            // 
            // btnPrintRequest
            // 
            this.btnPrintRequest.Location = new System.Drawing.Point(32, 622);
            this.btnPrintRequest.Name = "btnPrintRequest";
            this.btnPrintRequest.Size = new System.Drawing.Size(133, 23);
            this.btnPrintRequest.TabIndex = 48;
            this.btnPrintRequest.Text = "Print request";
            this.btnPrintRequest.Click += new System.EventHandler(this.btnPrintRequest_Click);
            // 
            // VacationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 649);
            this.ControlBox = false;
            this.Controls.Add(this.btnPrintRequest);
            this.Controls.Add(this.gbUsingDetails);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbUsingPlan);
            this.Controls.Add(this.gbSummaryData);
            this.Controls.Add(this.gbEmployeeData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "VacationDetails";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Vacation using details";
            this.Load += new System.EventHandler(this.VacationDetails_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VacationDetails_KeyUp);
            this.gbEmployeeData.ResumeLayout(false);
            this.gbEmployeeData.PerformLayout();
            this.gbSummaryData.ResumeLayout(false);
            this.gbSummaryData.PerformLayout();
            this.gbUsingPlan.ResumeLayout(false);
            this.gbUsingPlan.PerformLayout();
            this.gbUsingDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEmployeeData;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.TextBox tbWorkingUnit;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.GroupBox gbSummaryData;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblUsed;
        private System.Windows.Forms.Label lblTransmited;
        private System.Windows.Forms.Label lblApproved;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.TextBox tbLeft;
        private System.Windows.Forms.TextBox tbUsed;
        private System.Windows.Forms.TextBox tbTransmited;
        private System.Windows.Forms.TextBox tbApproved;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.Label lblForCurrentYear;
        private System.Windows.Forms.GroupBox gbUsingPlan;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblUsingPlan;
        private System.Windows.Forms.GroupBox gbUsingDetails;
        private System.Windows.Forms.ListView lvUsingDetails;
        private System.Windows.Forms.ListView lvUsingPlan;
        private System.Windows.Forms.TextBox tbValidTo;
        private System.Windows.Forms.Label lblValidTo;
        private System.Windows.Forms.Button btnPrintRequest;
    }
}