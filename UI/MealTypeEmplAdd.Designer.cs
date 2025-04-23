namespace UI
{
    partial class MealTypeEmplAdd
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbMealTypeEmpl = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealTypeEmplID = new System.Windows.Forms.TextBox();
            this.lblMealTypeEmplID = new System.Windows.Forms.Label();
            this.gbMealTypeEmpl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 193);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(292, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 193);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbMealTypeEmpl
            // 
            this.gbMealTypeEmpl.Controls.Add(this.label1);
            this.gbMealTypeEmpl.Controls.Add(this.tbDescription);
            this.gbMealTypeEmpl.Controls.Add(this.lblDescription);
            this.gbMealTypeEmpl.Controls.Add(this.tbName);
            this.gbMealTypeEmpl.Controls.Add(this.lblName);
            this.gbMealTypeEmpl.Controls.Add(this.tbMealTypeEmplID);
            this.gbMealTypeEmpl.Controls.Add(this.lblMealTypeEmplID);
            this.gbMealTypeEmpl.Location = new System.Drawing.Point(12, 12);
            this.gbMealTypeEmpl.Name = "gbMealTypeEmpl";
            this.gbMealTypeEmpl.Size = new System.Drawing.Size(355, 161);
            this.gbMealTypeEmpl.TabIndex = 0;
            this.gbMealTypeEmpl.TabStop = false;
            this.gbMealTypeEmpl.Text = "Meal type employee";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(315, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "*";
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
            this.lblDescription.Location = new System.Drawing.Point(9, 106);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(126, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(144, 64);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(9, 67);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(126, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbMealTypeEmplID
            // 
            this.tbMealTypeEmplID.Location = new System.Drawing.Point(144, 27);
            this.tbMealTypeEmplID.Name = "tbMealTypeEmplID";
            this.tbMealTypeEmplID.Size = new System.Drawing.Size(100, 20);
            this.tbMealTypeEmplID.TabIndex = 2;
            // 
            // lblMealTypeEmplID
            // 
            this.lblMealTypeEmplID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMealTypeEmplID.Location = new System.Drawing.Point(6, 30);
            this.lblMealTypeEmplID.Name = "lblMealTypeEmplID";
            this.lblMealTypeEmplID.Size = new System.Drawing.Size(132, 13);
            this.lblMealTypeEmplID.TabIndex = 1;
            this.lblMealTypeEmplID.Text = "Meal type ID:";
            this.lblMealTypeEmplID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MealTypeEmplAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 242);
            this.KeyPreview = true;
            this.ControlBox = false;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMealTypeEmpl);
            this.Name = "MealTypeEmplAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MealTypeEmplAdd";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MealTypeEmplAdd_KeyUp);
            this.gbMealTypeEmpl.ResumeLayout(false);
            this.gbMealTypeEmpl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbMealTypeEmpl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbMealTypeEmplID;
        private System.Windows.Forms.Label lblMealTypeEmplID;
    }
}