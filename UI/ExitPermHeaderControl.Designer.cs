namespace UI
{
    partial class ExitPermHeaderControl
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
            this.lblDay = new System.Windows.Forms.Label();
            this.lblSchedule = new System.Windows.Forms.Label();
            this.tbDay = new System.Windows.Forms.TextBox();
            this.cbSchedule = new System.Windows.Forms.ComboBox();
            this.lblPassType = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblManualPairs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDay
            // 
            this.lblDay.Location = new System.Drawing.Point(3, 0);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(66, 23);
            this.lblDay.TabIndex = 3;
            this.lblDay.Text = "Day:";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSchedule
            // 
            this.lblSchedule.Location = new System.Drawing.Point(220, 3);
            this.lblSchedule.Name = "lblSchedule";
            this.lblSchedule.Size = new System.Drawing.Size(88, 23);
            this.lblSchedule.TabIndex = 4;
            this.lblSchedule.Text = "Schedule:";
            this.lblSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDay
            // 
            this.tbDay.Enabled = false;
            this.tbDay.Location = new System.Drawing.Point(75, 3);
            this.tbDay.Name = "tbDay";
            this.tbDay.Size = new System.Drawing.Size(100, 20);
            this.tbDay.TabIndex = 5;
            // 
            // cbSchedule
            // 
            this.cbSchedule.BackColor = System.Drawing.SystemColors.Control;
            this.cbSchedule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSchedule.FormattingEnabled = true;
            this.cbSchedule.Location = new System.Drawing.Point(314, 3);
            this.cbSchedule.Name = "cbSchedule";
            this.cbSchedule.Size = new System.Drawing.Size(85, 21);
            this.cbSchedule.TabIndex = 6;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(174, 26);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(88, 23);
            this.lblPassType.TabIndex = 7;
            this.lblPassType.Text = "Pass type";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(455, 26);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(43, 23);
            this.lblFrom.TabIndex = 8;
            this.lblFrom.Text = "From";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(529, 26);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(43, 23);
            this.lblTo.TabIndex = 9;
            this.lblTo.Text = "To";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(653, 26);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(178, 23);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblManualPairs
            // 
            this.lblManualPairs.ForeColor = System.Drawing.Color.Red;
            this.lblManualPairs.Location = new System.Drawing.Point(405, 1);
            this.lblManualPairs.Name = "lblManualPairs";
            this.lblManualPairs.Size = new System.Drawing.Size(470, 23);
            this.lblManualPairs.TabIndex = 11;
            this.lblManualPairs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExitPermHeaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblManualPairs);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.lblPassType);
            this.Controls.Add(this.cbSchedule);
            this.Controls.Add(this.tbDay);
            this.Controls.Add(this.lblSchedule);
            this.Controls.Add(this.lblDay);
            this.Name = "ExitPermHeaderControl";
            this.Size = new System.Drawing.Size(878, 51);
            this.Load += new System.EventHandler(this.ExitPermHeaderControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblSchedule;
        private System.Windows.Forms.TextBox tbDay;
        private System.Windows.Forms.ComboBox cbSchedule;
        private System.Windows.Forms.Label lblPassType;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblManualPairs;
    }
}
