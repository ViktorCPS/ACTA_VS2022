namespace UI
{
    partial class OnlineMealTypes
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
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.btnMealTypeDelete = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealTypeID = new System.Windows.Forms.TextBox();
            this.btnMealTypeUpdate = new System.Windows.Forms.Button();
            this.lblMealTypeID = new System.Windows.Forms.Label();
            this.btnMealTypeAdd = new System.Windows.Forms.Button();
            this.lvMealTypes = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(124, 79);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(197, 20);
            this.tbDescription.TabIndex = 6;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(39, 82);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(79, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(233, 115);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(124, 115);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(124, 49);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(197, 20);
            this.tbName.TabIndex = 4;
            // 
            // btnMealTypeDelete
            // 
            this.btnMealTypeDelete.Location = new System.Drawing.Point(216, 585);
            this.btnMealTypeDelete.Name = "btnMealTypeDelete";
            this.btnMealTypeDelete.Size = new System.Drawing.Size(75, 23);
            this.btnMealTypeDelete.TabIndex = 21;
            this.btnMealTypeDelete.Text = "Delete";
            this.btnMealTypeDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(42, 52);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(76, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMealTypeID
            // 
            this.tbMealTypeID.Location = new System.Drawing.Point(124, 20);
            this.tbMealTypeID.Name = "tbMealTypeID";
            this.tbMealTypeID.Size = new System.Drawing.Size(100, 20);
            this.tbMealTypeID.TabIndex = 2;
            this.tbMealTypeID.TextChanged += new System.EventHandler(this.tbMealTypeID_TextChanged);
            // 
            // btnMealTypeUpdate
            // 
            this.btnMealTypeUpdate.Location = new System.Drawing.Point(118, 585);
            this.btnMealTypeUpdate.Name = "btnMealTypeUpdate";
            this.btnMealTypeUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnMealTypeUpdate.TabIndex = 20;
            this.btnMealTypeUpdate.Text = "Update";
            this.btnMealTypeUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblMealTypeID
            // 
            this.lblMealTypeID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMealTypeID.Location = new System.Drawing.Point(6, 24);
            this.lblMealTypeID.Name = "lblMealTypeID";
            this.lblMealTypeID.Size = new System.Drawing.Size(112, 13);
            this.lblMealTypeID.TabIndex = 1;
            this.lblMealTypeID.Text = "Meal type ID:";
            this.lblMealTypeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnMealTypeAdd
            // 
            this.btnMealTypeAdd.Location = new System.Drawing.Point(11, 585);
            this.btnMealTypeAdd.Name = "btnMealTypeAdd";
            this.btnMealTypeAdd.Size = new System.Drawing.Size(80, 23);
            this.btnMealTypeAdd.TabIndex = 19;
            this.btnMealTypeAdd.Text = "Add new";
            this.btnMealTypeAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvMealTypes
            // 
            this.lvMealTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMealTypes.FullRowSelect = true;
            this.lvMealTypes.GridLines = true;
            this.lvMealTypes.HideSelection = false;
            this.lvMealTypes.Location = new System.Drawing.Point(5, 191);
            this.lvMealTypes.Name = "lvMealTypes";
            this.lvMealTypes.Size = new System.Drawing.Size(824, 369);
            this.lvMealTypes.TabIndex = 18;
            this.lvMealTypes.UseCompatibleStateImageBehavior = false;
            this.lvMealTypes.View = System.Windows.Forms.View.Details;
            this.lvMealTypes.SelectedIndexChanged += new System.EventHandler(this.lvMealTypes_SelectedIndexChanged);
            this.lvMealTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealTypes_ColumnClick_1);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.tbDescription);
            this.gbSearch.Controls.Add(this.lblDescription);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.tbName);
            this.gbSearch.Controls.Add(this.lblName);
            this.gbSearch.Controls.Add(this.tbMealTypeID);
            this.gbSearch.Controls.Add(this.lblMealTypeID);
            this.gbSearch.Location = new System.Drawing.Point(5, 10);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(374, 152);
            this.gbSearch.TabIndex = 17;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // OnlineMealTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnMealTypeDelete);
            this.Controls.Add(this.btnMealTypeUpdate);
            this.Controls.Add(this.btnMealTypeAdd);
            this.Controls.Add(this.lvMealTypes);
            this.Controls.Add(this.gbSearch);
            this.Name = "OnlineMealTypes";
            this.Size = new System.Drawing.Size(857, 622);
            this.Load += new System.EventHandler(this.MealTypes_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Button btnMealTypeDelete;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealTypeID;
        private System.Windows.Forms.Button btnMealTypeUpdate;
        private System.Windows.Forms.Label lblMealTypeID;
        private System.Windows.Forms.Button btnMealTypeAdd;
        private System.Windows.Forms.ListView lvMealTypes;
        private System.Windows.Forms.GroupBox gbSearch;

    }
}
