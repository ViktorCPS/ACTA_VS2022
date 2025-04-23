namespace EmailNotificationServiceWinClient
{
    partial class ACTAEmailNotificationServiceClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ACTAEmailNotificationServiceClient));
            this.lblMessage = new System.Windows.Forms.Label();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openACTAEmailNotificationManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.notificationStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notificationEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ServiceStatusWorker = new System.ComponentModel.BackgroundWorker();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btStartStop = new System.Windows.Forms.Button();
            this.lblNotificationStatus = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMessage.Location = new System.Drawing.Point(28, 25);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(422, 72);
            this.lblMessage.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openACTAEmailNotificationManagerToolStripMenuItem,
            this.toolStripSeparator1,
            this.notificationStartToolStripMenuItem,
            this.notificationEndToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(283, 104);
            // 
            // openACTAEmailNotificationManagerToolStripMenuItem
            // 
            this.openACTAEmailNotificationManagerToolStripMenuItem.Name = "openACTAEmailNotificationManagerToolStripMenuItem";
            this.openACTAEmailNotificationManagerToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.openACTAEmailNotificationManagerToolStripMenuItem.Text = "Open ACTA EmailNotification Manager";
            this.openACTAEmailNotificationManagerToolStripMenuItem.Click += new System.EventHandler(this.openACTAEmailNotificationManagerToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(279, 6);
            // 
            // notificationStartToolStripMenuItem
            // 
            this.notificationStartToolStripMenuItem.Name = "notificationStartToolStripMenuItem";
            this.notificationStartToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.notificationStartToolStripMenuItem.Text = "Notification- Start";
            // 
            // notificationEndToolStripMenuItem
            // 
            this.notificationEndToolStripMenuItem.Name = "notificationEndToolStripMenuItem";
            this.notificationEndToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.notificationEndToolStripMenuItem.Text = "Notification- Stop";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(279, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "EmailNotification Manager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // btStartStop
            // 
            this.btStartStop.Location = new System.Drawing.Point(185, 195);
            this.btStartStop.Name = "btStartStop";
            this.btStartStop.Size = new System.Drawing.Size(75, 23);
            this.btStartStop.TabIndex = 1;
            this.btStartStop.Text = "Start";
            this.btStartStop.UseVisualStyleBackColor = true;
            this.btStartStop.TextChanged += new System.EventHandler(this.btnStartStop_TextChanged);
            this.btStartStop.Click += new System.EventHandler(this.btStartStop_Click);
            // 
            // lblNotificationStatus
            // 
            this.lblNotificationStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNotificationStatus.Location = new System.Drawing.Point(28, 107);
            this.lblNotificationStatus.Name = "lblNotificationStatus";
            this.lblNotificationStatus.Size = new System.Drawing.Size(422, 72);
            this.lblNotificationStatus.TabIndex = 2;
            // 
            // ACTAEmailNotificationServiceClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 262);
            this.Controls.Add(this.lblNotificationStatus);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btStartStop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ACTAEmailNotificationServiceClient";
            this.Text = "ACTAEmailNotification";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.ACTAEmailNotificationServiceClient_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ACTAEmailNotificationServiceClient_FormClosing);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }





        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btStartStop;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem openACTAEmailNotificationManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notificationStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notificationEndToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker ServiceStatusWorker;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label lblNotificationStatus;
    }
}

