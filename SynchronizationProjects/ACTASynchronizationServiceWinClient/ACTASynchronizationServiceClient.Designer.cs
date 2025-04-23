namespace ACTASynchronizationServiceWinClient
{
    partial class ACTASynchronizationServiceClient
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ACTASynchronizationServiceClient));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.syncStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SyncStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ServiceStatusWorker = new System.ComponentModel.BackgroundWorker();
            this.stsBar = new System.Windows.Forms.StatusBar();
            this.ctxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.ctxMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ACTA Synchronization Manager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.toolStripSeparator1,
            this.syncStartToolStripMenuItem,
            this.SyncStopToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(295, 126);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(294, 22);
            this.openMenuItem.Text = "Open ACTA Synchronization Manager";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(291, 6);
            // 
            // syncStartToolStripMenuItem
            // 
            this.syncStartToolStripMenuItem.Enabled = false;
            this.syncStartToolStripMenuItem.Name = "syncStartToolStripMenuItem";
            this.syncStartToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.syncStartToolStripMenuItem.Text = "Synchronization - Start";
            // 
            // SyncStopToolStripMenuItem
            // 
            this.SyncStopToolStripMenuItem.Enabled = false;
            this.SyncStopToolStripMenuItem.Name = "SyncStopToolStripMenuItem";
            this.SyncStopToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.SyncStopToolStripMenuItem.Text = "Synchronization - Stop";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(291, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(479, 113);
            this.lblMessage.TabIndex = 7;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(214, 135);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 8;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.TextChanged += new System.EventHandler(this.btnStartStop_TextChanged);
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ServiceStatusWorker
            // 
            this.ServiceStatusWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ServiceStatusWorker_DoWork);
            this.ServiceStatusWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ServiceStatusWorker_RunWorkerCompleted);
            // 
            // stsBar
            // 
            this.stsBar.Location = new System.Drawing.Point(0, 168);
            this.stsBar.Name = "stsBar";
            this.stsBar.Size = new System.Drawing.Size(503, 16);
            this.stsBar.TabIndex = 10;
            // 
            // ACTASynchronizationServiceClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 184);
            this.Controls.Add(this.stsBar);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ACTASynchronizationServiceClient";
            this.Text = "ACTA Synchronization";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.ACTASynchronizationServiceClient_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ACTASynchronizationServiceClient_FormClosing);
            this.ctxMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem syncStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SyncStopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker ServiceStatusWorker;
        private System.Windows.Forms.StatusBar stsBar;
    }
}

