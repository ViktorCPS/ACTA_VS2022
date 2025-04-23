namespace Reports.Magna
{
    partial class MagnaExcel
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.gbDateInterval = new System.Windows.Forms.GroupBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.rbOU = new System.Windows.Forms.RadioButton();
            this.rbWU = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.gbDateInterval.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Controls.Add(this.lvEmployees);
            this.groupBox1.Controls.Add(this.gbDateInterval);
            this.groupBox1.Controls.Add(this.tbEmployee);
            this.groupBox1.Controls.Add(this.lblEmployee);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 571);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(28, 507);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(97, 23);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(28, 267);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(364, 208);
            this.lvEmployees.TabIndex = 6;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            // 
            // gbDateInterval
            // 
            this.gbDateInterval.Controls.Add(this.dtpFrom);
            this.gbDateInterval.Controls.Add(this.lblFrom);
            this.gbDateInterval.Location = new System.Drawing.Point(50, 118);
            this.gbDateInterval.Name = "gbDateInterval";
            this.gbDateInterval.Size = new System.Drawing.Size(320, 83);
            this.gbDateInterval.TabIndex = 9;
            this.gbDateInterval.TabStop = false;
            this.gbDateInterval.Text = "Date interval";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(163, 50);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(100, 20);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(33, 50);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(124, 13);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Select desired from date:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEmployee
            // 
            this.tbEmployee.Location = new System.Drawing.Point(127, 224);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(243, 20);
            this.tbEmployee.TabIndex = 5;
            this.tbEmployee.TextChanged += new System.EventHandler(this.tbEmployee_TextChanged);
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Location = new System.Drawing.Point(59, 227);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(56, 13);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbOU);
            this.groupBox2.Controls.Add(this.cbWU);
            this.groupBox2.Controls.Add(this.rbOU);
            this.groupBox2.Controls.Add(this.rbWU);
            this.groupBox2.Location = new System.Drawing.Point(17, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter";
            // 
            // cbOU
            // 
            this.cbOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(95, 49);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(243, 21);
            this.cbOU.TabIndex = 3;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(95, 22);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(243, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // rbOU
            // 
            this.rbOU.AutoSize = true;
            this.rbOU.Location = new System.Drawing.Point(45, 50);
            this.rbOU.Name = "rbOU";
            this.rbOU.Size = new System.Drawing.Size(41, 17);
            this.rbOU.TabIndex = 2;
            this.rbOU.TabStop = true;
            this.rbOU.Text = "OU";
            this.rbOU.UseVisualStyleBackColor = true;
            this.rbOU.CheckedChanged += new System.EventHandler(this.rbOU_CheckedChanged);
            // 
            // rbWU
            // 
            this.rbWU.AutoSize = true;
            this.rbWU.Location = new System.Drawing.Point(45, 23);
            this.rbWU.Name = "rbWU";
            this.rbWU.Size = new System.Drawing.Size(38, 17);
            this.rbWU.TabIndex = 0;
            this.rbWU.TabStop = true;
            this.rbWU.Text = "FS";
            this.rbWU.UseVisualStyleBackColor = true;
            this.rbWU.CheckedChanged += new System.EventHandler(this.rbWU_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(317, 507);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // MagnaExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 598);
            this.Controls.Add(this.groupBox1);
            this.Name = "MagnaExcel";
            this.Text = "MagnaExcel";
            this.Load += new System.EventHandler(this.MagnaExcel_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbDateInterval.ResumeLayout(false);
            this.gbDateInterval.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.GroupBox gbDateInterval;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.RadioButton rbOU;
        private System.Windows.Forms.RadioButton rbWU;
        private System.Windows.Forms.Button btnClose;
    }
}