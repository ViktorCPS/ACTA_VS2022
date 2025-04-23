namespace UI
{
    partial class LastTerminalReading
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
            this.lblReaders = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvReaders = new System.Windows.Forms.ListView();
            this.lvLogs = new System.Windows.Forms.ListView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblProcessed = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblReaders
            // 
            this.lblReaders.Location = new System.Drawing.Point(12, 9);
            this.lblReaders.Name = "lblReaders";
            this.lblReaders.Size = new System.Drawing.Size(344, 23);
            this.lblReaders.TabIndex = 0;
            this.lblReaders.Text = "Readers and their last reading time:";
            this.lblReaders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(907, 637);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvReaders
            // 
            this.lvReaders.BackColor = System.Drawing.SystemColors.Control;
            this.lvReaders.FullRowSelect = true;
            this.lvReaders.GridLines = true;
            this.lvReaders.HideSelection = false;
            this.lvReaders.Location = new System.Drawing.Point(12, 41);
            this.lvReaders.MultiSelect = false;
            this.lvReaders.Name = "lvReaders";
            this.lvReaders.Size = new System.Drawing.Size(970, 359);
            this.lvReaders.TabIndex = 1;
            this.lvReaders.UseCompatibleStateImageBehavior = false;
            this.lvReaders.View = System.Windows.Forms.View.Details;
            this.lvReaders.SelectedIndexChanged += new System.EventHandler(this.lvReaders_SelectedIndexChanged);
            // 
            // lvLogs
            // 
            this.lvLogs.BackColor = System.Drawing.SystemColors.Control;
            this.lvLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLogs.FullRowSelect = true;
            this.lvLogs.GridLines = true;
            this.lvLogs.HideSelection = false;
            this.lvLogs.Location = new System.Drawing.Point(12, 419);
            this.lvLogs.MultiSelect = false;
            this.lvLogs.Name = "lvLogs";
            this.lvLogs.ShowItemToolTips = true;
            this.lvLogs.Size = new System.Drawing.Size(970, 212);
            this.lvLogs.TabIndex = 3;
            this.lvLogs.UseCompatibleStateImageBehavior = false;
            this.lvLogs.View = System.Windows.Forms.View.Details;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 637);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblProcessed
            // 
            this.lblProcessed.AutoSize = true;
            this.lblProcessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessed.Location = new System.Drawing.Point(12, 403);
            this.lblProcessed.Name = "lblProcessed";
            this.lblProcessed.Size = new System.Drawing.Size(83, 13);
            this.lblProcessed.TabIndex = 2;
            this.lblProcessed.Text = "*Only processed";
            // 
            // LastTerminalReading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 672);
            this.ControlBox = false;
            this.Controls.Add(this.lblProcessed);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lvLogs);
            this.Controls.Add(this.lblReaders);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvReaders);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LastTerminalReading";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LastTerminalReading";
            this.Load += new System.EventHandler(this.LastTerminalReading_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblReaders;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvReaders;
        private System.Windows.Forms.ListView lvLogs;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblProcessed;
    }
}