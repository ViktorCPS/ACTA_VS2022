namespace ACTAConfigManipulation
{
    partial class GatesAuxPorts
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
            this.components = new System.ComponentModel.Container();
            this.lblID = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.cbAuxPort = new System.Windows.Forms.CheckBox();
            this.cbGateSel = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.BackColor = System.Drawing.Color.Lavender;
            this.lblID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblID.Location = new System.Drawing.Point(0, 0);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(45, 30);
            this.lblID.TabIndex = 1;
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Lavender;
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 30);
            this.lblName.TabIndex = 2;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblName.MouseHover += new System.EventHandler(this.lblName_MouseHover);
            // 
            // lblDesc
            // 
            this.lblDesc.BackColor = System.Drawing.Color.Lavender;
            this.lblDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDesc.Location = new System.Drawing.Point(0, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(100, 30);
            this.lblDesc.TabIndex = 3;
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDesc.MouseHover += new System.EventHandler(this.lblDesc_MouseHover);
            // 
            // cbAuxPort
            // 
            this.cbAuxPort.BackColor = System.Drawing.Color.Lavender;
            this.cbAuxPort.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAuxPort.Location = new System.Drawing.Point(0, 0);
            this.cbAuxPort.Name = "cbAuxPort";
            this.cbAuxPort.Size = new System.Drawing.Size(50, 30);
            this.cbAuxPort.TabIndex = 4;
            this.cbAuxPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAuxPort.UseVisualStyleBackColor = false;
            this.cbAuxPort.CheckedChanged += new System.EventHandler(this.cbAuxPort_CheckedChanged);
            // 
            // cbGateSel
            // 
            this.cbGateSel.BackColor = System.Drawing.Color.Lavender;
            this.cbGateSel.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbGateSel.Location = new System.Drawing.Point(0, 0);
            this.cbGateSel.Name = "cbGateSel";
            this.cbGateSel.Size = new System.Drawing.Size(30, 30);
            this.cbGateSel.TabIndex = 0;
            this.cbGateSel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbGateSel.UseVisualStyleBackColor = false;
            this.cbGateSel.CheckedChanged += new System.EventHandler(this.cbGateSel_CheckedChanged);
            // 
            // GatesAuxPorts
            // 
            this.Size = new System.Drawing.Size(100, 50);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.CheckBox cbAuxPort;
        private System.Windows.Forms.CheckBox cbGateSel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
