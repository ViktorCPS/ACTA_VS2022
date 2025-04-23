namespace UI
{
    partial class StatisticReportViewControl
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
            this.pccStatisticsView = new System.Drawing.PieChart.PieChartControl();
            this.lvStatistics = new System.Windows.Forms.ListView();
            this.lblPlaned = new System.Windows.Forms.Label();
            this.tbPlanned = new System.Windows.Forms.TextBox();
            this.lblRealized = new System.Windows.Forms.Label();
            this.tbRealized = new System.Windows.Forms.TextBox();
            this.lblGraphName = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblRotation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // pccStatisticsView
            // 
            this.pccStatisticsView.Location = new System.Drawing.Point(19, 32);
            this.pccStatisticsView.Name = "pccStatisticsView";
            this.pccStatisticsView.Size = new System.Drawing.Size(541, 248);
            this.pccStatisticsView.TabIndex = 0;
            this.pccStatisticsView.ToolTips = null;
            // 
            // lvStatistics
            // 
            this.lvStatistics.GridLines = true;
            this.lvStatistics.Location = new System.Drawing.Point(19, 338);
            this.lvStatistics.Name = "lvStatistics";
            this.lvStatistics.Size = new System.Drawing.Size(541, 178);
            this.lvStatistics.TabIndex = 1;
            this.lvStatistics.UseCompatibleStateImageBehavior = false;
            this.lvStatistics.View = System.Windows.Forms.View.Details;
            // 
            // lblPlaned
            // 
            this.lblPlaned.AutoSize = true;
            this.lblPlaned.Location = new System.Drawing.Point(55, 289);
            this.lblPlaned.Name = "lblPlaned";
            this.lblPlaned.Size = new System.Drawing.Size(49, 13);
            this.lblPlaned.TabIndex = 2;
            this.lblPlaned.Text = "Planned:";
            // 
            // tbPlanned
            // 
            this.tbPlanned.Location = new System.Drawing.Point(110, 286);
            this.tbPlanned.Name = "tbPlanned";
            this.tbPlanned.Size = new System.Drawing.Size(116, 20);
            this.tbPlanned.TabIndex = 4;
            this.tbPlanned.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblRealized
            // 
            this.lblRealized.AutoSize = true;
            this.lblRealized.Location = new System.Drawing.Point(59, 315);
            this.lblRealized.Name = "lblRealized";
            this.lblRealized.Size = new System.Drawing.Size(45, 13);
            this.lblRealized.TabIndex = 5;
            this.lblRealized.Text = "Realize:";
            // 
            // tbRealized
            // 
            this.tbRealized.Location = new System.Drawing.Point(110, 312);
            this.tbRealized.Name = "tbRealized";
            this.tbRealized.Size = new System.Drawing.Size(116, 20);
            this.tbRealized.TabIndex = 6;
            this.tbRealized.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblGraphName
            // 
            this.lblGraphName.AutoSize = true;
            this.lblGraphName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblGraphName.Location = new System.Drawing.Point(16, 16);
            this.lblGraphName.Name = "lblGraphName";
            this.lblGraphName.Size = new System.Drawing.Size(35, 13);
            this.lblGraphName.TabIndex = 7;
            this.lblGraphName.Text = "Name";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(517, 287);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // lblRotation
            // 
            this.lblRotation.AutoSize = true;
            this.lblRotation.Location = new System.Drawing.Point(461, 289);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(50, 13);
            this.lblRotation.TabIndex = 9;
            this.lblRotation.Text = "Rotation:";
            // 
            // StatisticReportViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblRotation);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.lblGraphName);
            this.Controls.Add(this.tbRealized);
            this.Controls.Add(this.lblRealized);
            this.Controls.Add(this.tbPlanned);
            this.Controls.Add(this.lblPlaned);
            this.Controls.Add(this.lvStatistics);
            this.Controls.Add(this.pccStatisticsView);
            this.Name = "StatisticReportViewControl";
            this.Size = new System.Drawing.Size(583, 534);
            this.Load += new System.EventHandler(this.StatisticReportViewControl_Load);
            this.Resize += new System.EventHandler(this.Resize_event);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Drawing.PieChart.PieChartControl pccStatisticsView;
        protected System.Windows.Forms.ListView lvStatistics;
        protected System.Windows.Forms.Label lblPlaned;
        protected System.Windows.Forms.TextBox tbPlanned;
        protected System.Windows.Forms.Label lblRealized;
        protected System.Windows.Forms.TextBox tbRealized;
        protected System.Windows.Forms.Label lblGraphName;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lblRotation;
    }
}
