namespace UI
{
    partial class AccessControlStatus
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
            this.lvReaders = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelGreen = new System.Windows.Forms.Panel();
            this.lblGreen = new System.Windows.Forms.Label();
            this.lblOrange = new System.Windows.Forms.Label();
            this.panelOrange = new System.Windows.Forms.Panel();
            this.lblYellow = new System.Windows.Forms.Label();
            this.panelYellow = new System.Windows.Forms.Panel();
            this.lblRed = new System.Windows.Forms.Label();
            this.panelRed = new System.Windows.Forms.Panel();
            this.lblLegend = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvReaders
            // 
            this.lvReaders.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvReaders.FullRowSelect = true;
            this.lvReaders.GridLines = true;
            this.lvReaders.HideSelection = false;
            this.lvReaders.Location = new System.Drawing.Point(16, 16);
            this.lvReaders.MultiSelect = false;
            this.lvReaders.Name = "lvReaders";
            this.lvReaders.Size = new System.Drawing.Size(860, 328);
            this.lvReaders.TabIndex = 1;
            this.lvReaders.UseCompatibleStateImageBehavior = false;
            this.lvReaders.View = System.Windows.Forms.View.Details;
            this.lvReaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvReaders_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(801, 460);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(16, 460);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panelGreen
            // 
            this.panelGreen.BackColor = System.Drawing.Color.LightGreen;
            this.panelGreen.Location = new System.Drawing.Point(16, 390);
            this.panelGreen.Name = "panelGreen";
            this.panelGreen.Size = new System.Drawing.Size(32, 16);
            this.panelGreen.TabIndex = 3;
            // 
            // lblGreen
            // 
            this.lblGreen.Location = new System.Drawing.Point(60, 390);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(310, 16);
            this.lblGreen.TabIndex = 4;
            this.lblGreen.Text = "Reader uploaded with last issued access control parameters";
            this.lblGreen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrange
            // 
            this.lblOrange.Location = new System.Drawing.Point(60, 414);
            this.lblOrange.Name = "lblOrange";
            this.lblOrange.Size = new System.Drawing.Size(310, 16);
            this.lblOrange.TabIndex = 6;
            this.lblOrange.Text = "Uploading last issued access control parameters";
            this.lblOrange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelOrange
            // 
            this.panelOrange.BackColor = System.Drawing.Color.Orange;
            this.panelOrange.Location = new System.Drawing.Point(16, 414);
            this.panelOrange.Name = "panelOrange";
            this.panelOrange.Size = new System.Drawing.Size(32, 16);
            this.panelOrange.TabIndex = 5;
            // 
            // lblYellow
            // 
            this.lblYellow.Location = new System.Drawing.Point(426, 390);
            this.lblYellow.Name = "lblYellow";
            this.lblYellow.Size = new System.Drawing.Size(380, 16);
            this.lblYellow.TabIndex = 8;
            this.lblYellow.Text = "Last issued access control parameters for the reader have status \"pending\"";
            this.lblYellow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelYellow
            // 
            this.panelYellow.BackColor = System.Drawing.Color.Yellow;
            this.panelYellow.Location = new System.Drawing.Point(382, 390);
            this.panelYellow.Name = "panelYellow";
            this.panelYellow.Size = new System.Drawing.Size(32, 16);
            this.panelYellow.TabIndex = 7;
            // 
            // lblRed
            // 
            this.lblRed.Location = new System.Drawing.Point(426, 414);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(380, 16);
            this.lblRed.TabIndex = 10;
            this.lblRed.Text = "Last issued access control parameters can not be uploaded to the reader";
            this.lblRed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelRed
            // 
            this.panelRed.BackColor = System.Drawing.Color.OrangeRed;
            this.panelRed.Location = new System.Drawing.Point(382, 414);
            this.panelRed.Name = "panelRed";
            this.panelRed.Size = new System.Drawing.Size(32, 16);
            this.panelRed.TabIndex = 9;
            // 
            // lblLegend
            // 
            this.lblLegend.Location = new System.Drawing.Point(13, 366);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(100, 16);
            this.lblLegend.TabIndex = 2;
            this.lblLegend.Text = "Legend:";
            // 
            // AccessControlStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 496);
            this.ControlBox = false;
            this.Controls.Add(this.lblLegend);
            this.Controls.Add(this.lblRed);
            this.Controls.Add(this.panelRed);
            this.Controls.Add(this.lblYellow);
            this.Controls.Add(this.panelYellow);
            this.Controls.Add(this.lblOrange);
            this.Controls.Add(this.panelOrange);
            this.Controls.Add(this.lblGreen);
            this.Controls.Add(this.panelGreen);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvReaders);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 530);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 530);
            this.Name = "AccessControlStatus";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Access control status";
            this.Load += new System.EventHandler(this.AccessControlStatus_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AccessControlStatus_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvReaders;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panelGreen;
        private System.Windows.Forms.Label lblGreen;
        private System.Windows.Forms.Label lblOrange;
        private System.Windows.Forms.Panel panelOrange;
        private System.Windows.Forms.Label lblYellow;
        private System.Windows.Forms.Panel panelYellow;
        private System.Windows.Forms.Label lblRed;
        private System.Windows.Forms.Panel panelRed;
        private System.Windows.Forms.Label lblLegend;
    }
}