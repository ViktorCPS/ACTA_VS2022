namespace UI
{
    partial class MCRisksAdd
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
            this.gbInserRisk = new System.Windows.Forms.GroupBox();
            this.txtDescSr = new System.Windows.Forms.TextBox();
            this.lblDescSr = new System.Windows.Forms.Label();
            this.txtDescEn = new System.Windows.Forms.TextBox();
            this.lblDescEn = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblDefaultRot = new System.Windows.Forms.Label();
            this.cbDefRot = new System.Windows.Forms.ComboBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gbInserRisk.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInserRisk
            // 
            this.gbInserRisk.Controls.Add(this.txtDescSr);
            this.gbInserRisk.Controls.Add(this.lblDescSr);
            this.gbInserRisk.Controls.Add(this.txtDescEn);
            this.gbInserRisk.Controls.Add(this.lblDescEn);
            this.gbInserRisk.Controls.Add(this.txtCode);
            this.gbInserRisk.Controls.Add(this.lblCode);
            this.gbInserRisk.Controls.Add(this.lblDefaultRot);
            this.gbInserRisk.Controls.Add(this.cbDefRot);
            this.gbInserRisk.Controls.Add(this.lblCompany);
            this.gbInserRisk.Controls.Add(this.cbCompany);
            this.gbInserRisk.Location = new System.Drawing.Point(12, 12);
            this.gbInserRisk.Name = "gbInserRisk";
            this.gbInserRisk.Size = new System.Drawing.Size(321, 170);
            this.gbInserRisk.TabIndex = 3;
            this.gbInserRisk.TabStop = false;
            this.gbInserRisk.Text = "Insert";
            // 
            // txtDescSr
            // 
            this.txtDescSr.Location = new System.Drawing.Point(175, 133);
            this.txtDescSr.Name = "txtDescSr";
            this.txtDescSr.Size = new System.Drawing.Size(121, 20);
            this.txtDescSr.TabIndex = 9;
            // 
            // lblDescSr
            // 
            this.lblDescSr.AutoSize = true;
            this.lblDescSr.Location = new System.Drawing.Point(19, 136);
            this.lblDescSr.Name = "lblDescSr";
            this.lblDescSr.Size = new System.Drawing.Size(71, 13);
            this.lblDescSr.TabIndex = 8;
            this.lblDescSr.Text = "Description sr";
            // 
            // txtDescEn
            // 
            this.txtDescEn.Location = new System.Drawing.Point(175, 107);
            this.txtDescEn.Name = "txtDescEn";
            this.txtDescEn.Size = new System.Drawing.Size(121, 20);
            this.txtDescEn.TabIndex = 7;
            // 
            // lblDescEn
            // 
            this.lblDescEn.AutoSize = true;
            this.lblDescEn.Location = new System.Drawing.Point(19, 110);
            this.lblDescEn.Name = "lblDescEn";
            this.lblDescEn.Size = new System.Drawing.Size(75, 13);
            this.lblDescEn.TabIndex = 6;
            this.lblDescEn.Text = "Description en";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(175, 84);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(121, 20);
            this.txtCode.TabIndex = 5;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(19, 84);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(55, 13);
            this.lblCode.TabIndex = 4;
            this.lblCode.Text = "Risk code";
            // 
            // lblDefaultRot
            // 
            this.lblDefaultRot.AutoSize = true;
            this.lblDefaultRot.Location = new System.Drawing.Point(19, 57);
            this.lblDefaultRot.Name = "lblDefaultRot";
            this.lblDefaultRot.Size = new System.Drawing.Size(79, 13);
            this.lblDefaultRot.TabIndex = 2;
            this.lblDefaultRot.Text = "Default rotation";
            // 
            // cbDefRot
            // 
            this.cbDefRot.FormattingEnabled = true;
            this.cbDefRot.Location = new System.Drawing.Point(175, 54);
            this.cbDefRot.Name = "cbDefRot";
            this.cbDefRot.Size = new System.Drawing.Size(121, 21);
            this.cbDefRot.TabIndex = 3;
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.Location = new System.Drawing.Point(19, 30);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(51, 13);
            this.lblCompany.TabIndex = 0;
            this.lblCompany.Text = "Company";
            // 
            // cbCompany
            // 
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(175, 27);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(121, 21);
            this.cbCompany.TabIndex = 1;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(16, 203);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 8;
            this.btnInsert.Text = "Add";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Visible = false;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(258, 201);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 201);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // MCRisksAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 238);
            this.ControlBox = false;
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbInserRisk);
            this.Controls.Add(this.btnUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MCRisksAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RisksAdd";
            this.gbInserRisk.ResumeLayout(false);
            this.gbInserRisk.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInserRisk;
        private System.Windows.Forms.TextBox txtDescEn;
        private System.Windows.Forms.Label lblDescEn;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblDefaultRot;
        private System.Windows.Forms.ComboBox cbDefRot;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtDescSr;
        private System.Windows.Forms.Label lblDescSr;
        private System.Windows.Forms.Button btnUpdate;
    }
}