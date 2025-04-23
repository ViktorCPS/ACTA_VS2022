namespace UI
{
    partial class ExitPermRowControl
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
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.lblto = new System.Windows.Forms.Label();
            this.tbTo = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.chbInsertPerm = new System.Windows.Forms.CheckBox();
            this.dtpExitPermStartTime = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // cbPassType
            // 
            this.cbPassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPassType.FormattingEnabled = true;
            this.cbPassType.Location = new System.Drawing.Point(3, 3);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(221, 21);
            this.cbPassType.TabIndex = 0;
            this.cbPassType.SelectedIndexChanged += new System.EventHandler(this.cbPassType_SelectedIndexChanged);
            // 
            // tbFrom
            // 
            this.tbFrom.Location = new System.Drawing.Point(275, 3);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(57, 20);
            this.tbFrom.TabIndex = 1;
            this.tbFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblto
            // 
            this.lblto.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblto.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblto.Location = new System.Drawing.Point(334, -8);
            this.lblto.Name = "lblto";
            this.lblto.Size = new System.Drawing.Size(19, 36);
            this.lblto.TabIndex = 9;
            this.lblto.Text = "-";
            this.lblto.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbTo
            // 
            this.tbTo.Location = new System.Drawing.Point(359, 3);
            this.tbTo.Name = "tbTo";
            this.tbTo.Size = new System.Drawing.Size(57, 20);
            this.tbTo.TabIndex = 10;
            this.tbTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(478, 3);
            this.tbDescription.MaxLength = 500;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(230, 20);
            this.tbDescription.TabIndex = 11;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // chbInsertPerm
            // 
            this.chbInsertPerm.AutoSize = true;
            this.chbInsertPerm.Location = new System.Drawing.Point(714, 6);
            this.chbInsertPerm.Name = "chbInsertPerm";
            this.chbInsertPerm.Size = new System.Drawing.Size(15, 14);
            this.chbInsertPerm.TabIndex = 12;
            this.chbInsertPerm.UseVisualStyleBackColor = true;
            this.chbInsertPerm.CheckedChanged += new System.EventHandler(this.chbInsertPerm_CheckedChanged);
            // 
            // dtpExitPermStartTime
            // 
            this.dtpExitPermStartTime.CustomFormat = "HH:mm";
            this.dtpExitPermStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExitPermStartTime.Location = new System.Drawing.Point(275, 3);
            this.dtpExitPermStartTime.Name = "dtpExitPermStartTime";
            this.dtpExitPermStartTime.ShowUpDown = true;
            this.dtpExitPermStartTime.Size = new System.Drawing.Size(57, 20);
            this.dtpExitPermStartTime.TabIndex = 13;
            this.dtpExitPermStartTime.ValueChanged += new System.EventHandler(this.dtpExitPermStartTime_ValueChanged);
            // 
            // ExitPermRowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtpExitPermStartTime);
            this.Controls.Add(this.chbInsertPerm);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbTo);
            this.Controls.Add(this.lblto);
            this.Controls.Add(this.tbFrom);
            this.Controls.Add(this.cbPassType);
            this.Name = "ExitPermRowControl";
            this.Size = new System.Drawing.Size(729, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPassType;
        private System.Windows.Forms.TextBox tbFrom;
        private System.Windows.Forms.Label lblto;
        private System.Windows.Forms.TextBox tbTo;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.CheckBox chbInsertPerm;
        private System.Windows.Forms.DateTimePicker dtpExitPermStartTime;
    }
}
