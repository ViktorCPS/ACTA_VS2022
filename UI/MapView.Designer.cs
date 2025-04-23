namespace UI
{
    partial class MapView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapView));
            this.btnClose = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.cbMap = new System.Windows.Forms.ComboBox();
            this.lblMap = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gateStartMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locationCurrentPresenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraLiveStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbLegend = new System.Windows.Forms.GroupBox();
            this.lblReader = new System.Windows.Forms.Label();
            this.lblCamera = new System.Windows.Forms.Label();
            this.lblGate = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbCamera = new System.Windows.Forms.PictureBox();
            this.pbGate = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mapControl1 = new UI.MapControl();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.gbLegend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGate)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(903, 683);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 269);
            this.trackBar1.Maximum = 3;
            this.trackBar1.Minimum = -3;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 155);
            this.trackBar1.TabIndex = 18;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // cbMap
            // 
            this.cbMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMap.FormattingEnabled = true;
            this.cbMap.Location = new System.Drawing.Point(137, 14);
            this.cbMap.Name = "cbMap";
            this.cbMap.Size = new System.Drawing.Size(168, 21);
            this.cbMap.TabIndex = 20;
            this.cbMap.SelectedIndexChanged += new System.EventHandler(this.cbMap_SelectedIndexChanged);
            // 
            // lblMap
            // 
            this.lblMap.Location = new System.Drawing.Point(76, 12);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(55, 23);
            this.lblMap.TabIndex = 21;
            this.lblMap.Text = "Map:";
            this.lblMap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gateStartMonitorToolStripMenuItem,
            this.locationCurrentPresenceToolStripMenuItem,
            this.cameraLiveStreamToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(218, 70);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // gateStartMonitorToolStripMenuItem
            // 
            this.gateStartMonitorToolStripMenuItem.Name = "gateStartMonitorToolStripMenuItem";
            this.gateStartMonitorToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.gateStartMonitorToolStripMenuItem.Text = "Gate - start monitor";
            this.gateStartMonitorToolStripMenuItem.Click += new System.EventHandler(this.gateStartMonitorToolStripMenuItem_Click);
            // 
            // locationCurrentPresenceToolStripMenuItem
            // 
            this.locationCurrentPresenceToolStripMenuItem.Name = "locationCurrentPresenceToolStripMenuItem";
            this.locationCurrentPresenceToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.locationCurrentPresenceToolStripMenuItem.Text = "Location - current presence";
            this.locationCurrentPresenceToolStripMenuItem.Click += new System.EventHandler(this.locationCurrentPresenceToolStripMenuItem_Click);
            // 
            // cameraLiveStreamToolStripMenuItem
            // 
            this.cameraLiveStreamToolStripMenuItem.Name = "cameraLiveStreamToolStripMenuItem";
            this.cameraLiveStreamToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.cameraLiveStreamToolStripMenuItem.Text = "Camera - live stream";
            this.cameraLiveStreamToolStripMenuItem.Click += new System.EventHandler(this.cameraLiveStreamToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "+";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(41, 400);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 24);
            this.label3.TabIndex = 24;
            this.label3.Text = "-";
            // 
            // gbLegend
            // 
            this.gbLegend.Controls.Add(this.lblReader);
            this.gbLegend.Controls.Add(this.lblCamera);
            this.gbLegend.Controls.Add(this.lblGate);
            this.gbLegend.Controls.Add(this.pictureBox1);
            this.gbLegend.Controls.Add(this.pbCamera);
            this.gbLegend.Controls.Add(this.pbGate);
            this.gbLegend.Location = new System.Drawing.Point(79, 651);
            this.gbLegend.Name = "gbLegend";
            this.gbLegend.Size = new System.Drawing.Size(734, 55);
            this.gbLegend.TabIndex = 37;
            this.gbLegend.TabStop = false;
            this.gbLegend.Text = "Legend";
            // 
            // lblReader
            // 
            this.lblReader.AutoSize = true;
            this.lblReader.Location = new System.Drawing.Point(577, 25);
            this.lblReader.Name = "lblReader";
            this.lblReader.Size = new System.Drawing.Size(42, 13);
            this.lblReader.TabIndex = 19;
            this.lblReader.Text = "Reader";
            // 
            // lblCamera
            // 
            this.lblCamera.AutoSize = true;
            this.lblCamera.Location = new System.Drawing.Point(313, 25);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(43, 13);
            this.lblCamera.TabIndex = 18;
            this.lblCamera.Text = "Camera";
            // 
            // lblGate
            // 
            this.lblGate.AutoSize = true;
            this.lblGate.Location = new System.Drawing.Point(55, 25);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(30, 13);
            this.lblGate.TabIndex = 17;
            this.lblGate.Text = "Gate";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(534, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(37, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pbCamera
            // 
            this.pbCamera.Image = ((System.Drawing.Image)(resources.GetObject("pbCamera.Image")));
            this.pbCamera.Location = new System.Drawing.Point(270, 19);
            this.pbCamera.Name = "pbCamera";
            this.pbCamera.Size = new System.Drawing.Size(37, 29);
            this.pbCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCamera.TabIndex = 1;
            this.pbCamera.TabStop = false;
            // 
            // pbGate
            // 
            this.pbGate.Image = ((System.Drawing.Image)(resources.GetObject("pbGate.Image")));
            this.pbGate.Location = new System.Drawing.Point(6, 19);
            this.pbGate.Name = "pbGate";
            this.pbGate.Size = new System.Drawing.Size(37, 29);
            this.pbGate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbGate.TabIndex = 0;
            this.pbGate.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 339);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "zoom";
            // 
            // mapControl1
            // 
            this.mapControl1.AutoScroll = true;
            this.mapControl1.Image = null;
            this.mapControl1.Location = new System.Drawing.Point(79, 41);
            this.mapControl1.Name = "mapControl1";
            //this.mapControl1.PathsArray = ((List<LocationTO>)(resources.GetObject("mapControl1.PathsArray")));
            this.mapControl1.PrevPictureBoxHeight = 0;
            this.mapControl1.PrevPictureBoxWidth = 0;
            this.mapControl1.Size = new System.Drawing.Size(907, 604);
            this.mapControl1.TabIndex = 19;
            this.mapControl1.RightClick += new UI.MapControl.RightClickDelegate(this.rightClickOnObject);
            // 
            // MapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 718);
            this.ControlBox = false;
            this.Controls.Add(this.gbLegend);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMap);
            this.Controls.Add(this.cbMap);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.mapControl1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "MapView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MapView";
            this.Load += new System.EventHandler(this.MapView_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MapView_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.gbLegend.ResumeLayout(false);
            this.gbLegend.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TrackBar trackBar1;
        private MapControl mapControl1;
        private System.Windows.Forms.ComboBox cbMap;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gateStartMonitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locationCurrentPresenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cameraLiveStreamToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbLegend;
        private System.Windows.Forms.PictureBox pbGate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbCamera;
        protected System.Windows.Forms.Label lblCamera;
        protected System.Windows.Forms.Label lblGate;
        protected System.Windows.Forms.Label lblReader;
        private System.Windows.Forms.Label label1;
    }
}