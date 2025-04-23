namespace UI
{
    partial class OnlineMealRestaurantAdd
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
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.gbRestaurant = new System.Windows.Forms.GroupBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbRestaurantID = new System.Windows.Forms.TextBox();
            this.lblRestaurantID = new System.Windows.Forms.Label();
            this.gbRestaurant.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(292, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.MouseCaptureChanged += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 188);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(316, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(253, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "*";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 188);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 19;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(20, 87);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(121, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbRestaurant
            // 
            this.gbRestaurant.Controls.Add(this.label1);
            this.gbRestaurant.Controls.Add(this.label3);
            this.gbRestaurant.Controls.Add(this.tbDescription);
            this.gbRestaurant.Controls.Add(this.lblDescription);
            this.gbRestaurant.Controls.Add(this.tbName);
            this.gbRestaurant.Controls.Add(this.lblName);
            this.gbRestaurant.Controls.Add(this.tbRestaurantID);
            this.gbRestaurant.Controls.Add(this.lblRestaurantID);
            this.gbRestaurant.Location = new System.Drawing.Point(12, 17);
            this.gbRestaurant.Name = "gbRestaurant";
            this.gbRestaurant.Size = new System.Drawing.Size(355, 148);
            this.gbRestaurant.TabIndex = 18;
            this.gbRestaurant.TabStop = false;
            this.gbRestaurant.Text = "Restaurant";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(147, 84);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(165, 20);
            this.tbDescription.TabIndex = 6;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(147, 48);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(165, 20);
            this.tbName.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Location = new System.Drawing.Point(12, 51);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(129, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRestaurantID
            // 
            this.tbRestaurantID.Location = new System.Drawing.Point(147, 13);
            this.tbRestaurantID.Name = "tbRestaurantID";
            this.tbRestaurantID.Size = new System.Drawing.Size(100, 20);
            this.tbRestaurantID.TabIndex = 2;
            // 
            // lblRestaurantID
            // 
            this.lblRestaurantID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRestaurantID.Location = new System.Drawing.Point(9, 16);
            this.lblRestaurantID.Name = "lblRestaurantID";
            this.lblRestaurantID.Size = new System.Drawing.Size(132, 13);
            this.lblRestaurantID.TabIndex = 1;
            this.lblRestaurantID.Text = "Restaurant ID:";
            this.lblRestaurantID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OnlineMealRestaurantAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 229);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.gbRestaurant);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OnlineMealRestaurantAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OnlineMealRestaurantAdd";
            this.gbRestaurant.ResumeLayout(false);
            this.gbRestaurant.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.GroupBox gbRestaurant;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbRestaurantID;
        private System.Windows.Forms.Label lblRestaurantID;
    }
}