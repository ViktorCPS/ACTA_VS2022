namespace UI
{
    partial class Rules
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
            this.lvRules = new System.Windows.Forms.ListView();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblType = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblEmplType = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.cbEmplType = new System.Windows.Forms.ComboBox();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbRuleDesc = new System.Windows.Forms.TextBox();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvRules
            // 
            this.lvRules.FullRowSelect = true;
            this.lvRules.GridLines = true;
            this.lvRules.HideSelection = false;
            this.lvRules.Location = new System.Drawing.Point(12, 102);
            this.lvRules.MultiSelect = false;
            this.lvRules.Name = "lvRules";
            this.lvRules.ShowItemToolTips = true;
            this.lvRules.Size = new System.Drawing.Size(761, 370);
            this.lvRules.TabIndex = 1;
            this.lvRules.UseCompatibleStateImageBehavior = false;
            this.lvRules.View = System.Windows.Forms.View.Details;
            this.lvRules.SelectedIndexChanged += new System.EventHandler(this.lvRules_SelectedIndexChanged);
            this.lvRules.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvRules_ColumnClick);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSearch);
            this.gbFilter.Controls.Add(this.lblType);
            this.gbFilter.Controls.Add(this.cbType);
            this.gbFilter.Controls.Add(this.lblEmplType);
            this.gbFilter.Controls.Add(this.lblCompany);
            this.gbFilter.Controls.Add(this.cbEmplType);
            this.gbFilter.Controls.Add(this.cbCompany);
            this.gbFilter.Location = new System.Drawing.Point(12, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(761, 79);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(614, 43);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(127, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(388, 21);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(62, 13);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbType
            // 
            this.cbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(456, 18);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(285, 21);
            this.cbType.TabIndex = 3;
            // 
            // lblEmplType
            // 
            this.lblEmplType.Location = new System.Drawing.Point(9, 48);
            this.lblEmplType.Name = "lblEmplType";
            this.lblEmplType.Size = new System.Drawing.Size(83, 13);
            this.lblEmplType.TabIndex = 4;
            this.lblEmplType.Text = "Employee type:";
            this.lblEmplType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(6, 21);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(86, 13);
            this.lblCompany.TabIndex = 0;
            this.lblCompany.Text = "Company:";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmplType
            // 
            this.cbEmplType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmplType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmplType.FormattingEnabled = true;
            this.cbEmplType.Location = new System.Drawing.Point(98, 45);
            this.cbEmplType.Name = "cbEmplType";
            this.cbEmplType.Size = new System.Drawing.Size(284, 21);
            this.cbEmplType.TabIndex = 5;
            // 
            // cbCompany
            // 
            this.cbCompany.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCompany.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(98, 18);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(284, 21);
            this.cbCompany.TabIndex = 1;
            this.cbCompany.SelectedIndexChanged += new System.EventHandler(this.cbCompany_SelectedIndexChanged);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 552);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(698, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbRuleDesc
            // 
            this.tbRuleDesc.Enabled = false;
            this.tbRuleDesc.Location = new System.Drawing.Point(12, 485);
            this.tbRuleDesc.Multiline = true;
            this.tbRuleDesc.Name = "tbRuleDesc";
            this.tbRuleDesc.Size = new System.Drawing.Size(761, 61);
            this.tbRuleDesc.TabIndex = 2;
            // 
            // Rules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 604);
            this.ControlBox = false;
            this.Controls.Add(this.tbRuleDesc);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.lvRules);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Rules";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rules";
            this.Load += new System.EventHandler(this.Rules_Load);
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvRules;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.ComboBox cbEmplType;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lblEmplType;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbRuleDesc;
    }
}