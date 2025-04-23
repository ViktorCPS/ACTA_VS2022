namespace SiemensUI
{
    partial class SiemensDevicesControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.chbReadPasses = new System.Windows.Forms.CheckBox();
            this.chTimeAttCounter = new System.Windows.Forms.CheckBox();
            this.gbDirection = new System.Windows.Forms.GroupBox();
            this.rbOut = new System.Windows.Forms.RadioButton();
            this.rbIn = new System.Windows.Forms.RadioButton();
            this.gbPointName = new System.Windows.Forms.GroupBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.gbBrezaGate = new System.Windows.Forms.GroupBox();
            this.nudBrezaGate = new System.Windows.Forms.NumericUpDown();
            this.gbDevice.SuspendLayout();
            this.gbDirection.SuspendLayout();
            this.gbPointName.SuspendLayout();
            this.gbBrezaGate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBrezaGate)).BeginInit();
            this.SuspendLayout();
            // 
            // gbDevice
            // 
            this.gbDevice.Controls.Add(this.gbBrezaGate);
            this.gbDevice.Controls.Add(this.chbReadPasses);
            this.gbDevice.Controls.Add(this.chTimeAttCounter);
            this.gbDevice.Controls.Add(this.gbDirection);
            this.gbDevice.Controls.Add(this.gbPointName);
            this.gbDevice.Location = new System.Drawing.Point(3, 3);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Size = new System.Drawing.Size(921, 106);
            this.gbDevice.TabIndex = 0;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Devices";
            // 
            // chbReadPasses
            // 
            this.chbReadPasses.AutoSize = true;
            this.chbReadPasses.Location = new System.Drawing.Point(6, 19);
            this.chbReadPasses.Name = "chbReadPasses";
            this.chbReadPasses.Size = new System.Drawing.Size(146, 17);
            this.chbReadPasses.TabIndex = 4;
            this.chbReadPasses.Text = "Read passes from device";
            this.chbReadPasses.UseVisualStyleBackColor = true;
            this.chbReadPasses.CheckedChanged += new System.EventHandler(this.chbReadPasses_CheckedChanged);
            // 
            // chTimeAttCounter
            // 
            this.chTimeAttCounter.AutoSize = true;
            this.chTimeAttCounter.Location = new System.Drawing.Point(373, 16);
            this.chTimeAttCounter.Name = "chTimeAttCounter";
            this.chTimeAttCounter.Size = new System.Drawing.Size(145, 17);
            this.chTimeAttCounter.TabIndex = 3;
            this.chTimeAttCounter.Text = "Time attendance counter";
            this.chTimeAttCounter.UseVisualStyleBackColor = true;
            this.chTimeAttCounter.CheckedChanged += new System.EventHandler(this.chTimeAttCounter_CheckedChanged);
            // 
            // gbDirection
            // 
            this.gbDirection.Controls.Add(this.rbOut);
            this.gbDirection.Controls.Add(this.rbIn);
            this.gbDirection.Location = new System.Drawing.Point(372, 39);
            this.gbDirection.Name = "gbDirection";
            this.gbDirection.Size = new System.Drawing.Size(177, 56);
            this.gbDirection.TabIndex = 1;
            this.gbDirection.TabStop = false;
            this.gbDirection.Text = "Direction";
            // 
            // rbOut
            // 
            this.rbOut.AutoSize = true;
            this.rbOut.Location = new System.Drawing.Point(101, 19);
            this.rbOut.Name = "rbOut";
            this.rbOut.Size = new System.Drawing.Size(48, 17);
            this.rbOut.TabIndex = 1;
            this.rbOut.TabStop = true;
            this.rbOut.Text = "OUT";
            this.rbOut.UseVisualStyleBackColor = true;
            // 
            // rbIn
            // 
            this.rbIn.AutoSize = true;
            this.rbIn.Checked = true;
            this.rbIn.Location = new System.Drawing.Point(29, 20);
            this.rbIn.Name = "rbIn";
            this.rbIn.Size = new System.Drawing.Size(36, 17);
            this.rbIn.TabIndex = 0;
            this.rbIn.TabStop = true;
            this.rbIn.Text = "IN";
            this.rbIn.UseVisualStyleBackColor = true;
            this.rbIn.CheckedChanged += new System.EventHandler(this.rbIn_CheckedChanged);
            // 
            // gbPointName
            // 
            this.gbPointName.Controls.Add(this.tbName);
            this.gbPointName.Location = new System.Drawing.Point(6, 39);
            this.gbPointName.Name = "gbPointName";
            this.gbPointName.Size = new System.Drawing.Size(360, 56);
            this.gbPointName.TabIndex = 0;
            this.gbPointName.TabStop = false;
            this.gbPointName.Text = "Point name";
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(6, 20);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(348, 20);
            this.tbName.TabIndex = 2;
            // 
            // gbBrezaGate
            // 
            this.gbBrezaGate.Controls.Add(this.nudBrezaGate);
            this.gbBrezaGate.Location = new System.Drawing.Point(555, 39);
            this.gbBrezaGate.Name = "gbBrezaGate";
            this.gbBrezaGate.Size = new System.Drawing.Size(360, 56);
            this.gbBrezaGate.TabIndex = 5;
            this.gbBrezaGate.TabStop = false;
            this.gbBrezaGate.Text = "Breza gate";
            // 
            // nudBrezaGate
            // 
            this.nudBrezaGate.Location = new System.Drawing.Point(6, 21);
            this.nudBrezaGate.Name = "nudBrezaGate";
            this.nudBrezaGate.Size = new System.Drawing.Size(348, 20);
            this.nudBrezaGate.TabIndex = 0;
            this.nudBrezaGate.ValueChanged += new System.EventHandler(this.nudBrezaGate_ValueChanged);
            // 
            // SiemensDevicesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDevice);
            this.Name = "SiemensDevicesControl";
            this.Size = new System.Drawing.Size(928, 113);
            this.Load += new System.EventHandler(this.SiemensDevicesControl_Load);
            this.gbDevice.ResumeLayout(false);
            this.gbDevice.PerformLayout();
            this.gbDirection.ResumeLayout(false);
            this.gbDirection.PerformLayout();
            this.gbPointName.ResumeLayout(false);
            this.gbPointName.PerformLayout();
            this.gbBrezaGate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudBrezaGate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.GroupBox gbPointName;
        private System.Windows.Forms.GroupBox gbDirection;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.RadioButton rbOut;
        private System.Windows.Forms.RadioButton rbIn;
        private System.Windows.Forms.CheckBox chTimeAttCounter;
        private System.Windows.Forms.CheckBox chbReadPasses;
        private System.Windows.Forms.GroupBox gbBrezaGate;
        private System.Windows.Forms.NumericUpDown nudBrezaGate;
    }
}
