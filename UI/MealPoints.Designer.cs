namespace UI
{
    partial class MealPoints
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
            this.lvMealPoints = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.tbTerminalSerial = new System.Windows.Forms.TextBox();
            this.lblTerminalSerial = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealPointID = new System.Windows.Forms.TextBox();
            this.lblPointID = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(206, 468);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(114, 468);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(15, 468);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Add new";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvMealPoints
            // 
            this.lvMealPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMealPoints.FullRowSelect = true;
            this.lvMealPoints.GridLines = true;
            this.lvMealPoints.HideSelection = false;
            this.lvMealPoints.Location = new System.Drawing.Point(15, 162);
            this.lvMealPoints.Name = "lvMealPoints";
            this.lvMealPoints.Size = new System.Drawing.Size(703, 300);
            this.lvMealPoints.TabIndex = 11;
            this.lvMealPoints.UseCompatibleStateImageBehavior = false;
            this.lvMealPoints.View = System.Windows.Forms.View.Details;
            this.lvMealPoints.SelectedIndexChanged += new System.EventHandler(this.lvMealPoints_SelectedIndexChanged);
            this.lvMealPoints.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealPoints_ColumnClick);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.tbTerminalSerial);
            this.gbSearch.Controls.Add(this.lblTerminalSerial);
            this.gbSearch.Controls.Add(this.tbDescription);
            this.gbSearch.Controls.Add(this.lblDescription);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.tbName);
            this.gbSearch.Controls.Add(this.lblName);
            this.gbSearch.Controls.Add(this.tbMealPointID);
            this.gbSearch.Controls.Add(this.lblPointID);
            this.gbSearch.Location = new System.Drawing.Point(15, 18);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(595, 127);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // tbTerminalSerial
            // 
            this.tbTerminalSerial.Location = new System.Drawing.Point(142, 49);
            this.tbTerminalSerial.Name = "tbTerminalSerial";
            this.tbTerminalSerial.Size = new System.Drawing.Size(165, 20);
            this.tbTerminalSerial.TabIndex = 4;
            // 
            // lblTerminalSerial
            // 
            this.lblTerminalSerial.Location = new System.Drawing.Point(9, 52);
            this.lblTerminalSerial.Name = "lblTerminalSerial";
            this.lblTerminalSerial.Size = new System.Drawing.Size(127, 13);
            this.lblTerminalSerial.TabIndex = 3;
            this.lblTerminalSerial.Text = "Terminal serial:";
            this.lblTerminalSerial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(405, 49);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
            this.tbDescription.TabIndex = 8;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(329, 52);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(70, 13);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(482, 87);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(371, 87);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(405, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(326, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(73, 13);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMealPointID
            // 
            this.tbMealPointID.Location = new System.Drawing.Point(142, 19);
            this.tbMealPointID.Name = "tbMealPointID";
            this.tbMealPointID.Size = new System.Drawing.Size(100, 20);
            this.tbMealPointID.TabIndex = 2;
            this.tbMealPointID.TextChanged += new System.EventHandler(this.tbMealPointID_TextChanged);
            // 
            // lblPointID
            // 
            this.lblPointID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPointID.Location = new System.Drawing.Point(6, 22);
            this.lblPointID.Name = "lblPointID";
            this.lblPointID.Size = new System.Drawing.Size(130, 13);
            this.lblPointID.TabIndex = 1;
            this.lblPointID.Text = "Point ID:";
            this.lblPointID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MealPoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvMealPoints);
            this.Controls.Add(this.gbSearch);
            this.Name = "MealPoints";
            this.Size = new System.Drawing.Size(734, 511);
            this.Load += new System.EventHandler(this.MealPoints_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvMealPoints;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.TextBox tbTerminalSerial;
        private System.Windows.Forms.Label lblTerminalSerial;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealPointID;
        private System.Windows.Forms.Label lblPointID;
    }
}
