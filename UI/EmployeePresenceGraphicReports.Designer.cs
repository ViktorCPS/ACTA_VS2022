namespace UI
{
    partial class EmployeePresenceGraphicReports
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpDay = new System.Windows.Forms.TabPage();
            this.presenceGraphForDayControl1 = new UI.PresenceGraphForDayControl();
            this.tpEmployee = new System.Windows.Forms.TabPage();
            this.presenceGraphForEmplControl1 = new UI.PresenceGraphForEmplControl();
            this.tabControl1.SuspendLayout();
            this.tpDay.SuspendLayout();
            this.tpEmployee.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpDay);
            this.tabControl1.Controls.Add(this.tpEmployee);
            this.tabControl1.Location = new System.Drawing.Point(12, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(995, 703);
            this.tabControl1.TabIndex = 0;
            // 
            // tpDay
            // 
            this.tpDay.Controls.Add(this.presenceGraphForDayControl1);
            this.tpDay.Location = new System.Drawing.Point(4, 22);
            this.tpDay.Name = "tpDay";
            this.tpDay.Padding = new System.Windows.Forms.Padding(3);
            this.tpDay.Size = new System.Drawing.Size(987, 677);
            this.tpDay.TabIndex = 0;
            this.tpDay.Text = "Day";
            this.tpDay.UseVisualStyleBackColor = true;
            // 
            // presenceGraphForDayControl1
            // 
            this.presenceGraphForDayControl1.Location = new System.Drawing.Point(-4, 0);
            this.presenceGraphForDayControl1.Name = "presenceGraphForDayControl1";
            this.presenceGraphForDayControl1.Size = new System.Drawing.Size(977, 682);
            this.presenceGraphForDayControl1.TabIndex = 0;
            // 
            // tpEmployee
            // 
            this.tpEmployee.Controls.Add(this.presenceGraphForEmplControl1);
            this.tpEmployee.Location = new System.Drawing.Point(4, 22);
            this.tpEmployee.Name = "tpEmployee";
            this.tpEmployee.Padding = new System.Windows.Forms.Padding(3);
            this.tpEmployee.Size = new System.Drawing.Size(987, 677);
            this.tpEmployee.TabIndex = 1;
            this.tpEmployee.Text = "Employee";
            this.tpEmployee.UseVisualStyleBackColor = true;
            // 
            // presenceGraphForEmplControl1
            // 
            this.presenceGraphForEmplControl1.Location = new System.Drawing.Point(-4, 0);
            this.presenceGraphForEmplControl1.Name = "presenceGraphForEmplControl1";
            this.presenceGraphForEmplControl1.Size = new System.Drawing.Size(977, 682);
            this.presenceGraphForEmplControl1.TabIndex = 0;
            // 
            // EmployeePresenceGraphicReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 728);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.MaximumSize = new System.Drawing.Size(1020, 762);
            this.MinimumSize = new System.Drawing.Size(1020, 762);
            this.Name = "EmployeePresenceGraphicReports";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "EmployeePresenceGraphicReports";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeePresenceGraphicReports_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tpDay.ResumeLayout(false);
            this.tpEmployee.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpDay;
        private System.Windows.Forms.TabPage tpEmployee;
        private PresenceGraphForDayControl presenceGraphForDayControl1;
        private PresenceGraphForEmplControl presenceGraphForEmplControl1;
    }
}