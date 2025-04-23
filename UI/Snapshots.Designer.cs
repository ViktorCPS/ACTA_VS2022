namespace UI
{
    partial class Snapshots
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbPhotoNavigation = new System.Windows.Forms.GroupBox();
            this.btnPhotoFirst = new System.Windows.Forms.Button();
            this.btnPhotoPrev = new System.Windows.Forms.Button();
            this.btnPhotoNext = new System.Windows.Forms.Button();
            this.btnPhotoLast = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lblZoom = new System.Windows.Forms.Label();
            this.lblPlus = new System.Windows.Forms.Label();
            this.lblMinus = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelSnapshot = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbPhotoNavigation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panelSnapshot.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(909, 671);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(896, 635);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // gbPhotoNavigation
            // 
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoFirst);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoPrev);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoNext);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoLast);
            this.gbPhotoNavigation.Location = new System.Drawing.Point(260, 653);
            this.gbPhotoNavigation.Name = "gbPhotoNavigation";
            this.gbPhotoNavigation.Size = new System.Drawing.Size(400, 50);
            this.gbPhotoNavigation.TabIndex = 8;
            this.gbPhotoNavigation.TabStop = false;
            this.gbPhotoNavigation.Text = "Photo navigation";
            // 
            // btnPhotoFirst
            // 
            this.btnPhotoFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoFirst.Location = new System.Drawing.Point(10, 18);
            this.btnPhotoFirst.Name = "btnPhotoFirst";
            this.btnPhotoFirst.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoFirst.TabIndex = 1;
            this.btnPhotoFirst.Text = "|<";
            this.btnPhotoFirst.Click += new System.EventHandler(this.btnPhotoFirst_Click);
            // 
            // btnPhotoPrev
            // 
            this.btnPhotoPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoPrev.Location = new System.Drawing.Point(110, 18);
            this.btnPhotoPrev.Name = "btnPhotoPrev";
            this.btnPhotoPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoPrev.TabIndex = 2;
            this.btnPhotoPrev.Text = "<<";
            this.btnPhotoPrev.Click += new System.EventHandler(this.btnPhotoPrev_Click);
            // 
            // btnPhotoNext
            // 
            this.btnPhotoNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoNext.Location = new System.Drawing.Point(210, 18);
            this.btnPhotoNext.Name = "btnPhotoNext";
            this.btnPhotoNext.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoNext.TabIndex = 3;
            this.btnPhotoNext.Text = ">>";
            this.btnPhotoNext.Click += new System.EventHandler(this.btnPhotoNext_Click);
            // 
            // btnPhotoLast
            // 
            this.btnPhotoLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoLast.Location = new System.Drawing.Point(310, 18);
            this.btnPhotoLast.Name = "btnPhotoLast";
            this.btnPhotoLast.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoLast.TabIndex = 4;
            this.btnPhotoLast.Text = ">|";
            this.btnPhotoLast.Click += new System.EventHandler(this.btnPhotoLast_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(955, 318);
            this.trackBar1.Maximum = 5;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 104);
            this.trackBar1.TabIndex = 9;
            this.trackBar1.Value = 1;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Location = new System.Drawing.Point(926, 361);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(32, 13);
            this.lblZoom.TabIndex = 10;
            this.lblZoom.Text = "zoom";
            // 
            // lblPlus
            // 
            this.lblPlus.Location = new System.Drawing.Point(934, 318);
            this.lblPlus.Name = "lblPlus";
            this.lblPlus.Size = new System.Drawing.Size(24, 23);
            this.lblPlus.TabIndex = 11;
            this.lblPlus.Text = "+";
            this.lblPlus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMinus
            // 
            this.lblMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinus.Location = new System.Drawing.Point(933, 399);
            this.lblMinus.Name = "lblMinus";
            this.lblMinus.Size = new System.Drawing.Size(24, 23);
            this.lblMinus.TabIndex = 12;
            this.lblMinus.Text = "-";
            this.lblMinus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 671);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // panelSnapshot
            // 
            this.panelSnapshot.AutoScroll = true;
            this.panelSnapshot.Controls.Add(this.pictureBox1);
            this.panelSnapshot.Location = new System.Drawing.Point(12, 12);
            this.panelSnapshot.Name = "panelSnapshot";
            this.panelSnapshot.Size = new System.Drawing.Size(896, 635);
            this.panelSnapshot.TabIndex = 14;
            // 
            // Snapshots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 706);
            this.ControlBox = false;
            this.Controls.Add(this.panelSnapshot);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblMinus);
            this.Controls.Add(this.lblPlus);
            this.Controls.Add(this.lblZoom);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.gbPhotoNavigation);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Snapshots";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Snapshots";
            this.Load += new System.EventHandler(this.Snapshots_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbPhotoNavigation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panelSnapshot.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox gbPhotoNavigation;
        private System.Windows.Forms.Button btnPhotoFirst;
        private System.Windows.Forms.Button btnPhotoPrev;
        private System.Windows.Forms.Button btnPhotoNext;
        private System.Windows.Forms.Button btnPhotoLast;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.Label lblPlus;
        private System.Windows.Forms.Label lblMinus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panelSnapshot;
    }
}