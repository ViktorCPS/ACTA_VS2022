namespace UI
{
    partial class MCRisks
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
            this.lblCompany = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.gbSearchRisk = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblDefaultRot = new System.Windows.Forms.Label();
            this.cmbDefRot = new System.Windows.Forms.ComboBox();
            this.lvRisks = new System.Windows.Forms.ListView();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gbSearchRisk.SuspendLayout();
            this.SuspendLayout();
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
            // gbSearchRisk
            // 
            this.gbSearchRisk.Controls.Add(this.btnSearch);
            this.gbSearchRisk.Controls.Add(this.txtDesc);
            this.gbSearchRisk.Controls.Add(this.lblDesc);
            this.gbSearchRisk.Controls.Add(this.txtCode);
            this.gbSearchRisk.Controls.Add(this.lblCode);
            this.gbSearchRisk.Controls.Add(this.lblDefaultRot);
            this.gbSearchRisk.Controls.Add(this.cmbDefRot);
            this.gbSearchRisk.Controls.Add(this.lblCompany);
            this.gbSearchRisk.Controls.Add(this.cbCompany);
            this.gbSearchRisk.Location = new System.Drawing.Point(14, 15);
            this.gbSearchRisk.Name = "gbSearchRisk";
            this.gbSearchRisk.Size = new System.Drawing.Size(535, 152);
            this.gbSearchRisk.TabIndex = 2;
            this.gbSearchRisk.TabStop = false;
            this.gbSearchRisk.Text = "Search";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(313, 104);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(175, 107);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(121, 20);
            this.txtDesc.TabIndex = 7;
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(19, 110);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(60, 13);
            this.lblDesc.TabIndex = 6;
            this.lblDesc.Text = "Description";
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
            // cmbDefRot
            // 
            this.cmbDefRot.FormattingEnabled = true;
            this.cmbDefRot.Location = new System.Drawing.Point(175, 54);
            this.cmbDefRot.Name = "cmbDefRot";
            this.cmbDefRot.Size = new System.Drawing.Size(121, 21);
            this.cmbDefRot.TabIndex = 3;
            // 
            // lvRisks
            // 
            this.lvRisks.FullRowSelect = true;
            this.lvRisks.GridLines = true;
            this.lvRisks.Location = new System.Drawing.Point(14, 198);
            this.lvRisks.Name = "lvRisks";
            this.lvRisks.Size = new System.Drawing.Size(578, 183);
            this.lvRisks.TabIndex = 3;
            this.lvRisks.UseCompatibleStateImageBehavior = false;
            this.lvRisks.View = System.Windows.Forms.View.Details;
            this.lvRisks.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvVRisks_ColumnClick);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(548, 169);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(44, 23);
            this.btnNext.TabIndex = 34;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(489, 169);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(44, 23);
            this.btnPrev.TabIndex = 33;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 387);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 35;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(215, 387);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 36;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(113, 387);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 37;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // MCRisks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lvRisks);
            this.Controls.Add(this.gbSearchRisk);
            this.Name = "MCRisks";
            this.Size = new System.Drawing.Size(600, 440);
            this.Load += new System.EventHandler(this.MCRisks_Load);
            this.gbSearchRisk.ResumeLayout(false);
            this.gbSearchRisk.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.GroupBox gbSearchRisk;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblDefaultRot;
        private System.Windows.Forms.ComboBox cmbDefRot;
        private System.Windows.Forms.ListView lvRisks;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
    }
}
