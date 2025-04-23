namespace UI
{
    partial class GroupChangedMassiveInput
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
            this.lblFilePath = new System.Windows.Forms.Label();
            this.tbFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.gbFileContent = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lvFileContent = new System.Windows.Forms.ListView();
            this.gbGroups = new System.Windows.Forms.GroupBox();
            this.lvGroups = new System.Windows.Forms.ListView();
            this.btnGenerateFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbFileContent.SuspendLayout();
            this.gbGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFilePath
            // 
            this.lblFilePath.Location = new System.Drawing.Point(23, 15);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(63, 23);
            this.lblFilePath.TabIndex = 0;
            this.lblFilePath.Text = "File path:";
            this.lblFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFilePath
            // 
            this.tbFilePath.Enabled = false;
            this.tbFilePath.Location = new System.Drawing.Point(92, 17);
            this.tbFilePath.Name = "tbFilePath";
            this.tbFilePath.Size = new System.Drawing.Size(524, 20);
            this.tbFilePath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(634, 15);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // gbFileContent
            // 
            this.gbFileContent.Controls.Add(this.btnSave);
            this.gbFileContent.Controls.Add(this.lvFileContent);
            this.gbFileContent.Location = new System.Drawing.Point(12, 53);
            this.gbFileContent.Name = "gbFileContent";
            this.gbFileContent.Size = new System.Drawing.Size(396, 388);
            this.gbFileContent.TabIndex = 3;
            this.gbFileContent.TabStop = false;
            this.gbFileContent.Text = "File content";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(14, 345);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lvFileContent
            // 
            this.lvFileContent.FullRowSelect = true;
            this.lvFileContent.GridLines = true;
            this.lvFileContent.Location = new System.Drawing.Point(14, 28);
            this.lvFileContent.Name = "lvFileContent";
            this.lvFileContent.Size = new System.Drawing.Size(362, 301);
            this.lvFileContent.TabIndex = 4;
            this.lvFileContent.UseCompatibleStateImageBehavior = false;
            this.lvFileContent.View = System.Windows.Forms.View.Details;
            // 
            // gbGroups
            // 
            this.gbGroups.Controls.Add(this.lvGroups);
            this.gbGroups.Location = new System.Drawing.Point(425, 53);
            this.gbGroups.Name = "gbGroups";
            this.gbGroups.Size = new System.Drawing.Size(284, 388);
            this.gbGroups.TabIndex = 4;
            this.gbGroups.TabStop = false;
            this.gbGroups.Text = "Groups";
            // 
            // lvGroups
            // 
            this.lvGroups.FullRowSelect = true;
            this.lvGroups.GridLines = true;
            this.lvGroups.Location = new System.Drawing.Point(18, 28);
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.Size = new System.Drawing.Size(248, 301);
            this.lvGroups.TabIndex = 5;
            this.lvGroups.UseCompatibleStateImageBehavior = false;
            this.lvGroups.View = System.Windows.Forms.View.Details;
            // 
            // btnGenerateFile
            // 
            this.btnGenerateFile.Location = new System.Drawing.Point(12, 462);
            this.btnGenerateFile.Name = "btnGenerateFile";
            this.btnGenerateFile.Size = new System.Drawing.Size(179, 23);
            this.btnGenerateFile.TabIndex = 5;
            this.btnGenerateFile.Text = "Generate empty file";
            this.btnGenerateFile.UseVisualStyleBackColor = true;
            this.btnGenerateFile.Click += new System.EventHandler(this.btnGenerateFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(634, 462);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // GroupChangedMassiveInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 518);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerateFile);
            this.Controls.Add(this.gbGroups);
            this.Controls.Add(this.gbFileContent);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbFilePath);
            this.Controls.Add(this.lblFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GroupChangedMassiveInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Massive Input";
            this.Load += new System.EventHandler(this.GroupChangedMassiveInput_Load);
            this.gbFileContent.ResumeLayout(false);
            this.gbGroups.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox gbFileContent;
        private System.Windows.Forms.GroupBox gbGroups;
        private System.Windows.Forms.Button btnGenerateFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListView lvFileContent;
        private System.Windows.Forms.ListView lvGroups;
    }
}