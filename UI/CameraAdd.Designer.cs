namespace UI
{
    partial class CameraAdd
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbCameraID = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblCameraID = new System.Windows.Forms.Label();
            this.gbCameraReaders = new System.Windows.Forms.GroupBox();
            this.lvDirection = new System.Windows.Forms.ListView();
            this.btnDirectionAdd = new System.Windows.Forms.Button();
            this.lblSelReaders = new System.Windows.Forms.Label();
            this.lblNotSelReaders = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvSelReaders = new System.Windows.Forms.ListView();
            this.lvNotSelReaders = new System.Windows.Forms.ListView();
            this.tbType = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDot2 = new System.Windows.Forms.Label();
            this.lblDot1 = new System.Windows.Forms.Label();
            this.lblDot0 = new System.Windows.Forms.Label();
            this.tbIP2 = new System.Windows.Forms.TextBox();
            this.tbIP1 = new System.Windows.Forms.TextBox();
            this.tbIP0 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbIP3 = new System.Windows.Forms.TextBox();
            this.gbCamera = new System.Windows.Forms.GroupBox();
            this.gbCameraReaders.SuspendLayout();
            this.gbCamera.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(10, 560);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(266, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "*";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(607, 560);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(10, 560);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(106, 50);
            this.tbDesc.MaxLength = 35;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(152, 20);
            this.tbDesc.TabIndex = 4;
            // 
            // tbCameraID
            // 
            this.tbCameraID.Location = new System.Drawing.Point(106, 18);
            this.tbCameraID.MaxLength = 50;
            this.tbCameraID.Name = "tbCameraID";
            this.tbCameraID.Size = new System.Drawing.Size(152, 20);
            this.tbCameraID.TabIndex = 2;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(18, 48);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(80, 23);
            this.lblDesc.TabIndex = 3;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCameraID
            // 
            this.lblCameraID.Location = new System.Drawing.Point(18, 16);
            this.lblCameraID.Name = "lblCameraID";
            this.lblCameraID.Size = new System.Drawing.Size(80, 23);
            this.lblCameraID.TabIndex = 1;
            this.lblCameraID.Text = "ID:";
            this.lblCameraID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbCameraReaders
            // 
            this.gbCameraReaders.Controls.Add(this.lvDirection);
            this.gbCameraReaders.Controls.Add(this.btnDirectionAdd);
            this.gbCameraReaders.Controls.Add(this.lblSelReaders);
            this.gbCameraReaders.Controls.Add(this.lblNotSelReaders);
            this.gbCameraReaders.Controls.Add(this.btnRemove);
            this.gbCameraReaders.Controls.Add(this.btnAdd);
            this.gbCameraReaders.Controls.Add(this.lvSelReaders);
            this.gbCameraReaders.Controls.Add(this.lvNotSelReaders);
            this.gbCameraReaders.Location = new System.Drawing.Point(10, 170);
            this.gbCameraReaders.Name = "gbCameraReaders";
            this.gbCameraReaders.Size = new System.Drawing.Size(672, 380);
            this.gbCameraReaders.TabIndex = 19;
            this.gbCameraReaders.TabStop = false;
            this.gbCameraReaders.Text = "Camera <-> Reader <-> Direction";
            // 
            // lvDirection
            // 
            this.lvDirection.FullRowSelect = true;
            this.lvDirection.GridLines = true;
            this.lvDirection.HideSelection = false;
            this.lvDirection.Location = new System.Drawing.Point(18, 264);
            this.lvDirection.MultiSelect = false;
            this.lvDirection.Name = "lvDirection";
            this.lvDirection.Size = new System.Drawing.Size(270, 65);
            this.lvDirection.TabIndex = 6;
            this.lvDirection.UseCompatibleStateImageBehavior = false;
            this.lvDirection.View = System.Windows.Forms.View.Details;
            // 
            // btnDirectionAdd
            // 
            this.btnDirectionAdd.Location = new System.Drawing.Point(302, 290);
            this.btnDirectionAdd.Name = "btnDirectionAdd";
            this.btnDirectionAdd.Size = new System.Drawing.Size(48, 23);
            this.btnDirectionAdd.TabIndex = 7;
            this.btnDirectionAdd.Text = ">";
            this.btnDirectionAdd.Click += new System.EventHandler(this.btnDirectionAdd_Click);
            // 
            // lblSelReaders
            // 
            this.lblSelReaders.Location = new System.Drawing.Point(364, 16);
            this.lblSelReaders.Name = "lblSelReaders";
            this.lblSelReaders.Size = new System.Drawing.Size(290, 35);
            this.lblSelReaders.TabIndex = 4;
            this.lblSelReaders.Text = "List of all readers assigned to camera:";
            this.lblSelReaders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNotSelReaders
            // 
            this.lblNotSelReaders.Location = new System.Drawing.Point(18, 16);
            this.lblNotSelReaders.Name = "lblNotSelReaders";
            this.lblNotSelReaders.Size = new System.Drawing.Size(270, 35);
            this.lblNotSelReaders.TabIndex = 1;
            this.lblNotSelReaders.Text = "List of all readers not assigned to camera:";
            this.lblNotSelReaders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(449, 340);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(120, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove selected";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(302, 141);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = ">";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvSelReaders
            // 
            this.lvSelReaders.FullRowSelect = true;
            this.lvSelReaders.GridLines = true;
            this.lvSelReaders.HideSelection = false;
            this.lvSelReaders.Location = new System.Drawing.Point(364, 54);
            this.lvSelReaders.Name = "lvSelReaders";
            this.lvSelReaders.Size = new System.Drawing.Size(290, 275);
            this.lvSelReaders.TabIndex = 5;
            this.lvSelReaders.UseCompatibleStateImageBehavior = false;
            this.lvSelReaders.View = System.Windows.Forms.View.Details;
            this.lvSelReaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSelReaders_ColumnClick);
            // 
            // lvNotSelReaders
            // 
            this.lvNotSelReaders.FullRowSelect = true;
            this.lvNotSelReaders.GridLines = true;
            this.lvNotSelReaders.HideSelection = false;
            this.lvNotSelReaders.Location = new System.Drawing.Point(18, 54);
            this.lvNotSelReaders.Name = "lvNotSelReaders";
            this.lvNotSelReaders.Size = new System.Drawing.Size(270, 192);
            this.lvNotSelReaders.TabIndex = 2;
            this.lvNotSelReaders.UseCompatibleStateImageBehavior = false;
            this.lvNotSelReaders.View = System.Windows.Forms.View.Details;
            this.lvNotSelReaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvNotSelReaders_ColumnClick);
            // 
            // tbType
            // 
            this.tbType.Location = new System.Drawing.Point(106, 114);
            this.tbType.MaxLength = 100;
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(152, 20);
            this.tbType.TabIndex = 14;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(18, 112);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(80, 23);
            this.lblType.TabIndex = 13;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIP
            // 
            this.lblIP.Location = new System.Drawing.Point(18, 80);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(80, 23);
            this.lblIP.TabIndex = 5;
            this.lblIP.Text = "Address:";
            this.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(266, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(266, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "*";
            // 
            // lblDot2
            // 
            this.lblDot2.Location = new System.Drawing.Point(218, 83);
            this.lblDot2.Name = "lblDot2";
            this.lblDot2.Size = new System.Drawing.Size(8, 23);
            this.lblDot2.TabIndex = 11;
            this.lblDot2.Text = ".";
            this.lblDot2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblDot1
            // 
            this.lblDot1.Location = new System.Drawing.Point(178, 83);
            this.lblDot1.Name = "lblDot1";
            this.lblDot1.Size = new System.Drawing.Size(8, 23);
            this.lblDot1.TabIndex = 9;
            this.lblDot1.Text = ".";
            this.lblDot1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblDot0
            // 
            this.lblDot0.Location = new System.Drawing.Point(138, 83);
            this.lblDot0.Name = "lblDot0";
            this.lblDot0.Size = new System.Drawing.Size(8, 23);
            this.lblDot0.TabIndex = 7;
            this.lblDot0.Text = ".";
            this.lblDot0.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tbIP2
            // 
            this.tbIP2.Location = new System.Drawing.Point(186, 82);
            this.tbIP2.MaxLength = 3;
            this.tbIP2.Name = "tbIP2";
            this.tbIP2.Size = new System.Drawing.Size(32, 20);
            this.tbIP2.TabIndex = 10;
            this.tbIP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbIP1
            // 
            this.tbIP1.Location = new System.Drawing.Point(146, 82);
            this.tbIP1.MaxLength = 3;
            this.tbIP1.Name = "tbIP1";
            this.tbIP1.Size = new System.Drawing.Size(32, 20);
            this.tbIP1.TabIndex = 8;
            this.tbIP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbIP0
            // 
            this.tbIP0.Location = new System.Drawing.Point(106, 82);
            this.tbIP0.MaxLength = 3;
            this.tbIP0.Name = "tbIP0";
            this.tbIP0.Size = new System.Drawing.Size(32, 20);
            this.tbIP0.TabIndex = 6;
            this.tbIP0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(266, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "*";
            // 
            // tbIP3
            // 
            this.tbIP3.Location = new System.Drawing.Point(226, 82);
            this.tbIP3.MaxLength = 3;
            this.tbIP3.Name = "tbIP3";
            this.tbIP3.Size = new System.Drawing.Size(32, 20);
            this.tbIP3.TabIndex = 12;
            this.tbIP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbCamera
            // 
            this.gbCamera.Controls.Add(this.tbDesc);
            this.gbCamera.Controls.Add(this.lblDot2);
            this.gbCamera.Controls.Add(this.lblCameraID);
            this.gbCamera.Controls.Add(this.lblDot1);
            this.gbCamera.Controls.Add(this.lblDesc);
            this.gbCamera.Controls.Add(this.lblDot0);
            this.gbCamera.Controls.Add(this.tbCameraID);
            this.gbCamera.Controls.Add(this.tbIP2);
            this.gbCamera.Controls.Add(this.label1);
            this.gbCamera.Controls.Add(this.tbIP1);
            this.gbCamera.Controls.Add(this.lblIP);
            this.gbCamera.Controls.Add(this.tbIP0);
            this.gbCamera.Controls.Add(this.lblType);
            this.gbCamera.Controls.Add(this.label3);
            this.gbCamera.Controls.Add(this.tbType);
            this.gbCamera.Controls.Add(this.tbIP3);
            this.gbCamera.Controls.Add(this.label2);
            this.gbCamera.Controls.Add(this.label4);
            this.gbCamera.Location = new System.Drawing.Point(10, 8);
            this.gbCamera.Name = "gbCamera";
            this.gbCamera.Size = new System.Drawing.Size(672, 148);
            this.gbCamera.TabIndex = 1;
            this.gbCamera.TabStop = false;
            this.gbCamera.Text = "Camera";
            // 
            // CameraAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 586);
            this.ControlBox = false;
            this.Controls.Add(this.gbCamera);
            this.Controls.Add(this.gbCameraReaders);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 620);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 620);
            this.Name = "CameraAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "CameraAdd";
            this.Load += new System.EventHandler(this.CameraAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CameraAdd_KeyUp);
            this.gbCameraReaders.ResumeLayout(false);
            this.gbCamera.ResumeLayout(false);
            this.gbCamera.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.TextBox tbCameraID;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblCameraID;
        private System.Windows.Forms.GroupBox gbCameraReaders;
        private System.Windows.Forms.Label lblSelReaders;
        private System.Windows.Forms.Label lblNotSelReaders;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvSelReaders;
        private System.Windows.Forms.ListView lvNotSelReaders;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDot2;
        private System.Windows.Forms.Label lblDot1;
        private System.Windows.Forms.Label lblDot0;
        private System.Windows.Forms.TextBox tbIP2;
        private System.Windows.Forms.TextBox tbIP1;
        private System.Windows.Forms.TextBox tbIP0;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbIP3;
        private System.Windows.Forms.Button btnDirectionAdd;
        private System.Windows.Forms.ListView lvDirection;
        private System.Windows.Forms.GroupBox gbCamera;
    }
}