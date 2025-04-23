namespace UI
{
    partial class MealTypesAdditionalData
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
            this.gbPicture = new System.Windows.Forms.GroupBox();
            this.pbMeal = new System.Windows.Forms.PictureBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbPicture = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbMealTypeID = new System.Windows.Forms.TextBox();
            this.lblMealTypeID = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbMealType.SuspendLayout();
            this.gbPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMeal)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMealType
            // 
            this.gbMealType.Controls.Add(this.gbPicture);
            this.gbMealType.Controls.Add(this.tbDescription);
            this.gbMealType.Controls.Add(this.lblDescription);
            this.gbMealType.Controls.Add(this.tbName);
            this.gbMealType.Controls.Add(this.lblName);
            this.gbMealType.Controls.Add(this.tbMealTypeID);
            this.gbMealType.Controls.Add(this.lblMealTypeID);
            this.gbMealType.Location = new System.Drawing.Point(8, 12);
            this.gbMealType.Name = "gbMealType";
            this.gbMealType.Size = new System.Drawing.Size(507, 541);
            this.gbMealType.TabIndex = 0;
            this.gbMealType.TabStop = false;
            this.gbMealType.Text = "Meal type";
            this.gbMealType.Enter += new System.EventHandler(this.gbMealType_Enter);
            // 
            // gbPicture
            // 
            this.gbPicture.Controls.Add(this.pbMeal);
            this.gbPicture.Controls.Add(this.btnBrowse);
            this.gbPicture.Controls.Add(this.tbPicture);
            this.gbPicture.Controls.Add(this.lblPath);
            this.gbPicture.Location = new System.Drawing.Point(6, 289);
            this.gbPicture.Name = "gbPicture";
            this.gbPicture.Size = new System.Drawing.Size(486, 243);
            this.gbPicture.TabIndex = 6;
            this.gbPicture.TabStop = false;
            this.gbPicture.Text = "Picture";
            // 
            // pbMeal
            // 
            this.pbMeal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbMeal.Location = new System.Drawing.Point(125, 70);
            this.pbMeal.Name = "pbMeal";
            this.pbMeal.Size = new System.Drawing.Size(254, 157);
            this.pbMeal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMeal.TabIndex = 3;
            this.pbMeal.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(385, 33);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbPicture
            // 
            this.tbPicture.Location = new System.Drawing.Point(125, 35);
            this.tbPicture.Name = "tbPicture";
            this.tbPicture.Size = new System.Drawing.Size(254, 20);
            this.tbPicture.TabIndex = 1;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(23, 38);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(32, 13);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "Path:";
            this.lblPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(131, 106);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(254, 177);
            this.tbDescription.TabIndex = 5;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(29, 109);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(131, 65);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(254, 20);
            this.tbName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(29, 68);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbMealTypeID
            // 
            this.tbMealTypeID.Enabled = false;
            this.tbMealTypeID.Location = new System.Drawing.Point(131, 29);
            this.tbMealTypeID.Name = "tbMealTypeID";
            this.tbMealTypeID.Size = new System.Drawing.Size(90, 20);
            this.tbMealTypeID.TabIndex = 1;
            // 
            // lblMealTypeID
            // 
            this.lblMealTypeID.AutoSize = true;
            this.lblMealTypeID.Location = new System.Drawing.Point(29, 32);
            this.lblMealTypeID.Name = "lblMealTypeID";
            this.lblMealTypeID.Size = new System.Drawing.Size(70, 13);
            this.lblMealTypeID.TabIndex = 0;
            this.lblMealTypeID.Text = "Meal type ID:";
            this.lblMealTypeID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(49, 559);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(49, 559);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(409, 559);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MealTypesAdditionalData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 604);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMealType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MealTypesAdditionalData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Additional Data";
            this.gbMealType.ResumeLayout(false);
            this.gbMealType.PerformLayout();
            this.gbPicture.ResumeLayout(false);
            this.gbPicture.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMeal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMealType;
        private System.Windows.Forms.Label lblMealTypeID;
        private System.Windows.Forms.TextBox tbMealTypeID;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.GroupBox gbPicture;
        private System.Windows.Forms.TextBox tbPicture;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox pbMeal;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
    }
}