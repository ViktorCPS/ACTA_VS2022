namespace UI
{
    partial class EmployeeDocMaintenance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeDocMaintenance));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpDump = new System.Windows.Forms.TabPage();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.gbProgress = new System.Windows.Forms.GroupBox();
            this.lblProgressStatus = new System.Windows.Forms.Label();
            this.progressBarDump = new System.Windows.Forms.ProgressBar();
            this.gbDestination = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbDestination = new System.Windows.Forms.TextBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.tpLoad = new System.Windows.Forms.TabPage();
            this.btnStartLoad = new System.Windows.Forms.Button();
            this.gbProgressLoad = new System.Windows.Forms.GroupBox();
            this.lblProgressLoadStatus = new System.Windows.Forms.Label();
            this.progressBarLoad = new System.Windows.Forms.ProgressBar();
            this.gbSource = new System.Windows.Forms.GroupBox();
            this.btnBrowseLoad = new System.Windows.Forms.Button();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.lblEmplPhotosNote = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpDump.SuspendLayout();
            this.gbProgress.SuspendLayout();
            this.gbDestination.SuspendLayout();
            this.tpLoad.SuspendLayout();
            this.gbProgressLoad.SuspendLayout();
            this.gbSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpDump);
            this.tabControl1.Controls.Add(this.tpLoad);
            this.tabControl1.Location = new System.Drawing.Point(12, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(516, 373);
            this.tabControl1.TabIndex = 0;
            // 
            // tpDump
            // 
            this.tpDump.Controls.Add(this.btnWUTree);
            this.tpDump.Controls.Add(this.btnStart);
            this.tpDump.Controls.Add(this.gbProgress);
            this.tpDump.Controls.Add(this.gbDestination);
            this.tpDump.Controls.Add(this.lblEmployee);
            this.tpDump.Controls.Add(this.cbEmployee);
            this.tpDump.Controls.Add(this.lblWU);
            this.tpDump.Controls.Add(this.cbWU);
            this.tpDump.Location = new System.Drawing.Point(4, 22);
            this.tpDump.Name = "tpDump";
            this.tpDump.Padding = new System.Windows.Forms.Padding(3);
            this.tpDump.Size = new System.Drawing.Size(508, 347);
            this.tpDump.TabIndex = 0;
            this.tpDump.Text = "Dump";
            this.tpDump.UseVisualStyleBackColor = true;
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(406, 38);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 41;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(222, 318);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbProgress
            // 
            this.gbProgress.Controls.Add(this.lblProgressStatus);
            this.gbProgress.Controls.Add(this.progressBarDump);
            this.gbProgress.Location = new System.Drawing.Point(55, 230);
            this.gbProgress.Name = "gbProgress";
            this.gbProgress.Size = new System.Drawing.Size(398, 66);
            this.gbProgress.TabIndex = 10;
            this.gbProgress.TabStop = false;
            this.gbProgress.Text = "Progress";
            this.gbProgress.Enter += new System.EventHandler(this.gbProgress_Enter);
            // 
            // lblProgressStatus
            // 
            this.lblProgressStatus.AutoSize = true;
            this.lblProgressStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressStatus.Location = new System.Drawing.Point(126, 0);
            this.lblProgressStatus.MaximumSize = new System.Drawing.Size(140, 10);
            this.lblProgressStatus.MinimumSize = new System.Drawing.Size(140, 10);
            this.lblProgressStatus.Name = "lblProgressStatus";
            this.lblProgressStatus.Size = new System.Drawing.Size(140, 10);
            this.lblProgressStatus.TabIndex = 11;
            this.lblProgressStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarDump
            // 
            this.progressBarDump.Location = new System.Drawing.Point(15, 28);
            this.progressBarDump.Name = "progressBarDump";
            this.progressBarDump.Size = new System.Drawing.Size(368, 23);
            this.progressBarDump.Step = 1;
            this.progressBarDump.TabIndex = 9;
            // 
            // gbDestination
            // 
            this.gbDestination.Controls.Add(this.btnBrowse);
            this.gbDestination.Controls.Add(this.tbDestination);
            this.gbDestination.Location = new System.Drawing.Point(55, 124);
            this.gbDestination.Name = "gbDestination";
            this.gbDestination.Size = new System.Drawing.Size(398, 84);
            this.gbDestination.TabIndex = 8;
            this.gbDestination.TabStop = false;
            this.gbDestination.Text = "Destination";
            this.gbDestination.Enter += new System.EventHandler(this.gbDestination_Enter);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(308, 55);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbDestination
            // 
            this.tbDestination.Location = new System.Drawing.Point(15, 19);
            this.tbDestination.Name = "tbDestination";
            this.tbDestination.Size = new System.Drawing.Size(368, 20);
            this.tbDestination.TabIndex = 0;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(61, 63);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 7;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEmployee.Click += new System.EventHandler(this.lblEmployee_Click);
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(172, 65);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(228, 21);
            this.cbEmployee.TabIndex = 6;
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(29, 38);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(104, 23);
            this.lblWU.TabIndex = 5;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblWU.Click += new System.EventHandler(this.lblWU_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(172, 40);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(228, 21);
            this.cbWU.TabIndex = 4;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // tpLoad
            // 
            this.tpLoad.Controls.Add(this.btnStartLoad);
            this.tpLoad.Controls.Add(this.gbProgressLoad);
            this.tpLoad.Controls.Add(this.gbSource);
            this.tpLoad.Controls.Add(this.lblEmplPhotosNote);
            this.tpLoad.Location = new System.Drawing.Point(4, 22);
            this.tpLoad.Name = "tpLoad";
            this.tpLoad.Padding = new System.Windows.Forms.Padding(3);
            this.tpLoad.Size = new System.Drawing.Size(508, 347);
            this.tpLoad.TabIndex = 1;
            this.tpLoad.Text = "Load";
            this.tpLoad.UseVisualStyleBackColor = true;
            // 
            // btnStartLoad
            // 
            this.btnStartLoad.Location = new System.Drawing.Point(222, 318);
            this.btnStartLoad.Name = "btnStartLoad";
            this.btnStartLoad.Size = new System.Drawing.Size(75, 23);
            this.btnStartLoad.TabIndex = 12;
            this.btnStartLoad.Text = "Start";
            this.btnStartLoad.UseVisualStyleBackColor = true;
            this.btnStartLoad.Click += new System.EventHandler(this.btnStartLoad_Click);
            // 
            // gbProgressLoad
            // 
            this.gbProgressLoad.Controls.Add(this.lblProgressLoadStatus);
            this.gbProgressLoad.Controls.Add(this.progressBarLoad);
            this.gbProgressLoad.Location = new System.Drawing.Point(55, 230);
            this.gbProgressLoad.Name = "gbProgressLoad";
            this.gbProgressLoad.Size = new System.Drawing.Size(398, 66);
            this.gbProgressLoad.TabIndex = 11;
            this.gbProgressLoad.TabStop = false;
            this.gbProgressLoad.Text = "Progress";
            // 
            // lblProgressLoadStatus
            // 
            this.lblProgressLoadStatus.AutoSize = true;
            this.lblProgressLoadStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressLoadStatus.Location = new System.Drawing.Point(128, 0);
            this.lblProgressLoadStatus.MaximumSize = new System.Drawing.Size(140, 10);
            this.lblProgressLoadStatus.MinimumSize = new System.Drawing.Size(140, 10);
            this.lblProgressLoadStatus.Name = "lblProgressLoadStatus";
            this.lblProgressLoadStatus.Size = new System.Drawing.Size(140, 10);
            this.lblProgressLoadStatus.TabIndex = 11;
            this.lblProgressLoadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarLoad
            // 
            this.progressBarLoad.Location = new System.Drawing.Point(15, 28);
            this.progressBarLoad.Name = "progressBarLoad";
            this.progressBarLoad.Size = new System.Drawing.Size(368, 23);
            this.progressBarLoad.Step = 1;
            this.progressBarLoad.TabIndex = 9;
            // 
            // gbSource
            // 
            this.gbSource.Controls.Add(this.btnBrowseLoad);
            this.gbSource.Controls.Add(this.tbSource);
            this.gbSource.Location = new System.Drawing.Point(55, 124);
            this.gbSource.Name = "gbSource";
            this.gbSource.Size = new System.Drawing.Size(398, 84);
            this.gbSource.TabIndex = 9;
            this.gbSource.TabStop = false;
            this.gbSource.Text = "Source";
            // 
            // btnBrowseLoad
            // 
            this.btnBrowseLoad.Location = new System.Drawing.Point(308, 55);
            this.btnBrowseLoad.Name = "btnBrowseLoad";
            this.btnBrowseLoad.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseLoad.TabIndex = 1;
            this.btnBrowseLoad.Text = "Browse";
            this.btnBrowseLoad.UseVisualStyleBackColor = true;
            this.btnBrowseLoad.Click += new System.EventHandler(this.btnBrowseLoad_Click);
            // 
            // tbSource
            // 
            this.tbSource.Location = new System.Drawing.Point(15, 19);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(368, 20);
            this.tbSource.TabIndex = 0;
            // 
            // lblEmplPhotosNote
            // 
            this.lblEmplPhotosNote.Location = new System.Drawing.Point(48, 26);
            this.lblEmplPhotosNote.Name = "lblEmplPhotosNote";
            this.lblEmplPhotosNote.Size = new System.Drawing.Size(398, 41);
            this.lblEmplPhotosNote.TabIndex = 0;
            this.lblEmplPhotosNote.Text = "Employee photos note";
            this.lblEmplPhotosNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(453, 409);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // EmployeeDocMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 436);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(551, 475);
            this.MinimumSize = new System.Drawing.Size(551, 475);
            this.Name = "EmployeeDocMaintenance";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Document manipulation";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmployeeDocMaintenance_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tpDump.ResumeLayout(false);
            this.gbProgress.ResumeLayout(false);
            this.gbProgress.PerformLayout();
            this.gbDestination.ResumeLayout(false);
            this.gbDestination.PerformLayout();
            this.tpLoad.ResumeLayout(false);
            this.gbProgressLoad.ResumeLayout(false);
            this.gbProgressLoad.PerformLayout();
            this.gbSource.ResumeLayout(false);
            this.gbSource.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpDump;
        private System.Windows.Forms.TabPage tpLoad;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.GroupBox gbDestination;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbDestination;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ProgressBar progressBarDump;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox gbProgress;
        private System.Windows.Forms.Label lblProgressStatus;
        private System.Windows.Forms.GroupBox gbSource;
        private System.Windows.Forms.Button btnBrowseLoad;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.Label lblEmplPhotosNote;
        private System.Windows.Forms.Button btnStartLoad;
        private System.Windows.Forms.GroupBox gbProgressLoad;
        private System.Windows.Forms.Label lblProgressLoadStatus;
        private System.Windows.Forms.ProgressBar progressBarLoad;
        private System.Windows.Forms.Button btnWUTree;
    }
}