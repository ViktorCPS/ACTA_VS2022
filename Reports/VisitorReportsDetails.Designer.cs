namespace Reports
{
    partial class VisitorReportsDetails
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
            this.lvIOPairs = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblVisitor = new System.Windows.Forms.Label();
            this.lblVisitorText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvIOPairs
            // 
            this.lvIOPairs.FullRowSelect = true;
            this.lvIOPairs.GridLines = true;
            this.lvIOPairs.HideSelection = false;
            this.lvIOPairs.Location = new System.Drawing.Point(19, 45);
            this.lvIOPairs.MultiSelect = false;
            this.lvIOPairs.Name = "lvIOPairs";
            this.lvIOPairs.Size = new System.Drawing.Size(584, 300);
            this.lvIOPairs.TabIndex = 3;
            this.lvIOPairs.UseCompatibleStateImageBehavior = false;
            this.lvIOPairs.View = System.Windows.Forms.View.Details;
            this.lvIOPairs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvIOPairs_ColumnClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(528, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblVisitor
            // 
            this.lblVisitor.Location = new System.Drawing.Point(16, 9);
            this.lblVisitor.Name = "lblVisitor";
            this.lblVisitor.Size = new System.Drawing.Size(104, 23);
            this.lblVisitor.TabIndex = 1;
            this.lblVisitor.Text = "Visitor:";
            this.lblVisitor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVisitorText
            // 
            this.lblVisitorText.Location = new System.Drawing.Point(126, 9);
            this.lblVisitorText.Name = "lblVisitorText";
            this.lblVisitorText.Size = new System.Drawing.Size(477, 23);
            this.lblVisitorText.TabIndex = 2;
            this.lblVisitorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VisitorReportsDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 390);
            this.ControlBox = false;
            this.Controls.Add(this.lblVisitorText);
            this.Controls.Add(this.lblVisitor);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvIOPairs);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 424);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(624, 424);
            this.Name = "VisitorReportsDetails";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Visit details";
            this.Load += new System.EventHandler(this.VisitorReportsDetails_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VisitorReportsDetails_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvIOPairs;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblVisitor;
        private System.Windows.Forms.Label lblVisitorText;
    }
}