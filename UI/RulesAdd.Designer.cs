namespace UI
{
    partial class RulesAdd
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
            this.lblCompany = new System.Windows.Forms.Label();
            this.tbCompany = new System.Windows.Forms.TextBox();
            this.tbEmplType = new System.Windows.Forms.TextBox();
            this.lblEmplType = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            this.lblDate1 = new System.Windows.Forms.Label();
            this.lblDate2 = new System.Windows.Forms.Label();
            this.dtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.dtpDate2 = new System.Windows.Forms.DateTimePicker();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.gbRule = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            this.gbRule.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(23, 22);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(86, 13);
            this.lblCompany.TabIndex = 0;
            this.lblCompany.Text = "Company:";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCompany
            // 
            this.tbCompany.Enabled = false;
            this.tbCompany.Location = new System.Drawing.Point(115, 19);
            this.tbCompany.Name = "tbCompany";
            this.tbCompany.Size = new System.Drawing.Size(249, 20);
            this.tbCompany.TabIndex = 1;
            // 
            // tbEmplType
            // 
            this.tbEmplType.Enabled = false;
            this.tbEmplType.Location = new System.Drawing.Point(115, 45);
            this.tbEmplType.Name = "tbEmplType";
            this.tbEmplType.Size = new System.Drawing.Size(249, 20);
            this.tbEmplType.TabIndex = 3;
            // 
            // lblEmplType
            // 
            this.lblEmplType.Location = new System.Drawing.Point(23, 48);
            this.lblEmplType.Name = "lblEmplType";
            this.lblEmplType.Size = new System.Drawing.Size(86, 13);
            this.lblEmplType.TabIndex = 2;
            this.lblEmplType.Text = "Employee type:";
            this.lblEmplType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbType
            // 
            this.tbType.Enabled = false;
            this.tbType.Location = new System.Drawing.Point(115, 71);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(345, 20);
            this.tbType.TabIndex = 5;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(23, 74);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(86, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(115, 97);
            this.tbDesc.Multiline = true;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(345, 112);
            this.tbDesc.TabIndex = 7;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(23, 100);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(86, 13);
            this.lblDesc.TabIndex = 6;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblValue
            // 
            this.lblValue.Location = new System.Drawing.Point(23, 220);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(86, 13);
            this.lblValue.TabIndex = 8;
            this.lblValue.Text = "Value:";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numValue
            // 
            this.numValue.Location = new System.Drawing.Point(115, 218);
            this.numValue.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numValue.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numValue.Name = "numValue";
            this.numValue.Size = new System.Drawing.Size(86, 20);
            this.numValue.TabIndex = 9;
            // 
            // lblDate1
            // 
            this.lblDate1.Location = new System.Drawing.Point(23, 246);
            this.lblDate1.Name = "lblDate1";
            this.lblDate1.Size = new System.Drawing.Size(86, 13);
            this.lblDate1.TabIndex = 10;
            this.lblDate1.Text = "Date 1:";
            this.lblDate1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDate2
            // 
            this.lblDate2.Location = new System.Drawing.Point(23, 272);
            this.lblDate2.Name = "lblDate2";
            this.lblDate2.Size = new System.Drawing.Size(86, 13);
            this.lblDate2.TabIndex = 12;
            this.lblDate2.Text = "Date 2:";
            this.lblDate2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDate1
            // 
            this.dtpDate1.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate1.Location = new System.Drawing.Point(115, 244);
            this.dtpDate1.Name = "dtpDate1";
            this.dtpDate1.ShowUpDown = true;
            this.dtpDate1.Size = new System.Drawing.Size(149, 20);
            this.dtpDate1.TabIndex = 11;
            // 
            // dtpDate2
            // 
            this.dtpDate2.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpDate2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate2.Location = new System.Drawing.Point(115, 270);
            this.dtpDate2.Name = "dtpDate2";
            this.dtpDate2.ShowUpDown = true;
            this.dtpDate2.Size = new System.Drawing.Size(149, 20);
            this.dtpDate2.TabIndex = 13;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 339);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(415, 339);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(112, 296);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(155, 13);
            this.lblInfo.TabIndex = 14;
            this.lblInfo.Text = "* 01.01.1900. is undefined date";
            // 
            // gbRule
            // 
            this.gbRule.Controls.Add(this.tbCompany);
            this.gbRule.Controls.Add(this.lblInfo);
            this.gbRule.Controls.Add(this.lblCompany);
            this.gbRule.Controls.Add(this.lblEmplType);
            this.gbRule.Controls.Add(this.tbEmplType);
            this.gbRule.Controls.Add(this.dtpDate2);
            this.gbRule.Controls.Add(this.lblType);
            this.gbRule.Controls.Add(this.dtpDate1);
            this.gbRule.Controls.Add(this.tbType);
            this.gbRule.Controls.Add(this.lblDate2);
            this.gbRule.Controls.Add(this.lblDesc);
            this.gbRule.Controls.Add(this.lblDate1);
            this.gbRule.Controls.Add(this.tbDesc);
            this.gbRule.Controls.Add(this.numValue);
            this.gbRule.Controls.Add(this.lblValue);
            this.gbRule.Location = new System.Drawing.Point(12, 12);
            this.gbRule.Name = "gbRule";
            this.gbRule.Size = new System.Drawing.Size(478, 321);
            this.gbRule.TabIndex = 0;
            this.gbRule.TabStop = false;
            this.gbRule.Text = "Rule";
            // 
            // RulesAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 387);
            this.ControlBox = false;
            this.Controls.Add(this.gbRule);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RulesAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rules Add";
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            this.gbRule.ResumeLayout(false);
            this.gbRule.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.TextBox tbCompany;
        private System.Windows.Forms.TextBox tbEmplType;
        private System.Windows.Forms.Label lblEmplType;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.NumericUpDown numValue;
        private System.Windows.Forms.Label lblDate1;
        private System.Windows.Forms.Label lblDate2;
        private System.Windows.Forms.DateTimePicker dtpDate1;
        private System.Windows.Forms.DateTimePicker dtpDate2;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox gbRule;
    }
}