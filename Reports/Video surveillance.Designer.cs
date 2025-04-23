namespace Reports
{
    partial class Video_surveillance
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
            this.btnClose = new System.Windows.Forms.Button();
            this.gbGatesAndCameras = new System.Windows.Forms.GroupBox();
            this.lblLegend = new System.Windows.Forms.Label();
            this.lvCameras = new System.Windows.Forms.ListView();
            this.lblCamerasOnGate = new System.Windows.Forms.Label();
            this.lblGate = new System.Windows.Forms.Label();
            this.cbGate = new System.Windows.Forms.ComboBox();
            this.gbSurveillance = new System.Windows.Forms.GroupBox();
            this.lblCamera = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.gbGatesAndCameras.SuspendLayout();
            this.gbSurveillance.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(725, 408);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbGatesAndCameras
            // 
            this.gbGatesAndCameras.Controls.Add(this.lblLegend);
            this.gbGatesAndCameras.Controls.Add(this.lvCameras);
            this.gbGatesAndCameras.Controls.Add(this.lblCamerasOnGate);
            this.gbGatesAndCameras.Controls.Add(this.lblGate);
            this.gbGatesAndCameras.Controls.Add(this.cbGate);
            this.gbGatesAndCameras.Location = new System.Drawing.Point(12, 12);
            this.gbGatesAndCameras.Name = "gbGatesAndCameras";
            this.gbGatesAndCameras.Size = new System.Drawing.Size(430, 373);
            this.gbGatesAndCameras.TabIndex = 1;
            this.gbGatesAndCameras.TabStop = false;
            this.gbGatesAndCameras.Text = "Gates and cameras";
            // 
            // lblLegend
            // 
            this.lblLegend.Location = new System.Drawing.Point(12, 340);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(407, 23);
            this.lblLegend.TabIndex = 5;
            this.lblLegend.Text = "* Double click on camera to get live stream from it";
            this.lblLegend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvCameras
            // 
            this.lvCameras.FullRowSelect = true;
            this.lvCameras.GridLines = true;
            this.lvCameras.HideSelection = false;
            this.lvCameras.Location = new System.Drawing.Point(12, 94);
            this.lvCameras.MultiSelect = false;
            this.lvCameras.Name = "lvCameras";
            this.lvCameras.Size = new System.Drawing.Size(407, 240);
            this.lvCameras.TabIndex = 4;
            this.lvCameras.UseCompatibleStateImageBehavior = false;
            this.lvCameras.View = System.Windows.Forms.View.Details;
            this.lvCameras.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvCameras_MouseDoubleClick);
            this.lvCameras.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvCameras_ColumnClick);
            // 
            // lblCamerasOnGate
            // 
            this.lblCamerasOnGate.Location = new System.Drawing.Point(12, 68);
            this.lblCamerasOnGate.Name = "lblCamerasOnGate";
            this.lblCamerasOnGate.Size = new System.Drawing.Size(204, 23);
            this.lblCamerasOnGate.TabIndex = 3;
            this.lblCamerasOnGate.Text = "Cameras on selected gates:";
            this.lblCamerasOnGate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGate
            // 
            this.lblGate.Location = new System.Drawing.Point(12, 32);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(96, 23);
            this.lblGate.TabIndex = 1;
            this.lblGate.Text = "Gate:";
            this.lblGate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbGate
            // 
            this.cbGate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGate.Location = new System.Drawing.Point(110, 32);
            this.cbGate.Name = "cbGate";
            this.cbGate.Size = new System.Drawing.Size(309, 21);
            this.cbGate.TabIndex = 2;
            this.cbGate.SelectedIndexChanged += new System.EventHandler(this.cbGate_SelectedIndexChanged);
            // 
            // gbSurveillance
            // 
            this.gbSurveillance.Controls.Add(this.lblCamera);
            this.gbSurveillance.Controls.Add(this.webBrowser1);
            this.gbSurveillance.Location = new System.Drawing.Point(460, 12);
            this.gbSurveillance.Name = "gbSurveillance";
            this.gbSurveillance.Size = new System.Drawing.Size(340, 373);
            this.gbSurveillance.TabIndex = 2;
            this.gbSurveillance.TabStop = false;
            this.gbSurveillance.Text = "Live stream for selected camera";
            // 
            // lblCamera
            // 
            this.lblCamera.Location = new System.Drawing.Point(10, 55);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(320, 23);
            this.lblCamera.TabIndex = 2;
            this.lblCamera.Text = "Camera:";
            this.lblCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(10, 94);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(320, 240);
            this.webBrowser1.TabIndex = 1;
            // 
            // Video_surveillance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 436);
            this.ControlBox = false;
            this.Controls.Add(this.gbSurveillance);
            this.Controls.Add(this.gbGatesAndCameras);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(824, 470);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(824, 470);
            this.Name = "Video_surveillance";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Video surveillance";
            this.Load += new System.EventHandler(this.Video_surveillance_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Video_surveillance_KeyUp);
            this.gbGatesAndCameras.ResumeLayout(false);
            this.gbSurveillance.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbGatesAndCameras;
        private System.Windows.Forms.ComboBox cbGate;
        private System.Windows.Forms.Label lblGate;
        private System.Windows.Forms.Label lblCamerasOnGate;
        private System.Windows.Forms.ListView lvCameras;
        private System.Windows.Forms.GroupBox gbSurveillance;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label lblLegend;
        private System.Windows.Forms.Label lblCamera;
    }
}