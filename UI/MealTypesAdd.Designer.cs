namespace UI
{
    partial class MealTypesAdd
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
            this.gbMealType = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpHoursTo = new System.Windows.Forms.DateTimePicker();
            this.lblHoursTo = new System.Windows.Forms.Label();
            this.dtpHoursFrom = new System.Windows.Forms.DateTimePicker();
            this.lblHoursFrom = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealTypeID = new System.Windows.Forms.TextBox();
            this.lblMealTypeID = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gbMealType.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMealType
            // 
            this.gbMealType.Controls.Add(this.label1);
            this.gbMealType.Controls.Add(this.label3);
            this.gbMealType.Controls.Add(this.dtpHoursTo);
            this.gbMealType.Controls.Add(this.lblHoursTo);
            this.gbMealType.Controls.Add(this.dtpHoursFrom);
            this.gbMealType.Controls.Add(this.lblHoursFrom);
            this.gbMealType.Controls.Add(this.tbDescription);
            this.gbMealType.Controls.Add(this.lblDescription);
            this.gbMealType.Controls.Add(this.tbName);
            this.gbMealType.Controls.Add(this.lblName);
            this.gbMealType.Controls.Add(this.tbMealTypeID);
            this.gbMealType.Controls.Add(this.lblMealTypeID);
            this.gbMealType.Location = new System.Drawing.Point(12, 12);
            this.gbMealType.Name = "gbMealType";
            this.gbMealType.Size = new System.Drawing.Size(355, 215);
            this.gbMealType.TabIndex = 0;
            this.gbMealType.TabStop = false;
            this.gbMealType.Text = "Meal type";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(313, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(250, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "*";
            // 
            // dtpHoursTo
            // 
            this.dtpHoursTo.CustomFormat = "HH:mm";
            this.dtpHoursTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursTo.Location = new System.Drawing.Point(144, 170);
            this.dtpHoursTo.Name = "dtpHoursTo";
            this.dtpHoursTo.ShowUpDown = true;
            this.dtpHoursTo.Size = new System.Drawing.Size(52, 20);
            this.dtpHoursTo.TabIndex = 10;
            this.dtpHoursTo.Value = new System.DateTime(2008, 8, 11, 23, 59, 0, 0);
            // 
            // lblHoursTo
            // 
            this.lblHoursTo.Location = new System.Drawing.Point(15, 174);
            this.lblHoursTo.Name = "lblHoursTo";
            this.lblHoursTo.Size = new System.Drawing.Size(123, 13);
            this.lblHoursTo.TabIndex = 9;
            this.lblHoursTo.Text = "Hours to:";
            this.lblHoursTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpHoursFrom
            // 
            this.dtpHoursFrom.CustomFormat = "HH:mm";
            this.dtpHoursFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursFrom.Location = new System.Drawing.Point(144, 137);
            this.dtpHoursFrom.Name = "dtpHoursFrom";
            this.dtpHoursFrom.ShowUpDown = true;
            this.dtpHoursFrom.Size = new System.Drawing.Size(52, 20);
            this.dtpHoursFrom.TabIndex = 8;
            this.dtpHoursFrom.Value = new System.DateTime(2008, 8, 11, 0, 0, 0, 0);
            // 
            // lblHoursFrom
            // 
            this.lblHoursFrom.Location = new System.Drawing.Point(14, 141);
            this.lblHoursFrom.Name = "lblHoursFrom";
            this.lblHoursFrom.Size = new System.Drawing.Size(124, 13);
            this.lblHoursFrom.TabIndex = 7;
            this.lblHoursFrom.Text = "Hours from:";
            this.lblHoursFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(144, 103);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
            this.tbDescription.TabIndex = 6;
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(17, 106);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(121, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(144, 67);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(9, 70);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(129, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMealTypeID
            // 
            this.tbMealTypeID.Location = new System.Drawing.Point(144, 30);
            this.tbMealTypeID.Name = "tbMealTypeID";
            this.tbMealTypeID.Size = new System.Drawing.Size(100, 20);
            this.tbMealTypeID.TabIndex = 2;
            // 
            // lblMealTypeID
            // 
            this.lblMealTypeID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMealTypeID.Location = new System.Drawing.Point(6, 33);
            this.lblMealTypeID.Name = "lblMealTypeID";
            this.lblMealTypeID.Size = new System.Drawing.Size(132, 13);
            this.lblMealTypeID.TabIndex = 1;
            this.lblMealTypeID.Text = "Meal type ID:";
            this.lblMealTypeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 247);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(292, 247);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 247);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // MealTypesAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.KeyPreview = true;
            this.ClientSize = new System.Drawing.Size(379, 284);
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMealType);
            this.Name = "MealTypesAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Meal type";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MealTypesAdd_KeyUp);
            this.gbMealType.ResumeLayout(false);
            this.gbMealType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMealType;
        private System.Windows.Forms.Label lblMealTypeID;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealTypeID;
        private System.Windows.Forms.Label lblHoursTo;
        private System.Windows.Forms.DateTimePicker dtpHoursFrom;
        private System.Windows.Forms.Label lblHoursFrom;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.DateTimePicker dtpHoursTo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}