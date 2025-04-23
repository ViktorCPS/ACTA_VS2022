namespace UI
{
    partial class OnlineMealRestaurant
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
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbRestaurantID = new System.Windows.Forms.TextBox();
            this.lblRestaurantID = new System.Windows.Forms.Label();
            this.lvMealTypes = new System.Windows.Forms.ListView();
            this.btnRestaurantAdd = new System.Windows.Forms.Button();
            this.btnRestaurantDelete = new System.Windows.Forms.Button();
            this.btnRestaurantUpdate = new System.Windows.Forms.Button();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.tbDescription);
            this.gbSearch.Controls.Add(this.lblDescription);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.tbName);
            this.gbSearch.Controls.Add(this.lblName);
            this.gbSearch.Controls.Add(this.tbRestaurantID);
            this.gbSearch.Controls.Add(this.lblRestaurantID);
            this.gbSearch.Location = new System.Drawing.Point(8, 7);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(374, 167);
            this.gbSearch.TabIndex = 27;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(130, 88);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(197, 20);
            this.tbDescription.TabIndex = 6;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(45, 91);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(79, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(239, 138);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(130, 138);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(130, 58);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(197, 20);
            this.tbName.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(48, 61);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(76, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRestaurantID
            // 
            this.tbRestaurantID.Location = new System.Drawing.Point(130, 29);
            this.tbRestaurantID.Name = "tbRestaurantID";
            this.tbRestaurantID.Size = new System.Drawing.Size(100, 20);
            this.tbRestaurantID.TabIndex = 2;
            this.tbRestaurantID.TextChanged += new System.EventHandler(this.tbMealTypeID_TextChanged);
            // 
            // lblRestaurantID
            // 
            this.lblRestaurantID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRestaurantID.Location = new System.Drawing.Point(12, 33);
            this.lblRestaurantID.Name = "lblRestaurantID";
            this.lblRestaurantID.Size = new System.Drawing.Size(112, 13);
            this.lblRestaurantID.TabIndex = 1;
            this.lblRestaurantID.Text = "Restaurant ID:";
            this.lblRestaurantID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvMealTypes
            // 
            this.lvMealTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMealTypes.FullRowSelect = true;
            this.lvMealTypes.GridLines = true;
            this.lvMealTypes.HideSelection = false;
            this.lvMealTypes.Location = new System.Drawing.Point(8, 180);
            this.lvMealTypes.Name = "lvMealTypes";
            this.lvMealTypes.Size = new System.Drawing.Size(824, 369);
            this.lvMealTypes.TabIndex = 28;
            this.lvMealTypes.UseCompatibleStateImageBehavior = false;
            this.lvMealTypes.View = System.Windows.Forms.View.Details;
            this.lvMealTypes.SelectedIndexChanged += new System.EventHandler(this.lvMealTypes_SelectedIndexChanged);
            this.lvMealTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealTypes_ColumnClick);
            // 
            // btnRestaurantAdd
            // 
            this.btnRestaurantAdd.Location = new System.Drawing.Point(8, 579);
            this.btnRestaurantAdd.Name = "btnRestaurantAdd";
            this.btnRestaurantAdd.Size = new System.Drawing.Size(80, 23);
            this.btnRestaurantAdd.TabIndex = 29;
            this.btnRestaurantAdd.Text = "Add new";
            this.btnRestaurantAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRestaurantDelete
            // 
            this.btnRestaurantDelete.Location = new System.Drawing.Point(213, 579);
            this.btnRestaurantDelete.Name = "btnRestaurantDelete";
            this.btnRestaurantDelete.Size = new System.Drawing.Size(75, 23);
            this.btnRestaurantDelete.TabIndex = 31;
            this.btnRestaurantDelete.Text = "Delete";
            this.btnRestaurantDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRestaurantUpdate
            // 
            this.btnRestaurantUpdate.Location = new System.Drawing.Point(115, 579);
            this.btnRestaurantUpdate.Name = "btnRestaurantUpdate";
            this.btnRestaurantUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnRestaurantUpdate.TabIndex = 30;
            this.btnRestaurantUpdate.Text = "Update";
            this.btnRestaurantUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // OnlineMealRestaurant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.lvMealTypes);
            this.Controls.Add(this.btnRestaurantAdd);
            this.Controls.Add(this.btnRestaurantDelete);
            this.Controls.Add(this.btnRestaurantUpdate);
            this.Name = "OnlineMealRestaurant";
            this.Size = new System.Drawing.Size(857, 622);
            this.Load += new System.EventHandler(this.MealTypes_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbRestaurantID;
        private System.Windows.Forms.Label lblRestaurantID;
        private System.Windows.Forms.ListView lvMealTypes;
        private System.Windows.Forms.Button btnRestaurantAdd;
        private System.Windows.Forms.Button btnRestaurantDelete;
        private System.Windows.Forms.Button btnRestaurantUpdate;

    }
}
