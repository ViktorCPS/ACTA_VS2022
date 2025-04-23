namespace UI
{
    partial class MachinesXEmployees
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
            this.lblSelEmployees = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvSelectedEmployees = new System.Windows.Forms.ListView();
            this.tbRoleDesc = new System.Windows.Forms.TextBox();
            this.lblRoleDesc = new System.Windows.Forms.Label();
            this.cbMachines = new System.Windows.Forms.ComboBox();
            this.lblMachine = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSelEmployees
            // 
            this.lblSelEmployees.Location = new System.Drawing.Point(19, 79);
            this.lblSelEmployees.Name = "lblSelEmployees";
            this.lblSelEmployees.Size = new System.Drawing.Size(248, 48);
            this.lblSelEmployees.TabIndex = 29;
            this.lblSelEmployees.Text = "List of all Employees who are granted selected Machine:";
            this.lblSelEmployees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(526, 338);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 32;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(22, 338);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 31;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lvSelectedEmployees
            // 
            this.lvSelectedEmployees.FullRowSelect = true;
            this.lvSelectedEmployees.GridLines = true;
            this.lvSelectedEmployees.HideSelection = false;
            this.lvSelectedEmployees.Location = new System.Drawing.Point(22, 130);
            this.lvSelectedEmployees.Name = "lvSelectedEmployees";
            this.lvSelectedEmployees.Size = new System.Drawing.Size(576, 200);
            this.lvSelectedEmployees.TabIndex = 30;
            this.lvSelectedEmployees.UseCompatibleStateImageBehavior = false;
            this.lvSelectedEmployees.View = System.Windows.Forms.View.Details;
            // 
            // tbRoleDesc
            // 
            this.tbRoleDesc.Enabled = false;
            this.tbRoleDesc.Location = new System.Drawing.Point(190, 42);
            this.tbRoleDesc.Name = "tbRoleDesc";
            this.tbRoleDesc.Size = new System.Drawing.Size(240, 20);
            this.tbRoleDesc.TabIndex = 22;
            // 
            // lblRoleDesc
            // 
            this.lblRoleDesc.Location = new System.Drawing.Point(54, 45);
            this.lblRoleDesc.Name = "lblRoleDesc";
            this.lblRoleDesc.Size = new System.Drawing.Size(120, 23);
            this.lblRoleDesc.TabIndex = 21;
            this.lblRoleDesc.Text = "Description";
            this.lblRoleDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMachines
            // 
            this.cbMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMachines.Location = new System.Drawing.Point(190, 10);
            this.cbMachines.Name = "cbMachines";
            this.cbMachines.Size = new System.Drawing.Size(240, 21);
            this.cbMachines.TabIndex = 20;
            this.cbMachines.SelectedIndexChanged += new System.EventHandler(this.cbMachines_SelectedIndexChanged);
            // 
            // lblMachine
            // 
            this.lblMachine.Location = new System.Drawing.Point(54, 13);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(120, 23);
            this.lblMachine.TabIndex = 19;
            this.lblMachine.Text = "Machine:";
            this.lblMachine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(112, 338);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 33;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(203, 338);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 34;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(356, 338);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(115, 23);
            this.btnReport.TabIndex = 35;
            this.btnReport.Text = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // MachinesXEmployees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 371);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lblSelEmployees);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvSelectedEmployees);
            this.Controls.Add(this.tbRoleDesc);
            this.Controls.Add(this.lblRoleDesc);
            this.Controls.Add(this.cbMachines);
            this.Controls.Add(this.lblMachine);
            this.Name = "MachinesXEmployees";
            this.Text = "MachinesXEmployees";
            this.Load += new System.EventHandler(this.MachinesXEmployees_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelEmployees;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvSelectedEmployees;
        private System.Windows.Forms.TextBox tbRoleDesc;
        private System.Windows.Forms.Label lblRoleDesc;
        private System.Windows.Forms.ComboBox cbMachines;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReport;
    }
}