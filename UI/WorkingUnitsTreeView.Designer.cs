namespace UI
{
    partial class WorkingUnitsTreeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkingUnitsTreeView));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dataTreeView1 = new UI.DataTreeView();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Network.ico");
            this.imageList1.Images.SetKeyName(1, "Msn-Buddy-mobile.ico");
            this.imageList1.Images.SetKeyName(2, "Msn-Buddy.ico");
            this.imageList1.Images.SetKeyName(3, "Msn-Messenger.ico");
            // 
            // dataTreeView1
            // 
            this.dataTreeView1.DataMember = null;
            this.dataTreeView1.FullRowSelect = true;
            this.dataTreeView1.HideSelection = false;
            this.dataTreeView1.HotTracking = true;
            this.dataTreeView1.ImageIndex = 3;
            this.dataTreeView1.ImageList = this.imageList1;
            this.dataTreeView1.Location = new System.Drawing.Point(12, 12);
            this.dataTreeView1.Name = "dataTreeView1";
            this.dataTreeView1.SelectedImageIndex = 3;
            this.dataTreeView1.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("dataTreeView1.SelectedNodes")));
            this.dataTreeView1.ShowNodeToolTips = true;
            this.dataTreeView1.Size = new System.Drawing.Size(407, 390);
            this.dataTreeView1.TabIndex = 0;
            this.dataTreeView1.ValueColumn = null;
            this.dataTreeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.workingUnitsTreeView_Click);
            this.dataTreeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataTreeView1_MouseMove);
            this.dataTreeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataTreeView1_KeyUp);
            // 
            // WorkingUnitsTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 418);
            this.Controls.Add(this.dataTreeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkingUnitsTreeView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "WorkingUnitsTreeView";
            this.Load += new System.EventHandler(this.WorkingUnitsTreeView_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WorkingUnits_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private DataTreeView dataTreeView1;
        public System.Windows.Forms.ImageList imageList1;
    }
}