namespace UI
{
    partial class Lockings
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
            this.tcLocking = new System.Windows.Forms.TabControl();
            this.tpPreview = new System.Windows.Forms.TabPage();
            this.gbPreviewAction = new System.Windows.Forms.GroupBox();
            this.gbComment = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.gbCurentStatus = new System.Windows.Forms.GroupBox();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.tpLockingUnlocking = new System.Windows.Forms.TabPage();
            this.gbComment1 = new System.Windows.Forms.GroupBox();
            this.tbComment = new System.Windows.Forms.RichTextBox();
            this.lblLastReaderTime = new System.Windows.Forms.Label();
            this.gbTerminals = new System.Windows.Forms.GroupBox();
            this.lvReaders = new System.Windows.Forms.ListView();
            this.btnLockUnlock = new System.Windows.Forms.Button();
            this.gbActionDef = new System.Windows.Forms.GroupBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblActionDef = new System.Windows.Forms.Label();
            this.gbActionType = new System.Windows.Forms.GroupBox();
            this.rbUnlocking = new System.Windows.Forms.RadioButton();
            this.rbLocking = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.tcLocking.SuspendLayout();
            this.tpPreview.SuspendLayout();
            this.gbPreviewAction.SuspendLayout();
            this.gbComment.SuspendLayout();
            this.gbCurentStatus.SuspendLayout();
            this.tpLockingUnlocking.SuspendLayout();
            this.gbComment1.SuspendLayout();
            this.gbTerminals.SuspendLayout();
            this.gbActionDef.SuspendLayout();
            this.gbActionType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcLocking
            // 
            this.tcLocking.Controls.Add(this.tpPreview);
            this.tcLocking.Controls.Add(this.tpLockingUnlocking);
            this.tcLocking.Location = new System.Drawing.Point(12, 12);
            this.tcLocking.Name = "tcLocking";
            this.tcLocking.SelectedIndex = 0;
            this.tcLocking.Size = new System.Drawing.Size(689, 490);
            this.tcLocking.TabIndex = 0;
            // 
            // tpPreview
            // 
            this.tpPreview.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tpPreview.Controls.Add(this.gbPreviewAction);
            this.tpPreview.Controls.Add(this.gbCurentStatus);
            this.tpPreview.Location = new System.Drawing.Point(4, 22);
            this.tpPreview.Name = "tpPreview";
            this.tpPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tpPreview.Size = new System.Drawing.Size(681, 464);
            this.tpPreview.TabIndex = 0;
            this.tpPreview.Text = "Preview";
            // 
            // gbPreviewAction
            // 
            this.gbPreviewAction.Controls.Add(this.gbComment);
            this.gbPreviewAction.Controls.Add(this.lvResults);
            this.gbPreviewAction.Location = new System.Drawing.Point(19, 107);
            this.gbPreviewAction.Name = "gbPreviewAction";
            this.gbPreviewAction.Size = new System.Drawing.Size(642, 338);
            this.gbPreviewAction.TabIndex = 1;
            this.gbPreviewAction.TabStop = false;
            this.gbPreviewAction.Text = "Action preview";
            // 
            // gbComment
            // 
            this.gbComment.Controls.Add(this.richTextBox1);
            this.gbComment.Location = new System.Drawing.Point(23, 244);
            this.gbComment.Name = "gbComment";
            this.gbComment.Size = new System.Drawing.Size(597, 71);
            this.gbComment.TabIndex = 21;
            this.gbComment.TabStop = false;
            this.gbComment.Text = "Comment";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(20, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(557, 37);
            this.richTextBox1.TabIndex = 0;
            // 
            // lvResults
            // 
            this.lvResults.FullRowSelect = true;
            this.lvResults.GridLines = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(23, 28);
            this.lvResults.MultiSelect = false;
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(597, 199);
            this.lvResults.TabIndex = 20;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.lvResults_SelectedIndexChanged);
            this.lvResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResults_ColumnClick);
            // 
            // gbCurentStatus
            // 
            this.gbCurentStatus.Controls.Add(this.dtDate);
            this.gbCurentStatus.Controls.Add(this.lblEndDate);
            this.gbCurentStatus.Location = new System.Drawing.Point(19, 15);
            this.gbCurentStatus.Name = "gbCurentStatus";
            this.gbCurentStatus.Size = new System.Drawing.Size(642, 73);
            this.gbCurentStatus.TabIndex = 0;
            this.gbCurentStatus.TabStop = false;
            this.gbCurentStatus.Text = "Curent status";
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "dd.MM.yyyy";
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(238, 28);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(167, 20);
            this.dtDate.TabIndex = 8;
            this.dtDate.Value = new System.DateTime(2009, 7, 16, 0, 0, 0, 0);
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(6, 21);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(226, 34);
            this.lblEndDate.TabIndex = 0;
            this.lblEndDate.Text = "Possible changes to data including: ";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tpLockingUnlocking
            // 
            this.tpLockingUnlocking.BackColor = System.Drawing.SystemColors.Control;
            this.tpLockingUnlocking.Controls.Add(this.gbComment1);
            this.tpLockingUnlocking.Controls.Add(this.lblLastReaderTime);
            this.tpLockingUnlocking.Controls.Add(this.gbTerminals);
            this.tpLockingUnlocking.Controls.Add(this.btnLockUnlock);
            this.tpLockingUnlocking.Controls.Add(this.gbActionDef);
            this.tpLockingUnlocking.Controls.Add(this.gbActionType);
            this.tpLockingUnlocking.Location = new System.Drawing.Point(4, 22);
            this.tpLockingUnlocking.Name = "tpLockingUnlocking";
            this.tpLockingUnlocking.Padding = new System.Windows.Forms.Padding(3);
            this.tpLockingUnlocking.Size = new System.Drawing.Size(681, 464);
            this.tpLockingUnlocking.TabIndex = 1;
            this.tpLockingUnlocking.Text = "Locking/Unlocking";
            // 
            // gbComment1
            // 
            this.gbComment1.Controls.Add(this.tbComment);
            this.gbComment1.Location = new System.Drawing.Point(45, 93);
            this.gbComment1.Name = "gbComment1";
            this.gbComment1.Size = new System.Drawing.Size(591, 60);
            this.gbComment1.TabIndex = 22;
            this.gbComment1.TabStop = false;
            this.gbComment1.Text = "Comment";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(17, 14);
            this.tbComment.MaxLength = 256;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(557, 37);
            this.tbComment.TabIndex = 0;
            // 
            // lblLastReaderTime
            // 
            this.lblLastReaderTime.BackColor = System.Drawing.Color.LightGray;
            this.lblLastReaderTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastReaderTime.Location = new System.Drawing.Point(45, 427);
            this.lblLastReaderTime.Name = "lblLastReaderTime";
            this.lblLastReaderTime.Size = new System.Drawing.Size(590, 25);
            this.lblLastReaderTime.TabIndex = 11;
            this.lblLastReaderTime.Text = "Last reading";
            this.lblLastReaderTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbTerminals
            // 
            this.gbTerminals.Controls.Add(this.lvReaders);
            this.gbTerminals.Location = new System.Drawing.Point(45, 198);
            this.gbTerminals.Name = "gbTerminals";
            this.gbTerminals.Size = new System.Drawing.Size(590, 215);
            this.gbTerminals.TabIndex = 10;
            this.gbTerminals.TabStop = false;
            this.gbTerminals.Text = "List of terminals with time of last reading";
            // 
            // lvReaders
            // 
            this.lvReaders.FullRowSelect = true;
            this.lvReaders.GridLines = true;
            this.lvReaders.HideSelection = false;
            this.lvReaders.Location = new System.Drawing.Point(24, 23);
            this.lvReaders.Name = "lvReaders";
            this.lvReaders.Size = new System.Drawing.Size(541, 176);
            this.lvReaders.TabIndex = 21;
            this.lvReaders.UseCompatibleStateImageBehavior = false;
            this.lvReaders.View = System.Windows.Forms.View.Details;
            // 
            // btnLockUnlock
            // 
            this.btnLockUnlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLockUnlock.Location = new System.Drawing.Point(303, 169);
            this.btnLockUnlock.Name = "btnLockUnlock";
            this.btnLockUnlock.Size = new System.Drawing.Size(75, 23);
            this.btnLockUnlock.TabIndex = 9;
            this.btnLockUnlock.Text = "Lock/Unlock";
            this.btnLockUnlock.Click += new System.EventHandler(this.btnLockUnlock_Click);
            // 
            // gbActionDef
            // 
            this.gbActionDef.Controls.Add(this.dtpDate);
            this.gbActionDef.Controls.Add(this.lblActionDef);
            this.gbActionDef.Location = new System.Drawing.Point(242, 15);
            this.gbActionDef.Name = "gbActionDef";
            this.gbActionDef.Size = new System.Drawing.Size(394, 67);
            this.gbActionDef.TabIndex = 1;
            this.gbActionDef.TabStop = false;
            this.gbActionDef.Text = "Define action";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd.MM.yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(177, 27);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(175, 20);
            this.dtpDate.TabIndex = 10;
            this.dtpDate.Value = new System.DateTime(2009, 7, 16, 0, 0, 0, 0);
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // lblActionDef
            // 
            this.lblActionDef.Location = new System.Drawing.Point(6, 19);
            this.lblActionDef.Name = "lblActionDef";
            this.lblActionDef.Size = new System.Drawing.Size(165, 34);
            this.lblActionDef.TabIndex = 9;
            this.lblActionDef.Text = "Action def label";
            this.lblActionDef.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbActionType
            // 
            this.gbActionType.Controls.Add(this.rbUnlocking);
            this.gbActionType.Controls.Add(this.rbLocking);
            this.gbActionType.Location = new System.Drawing.Point(45, 14);
            this.gbActionType.Name = "gbActionType";
            this.gbActionType.Size = new System.Drawing.Size(167, 67);
            this.gbActionType.TabIndex = 0;
            this.gbActionType.TabStop = false;
            this.gbActionType.Text = "Action type";
            // 
            // rbUnlocking
            // 
            this.rbUnlocking.AutoSize = true;
            this.rbUnlocking.Location = new System.Drawing.Point(24, 43);
            this.rbUnlocking.Name = "rbUnlocking";
            this.rbUnlocking.Size = new System.Drawing.Size(73, 17);
            this.rbUnlocking.TabIndex = 1;
            this.rbUnlocking.Text = "Unlocking";
            this.rbUnlocking.UseVisualStyleBackColor = true;
            // 
            // rbLocking
            // 
            this.rbLocking.AutoSize = true;
            this.rbLocking.Checked = true;
            this.rbLocking.Location = new System.Drawing.Point(24, 19);
            this.rbLocking.Name = "rbLocking";
            this.rbLocking.Size = new System.Drawing.Size(63, 17);
            this.rbLocking.TabIndex = 0;
            this.rbLocking.TabStop = true;
            this.rbLocking.Text = "Locking";
            this.rbLocking.UseVisualStyleBackColor = true;
            this.rbLocking.CheckedChanged += new System.EventHandler(this.rbLocking_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(626, 510);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Lockings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 542);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tcLocking);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Lockings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Locking";
            this.Load += new System.EventHandler(this.Lockings_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Lockings_KeyUp);
            this.tcLocking.ResumeLayout(false);
            this.tpPreview.ResumeLayout(false);
            this.gbPreviewAction.ResumeLayout(false);
            this.gbComment.ResumeLayout(false);
            this.gbCurentStatus.ResumeLayout(false);
            this.tpLockingUnlocking.ResumeLayout(false);
            this.gbComment1.ResumeLayout(false);
            this.gbTerminals.ResumeLayout(false);
            this.gbActionDef.ResumeLayout(false);
            this.gbActionType.ResumeLayout(false);
            this.gbActionType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcLocking;
        private System.Windows.Forms.TabPage tpPreview;
        private System.Windows.Forms.TabPage tpLockingUnlocking;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbCurentStatus;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.GroupBox gbPreviewAction;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.GroupBox gbComment;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox gbActionDef;
        private System.Windows.Forms.GroupBox gbActionType;
        private System.Windows.Forms.RadioButton rbUnlocking;
        private System.Windows.Forms.RadioButton rbLocking;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblActionDef;
        private System.Windows.Forms.Button btnLockUnlock;
        private System.Windows.Forms.GroupBox gbTerminals;
        private System.Windows.Forms.ListView lvReaders;
        private System.Windows.Forms.Label lblLastReaderTime;
        private System.Windows.Forms.GroupBox gbComment1;
        private System.Windows.Forms.RichTextBox tbComment;
    }
}