namespace UI
{
    partial class MCRisksPositions
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
            this.lvPostions = new System.Windows.Forms.ListView();
            this.lvRisks = new System.Windows.Forms.ListView();
            this.lvRisksPositions = new System.Windows.Forms.ListView();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblCompany = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvPostions
            // 
            this.lvPostions.FullRowSelect = true;
            this.lvPostions.GridLines = true;
            this.lvPostions.HideSelection = false;
            this.lvPostions.Location = new System.Drawing.Point(25, 77);
            this.lvPostions.Name = "lvPostions";
            this.lvPostions.Size = new System.Drawing.Size(211, 155);
            this.lvPostions.TabIndex = 0;
            this.lvPostions.UseCompatibleStateImageBehavior = false;
            this.lvPostions.View = System.Windows.Forms.View.Details;
            // 
            // lvRisks
            // 
            this.lvRisks.FullRowSelect = true;
            this.lvRisks.GridLines = true;
            this.lvRisks.HideSelection = false;
            this.lvRisks.Location = new System.Drawing.Point(25, 252);
            this.lvRisks.Name = "lvRisks";
            this.lvRisks.Size = new System.Drawing.Size(211, 155);
            this.lvRisks.TabIndex = 1;
            this.lvRisks.UseCompatibleStateImageBehavior = false;
            this.lvRisks.View = System.Windows.Forms.View.Details;
            // 
            // lvRisksPositions
            // 
            this.lvRisksPositions.FullRowSelect = true;
            this.lvRisksPositions.GridLines = true;
            this.lvRisksPositions.HideSelection = false;
            this.lvRisksPositions.Location = new System.Drawing.Point(354, 77);
            this.lvRisksPositions.Name = "lvRisksPositions";
            this.lvRisksPositions.Size = new System.Drawing.Size(211, 330);
            this.lvRisksPositions.TabIndex = 2;
            this.lvRisksPositions.UseCompatibleStateImageBehavior = false;
            this.lvRisksPositions.View = System.Windows.Forms.View.Details;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(263, 225);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "-->";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.Location = new System.Drawing.Point(24, 16);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(51, 13);
            this.lblCompany.TabIndex = 4;
            this.lblCompany.Text = "Company";
            // 
            // cbCompany
            // 
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(115, 13);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(121, 21);
            this.cbCompany.TabIndex = 5;
            this.cbCompany.SelectedIndexChanged += new System.EventHandler(this.cbCompany_SelectedIndexChanged);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(263, 265);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPrev.TabIndex = 8;
            this.btnPrev.Text = "<--";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(27, 430);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(490, 430);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // MCRisksPositions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblCompany);
            this.Controls.Add(this.cbCompany);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lvRisksPositions);
            this.Controls.Add(this.lvRisks);
            this.Controls.Add(this.lvPostions);
            this.Name = "MCRisksPositions";
            this.Size = new System.Drawing.Size(612, 480);
            this.Load += new System.EventHandler(this.MCRisksPositions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvPostions;
        private System.Windows.Forms.ListView lvRisks;
        private System.Windows.Forms.ListView lvRisksPositions;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}
