namespace UI
{
    partial class StatisticReportViewMultiWUControl
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
            this.SuspendLayout();
            // 
            // pccStatisticsView
            // 
            this.pccStatisticsView.Location = new System.Drawing.Point(5, 5);
            this.pccStatisticsView.Size = new System.Drawing.Size(573, 267);
            // 
            // lvStatistics
            // 
            this.lvStatistics.Location = new System.Drawing.Point(5, 338);
            this.lvStatistics.Size = new System.Drawing.Size(573, 186);
            // 
            // lblPlaned
            // 
            this.lblPlaned.Location = new System.Drawing.Point(51, 282);
            this.lblPlaned.Size = new System.Drawing.Size(43, 13);
            this.lblPlaned.Text = "Planed:";
            // 
            // lblRealized
            // 
            this.lblRealized.Location = new System.Drawing.Point(55, 305);
            this.lblRealized.Size = new System.Drawing.Size(51, 13);
            this.lblRealized.Text = "Realized:";
            // 
            // tbRealized
            // 
            this.tbRealized.Location = new System.Drawing.Point(110, 305);
            // 
            // StatisticReportViewMultiWUControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "StatisticReportViewMultiWUControl";
            this.Load += new System.EventHandler(this.StatisticReportViewMultiWUControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
