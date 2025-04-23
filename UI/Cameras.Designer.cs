namespace UI
{
    partial class Cameras
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cameras));
            this.lvCameras = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMaps = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvCameras
            // 
            this.lvCameras.FullRowSelect = true;
            this.lvCameras.GridLines = true;
            this.lvCameras.HideSelection = false;
            this.lvCameras.Location = new System.Drawing.Point(16, 16);
            this.lvCameras.Name = "lvCameras";
            this.lvCameras.Size = new System.Drawing.Size(584, 328);
            this.lvCameras.TabIndex = 1;
            this.lvCameras.UseCompatibleStateImageBehavior = false;
            this.lvCameras.View = System.Windows.Forms.View.Details;
            this.lvCameras.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvCameras_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(525, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(192, 360);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(104, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 360);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMaps
            // 
            this.btnMaps.Image = ((System.Drawing.Image)(resources.GetObject("btnMaps.Image")));
            this.btnMaps.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMaps.Location = new System.Drawing.Point(359, 360);
            this.btnMaps.Name = "btnMaps";
            this.btnMaps.Size = new System.Drawing.Size(75, 23);
            this.btnMaps.TabIndex = 11;
            this.btnMaps.Text = "Maps";
            this.btnMaps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMaps.Click += new System.EventHandler(this.btnMaps_Click);
            // 
            // Cameras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 390);
            this.ControlBox = false;
            this.Controls.Add(this.btnMaps);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvCameras);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 424);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "Cameras";
            this.ShowInTaskbar = false;
            this.Text = "Cameras";
            this.Load += new System.EventHandler(this.Cameras_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Cameras_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvCameras;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnMaps;
    }
}