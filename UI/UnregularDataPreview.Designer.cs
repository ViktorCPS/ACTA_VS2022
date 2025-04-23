namespace UI
{
    partial class UnregularDataPreview
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
            this.lvPreview = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportPreview = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvPreview
            // 
            this.lvPreview.FullRowSelect = true;
            this.lvPreview.GridLines = true;
            this.lvPreview.HideSelection = false;
            this.lvPreview.Location = new System.Drawing.Point(12, 12);
            this.lvPreview.Name = "lvPreview";
            this.lvPreview.ShowItemToolTips = true;
            this.lvPreview.Size = new System.Drawing.Size(620, 345);
            this.lvPreview.TabIndex = 0;
            this.lvPreview.UseCompatibleStateImageBehavior = false;
            this.lvPreview.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(557, 363);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportPreview
            // 
            this.btnExportPreview.Location = new System.Drawing.Point(12, 363);
            this.btnExportPreview.Name = "btnExportPreview";
            this.btnExportPreview.Size = new System.Drawing.Size(161, 23);
            this.btnExportPreview.TabIndex = 1;
            this.btnExportPreview.Text = "Export preview";
            this.btnExportPreview.UseVisualStyleBackColor = true;
            this.btnExportPreview.Click += new System.EventHandler(this.btnExportPreview_Click);
            // 
            // UnregularDataPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 415);
            this.ControlBox = false;
            this.Controls.Add(this.btnExportPreview);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UnregularDataPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unregular Data Preview";
            this.Load += new System.EventHandler(this.UnregularDataPreview_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvPreview;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExportPreview;
    }
}