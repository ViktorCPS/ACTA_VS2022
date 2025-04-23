namespace ACTASelftService
{
    partial class SelfServMain
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
            this.lblNote = new System.Windows.Forms.Label();
            this.timerReadingTag = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(31, 33);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(426, 147);
            this.lblNote.TabIndex = 0;
            this.lblNote.Text = "NOTE";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerReadingTag
            // 
            this.timerReadingTag.Enabled = true;
            this.timerReadingTag.Interval = 500;
            this.timerReadingTag.Tick += new System.EventHandler(this.timerReadingTag_Tick);
            // 
            // SelfServMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 218);
            this.Controls.Add(this.lblNote);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SelfServMain";
            this.ShowIcon = false;
            this.Text = "CheckIn";
            this.Load += new System.EventHandler(this.CheckInForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelfServMain_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Timer timerReadingTag;
    }
}

