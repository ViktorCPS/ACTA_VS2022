namespace UI
{
    partial class OnlineMealPointAdd
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblTerminalSerial = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tbMealPointID = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMealPointID = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbMealPoint = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.cbMealType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblMealType = new System.Windows.Forms.Label();
            this.cbRestaurant = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPeripherial = new System.Windows.Forms.TextBox();
            this.lblPreipherial = new System.Windows.Forms.Label();
            this.tbIPAddress = new System.Windows.Forms.TextBox();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.tbPeripherialDesc = new System.Windows.Forms.TextBox();
            this.lblPeripherialDesc = new System.Windows.Forms.Label();
            this.tbAntenna = new System.Windows.Forms.TextBox();
            this.lblAntenna = new System.Windows.Forms.Label();
            this.gbMealPoint.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(327, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "*";
            // 
            // lblTerminalSerial
            // 
            this.lblTerminalSerial.Location = new System.Drawing.Point(12, 117);
            this.lblTerminalSerial.Name = "lblTerminalSerial";
            this.lblTerminalSerial.Size = new System.Drawing.Size(141, 13);
            this.lblTerminalSerial.TabIndex = 3;
            this.lblTerminalSerial.Text = "Restaurant: ";
            this.lblTerminalSerial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 332);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // tbMealPointID
            // 
            this.tbMealPointID.Location = new System.Drawing.Point(156, 27);
            this.tbMealPointID.Name = "tbMealPointID";
            this.tbMealPointID.Size = new System.Drawing.Size(100, 20);
            this.tbMealPointID.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(302, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMealPointID
            // 
            this.lblMealPointID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMealPointID.Location = new System.Drawing.Point(6, 30);
            this.lblMealPointID.Name = "lblMealPointID";
            this.lblMealPointID.Size = new System.Drawing.Size(144, 13);
            this.lblMealPointID.TabIndex = 1;
            this.lblMealPointID.Text = "Line ID:";
            this.lblMealPointID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 332);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbMealPoint
            // 
            this.gbMealPoint.Controls.Add(this.label2);
            this.gbMealPoint.Controls.Add(this.tbDescription);
            this.gbMealPoint.Controls.Add(this.lblDescription);
            this.gbMealPoint.Controls.Add(this.tbName);
            this.gbMealPoint.Controls.Add(this.lblName);
            this.gbMealPoint.Controls.Add(this.cbMealType);
            this.gbMealPoint.Controls.Add(this.label6);
            this.gbMealPoint.Controls.Add(this.lblMealType);
            this.gbMealPoint.Controls.Add(this.cbRestaurant);
            this.gbMealPoint.Controls.Add(this.label5);
            this.gbMealPoint.Controls.Add(this.label4);
            this.gbMealPoint.Controls.Add(this.label3);
            this.gbMealPoint.Controls.Add(this.tbPeripherial);
            this.gbMealPoint.Controls.Add(this.lblPreipherial);
            this.gbMealPoint.Controls.Add(this.tbIPAddress);
            this.gbMealPoint.Controls.Add(this.lblIPAddress);
            this.gbMealPoint.Controls.Add(this.tbPeripherialDesc);
            this.gbMealPoint.Controls.Add(this.lblPeripherialDesc);
            this.gbMealPoint.Controls.Add(this.tbAntenna);
            this.gbMealPoint.Controls.Add(this.lblAntenna);
            this.gbMealPoint.Controls.Add(this.label1);
            this.gbMealPoint.Controls.Add(this.lblTerminalSerial);
            this.gbMealPoint.Controls.Add(this.tbMealPointID);
            this.gbMealPoint.Controls.Add(this.lblMealPointID);
            this.gbMealPoint.Location = new System.Drawing.Point(12, 12);
            this.gbMealPoint.Name = "gbMealPoint";
            this.gbMealPoint.Size = new System.Drawing.Size(365, 290);
            this.gbMealPoint.TabIndex = 11;
            this.gbMealPoint.TabStop = false;
            this.gbMealPoint.Text = "Meal point";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(327, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 42;
            this.label2.Text = "*";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(156, 85);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
            this.tbDescription.TabIndex = 41;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(12, 88);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(138, 13);
            this.lblDescription.TabIndex = 40;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(156, 53);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 39;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(9, 56);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(138, 13);
            this.lblName.TabIndex = 38;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMealType
            // 
            this.cbMealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMealType.FormattingEnabled = true;
            this.cbMealType.Location = new System.Drawing.Point(156, 141);
            this.cbMealType.Name = "cbMealType";
            this.cbMealType.Size = new System.Drawing.Size(165, 21);
            this.cbMealType.TabIndex = 37;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(327, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 36;
            this.label6.Text = "*";
            // 
            // lblMealType
            // 
            this.lblMealType.Location = new System.Drawing.Point(23, 141);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(127, 13);
            this.lblMealType.TabIndex = 35;
            this.lblMealType.Text = "Meal type:";
            this.lblMealType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbRestaurant
            // 
            this.cbRestaurant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRestaurant.FormattingEnabled = true;
            this.cbRestaurant.Location = new System.Drawing.Point(156, 114);
            this.cbRestaurant.Name = "cbRestaurant";
            this.cbRestaurant.Size = new System.Drawing.Size(165, 21);
            this.cbRestaurant.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(327, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 29;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(327, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 28;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(327, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 27;
            this.label3.Text = "*";
            // 
            // tbPeripherial
            // 
            this.tbPeripherial.Location = new System.Drawing.Point(156, 225);
            this.tbPeripherial.Name = "tbPeripherial";
            this.tbPeripherial.Size = new System.Drawing.Size(165, 20);
            this.tbPeripherial.TabIndex = 26;
            // 
            // lblPreipherial
            // 
            this.lblPreipherial.Location = new System.Drawing.Point(23, 228);
            this.lblPreipherial.Name = "lblPreipherial";
            this.lblPreipherial.Size = new System.Drawing.Size(127, 13);
            this.lblPreipherial.TabIndex = 25;
            this.lblPreipherial.Text = "Peripherial:";
            this.lblPreipherial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbIPAddress
            // 
            this.tbIPAddress.Location = new System.Drawing.Point(156, 168);
            this.tbIPAddress.Name = "tbIPAddress";
            this.tbIPAddress.Size = new System.Drawing.Size(165, 20);
            this.tbIPAddress.TabIndex = 24;
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.Location = new System.Drawing.Point(23, 171);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(127, 13);
            this.lblIPAddress.TabIndex = 23;
            this.lblIPAddress.Text = "IP address:";
            this.lblIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPeripherialDesc
            // 
            this.tbPeripherialDesc.Location = new System.Drawing.Point(156, 254);
            this.tbPeripherialDesc.Name = "tbPeripherialDesc";
            this.tbPeripherialDesc.Size = new System.Drawing.Size(165, 20);
            this.tbPeripherialDesc.TabIndex = 22;
            // 
            // lblPeripherialDesc
            // 
            this.lblPeripherialDesc.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPeripherialDesc.Location = new System.Drawing.Point(6, 257);
            this.lblPeripherialDesc.Name = "lblPeripherialDesc";
            this.lblPeripherialDesc.Size = new System.Drawing.Size(144, 13);
            this.lblPeripherialDesc.TabIndex = 21;
            this.lblPeripherialDesc.Text = "Peripherial description:";
            this.lblPeripherialDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAntenna
            // 
            this.tbAntenna.Location = new System.Drawing.Point(156, 196);
            this.tbAntenna.Name = "tbAntenna";
            this.tbAntenna.Size = new System.Drawing.Size(165, 20);
            this.tbAntenna.TabIndex = 20;
            // 
            // lblAntenna
            // 
            this.lblAntenna.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAntenna.Location = new System.Drawing.Point(6, 199);
            this.lblAntenna.Name = "lblAntenna";
            this.lblAntenna.Size = new System.Drawing.Size(144, 13);
            this.lblAntenna.TabIndex = 19;
            this.lblAntenna.Text = "Reader antenna:";
            this.lblAntenna.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OnlineMealPointAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 379);
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMealPoint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OnlineMealPointAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OnlineMealPointAdd";
            this.gbMealPoint.ResumeLayout(false);
            this.gbMealPoint.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTerminalSerial;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox tbMealPointID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMealPointID;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbMealPoint;
        private System.Windows.Forms.TextBox tbPeripherialDesc;
        private System.Windows.Forms.Label lblPeripherialDesc;
        private System.Windows.Forms.TextBox tbAntenna;
        private System.Windows.Forms.Label lblAntenna;
        private System.Windows.Forms.TextBox tbPeripherial;
        private System.Windows.Forms.Label lblPreipherial;
        private System.Windows.Forms.TextBox tbIPAddress;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRestaurant;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbMealType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblMealType;
    }
}