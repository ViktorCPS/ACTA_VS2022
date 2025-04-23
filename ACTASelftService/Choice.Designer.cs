namespace ACTASelftService
{
    partial class Choice
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
            this.lblName = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnEnterAbsence = new System.Windows.Forms.Button();
            this.btnForgotenCard = new System.Windows.Forms.Button();
            this.btbClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(12, 23);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(134, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(152, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(323, 20);
            this.textBox1.TabIndex = 1;
            // 
            // btnEnterAbsence
            // 
            this.btnEnterAbsence.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnterAbsence.Location = new System.Drawing.Point(152, 123);
            this.btnEnterAbsence.Name = "btnEnterAbsence";
            this.btnEnterAbsence.Size = new System.Drawing.Size(148, 129);
            this.btnEnterAbsence.TabIndex = 2;
            this.btnEnterAbsence.Text = "Enter absence";
            this.btnEnterAbsence.UseVisualStyleBackColor = true;
            this.btnEnterAbsence.Click += new System.EventHandler(this.btnEnterAbsence_Click);
            // 
            // btnForgotenCard
            // 
            this.btnForgotenCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForgotenCard.Location = new System.Drawing.Point(327, 123);
            this.btnForgotenCard.Name = "btnForgotenCard";
            this.btnForgotenCard.Size = new System.Drawing.Size(148, 129);
            this.btnForgotenCard.TabIndex = 3;
            this.btnForgotenCard.Text = "Forgoten card";
            this.btnForgotenCard.UseVisualStyleBackColor = true;
            this.btnForgotenCard.Click += new System.EventHandler(this.btnForgotenCard_Click);
            // 
            // btbClose
            // 
            this.btbClose.Location = new System.Drawing.Point(502, 321);
            this.btbClose.Name = "btbClose";
            this.btbClose.Size = new System.Drawing.Size(75, 23);
            this.btbClose.TabIndex = 4;
            this.btbClose.Text = "Close";
            this.btbClose.UseVisualStyleBackColor = true;
            this.btbClose.Click += new System.EventHandler(this.btbCancel_Click);
            // 
            // Choice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 366);
            this.ControlBox = false;
            this.Controls.Add(this.btbClose);
            this.Controls.Add(this.btnForgotenCard);
            this.Controls.Add(this.btnEnterAbsence);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Choice";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Choice";
            this.Load += new System.EventHandler(this.Choice_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnEnterAbsence;
        private System.Windows.Forms.Button btnForgotenCard;
        private System.Windows.Forms.Button btbClose;
    }
}