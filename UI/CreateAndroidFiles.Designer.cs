namespace UI
{
    partial class CreateAndroidFiles
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
            this.btnCreateFiles = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbIMEI = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbLati = new System.Windows.Forms.TextBox();
            this.tbLongi = new System.Windows.Forms.TextBox();
            this.dtpDateTime = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.ckbxInOut = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lvEmployee = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateFiles
            // 
            this.btnCreateFiles.Location = new System.Drawing.Point(175, 428);
            this.btnCreateFiles.Name = "btnCreateFiles";
            this.btnCreateFiles.Size = new System.Drawing.Size(131, 23);
            this.btnCreateFiles.TabIndex = 0;
            this.btnCreateFiles.Text = "Kreiraj fajlove";
            this.btnCreateFiles.UseVisualStyleBackColor = true;
            this.btnCreateFiles.Click += new System.EventHandler(this.btnCreateFiles_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(132, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "ActA mobile registrations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IMEI:";
            // 
            // tbIMEI
            // 
            this.tbIMEI.Location = new System.Drawing.Point(136, 72);
            this.tbIMEI.MaxLength = 15;
            this.tbIMEI.Name = "tbIMEI";
            this.tbIMEI.Size = new System.Drawing.Size(143, 20);
            this.tbIMEI.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Geografska širina:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Geografska dužina:";
            // 
            // tbLati
            // 
            this.tbLati.Location = new System.Drawing.Point(136, 112);
            this.tbLati.MaxLength = 10;
            this.tbLati.Name = "tbLati";
            this.tbLati.Size = new System.Drawing.Size(143, 20);
            this.tbLati.TabIndex = 7;
            // 
            // tbLongi
            // 
            this.tbLongi.Location = new System.Drawing.Point(136, 153);
            this.tbLongi.MaxLength = 10;
            this.tbLongi.Name = "tbLongi";
            this.tbLongi.Size = new System.Drawing.Size(143, 20);
            this.tbLongi.TabIndex = 8;
            // 
            // dtpDateTime
            // 
            this.dtpDateTime.CustomFormat = "yyyy-MM-dd hh:mm:ss";
            this.dtpDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTime.Location = new System.Drawing.Point(136, 188);
            this.dtpDateTime.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtpDateTime.Name = "dtpDateTime";
            this.dtpDateTime.Size = new System.Drawing.Size(143, 20);
            this.dtpDateTime.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Datum i vreme:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(85, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "UID:";
            // 
            // tbUID
            // 
            this.tbUID.Location = new System.Drawing.Point(136, 230);
            this.tbUID.MaxLength = 10;
            this.tbUID.Name = "tbUID";
            this.tbUID.Size = new System.Drawing.Size(143, 20);
            this.tbUID.TabIndex = 12;
            // 
            // ckbxInOut
            // 
            this.ckbxInOut.AutoSize = true;
            this.ckbxInOut.Location = new System.Drawing.Point(136, 274);
            this.ckbxInOut.Name = "ckbxInOut";
            this.ckbxInOut.Size = new System.Drawing.Size(65, 17);
            this.ckbxInOut.TabIndex = 13;
            this.ckbxInOut.Text = "IN/OUT";
            this.ckbxInOut.UseVisualStyleBackColor = true;
            this.ckbxInOut.CheckedChanged += new System.EventHandler(this.ckbxInOut_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(58, 274);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Zaposleni:";
            // 
            // lvEmployee
            // 
            this.lvEmployee.FullRowSelect = true;
            this.lvEmployee.GridLines = true;
            this.lvEmployee.HideSelection = false;
            this.lvEmployee.Location = new System.Drawing.Point(136, 310);
            this.lvEmployee.Name = "lvEmployee";
            this.lvEmployee.Size = new System.Drawing.Size(336, 138);
            this.lvEmployee.TabIndex = 16;
            this.lvEmployee.UseCompatibleStateImageBehavior = false;
            this.lvEmployee.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnCreateFiles);
            this.groupBox1.Location = new System.Drawing.Point(7, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 467);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mobile registrations";
            // 
            // CreateAndroidFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 525);
            this.Controls.Add(this.lvEmployee);
            this.Controls.Add(this.ckbxInOut);
            this.Controls.Add(this.tbUID);
            this.Controls.Add(this.dtpDateTime);
            this.Controls.Add(this.tbLongi);
            this.Controls.Add(this.tbLati);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbIMEI);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CreateAndroidFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mobile registrations";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbIMEI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbLati;
        private System.Windows.Forms.TextBox tbLongi;
        private System.Windows.Forms.DateTimePicker dtpDateTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.CheckBox ckbxInOut;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListView lvEmployee;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

