namespace UI
{
    partial class MedicalCheck
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
            this.tcMC = new System.Windows.Forms.TabControl();
            this.tpRisk = new System.Windows.Forms.TabPage();
            this.mcRisks1 = new UI.MCRisks();
            this.tpVaccines = new System.Windows.Forms.TabPage();
            this.mcVaccines1 = new UI.MCVaccines();
            this.tpDisabilities = new System.Windows.Forms.TabPage();
            this.mcDisabilities1 = new UI.MCDisabilities();
            this.tpRiskPostion = new System.Windows.Forms.TabPage();
            this.mcRiskPosition = new UI.MCRisksPositions();
            this.btnClose = new System.Windows.Forms.Button();
            this.tcMC.SuspendLayout();
            this.tpRisk.SuspendLayout();
            this.tpVaccines.SuspendLayout();
            this.tpDisabilities.SuspendLayout();
            this.tpRiskPostion.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMC
            // 
            this.tcMC.Controls.Add(this.tpRisk);
            this.tcMC.Controls.Add(this.tpVaccines);
            this.tcMC.Controls.Add(this.tpDisabilities);
            this.tcMC.Controls.Add(this.tpRiskPostion);
            this.tcMC.Location = new System.Drawing.Point(12, 12);
            this.tcMC.Name = "tcMC";
            this.tcMC.SelectedIndex = 0;
            this.tcMC.Size = new System.Drawing.Size(632, 499);
            this.tcMC.TabIndex = 0;
            // 
            // tpRisk
            // 
            this.tpRisk.Controls.Add(this.mcRisks1);
            this.tpRisk.Location = new System.Drawing.Point(4, 22);
            this.tpRisk.Name = "tpRisk";
            this.tpRisk.Padding = new System.Windows.Forms.Padding(3);
            this.tpRisk.Size = new System.Drawing.Size(624, 473);
            this.tpRisk.TabIndex = 0;
            this.tpRisk.Text = "Risks";
            this.tpRisk.UseVisualStyleBackColor = true;
            // 
            // mcRisks1
            // 
            this.mcRisks1.Location = new System.Drawing.Point(6, 6);
            this.mcRisks1.Name = "mcRisks1";
            this.mcRisks1.Size = new System.Drawing.Size(612, 480);
            this.mcRisks1.TabIndex = 0;
            // 
            // tpVaccines
            // 
            this.tpVaccines.Controls.Add(this.mcVaccines1);
            this.tpVaccines.Location = new System.Drawing.Point(4, 22);
            this.tpVaccines.Name = "tpVaccines";
            this.tpVaccines.Padding = new System.Windows.Forms.Padding(3);
            this.tpVaccines.Size = new System.Drawing.Size(624, 473);
            this.tpVaccines.TabIndex = 1;
            this.tpVaccines.Text = "Vaccines";
            this.tpVaccines.UseVisualStyleBackColor = true;
            // 
            // mcVaccines1
            // 
            this.mcVaccines1.Location = new System.Drawing.Point(12, 7);
            this.mcVaccines1.Name = "mcVaccines1";
            this.mcVaccines1.Size = new System.Drawing.Size(414, 439);
            this.mcVaccines1.TabIndex = 0;
            // 
            // tpDisabilities
            // 
            this.tpDisabilities.Controls.Add(this.mcDisabilities1);
            this.tpDisabilities.Location = new System.Drawing.Point(4, 22);
            this.tpDisabilities.Name = "tpDisabilities";
            this.tpDisabilities.Padding = new System.Windows.Forms.Padding(3);
            this.tpDisabilities.Size = new System.Drawing.Size(624, 473);
            this.tpDisabilities.TabIndex = 2;
            this.tpDisabilities.Text = "Disabilities";
            this.tpDisabilities.UseVisualStyleBackColor = true;
            // 
            // mcDisabilities1
            // 
            this.mcDisabilities1.Location = new System.Drawing.Point(12, 7);
            this.mcDisabilities1.Name = "mcDisabilities1";
            this.mcDisabilities1.Size = new System.Drawing.Size(612, 480);
            this.mcDisabilities1.TabIndex = 0;
            // 
            // tpRiskPostion
            // 
            this.tpRiskPostion.Controls.Add(this.mcRiskPosition);
            this.tpRiskPostion.Location = new System.Drawing.Point(4, 22);
            this.tpRiskPostion.Name = "tpRiskPostion";
            this.tpRiskPostion.Padding = new System.Windows.Forms.Padding(3);
            this.tpRiskPostion.Size = new System.Drawing.Size(624, 473);
            this.tpRiskPostion.TabIndex = 3;
            this.tpRiskPostion.Text = "Risks <> Positions";
            this.tpRiskPostion.UseVisualStyleBackColor = true;
            // 
            // mcRiskPosition
            // 
            this.mcRiskPosition.Location = new System.Drawing.Point(6, 6);
            this.mcRiskPosition.Name = "mcRiskPosition";
            this.mcRiskPosition.Size = new System.Drawing.Size(612, 480);
            this.mcRiskPosition.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(565, 521);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // MedicalCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 563);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tcMC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MedicalCheck";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MedicalCheck";
            this.tcMC.ResumeLayout(false);
            this.tpRisk.ResumeLayout(false);
            this.tpVaccines.ResumeLayout(false);
            this.tpDisabilities.ResumeLayout(false);
            this.tpRiskPostion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMC;
        private System.Windows.Forms.TabPage tpRisk;
        private System.Windows.Forms.TabPage tpVaccines;
        private MCRisks mcRisks1;
        private MCVaccines mcVaccines1;
        private MCDisabilities mcDisabilities1;
        private MCRisksPositions mcRiskPosition;
        private System.Windows.Forms.TabPage tpDisabilities;
        private System.Windows.Forms.TabPage tpRiskPostion;
        private System.Windows.Forms.Button btnClose;
    }
}