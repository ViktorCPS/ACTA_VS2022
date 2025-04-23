namespace UI
{
    partial class ExitPermControl
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
            this.gbExitPerm = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // gbExitPerm
            // 
            this.gbExitPerm.Location = new System.Drawing.Point(3, 0);
            this.gbExitPerm.Name = "gbExitPerm";
            this.gbExitPerm.Size = new System.Drawing.Size(959, 32);
            this.gbExitPerm.TabIndex = 1;
            this.gbExitPerm.TabStop = false;
            // 
            // ExitPermControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbExitPerm);
            this.Name = "ExitPermControl";
            this.Size = new System.Drawing.Size(940, 36);
            this.SizeChanged += new System.EventHandler(this.ExitPermControl_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbExitPerm;
    }
}
