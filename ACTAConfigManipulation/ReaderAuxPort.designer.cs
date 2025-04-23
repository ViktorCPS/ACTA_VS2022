namespace ACTAConfigManipulation
{
    partial class ReaderAuxPort
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
            this.cbAuxPort = new System.Windows.Forms.ComboBox();
            this.lblReader = new System.Windows.Forms.Label();
            this.toolTipReader = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // cbAuxPort
            // 
            this.cbAuxPort.BackColor = System.Drawing.Color.Lavender;
            this.cbAuxPort.DropDownWidth = 60;
            this.cbAuxPort.FormattingEnabled = true;
            this.cbAuxPort.Location = new System.Drawing.Point(0, 0);
            this.cbAuxPort.MaxDropDownItems = 20;
            this.cbAuxPort.Name = "cbAuxPort";
            this.cbAuxPort.Size = new System.Drawing.Size(60, 21);
            this.cbAuxPort.TabIndex = 1;
            this.cbAuxPort.SelectedIndexChanged += new System.EventHandler(this.cbAuxPort_SelectedIndexChanged);
            // 
            // lblReader
            // 
            this.lblReader.BackColor = System.Drawing.Color.Lavender;
            this.lblReader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReader.Location = new System.Drawing.Point(0, 0);
            this.lblReader.Name = "lblReader";
            this.lblReader.Size = new System.Drawing.Size(120, 25);
            this.lblReader.TabIndex = 0;
            this.lblReader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblReader.MouseHover += new System.EventHandler(this.lblReader_MouseHover);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbAuxPort;
        private System.Windows.Forms.Label lblReader;
        private System.Windows.Forms.ToolTip toolTipReader;
    }
}
