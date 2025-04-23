namespace Reports.GSK
{
    partial class GSKVAcationRequest
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
            this.btnGenerateRequest = new System.Windows.Forms.Button();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblFor = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.cbEmployeeTo = new System.Windows.Forms.ComboBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.cbReplacement = new System.Windows.Forms.ComboBox();
            this.lblReplacement = new System.Windows.Forms.Label();
            this.lblTelephone = new System.Windows.Forms.Label();
            this.tbTelephone = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(352, 323);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateRequest
            // 
            this.btnGenerateRequest.Location = new System.Drawing.Point(12, 323);
            this.btnGenerateRequest.Name = "btnGenerateRequest";
            this.btnGenerateRequest.Size = new System.Drawing.Size(136, 23);
            this.btnGenerateRequest.TabIndex = 20;
            this.btnGenerateRequest.Text = "Generate Request";
            this.btnGenerateRequest.Click += new System.EventHandler(this.btnGenerateRequest_Click);
            // 
            // tbEmployee
            // 
            this.tbEmployee.Enabled = false;
            this.tbEmployee.Location = new System.Drawing.Point(121, 26);
            this.tbEmployee.MaxLength = 50;
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(287, 20);
            this.tbEmployee.TabIndex = 22;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(9, 26);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(100, 23);
            this.lblEmployee.TabIndex = 21;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFor
            // 
            this.lblFor.Location = new System.Drawing.Point(9, 75);
            this.lblFor.Name = "lblFor";
            this.lblFor.Size = new System.Drawing.Size(100, 23);
            this.lblFor.TabIndex = 23;
            this.lblFor.Text = "To:";
            this.lblFor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(45, 138);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(42, 23);
            this.lblFrom.TabIndex = 25;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd.MM.yyyy";
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(93, 139);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(117, 20);
            this.dateTimePicker1.TabIndex = 26;
            this.dateTimePicker1.Value = new System.DateTime(2010, 4, 26, 0, 0, 0, 0);
            // 
            // cbEmployeeTo
            // 
            this.cbEmployeeTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployeeTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployeeTo.Location = new System.Drawing.Point(121, 77);
            this.cbEmployeeTo.Name = "cbEmployeeTo";
            this.cbEmployeeTo.Size = new System.Drawing.Size(287, 21);
            this.cbEmployeeTo.TabIndex = 27;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "dd.MM.yyyy";
            this.dateTimePicker2.Enabled = false;
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(301, 139);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(121, 20);
            this.dateTimePicker2.TabIndex = 31;
            this.dateTimePicker2.Value = new System.DateTime(2010, 4, 26, 0, 0, 0, 0);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(249, 138);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(46, 23);
            this.lblTo.TabIndex = 30;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbReplacement
            // 
            this.cbReplacement.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbReplacement.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbReplacement.Location = new System.Drawing.Point(121, 211);
            this.cbReplacement.Name = "cbReplacement";
            this.cbReplacement.Size = new System.Drawing.Size(287, 21);
            this.cbReplacement.TabIndex = 33;
            // 
            // lblReplacement
            // 
            this.lblReplacement.Location = new System.Drawing.Point(9, 209);
            this.lblReplacement.Name = "lblReplacement";
            this.lblReplacement.Size = new System.Drawing.Size(100, 23);
            this.lblReplacement.TabIndex = 32;
            this.lblReplacement.Text = "Replacement:";
            this.lblReplacement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTelephone
            // 
            this.lblTelephone.Location = new System.Drawing.Point(12, 263);
            this.lblTelephone.Name = "lblTelephone";
            this.lblTelephone.Size = new System.Drawing.Size(100, 23);
            this.lblTelephone.TabIndex = 34;
            this.lblTelephone.Text = "Telephone number:";
            this.lblTelephone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTelephone
            // 
            this.tbTelephone.Location = new System.Drawing.Point(121, 265);
            this.tbTelephone.MaxLength = 50;
            this.tbTelephone.Name = "tbTelephone";
            this.tbTelephone.Size = new System.Drawing.Size(287, 20);
            this.tbTelephone.TabIndex = 35;
            // 
            // GSKVAcationRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 366);
            this.ControlBox = false;
            this.Controls.Add(this.tbTelephone);
            this.Controls.Add(this.lblTelephone);
            this.Controls.Add(this.cbReplacement);
            this.Controls.Add(this.lblReplacement);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.cbEmployeeTo);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.lblFor);
            this.Controls.Add(this.tbEmployee);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.btnGenerateRequest);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GSKVAcationRequest";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "GSKVAcationRequest";
            this.Load += new System.EventHandler(this.GSKVAcationRequest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerateRequest;
        private System.Windows.Forms.TextBox tbEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Label lblFor;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox cbEmployeeTo;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.ComboBox cbReplacement;
        private System.Windows.Forms.Label lblReplacement;
        private System.Windows.Forms.Label lblTelephone;
        private System.Windows.Forms.TextBox tbTelephone;
    }
}