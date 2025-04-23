namespace UI
{
    partial class EmployeeAdditionalData
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbData = new System.Windows.Forms.GroupBox();
            this.panelTextData = new System.Windows.Forms.Panel();
            this.panelDateTimeData = new System.Windows.Forms.Panel();
            this.panelIntData = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbData.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(771, 444);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 444);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.panelTextData);
            this.gbData.Controls.Add(this.panelDateTimeData);
            this.gbData.Controls.Add(this.panelIntData);
            this.gbData.Location = new System.Drawing.Point(12, 12);
            this.gbData.Name = "gbData";
            this.gbData.Size = new System.Drawing.Size(834, 402);
            this.gbData.TabIndex = 0;
            this.gbData.TabStop = false;
            this.gbData.Text = "Additional data";
            // 
            // panelTextData
            // 
            this.panelTextData.Location = new System.Drawing.Point(492, 19);
            this.panelTextData.Name = "panelTextData";
            this.panelTextData.Size = new System.Drawing.Size(330, 370);
            this.panelTextData.TabIndex = 2;
            // 
            // panelDateTimeData
            // 
            this.panelDateTimeData.Location = new System.Drawing.Point(226, 19);
            this.panelDateTimeData.Name = "panelDateTimeData";
            this.panelDateTimeData.Size = new System.Drawing.Size(260, 370);
            this.panelDateTimeData.TabIndex = 1;
            // 
            // panelIntData
            // 
            this.panelIntData.Location = new System.Drawing.Point(10, 19);
            this.panelIntData.Name = "panelIntData";
            this.panelIntData.Size = new System.Drawing.Size(210, 370);
            this.panelIntData.TabIndex = 0;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(12, 417);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(344, 13);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "* -1 is undefined numeric value, 01.01.1900. is undefined dat time value";
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // EmployeeAdditionalData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 493);
            this.ControlBox = false;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.gbData);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EmployeeAdditionalData";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save";
            this.Load += new System.EventHandler(this.EmployeeAdditionalData_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EmployeeAdditionalData_FormClosed);
            this.gbData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbData;
        private System.Windows.Forms.Panel panelIntData;
        private System.Windows.Forms.Panel panelDateTimeData;
        private System.Windows.Forms.Panel panelTextData;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}