namespace Reports
{
    partial class ReportForSalaryForecast
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
            this.gbChooseMonth = new System.Windows.Forms.GroupBox();
            this.btnSmanjiBankuSati = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelMessage = new System.Windows.Forms.Label();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.dtpMesec = new System.Windows.Forms.DateTimePicker();
            //this.clockDataDS1 = new ReportsWeb.ClockDataDS();
            this.gbPeriodical = new System.Windows.Forms.GroupBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.btnPeriodicalReport = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbChooseMonth.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.clockDataDS1)).BeginInit();
            this.gbPeriodical.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbChooseMonth
            // 
            this.gbChooseMonth.AutoSize = true;
            this.gbChooseMonth.Controls.Add(this.btnSmanjiBankuSati);
            this.gbChooseMonth.Controls.Add(this.progressBar1);
            this.gbChooseMonth.Controls.Add(this.labelMessage);
            this.gbChooseMonth.Controls.Add(this.btnGenerateReport);
            this.gbChooseMonth.Controls.Add(this.dtpMesec);
            this.gbChooseMonth.Location = new System.Drawing.Point(13, 12);
            this.gbChooseMonth.Name = "gbChooseMonth";
            this.gbChooseMonth.Size = new System.Drawing.Size(292, 214);
            this.gbChooseMonth.TabIndex = 4;
            this.gbChooseMonth.TabStop = false;
            this.gbChooseMonth.Text = "Choose month:";
            // 
            // btnSmanjiBankuSati
            // 
            this.btnSmanjiBankuSati.Location = new System.Drawing.Point(162, 172);
            this.btnSmanjiBankuSati.Name = "btnSmanjiBankuSati";
            this.btnSmanjiBankuSati.Size = new System.Drawing.Size(124, 23);
            this.btnSmanjiBankuSati.TabIndex = 5;
            this.btnSmanjiBankuSati.Text = "Smanji banku sati";
            this.btnSmanjiBankuSati.UseVisualStyleBackColor = true;
            this.btnSmanjiBankuSati.Click += new System.EventHandler(this.btnSmanjiBankuSati_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 118);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(280, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Visible = false;
            // 
            // labelMessage
            // 
            this.labelMessage.Location = new System.Drawing.Point(24, 78);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(242, 63);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "labelMessage";
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(6, 172);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(97, 23);
            this.btnGenerateReport.TabIndex = 2;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // dtpMesec
            // 
            this.dtpMesec.CustomFormat = "MM/yyyy";
            this.dtpMesec.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMesec.Location = new System.Drawing.Point(27, 40);
            this.dtpMesec.Name = "dtpMesec";
            this.dtpMesec.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpMesec.Size = new System.Drawing.Size(97, 20);
            this.dtpMesec.TabIndex = 1;
            this.dtpMesec.ValueChanged += new System.EventHandler(this.dtpMesec_ValueChanged);
            // 
            // clockDataDS1
            // 
            //this.clockDataDS1.DataSetName = "ClockDataDS";
            //this.clockDataDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gbPeriodical
            // 
            this.gbPeriodical.Controls.Add(this.progressBar2);
            this.gbPeriodical.Controls.Add(this.btnPeriodicalReport);
            this.gbPeriodical.Controls.Add(this.dtpTo);
            this.gbPeriodical.Controls.Add(this.dtpFrom);
            this.gbPeriodical.Controls.Add(this.label2);
            this.gbPeriodical.Controls.Add(this.label1);
            this.gbPeriodical.Location = new System.Drawing.Point(311, 12);
            this.gbPeriodical.Name = "gbPeriodical";
            this.gbPeriodical.Size = new System.Drawing.Size(297, 214);
            this.gbPeriodical.TabIndex = 5;
            this.gbPeriodical.TabStop = false;
            this.gbPeriodical.Text = "Periodični izveštaj:";
            // 
            // progressBar2
            // 
            this.progressBar2.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar2.Location = new System.Drawing.Point(6, 118);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(285, 23);
            this.progressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar2.TabIndex = 6;
            this.progressBar2.Visible = false;
            // 
            // btnPeriodicalReport
            // 
            this.btnPeriodicalReport.Location = new System.Drawing.Point(160, 172);
            this.btnPeriodicalReport.Name = "btnPeriodicalReport";
            this.btnPeriodicalReport.Size = new System.Drawing.Size(97, 23);
            this.btnPeriodicalReport.TabIndex = 6;
            this.btnPeriodicalReport.Text = "Generiši izveštaj";
            this.btnPeriodicalReport.UseVisualStyleBackColor = true;
            this.btnPeriodicalReport.Click += new System.EventHandler(this.btnPeriodicalReport_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(36, 60);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(144, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(36, 21);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(144, 20);
            this.dtpFrom.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Do:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Od:";
            // 
            // ReportForSalaryForecast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 233);
            this.Controls.Add(this.gbPeriodical);
            this.Controls.Add(this.gbChooseMonth);
            this.Name = "ReportForSalaryForecast";
            this.Text = "ReportForSalaryForecast";
            this.gbChooseMonth.ResumeLayout(false);
            //((System.ComponentModel.ISupportInitialize)(this.clockDataDS1)).EndInit();
            this.gbPeriodical.ResumeLayout(false);
            this.gbPeriodical.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbChooseMonth;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.DateTimePicker dtpMesec;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnSmanjiBankuSati;
        private System.Windows.Forms.GroupBox gbPeriodical;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Button btnPeriodicalReport;
    }
}