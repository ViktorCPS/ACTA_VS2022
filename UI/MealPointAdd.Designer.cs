namespace UI
{
    partial class MealPointAdd
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbMealPoint = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTerminalSerial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTerminalSerial = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealPointID = new System.Windows.Forms.TextBox();
            this.lblMealPointID = new System.Windows.Forms.Label();
            this.gbMealPoint.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 201);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(302, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 201);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbMealPoint
            // 
            this.gbMealPoint.Controls.Add(this.label2);
            this.gbMealPoint.Controls.Add(this.tbTerminalSerial);
            this.gbMealPoint.Controls.Add(this.label1);
            this.gbMealPoint.Controls.Add(this.lblTerminalSerial);
            this.gbMealPoint.Controls.Add(this.tbDescription);
            this.gbMealPoint.Controls.Add(this.lblDescription);
            this.gbMealPoint.Controls.Add(this.tbName);
            this.gbMealPoint.Controls.Add(this.lblName);
            this.gbMealPoint.Controls.Add(this.tbMealPointID);
            this.gbMealPoint.Controls.Add(this.lblMealPointID);
            this.gbMealPoint.Location = new System.Drawing.Point(12, 12);
            this.gbMealPoint.Name = "gbMealPoint";
            this.gbMealPoint.Size = new System.Drawing.Size(365, 183);
            this.gbMealPoint.TabIndex = 0;
            this.gbMealPoint.TabStop = false;
            this.gbMealPoint.Text = "Meal point";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(327, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "*";
            // 
            // tbTerminalSerial
            // 
            this.tbTerminalSerial.Location = new System.Drawing.Point(156, 66);
            this.tbTerminalSerial.Name = "tbTerminalSerial";
            this.tbTerminalSerial.Size = new System.Drawing.Size(165, 20);
            this.tbTerminalSerial.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(327, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "*";
            // 
            // lblTerminalSerial
            // 
            this.lblTerminalSerial.Location = new System.Drawing.Point(9, 69);
            this.lblTerminalSerial.Name = "lblTerminalSerial";
            this.lblTerminalSerial.Size = new System.Drawing.Size(141, 13);
            this.lblTerminalSerial.TabIndex = 3;
            this.lblTerminalSerial.Text = "Terminal serial:";
            this.lblTerminalSerial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(156, 130);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
            this.tbDescription.TabIndex = 8;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(12, 133);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(138, 13);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(156, 98);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(9, 101);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(138, 13);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMealPointID
            // 
            this.tbMealPointID.Location = new System.Drawing.Point(156, 27);
            this.tbMealPointID.Name = "tbMealPointID";
            this.tbMealPointID.Size = new System.Drawing.Size(100, 20);
            this.tbMealPointID.TabIndex = 2;
            // 
            // lblMealPointID
            // 
            this.lblMealPointID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMealPointID.Location = new System.Drawing.Point(6, 30);
            this.lblMealPointID.Name = "lblMealPointID";
            this.lblMealPointID.Size = new System.Drawing.Size(144, 13);
            this.lblMealPointID.TabIndex = 1;
            this.lblMealPointID.Text = "Meal point ID:";
            this.lblMealPointID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MealPointAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 236);
            this.KeyPreview = true;
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMealPoint);
            this.Name = "MealPointAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MealPointAdd";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MealPointAdd_KeyUp);
            this.gbMealPoint.ResumeLayout(false);
            this.gbMealPoint.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbMealPoint;
        private System.Windows.Forms.TextBox tbTerminalSerial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealPointID;
        private System.Windows.Forms.Label lblMealPointID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTerminalSerial;
    }
}