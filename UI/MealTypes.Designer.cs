namespace UI
{
    partial class MealTypes
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
            this.lvMealTypes = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dtpHoursTo = new System.Windows.Forms.DateTimePicker();
            this.lblHoursTo = new System.Windows.Forms.Label();
            this.dtpHoursFrom = new System.Windows.Forms.DateTimePicker();
            this.lblHoursFrom = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealTypeID = new System.Windows.Forms.TextBox();
            this.lblMealTypeID = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(224, 474);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(126, 474);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(19, 474);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Add new";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvMealTypes
            // 
            this.lvMealTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMealTypes.FullRowSelect = true;
            this.lvMealTypes.GridLines = true;
            this.lvMealTypes.HideSelection = false;
            this.lvMealTypes.Location = new System.Drawing.Point(19, 171);
            this.lvMealTypes.Name = "lvMealTypes";
            this.lvMealTypes.Size = new System.Drawing.Size(704, 287);
            this.lvMealTypes.TabIndex = 13;
            this.lvMealTypes.UseCompatibleStateImageBehavior = false;
            this.lvMealTypes.View = System.Windows.Forms.View.Details;
            this.lvMealTypes.SelectedIndexChanged += new System.EventHandler(this.lvMealTypes_SelectedIndexChanged);
            this.lvMealTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealTypes_ColumnClick);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.tbDescription);
            this.gbSearch.Controls.Add(this.lblDescription);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.dtpHoursTo);
            this.gbSearch.Controls.Add(this.lblHoursTo);
            this.gbSearch.Controls.Add(this.dtpHoursFrom);
            this.gbSearch.Controls.Add(this.lblHoursFrom);
            this.gbSearch.Controls.Add(this.tbName);
            this.gbSearch.Controls.Add(this.lblName);
            this.gbSearch.Controls.Add(this.tbMealTypeID);
            this.gbSearch.Controls.Add(this.lblMealTypeID);
            this.gbSearch.Location = new System.Drawing.Point(19, 3);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(499, 152);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(124, 79);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
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
            this.btnSearch.Location = new System.Drawing.Point(401, 115);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(297, 115);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dtpHoursTo
            // 
            this.dtpHoursTo.CustomFormat = "HH:mm";
            this.dtpHoursTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursTo.Location = new System.Drawing.Point(426, 48);
            this.dtpHoursTo.Name = "dtpHoursTo";
            this.dtpHoursTo.ShowUpDown = true;
            this.dtpHoursTo.Size = new System.Drawing.Size(52, 20);
            this.dtpHoursTo.TabIndex = 10;
            this.dtpHoursTo.Value = new System.DateTime(2008, 8, 11, 23, 59, 0, 0);
            // 
            // lblHoursTo
            // 
            this.lblHoursTo.Location = new System.Drawing.Point(331, 52);
            this.lblHoursTo.Name = "lblHoursTo";
            this.lblHoursTo.Size = new System.Drawing.Size(89, 13);
            this.lblHoursTo.TabIndex = 9;
            this.lblHoursTo.Text = "Hours to:";
            this.lblHoursTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpHoursFrom
            // 
            this.dtpHoursFrom.CustomFormat = "HH:mm";
            this.dtpHoursFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursFrom.Location = new System.Drawing.Point(426, 20);
            this.dtpHoursFrom.Name = "dtpHoursFrom";
            this.dtpHoursFrom.ShowUpDown = true;
            this.dtpHoursFrom.Size = new System.Drawing.Size(52, 20);
            this.dtpHoursFrom.TabIndex = 8;
            this.dtpHoursFrom.Value = new System.DateTime(2008, 8, 11, 0, 0, 0, 0);
            // 
            // lblHoursFrom
            // 
            this.lblHoursFrom.Location = new System.Drawing.Point(336, 24);
            this.lblHoursFrom.Name = "lblHoursFrom";
            this.lblHoursFrom.Size = new System.Drawing.Size(84, 13);
            this.lblHoursFrom.TabIndex = 7;
            this.lblHoursFrom.Text = "Hours from:";
            this.lblHoursFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(124, 49);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 4;
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
            // MealTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvMealTypes);
            this.Controls.Add(this.gbSearch);
            this.Name = "MealTypes";
            this.Size = new System.Drawing.Size(734, 511);
            this.Load += new System.EventHandler(this.MealTypes_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvMealTypes;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DateTimePicker dtpHoursTo;
        private System.Windows.Forms.Label lblHoursTo;
        private System.Windows.Forms.DateTimePicker dtpHoursFrom;
        private System.Windows.Forms.Label lblHoursFrom;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealTypeID;
        private System.Windows.Forms.Label lblMealTypeID;
    }
}
