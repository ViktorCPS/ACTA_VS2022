namespace UI
{
    partial class MachinesXEmployeesAdd
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
            this.lblEmployeesForMachine = new System.Windows.Forms.Label();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.tbRoleDesc = new System.Windows.Forms.TextBox();
            this.lblRoleDesc = new System.Windows.Forms.Label();
            this.cbMachines = new System.Windows.Forms.ComboBox();
            this.lblMachine = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblEmployeesForMachine
            // 
            this.lblEmployeesForMachine.Location = new System.Drawing.Point(9, 79);
            this.lblEmployeesForMachine.Name = "lblEmployeesForMachine";
            this.lblEmployeesForMachine.Size = new System.Drawing.Size(178, 43);
            this.lblEmployeesForMachine.TabIndex = 23;
            this.lblEmployeesForMachine.Text = "List of all Employees who are not granted selected Machine:";
            this.lblEmployeesForMachine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(12, 125);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(576, 200);
            this.lvEmployees.TabIndex = 24;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            // 
            // tbRoleDesc
            // 
            this.tbRoleDesc.Enabled = false;
            this.tbRoleDesc.Location = new System.Drawing.Point(243, 44);
            this.tbRoleDesc.Name = "tbRoleDesc";
            this.tbRoleDesc.Size = new System.Drawing.Size(240, 20);
            this.tbRoleDesc.TabIndex = 28;
            // 
            // lblRoleDesc
            // 
            this.lblRoleDesc.Location = new System.Drawing.Point(107, 47);
            this.lblRoleDesc.Name = "lblRoleDesc";
            this.lblRoleDesc.Size = new System.Drawing.Size(120, 23);
            this.lblRoleDesc.TabIndex = 27;
            this.lblRoleDesc.Text = "Description";
            this.lblRoleDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMachines
            // 
            this.cbMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMachines.Location = new System.Drawing.Point(243, 12);
            this.cbMachines.Name = "cbMachines";
            this.cbMachines.Size = new System.Drawing.Size(240, 21);
            this.cbMachines.TabIndex = 26;
            // 
            // lblMachine
            // 
            this.lblMachine.Location = new System.Drawing.Point(107, 15);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(120, 23);
            this.lblMachine.TabIndex = 25;
            this.lblMachine.Text = "Machine:";
            this.lblMachine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(513, 336);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 34;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 336);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // MachinesXEmployeesAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 371);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbRoleDesc);
            this.Controls.Add(this.lblRoleDesc);
            this.Controls.Add(this.cbMachines);
            this.Controls.Add(this.lblMachine);
            this.Controls.Add(this.lblEmployeesForMachine);
            this.Controls.Add(this.lvEmployees);
            this.Name = "MachinesXEmployeesAdd";
            this.Text = "MachinesXEmployeesAdd";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEmployeesForMachine;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.TextBox tbRoleDesc;
        private System.Windows.Forms.Label lblRoleDesc;
        private System.Windows.Forms.ComboBox cbMachines;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
    }
}