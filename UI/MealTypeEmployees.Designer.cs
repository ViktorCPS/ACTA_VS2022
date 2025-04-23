namespace UI
{
    partial class MealTypeEmployees
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvMealTypeEmpl = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealTypeEmplID = new System.Windows.Forms.TextBox();
            this.lblMealTypeEmplID = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(219, 467);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(119, 467);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 467);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add new";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvMealTypeEmpl
            // 
            this.lvMealTypeEmpl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMealTypeEmpl.FullRowSelect = true;
            this.lvMealTypeEmpl.GridLines = true;
            this.lvMealTypeEmpl.HideSelection = false;
            this.lvMealTypeEmpl.Location = new System.Drawing.Point(14, 177);
            this.lvMealTypeEmpl.Name = "lvMealTypeEmpl";
            this.lvMealTypeEmpl.Size = new System.Drawing.Size(704, 274);
            this.lvMealTypeEmpl.TabIndex = 9;
            this.lvMealTypeEmpl.UseCompatibleStateImageBehavior = false;
            this.lvMealTypeEmpl.View = System.Windows.Forms.View.Details;
            this.lvMealTypeEmpl.SelectedIndexChanged += new System.EventHandler(this.lvMealTypeEmpl_SelectedIndexChanged);
            this.lvMealTypeEmpl.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealTypeEmpl_ColumnClick);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.tbDescription);
            this.gbSearch.Controls.Add(this.lblDescription);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.tbName);
            this.gbSearch.Controls.Add(this.lblName);
            this.gbSearch.Controls.Add(this.tbMealTypeEmplID);
            this.gbSearch.Controls.Add(this.lblMealTypeEmplID);
            this.gbSearch.Location = new System.Drawing.Point(14, 14);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(528, 142);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(141, 80);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
            this.tbDescription.TabIndex = 6;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(9, 83);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(126, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(407, 108);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(313, 108);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(141, 51);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(25, 54);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(110, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMealTypeEmplID
            // 
            this.tbMealTypeEmplID.Location = new System.Drawing.Point(141, 22);
            this.tbMealTypeEmplID.Name = "tbMealTypeEmplID";
            this.tbMealTypeEmplID.Size = new System.Drawing.Size(100, 20);
            this.tbMealTypeEmplID.TabIndex = 2;
            this.tbMealTypeEmplID.TextChanged += new System.EventHandler(this.tbMealTypeEmplID_TextChanged);
            // 
            // lblMealTypeEmplID
            // 
            this.lblMealTypeEmplID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMealTypeEmplID.Location = new System.Drawing.Point(6, 25);
            this.lblMealTypeEmplID.Name = "lblMealTypeEmplID";
            this.lblMealTypeEmplID.Size = new System.Drawing.Size(129, 13);
            this.lblMealTypeEmplID.TabIndex = 1;
            this.lblMealTypeEmplID.Text = "Meal type empl ID:";
            this.lblMealTypeEmplID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MealTypeEmployees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvMealTypeEmpl);
            this.Controls.Add(this.gbSearch);
            this.Name = "MealTypeEmployees";
            this.Size = new System.Drawing.Size(734, 511);
            this.Load += new System.EventHandler(this.MealTypeEmployees_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvMealTypeEmpl;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealTypeEmplID;
        private System.Windows.Forms.Label lblMealTypeEmplID;
    }
}
