namespace UI
{
    partial class PresenceGraphForEmplControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TransferObjects.WorkingUnitTO workingUnitTO1 = new TransferObjects.WorkingUnitTO();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblIOPairDateChanged = new System.Windows.Forms.Label();
            this.gbMonthNavigation = new System.Windows.Forms.GroupBox();
            this.btnPrevMonth = new System.Windows.Forms.Button();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.gbWorkingUnits.SuspendLayout();
            this.gbEmployees.SuspendLayout();
            this.gbDay.SuspendLayout();
            this.gbGraphicReport.SuspendLayout();
            this.gbLegend.SuspendLayout();
            this.gbDaysNavigation.SuspendLayout();
            this.gbPageNavigation.SuspendLayout();
            this.gbLocation.SuspendLayout();
            this.gbIsWrkHrs.SuspendLayout();
            this.gbMonthNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnits
            // 
            this.gbWorkingUnits.Text = "Search working units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Size = new System.Drawing.Size(75, 17);
            this.chbHierarhicly.Text = "Hierarhicly";
            // 
            // lblParentWUID
            // 
            this.lblParentWUID.Size = new System.Drawing.Size(115, 13);
            this.lblParentWUID.Text = "Parent working unit ID:";
            // 
            // cbParentWorkingUnit
            // 
            this.cbParentWorkingUnit.DisplayMember = "Name";
            workingUnitTO1.AddressID = 0;
            workingUnitTO1.ChildWUNumber = -1;
            workingUnitTO1.Description = "*";
            workingUnitTO1.EmplNumber = -1;
            workingUnitTO1.Name = "*";
            workingUnitTO1.ParentWorkingUID = 0;
            workingUnitTO1.Status = "";
            workingUnitTO1.WorkingUnitID = -1;
            this.cbParentWorkingUnit.Items.AddRange(new object[] {
            workingUnitTO1});
            this.cbParentWorkingUnit.ValueMember = "WorkingUnitID";
            // 
            // gbEmployees
            // 
            this.gbEmployees.Text = "Search employees";
            // 
            // gbDay
            // 
            this.gbDay.Text = "Month";
            // 
            // dtpDay
            // 
            this.dtpDay.CustomFormat = "MMM, yyyy";
            this.dtpDay.Value = new System.DateTime(2011, 12, 12, 12, 50, 48, 223);
            // 
            // gbGraphicReport
            // 
            this.gbGraphicReport.Controls.Add(this.lblDate);
            this.gbGraphicReport.Controls.SetChildIndex(this.lblDate, 0);
            this.gbGraphicReport.Controls.SetChildIndex(this.lblEmployee, 0);
            this.gbGraphicReport.Controls.SetChildIndex(this.lblTotal, 0);
            // 
            // lblTotal
            // 
            this.lblTotal.Size = new System.Drawing.Size(37, 13);
            this.lblTotal.Text = "Total: ";
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(11, 16);
            this.lblEmployee.Size = new System.Drawing.Size(56, 13);
            this.lblEmployee.Text = "Employee:";
            // 
            // lblLate
            // 
            this.lblLate.Size = new System.Drawing.Size(89, 13);
            this.lblLate.Text = "Late/ early arrival";
            // 
            // lblOfficiallyOutgoing
            // 
            this.lblOfficiallyOutgoing.Size = new System.Drawing.Size(0, 13);
            this.lblOfficiallyOutgoing.Text = "";
            // 
            // lblPause
            // 
            this.lblPause.Size = new System.Drawing.Size(40, 13);
            this.lblPause.Text = "Pause:";
            // 
            // gbDaysNavigation
            // 
            this.gbDaysNavigation.Location = new System.Drawing.Point(240, 638);
            this.gbDaysNavigation.Size = new System.Drawing.Size(223, 29);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(11, 16);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(33, 13);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Date:";
            // 
            // lblIOPairDateChanged
            // 
            this.lblIOPairDateChanged.ForeColor = System.Drawing.Color.Red;
            this.lblIOPairDateChanged.Location = new System.Drawing.Point(9, 612);
            this.lblIOPairDateChanged.Name = "lblIOPairDateChanged";
            this.lblIOPairDateChanged.Size = new System.Drawing.Size(216, 34);
            this.lblIOPairDateChanged.TabIndex = 0;
            this.lblIOPairDateChanged.Text = "IOPairDateChanged";
            // 
            // gbMonthNavigation
            // 
            this.gbMonthNavigation.Controls.Add(this.btnPrevMonth);
            this.gbMonthNavigation.Controls.Add(this.btnNextMonth);
            this.gbMonthNavigation.Location = new System.Drawing.Point(240, 628);
            this.gbMonthNavigation.Name = "gbMonthNavigation";
            this.gbMonthNavigation.Size = new System.Drawing.Size(223, 52);
            this.gbMonthNavigation.TabIndex = 12;
            this.gbMonthNavigation.TabStop = false;
            this.gbMonthNavigation.Text = "Month navigation";
            // 
            // btnPrevMonth
            // 
            this.btnPrevMonth.Location = new System.Drawing.Point(9, 23);
            this.btnPrevMonth.Name = "btnPrevMonth";
            this.btnPrevMonth.Size = new System.Drawing.Size(101, 23);
            this.btnPrevMonth.TabIndex = 2;
            this.btnPrevMonth.Text = "<-";
            this.btnPrevMonth.UseVisualStyleBackColor = true;
            this.btnPrevMonth.Click += new System.EventHandler(this.btnPrevMonth_Click);
            // 
            // btnNextMonth
            // 
            this.btnNextMonth.Location = new System.Drawing.Point(116, 23);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(101, 23);
            this.btnNextMonth.TabIndex = 1;
            this.btnNextMonth.Text = "->";
            this.btnNextMonth.UseVisualStyleBackColor = true;
            this.btnNextMonth.Click += new System.EventHandler(this.btnNextMonth_Click);
            // 
            // PresenceGraphForEmplControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMonthNavigation);
            this.Controls.Add(this.lblIOPairDateChanged);
            this.Name = "PresenceGraphForEmplControl";
            this.Load += new System.EventHandler(this.PresenceGraphForEmplControl_Load);
            this.Controls.SetChildIndex(this.gbLegend, 0);
            this.Controls.SetChildIndex(this.gbLocation, 0);
            this.Controls.SetChildIndex(this.gbIsWrkHrs, 0);
            this.Controls.SetChildIndex(this.lblIOPairDateChanged, 0);
            this.Controls.SetChildIndex(this.gbDaysNavigation, 0);
            this.Controls.SetChildIndex(this.gbMonthNavigation, 0);
            this.Controls.SetChildIndex(this.gbWorkingUnits, 0);
            this.Controls.SetChildIndex(this.gbEmployees, 0);
            this.Controls.SetChildIndex(this.gbDay, 0);
            this.Controls.SetChildIndex(this.gbGraphicReport, 0);
            this.Controls.SetChildIndex(this.btnShow, 0);
            this.Controls.SetChildIndex(this.btnPrint, 0);
            this.Controls.SetChildIndex(this.gbPageNavigation, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.gbWorkingUnits.ResumeLayout(false);
            this.gbWorkingUnits.PerformLayout();
            this.gbEmployees.ResumeLayout(false);
            this.gbEmployees.PerformLayout();
            this.gbDay.ResumeLayout(false);
            this.gbDay.PerformLayout();
            this.gbGraphicReport.ResumeLayout(false);
            this.gbGraphicReport.PerformLayout();
            this.gbLegend.ResumeLayout(false);
            this.gbLegend.PerformLayout();
            this.gbDaysNavigation.ResumeLayout(false);
            this.gbPageNavigation.ResumeLayout(false);
            this.gbLocation.ResumeLayout(false);
            this.gbIsWrkHrs.ResumeLayout(false);
            this.gbIsWrkHrs.PerformLayout();
            this.gbMonthNavigation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblIOPairDateChanged;
        private System.Windows.Forms.GroupBox gbMonthNavigation;
        private System.Windows.Forms.Button btnPrevMonth;
        private System.Windows.Forms.Button btnNextMonth;

    }
}
