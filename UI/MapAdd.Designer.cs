namespace UI
{
    partial class MapAdd
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMapID = new System.Windows.Forms.TextBox();
            this.lblMapID = new System.Windows.Forms.Label();
            this.gbMap = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStar1 = new System.Windows.Forms.Label();
            this.pbPicture = new System.Windows.Forms.PictureBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPicture = new System.Windows.Forms.TextBox();
            this.lblPisture = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gbMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(116, 62);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(186, 20);
            this.tbName.TabIndex = 7;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(6, 60);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(104, 23);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMapID
            // 
            this.tbMapID.Location = new System.Drawing.Point(116, 29);
            this.tbMapID.Name = "tbMapID";
            this.tbMapID.Size = new System.Drawing.Size(186, 20);
            this.tbMapID.TabIndex = 5;
            // 
            // lblMapID
            // 
            this.lblMapID.Location = new System.Drawing.Point(6, 27);
            this.lblMapID.Name = "lblMapID";
            this.lblMapID.Size = new System.Drawing.Size(104, 23);
            this.lblMapID.TabIndex = 4;
            this.lblMapID.Text = "Map id:";
            this.lblMapID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbMap
            // 
            this.gbMap.Controls.Add(this.label1);
            this.gbMap.Controls.Add(this.lblStar1);
            this.gbMap.Controls.Add(this.pbPicture);
            this.gbMap.Controls.Add(this.btnBrowse);
            this.gbMap.Controls.Add(this.tbPicture);
            this.gbMap.Controls.Add(this.lblPisture);
            this.gbMap.Controls.Add(this.tbDescription);
            this.gbMap.Controls.Add(this.lblDescription);
            this.gbMap.Controls.Add(this.lblMapID);
            this.gbMap.Controls.Add(this.tbName);
            this.gbMap.Controls.Add(this.tbMapID);
            this.gbMap.Controls.Add(this.lblName);
            this.gbMap.Location = new System.Drawing.Point(12, 12);
            this.gbMap.Name = "gbMap";
            this.gbMap.Size = new System.Drawing.Size(837, 637);
            this.gbMap.TabIndex = 8;
            this.gbMap.TabStop = false;
            this.gbMap.Text = "Map";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(308, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 38;
            this.label1.Text = "*";
            // 
            // lblStar1
            // 
            this.lblStar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar1.ForeColor = System.Drawing.Color.Red;
            this.lblStar1.Location = new System.Drawing.Point(308, 27);
            this.lblStar1.Name = "lblStar1";
            this.lblStar1.Size = new System.Drawing.Size(16, 16);
            this.lblStar1.TabIndex = 37;
            this.lblStar1.Text = "*";
            // 
            // pbPicture
            // 
            this.pbPicture.Location = new System.Drawing.Point(19, 174);
            this.pbPicture.Name = "pbPicture";
            this.pbPicture.Size = new System.Drawing.Size(802, 457);
            this.pbPicture.TabIndex = 13;
            this.pbPicture.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(308, 131);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 12;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPicture
            // 
            this.tbPicture.Location = new System.Drawing.Point(116, 133);
            this.tbPicture.Name = "tbPicture";
            this.tbPicture.Size = new System.Drawing.Size(186, 20);
            this.tbPicture.TabIndex = 11;
            // 
            // lblPisture
            // 
            this.lblPisture.Location = new System.Drawing.Point(6, 131);
            this.lblPisture.Name = "lblPisture";
            this.lblPisture.Size = new System.Drawing.Size(104, 23);
            this.lblPisture.TabIndex = 10;
            this.lblPisture.Text = "Picture:";
            this.lblPisture.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(116, 97);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(186, 20);
            this.tbDescription.TabIndex = 9;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(6, 95);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(104, 23);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 655);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(774, 655);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 655);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // MapAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 684);
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "MapAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "New map";
            this.Load += new System.EventHandler(this.MapAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MapAdd_KeyUp);
            this.gbMap.ResumeLayout(false);
            this.gbMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMapID;
        private System.Windows.Forms.Label lblMapID;
        private System.Windows.Forms.GroupBox gbMap;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbPicture;
        private System.Windows.Forms.Label lblPisture;
        private System.Windows.Forms.PictureBox pbPicture;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblStar1;
        private System.Windows.Forms.Label label1;
    }
}