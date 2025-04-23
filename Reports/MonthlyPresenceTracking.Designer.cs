namespace Reports
{
    partial class MonthlyPresenceTracking
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
            this.dtpMesec = new System.Windows.Forms.DateTimePicker();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.gbChooseMonth = new System.Windows.Forms.GroupBox();
            this.gbChooseMonth.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpMesec
            // 
            this.dtpMesec.CustomFormat = "MM/yyyy";
            this.dtpMesec.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMesec.Location = new System.Drawing.Point(53, 44);
            this.dtpMesec.Name = "dtpMesec";
            this.dtpMesec.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpMesec.Size = new System.Drawing.Size(97, 20);
            this.dtpMesec.TabIndex = 1;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(53, 87);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(97, 23);
            this.btnGenerateReport.TabIndex = 2;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // gbChooseMonth
            // 
            this.gbChooseMonth.AutoSize = true;
            this.gbChooseMonth.Controls.Add(this.btnGenerateReport);
            this.gbChooseMonth.Controls.Add(this.dtpMesec);
            this.gbChooseMonth.Location = new System.Drawing.Point(12, 21);
            this.gbChooseMonth.Name = "gbChooseMonth";
            this.gbChooseMonth.Size = new System.Drawing.Size(213, 129);
            this.gbChooseMonth.TabIndex = 3;
            this.gbChooseMonth.TabStop = false;
            this.gbChooseMonth.Text = "Choose month:";
            // 
            // MonthlyPresenceTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(236, 155);
            this.Controls.Add(this.gbChooseMonth);
            this.Name = "MonthlyPresenceTracking";
            this.Text = "MonthlyPresenceTracking";
            this.Load += new System.EventHandler(this.MonthlyPresenceTracking_Load);
            this.gbChooseMonth.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpMesec;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.GroupBox gbChooseMonth;
    }
}