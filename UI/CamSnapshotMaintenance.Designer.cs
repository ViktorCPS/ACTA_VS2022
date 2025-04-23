namespace UI
{
    partial class CamSnapshotMaintenance
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpDump = new System.Windows.Forms.TabPage();
            this.btnStartDump = new System.Windows.Forms.Button();
            this.gbFound = new System.Windows.Forms.GroupBox();
            this.lblSizeOfFilesDump = new System.Windows.Forms.Label();
            this.lblNumOfFilesDump = new System.Windows.Forms.Label();
            this.lblNumberDump = new System.Windows.Forms.Label();
            this.lblSizeDump = new System.Windows.Forms.Label();
            this.gbSearchCriteria = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.gbProgressDump = new System.Windows.Forms.GroupBox();
            this.lblPBDumpStatus = new System.Windows.Forms.Label();
            this.pbDump = new System.Windows.Forms.ProgressBar();
            this.gbDestinationDump = new System.Windows.Forms.GroupBox();
            this.btnBrowseDump = new System.Windows.Forms.Button();
            this.tbDestinationDump = new System.Windows.Forms.TextBox();
            this.tpClearHistory = new System.Windows.Forms.TabPage();
            this.btnStart = new System.Windows.Forms.Button();
            this.gbProgress = new System.Windows.Forms.GroupBox();
            this.lblProgressStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.gbDestination = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbDestination = new System.Windows.Forms.TextBox();
            this.gbSelectEndDate = new System.Windows.Forms.GroupBox();
            this.cbSaveFiles = new System.Windows.Forms.CheckBox();
            this.lblTotalSize = new System.Windows.Forms.Label();
            this.lblNumberOfFiles = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.dtpUntilDate = new System.Windows.Forms.DateTimePicker();
            this.lblUntil = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPhotoPreview = new System.Windows.Forms.Button();
            this.btnSearch1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpDump.SuspendLayout();
            this.gbFound.SuspendLayout();
            this.gbSearchCriteria.SuspendLayout();
            this.gbProgressDump.SuspendLayout();
            this.gbDestinationDump.SuspendLayout();
            this.tpClearHistory.SuspendLayout();
            this.gbProgress.SuspendLayout();
            this.gbDestination.SuspendLayout();
            this.gbSelectEndDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpDump);
            this.tabControl1.Controls.Add(this.tpClearHistory);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(653, 428);
            this.tabControl1.TabIndex = 0;
            // 
            // tpDump
            // 
            this.tpDump.Controls.Add(this.btnStartDump);
            this.tpDump.Controls.Add(this.gbFound);
            this.tpDump.Controls.Add(this.gbSearchCriteria);
            this.tpDump.Controls.Add(this.gbProgressDump);
            this.tpDump.Controls.Add(this.gbDestinationDump);
            this.tpDump.Location = new System.Drawing.Point(4, 22);
            this.tpDump.Name = "tpDump";
            this.tpDump.Padding = new System.Windows.Forms.Padding(3);
            this.tpDump.Size = new System.Drawing.Size(645, 402);
            this.tpDump.TabIndex = 0;
            this.tpDump.Text = "Dump";
            this.tpDump.UseVisualStyleBackColor = true;
            // 
            // btnStartDump
            // 
            this.btnStartDump.Location = new System.Drawing.Point(283, 369);
            this.btnStartDump.Name = "btnStartDump";
            this.btnStartDump.Size = new System.Drawing.Size(75, 23);
            this.btnStartDump.TabIndex = 15;
            this.btnStartDump.Text = "Start";
            this.btnStartDump.UseVisualStyleBackColor = true;
            this.btnStartDump.Click += new System.EventHandler(this.btnStartDump_Click);
            // 
            // gbFound
            // 
            this.gbFound.Controls.Add(this.lblSizeOfFilesDump);
            this.gbFound.Controls.Add(this.lblNumOfFilesDump);
            this.gbFound.Controls.Add(this.lblNumberDump);
            this.gbFound.Controls.Add(this.lblSizeDump);
            this.gbFound.Location = new System.Drawing.Point(107, 130);
            this.gbFound.Name = "gbFound";
            this.gbFound.Size = new System.Drawing.Size(430, 71);
            this.gbFound.TabIndex = 14;
            this.gbFound.TabStop = false;
            this.gbFound.Text = "Found";
            // 
            // lblSizeOfFilesDump
            // 
            this.lblSizeOfFilesDump.AutoSize = true;
            this.lblSizeOfFilesDump.Location = new System.Drawing.Point(145, 46);
            this.lblSizeOfFilesDump.MinimumSize = new System.Drawing.Size(10, 0);
            this.lblSizeOfFilesDump.Name = "lblSizeOfFilesDump";
            this.lblSizeOfFilesDump.Size = new System.Drawing.Size(10, 13);
            this.lblSizeOfFilesDump.TabIndex = 5;
            // 
            // lblNumOfFilesDump
            // 
            this.lblNumOfFilesDump.AutoSize = true;
            this.lblNumOfFilesDump.Location = new System.Drawing.Point(145, 16);
            this.lblNumOfFilesDump.MinimumSize = new System.Drawing.Size(10, 0);
            this.lblNumOfFilesDump.Name = "lblNumOfFilesDump";
            this.lblNumOfFilesDump.Size = new System.Drawing.Size(10, 13);
            this.lblNumOfFilesDump.TabIndex = 4;
            // 
            // lblNumberDump
            // 
            this.lblNumberDump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNumberDump.Location = new System.Drawing.Point(38, 16);
            this.lblNumberDump.Name = "lblNumberDump";
            this.lblNumberDump.Size = new System.Drawing.Size(81, 13);
            this.lblNumberDump.TabIndex = 2;
            this.lblNumberDump.Text = "Number:";
            this.lblNumberDump.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSizeDump
            // 
            this.lblSizeDump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSizeDump.Location = new System.Drawing.Point(41, 46);
            this.lblSizeDump.Name = "lblSizeDump";
            this.lblSizeDump.Size = new System.Drawing.Size(78, 13);
            this.lblSizeDump.TabIndex = 3;
            this.lblSizeDump.Text = "Size:";
            this.lblSizeDump.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbSearchCriteria
            // 
            this.gbSearchCriteria.Controls.Add(this.btnSearch);
            this.gbSearchCriteria.Controls.Add(this.dtpDateTo);
            this.gbSearchCriteria.Controls.Add(this.lblTo);
            this.gbSearchCriteria.Controls.Add(this.dtpDateFrom);
            this.gbSearchCriteria.Controls.Add(this.lblFrom);
            this.gbSearchCriteria.Location = new System.Drawing.Point(107, 6);
            this.gbSearchCriteria.Name = "gbSearchCriteria";
            this.gbSearchCriteria.Size = new System.Drawing.Size(430, 118);
            this.gbSearchCriteria.TabIndex = 13;
            this.gbSearchCriteria.TabStop = false;
            this.gbSearchCriteria.Text = "Search criteria";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(349, 86);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.CustomFormat = "dd.MM.yyyy";
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(148, 60);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(200, 20);
            this.dtpDateTo.TabIndex = 7;
            // 
            // lblTo
            // 
            this.lblTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTo.Location = new System.Drawing.Point(35, 64);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(84, 16);
            this.lblTo.TabIndex = 6;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(148, 26);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpDateFrom.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFrom.Location = new System.Drawing.Point(35, 30);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(84, 16);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbProgressDump
            // 
            this.gbProgressDump.Controls.Add(this.lblPBDumpStatus);
            this.gbProgressDump.Controls.Add(this.pbDump);
            this.gbProgressDump.Location = new System.Drawing.Point(107, 297);
            this.gbProgressDump.Name = "gbProgressDump";
            this.gbProgressDump.Size = new System.Drawing.Size(430, 66);
            this.gbProgressDump.TabIndex = 12;
            this.gbProgressDump.TabStop = false;
            this.gbProgressDump.Text = "Progress";
            // 
            // lblPBDumpStatus
            // 
            this.lblPBDumpStatus.AutoSize = true;
            this.lblPBDumpStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPBDumpStatus.Location = new System.Drawing.Point(145, 0);
            this.lblPBDumpStatus.MaximumSize = new System.Drawing.Size(140, 10);
            this.lblPBDumpStatus.MinimumSize = new System.Drawing.Size(140, 10);
            this.lblPBDumpStatus.Name = "lblPBDumpStatus";
            this.lblPBDumpStatus.Size = new System.Drawing.Size(140, 10);
            this.lblPBDumpStatus.TabIndex = 11;
            this.lblPBDumpStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbDump
            // 
            this.pbDump.Location = new System.Drawing.Point(15, 28);
            this.pbDump.Name = "pbDump";
            this.pbDump.Size = new System.Drawing.Size(399, 23);
            this.pbDump.Step = 1;
            this.pbDump.TabIndex = 9;
            // 
            // gbDestinationDump
            // 
            this.gbDestinationDump.Controls.Add(this.btnBrowseDump);
            this.gbDestinationDump.Controls.Add(this.tbDestinationDump);
            this.gbDestinationDump.Location = new System.Drawing.Point(107, 207);
            this.gbDestinationDump.Name = "gbDestinationDump";
            this.gbDestinationDump.Size = new System.Drawing.Size(430, 84);
            this.gbDestinationDump.TabIndex = 10;
            this.gbDestinationDump.TabStop = false;
            this.gbDestinationDump.Text = "Destination";
            // 
            // btnBrowseDump
            // 
            this.btnBrowseDump.Location = new System.Drawing.Point(349, 55);
            this.btnBrowseDump.Name = "btnBrowseDump";
            this.btnBrowseDump.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDump.TabIndex = 1;
            this.btnBrowseDump.Text = "Browse";
            this.btnBrowseDump.UseVisualStyleBackColor = true;
            this.btnBrowseDump.Click += new System.EventHandler(this.btnBrowseDump_Click);
            // 
            // tbDestinationDump
            // 
            this.tbDestinationDump.Location = new System.Drawing.Point(15, 19);
            this.tbDestinationDump.Name = "tbDestinationDump";
            this.tbDestinationDump.Size = new System.Drawing.Size(399, 20);
            this.tbDestinationDump.TabIndex = 0;
            // 
            // tpClearHistory
            // 
            this.tpClearHistory.Controls.Add(this.btnStart);
            this.tpClearHistory.Controls.Add(this.gbProgress);
            this.tpClearHistory.Controls.Add(this.gbDestination);
            this.tpClearHistory.Controls.Add(this.gbSelectEndDate);
            this.tpClearHistory.Location = new System.Drawing.Point(4, 22);
            this.tpClearHistory.Name = "tpClearHistory";
            this.tpClearHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tpClearHistory.Size = new System.Drawing.Size(645, 402);
            this.tpClearHistory.TabIndex = 1;
            this.tpClearHistory.Text = "Clear history";
            this.tpClearHistory.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(280, 373);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbProgress
            // 
            this.gbProgress.Controls.Add(this.lblProgressStatus);
            this.gbProgress.Controls.Add(this.progressBar);
            this.gbProgress.Location = new System.Drawing.Point(115, 301);
            this.gbProgress.Name = "gbProgress";
            this.gbProgress.Size = new System.Drawing.Size(430, 66);
            this.gbProgress.TabIndex = 11;
            this.gbProgress.TabStop = false;
            this.gbProgress.Text = "Progress";
            // 
            // lblProgressStatus
            // 
            this.lblProgressStatus.AutoSize = true;
            this.lblProgressStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressStatus.Location = new System.Drawing.Point(145, 0);
            this.lblProgressStatus.MaximumSize = new System.Drawing.Size(140, 10);
            this.lblProgressStatus.MinimumSize = new System.Drawing.Size(140, 10);
            this.lblProgressStatus.Name = "lblProgressStatus";
            this.lblProgressStatus.Size = new System.Drawing.Size(140, 10);
            this.lblProgressStatus.TabIndex = 11;
            this.lblProgressStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 28);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(399, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 9;
            // 
            // gbDestination
            // 
            this.gbDestination.Controls.Add(this.btnBrowse);
            this.gbDestination.Controls.Add(this.tbDestination);
            this.gbDestination.Location = new System.Drawing.Point(115, 194);
            this.gbDestination.Name = "gbDestination";
            this.gbDestination.Size = new System.Drawing.Size(430, 84);
            this.gbDestination.TabIndex = 9;
            this.gbDestination.TabStop = false;
            this.gbDestination.Text = "Destination";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(339, 55);
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
            this.tbDestination.Size = new System.Drawing.Size(399, 20);
            this.tbDestination.TabIndex = 0;
            // 
            // gbSelectEndDate
            // 
            this.gbSelectEndDate.Controls.Add(this.btnSearch1);
            this.gbSelectEndDate.Controls.Add(this.cbSaveFiles);
            this.gbSelectEndDate.Controls.Add(this.lblTotalSize);
            this.gbSelectEndDate.Controls.Add(this.lblNumberOfFiles);
            this.gbSelectEndDate.Controls.Add(this.lblSize);
            this.gbSelectEndDate.Controls.Add(this.lblNumber);
            this.gbSelectEndDate.Controls.Add(this.dtpUntilDate);
            this.gbSelectEndDate.Controls.Add(this.lblUntil);
            this.gbSelectEndDate.Location = new System.Drawing.Point(115, 22);
            this.gbSelectEndDate.Name = "gbSelectEndDate";
            this.gbSelectEndDate.Size = new System.Drawing.Size(430, 166);
            this.gbSelectEndDate.TabIndex = 0;
            this.gbSelectEndDate.TabStop = false;
            this.gbSelectEndDate.Text = "Select the end date for deleting files";
            // 
            // cbSaveFiles
            // 
            this.cbSaveFiles.AutoSize = true;
            this.cbSaveFiles.Checked = true;
            this.cbSaveFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSaveFiles.Location = new System.Drawing.Point(148, 143);
            this.cbSaveFiles.Name = "cbSaveFiles";
            this.cbSaveFiles.Size = new System.Drawing.Size(72, 17);
            this.cbSaveFiles.TabIndex = 6;
            this.cbSaveFiles.Text = "Save files";
            this.cbSaveFiles.UseVisualStyleBackColor = true;
            this.cbSaveFiles.CheckedChanged += new System.EventHandler(this.cbSaveFiles_CheckedChanged);
            // 
            // lblTotalSize
            // 
            this.lblTotalSize.AutoSize = true;
            this.lblTotalSize.Location = new System.Drawing.Point(145, 93);
            this.lblTotalSize.MinimumSize = new System.Drawing.Size(10, 10);
            this.lblTotalSize.Name = "lblTotalSize";
            this.lblTotalSize.Size = new System.Drawing.Size(10, 13);
            this.lblTotalSize.TabIndex = 5;
            // 
            // lblNumberOfFiles
            // 
            this.lblNumberOfFiles.AutoSize = true;
            this.lblNumberOfFiles.Location = new System.Drawing.Point(145, 68);
            this.lblNumberOfFiles.MinimumSize = new System.Drawing.Size(10, 10);
            this.lblNumberOfFiles.Name = "lblNumberOfFiles";
            this.lblNumberOfFiles.Size = new System.Drawing.Size(10, 13);
            this.lblNumberOfFiles.TabIndex = 4;
            // 
            // lblSize
            // 
            this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSize.Location = new System.Drawing.Point(41, 93);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(78, 13);
            this.lblSize.TabIndex = 3;
            this.lblSize.Text = "Size:";
            this.lblSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNumber
            // 
            this.lblNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNumber.Location = new System.Drawing.Point(38, 68);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(81, 13);
            this.lblNumber.TabIndex = 2;
            this.lblNumber.Text = "Number:";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtpUntilDate
            // 
            this.dtpUntilDate.CustomFormat = "dd.MM.yyyy";
            this.dtpUntilDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUntilDate.Location = new System.Drawing.Point(148, 26);
            this.dtpUntilDate.Name = "dtpUntilDate";
            this.dtpUntilDate.Size = new System.Drawing.Size(200, 20);
            this.dtpUntilDate.TabIndex = 1;
            this.dtpUntilDate.ValueChanged += new System.EventHandler(this.dtpUntilDate_ValueChanged);
            // 
            // lblUntil
            // 
            this.lblUntil.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUntil.Location = new System.Drawing.Point(35, 30);
            this.lblUntil.Name = "lblUntil";
            this.lblUntil.Size = new System.Drawing.Size(84, 16);
            this.lblUntil.TabIndex = 0;
            this.lblUntil.Text = "Until:";
            this.lblUntil.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(590, 446);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPhotoPreview
            // 
            this.btnPhotoPreview.Location = new System.Drawing.Point(12, 446);
            this.btnPhotoPreview.Name = "btnPhotoPreview";
            this.btnPhotoPreview.Size = new System.Drawing.Size(99, 23);
            this.btnPhotoPreview.TabIndex = 3;
            this.btnPhotoPreview.Text = "Preview photos";
            this.btnPhotoPreview.UseVisualStyleBackColor = true;
            this.btnPhotoPreview.Click += new System.EventHandler(this.btnPhotoPreview_Click);
            // 
            // btnSearch1
            // 
            this.btnSearch1.Location = new System.Drawing.Point(339, 125);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(75, 23);
            this.btnSearch1.TabIndex = 9;
            this.btnSearch1.Text = "Search";
            this.btnSearch1.UseVisualStyleBackColor = true;
            this.btnSearch1.Click += new System.EventHandler(this.btnSearch1_Click);
            // 
            // CamSnapshotMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 480);
            this.ControlBox = false;
            this.Controls.Add(this.btnPhotoPreview);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(685, 514);
            this.MinimumSize = new System.Drawing.Size(685, 514);
            this.Name = "CamSnapshotMaintenance";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CameraSnapshotMaintenance";
            this.Load += new System.EventHandler(this.CamSnapshotMaintenance_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CamSnapshotMaintenance_KeyUp);
            this.tabControl1.ResumeLayout(false);
            this.tpDump.ResumeLayout(false);
            this.gbFound.ResumeLayout(false);
            this.gbFound.PerformLayout();
            this.gbSearchCriteria.ResumeLayout(false);
            this.gbProgressDump.ResumeLayout(false);
            this.gbProgressDump.PerformLayout();
            this.gbDestinationDump.ResumeLayout(false);
            this.gbDestinationDump.PerformLayout();
            this.tpClearHistory.ResumeLayout(false);
            this.gbProgress.ResumeLayout(false);
            this.gbProgress.PerformLayout();
            this.gbDestination.ResumeLayout(false);
            this.gbDestination.PerformLayout();
            this.gbSelectEndDate.ResumeLayout(false);
            this.gbSelectEndDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpDump;
        private System.Windows.Forms.TabPage tpClearHistory;
        private System.Windows.Forms.GroupBox gbSelectEndDate;
        private System.Windows.Forms.DateTimePicker dtpUntilDate;
        private System.Windows.Forms.Label lblUntil;
        private System.Windows.Forms.Label lblTotalSize;
        private System.Windows.Forms.Label lblNumberOfFiles;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.CheckBox cbSaveFiles;
        private System.Windows.Forms.GroupBox gbDestination;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbDestination;
        private System.Windows.Forms.GroupBox gbProgress;
        private System.Windows.Forms.Label lblProgressStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox gbFound;
        private System.Windows.Forms.GroupBox gbSearchCriteria;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblSizeDump;
        private System.Windows.Forms.Label lblNumberDump;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.GroupBox gbProgressDump;
        private System.Windows.Forms.Label lblPBDumpStatus;
        private System.Windows.Forms.ProgressBar pbDump;
        private System.Windows.Forms.GroupBox gbDestinationDump;
        private System.Windows.Forms.Button btnBrowseDump;
        private System.Windows.Forms.TextBox tbDestinationDump;
        private System.Windows.Forms.Button btnStartDump;
        private System.Windows.Forms.Label lblSizeOfFilesDump;
        private System.Windows.Forms.Label lblNumOfFilesDump;
        private System.Windows.Forms.Button btnPhotoPreview;
        private System.Windows.Forms.Button btnSearch1;
    }
}