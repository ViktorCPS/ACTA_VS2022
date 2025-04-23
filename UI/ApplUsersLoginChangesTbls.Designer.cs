namespace UI
{
    partial class ApplUsersLoginChangesTbls
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
            this.gbULChangesTbl = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lvULChangesTbl = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvAllTables = new System.Windows.Forms.ListView();
            this.gbULChangesTbl.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbULChangesTbl
            // 
            this.gbULChangesTbl.Controls.Add(this.btnRemove);
            this.gbULChangesTbl.Controls.Add(this.lvULChangesTbl);
            this.gbULChangesTbl.Controls.Add(this.btnClose);
            this.gbULChangesTbl.Controls.Add(this.btnSave);
            this.gbULChangesTbl.Controls.Add(this.btnAdd);
            this.gbULChangesTbl.Controls.Add(this.lvAllTables);
            this.gbULChangesTbl.Location = new System.Drawing.Point(12, 15);
            this.gbULChangesTbl.Name = "gbULChangesTbl";
            this.gbULChangesTbl.Size = new System.Drawing.Size(743, 440);
            this.gbULChangesTbl.TabIndex = 0;
            this.gbULChangesTbl.TabStop = false;
            this.gbULChangesTbl.Text = "User login changes table";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(348, 211);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(40, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvULChangesTbl
            // 
            this.lvULChangesTbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvULChangesTbl.FullRowSelect = true;
            this.lvULChangesTbl.GridLines = true;
            this.lvULChangesTbl.HideSelection = false;
            this.lvULChangesTbl.Location = new System.Drawing.Point(416, 32);
            this.lvULChangesTbl.Name = "lvULChangesTbl";
            this.lvULChangesTbl.ShowItemToolTips = true;
            this.lvULChangesTbl.Size = new System.Drawing.Size(302, 350);
            this.lvULChangesTbl.TabIndex = 2;
            this.lvULChangesTbl.UseCompatibleStateImageBehavior = false;
            this.lvULChangesTbl.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(643, 397);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(332, 397);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(348, 181);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(40, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvAllTables
            // 
            this.lvAllTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAllTables.FullRowSelect = true;
            this.lvAllTables.GridLines = true;
            this.lvAllTables.HideSelection = false;
            this.lvAllTables.Location = new System.Drawing.Point(18, 32);
            this.lvAllTables.Name = "lvAllTables";
            this.lvAllTables.ShowItemToolTips = true;
            this.lvAllTables.Size = new System.Drawing.Size(302, 350);
            this.lvAllTables.TabIndex = 0;
            this.lvAllTables.UseCompatibleStateImageBehavior = false;
            this.lvAllTables.View = System.Windows.Forms.View.Details;
            // 
            // ApplUsersLoginChangesTbls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 467);
            this.ControlBox = false;
            this.Controls.Add(this.gbULChangesTbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ApplUsersLoginChangesTbls";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ApplUsersLoginChangesTbls";
            this.gbULChangesTbl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbULChangesTbl;
        private System.Windows.Forms.ListView lvAllTables;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvULChangesTbl;
        private System.Windows.Forms.Button btnRemove;
    }
}