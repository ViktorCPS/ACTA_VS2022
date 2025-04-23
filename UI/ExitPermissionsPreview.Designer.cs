namespace UI
{
    partial class ExitPermissionsPreview
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.exitPermPanel = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(902, 659);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // exitPermPanel
            // 
            this.exitPermPanel.AutoScroll = true;
            this.exitPermPanel.Location = new System.Drawing.Point(12, 12);
            this.exitPermPanel.Name = "exitPermPanel";
            this.exitPermPanel.Size = new System.Drawing.Size(965, 626);
            this.exitPermPanel.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 659);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ExitPermissionsPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 694);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.exitPermPanel);
            this.Controls.Add(this.btnCancel);
            this.KeyPreview = true;
            this.Name = "ExitPermissionsPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ExitPermissionsPreview";
            this.Load += new System.EventHandler(this.ExitPermissionsPreview_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExitPermissionsPreview_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ExitPermissionsPreview_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel exitPermPanel;
        private System.Windows.Forms.Button btnSave;
    }
}