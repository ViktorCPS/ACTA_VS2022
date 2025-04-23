namespace UI
{
    partial class MCVaccinesAdd
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
            this.btnClose = new System.Windows.Forms.Button();
            this.gbInserRisk = new System.Windows.Forms.GroupBox();
            this.txtDescSr = new System.Windows.Forms.TextBox();
            this.lblDescSr = new System.Windows.Forms.Label();
            this.txtDescEn = new System.Windows.Forms.TextBox();
            this.lblDescEn = new System.Windows.Forms.Label();
            this.txtType = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.gbInserRisk.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(258, 168);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbInserRisk
            // 
            this.gbInserRisk.Controls.Add(this.txtDescSr);
            this.gbInserRisk.Controls.Add(this.lblDescSr);
            this.gbInserRisk.Controls.Add(this.txtDescEn);
            this.gbInserRisk.Controls.Add(this.lblDescEn);
            this.gbInserRisk.Controls.Add(this.txtType);
            this.gbInserRisk.Controls.Add(this.lblType);
            this.gbInserRisk.Location = new System.Drawing.Point(12, 22);
            this.gbInserRisk.Name = "gbInserRisk";
            this.gbInserRisk.Size = new System.Drawing.Size(321, 122);
            this.gbInserRisk.TabIndex = 11;
            this.gbInserRisk.TabStop = false;
            this.gbInserRisk.Text = "Insert";
            // 
            // txtDescSr
            // 
            this.txtDescSr.Location = new System.Drawing.Point(171, 74);
            this.txtDescSr.Name = "txtDescSr";
            this.txtDescSr.Size = new System.Drawing.Size(121, 20);
            this.txtDescSr.TabIndex = 9;
            // 
            // lblDescSr
            // 
            this.lblDescSr.AutoSize = true;
            this.lblDescSr.Location = new System.Drawing.Point(15, 77);
            this.lblDescSr.Name = "lblDescSr";
            this.lblDescSr.Size = new System.Drawing.Size(71, 13);
            this.lblDescSr.TabIndex = 8;
            this.lblDescSr.Text = "Description sr";
            // 
            // txtDescEn
            // 
            this.txtDescEn.Location = new System.Drawing.Point(171, 48);
            this.txtDescEn.Name = "txtDescEn";
            this.txtDescEn.Size = new System.Drawing.Size(121, 20);
            this.txtDescEn.TabIndex = 7;
            // 
            // lblDescEn
            // 
            this.lblDescEn.AutoSize = true;
            this.lblDescEn.Location = new System.Drawing.Point(15, 51);
            this.lblDescEn.Name = "lblDescEn";
            this.lblDescEn.Size = new System.Drawing.Size(75, 13);
            this.lblDescEn.TabIndex = 6;
            this.lblDescEn.Text = "Description en";
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(171, 25);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(121, 20);
            this.txtType.TabIndex = 5;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(15, 25);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Type";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 168);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(12, 168);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 14;
            this.btnInsert.Text = "Add";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Visible = false;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // MCVaccinesAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 215);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbInserRisk);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnInsert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MCVaccinesAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MCVaccinesAdd";
            this.Load += new System.EventHandler(this.MCVaccinesAdd_Load);
            this.gbInserRisk.ResumeLayout(false);
            this.gbInserRisk.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbInserRisk;
        private System.Windows.Forms.TextBox txtDescSr;
        private System.Windows.Forms.Label lblDescSr;
        private System.Windows.Forms.TextBox txtDescEn;
        private System.Windows.Forms.Label lblDescEn;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnInsert;
    }
}