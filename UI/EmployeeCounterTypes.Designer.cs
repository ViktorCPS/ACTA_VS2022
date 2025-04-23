namespace UI
{
    partial class EmployeeCounterTypes
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.lvECTypes = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbEmployeeCounterTypes = new System.Windows.Forms.GroupBox();
            this.gbEmployeeCounterTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(306, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(27, 29);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // cbName
            // 
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(90, 25);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(192, 21);
            this.cbName.TabIndex = 1;
            // 
            // lvECTypes
            // 
            this.lvECTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvECTypes.FullRowSelect = true;
            this.lvECTypes.GridLines = true;
            this.lvECTypes.HideSelection = false;
            this.lvECTypes.Location = new System.Drawing.Point(30, 67);
            this.lvECTypes.MultiSelect = false;
            this.lvECTypes.Name = "lvECTypes";
            this.lvECTypes.ShowItemToolTips = true;
            this.lvECTypes.Size = new System.Drawing.Size(550, 177);
            this.lvECTypes.TabIndex = 3;
            this.lvECTypes.UseCompatibleStateImageBehavior = false;
            this.lvECTypes.View = System.Windows.Forms.View.Details;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(37, 265);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(130, 265);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(230, 265);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(488, 265);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbEmployeeCounterTypes
            // 
            this.gbEmployeeCounterTypes.Controls.Add(this.btnClose);
            this.gbEmployeeCounterTypes.Controls.Add(this.btnDelete);
            this.gbEmployeeCounterTypes.Controls.Add(this.btnUpdate);
            this.gbEmployeeCounterTypes.Controls.Add(this.btnAdd);
            this.gbEmployeeCounterTypes.Controls.Add(this.lvECTypes);
            this.gbEmployeeCounterTypes.Controls.Add(this.btnSearch);
            this.gbEmployeeCounterTypes.Controls.Add(this.lblName);
            this.gbEmployeeCounterTypes.Controls.Add(this.cbName);
            this.gbEmployeeCounterTypes.Location = new System.Drawing.Point(12, 22);
            this.gbEmployeeCounterTypes.Name = "gbEmployeeCounterTypes";
            this.gbEmployeeCounterTypes.Size = new System.Drawing.Size(608, 316);
            this.gbEmployeeCounterTypes.TabIndex = 0;
            this.gbEmployeeCounterTypes.TabStop = false;
            this.gbEmployeeCounterTypes.Text = "Counter types";
            // 
            // EmployeeCounterTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 358);
            this.ControlBox = false;
            this.Controls.Add(this.gbEmployeeCounterTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EmployeeCounterTypes";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmployeeCounterTypes";
            this.gbEmployeeCounterTypes.ResumeLayout(false);
            this.gbEmployeeCounterTypes.PerformLayout();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbName;
        private System.Windows.Forms.ListView lvECTypes;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbEmployeeCounterTypes;

    }
}