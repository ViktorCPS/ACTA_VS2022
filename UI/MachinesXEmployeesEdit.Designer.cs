namespace UI
{
    partial class MachinesXEmployeesEdit
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
            this.btnClose = new System.Windows.Forms.Button();
            this.cbMachines = new System.Windows.Forms.ComboBox();
            this.lblMachine = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cbEmplName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 137);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 35;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(344, 137);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 34;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cbMachines
            // 
            this.cbMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMachines.Location = new System.Drawing.Point(149, 67);
            this.cbMachines.Name = "cbMachines";
            this.cbMachines.Size = new System.Drawing.Size(240, 21);
            this.cbMachines.TabIndex = 37;
            // 
            // lblMachine
            // 
            this.lblMachine.Location = new System.Drawing.Point(13, 70);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(120, 23);
            this.lblMachine.TabIndex = 36;
            this.lblMachine.Text = "Machine:";
            this.lblMachine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(37, 40);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(104, 23);
            this.lblName.TabIndex = 39;
            this.lblName.Text = "First Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmplName
            // 
            this.cbEmplName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmplName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmplName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmplName.Location = new System.Drawing.Point(149, 40);
            this.cbEmplName.Name = "cbEmplName";
            this.cbEmplName.Size = new System.Drawing.Size(240, 21);
            this.cbEmplName.TabIndex = 38;
            // 
            // MachinesXEmployeesEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 179);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cbEmplName);
            this.Controls.Add(this.cbMachines);
            this.Controls.Add(this.lblMachine);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Name = "MachinesXEmployeesEdit";
            this.Text = "MachinesXEmployeesEdit";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cbMachines;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbEmplName;
    }
}