namespace UI
{
    partial class SecurityRoutesScheduleAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityRoutesScheduleAdd));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.cbEmpl = new System.Windows.Forms.ComboBox();
            this.lblEmpl = new System.Windows.Forms.Label();
            this.gbAssignRoutes = new System.Windows.Forms.GroupBox();
            this.btnAssign = new System.Windows.Forms.Button();
            this.lblMonth = new System.Windows.Forms.Label();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbSun = new System.Windows.Forms.TextBox();
            this.tbSat = new System.Windows.Forms.TextBox();
            this.tbFri = new System.Windows.Forms.TextBox();
            this.tbThr = new System.Windows.Forms.TextBox();
            this.tbWed = new System.Windows.Forms.TextBox();
            this.tbTue = new System.Windows.Forms.TextBox();
            this.tbMon = new System.Windows.Forms.TextBox();
            this.gbRoutes = new System.Windows.Forms.GroupBox();
            this.lvDetails = new System.Windows.Forms.ListView();
            this.cbRouteName = new System.Windows.Forms.ComboBox();
            this.lblRouteName = new System.Windows.Forms.Label();
            this.gbEmployee = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClearSelDays = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.gbAssignRoutes.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbRoutes.SuspendLayout();
            this.gbEmployee.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(627, 595);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(265, 17);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 4;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(101, 19);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(158, 21);
            this.cbWU.TabIndex = 3;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(5, 20);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(95, 17);
            this.lblWU.TabIndex = 2;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmpl
            // 
            this.cbEmpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmpl.FormattingEnabled = true;
            this.cbEmpl.Location = new System.Drawing.Point(101, 52);
            this.cbEmpl.Name = "cbEmpl";
            this.cbEmpl.Size = new System.Drawing.Size(158, 21);
            this.cbEmpl.TabIndex = 6;
            this.cbEmpl.SelectedIndexChanged += new System.EventHandler(this.cbEmpl_SelectedIndexChanged);
            // 
            // lblEmpl
            // 
            this.lblEmpl.Location = new System.Drawing.Point(19, 53);
            this.lblEmpl.Name = "lblEmpl";
            this.lblEmpl.Size = new System.Drawing.Size(81, 17);
            this.lblEmpl.TabIndex = 5;
            this.lblEmpl.Text = "Employee:";
            this.lblEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbAssignRoutes
            // 
            this.gbAssignRoutes.Controls.Add(this.btnAssign);
            this.gbAssignRoutes.Controls.Add(this.lblMonth);
            this.gbAssignRoutes.Controls.Add(this.dtpMonth);
            this.gbAssignRoutes.Controls.Add(this.panel1);
            this.gbAssignRoutes.Controls.Add(this.gbRoutes);
            this.gbAssignRoutes.Controls.Add(this.gbEmployee);
            this.gbAssignRoutes.Location = new System.Drawing.Point(12, 12);
            this.gbAssignRoutes.Name = "gbAssignRoutes";
            this.gbAssignRoutes.Size = new System.Drawing.Size(690, 577);
            this.gbAssignRoutes.TabIndex = 0;
            this.gbAssignRoutes.TabStop = false;
            this.gbAssignRoutes.Text = "Assigning routes to employee";
            // 
            // btnAssign
            // 
            this.btnAssign.Location = new System.Drawing.Point(330, 227);
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(75, 23);
            this.btnAssign.TabIndex = 11;
            this.btnAssign.Text = "Assign";
            this.btnAssign.Click += new System.EventHandler(this.btnAssign_Click);
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(27, 227);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(55, 23);
            this.lblMonth.TabIndex = 12;
            this.lblMonth.Text = "Month:";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpMonth
            // 
            this.dtpMonth.CustomFormat = "MMM, yyyy";
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(88, 228);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.ShowUpDown = true;
            this.dtpMonth.Size = new System.Drawing.Size(149, 20);
            this.dtpMonth.TabIndex = 13;
            this.dtpMonth.ValueChanged += new System.EventHandler(this.dtpMonth_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbSun);
            this.panel1.Controls.Add(this.tbSat);
            this.panel1.Controls.Add(this.tbFri);
            this.panel1.Controls.Add(this.tbThr);
            this.panel1.Controls.Add(this.tbWed);
            this.panel1.Controls.Add(this.tbTue);
            this.panel1.Controls.Add(this.tbMon);
            this.panel1.Location = new System.Drawing.Point(25, 267);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 295);
            this.panel1.TabIndex = 14;
            // 
            // tbSun
            // 
            this.tbSun.BackColor = System.Drawing.Color.Lavender;
            this.tbSun.Location = new System.Drawing.Point(545, 0);
            this.tbSun.Name = "tbSun";
            this.tbSun.ReadOnly = true;
            this.tbSun.Size = new System.Drawing.Size(90, 20);
            this.tbSun.TabIndex = 21;
            this.tbSun.Text = "Sun";
            this.tbSun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSat
            // 
            this.tbSat.BackColor = System.Drawing.Color.Lavender;
            this.tbSat.Location = new System.Drawing.Point(455, 0);
            this.tbSat.Name = "tbSat";
            this.tbSat.ReadOnly = true;
            this.tbSat.Size = new System.Drawing.Size(90, 20);
            this.tbSat.TabIndex = 20;
            this.tbSat.Text = "Sat";
            this.tbSat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbFri
            // 
            this.tbFri.BackColor = System.Drawing.Color.Lavender;
            this.tbFri.Location = new System.Drawing.Point(365, 0);
            this.tbFri.Name = "tbFri";
            this.tbFri.ReadOnly = true;
            this.tbFri.Size = new System.Drawing.Size(90, 20);
            this.tbFri.TabIndex = 19;
            this.tbFri.Text = "Fri";
            this.tbFri.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbThr
            // 
            this.tbThr.BackColor = System.Drawing.Color.Lavender;
            this.tbThr.Location = new System.Drawing.Point(275, 0);
            this.tbThr.Name = "tbThr";
            this.tbThr.ReadOnly = true;
            this.tbThr.Size = new System.Drawing.Size(90, 20);
            this.tbThr.TabIndex = 18;
            this.tbThr.Text = "Thr";
            this.tbThr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbWed
            // 
            this.tbWed.BackColor = System.Drawing.Color.Lavender;
            this.tbWed.Location = new System.Drawing.Point(185, 0);
            this.tbWed.Name = "tbWed";
            this.tbWed.ReadOnly = true;
            this.tbWed.Size = new System.Drawing.Size(90, 20);
            this.tbWed.TabIndex = 17;
            this.tbWed.Text = "Wed";
            this.tbWed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbTue
            // 
            this.tbTue.BackColor = System.Drawing.Color.Lavender;
            this.tbTue.Location = new System.Drawing.Point(95, 0);
            this.tbTue.Name = "tbTue";
            this.tbTue.ReadOnly = true;
            this.tbTue.Size = new System.Drawing.Size(90, 20);
            this.tbTue.TabIndex = 16;
            this.tbTue.Text = "Tue";
            this.tbTue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbMon
            // 
            this.tbMon.BackColor = System.Drawing.Color.Lavender;
            this.tbMon.Location = new System.Drawing.Point(5, 0);
            this.tbMon.Name = "tbMon";
            this.tbMon.ReadOnly = true;
            this.tbMon.Size = new System.Drawing.Size(90, 20);
            this.tbMon.TabIndex = 15;
            this.tbMon.Text = "Mon";
            this.tbMon.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gbRoutes
            // 
            this.gbRoutes.Controls.Add(this.lvDetails);
            this.gbRoutes.Controls.Add(this.cbRouteName);
            this.gbRoutes.Controls.Add(this.lblRouteName);
            this.gbRoutes.Location = new System.Drawing.Point(330, 19);
            this.gbRoutes.Name = "gbRoutes";
            this.gbRoutes.Size = new System.Drawing.Size(344, 201);
            this.gbRoutes.TabIndex = 7;
            this.gbRoutes.TabStop = false;
            this.gbRoutes.Text = "Routes";
            // 
            // lvDetails
            // 
            this.lvDetails.FullRowSelect = true;
            this.lvDetails.GridLines = true;
            this.lvDetails.HideSelection = false;
            this.lvDetails.Location = new System.Drawing.Point(9, 52);
            this.lvDetails.Name = "lvDetails";
            this.lvDetails.ShowItemToolTips = true;
            this.lvDetails.Size = new System.Drawing.Size(326, 134);
            this.lvDetails.TabIndex = 10;
            this.lvDetails.UseCompatibleStateImageBehavior = false;
            this.lvDetails.View = System.Windows.Forms.View.Details;
            this.lvDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDetails_ColumnClick);
            // 
            // cbRouteName
            // 
            this.cbRouteName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRouteName.FormattingEnabled = true;
            this.cbRouteName.Location = new System.Drawing.Point(62, 19);
            this.cbRouteName.Name = "cbRouteName";
            this.cbRouteName.Size = new System.Drawing.Size(157, 21);
            this.cbRouteName.TabIndex = 9;
            this.cbRouteName.SelectedIndexChanged += new System.EventHandler(this.cbRouteName_SelectedIndexChanged);
            // 
            // lblRouteName
            // 
            this.lblRouteName.Location = new System.Drawing.Point(7, 20);
            this.lblRouteName.Name = "lblRouteName";
            this.lblRouteName.Size = new System.Drawing.Size(53, 17);
            this.lblRouteName.TabIndex = 8;
            this.lblRouteName.Text = "Route:";
            this.lblRouteName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbEmployee
            // 
            this.gbEmployee.Controls.Add(this.cbWU);
            this.gbEmployee.Controls.Add(this.cbEmpl);
            this.gbEmployee.Controls.Add(this.lblWU);
            this.gbEmployee.Controls.Add(this.lblEmpl);
            this.gbEmployee.Controls.Add(this.btnWUTree);
            this.gbEmployee.Location = new System.Drawing.Point(18, 19);
            this.gbEmployee.Name = "gbEmployee";
            this.gbEmployee.Size = new System.Drawing.Size(306, 92);
            this.gbEmployee.TabIndex = 1;
            this.gbEmployee.TabStop = false;
            this.gbEmployee.Text = "Employee";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 595);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClearSelDays
            // 
            this.btnClearSelDays.Location = new System.Drawing.Point(100, 595);
            this.btnClearSelDays.Name = "btnClearSelDays";
            this.btnClearSelDays.Size = new System.Drawing.Size(149, 23);
            this.btnClearSelDays.TabIndex = 23;
            this.btnClearSelDays.Text = "Clear selected days";
            this.btnClearSelDays.Click += new System.EventHandler(this.btnClearSelDays_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.Control;
            this.btnClear.Location = new System.Drawing.Point(261, 595);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // SecurityRoutesScheduleAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 630);
            this.ControlBox = false;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClearSelDays);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbAssignRoutes);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "SecurityRoutesScheduleAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "SecurityRoutesScheduleAdd";
            this.Load += new System.EventHandler(this.SecurityRoutesScheduleAdd_Load);
            this.Closed += new System.EventHandler(this.SecurityRoutesScheduleAdd_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SecurityRoutesScheduleAdd_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SecurityRoutesScheduleAdd_KeyDown);
            this.gbAssignRoutes.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbRoutes.ResumeLayout(false);
            this.gbEmployee.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.ComboBox cbEmpl;
        private System.Windows.Forms.Label lblEmpl;
        private System.Windows.Forms.GroupBox gbAssignRoutes;
        private System.Windows.Forms.GroupBox gbRoutes;
        private System.Windows.Forms.GroupBox gbEmployee;
        private System.Windows.Forms.ComboBox cbRouteName;
        private System.Windows.Forms.Label lblRouteName;
        private System.Windows.Forms.ListView lvDetails;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.DateTimePicker dtpMonth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbSun;
        private System.Windows.Forms.TextBox tbSat;
        private System.Windows.Forms.TextBox tbFri;
        private System.Windows.Forms.TextBox tbThr;
        private System.Windows.Forms.TextBox tbWed;
        private System.Windows.Forms.TextBox tbTue;
        private System.Windows.Forms.TextBox tbMon;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAssign;
        private System.Windows.Forms.Button btnClearSelDays;
        private System.Windows.Forms.Button btnClear;
    }
}