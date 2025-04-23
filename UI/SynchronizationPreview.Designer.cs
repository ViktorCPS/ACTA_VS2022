namespace UI
{
    partial class SynchronizationPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SynchronizationPreview));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFS = new System.Windows.Forms.TabPage();
            this.lvWU = new System.Windows.Forms.ListView();
            this.lblWU = new System.Windows.Forms.Label();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.tabOU = new System.Windows.Forms.TabPage();
            this.lvOU = new System.Windows.Forms.ListView();
            this.lblOU = new System.Windows.Forms.Label();
            this.btnOUTree = new System.Windows.Forms.Button();
            this.cbOU = new System.Windows.Forms.ComboBox();
            this.tabCC = new System.Windows.Forms.TabPage();
            this.lvCC = new System.Windows.Forms.ListView();
            this.lblCCCode = new System.Windows.Forms.Label();
            this.cbCCCode = new System.Windows.Forms.ComboBox();
            this.tabPositions = new System.Windows.Forms.TabPage();
            this.lvPosition = new System.Windows.Forms.ListView();
            this.lblPosition = new System.Windows.Forms.Label();
            this.cbPosition = new System.Windows.Forms.ComboBox();
            this.tabEmployees = new System.Windows.Forms.TabPage();
            this.lblEmplEmployee = new System.Windows.Forms.Label();
            this.cbEmplEmployee = new System.Windows.Forms.ComboBox();
            this.gbEmplUnitFilter = new System.Windows.Forms.GroupBox();
            this.btnEmplOUTree = new System.Windows.Forms.Button();
            this.cbEmplOU = new System.Windows.Forms.ComboBox();
            this.btnEmplWUTree = new System.Windows.Forms.Button();
            this.cbEmplWU = new System.Windows.Forms.ComboBox();
            this.rbEmplOU = new System.Windows.Forms.RadioButton();
            this.rbEmplFS = new System.Windows.Forms.RadioButton();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.tabResponsibility = new System.Windows.Forms.TabPage();
            this.lblResEmployee = new System.Windows.Forms.Label();
            this.cbResEmployee = new System.Windows.Forms.ComboBox();
            this.gbResUnitFilter = new System.Windows.Forms.GroupBox();
            this.btnResOUTree = new System.Windows.Forms.Button();
            this.cbResOU = new System.Windows.Forms.ComboBox();
            this.btnResWUTree = new System.Windows.Forms.Button();
            this.cbResWU = new System.Windows.Forms.ComboBox();
            this.rbResOU = new System.Windows.Forms.RadioButton();
            this.rbResFS = new System.Windows.Forms.RadioButton();
            this.lvResponsibilities = new System.Windows.Forms.ListView();
            this.tabALRecalc = new System.Windows.Forms.TabPage();
            this.lblALEmployee = new System.Windows.Forms.Label();
            this.cbALEmployee = new System.Windows.Forms.ComboBox();
            this.gbALUnitFilter = new System.Windows.Forms.GroupBox();
            this.btnALOUTree = new System.Windows.Forms.Button();
            this.cbALOU = new System.Windows.Forms.ComboBox();
            this.btnALWUTree = new System.Windows.Forms.Button();
            this.cbALWU = new System.Windows.Forms.ComboBox();
            this.rbALOU = new System.Windows.Forms.RadioButton();
            this.rbALFS = new System.Windows.Forms.RadioButton();
            this.lvALRecalculation = new System.Windows.Forms.ListView();
            this.tabALApproval = new System.Windows.Forms.TabPage();
            this.btnSearchALRecalc = new System.Windows.Forms.Button();
            this.btnNotApprove = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.lblALAppEmployee = new System.Windows.Forms.Label();
            this.cbALAppEmployee = new System.Windows.Forms.ComboBox();
            this.gbALAppUnitFilter = new System.Windows.Forms.GroupBox();
            this.btnALAppOUTree = new System.Windows.Forms.Button();
            this.cbALAppOU = new System.Windows.Forms.ComboBox();
            this.btnALAppWUTree = new System.Windows.Forms.Button();
            this.cbALAppWU = new System.Windows.Forms.ComboBox();
            this.rbALAppOU = new System.Windows.Forms.RadioButton();
            this.rbALAppFS = new System.Windows.Forms.RadioButton();
            this.lvALRecalc = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblResult = new System.Windows.Forms.Label();
            this.cbResult = new System.Windows.Forms.ComboBox();
            this.gbCriteria = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabFS.SuspendLayout();
            this.tabOU.SuspendLayout();
            this.tabCC.SuspendLayout();
            this.tabPositions.SuspendLayout();
            this.tabEmployees.SuspendLayout();
            this.gbEmplUnitFilter.SuspendLayout();
            this.tabResponsibility.SuspendLayout();
            this.gbResUnitFilter.SuspendLayout();
            this.tabALRecalc.SuspendLayout();
            this.gbALUnitFilter.SuspendLayout();
            this.tabALApproval.SuspendLayout();
            this.gbALAppUnitFilter.SuspendLayout();
            this.gbCriteria.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabFS);
            this.tabControl.Controls.Add(this.tabOU);
            this.tabControl.Controls.Add(this.tabCC);
            this.tabControl.Controls.Add(this.tabPositions);
            this.tabControl.Controls.Add(this.tabEmployees);
            this.tabControl.Controls.Add(this.tabResponsibility);
            this.tabControl.Controls.Add(this.tabALRecalc);
            this.tabControl.Controls.Add(this.tabALApproval);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(920, 515);
            this.tabControl.TabIndex = 0;
            // 
            // tabFS
            // 
            this.tabFS.Controls.Add(this.lvWU);
            this.tabFS.Controls.Add(this.lblWU);
            this.tabFS.Controls.Add(this.btnWUTree);
            this.tabFS.Controls.Add(this.cbWU);
            this.tabFS.Location = new System.Drawing.Point(4, 22);
            this.tabFS.Name = "tabFS";
            this.tabFS.Padding = new System.Windows.Forms.Padding(3);
            this.tabFS.Size = new System.Drawing.Size(912, 489);
            this.tabFS.TabIndex = 0;
            this.tabFS.Text = "FS";
            this.tabFS.UseVisualStyleBackColor = true;
            // 
            // lvWU
            // 
            this.lvWU.FullRowSelect = true;
            this.lvWU.GridLines = true;
            this.lvWU.HideSelection = false;
            this.lvWU.Location = new System.Drawing.Point(10, 50);
            this.lvWU.Name = "lvWU";
            this.lvWU.ShowItemToolTips = true;
            this.lvWU.Size = new System.Drawing.Size(890, 425);
            this.lvWU.TabIndex = 3;
            this.lvWU.UseCompatibleStateImageBehavior = false;
            this.lvWU.View = System.Windows.Forms.View.Details;
            this.lvWU.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvWU_ColumnClick);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(10, 20);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(120, 15);
            this.lblWU.TabIndex = 0;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(363, 16);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 2;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(136, 18);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(221, 21);
            this.cbWU.TabIndex = 1;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // tabOU
            // 
            this.tabOU.Controls.Add(this.lvOU);
            this.tabOU.Controls.Add(this.lblOU);
            this.tabOU.Controls.Add(this.btnOUTree);
            this.tabOU.Controls.Add(this.cbOU);
            this.tabOU.Location = new System.Drawing.Point(4, 22);
            this.tabOU.Name = "tabOU";
            this.tabOU.Padding = new System.Windows.Forms.Padding(3);
            this.tabOU.Size = new System.Drawing.Size(912, 489);
            this.tabOU.TabIndex = 1;
            this.tabOU.Text = "OU";
            this.tabOU.UseVisualStyleBackColor = true;
            // 
            // lvOU
            // 
            this.lvOU.FullRowSelect = true;
            this.lvOU.GridLines = true;
            this.lvOU.HideSelection = false;
            this.lvOU.Location = new System.Drawing.Point(10, 50);
            this.lvOU.Name = "lvOU";
            this.lvOU.ShowItemToolTips = true;
            this.lvOU.Size = new System.Drawing.Size(890, 425);
            this.lvOU.TabIndex = 3;
            this.lvOU.UseCompatibleStateImageBehavior = false;
            this.lvOU.View = System.Windows.Forms.View.Details;
            this.lvOU.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOU_ColumnClick);
            // 
            // lblOU
            // 
            this.lblOU.Location = new System.Drawing.Point(10, 20);
            this.lblOU.Name = "lblOU";
            this.lblOU.Size = new System.Drawing.Size(120, 15);
            this.lblOU.TabIndex = 0;
            this.lblOU.Text = "Organizational unit:";
            this.lblOU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOUTree
            // 
            this.btnOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnOUTree.Image")));
            this.btnOUTree.Location = new System.Drawing.Point(363, 16);
            this.btnOUTree.Name = "btnOUTree";
            this.btnOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnOUTree.TabIndex = 2;
            this.btnOUTree.Click += new System.EventHandler(this.btnOUTree_Click);
            // 
            // cbOU
            // 
            this.cbOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOU.FormattingEnabled = true;
            this.cbOU.Location = new System.Drawing.Point(136, 18);
            this.cbOU.Name = "cbOU";
            this.cbOU.Size = new System.Drawing.Size(221, 21);
            this.cbOU.TabIndex = 1;
            this.cbOU.SelectedIndexChanged += new System.EventHandler(this.cbOU_SelectedIndexChanged);
            // 
            // tabCC
            // 
            this.tabCC.Controls.Add(this.lvCC);
            this.tabCC.Controls.Add(this.lblCCCode);
            this.tabCC.Controls.Add(this.cbCCCode);
            this.tabCC.Location = new System.Drawing.Point(4, 22);
            this.tabCC.Name = "tabCC";
            this.tabCC.Size = new System.Drawing.Size(912, 489);
            this.tabCC.TabIndex = 2;
            this.tabCC.Text = "CC";
            this.tabCC.UseVisualStyleBackColor = true;
            // 
            // lvCC
            // 
            this.lvCC.FullRowSelect = true;
            this.lvCC.GridLines = true;
            this.lvCC.HideSelection = false;
            this.lvCC.Location = new System.Drawing.Point(10, 50);
            this.lvCC.Name = "lvCC";
            this.lvCC.ShowItemToolTips = true;
            this.lvCC.Size = new System.Drawing.Size(890, 425);
            this.lvCC.TabIndex = 2;
            this.lvCC.UseCompatibleStateImageBehavior = false;
            this.lvCC.View = System.Windows.Forms.View.Details;
            this.lvCC.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvCC_ColumnClick);
            // 
            // lblCCCode
            // 
            this.lblCCCode.Location = new System.Drawing.Point(10, 20);
            this.lblCCCode.Name = "lblCCCode";
            this.lblCCCode.Size = new System.Drawing.Size(120, 15);
            this.lblCCCode.TabIndex = 0;
            this.lblCCCode.Text = "Cost Center Code:";
            this.lblCCCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCCCode
            // 
            this.cbCCCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCCCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCCCode.FormattingEnabled = true;
            this.cbCCCode.Location = new System.Drawing.Point(136, 18);
            this.cbCCCode.Name = "cbCCCode";
            this.cbCCCode.Size = new System.Drawing.Size(221, 21);
            this.cbCCCode.TabIndex = 1;
            this.cbCCCode.SelectedIndexChanged += new System.EventHandler(this.cbCCCode_SelectedIndexChanged);
            // 
            // tabPositions
            // 
            this.tabPositions.Controls.Add(this.lvPosition);
            this.tabPositions.Controls.Add(this.lblPosition);
            this.tabPositions.Controls.Add(this.cbPosition);
            this.tabPositions.Location = new System.Drawing.Point(4, 22);
            this.tabPositions.Name = "tabPositions";
            this.tabPositions.Size = new System.Drawing.Size(912, 489);
            this.tabPositions.TabIndex = 3;
            this.tabPositions.Text = "Positions";
            this.tabPositions.UseVisualStyleBackColor = true;
            // 
            // lvPosition
            // 
            this.lvPosition.FullRowSelect = true;
            this.lvPosition.GridLines = true;
            this.lvPosition.HideSelection = false;
            this.lvPosition.Location = new System.Drawing.Point(10, 50);
            this.lvPosition.Name = "lvPosition";
            this.lvPosition.ShowItemToolTips = true;
            this.lvPosition.Size = new System.Drawing.Size(890, 425);
            this.lvPosition.TabIndex = 2;
            this.lvPosition.UseCompatibleStateImageBehavior = false;
            this.lvPosition.View = System.Windows.Forms.View.Details;
            this.lvPosition.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvPosition_ColumnClick);
            // 
            // lblPosition
            // 
            this.lblPosition.Location = new System.Drawing.Point(10, 20);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(120, 15);
            this.lblPosition.TabIndex = 0;
            this.lblPosition.Text = "Position Code:";
            this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPosition
            // 
            this.cbPosition.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbPosition.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbPosition.FormattingEnabled = true;
            this.cbPosition.Location = new System.Drawing.Point(136, 18);
            this.cbPosition.Name = "cbPosition";
            this.cbPosition.Size = new System.Drawing.Size(221, 21);
            this.cbPosition.TabIndex = 1;
            this.cbPosition.SelectedIndexChanged += new System.EventHandler(this.cbPosition_SelectedIndexChanged);
            // 
            // tabEmployees
            // 
            this.tabEmployees.Controls.Add(this.lblEmplEmployee);
            this.tabEmployees.Controls.Add(this.cbEmplEmployee);
            this.tabEmployees.Controls.Add(this.gbEmplUnitFilter);
            this.tabEmployees.Controls.Add(this.lvEmployees);
            this.tabEmployees.Location = new System.Drawing.Point(4, 22);
            this.tabEmployees.Name = "tabEmployees";
            this.tabEmployees.Size = new System.Drawing.Size(912, 489);
            this.tabEmployees.TabIndex = 4;
            this.tabEmployees.Text = "Employees";
            this.tabEmployees.UseVisualStyleBackColor = true;
            // 
            // lblEmplEmployee
            // 
            this.lblEmplEmployee.Location = new System.Drawing.Point(350, 52);
            this.lblEmplEmployee.Name = "lblEmplEmployee";
            this.lblEmplEmployee.Size = new System.Drawing.Size(82, 15);
            this.lblEmplEmployee.TabIndex = 1;
            this.lblEmplEmployee.Text = "Employee:";
            this.lblEmplEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmplEmployee
            // 
            this.cbEmplEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmplEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmplEmployee.FormattingEnabled = true;
            this.cbEmplEmployee.Location = new System.Drawing.Point(438, 50);
            this.cbEmplEmployee.Name = "cbEmplEmployee";
            this.cbEmplEmployee.Size = new System.Drawing.Size(221, 21);
            this.cbEmplEmployee.TabIndex = 2;
            this.cbEmplEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmplEmployee_SelectedIndexChanged);
            // 
            // gbEmplUnitFilter
            // 
            this.gbEmplUnitFilter.Controls.Add(this.btnEmplOUTree);
            this.gbEmplUnitFilter.Controls.Add(this.cbEmplOU);
            this.gbEmplUnitFilter.Controls.Add(this.btnEmplWUTree);
            this.gbEmplUnitFilter.Controls.Add(this.cbEmplWU);
            this.gbEmplUnitFilter.Controls.Add(this.rbEmplOU);
            this.gbEmplUnitFilter.Controls.Add(this.rbEmplFS);
            this.gbEmplUnitFilter.Location = new System.Drawing.Point(10, 5);
            this.gbEmplUnitFilter.Name = "gbEmplUnitFilter";
            this.gbEmplUnitFilter.Size = new System.Drawing.Size(334, 82);
            this.gbEmplUnitFilter.TabIndex = 0;
            this.gbEmplUnitFilter.TabStop = false;
            this.gbEmplUnitFilter.Text = "Unit filter";
            // 
            // btnEmplOUTree
            // 
            this.btnEmplOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnEmplOUTree.Image")));
            this.btnEmplOUTree.Location = new System.Drawing.Point(296, 43);
            this.btnEmplOUTree.Name = "btnEmplOUTree";
            this.btnEmplOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnEmplOUTree.TabIndex = 5;
            this.btnEmplOUTree.Click += new System.EventHandler(this.btnEmplOUTree_Click);
            // 
            // cbEmplOU
            // 
            this.cbEmplOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmplOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmplOU.FormattingEnabled = true;
            this.cbEmplOU.Location = new System.Drawing.Point(69, 45);
            this.cbEmplOU.Name = "cbEmplOU";
            this.cbEmplOU.Size = new System.Drawing.Size(221, 21);
            this.cbEmplOU.TabIndex = 4;
            this.cbEmplOU.SelectedIndexChanged += new System.EventHandler(this.cbEmplOU_SelectedIndexChanged);
            // 
            // btnEmplWUTree
            // 
            this.btnEmplWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnEmplWUTree.Image")));
            this.btnEmplWUTree.Location = new System.Drawing.Point(296, 16);
            this.btnEmplWUTree.Name = "btnEmplWUTree";
            this.btnEmplWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnEmplWUTree.TabIndex = 2;
            this.btnEmplWUTree.Click += new System.EventHandler(this.btnEmplWUTree_Click);
            // 
            // cbEmplWU
            // 
            this.cbEmplWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmplWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmplWU.FormattingEnabled = true;
            this.cbEmplWU.Location = new System.Drawing.Point(69, 18);
            this.cbEmplWU.Name = "cbEmplWU";
            this.cbEmplWU.Size = new System.Drawing.Size(221, 21);
            this.cbEmplWU.TabIndex = 1;
            this.cbEmplWU.SelectedIndexChanged += new System.EventHandler(this.cbEmplWU_SelectedIndexChanged);
            // 
            // rbEmplOU
            // 
            this.rbEmplOU.AutoSize = true;
            this.rbEmplOU.Location = new System.Drawing.Point(20, 46);
            this.rbEmplOU.Name = "rbEmplOU";
            this.rbEmplOU.Size = new System.Drawing.Size(41, 17);
            this.rbEmplOU.TabIndex = 3;
            this.rbEmplOU.TabStop = true;
            this.rbEmplOU.Text = "OU";
            this.rbEmplOU.UseVisualStyleBackColor = true;
            this.rbEmplOU.CheckedChanged += new System.EventHandler(this.rbEmplOU_CheckedChanged);
            // 
            // rbEmplFS
            // 
            this.rbEmplFS.AutoSize = true;
            this.rbEmplFS.Location = new System.Drawing.Point(20, 19);
            this.rbEmplFS.Name = "rbEmplFS";
            this.rbEmplFS.Size = new System.Drawing.Size(38, 17);
            this.rbEmplFS.TabIndex = 0;
            this.rbEmplFS.TabStop = true;
            this.rbEmplFS.Text = "FS";
            this.rbEmplFS.UseVisualStyleBackColor = true;
            this.rbEmplFS.CheckedChanged += new System.EventHandler(this.rbEmplFS_CheckedChanged);
            // 
            // lvEmployees
            // 
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.GridLines = true;
            this.lvEmployees.HideSelection = false;
            this.lvEmployees.Location = new System.Drawing.Point(10, 95);
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.ShowItemToolTips = true;
            this.lvEmployees.Size = new System.Drawing.Size(890, 385);
            this.lvEmployees.TabIndex = 3;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployees_ColumnClick);
            // 
            // tabResponsibility
            // 
            this.tabResponsibility.Controls.Add(this.lblResEmployee);
            this.tabResponsibility.Controls.Add(this.cbResEmployee);
            this.tabResponsibility.Controls.Add(this.gbResUnitFilter);
            this.tabResponsibility.Controls.Add(this.lvResponsibilities);
            this.tabResponsibility.Location = new System.Drawing.Point(4, 22);
            this.tabResponsibility.Name = "tabResponsibility";
            this.tabResponsibility.Size = new System.Drawing.Size(912, 489);
            this.tabResponsibility.TabIndex = 5;
            this.tabResponsibility.Text = "Responsibility";
            this.tabResponsibility.UseVisualStyleBackColor = true;
            // 
            // lblResEmployee
            // 
            this.lblResEmployee.Location = new System.Drawing.Point(350, 52);
            this.lblResEmployee.Name = "lblResEmployee";
            this.lblResEmployee.Size = new System.Drawing.Size(82, 15);
            this.lblResEmployee.TabIndex = 1;
            this.lblResEmployee.Text = "Employee:";
            this.lblResEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbResEmployee
            // 
            this.cbResEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbResEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbResEmployee.FormattingEnabled = true;
            this.cbResEmployee.Location = new System.Drawing.Point(438, 50);
            this.cbResEmployee.Name = "cbResEmployee";
            this.cbResEmployee.Size = new System.Drawing.Size(221, 21);
            this.cbResEmployee.TabIndex = 2;
            this.cbResEmployee.SelectedIndexChanged += new System.EventHandler(this.cbResEmployee_SelectedIndexChanged);
            // 
            // gbResUnitFilter
            // 
            this.gbResUnitFilter.Controls.Add(this.btnResOUTree);
            this.gbResUnitFilter.Controls.Add(this.cbResOU);
            this.gbResUnitFilter.Controls.Add(this.btnResWUTree);
            this.gbResUnitFilter.Controls.Add(this.cbResWU);
            this.gbResUnitFilter.Controls.Add(this.rbResOU);
            this.gbResUnitFilter.Controls.Add(this.rbResFS);
            this.gbResUnitFilter.Location = new System.Drawing.Point(10, 5);
            this.gbResUnitFilter.Name = "gbResUnitFilter";
            this.gbResUnitFilter.Size = new System.Drawing.Size(334, 82);
            this.gbResUnitFilter.TabIndex = 0;
            this.gbResUnitFilter.TabStop = false;
            this.gbResUnitFilter.Text = "Unit filter";
            // 
            // btnResOUTree
            // 
            this.btnResOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnResOUTree.Image")));
            this.btnResOUTree.Location = new System.Drawing.Point(296, 43);
            this.btnResOUTree.Name = "btnResOUTree";
            this.btnResOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnResOUTree.TabIndex = 5;
            this.btnResOUTree.Click += new System.EventHandler(this.btnResOUTree_Click);
            // 
            // cbResOU
            // 
            this.cbResOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbResOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbResOU.FormattingEnabled = true;
            this.cbResOU.Location = new System.Drawing.Point(69, 45);
            this.cbResOU.Name = "cbResOU";
            this.cbResOU.Size = new System.Drawing.Size(221, 21);
            this.cbResOU.TabIndex = 4;
            this.cbResOU.SelectedIndexChanged += new System.EventHandler(this.cbResOU_SelectedIndexChanged);
            // 
            // btnResWUTree
            // 
            this.btnResWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnResWUTree.Image")));
            this.btnResWUTree.Location = new System.Drawing.Point(296, 16);
            this.btnResWUTree.Name = "btnResWUTree";
            this.btnResWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnResWUTree.TabIndex = 2;
            this.btnResWUTree.Click += new System.EventHandler(this.btnResWUTree_Click);
            // 
            // cbResWU
            // 
            this.cbResWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbResWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbResWU.FormattingEnabled = true;
            this.cbResWU.Location = new System.Drawing.Point(69, 18);
            this.cbResWU.Name = "cbResWU";
            this.cbResWU.Size = new System.Drawing.Size(221, 21);
            this.cbResWU.TabIndex = 1;
            this.cbResWU.SelectedIndexChanged += new System.EventHandler(this.cbResWU_SelectedIndexChanged);
            // 
            // rbResOU
            // 
            this.rbResOU.AutoSize = true;
            this.rbResOU.Location = new System.Drawing.Point(20, 46);
            this.rbResOU.Name = "rbResOU";
            this.rbResOU.Size = new System.Drawing.Size(41, 17);
            this.rbResOU.TabIndex = 3;
            this.rbResOU.TabStop = true;
            this.rbResOU.Text = "OU";
            this.rbResOU.UseVisualStyleBackColor = true;
            this.rbResOU.CheckedChanged += new System.EventHandler(this.rbResOU_CheckedChanged);
            // 
            // rbResFS
            // 
            this.rbResFS.AutoSize = true;
            this.rbResFS.Location = new System.Drawing.Point(20, 19);
            this.rbResFS.Name = "rbResFS";
            this.rbResFS.Size = new System.Drawing.Size(38, 17);
            this.rbResFS.TabIndex = 0;
            this.rbResFS.TabStop = true;
            this.rbResFS.Text = "FS";
            this.rbResFS.UseVisualStyleBackColor = true;
            this.rbResFS.CheckedChanged += new System.EventHandler(this.rbResFS_CheckedChanged);
            // 
            // lvResponsibilities
            // 
            this.lvResponsibilities.FullRowSelect = true;
            this.lvResponsibilities.GridLines = true;
            this.lvResponsibilities.HideSelection = false;
            this.lvResponsibilities.Location = new System.Drawing.Point(10, 95);
            this.lvResponsibilities.Name = "lvResponsibilities";
            this.lvResponsibilities.ShowItemToolTips = true;
            this.lvResponsibilities.Size = new System.Drawing.Size(890, 385);
            this.lvResponsibilities.TabIndex = 3;
            this.lvResponsibilities.UseCompatibleStateImageBehavior = false;
            this.lvResponsibilities.View = System.Windows.Forms.View.Details;
            this.lvResponsibilities.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResponsibilities_ColumnClick);
            // 
            // tabALRecalc
            // 
            this.tabALRecalc.Controls.Add(this.lblALEmployee);
            this.tabALRecalc.Controls.Add(this.cbALEmployee);
            this.tabALRecalc.Controls.Add(this.gbALUnitFilter);
            this.tabALRecalc.Controls.Add(this.lvALRecalculation);
            this.tabALRecalc.Location = new System.Drawing.Point(4, 22);
            this.tabALRecalc.Name = "tabALRecalc";
            this.tabALRecalc.Size = new System.Drawing.Size(912, 489);
            this.tabALRecalc.TabIndex = 6;
            this.tabALRecalc.Text = "Annual Leave Recalculation";
            this.tabALRecalc.UseVisualStyleBackColor = true;
            // 
            // lblALEmployee
            // 
            this.lblALEmployee.Location = new System.Drawing.Point(350, 52);
            this.lblALEmployee.Name = "lblALEmployee";
            this.lblALEmployee.Size = new System.Drawing.Size(82, 15);
            this.lblALEmployee.TabIndex = 1;
            this.lblALEmployee.Text = "Employee:";
            this.lblALEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbALEmployee
            // 
            this.cbALEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbALEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbALEmployee.FormattingEnabled = true;
            this.cbALEmployee.Location = new System.Drawing.Point(438, 50);
            this.cbALEmployee.Name = "cbALEmployee";
            this.cbALEmployee.Size = new System.Drawing.Size(221, 21);
            this.cbALEmployee.TabIndex = 2;
            this.cbALEmployee.SelectedIndexChanged += new System.EventHandler(this.cbALEmployee_SelectedIndexChanged);
            // 
            // gbALUnitFilter
            // 
            this.gbALUnitFilter.Controls.Add(this.btnALOUTree);
            this.gbALUnitFilter.Controls.Add(this.cbALOU);
            this.gbALUnitFilter.Controls.Add(this.btnALWUTree);
            this.gbALUnitFilter.Controls.Add(this.cbALWU);
            this.gbALUnitFilter.Controls.Add(this.rbALOU);
            this.gbALUnitFilter.Controls.Add(this.rbALFS);
            this.gbALUnitFilter.Location = new System.Drawing.Point(10, 5);
            this.gbALUnitFilter.Name = "gbALUnitFilter";
            this.gbALUnitFilter.Size = new System.Drawing.Size(334, 82);
            this.gbALUnitFilter.TabIndex = 0;
            this.gbALUnitFilter.TabStop = false;
            this.gbALUnitFilter.Text = "Unit filter";
            // 
            // btnALOUTree
            // 
            this.btnALOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnALOUTree.Image")));
            this.btnALOUTree.Location = new System.Drawing.Point(296, 43);
            this.btnALOUTree.Name = "btnALOUTree";
            this.btnALOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnALOUTree.TabIndex = 5;
            this.btnALOUTree.Click += new System.EventHandler(this.btnALOUTree_Click);
            // 
            // cbALOU
            // 
            this.cbALOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbALOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbALOU.FormattingEnabled = true;
            this.cbALOU.Location = new System.Drawing.Point(69, 45);
            this.cbALOU.Name = "cbALOU";
            this.cbALOU.Size = new System.Drawing.Size(221, 21);
            this.cbALOU.TabIndex = 4;
            this.cbALOU.SelectedIndexChanged += new System.EventHandler(this.cbALOU_SelectedIndexChanged);
            // 
            // btnALWUTree
            // 
            this.btnALWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnALWUTree.Image")));
            this.btnALWUTree.Location = new System.Drawing.Point(296, 16);
            this.btnALWUTree.Name = "btnALWUTree";
            this.btnALWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnALWUTree.TabIndex = 2;
            this.btnALWUTree.Click += new System.EventHandler(this.btnALWUTree_Click);
            // 
            // cbALWU
            // 
            this.cbALWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbALWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbALWU.FormattingEnabled = true;
            this.cbALWU.Location = new System.Drawing.Point(69, 18);
            this.cbALWU.Name = "cbALWU";
            this.cbALWU.Size = new System.Drawing.Size(221, 21);
            this.cbALWU.TabIndex = 1;
            this.cbALWU.SelectedIndexChanged += new System.EventHandler(this.cbALWU_SelectedIndexChanged);
            // 
            // rbALOU
            // 
            this.rbALOU.AutoSize = true;
            this.rbALOU.Location = new System.Drawing.Point(20, 46);
            this.rbALOU.Name = "rbALOU";
            this.rbALOU.Size = new System.Drawing.Size(41, 17);
            this.rbALOU.TabIndex = 3;
            this.rbALOU.TabStop = true;
            this.rbALOU.Text = "OU";
            this.rbALOU.UseVisualStyleBackColor = true;
            this.rbALOU.CheckedChanged += new System.EventHandler(this.rbALOU_CheckedChanged);
            // 
            // rbALFS
            // 
            this.rbALFS.AutoSize = true;
            this.rbALFS.Location = new System.Drawing.Point(20, 19);
            this.rbALFS.Name = "rbALFS";
            this.rbALFS.Size = new System.Drawing.Size(38, 17);
            this.rbALFS.TabIndex = 0;
            this.rbALFS.TabStop = true;
            this.rbALFS.Text = "FS";
            this.rbALFS.UseVisualStyleBackColor = true;
            this.rbALFS.CheckedChanged += new System.EventHandler(this.rbALFS_CheckedChanged);
            // 
            // lvALRecalculation
            // 
            this.lvALRecalculation.FullRowSelect = true;
            this.lvALRecalculation.GridLines = true;
            this.lvALRecalculation.HideSelection = false;
            this.lvALRecalculation.Location = new System.Drawing.Point(10, 95);
            this.lvALRecalculation.Name = "lvALRecalculation";
            this.lvALRecalculation.ShowItemToolTips = true;
            this.lvALRecalculation.Size = new System.Drawing.Size(890, 385);
            this.lvALRecalculation.TabIndex = 3;
            this.lvALRecalculation.UseCompatibleStateImageBehavior = false;
            this.lvALRecalculation.View = System.Windows.Forms.View.Details;
            this.lvALRecalculation.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvALRecalculation_ColumnClick);
            // 
            // tabALApproval
            // 
            this.tabALApproval.Controls.Add(this.btnSearchALRecalc);
            this.tabALApproval.Controls.Add(this.btnNotApprove);
            this.tabALApproval.Controls.Add(this.btnApprove);
            this.tabALApproval.Controls.Add(this.lblALAppEmployee);
            this.tabALApproval.Controls.Add(this.cbALAppEmployee);
            this.tabALApproval.Controls.Add(this.gbALAppUnitFilter);
            this.tabALApproval.Controls.Add(this.lvALRecalc);
            this.tabALApproval.Location = new System.Drawing.Point(4, 22);
            this.tabALApproval.Name = "tabALApproval";
            this.tabALApproval.Size = new System.Drawing.Size(912, 489);
            this.tabALApproval.TabIndex = 7;
            this.tabALApproval.Text = "Annual Leave Recalculation Approval";
            this.tabALApproval.UseVisualStyleBackColor = true;
            // 
            // btnSearchALRecalc
            // 
            this.btnSearchALRecalc.Location = new System.Drawing.Point(801, 48);
            this.btnSearchALRecalc.Name = "btnSearchALRecalc";
            this.btnSearchALRecalc.Size = new System.Drawing.Size(99, 23);
            this.btnSearchALRecalc.TabIndex = 3;
            this.btnSearchALRecalc.Text = "Search";
            this.btnSearchALRecalc.UseVisualStyleBackColor = true;
            this.btnSearchALRecalc.Click += new System.EventHandler(this.btnSearchALRecalc_Click);
            // 
            // btnNotApprove
            // 
            this.btnNotApprove.Location = new System.Drawing.Point(217, 453);
            this.btnNotApprove.Name = "btnNotApprove";
            this.btnNotApprove.Size = new System.Drawing.Size(153, 23);
            this.btnNotApprove.TabIndex = 6;
            this.btnNotApprove.Text = "Not approve";
            this.btnNotApprove.UseVisualStyleBackColor = true;
            this.btnNotApprove.Click += new System.EventHandler(this.btnNotApprove_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(11, 453);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(153, 23);
            this.btnApprove.TabIndex = 5;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // lblALAppEmployee
            // 
            this.lblALAppEmployee.Location = new System.Drawing.Point(350, 52);
            this.lblALAppEmployee.Name = "lblALAppEmployee";
            this.lblALAppEmployee.Size = new System.Drawing.Size(82, 15);
            this.lblALAppEmployee.TabIndex = 1;
            this.lblALAppEmployee.Text = "Employee:";
            this.lblALAppEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbALAppEmployee
            // 
            this.cbALAppEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbALAppEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbALAppEmployee.FormattingEnabled = true;
            this.cbALAppEmployee.Location = new System.Drawing.Point(438, 50);
            this.cbALAppEmployee.Name = "cbALAppEmployee";
            this.cbALAppEmployee.Size = new System.Drawing.Size(221, 21);
            this.cbALAppEmployee.TabIndex = 2;
            // 
            // gbALAppUnitFilter
            // 
            this.gbALAppUnitFilter.Controls.Add(this.btnALAppOUTree);
            this.gbALAppUnitFilter.Controls.Add(this.cbALAppOU);
            this.gbALAppUnitFilter.Controls.Add(this.btnALAppWUTree);
            this.gbALAppUnitFilter.Controls.Add(this.cbALAppWU);
            this.gbALAppUnitFilter.Controls.Add(this.rbALAppOU);
            this.gbALAppUnitFilter.Controls.Add(this.rbALAppFS);
            this.gbALAppUnitFilter.Location = new System.Drawing.Point(10, 5);
            this.gbALAppUnitFilter.Name = "gbALAppUnitFilter";
            this.gbALAppUnitFilter.Size = new System.Drawing.Size(334, 82);
            this.gbALAppUnitFilter.TabIndex = 0;
            this.gbALAppUnitFilter.TabStop = false;
            this.gbALAppUnitFilter.Text = "Unit filter";
            // 
            // btnALAppOUTree
            // 
            this.btnALAppOUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnALAppOUTree.Image")));
            this.btnALAppOUTree.Location = new System.Drawing.Point(296, 43);
            this.btnALAppOUTree.Name = "btnALAppOUTree";
            this.btnALAppOUTree.Size = new System.Drawing.Size(25, 23);
            this.btnALAppOUTree.TabIndex = 5;
            this.btnALAppOUTree.Click += new System.EventHandler(this.btnALAppOUTree_Click);
            // 
            // cbALAppOU
            // 
            this.cbALAppOU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbALAppOU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbALAppOU.FormattingEnabled = true;
            this.cbALAppOU.Location = new System.Drawing.Point(69, 45);
            this.cbALAppOU.Name = "cbALAppOU";
            this.cbALAppOU.Size = new System.Drawing.Size(221, 21);
            this.cbALAppOU.TabIndex = 4;
            this.cbALAppOU.SelectedIndexChanged += new System.EventHandler(this.cbALAppOU_SelectedIndexChanged);
            // 
            // btnALAppWUTree
            // 
            this.btnALAppWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnALAppWUTree.Image")));
            this.btnALAppWUTree.Location = new System.Drawing.Point(296, 16);
            this.btnALAppWUTree.Name = "btnALAppWUTree";
            this.btnALAppWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnALAppWUTree.TabIndex = 2;
            this.btnALAppWUTree.Click += new System.EventHandler(this.btnALAppWUTree_Click);
            // 
            // cbALAppWU
            // 
            this.cbALAppWU.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbALAppWU.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbALAppWU.FormattingEnabled = true;
            this.cbALAppWU.Location = new System.Drawing.Point(69, 18);
            this.cbALAppWU.Name = "cbALAppWU";
            this.cbALAppWU.Size = new System.Drawing.Size(221, 21);
            this.cbALAppWU.TabIndex = 1;
            this.cbALAppWU.SelectedIndexChanged += new System.EventHandler(this.cbALAppWU_SelectedIndexChanged);
            // 
            // rbALAppOU
            // 
            this.rbALAppOU.AutoSize = true;
            this.rbALAppOU.Location = new System.Drawing.Point(20, 46);
            this.rbALAppOU.Name = "rbALAppOU";
            this.rbALAppOU.Size = new System.Drawing.Size(41, 17);
            this.rbALAppOU.TabIndex = 3;
            this.rbALAppOU.TabStop = true;
            this.rbALAppOU.Text = "OU";
            this.rbALAppOU.UseVisualStyleBackColor = true;
            this.rbALAppOU.CheckedChanged += new System.EventHandler(this.rbALAppOU_CheckedChanged);
            // 
            // rbALAppFS
            // 
            this.rbALAppFS.AutoSize = true;
            this.rbALAppFS.Location = new System.Drawing.Point(20, 19);
            this.rbALAppFS.Name = "rbALAppFS";
            this.rbALAppFS.Size = new System.Drawing.Size(38, 17);
            this.rbALAppFS.TabIndex = 0;
            this.rbALAppFS.TabStop = true;
            this.rbALAppFS.Text = "FS";
            this.rbALAppFS.UseVisualStyleBackColor = true;
            this.rbALAppFS.CheckedChanged += new System.EventHandler(this.rbALAppFS_CheckedChanged);
            // 
            // lvALRecalc
            // 
            this.lvALRecalc.FullRowSelect = true;
            this.lvALRecalc.GridLines = true;
            this.lvALRecalc.HideSelection = false;
            this.lvALRecalc.Location = new System.Drawing.Point(10, 95);
            this.lvALRecalc.Name = "lvALRecalc";
            this.lvALRecalc.ShowItemToolTips = true;
            this.lvALRecalc.Size = new System.Drawing.Size(890, 352);
            this.lvALRecalc.TabIndex = 4;
            this.lvALRecalc.UseCompatibleStateImageBehavior = false;
            this.lvALRecalc.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(853, 578);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(12, 49);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 13);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(12, 23);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 13);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(58, 45);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(58, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblResult
            // 
            this.lblResult.Location = new System.Drawing.Point(182, 21);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(62, 13);
            this.lblResult.TabIndex = 4;
            this.lblResult.Text = "Result:";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbResult
            // 
            this.cbResult.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbResult.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResult.FormattingEnabled = true;
            this.cbResult.Location = new System.Drawing.Point(250, 18);
            this.cbResult.Name = "cbResult";
            this.cbResult.Size = new System.Drawing.Size(124, 21);
            this.cbResult.TabIndex = 5;
            // 
            // gbCriteria
            // 
            this.gbCriteria.Controls.Add(this.btnSearch);
            this.gbCriteria.Controls.Add(this.dtpFrom);
            this.gbCriteria.Controls.Add(this.lblResult);
            this.gbCriteria.Controls.Add(this.dtpTo);
            this.gbCriteria.Controls.Add(this.cbResult);
            this.gbCriteria.Controls.Add(this.lblFrom);
            this.gbCriteria.Controls.Add(this.lblTo);
            this.gbCriteria.Location = new System.Drawing.Point(12, 532);
            this.gbCriteria.Name = "gbCriteria";
            this.gbCriteria.Size = new System.Drawing.Size(392, 77);
            this.gbCriteria.TabIndex = 1;
            this.gbCriteria.TabStop = false;
            this.gbCriteria.Text = "Criteria";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(250, 44);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(124, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // SynchronizationPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 631);
            this.ControlBox = false;
            this.Controls.Add(this.gbCriteria);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SynchronizationPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Synchronization Preview";
            this.Load += new System.EventHandler(this.SynchronizationPreview_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SynchronizationPreview_FormClosed);
            this.tabControl.ResumeLayout(false);
            this.tabFS.ResumeLayout(false);
            this.tabOU.ResumeLayout(false);
            this.tabCC.ResumeLayout(false);
            this.tabPositions.ResumeLayout(false);
            this.tabEmployees.ResumeLayout(false);
            this.gbEmplUnitFilter.ResumeLayout(false);
            this.gbEmplUnitFilter.PerformLayout();
            this.tabResponsibility.ResumeLayout(false);
            this.gbResUnitFilter.ResumeLayout(false);
            this.gbResUnitFilter.PerformLayout();
            this.tabALRecalc.ResumeLayout(false);
            this.gbALUnitFilter.ResumeLayout(false);
            this.gbALUnitFilter.PerformLayout();
            this.tabALApproval.ResumeLayout(false);
            this.gbALAppUnitFilter.ResumeLayout(false);
            this.gbALAppUnitFilter.PerformLayout();
            this.gbCriteria.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabFS;
        private System.Windows.Forms.TabPage tabOU;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tabCC;
        private System.Windows.Forms.TabPage tabPositions;
        private System.Windows.Forms.TabPage tabEmployees;
        private System.Windows.Forms.TabPage tabResponsibility;
        private System.Windows.Forms.TabPage tabALRecalc;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.ComboBox cbResult;
        private System.Windows.Forms.GroupBox gbCriteria;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.ListView lvWU;
        private System.Windows.Forms.ListView lvOU;
        private System.Windows.Forms.Label lblOU;
        private System.Windows.Forms.Button btnOUTree;
        private System.Windows.Forms.ComboBox cbOU;
        private System.Windows.Forms.ListView lvCC;
        private System.Windows.Forms.Label lblCCCode;
        private System.Windows.Forms.ComboBox cbCCCode;
        private System.Windows.Forms.ListView lvPosition;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.ComboBox cbPosition;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.Label lblEmplEmployee;
        private System.Windows.Forms.ComboBox cbEmplEmployee;
        private System.Windows.Forms.GroupBox gbEmplUnitFilter;
        private System.Windows.Forms.Button btnEmplOUTree;
        private System.Windows.Forms.ComboBox cbEmplOU;
        private System.Windows.Forms.Button btnEmplWUTree;
        private System.Windows.Forms.ComboBox cbEmplWU;
        private System.Windows.Forms.RadioButton rbEmplOU;
        private System.Windows.Forms.RadioButton rbEmplFS;
        private System.Windows.Forms.Label lblResEmployee;
        private System.Windows.Forms.ComboBox cbResEmployee;
        private System.Windows.Forms.GroupBox gbResUnitFilter;
        private System.Windows.Forms.Button btnResOUTree;
        private System.Windows.Forms.ComboBox cbResOU;
        private System.Windows.Forms.Button btnResWUTree;
        private System.Windows.Forms.ComboBox cbResWU;
        private System.Windows.Forms.RadioButton rbResOU;
        private System.Windows.Forms.RadioButton rbResFS;
        private System.Windows.Forms.ListView lvResponsibilities;
        private System.Windows.Forms.Label lblALEmployee;
        private System.Windows.Forms.ComboBox cbALEmployee;
        private System.Windows.Forms.GroupBox gbALUnitFilter;
        private System.Windows.Forms.Button btnALOUTree;
        private System.Windows.Forms.ComboBox cbALOU;
        private System.Windows.Forms.Button btnALWUTree;
        private System.Windows.Forms.ComboBox cbALWU;
        private System.Windows.Forms.RadioButton rbALOU;
        private System.Windows.Forms.RadioButton rbALFS;
        private System.Windows.Forms.ListView lvALRecalculation;
        private System.Windows.Forms.TabPage tabALApproval;
        private System.Windows.Forms.Label lblALAppEmployee;
        private System.Windows.Forms.ComboBox cbALAppEmployee;
        private System.Windows.Forms.GroupBox gbALAppUnitFilter;
        private System.Windows.Forms.Button btnALAppOUTree;
        private System.Windows.Forms.ComboBox cbALAppOU;
        private System.Windows.Forms.Button btnALAppWUTree;
        private System.Windows.Forms.ComboBox cbALAppWU;
        private System.Windows.Forms.RadioButton rbALAppOU;
        private System.Windows.Forms.RadioButton rbALAppFS;
        private System.Windows.Forms.ListView lvALRecalc;
        private System.Windows.Forms.Button btnSearchALRecalc;
        private System.Windows.Forms.Button btnNotApprove;
        private System.Windows.Forms.Button btnApprove;
    }
}