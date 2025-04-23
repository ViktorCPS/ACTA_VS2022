namespace Reports
{
    partial class VisitorReportsScanDocument
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
            this.btnClose = new System.Windows.Forms.Button();
            this.gbScanDoc = new System.Windows.Forms.GroupBox();
            this.pbNoImage = new System.Windows.Forms.PictureBox();
            this.pbScanDoc = new System.Windows.Forms.PictureBox();
            this.gbVisitor = new System.Windows.Forms.GroupBox();
            this.tbWU = new System.Windows.Forms.TextBox();
            this.tbEmpl = new System.Windows.Forms.TextBox();
            this.tbEnd = new System.Windows.Forms.TextBox();
            this.lblVisitStart = new System.Windows.Forms.Label();
            this.lblVisitEnd = new System.Windows.Forms.Label();
            this.tbStart = new System.Windows.Forms.TextBox();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbCard = new System.Windows.Forms.TextBox();
            this.lblCardNum = new System.Windows.Forms.Label();
            this.lblJMBG = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.lblVisitDescr = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.tbJMBG = new System.Windows.Forms.TextBox();
            this.gbScanDoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNoImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScanDoc)).BeginInit();
            this.gbVisitor.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(412, 395);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbScanDoc
            // 
            this.gbScanDoc.Controls.Add(this.pbNoImage);
            this.gbScanDoc.Controls.Add(this.pbScanDoc);
            this.gbScanDoc.Location = new System.Drawing.Point(12, 4);
            this.gbScanDoc.Name = "gbScanDoc";
            this.gbScanDoc.Size = new System.Drawing.Size(548, 375);
            this.gbScanDoc.TabIndex = 0;
            this.gbScanDoc.TabStop = false;
            this.gbScanDoc.Text = "Scaned document";
            // 
            // pbNoImage
            // 
            this.pbNoImage.Location = new System.Drawing.Point(9, 82);
            this.pbNoImage.Name = "pbNoImage";
            this.pbNoImage.Size = new System.Drawing.Size(530, 215);
            this.pbNoImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbNoImage.TabIndex = 21;
            this.pbNoImage.TabStop = false;
            this.pbNoImage.Visible = false;
            // 
            // pbScanDoc
            // 
            this.pbScanDoc.Location = new System.Drawing.Point(9, 22);
            this.pbScanDoc.Name = "pbScanDoc";
            this.pbScanDoc.Size = new System.Drawing.Size(530, 340);
            this.pbScanDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbScanDoc.TabIndex = 1;
            this.pbScanDoc.TabStop = false;
            // 
            // gbVisitor
            // 
            this.gbVisitor.Controls.Add(this.tbWU);
            this.gbVisitor.Controls.Add(this.tbEmpl);
            this.gbVisitor.Controls.Add(this.tbEnd);
            this.gbVisitor.Controls.Add(this.lblVisitStart);
            this.gbVisitor.Controls.Add(this.lblVisitEnd);
            this.gbVisitor.Controls.Add(this.tbStart);
            this.gbVisitor.Controls.Add(this.tbDesc);
            this.gbVisitor.Controls.Add(this.tbCard);
            this.gbVisitor.Controls.Add(this.lblCardNum);
            this.gbVisitor.Controls.Add(this.lblJMBG);
            this.gbVisitor.Controls.Add(this.lblFirstName);
            this.gbVisitor.Controls.Add(this.lblLastName);
            this.gbVisitor.Controls.Add(this.lblWU);
            this.gbVisitor.Controls.Add(this.lblEmployee);
            this.gbVisitor.Controls.Add(this.tbLastName);
            this.gbVisitor.Controls.Add(this.lblVisitDescr);
            this.gbVisitor.Controls.Add(this.tbFirstName);
            this.gbVisitor.Controls.Add(this.tbJMBG);
            this.gbVisitor.Location = new System.Drawing.Point(566, 4);
            this.gbVisitor.Name = "gbVisitor";
            this.gbVisitor.Size = new System.Drawing.Size(317, 375);
            this.gbVisitor.TabIndex = 1;
            this.gbVisitor.TabStop = false;
            this.gbVisitor.Text = "Visitor data";
            // 
            // tbWU
            // 
            this.tbWU.Enabled = false;
            this.tbWU.Location = new System.Drawing.Point(125, 300);
            this.tbWU.MaxLength = 13;
            this.tbWU.Name = "tbWU";
            this.tbWU.Size = new System.Drawing.Size(176, 20);
            this.tbWU.TabIndex = 17;
            // 
            // tbEmpl
            // 
            this.tbEmpl.Enabled = false;
            this.tbEmpl.Location = new System.Drawing.Point(125, 341);
            this.tbEmpl.MaxLength = 13;
            this.tbEmpl.Name = "tbEmpl";
            this.tbEmpl.Size = new System.Drawing.Size(176, 20);
            this.tbEmpl.TabIndex = 19;
            // 
            // tbEnd
            // 
            this.tbEnd.Enabled = false;
            this.tbEnd.Location = new System.Drawing.Point(125, 260);
            this.tbEnd.MaxLength = 13;
            this.tbEnd.Name = "tbEnd";
            this.tbEnd.Size = new System.Drawing.Size(176, 20);
            this.tbEnd.TabIndex = 15;
            // 
            // lblVisitStart
            // 
            this.lblVisitStart.Enabled = false;
            this.lblVisitStart.Location = new System.Drawing.Point(19, 216);
            this.lblVisitStart.Name = "lblVisitStart";
            this.lblVisitStart.Size = new System.Drawing.Size(100, 23);
            this.lblVisitStart.TabIndex = 12;
            this.lblVisitStart.Text = "Visit start:";
            this.lblVisitStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVisitEnd
            // 
            this.lblVisitEnd.Enabled = false;
            this.lblVisitEnd.Location = new System.Drawing.Point(19, 258);
            this.lblVisitEnd.Name = "lblVisitEnd";
            this.lblVisitEnd.Size = new System.Drawing.Size(100, 23);
            this.lblVisitEnd.TabIndex = 14;
            this.lblVisitEnd.Text = "Visit end:";
            this.lblVisitEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbStart
            // 
            this.tbStart.Enabled = false;
            this.tbStart.Location = new System.Drawing.Point(125, 218);
            this.tbStart.MaxLength = 13;
            this.tbStart.Name = "tbStart";
            this.tbStart.Size = new System.Drawing.Size(176, 20);
            this.tbStart.TabIndex = 13;
            // 
            // tbDesc
            // 
            this.tbDesc.Enabled = false;
            this.tbDesc.Location = new System.Drawing.Point(125, 176);
            this.tbDesc.MaxLength = 13;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(176, 20);
            this.tbDesc.TabIndex = 11;
            // 
            // tbCard
            // 
            this.tbCard.Enabled = false;
            this.tbCard.Location = new System.Drawing.Point(125, 18);
            this.tbCard.MaxLength = 13;
            this.tbCard.Name = "tbCard";
            this.tbCard.Size = new System.Drawing.Size(176, 20);
            this.tbCard.TabIndex = 3;
            // 
            // lblCardNum
            // 
            this.lblCardNum.Enabled = false;
            this.lblCardNum.Location = new System.Drawing.Point(19, 16);
            this.lblCardNum.Name = "lblCardNum";
            this.lblCardNum.Size = new System.Drawing.Size(100, 23);
            this.lblCardNum.TabIndex = 2;
            this.lblCardNum.Text = "Card number:";
            this.lblCardNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJMBG
            // 
            this.lblJMBG.Enabled = false;
            this.lblJMBG.Location = new System.Drawing.Point(19, 132);
            this.lblJMBG.Name = "lblJMBG";
            this.lblJMBG.Size = new System.Drawing.Size(100, 23);
            this.lblJMBG.TabIndex = 8;
            this.lblJMBG.Text = "PIN:";
            this.lblJMBG.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Enabled = false;
            this.lblFirstName.Location = new System.Drawing.Point(19, 54);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(100, 23);
            this.lblFirstName.TabIndex = 4;
            this.lblFirstName.Text = "First name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLastName
            // 
            this.lblLastName.Enabled = false;
            this.lblLastName.Location = new System.Drawing.Point(19, 93);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(100, 23);
            this.lblLastName.TabIndex = 6;
            this.lblLastName.Text = "Last name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Enabled = false;
            this.lblWU.Location = new System.Drawing.Point(6, 298);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(113, 23);
            this.lblWU.TabIndex = 16;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Enabled = false;
            this.lblEmployee.Location = new System.Drawing.Point(9, 339);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(110, 23);
            this.lblEmployee.TabIndex = 18;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbLastName
            // 
            this.tbLastName.Enabled = false;
            this.tbLastName.Location = new System.Drawing.Point(125, 95);
            this.tbLastName.MaxLength = 64;
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(176, 20);
            this.tbLastName.TabIndex = 7;
            // 
            // lblVisitDescr
            // 
            this.lblVisitDescr.Enabled = false;
            this.lblVisitDescr.Location = new System.Drawing.Point(19, 174);
            this.lblVisitDescr.Name = "lblVisitDescr";
            this.lblVisitDescr.Size = new System.Drawing.Size(100, 23);
            this.lblVisitDescr.TabIndex = 10;
            this.lblVisitDescr.Text = "Visit description:";
            this.lblVisitDescr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFirstName
            // 
            this.tbFirstName.Enabled = false;
            this.tbFirstName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbFirstName.Location = new System.Drawing.Point(125, 56);
            this.tbFirstName.MaxLength = 64;
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(176, 20);
            this.tbFirstName.TabIndex = 5;
            // 
            // tbJMBG
            // 
            this.tbJMBG.Enabled = false;
            this.tbJMBG.Location = new System.Drawing.Point(125, 134);
            this.tbJMBG.MaxLength = 13;
            this.tbJMBG.Name = "tbJMBG";
            this.tbJMBG.Size = new System.Drawing.Size(176, 20);
            this.tbJMBG.TabIndex = 9;
            // 
            // VisitorReportsScanDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 430);
            this.ControlBox = false;
            this.Controls.Add(this.gbScanDoc);
            this.Controls.Add(this.gbVisitor);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "VisitorReportsScanDocument";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "VisitorReportsScanDocument";
            this.gbScanDoc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbNoImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScanDoc)).EndInit();
            this.gbVisitor.ResumeLayout(false);
            this.gbVisitor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbScanDoc;
        private System.Windows.Forms.PictureBox pbScanDoc;
        private System.Windows.Forms.GroupBox gbVisitor;
        private System.Windows.Forms.Label lblCardNum;
        private System.Windows.Forms.Label lblJMBG;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.Label lblVisitDescr;
        private System.Windows.Forms.TextBox tbFirstName;
        private System.Windows.Forms.TextBox tbJMBG;
        private System.Windows.Forms.TextBox tbEnd;
        private System.Windows.Forms.Label lblVisitStart;
        private System.Windows.Forms.Label lblVisitEnd;
        private System.Windows.Forms.TextBox tbStart;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.TextBox tbCard;
        private System.Windows.Forms.TextBox tbWU;
        private System.Windows.Forms.TextBox tbEmpl;
        private System.Windows.Forms.PictureBox pbNoImage;
    }
}