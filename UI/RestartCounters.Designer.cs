namespace UI
{
    partial class RestartCounters
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
            this.gbPaidLeaves = new System.Windows.Forms.GroupBox();
            this.btnRecalculatePaidLeaves = new System.Windows.Forms.Button();
            this.gbVacationCutOffMonth = new System.Windows.Forms.GroupBox();
            this.btnRecalculateVac = new System.Windows.Forms.Button();
            this.gbBHCuttOffMonths = new System.Windows.Forms.GroupBox();
            this.btnRecalculateBH = new System.Windows.Forms.Button();
            this.gbRestartOvertimeCounter = new System.Windows.Forms.GroupBox();
            this.btnRecalculateOvertime = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbPaidLeaves.SuspendLayout();
            this.gbVacationCutOffMonth.SuspendLayout();
            this.gbBHCuttOffMonths.SuspendLayout();
            this.gbRestartOvertimeCounter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPaidLeaves
            // 
            this.gbPaidLeaves.Controls.Add(this.btnRecalculatePaidLeaves);
            this.gbPaidLeaves.Location = new System.Drawing.Point(12, 210);
            this.gbPaidLeaves.Name = "gbPaidLeaves";
            this.gbPaidLeaves.Size = new System.Drawing.Size(450, 60);
            this.gbPaidLeaves.TabIndex = 4;
            this.gbPaidLeaves.TabStop = false;
            this.gbPaidLeaves.Text = "Paid leaves";
            // 
            // btnRecalculatePaidLeaves
            // 
            this.btnRecalculatePaidLeaves.Location = new System.Drawing.Point(311, 22);
            this.btnRecalculatePaidLeaves.Name = "btnRecalculatePaidLeaves";
            this.btnRecalculatePaidLeaves.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculatePaidLeaves.TabIndex = 0;
            this.btnRecalculatePaidLeaves.Text = "Recalculate";
            this.btnRecalculatePaidLeaves.UseVisualStyleBackColor = true;
            this.btnRecalculatePaidLeaves.Click += new System.EventHandler(this.btnRecalculatePaidLeaves_Click);
            // 
            // gbVacationCutOffMonth
            // 
            this.gbVacationCutOffMonth.Controls.Add(this.btnRecalculateVac);
            this.gbVacationCutOffMonth.Location = new System.Drawing.Point(12, 144);
            this.gbVacationCutOffMonth.Name = "gbVacationCutOffMonth";
            this.gbVacationCutOffMonth.Size = new System.Drawing.Size(450, 60);
            this.gbVacationCutOffMonth.TabIndex = 2;
            this.gbVacationCutOffMonth.TabStop = false;
            this.gbVacationCutOffMonth.Text = "Vacation cut off month";
            // 
            // btnRecalculateVac
            // 
            this.btnRecalculateVac.Location = new System.Drawing.Point(311, 23);
            this.btnRecalculateVac.Name = "btnRecalculateVac";
            this.btnRecalculateVac.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateVac.TabIndex = 0;
            this.btnRecalculateVac.Text = "Recalculate";
            this.btnRecalculateVac.UseVisualStyleBackColor = true;
            this.btnRecalculateVac.Click += new System.EventHandler(this.btnRecalculateVac_Click);
            // 
            // gbBHCuttOffMonths
            // 
            this.gbBHCuttOffMonths.Controls.Add(this.btnRecalculateBH);
            this.gbBHCuttOffMonths.Location = new System.Drawing.Point(12, 78);
            this.gbBHCuttOffMonths.Name = "gbBHCuttOffMonths";
            this.gbBHCuttOffMonths.Size = new System.Drawing.Size(450, 60);
            this.gbBHCuttOffMonths.TabIndex = 1;
            this.gbBHCuttOffMonths.TabStop = false;
            this.gbBHCuttOffMonths.Text = "Bank hour cut off month";
            // 
            // btnRecalculateBH
            // 
            this.btnRecalculateBH.Location = new System.Drawing.Point(311, 22);
            this.btnRecalculateBH.Name = "btnRecalculateBH";
            this.btnRecalculateBH.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateBH.TabIndex = 0;
            this.btnRecalculateBH.Text = "Recalculate";
            this.btnRecalculateBH.UseVisualStyleBackColor = true;
            this.btnRecalculateBH.Click += new System.EventHandler(this.btnRecalculateBH_Click);
            // 
            // gbRestartOvertimeCounter
            // 
            this.gbRestartOvertimeCounter.Controls.Add(this.btnRecalculateOvertime);
            this.gbRestartOvertimeCounter.Location = new System.Drawing.Point(12, 12);
            this.gbRestartOvertimeCounter.Name = "gbRestartOvertimeCounter";
            this.gbRestartOvertimeCounter.Size = new System.Drawing.Size(450, 60);
            this.gbRestartOvertimeCounter.TabIndex = 0;
            this.gbRestartOvertimeCounter.TabStop = false;
            this.gbRestartOvertimeCounter.Text = "Restart overtime counter";
            // 
            // btnRecalculateOvertime
            // 
            this.btnRecalculateOvertime.Location = new System.Drawing.Point(311, 22);
            this.btnRecalculateOvertime.Name = "btnRecalculateOvertime";
            this.btnRecalculateOvertime.Size = new System.Drawing.Size(93, 23);
            this.btnRecalculateOvertime.TabIndex = 0;
            this.btnRecalculateOvertime.Text = "Recalculate";
            this.btnRecalculateOvertime.UseVisualStyleBackColor = true;
            this.btnRecalculateOvertime.Click += new System.EventHandler(this.btnRecalculateOvertime_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(369, 281);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // RestartCounters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 325);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbPaidLeaves);
            this.Controls.Add(this.gbVacationCutOffMonth);
            this.Controls.Add(this.gbBHCuttOffMonths);
            this.Controls.Add(this.gbRestartOvertimeCounter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RestartCounters";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Restart Counters";
            this.Load += new System.EventHandler(this.RestartCounters_Load);
            this.gbPaidLeaves.ResumeLayout(false);
            this.gbVacationCutOffMonth.ResumeLayout(false);
            this.gbBHCuttOffMonths.ResumeLayout(false);
            this.gbRestartOvertimeCounter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPaidLeaves;
        private System.Windows.Forms.Button btnRecalculatePaidLeaves;
        private System.Windows.Forms.GroupBox gbVacationCutOffMonth;
        private System.Windows.Forms.Button btnRecalculateVac;
        private System.Windows.Forms.GroupBox gbBHCuttOffMonths;
        private System.Windows.Forms.Button btnRecalculateBH;
        private System.Windows.Forms.GroupBox gbRestartOvertimeCounter;
        private System.Windows.Forms.Button btnRecalculateOvertime;
        private System.Windows.Forms.Button btnClose;
    }
}