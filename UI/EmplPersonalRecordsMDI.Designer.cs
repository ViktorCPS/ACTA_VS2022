namespace UI
{
    partial class EmplPersonalRecordsMDI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmplPersonalRecordsMDI));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.windowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maximizeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.cbEmplName = new System.Windows.Forms.ComboBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCriteriaShange = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.tcChildForms = new System.Windows.Forms.TabControl();
            this.Passes = new System.Windows.Forms.TabPage();
            this.IOPairs = new System.Windows.Forms.TabPage();
            this.EmployeeAbsences = new System.Windows.Forms.TabPage();
            this.ExitPermissions = new System.Windows.Forms.TabPage();
            this.ExtraHours = new System.Windows.Forms.TabPage();
            this.VacationEvidence = new System.Windows.Forms.TabPage();
            this.EmployeesReports = new System.Windows.Forms.TabPage();
            this.EmployeeAnaliticReport = new System.Windows.Forms.TabPage();
            this.menuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tcChildForms.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MdiWindowListItem = this.windowsMenu;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(1002, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // windowsMenu
            // 
            this.windowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileVerticalToolStripMenuItem,
            this.tileHorizontalToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.minimizeAllToolStripMenuItem,
            this.maximizeAllToolStripMenuItem});
            this.windowsMenu.Name = "windowsMenu";
            this.windowsMenu.Size = new System.Drawing.Size(62, 20);
            this.windowsMenu.Text = "&Windows";
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cascadeToolStripMenuItem.Text = "&Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.CascadeToolStripMenuItem_Click);
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tileVerticalToolStripMenuItem.Text = "Tile &Vertical";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.TileVerticleToolStripMenuItem_Click);
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tileHorizontalToolStripMenuItem.Text = "Tile &Horizontal";
            this.tileHorizontalToolStripMenuItem.Click += new System.EventHandler(this.TileHorizontalToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeAllToolStripMenuItem.Text = "C&lose All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.CloseAllToolStripMenuItem_Click);
            // 
            // minimizeAllToolStripMenuItem
            // 
            this.minimizeAllToolStripMenuItem.Name = "minimizeAllToolStripMenuItem";
            this.minimizeAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.minimizeAllToolStripMenuItem.Text = "Minimize All";
            this.minimizeAllToolStripMenuItem.Click += new System.EventHandler(this.minimizeAllToolStripMenuItem_Click);
            // 
            // maximizeAllToolStripMenuItem
            // 
            this.maximizeAllToolStripMenuItem.Name = "maximizeAllToolStripMenuItem";
            this.maximizeAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.maximizeAllToolStripMenuItem.Text = "Maximize All";
            this.maximizeAllToolStripMenuItem.Click += new System.EventHandler(this.maximizeAllToolStripMenuItem_Click);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(218, -2);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 4;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(328, -1);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(246, 21);
            this.cbWorkingUnit.TabIndex = 5;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(706, -1);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(71, 23);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "First Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmplName
            // 
            this.cbEmplName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmplName.Location = new System.Drawing.Point(783, 0);
            this.cbEmplName.Name = "cbEmplName";
            this.cbEmplName.Size = new System.Drawing.Size(170, 21);
            this.cbEmplName.TabIndex = 9;
            this.cbEmplName.SelectedIndexChanged += new System.EventHandler(this.cbEmplName_SelectedIndexChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(580, 0);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 22);
            this.btnWUTree.TabIndex = 13;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(611, -1);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(89, 22);
            this.chbHierarhicly.TabIndex = 14;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCriteriaShange);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.lblTo);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Controls.Add(this.lblFrom);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 33);
            this.panel1.TabIndex = 16;
            // 
            // lblCriteriaShange
            // 
            this.lblCriteriaShange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCriteriaShange.Location = new System.Drawing.Point(4, 1);
            this.lblCriteriaShange.Name = "lblCriteriaShange";
            this.lblCriteriaShange.Size = new System.Drawing.Size(249, 35);
            this.lblCriteriaShange.TabIndex = 16;
            this.lblCriteriaShange.Text = "New criteria will be implemeted on window search after activation of the window";
            this.lblCriteriaShange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(783, 3);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(170, 20);
            this.dtpTo.TabIndex = 15;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(706, 3);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(72, 21);
            this.lblTo.TabIndex = 14;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpFrom.Location = new System.Drawing.Point(328, 3);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(163, 20);
            this.dtpFrom.TabIndex = 13;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(259, 3);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(63, 21);
            this.lblFrom.TabIndex = 12;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tcChildForms
            // 
            this.tcChildForms.Controls.Add(this.Passes);
            this.tcChildForms.Controls.Add(this.IOPairs);
            this.tcChildForms.Controls.Add(this.EmployeeAbsences);
            this.tcChildForms.Controls.Add(this.ExitPermissions);
            this.tcChildForms.Controls.Add(this.ExtraHours);
            this.tcChildForms.Controls.Add(this.VacationEvidence);
            this.tcChildForms.Controls.Add(this.EmployeesReports);
            this.tcChildForms.Controls.Add(this.EmployeeAnaliticReport);
            this.tcChildForms.Dock = System.Windows.Forms.DockStyle.Top;
            this.tcChildForms.Location = new System.Drawing.Point(0, 57);
            this.tcChildForms.Name = "tcChildForms";
            this.tcChildForms.SelectedIndex = 0;
            this.tcChildForms.Size = new System.Drawing.Size(1002, 26);
            this.tcChildForms.TabIndex = 18;
            this.tcChildForms.SelectedIndexChanged += new System.EventHandler(this.tcChildForms_SelectedIndexChanged);
            // 
            // Passes
            // 
            this.Passes.Location = new System.Drawing.Point(4, 22);
            this.Passes.Name = "Passes";
            this.Passes.Padding = new System.Windows.Forms.Padding(3);
            this.Passes.Size = new System.Drawing.Size(994, 0);
            this.Passes.TabIndex = 0;
            this.Passes.Text = "Passes";
            this.Passes.UseVisualStyleBackColor = true;
            // 
            // IOPairs
            // 
            this.IOPairs.Location = new System.Drawing.Point(4, 22);
            this.IOPairs.Name = "IOPairs";
            this.IOPairs.Padding = new System.Windows.Forms.Padding(3);
            this.IOPairs.Size = new System.Drawing.Size(994, 0);
            this.IOPairs.TabIndex = 1;
            this.IOPairs.Text = "IOPairs";
            this.IOPairs.UseVisualStyleBackColor = true;
            // 
            // EmployeeAbsences
            // 
            this.EmployeeAbsences.Location = new System.Drawing.Point(4, 22);
            this.EmployeeAbsences.Name = "EmployeeAbsences";
            this.EmployeeAbsences.Padding = new System.Windows.Forms.Padding(3);
            this.EmployeeAbsences.Size = new System.Drawing.Size(994, 0);
            this.EmployeeAbsences.TabIndex = 2;
            this.EmployeeAbsences.Text = "Absences";
            this.EmployeeAbsences.UseVisualStyleBackColor = true;
            // 
            // ExitPermissions
            // 
            this.ExitPermissions.Location = new System.Drawing.Point(4, 22);
            this.ExitPermissions.Name = "ExitPermissions";
            this.ExitPermissions.Padding = new System.Windows.Forms.Padding(3);
            this.ExitPermissions.Size = new System.Drawing.Size(994, 0);
            this.ExitPermissions.TabIndex = 3;
            this.ExitPermissions.Text = "ExitPermissions";
            this.ExitPermissions.UseVisualStyleBackColor = true;
            // 
            // ExtraHours
            // 
            this.ExtraHours.Location = new System.Drawing.Point(4, 22);
            this.ExtraHours.Name = "ExtraHours";
            this.ExtraHours.Padding = new System.Windows.Forms.Padding(3);
            this.ExtraHours.Size = new System.Drawing.Size(994, 0);
            this.ExtraHours.TabIndex = 4;
            this.ExtraHours.Text = "ExtraHours";
            this.ExtraHours.UseVisualStyleBackColor = true;
            // 
            // VacationEvidence
            // 
            this.VacationEvidence.Location = new System.Drawing.Point(4, 22);
            this.VacationEvidence.Name = "VacationEvidence";
            this.VacationEvidence.Padding = new System.Windows.Forms.Padding(3);
            this.VacationEvidence.Size = new System.Drawing.Size(994, 0);
            this.VacationEvidence.TabIndex = 5;
            this.VacationEvidence.Text = "VacationEvidence";
            this.VacationEvidence.UseVisualStyleBackColor = true;
            // 
            // EmployeesReports
            // 
            this.EmployeesReports.Location = new System.Drawing.Point(4, 22);
            this.EmployeesReports.Name = "EmployeesReports";
            this.EmployeesReports.Padding = new System.Windows.Forms.Padding(3);
            this.EmployeesReports.Size = new System.Drawing.Size(994, 0);
            this.EmployeesReports.TabIndex = 6;
            this.EmployeesReports.Text = "EmployeesReports";
            this.EmployeesReports.UseVisualStyleBackColor = true;
            // 
            // EmployeeAnaliticReport
            // 
            this.EmployeeAnaliticReport.Location = new System.Drawing.Point(4, 22);
            this.EmployeeAnaliticReport.Name = "EmployeeAnaliticReport";
            this.EmployeeAnaliticReport.Padding = new System.Windows.Forms.Padding(3);
            this.EmployeeAnaliticReport.Size = new System.Drawing.Size(994, 0);
            this.EmployeeAnaliticReport.TabIndex = 7;
            this.EmployeeAnaliticReport.Text = "EmployeeAnaliticReport";
            this.EmployeeAnaliticReport.UseVisualStyleBackColor = true;
            // 
            // EmplPersonalRecordsMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 723);
            this.Controls.Add(this.tcChildForms);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chbHierarhicly);
            this.Controls.Add(this.btnWUTree);
            this.Controls.Add(this.cbEmplName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cbWorkingUnit);
            this.Controls.Add(this.lblWorkingUnit);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "EmplPersonalRecordsMDI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "EmplPersonalRecordsMDI";
            this.Load += new System.EventHandler(this.EmplPersonalRecordsMDI_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmplPersonalRecordsMDI_KeyUp);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tcChildForms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsMenu;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbEmplName;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.ToolStripMenuItem minimizeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maximizeAllToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tcChildForms;
        private System.Windows.Forms.TabPage Passes;
        private System.Windows.Forms.TabPage IOPairs;
        private System.Windows.Forms.TabPage EmployeeAbsences;
        private System.Windows.Forms.TabPage ExitPermissions;
        private System.Windows.Forms.TabPage ExtraHours;
        private System.Windows.Forms.TabPage VacationEvidence;
        private System.Windows.Forms.TabPage EmployeesReports;
        private System.Windows.Forms.TabPage EmployeeAnaliticReport;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblCriteriaShange;
    }
}



                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             