namespace UIFeatures
{
    partial class FilterAdd
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbDefault = new System.Windows.Forms.CheckBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lvFilters = new System.Windows.Forms.ListView();
            this.gbFilters = new System.Windows.Forms.GroupBox();
            this.btnAllFilters = new System.Windows.Forms.Button();
            this.lblDoubleClickToDelete = new System.Windows.Forms.Label();
            this.gbFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(427, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 43;
            this.label1.Text = "*";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(115, 57);
            this.tbDescription.MaxLength = 128;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(306, 20);
            this.tbDescription.TabIndex = 42;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(5, 55);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(104, 23);
            this.lblDescription.TabIndex = 41;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(115, 22);
            this.tbName.MaxLength = 128;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(306, 20);
            this.tbName.TabIndex = 40;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(5, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(104, 23);
            this.lblName.TabIndex = 39;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 146);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 44;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(424, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 45;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbDefault
            // 
            this.cbDefault.AutoSize = true;
            this.cbDefault.Location = new System.Drawing.Point(115, 107);
            this.cbDefault.Name = "cbDefault";
            this.cbDefault.Size = new System.Drawing.Size(60, 17);
            this.cbDefault.TabIndex = 46;
            this.cbDefault.Text = "Default";
            this.cbDefault.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 146);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 47;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lvFilters
            // 
            this.lvFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFilters.FullRowSelect = true;
            this.lvFilters.GridLines = true;
            this.lvFilters.HideSelection = false;
            this.lvFilters.Location = new System.Drawing.Point(6, 19);
            this.lvFilters.MultiSelect = false;
            this.lvFilters.Name = "lvFilters";
            this.lvFilters.Size = new System.Drawing.Size(475, 191);
            this.lvFilters.TabIndex = 48;
            this.lvFilters.UseCompatibleStateImageBehavior = false;
            this.lvFilters.View = System.Windows.Forms.View.Details;
            this.lvFilters.DoubleClick += new System.EventHandler(this.lvFilters_DoubleClick);
            this.lvFilters.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFilters_ColumnClick);
            // 
            // gbFilters
            // 
            this.gbFilters.Controls.Add(this.lvFilters);
            this.gbFilters.Location = new System.Drawing.Point(12, 218);
            this.gbFilters.Name = "gbFilters";
            this.gbFilters.Size = new System.Drawing.Size(487, 216);
            this.gbFilters.TabIndex = 49;
            this.gbFilters.TabStop = false;
            this.gbFilters.Text = "Filters";
            // 
            // btnAllFilters
            // 
            this.btnAllFilters.Location = new System.Drawing.Point(318, 146);
            this.btnAllFilters.Name = "btnAllFilters";
            this.btnAllFilters.Size = new System.Drawing.Size(100, 23);
            this.btnAllFilters.TabIndex = 50;
            this.btnAllFilters.Text = "Preview filters >>";
            this.btnAllFilters.UseVisualStyleBackColor = true;
            this.btnAllFilters.Click += new System.EventHandler(this.btnAllFilters_Click);
            // 
            // lblDoubleClickToDelete
            // 
            this.lblDoubleClickToDelete.Location = new System.Drawing.Point(13, 441);
            this.lblDoubleClickToDelete.Name = "lblDoubleClickToDelete";
            this.lblDoubleClickToDelete.Size = new System.Drawing.Size(480, 21);
            this.lblDoubleClickToDelete.TabIndex = 51;
            this.lblDoubleClickToDelete.Text = "*Use double click to delete filter from list.";
            // 
            // FilterAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 461);
            this.ControlBox = false;
            this.Controls.Add(this.lblDoubleClickToDelete);
            this.Controls.Add(this.btnAllFilters);
            this.Controls.Add(this.gbFilters);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.cbDefault);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FilterAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FilterAdd";
            this.Load += new System.EventHandler(this.FilterAdd_Load);
            this.gbFilters.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbDefault;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ListView lvFilters;
        private System.Windows.Forms.GroupBox gbFilters;
        private System.Windows.Forms.Button btnAllFilters;
        private System.Windows.Forms.Label lblDoubleClickToDelete;
    }
}