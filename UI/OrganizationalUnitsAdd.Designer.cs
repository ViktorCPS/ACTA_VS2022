namespace UI
{
    partial class OrganizationalUnitsAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrganizationalUnitsAdd));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbOUID = new System.Windows.Forms.TextBox();
            this.cbParentUnitID = new System.Windows.Forms.ComboBox();
            this.lblParentOUID = new System.Windows.Forms.Label();
            this.lblStar1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblOrgUnitID = new System.Windows.Forms.Label();
            this.bntWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWUID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(339, 205);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(17, 205);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 18;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(17, 205);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOUTree
            // 
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(386, 128);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 11;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(156, 52);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(224, 20);
            this.tbName.TabIndex = 4;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(156, 78);
            this.tbDescription.MaxLength = 50;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(224, 20);
            this.tbDescription.TabIndex = 6;
            // 
            // tbOUID
            // 
            this.tbOUID.Location = new System.Drawing.Point(156, 26);
            this.tbOUID.Name = "tbOUID";
            this.tbOUID.Size = new System.Drawing.Size(224, 20);
            this.tbOUID.TabIndex = 1;
            // 
            // cbParentUnitID
            // 
            this.cbParentUnitID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbParentUnitID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbParentUnitID.Location = new System.Drawing.Point(156, 130);
            this.cbParentUnitID.Name = "cbParentUnitID";
            this.cbParentUnitID.Size = new System.Drawing.Size(224, 21);
            this.cbParentUnitID.TabIndex = 10;
            // 
            // lblParentOUID
            // 
            this.lblParentOUID.Location = new System.Drawing.Point(1, 130);
            this.lblParentOUID.Name = "lblParentOUID";
            this.lblParentOUID.Size = new System.Drawing.Size(149, 23);
            this.lblParentOUID.TabIndex = 9;
            this.lblParentOUID.Text = "Parent organizational unit ID:";
            this.lblParentOUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStar1
            // 
            this.lblStar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar1.ForeColor = System.Drawing.Color.Red;
            this.lblStar1.Location = new System.Drawing.Point(388, 26);
            this.lblStar1.Name = "lblStar1";
            this.lblStar1.Size = new System.Drawing.Size(16, 16);
            this.lblStar1.TabIndex = 2;
            this.lblStar1.Text = "*";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(20, 52);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(130, 23);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(20, 78);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(130, 23);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOrgUnitID
            // 
            this.lblOrgUnitID.Location = new System.Drawing.Point(20, 26);
            this.lblOrgUnitID.Name = "lblOrgUnitID";
            this.lblOrgUnitID.Size = new System.Drawing.Size(130, 23);
            this.lblOrgUnitID.TabIndex = 0;
            this.lblOrgUnitID.Text = "Organizational unit ID:";
            this.lblOrgUnitID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bntWUTree
            // 
            this.bntWUTree.Image = ((System.Drawing.Image)(resources.GetObject("bntWUTree.Image")));
            this.bntWUTree.Location = new System.Drawing.Point(386, 155);
            this.bntWUTree.Name = "bntWUTree";
            this.bntWUTree.Size = new System.Drawing.Size(25, 23);
            this.bntWUTree.TabIndex = 15;
            this.bntWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.Location = new System.Drawing.Point(156, 157);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(224, 21);
            this.cbWU.TabIndex = 14;
            // 
            // lblWUID
            // 
            this.lblWUID.Location = new System.Drawing.Point(12, 157);
            this.lblWUID.Name = "lblWUID";
            this.lblWUID.Size = new System.Drawing.Size(138, 23);
            this.lblWUID.TabIndex = 13;
            this.lblWUID.Text = "Working unit ID:";
            this.lblWUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(417, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "*";
            // 
            // tbCode
            // 
            this.tbCode.Location = new System.Drawing.Point(156, 104);
            this.tbCode.MaxLength = 50;
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(224, 20);
            this.tbCode.TabIndex = 8;
            // 
            // lblCode
            // 
            this.lblCode.Location = new System.Drawing.Point(20, 104);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(130, 23);
            this.lblCode.TabIndex = 7;
            this.lblCode.Text = "Code:";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OrganizationalUnitsAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 257);
            this.ControlBox = false;
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bntWUTree);
            this.Controls.Add(this.cbWU);
            this.Controls.Add(this.lblWUID);
            this.Controls.Add(this.btnOUTree);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbOUID);
            this.Controls.Add(this.cbParentUnitID);
            this.Controls.Add(this.lblParentOUID);
            this.Controls.Add(this.lblStar1);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblOrgUnitID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OrganizationalUnitsAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Organizational Units Add";
            this.Load += new System.EventHandler(this.OrganizationalUnitsAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbOUID;
        private System.Windows.Forms.ComboBox cbParentUnitID;
        private System.Windows.Forms.Label lblParentOUID;
        private System.Windows.Forms.Label lblStar1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblOrgUnitID;
        private System.Windows.Forms.Button bntWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWUID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.Label lblCode;
    }
}