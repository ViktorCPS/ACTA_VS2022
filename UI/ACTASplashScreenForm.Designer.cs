namespace UI
{
    partial class ACTASplashScreenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ACTASplashScreenForm));
            this._splashPicBox = new System.Windows.Forms.PictureBox();
            this._activityLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._splashPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _splashPicBox
            // 
            resources.ApplyResources(this._splashPicBox, "_splashPicBox");
            this._splashPicBox.Name = "_splashPicBox";
            this._splashPicBox.TabStop = false;
            // 
            // _activityLabel
            // 
            resources.ApplyResources(this._activityLabel, "_activityLabel");
            this._activityLabel.BackColor = System.Drawing.Color.White;
            this._activityLabel.Name = "_activityLabel";
            // 
            // ACTASplashScreenForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this._activityLabel);
            this.Controls.Add(this._splashPicBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ACTASplashScreenForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this._splashPicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _splashPicBox;
        private System.Windows.Forms.Label _activityLabel;
    }
}