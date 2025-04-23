namespace UI
{
    partial class MealEmployeeSchedulePreview
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MealEmployeeSchedulePreview));
            this.gbSearchCriteria = new System.Windows.Forms.GroupBox();
            this.numQtyTo = new System.Windows.Forms.NumericUpDown();
            this.numQtyFrom = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cbMealType = new System.Windows.Forms.ComboBox();
            this.lblMealType = new System.Windows.Forms.Label();
            this.cbPlace = new System.Windows.Forms.ComboBox();
            this.lblPlace = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.gbSearchResults = new System.Windows.Forms.GroupBox();
            this.lvUsedOrdered = new System.Windows.Forms.ListView();
            this.lvOrders = new System.Windows.Forms.ListView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lvEmployeeMeals = new System.Windows.Forms.ListView();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.lblSumary = new System.Windows.Forms.Label();
            this.rbOrderExists = new System.Windows.Forms.RadioButton();
            this.rbOrderDoesntExist = new System.Windows.Forms.RadioButton();
            this.rbOrders = new System.Windows.Forms.RadioButton();
            this.rbUsedMeals = new System.Windows.Forms.RadioButton();
            this.gbWithOrWithout = new System.Windows.Forms.GroupBox();
            this.gbTypeReportChoice = new System.Windows.Forms.GroupBox();
            this.rbUsedOrdered = new System.Windows.Forms.RadioButton();
            this.gbSearchCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).BeginInit();
            this.gbSearchResults.SuspendLayout();
            this.gbReport.SuspendLayout();
            this.gbWithOrWithout.SuspendLayout();
            this.gbTypeReportChoice.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearchCriteria
            // 
            this.gbSearchCriteria.Controls.Add(this.numQtyTo);
            this.gbSearchCriteria.Controls.Add(this.numQtyFrom);
            this.gbSearchCriteria.Controls.Add(this.label1);
            this.gbSearchCriteria.Controls.Add(this.lblQty);
            this.gbSearchCriteria.Controls.Add(this.btnSearch);
            this.gbSearchCriteria.Controls.Add(this.btnClear);
            this.gbSearchCriteria.Controls.Add(this.cbMealType);
            this.gbSearchCriteria.Controls.Add(this.lblMealType);
            this.gbSearchCriteria.Controls.Add(this.cbPlace);
            this.gbSearchCriteria.Controls.Add(this.lblPlace);
            this.gbSearchCriteria.Controls.Add(this.dtpTo);
            this.gbSearchCriteria.Controls.Add(this.lblTo);
            this.gbSearchCriteria.Controls.Add(this.dtpFrom);
            this.gbSearchCriteria.Controls.Add(this.lblFrom);
            this.gbSearchCriteria.Controls.Add(this.cbEmployee);
            this.gbSearchCriteria.Controls.Add(this.lblEmployee);
            this.gbSearchCriteria.Controls.Add(this.chbHierarhicly);
            this.gbSearchCriteria.Controls.Add(this.btnWUTree);
            this.gbSearchCriteria.Controls.Add(this.cbWU);
            this.gbSearchCriteria.Controls.Add(this.lblWorkingUnit);
            this.gbSearchCriteria.Location = new System.Drawing.Point(15, 85);
            this.gbSearchCriteria.Name = "gbSearchCriteria";
            this.gbSearchCriteria.Size = new System.Drawing.Size(684, 159);
            this.gbSearchCriteria.TabIndex = 0;
            this.gbSearchCriteria.TabStop = false;
            this.gbSearchCriteria.Text = "Search criteria";
            this.gbSearchCriteria.Enter += new System.EventHandler(this.gbSearchCriteria_Enter);
            // 
            // numQtyTo
            // 
            this.numQtyTo.Location = new System.Drawing.Point(601, 74);
            this.numQtyTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyTo.Name = "numQtyTo";
            this.numQtyTo.Size = new System.Drawing.Size(65, 20);
            this.numQtyTo.TabIndex = 35;
            // 
            // numQtyFrom
            // 
            this.numQtyFrom.Location = new System.Drawing.Point(499, 74);
            this.numQtyFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyFrom.Name = "numQtyFrom";
            this.numQtyFrom.Size = new System.Drawing.Size(65, 20);
            this.numQtyFrom.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(579, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "-";
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(427, 76);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(66, 13);
            this.lblQty.TabIndex = 32;
            this.lblQty.Text = "Qyantity:";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(577, 131);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 31;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(466, 131);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 30;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cbMealType
            // 
            this.cbMealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMealType.Location = new System.Drawing.Point(499, 49);
            this.cbMealType.Name = "cbMealType";
            this.cbMealType.Size = new System.Drawing.Size(167, 21);
            this.cbMealType.TabIndex = 29;
            // 
            // lblMealType
            // 
            this.lblMealType.Location = new System.Drawing.Point(417, 47);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(76, 23);
            this.lblMealType.TabIndex = 28;
            this.lblMealType.Text = "Meal type:";
            this.lblMealType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPlace
            // 
            this.cbPlace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlace.Location = new System.Drawing.Point(499, 18);
            this.cbPlace.Name = "cbPlace";
            this.cbPlace.Size = new System.Drawing.Size(167, 21);
            this.cbPlace.TabIndex = 27;
            // 
            // lblPlace
            // 
            this.lblPlace.Location = new System.Drawing.Point(414, 16);
            this.lblPlace.Name = "lblPlace";
            this.lblPlace.Size = new System.Drawing.Size(79, 23);
            this.lblPlace.TabIndex = 26;
            this.lblPlace.Text = "Place:";
            this.lblPlace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(121, 134);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(160, 20);
            this.dtpTo.TabIndex = 25;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(43, 131);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(72, 23);
            this.lblTo.TabIndex = 24;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpFrom.Location = new System.Drawing.Point(121, 108);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(160, 20);
            this.dtpFrom.TabIndex = 23;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(27, 107);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(88, 23);
            this.lblFrom.TabIndex = 22;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(121, 76);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(246, 21);
            this.cbEmployee.TabIndex = 21;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(9, 74);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(104, 23);
            this.lblEmployee.TabIndex = 20;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(121, 46);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(89, 24);
            this.chbHierarhicly.TabIndex = 19;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(373, 17);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 18;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(121, 19);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(246, 21);
            this.cbWU.TabIndex = 16;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(9, 19);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 17;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbSearchResults
            // 
            this.gbSearchResults.Controls.Add(this.lvUsedOrdered);
            this.gbSearchResults.Controls.Add(this.lvOrders);
            this.gbSearchResults.Controls.Add(this.lblTotal);
            this.gbSearchResults.Controls.Add(this.btnNext);
            this.gbSearchResults.Controls.Add(this.btnPrev);
            this.gbSearchResults.Controls.Add(this.lvEmployeeMeals);
            this.gbSearchResults.Location = new System.Drawing.Point(15, 250);
            this.gbSearchResults.Name = "gbSearchResults";
            this.gbSearchResults.Size = new System.Drawing.Size(836, 328);
            this.gbSearchResults.TabIndex = 1;
            this.gbSearchResults.TabStop = false;
            this.gbSearchResults.Text = "Search results";
            // 
            // lvUsedOrdered
            // 
            this.lvUsedOrdered.GridLines = true;
            this.lvUsedOrdered.Location = new System.Drawing.Point(12, 36);
            this.lvUsedOrdered.Name = "lvUsedOrdered";
            this.lvUsedOrdered.Size = new System.Drawing.Size(810, 270);
            this.lvUsedOrdered.TabIndex = 22;
            this.lvUsedOrdered.UseCompatibleStateImageBehavior = false;
            this.lvUsedOrdered.View = System.Windows.Forms.View.Details;
            this.lvUsedOrdered.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvUsedOrdered_ColumnClick);
            // 
            // lvOrders
            // 
            this.lvOrders.GridLines = true;
            this.lvOrders.Location = new System.Drawing.Point(14, 36);
            this.lvOrders.Name = "lvOrders";
            this.lvOrders.Size = new System.Drawing.Size(808, 270);
            this.lvOrders.TabIndex = 21;
            this.lvOrders.UseCompatibleStateImageBehavior = false;
            this.lvOrders.View = System.Windows.Forms.View.Details;
            this.lvOrders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvOrders_ColumnClick);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(670, 309);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(152, 16);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(790, 9);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 19;
            this.btnNext.Text = ">";
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(750, 9);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 18;
            this.btnPrev.Text = "<";
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lvEmployeeMeals
            // 
            this.lvEmployeeMeals.FullRowSelect = true;
            this.lvEmployeeMeals.GridLines = true;
            this.lvEmployeeMeals.HideSelection = false;
            this.lvEmployeeMeals.Location = new System.Drawing.Point(14, 36);
            this.lvEmployeeMeals.Name = "lvEmployeeMeals";
            this.lvEmployeeMeals.Size = new System.Drawing.Size(808, 270);
            this.lvEmployeeMeals.TabIndex = 4;
            this.lvEmployeeMeals.UseCompatibleStateImageBehavior = false;
            this.lvEmployeeMeals.View = System.Windows.Forms.View.Details;
            this.lvEmployeeMeals.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvEmployeeMeals_ColumnClick);
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.btnGenerateReport);
            this.gbReport.Controls.Add(this.rbNo);
            this.gbReport.Controls.Add(this.rbYes);
            this.gbReport.Controls.Add(this.lblSumary);
            this.gbReport.Location = new System.Drawing.Point(15, 583);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(296, 55);
            this.gbReport.TabIndex = 2;
            this.gbReport.TabStop = false;
            this.gbReport.Text = "Report";
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(176, 19);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(105, 23);
            this.btnGenerateReport.TabIndex = 31;
            this.btnGenerateReport.Text = "Generate report";
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Location = new System.Drawing.Point(131, 22);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(39, 17);
            this.rbNo.TabIndex = 30;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "No";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Checked = true;
            this.rbYes.Location = new System.Drawing.Point(82, 22);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(43, 17);
            this.rbYes.TabIndex = 3;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "Yes";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // lblSumary
            // 
            this.lblSumary.Location = new System.Drawing.Point(9, 19);
            this.lblSumary.Name = "lblSumary";
            this.lblSumary.Size = new System.Drawing.Size(67, 23);
            this.lblSumary.TabIndex = 29;
            this.lblSumary.Text = "Sumary:";
            this.lblSumary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbOrderExists
            // 
            this.rbOrderExists.AutoSize = true;
            this.rbOrderExists.Location = new System.Drawing.Point(142, 14);
            this.rbOrderExists.Name = "rbOrderExists";
            this.rbOrderExists.Size = new System.Drawing.Size(74, 17);
            this.rbOrderExists.TabIndex = 3;
            this.rbOrderExists.Text = "With order";
            this.rbOrderExists.UseVisualStyleBackColor = true;
            // 
            // rbOrderDoesntExist
            // 
            this.rbOrderDoesntExist.AutoSize = true;
            this.rbOrderDoesntExist.Checked = true;
            this.rbOrderDoesntExist.Location = new System.Drawing.Point(10, 14);
            this.rbOrderDoesntExist.Name = "rbOrderDoesntExist";
            this.rbOrderDoesntExist.Size = new System.Drawing.Size(89, 17);
            this.rbOrderDoesntExist.TabIndex = 2;
            this.rbOrderDoesntExist.TabStop = true;
            this.rbOrderDoesntExist.Text = "Without order";
            this.rbOrderDoesntExist.UseVisualStyleBackColor = true;
            this.rbOrderDoesntExist.CheckedChanged += new System.EventHandler(this.rbOrderDoesntExist_CheckedChanged);
            // 
            // rbOrders
            // 
            this.rbOrders.AutoSize = true;
            this.rbOrders.Location = new System.Drawing.Point(14, 46);
            this.rbOrders.Name = "rbOrders";
            this.rbOrders.Size = new System.Drawing.Size(56, 17);
            this.rbOrders.TabIndex = 1;
            this.rbOrders.Text = "Orders";
            this.rbOrders.UseVisualStyleBackColor = true;
            this.rbOrders.CheckedChanged += new System.EventHandler(this.rbOrders_CheckedChanged);
            // 
            // rbUsedMeals
            // 
            this.rbUsedMeals.AutoSize = true;
            this.rbUsedMeals.Checked = true;
            this.rbUsedMeals.Location = new System.Drawing.Point(14, 19);
            this.rbUsedMeals.Name = "rbUsedMeals";
            this.rbUsedMeals.Size = new System.Drawing.Size(80, 17);
            this.rbUsedMeals.TabIndex = 0;
            this.rbUsedMeals.TabStop = true;
            this.rbUsedMeals.Text = "Used meals";
            this.rbUsedMeals.UseVisualStyleBackColor = true;
            this.rbUsedMeals.CheckedChanged += new System.EventHandler(this.rbUsedMeals_CheckedChanged);
            // 
            // gbWithOrWithout
            // 
            this.gbWithOrWithout.Controls.Add(this.rbOrderDoesntExist);
            this.gbWithOrWithout.Controls.Add(this.rbOrderExists);
            this.gbWithOrWithout.Location = new System.Drawing.Point(116, 31);
            this.gbWithOrWithout.Name = "gbWithOrWithout";
            this.gbWithOrWithout.Size = new System.Drawing.Size(251, 39);
            this.gbWithOrWithout.TabIndex = 4;
            this.gbWithOrWithout.TabStop = false;
            // 
            // gbTypeReportChoice
            // 
            this.gbTypeReportChoice.Controls.Add(this.rbUsedOrdered);
            this.gbTypeReportChoice.Controls.Add(this.gbWithOrWithout);
            this.gbTypeReportChoice.Controls.Add(this.rbOrders);
            this.gbTypeReportChoice.Controls.Add(this.rbUsedMeals);
            this.gbTypeReportChoice.Location = new System.Drawing.Point(15, -2);
            this.gbTypeReportChoice.Name = "gbTypeReportChoice";
            this.gbTypeReportChoice.Size = new System.Drawing.Size(684, 83);
            this.gbTypeReportChoice.TabIndex = 3;
            this.gbTypeReportChoice.TabStop = false;
            // 
            // rbUsedOrdered
            // 
            this.rbUsedOrdered.AutoSize = true;
            this.rbUsedOrdered.Location = new System.Drawing.Point(417, 19);
            this.rbUsedOrdered.Name = "rbUsedOrdered";
            this.rbUsedOrdered.Size = new System.Drawing.Size(183, 17);
            this.rbUsedOrdered.TabIndex = 5;
            this.rbUsedOrdered.TabStop = true;
            this.rbUsedOrdered.Text = "Orders and used meals - summary";
            this.rbUsedOrdered.UseVisualStyleBackColor = true;
            this.rbUsedOrdered.CheckedChanged += new System.EventHandler(this.rbUsedOrdered_CheckedChanged);
            // 
            // MealEmployeeSchedulePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTypeReportChoice);
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.gbSearchResults);
            this.Controls.Add(this.gbSearchCriteria);
            this.Name = "MealEmployeeSchedulePreview";
            this.Size = new System.Drawing.Size(882, 640);
            this.Load += new System.EventHandler(this.MealEmployeeSchedulePreview_Load);
            this.gbSearchCriteria.ResumeLayout(false);
            this.gbSearchCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).EndInit();
            this.gbSearchResults.ResumeLayout(false);
            this.gbReport.ResumeLayout(false);
            this.gbReport.PerformLayout();
            this.gbWithOrWithout.ResumeLayout(false);
            this.gbWithOrWithout.PerformLayout();
            this.gbTypeReportChoice.ResumeLayout(false);
            this.gbTypeReportChoice.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSearchCriteria;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.ComboBox cbMealType;
        private System.Windows.Forms.Label lblMealType;
        private System.Windows.Forms.ComboBox cbPlace;
        private System.Windows.Forms.Label lblPlace;
        private System.Windows.Forms.GroupBox gbSearchResults;
        private System.Windows.Forms.ListView lvEmployeeMeals;
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbYes;
        private System.Windows.Forms.Label lblSumary;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.NumericUpDown numQtyTo;
        private System.Windows.Forms.NumericUpDown numQtyFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.RadioButton rbOrderExists;
        private System.Windows.Forms.RadioButton rbOrderDoesntExist;
        private System.Windows.Forms.RadioButton rbOrders;
        private System.Windows.Forms.RadioButton rbUsedMeals;
        private System.Windows.Forms.GroupBox gbWithOrWithout;
        private System.Windows.Forms.GroupBox gbTypeReportChoice;
        private System.Windows.Forms.ListView lvOrders;
        private System.Windows.Forms.RadioButton rbUsedOrdered;
        private System.Windows.Forms.ListView lvUsedOrdered;
    }
}
