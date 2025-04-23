namespace UI
{
    partial class OnlineMealPoint
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
            this.lblTerminalSerial = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnLineDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealPointID = new System.Windows.Forms.TextBox();
            this.btnLineUpdate = new System.Windows.Forms.Button();
            this.lblPointID = new System.Windows.Forms.Label();
            this.btnLineAdd = new System.Windows.Forms.Button();
            this.lvMealPoints = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.cbRestaurant = new System.Windows.Forms.ComboBox();
            this.cbMealType = new System.Windows.Forms.ComboBox();
            this.lblMealType = new System.Windows.Forms.Label();
            this.tbPeripherial = new System.Windows.Forms.TextBox();
            this.lblPreipherial = new System.Windows.Forms.Label();
            this.tbPeripherialDesc = new System.Windows.Forms.TextBox();
            this.lblPeripherialDesc = new System.Windows.Forms.Label();
            this.tbIPAddress = new System.Windows.Forms.TextBox();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.tbAntenna = new System.Windows.Forms.TextBox();
            this.lblAntenna = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTerminalSerial
            // 
            this.lblTerminalSerial.Location = new System.Drawing.Point(334, 51);
            this.lblTerminalSerial.Name = "lblTerminalSerial";
            this.lblTerminalSerial.Size = new System.Drawing.Size(127, 13);
            this.lblTerminalSerial.TabIndex = 3;
            this.lblTerminalSerial.Text = "Restaurant:";
            this.lblTerminalSerial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(166, 69);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
            this.tbDescription.TabIndex = 8;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(90, 72);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(70, 13);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(544, 160);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnLineDelete
            // 
            this.btnLineDelete.Location = new System.Drawing.Point(203, 556);
            this.btnLineDelete.Name = "btnLineDelete";
            this.btnLineDelete.Size = new System.Drawing.Size(75, 23);
            this.btnLineDelete.TabIndex = 19;
            this.btnLineDelete.Text = "Delete";
            this.btnLineDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(435, 160);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(166, 43);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(87, 46);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(73, 13);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMealPointID
            // 
            this.tbMealPointID.Location = new System.Drawing.Point(166, 19);
            this.tbMealPointID.Name = "tbMealPointID";
            this.tbMealPointID.Size = new System.Drawing.Size(100, 20);
            this.tbMealPointID.TabIndex = 2;
            // 
            // btnLineUpdate
            // 
            this.btnLineUpdate.Location = new System.Drawing.Point(111, 556);
            this.btnLineUpdate.Name = "btnLineUpdate";
            this.btnLineUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnLineUpdate.TabIndex = 18;
            this.btnLineUpdate.Text = "Update";
            this.btnLineUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblPointID
            // 
            this.lblPointID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPointID.Location = new System.Drawing.Point(49, 22);
            this.lblPointID.Name = "lblPointID";
            this.lblPointID.Size = new System.Drawing.Size(111, 13);
            this.lblPointID.TabIndex = 1;
            this.lblPointID.Text = "Line ID:";
            this.lblPointID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLineAdd
            // 
            this.btnLineAdd.Location = new System.Drawing.Point(12, 556);
            this.btnLineAdd.Name = "btnLineAdd";
            this.btnLineAdd.Size = new System.Drawing.Size(80, 23);
            this.btnLineAdd.TabIndex = 17;
            this.btnLineAdd.Text = "Add new";
            this.btnLineAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvMealPoints
            // 
            this.lvMealPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMealPoints.FullRowSelect = true;
            this.lvMealPoints.GridLines = true;
            this.lvMealPoints.HideSelection = false;
            this.lvMealPoints.Location = new System.Drawing.Point(12, 235);
            this.lvMealPoints.Name = "lvMealPoints";
            this.lvMealPoints.Size = new System.Drawing.Size(907, 304);
            this.lvMealPoints.TabIndex = 16;
            this.lvMealPoints.UseCompatibleStateImageBehavior = false;
            this.lvMealPoints.View = System.Windows.Forms.View.Details;
            this.lvMealPoints.SelectedIndexChanged += new System.EventHandler(this.lvMealPoints_SelectedIndexChanged);
            this.lvMealPoints.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealPoints_ColumnClick_1);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.cbRestaurant);
            this.gbSearch.Controls.Add(this.cbMealType);
            this.gbSearch.Controls.Add(this.lblMealType);
            this.gbSearch.Controls.Add(this.tbPeripherial);
            this.gbSearch.Controls.Add(this.lblPreipherial);
            this.gbSearch.Controls.Add(this.tbPeripherialDesc);
            this.gbSearch.Controls.Add(this.lblPeripherialDesc);
            this.gbSearch.Controls.Add(this.tbIPAddress);
            this.gbSearch.Controls.Add(this.lblIPAddress);
            this.gbSearch.Controls.Add(this.tbAntenna);
            this.gbSearch.Controls.Add(this.lblAntenna);
            this.gbSearch.Controls.Add(this.lblTerminalSerial);
            this.gbSearch.Controls.Add(this.tbDescription);
            this.gbSearch.Controls.Add(this.lblDescription);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.tbName);
            this.gbSearch.Controls.Add(this.lblName);
            this.gbSearch.Controls.Add(this.tbMealPointID);
            this.gbSearch.Controls.Add(this.lblPointID);
            this.gbSearch.Location = new System.Drawing.Point(12, 20);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(703, 189);
            this.gbSearch.TabIndex = 15;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // cbRestaurant
            // 
            this.cbRestaurant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRestaurant.FormattingEnabled = true;
            this.cbRestaurant.Location = new System.Drawing.Point(467, 44);
            this.cbRestaurant.Name = "cbRestaurant";
            this.cbRestaurant.Size = new System.Drawing.Size(165, 21);
            this.cbRestaurant.TabIndex = 22;
            // 
            // cbMealType
            // 
            this.cbMealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMealType.FormattingEnabled = true;
            this.cbMealType.Location = new System.Drawing.Point(467, 15);
            this.cbMealType.Name = "cbMealType";
            this.cbMealType.Size = new System.Drawing.Size(165, 21);
            this.cbMealType.TabIndex = 21;
            // 
            // lblMealType
            // 
            this.lblMealType.Location = new System.Drawing.Point(334, 20);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(127, 13);
            this.lblMealType.TabIndex = 19;
            this.lblMealType.Text = "Meal type:";
            this.lblMealType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPeripherial
            // 
            this.tbPeripherial.Location = new System.Drawing.Point(166, 95);
            this.tbPeripherial.Name = "tbPeripherial";
            this.tbPeripherial.Size = new System.Drawing.Size(165, 20);
            this.tbPeripherial.TabIndex = 16;
            // 
            // lblPreipherial
            // 
            this.lblPreipherial.Location = new System.Drawing.Point(33, 98);
            this.lblPreipherial.Name = "lblPreipherial";
            this.lblPreipherial.Size = new System.Drawing.Size(127, 13);
            this.lblPreipherial.TabIndex = 15;
            this.lblPreipherial.Text = "Peripherial:";
            this.lblPreipherial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPeripherialDesc
            // 
            this.tbPeripherialDesc.Location = new System.Drawing.Point(166, 123);
            this.tbPeripherialDesc.Name = "tbPeripherialDesc";
            this.tbPeripherialDesc.Size = new System.Drawing.Size(165, 20);
            this.tbPeripherialDesc.TabIndex = 18;
            // 
            // lblPeripherialDesc
            // 
            this.lblPeripherialDesc.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPeripherialDesc.Location = new System.Drawing.Point(5, 126);
            this.lblPeripherialDesc.Name = "lblPeripherialDesc";
            this.lblPeripherialDesc.Size = new System.Drawing.Size(155, 13);
            this.lblPeripherialDesc.TabIndex = 17;
            this.lblPeripherialDesc.Text = "Peripherial description:";
            this.lblPeripherialDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbIPAddress
            // 
            this.tbIPAddress.Location = new System.Drawing.Point(467, 72);
            this.tbIPAddress.Name = "tbIPAddress";
            this.tbIPAddress.Size = new System.Drawing.Size(165, 20);
            this.tbIPAddress.TabIndex = 12;
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.Location = new System.Drawing.Point(334, 75);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(127, 13);
            this.lblIPAddress.TabIndex = 11;
            this.lblIPAddress.Text = "IP address:";
            this.lblIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAntenna
            // 
            this.tbAntenna.Location = new System.Drawing.Point(467, 98);
            this.tbAntenna.Name = "tbAntenna";
            this.tbAntenna.Size = new System.Drawing.Size(165, 20);
            this.tbAntenna.TabIndex = 14;
            // 
            // lblAntenna
            // 
            this.lblAntenna.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAntenna.Location = new System.Drawing.Point(359, 101);
            this.lblAntenna.Name = "lblAntenna";
            this.lblAntenna.Size = new System.Drawing.Size(102, 13);
            this.lblAntenna.TabIndex = 13;
            this.lblAntenna.Text = "Reader antenna:";
            this.lblAntenna.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OnlineMealPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLineDelete);
            this.Controls.Add(this.btnLineUpdate);
            this.Controls.Add(this.btnLineAdd);
            this.Controls.Add(this.lvMealPoints);
            this.Controls.Add(this.gbSearch);
            this.Name = "OnlineMealPoint";
            this.Size = new System.Drawing.Size(944, 607);
            this.Load += new System.EventHandler(this.MealPoints_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTerminalSerial;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnLineDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealPointID;
        private System.Windows.Forms.Button btnLineUpdate;
        private System.Windows.Forms.Label lblPointID;
        private System.Windows.Forms.Button btnLineAdd;
        private System.Windows.Forms.ListView lvMealPoints;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.TextBox tbIPAddress;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.TextBox tbAntenna;
        private System.Windows.Forms.Label lblAntenna;
        private System.Windows.Forms.TextBox tbPeripherial;
        private System.Windows.Forms.Label lblPreipherial;
        private System.Windows.Forms.TextBox tbPeripherialDesc;
        private System.Windows.Forms.Label lblPeripherialDesc;
        private System.Windows.Forms.Label lblMealType;
        private System.Windows.Forms.ComboBox cbMealType;
        private System.Windows.Forms.ComboBox cbRestaurant;
    }
}
