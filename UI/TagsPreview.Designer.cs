namespace UI
{
    partial class TagsPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagsPreview));
            this.tcPreview = new System.Windows.Forms.TabControl();
            this.tbTagSearch = new System.Windows.Forms.TabPage();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lvTags = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.cbEmpl = new System.Windows.Forms.ComboBox();
            this.lblEmpl = new System.Windows.Forms.Label();
            this.cbIncludeTimeSearch = new System.Windows.Forms.CheckBox();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnReadTagID = new System.Windows.Forms.Button();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.gbInvalid = new System.Windows.Forms.GroupBox();
            this.cbReturned = new System.Windows.Forms.CheckBox();
            this.cbDamaged = new System.Windows.Forms.CheckBox();
            this.cbLost = new System.Windows.Forms.CheckBox();
            this.gbValid = new System.Windows.Forms.GroupBox();
            this.cbBlocked = new System.Windows.Forms.CheckBox();
            this.cbActive = new System.Windows.Forms.CheckBox();
            this.lblTagID = new System.Windows.Forms.Label();
            this.tbTagID = new System.Windows.Forms.TextBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitID = new System.Windows.Forms.Label();
            this.tbSumary = new System.Windows.Forms.TabPage();
            this.lblTotalSum = new System.Windows.Forms.Label();
            this.btnNextSum = new System.Windows.Forms.Button();
            this.btnPrevSum = new System.Windows.Forms.Button();
            this.lvSum = new System.Windows.Forms.ListView();
            this.gbSearch1 = new System.Windows.Forms.GroupBox();
            this.btnSearchSum = new System.Windows.Forms.Button();
            this.gbNumOFTags = new System.Windows.Forms.GroupBox();
            this.tbLessThan = new System.Windows.Forms.NumericUpDown();
            this.tbMoreThan = new System.Windows.Forms.NumericUpDown();
            this.chbLessThan = new System.Windows.Forms.CheckBox();
            this.cbMoreThan = new System.Windows.Forms.CheckBox();
            this.chbHierarhicly1 = new System.Windows.Forms.CheckBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.btnWUTree1 = new System.Windows.Forms.Button();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.tcPreview.SuspendLayout();
            this.tbTagSearch.SuspendLayout();
            this.gbSearch.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.gbInvalid.SuspendLayout();
            this.gbValid.SuspendLayout();
            this.tbSumary.SuspendLayout();
            this.gbSearch1.SuspendLayout();
            this.gbNumOFTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLessThan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMoreThan)).BeginInit();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcPreview
            // 
            this.tcPreview.Controls.Add(this.tbTagSearch);
            this.tcPreview.Controls.Add(this.tbSumary);
            this.tcPreview.Location = new System.Drawing.Point(12, 26);
            this.tcPreview.Name = "tcPreview";
            this.tcPreview.SelectedIndex = 0;
            this.tcPreview.Size = new System.Drawing.Size(859, 528);
            this.tcPreview.TabIndex = 0;
            this.tcPreview.SelectedIndexChanged += new System.EventHandler(this.tcPreview_SelectedIndexChanged);
            // 
            // tbTagSearch
            // 
            this.tbTagSearch.Controls.Add(this.btnNext);
            this.tbTagSearch.Controls.Add(this.btnPrev);
            this.tbTagSearch.Controls.Add(this.lblTotal);
            this.tbTagSearch.Controls.Add(this.lvTags);
            this.tbTagSearch.Controls.Add(this.gbSearch);
            this.tbTagSearch.Location = new System.Drawing.Point(4, 22);
            this.tbTagSearch.Name = "tbTagSearch";
            this.tbTagSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tbTagSearch.Size = new System.Drawing.Size(851, 502);
            this.tbTagSearch.TabIndex = 0;
            this.tbTagSearch.Text = "Tag search";
            this.tbTagSearch.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(813, 204);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 39;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(773, 204);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 38;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(693, 477);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvTags
            // 
            this.lvTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTags.FullRowSelect = true;
            this.lvTags.GridLines = true;
            this.lvTags.HideSelection = false;
            this.lvTags.Location = new System.Drawing.Point(6, 233);
            this.lvTags.Name = "lvTags";
            this.lvTags.Size = new System.Drawing.Size(839, 241);
            this.lvTags.TabIndex = 19;
            this.lvTags.UseCompatibleStateImageBehavior = false;
            this.lvTags.View = System.Windows.Forms.View.Details;
            this.lvTags.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTags_ColumnClick);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.cbEmpl);
            this.gbSearch.Controls.Add(this.lblEmpl);
            this.gbSearch.Controls.Add(this.cbIncludeTimeSearch);
            this.gbSearch.Controls.Add(this.gbTimeInterval);
            this.gbSearch.Controls.Add(this.btnReadTagID);
            this.gbSearch.Controls.Add(this.gbStatus);
            this.gbSearch.Controls.Add(this.lblTagID);
            this.gbSearch.Controls.Add(this.tbTagID);
            this.gbSearch.Controls.Add(this.chbHierarhicly);
            this.gbSearch.Controls.Add(this.btnWUTree);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.cbWU);
            this.gbSearch.Controls.Add(this.lblWorkingUnitID);
            this.gbSearch.Location = new System.Drawing.Point(6, 6);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(839, 196);
            this.gbSearch.TabIndex = 1;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Search";
            this.gbSearch.Enter += new System.EventHandler(this.gbSearch_Enter);
            // 
            // cbEmpl
            // 
            this.cbEmpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmpl.FormattingEnabled = true;
            this.cbEmpl.Location = new System.Drawing.Point(480, 46);
            this.cbEmpl.Name = "cbEmpl";
            this.cbEmpl.Size = new System.Drawing.Size(205, 21);
            this.cbEmpl.TabIndex = 41;
            // 
            // lblEmpl
            // 
            this.lblEmpl.Location = new System.Drawing.Point(393, 47);
            this.lblEmpl.Name = "lblEmpl";
            this.lblEmpl.Size = new System.Drawing.Size(81, 17);
            this.lblEmpl.TabIndex = 40;
            this.lblEmpl.Text = "Employee:";
            this.lblEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbIncludeTimeSearch
            // 
            this.cbIncludeTimeSearch.AutoSize = true;
            this.cbIncludeTimeSearch.Location = new System.Drawing.Point(459, 85);
            this.cbIncludeTimeSearch.Name = "cbIncludeTimeSearch";
            this.cbIncludeTimeSearch.Size = new System.Drawing.Size(15, 14);
            this.cbIncludeTimeSearch.TabIndex = 39;
            this.cbIncludeTimeSearch.UseVisualStyleBackColor = true;
            this.cbIncludeTimeSearch.CheckedChanged += new System.EventHandler(this.cbIncludeTimeSearch_CheckedChanged);
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpTo);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFrom);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Enabled = false;
            this.gbTimeInterval.Location = new System.Drawing.Point(480, 80);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(228, 78);
            this.gbTimeInterval.TabIndex = 38;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "TimeInterval";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(55, 47);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(150, 20);
            this.dtpTo.TabIndex = 17;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(9, 44);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 23);
            this.lblTo.TabIndex = 16;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpFrom.Location = new System.Drawing.Point(55, 21);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(150, 20);
            this.dtpFrom.TabIndex = 15;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 20);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(43, 23);
            this.lblFrom.TabIndex = 14;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReadTagID
            // 
            this.btnReadTagID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadTagID.Image = ((System.Drawing.Image)(resources.GetObject("btnReadTagID.Image")));
            this.btnReadTagID.Location = new System.Drawing.Point(310, 21);
            this.btnReadTagID.Name = "btnReadTagID";
            this.btnReadTagID.Size = new System.Drawing.Size(42, 23);
            this.btnReadTagID.TabIndex = 37;
            this.btnReadTagID.UseVisualStyleBackColor = true;
            this.btnReadTagID.Click += new System.EventHandler(this.btnReadTagID_Click);
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.gbInvalid);
            this.gbStatus.Controls.Add(this.gbValid);
            this.gbStatus.Location = new System.Drawing.Point(14, 54);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(338, 133);
            this.gbStatus.TabIndex = 18;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Status";
            // 
            // gbInvalid
            // 
            this.gbInvalid.Controls.Add(this.cbReturned);
            this.gbInvalid.Controls.Add(this.cbDamaged);
            this.gbInvalid.Controls.Add(this.cbLost);
            this.gbInvalid.Location = new System.Drawing.Point(175, 31);
            this.gbInvalid.Name = "gbInvalid";
            this.gbInvalid.Size = new System.Drawing.Size(143, 86);
            this.gbInvalid.TabIndex = 20;
            this.gbInvalid.TabStop = false;
            this.gbInvalid.Text = "Invalid";
            // 
            // cbReturned
            // 
            this.cbReturned.AutoSize = true;
            this.cbReturned.Checked = true;
            this.cbReturned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReturned.Location = new System.Drawing.Point(7, 63);
            this.cbReturned.Name = "cbReturned";
            this.cbReturned.Size = new System.Drawing.Size(87, 17);
            this.cbReturned.TabIndex = 2;
            this.cbReturned.Text = "RETURNED";
            this.cbReturned.UseVisualStyleBackColor = true;
            // 
            // cbDamaged
            // 
            this.cbDamaged.AutoSize = true;
            this.cbDamaged.Checked = true;
            this.cbDamaged.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDamaged.Location = new System.Drawing.Point(7, 42);
            this.cbDamaged.Name = "cbDamaged";
            this.cbDamaged.Size = new System.Drawing.Size(80, 17);
            this.cbDamaged.TabIndex = 1;
            this.cbDamaged.Text = "DAMAGED";
            this.cbDamaged.UseVisualStyleBackColor = true;
            // 
            // cbLost
            // 
            this.cbLost.AutoSize = true;
            this.cbLost.Checked = true;
            this.cbLost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLost.Location = new System.Drawing.Point(7, 19);
            this.cbLost.Name = "cbLost";
            this.cbLost.Size = new System.Drawing.Size(54, 17);
            this.cbLost.TabIndex = 0;
            this.cbLost.Text = "LOST";
            this.cbLost.UseVisualStyleBackColor = true;
            // 
            // gbValid
            // 
            this.gbValid.Controls.Add(this.cbBlocked);
            this.gbValid.Controls.Add(this.cbActive);
            this.gbValid.Location = new System.Drawing.Point(16, 31);
            this.gbValid.Name = "gbValid";
            this.gbValid.Size = new System.Drawing.Size(143, 86);
            this.gbValid.TabIndex = 19;
            this.gbValid.TabStop = false;
            this.gbValid.Text = "Valid";
            // 
            // cbBlocked
            // 
            this.cbBlocked.AutoSize = true;
            this.cbBlocked.Checked = true;
            this.cbBlocked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBlocked.Location = new System.Drawing.Point(7, 52);
            this.cbBlocked.Name = "cbBlocked";
            this.cbBlocked.Size = new System.Drawing.Size(76, 17);
            this.cbBlocked.TabIndex = 1;
            this.cbBlocked.Text = "BLOCKED";
            this.cbBlocked.UseVisualStyleBackColor = true;
            // 
            // cbActive
            // 
            this.cbActive.AutoSize = true;
            this.cbActive.Checked = true;
            this.cbActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActive.Location = new System.Drawing.Point(7, 23);
            this.cbActive.Name = "cbActive";
            this.cbActive.Size = new System.Drawing.Size(64, 17);
            this.cbActive.TabIndex = 0;
            this.cbActive.Text = "ACTIVE";
            this.cbActive.UseVisualStyleBackColor = true;
            // 
            // lblTagID
            // 
            this.lblTagID.Location = new System.Drawing.Point(11, 20);
            this.lblTagID.Name = "lblTagID";
            this.lblTagID.Size = new System.Drawing.Size(82, 23);
            this.lblTagID.TabIndex = 16;
            this.lblTagID.Text = "Tag ID:";
            this.lblTagID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbTagID
            // 
            this.tbTagID.Location = new System.Drawing.Point(99, 23);
            this.tbTagID.Name = "tbTagID";
            this.tbTagID.Size = new System.Drawing.Size(205, 20);
            this.tbTagID.TabIndex = 17;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(724, 19);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(101, 24);
            this.chbHierarhicly.TabIndex = 9;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(693, 19);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 8;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(620, 164);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(737, 164);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbWU
            // 
            this.cbWU.AccessibleDescription = "FILTERABLE";
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(480, 19);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(205, 21);
            this.cbWU.TabIndex = 7;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnitID_SelectedIndexChanged);
            // 
            // lblWorkingUnitID
            // 
            this.lblWorkingUnitID.Location = new System.Drawing.Point(370, 19);
            this.lblWorkingUnitID.Name = "lblWorkingUnitID";
            this.lblWorkingUnitID.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnitID.TabIndex = 6;
            this.lblWorkingUnitID.Text = "Working Unit ID:";
            this.lblWorkingUnitID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSumary
            // 
            this.tbSumary.Controls.Add(this.lblTotalSum);
            this.tbSumary.Controls.Add(this.btnNextSum);
            this.tbSumary.Controls.Add(this.btnPrevSum);
            this.tbSumary.Controls.Add(this.lvSum);
            this.tbSumary.Controls.Add(this.gbSearch1);
            this.tbSumary.Location = new System.Drawing.Point(4, 22);
            this.tbSumary.Name = "tbSumary";
            this.tbSumary.Padding = new System.Windows.Forms.Padding(3);
            this.tbSumary.Size = new System.Drawing.Size(851, 502);
            this.tbSumary.TabIndex = 1;
            this.tbSumary.Text = "Sumary";
            this.tbSumary.UseVisualStyleBackColor = true;
            // 
            // lblTotalSum
            // 
            this.lblTotalSum.Location = new System.Drawing.Point(669, 454);
            this.lblTotalSum.Name = "lblTotalSum";
            this.lblTotalSum.Size = new System.Drawing.Size(152, 16);
            this.lblTotalSum.TabIndex = 24;
            this.lblTotalSum.Text = "Total:";
            this.lblTotalSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTotalSum.Visible = false;
            // 
            // btnNextSum
            // 
            this.btnNextSum.Location = new System.Drawing.Point(789, 171);
            this.btnNextSum.Name = "btnNextSum";
            this.btnNextSum.Size = new System.Drawing.Size(32, 23);
            this.btnNextSum.TabIndex = 23;
            this.btnNextSum.Text = ">";
            // 
            // btnPrevSum
            // 
            this.btnPrevSum.Location = new System.Drawing.Point(749, 171);
            this.btnPrevSum.Name = "btnPrevSum";
            this.btnPrevSum.Size = new System.Drawing.Size(32, 23);
            this.btnPrevSum.TabIndex = 22;
            this.btnPrevSum.Text = "<";
            // 
            // lvSum
            // 
            this.lvSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSum.FullRowSelect = true;
            this.lvSum.GridLines = true;
            this.lvSum.HideSelection = false;
            this.lvSum.Location = new System.Drawing.Point(25, 210);
            this.lvSum.MultiSelect = false;
            this.lvSum.Name = "lvSum";
            this.lvSum.Size = new System.Drawing.Size(796, 241);
            this.lvSum.TabIndex = 21;
            this.lvSum.UseCompatibleStateImageBehavior = false;
            this.lvSum.View = System.Windows.Forms.View.Details;
            this.lvSum.DoubleClick += new System.EventHandler(this.lvSum_DoubleClick);
            this.lvSum.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSum_ColumnClick);
            // 
            // gbSearch1
            // 
            this.gbSearch1.Controls.Add(this.btnSearchSum);
            this.gbSearch1.Controls.Add(this.gbNumOFTags);
            this.gbSearch1.Controls.Add(this.chbHierarhicly1);
            this.gbSearch1.Controls.Add(this.lblWorkingUnit);
            this.gbSearch1.Controls.Add(this.btnWUTree1);
            this.gbSearch1.Controls.Add(this.cbWorkingUnit);
            this.gbSearch1.Location = new System.Drawing.Point(25, 19);
            this.gbSearch1.Name = "gbSearch1";
            this.gbSearch1.Size = new System.Drawing.Size(668, 175);
            this.gbSearch1.TabIndex = 20;
            this.gbSearch1.TabStop = false;
            this.gbSearch1.Text = "Search";
            // 
            // btnSearchSum
            // 
            this.btnSearchSum.Location = new System.Drawing.Point(559, 136);
            this.btnSearchSum.Name = "btnSearchSum";
            this.btnSearchSum.Size = new System.Drawing.Size(88, 23);
            this.btnSearchSum.TabIndex = 16;
            this.btnSearchSum.Text = "Search";
            this.btnSearchSum.Click += new System.EventHandler(this.btnSearchSum_Click);
            // 
            // gbNumOFTags
            // 
            this.gbNumOFTags.Controls.Add(this.tbLessThan);
            this.gbNumOFTags.Controls.Add(this.tbMoreThan);
            this.gbNumOFTags.Controls.Add(this.chbLessThan);
            this.gbNumOFTags.Controls.Add(this.cbMoreThan);
            this.gbNumOFTags.Location = new System.Drawing.Point(44, 59);
            this.gbNumOFTags.Name = "gbNumOFTags";
            this.gbNumOFTags.Size = new System.Drawing.Size(200, 100);
            this.gbNumOFTags.TabIndex = 10;
            this.gbNumOFTags.TabStop = false;
            this.gbNumOFTags.Text = "Number";
            // 
            // tbLessThan
            // 
            this.tbLessThan.Enabled = false;
            this.tbLessThan.Location = new System.Drawing.Point(111, 59);
            this.tbLessThan.Name = "tbLessThan";
            this.tbLessThan.Size = new System.Drawing.Size(72, 20);
            this.tbLessThan.TabIndex = 4;
            // 
            // tbMoreThan
            // 
            this.tbMoreThan.Enabled = false;
            this.tbMoreThan.Location = new System.Drawing.Point(111, 30);
            this.tbMoreThan.Name = "tbMoreThan";
            this.tbMoreThan.Size = new System.Drawing.Size(72, 20);
            this.tbMoreThan.TabIndex = 3;
            // 
            // chbLessThan
            // 
            this.chbLessThan.AutoSize = true;
            this.chbLessThan.Location = new System.Drawing.Point(27, 62);
            this.chbLessThan.Name = "chbLessThan";
            this.chbLessThan.Size = new System.Drawing.Size(75, 17);
            this.chbLessThan.TabIndex = 2;
            this.chbLessThan.Text = "Less than:";
            this.chbLessThan.UseVisualStyleBackColor = true;
            this.chbLessThan.CheckedChanged += new System.EventHandler(this.chbLessThan_CheckedChanged);
            // 
            // cbMoreThan
            // 
            this.cbMoreThan.AutoSize = true;
            this.cbMoreThan.Location = new System.Drawing.Point(27, 30);
            this.cbMoreThan.Name = "cbMoreThan";
            this.cbMoreThan.Size = new System.Drawing.Size(77, 17);
            this.cbMoreThan.TabIndex = 0;
            this.cbMoreThan.Text = "More than:";
            this.cbMoreThan.UseVisualStyleBackColor = true;
            this.cbMoreThan.CheckedChanged += new System.EventHandler(this.cbMoreThan_CheckedChanged);
            // 
            // chbHierarhicly1
            // 
            this.chbHierarhicly1.Location = new System.Drawing.Point(406, 19);
            this.chbHierarhicly1.Name = "chbHierarhicly1";
            this.chbHierarhicly1.Size = new System.Drawing.Size(89, 24);
            this.chbHierarhicly1.TabIndex = 9;
            this.chbHierarhicly1.Text = "Hierarchy ";
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(11, 21);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 7;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnWUTree1
            // 
            this.btnWUTree1.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree1.Image")));
            this.btnWUTree1.Location = new System.Drawing.Point(375, 19);
            this.btnWUTree1.Name = "btnWUTree1";
            this.btnWUTree1.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree1.TabIndex = 8;
            this.btnWUTree1.Click += new System.EventHandler(this.btnWUTree1_Click);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(123, 21);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(246, 21);
            this.cbWorkingUnit.TabIndex = 6;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(796, 560);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(594, -1);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(272, 45);
            this.gbFilter.TabIndex = 27;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(175, 14);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 16);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // TagsPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 590);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tcPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "TagsPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TagsPreview";
            this.Load += new System.EventHandler(this.TagsPreview_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TagsPreview_KeyUp);
            this.tcPreview.ResumeLayout(false);
            this.tbTagSearch.ResumeLayout(false);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.gbTimeInterval.ResumeLayout(false);
            this.gbStatus.ResumeLayout(false);
            this.gbInvalid.ResumeLayout(false);
            this.gbInvalid.PerformLayout();
            this.gbValid.ResumeLayout(false);
            this.gbValid.PerformLayout();
            this.tbSumary.ResumeLayout(false);
            this.gbSearch1.ResumeLayout(false);
            this.gbNumOFTags.ResumeLayout(false);
            this.gbNumOFTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLessThan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMoreThan)).EndInit();
            this.gbFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcPreview;
        private System.Windows.Forms.TabPage tbTagSearch;
        private System.Windows.Forms.TabPage tbSumary;
        private System.Windows.Forms.ListView lvTags;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.CheckBox cbIncludeTimeSearch;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Button btnReadTagID;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.GroupBox gbInvalid;
        private System.Windows.Forms.CheckBox cbReturned;
        private System.Windows.Forms.CheckBox cbDamaged;
        private System.Windows.Forms.CheckBox cbLost;
        private System.Windows.Forms.GroupBox gbValid;
        private System.Windows.Forms.CheckBox cbBlocked;
        private System.Windows.Forms.CheckBox cbActive;
        private System.Windows.Forms.Label lblTagID;
        private System.Windows.Forms.TextBox tbTagID;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWorkingUnitID;
        private System.Windows.Forms.ComboBox cbEmpl;
        private System.Windows.Forms.Label lblEmpl;
        private System.Windows.Forms.GroupBox gbSearch1;
        private System.Windows.Forms.CheckBox chbHierarhicly1;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.Button btnWUTree1;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.GroupBox gbNumOFTags;
        private System.Windows.Forms.CheckBox chbLessThan;
        private System.Windows.Forms.CheckBox cbMoreThan;
        private System.Windows.Forms.ListView lvSum;
        private System.Windows.Forms.Button btnSearchSum;
        private System.Windows.Forms.NumericUpDown tbLessThan;
        private System.Windows.Forms.NumericUpDown tbMoreThan;
        private System.Windows.Forms.Button btnNextSum;
        private System.Windows.Forms.Button btnPrevSum;
        private System.Windows.Forms.Label lblTotalSum;
    }
}