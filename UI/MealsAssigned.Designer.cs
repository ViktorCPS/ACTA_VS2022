namespace UI
{
    partial class MealsAssigned
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MealsAssigned));
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvMealsAssigned = new System.Windows.Forms.ListView();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbUnlimited = new System.Windows.Forms.CheckBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numMoneyAmtTo = new System.Windows.Forms.NumericUpDown();
            this.numQtyTo = new System.Windows.Forms.NumericUpDown();
            this.numQtyDailyTo = new System.Windows.Forms.NumericUpDown();
            this.numQtyDailyFrom = new System.Windows.Forms.NumericUpDown();
            this.numMoneyAmtFrom = new System.Windows.Forms.NumericUpDown();
            this.numQtyFrom = new System.Windows.Forms.NumericUpDown();
            this.lblMoneyAmt = new System.Windows.Forms.Label();
            this.lblQtyDaily = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.cbMealType = new System.Windows.Forms.ComboBox();
            this.lblMealType = new System.Windows.Forms.Label();
            this.cbEmplName = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyDailyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyDailyFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(684, 193);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 23);
            this.btnNext.TabIndex = 27;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(646, 193);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 23);
            this.btnPrev.TabIndex = 26;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(121, 475);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 31;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(17, 475);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 30;
            this.btnAdd.Text = "Add new";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lvMealsAssigned
            // 
            this.lvMealsAssigned.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMealsAssigned.FullRowSelect = true;
            this.lvMealsAssigned.GridLines = true;
            this.lvMealsAssigned.HideSelection = false;
            this.lvMealsAssigned.Location = new System.Drawing.Point(17, 222);
            this.lvMealsAssigned.Name = "lvMealsAssigned";
            this.lvMealsAssigned.Size = new System.Drawing.Size(704, 239);
            this.lvMealsAssigned.TabIndex = 28;
            this.lvMealsAssigned.UseCompatibleStateImageBehavior = false;
            this.lvMealsAssigned.View = System.Windows.Forms.View.Details;
            this.lvMealsAssigned.SelectedIndexChanged += new System.EventHandler(this.lvMealsAssigned_SelectedIndexChanged);
            this.lvMealsAssigned.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealsAssigned_ColumnClick);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.chbHierarhicly);
            this.gbSearch.Controls.Add(this.btnWUTree);
            this.gbSearch.Controls.Add(this.cbUnlimited);
            this.gbSearch.Controls.Add(this.dtpTo);
            this.gbSearch.Controls.Add(this.lblTo);
            this.gbSearch.Controls.Add(this.dtpFrom);
            this.gbSearch.Controls.Add(this.lblFrom);
            this.gbSearch.Controls.Add(this.label6);
            this.gbSearch.Controls.Add(this.label5);
            this.gbSearch.Controls.Add(this.label4);
            this.gbSearch.Controls.Add(this.numMoneyAmtTo);
            this.gbSearch.Controls.Add(this.numQtyTo);
            this.gbSearch.Controls.Add(this.numQtyDailyTo);
            this.gbSearch.Controls.Add(this.numQtyDailyFrom);
            this.gbSearch.Controls.Add(this.numMoneyAmtFrom);
            this.gbSearch.Controls.Add(this.numQtyFrom);
            this.gbSearch.Controls.Add(this.lblMoneyAmt);
            this.gbSearch.Controls.Add(this.lblQtyDaily);
            this.gbSearch.Controls.Add(this.lblQty);
            this.gbSearch.Controls.Add(this.cbMealType);
            this.gbSearch.Controls.Add(this.lblMealType);
            this.gbSearch.Controls.Add(this.cbEmplName);
            this.gbSearch.Controls.Add(this.lblEmployee);
            this.gbSearch.Controls.Add(this.cbWorkingUnit);
            this.gbSearch.Controls.Add(this.lblWorkingUnit);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.btnClear);
            this.gbSearch.Location = new System.Drawing.Point(17, 3);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(606, 201);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(104, 49);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 33;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(303, 22);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 32;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbUnlimited
            // 
            this.cbUnlimited.AutoSize = true;
            this.cbUnlimited.Location = new System.Drawing.Point(225, 172);
            this.cbUnlimited.Name = "cbUnlimited";
            this.cbUnlimited.Size = new System.Drawing.Size(105, 17);
            this.cbUnlimited.TabIndex = 11;
            this.cbUnlimited.Text = "Include unlimited";
            this.cbUnlimited.UseVisualStyleBackColor = true;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(104, 172);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(115, 20);
            this.dtpTo.TabIndex = 10;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(27, 172);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(71, 20);
            this.lblTo.TabIndex = 9;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(104, 146);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(115, 20);
            this.dtpFrom.TabIndex = 8;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(27, 146);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(71, 20);
            this.lblFrom.TabIndex = 7;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(499, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "-";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(499, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "-";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(499, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "-";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numMoneyAmtTo
            // 
            this.numMoneyAmtTo.Location = new System.Drawing.Point(515, 81);
            this.numMoneyAmtTo.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numMoneyAmtTo.Name = "numMoneyAmtTo";
            this.numMoneyAmtTo.Size = new System.Drawing.Size(74, 20);
            this.numMoneyAmtTo.TabIndex = 23;
            // 
            // numQtyTo
            // 
            this.numQtyTo.Location = new System.Drawing.Point(515, 25);
            this.numQtyTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyTo.Name = "numQtyTo";
            this.numQtyTo.Size = new System.Drawing.Size(74, 20);
            this.numQtyTo.TabIndex = 15;
            // 
            // numQtyDailyTo
            // 
            this.numQtyDailyTo.Location = new System.Drawing.Point(544, 53);
            this.numQtyDailyTo.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numQtyDailyTo.Name = "numQtyDailyTo";
            this.numQtyDailyTo.Size = new System.Drawing.Size(45, 20);
            this.numQtyDailyTo.TabIndex = 19;
            // 
            // numQtyDailyFrom
            // 
            this.numQtyDailyFrom.Location = new System.Drawing.Point(448, 53);
            this.numQtyDailyFrom.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numQtyDailyFrom.Name = "numQtyDailyFrom";
            this.numQtyDailyFrom.Size = new System.Drawing.Size(45, 20);
            this.numQtyDailyFrom.TabIndex = 17;
            // 
            // numMoneyAmtFrom
            // 
            this.numMoneyAmtFrom.Location = new System.Drawing.Point(419, 81);
            this.numMoneyAmtFrom.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numMoneyAmtFrom.Name = "numMoneyAmtFrom";
            this.numMoneyAmtFrom.Size = new System.Drawing.Size(74, 20);
            this.numMoneyAmtFrom.TabIndex = 21;
            // 
            // numQtyFrom
            // 
            this.numQtyFrom.Location = new System.Drawing.Point(419, 25);
            this.numQtyFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyFrom.Name = "numQtyFrom";
            this.numQtyFrom.Size = new System.Drawing.Size(74, 20);
            this.numQtyFrom.TabIndex = 13;
            // 
            // lblMoneyAmt
            // 
            this.lblMoneyAmt.Location = new System.Drawing.Point(332, 83);
            this.lblMoneyAmt.Name = "lblMoneyAmt";
            this.lblMoneyAmt.Size = new System.Drawing.Size(81, 13);
            this.lblMoneyAmt.TabIndex = 20;
            this.lblMoneyAmt.Text = "Money amount:";
            this.lblMoneyAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQtyDaily
            // 
            this.lblQtyDaily.Location = new System.Drawing.Point(302, 55);
            this.lblQtyDaily.Name = "lblQtyDaily";
            this.lblQtyDaily.Size = new System.Drawing.Size(111, 13);
            this.lblQtyDaily.TabIndex = 16;
            this.lblQtyDaily.Text = "Quantity daily:";
            this.lblQtyDaily.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(333, 22);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(80, 23);
            this.lblQty.TabIndex = 12;
            this.lblQty.Text = "Quantity:";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMealType
            // 
            this.cbMealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMealType.FormattingEnabled = true;
            this.cbMealType.Location = new System.Drawing.Point(104, 114);
            this.cbMealType.Name = "cbMealType";
            this.cbMealType.Size = new System.Drawing.Size(193, 21);
            this.cbMealType.TabIndex = 6;
            // 
            // lblMealType
            // 
            this.lblMealType.Location = new System.Drawing.Point(9, 117);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(92, 13);
            this.lblMealType.TabIndex = 5;
            this.lblMealType.Text = "Meal type:";
            this.lblMealType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmplName
            // 
            this.cbEmplName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmplName.FormattingEnabled = true;
            this.cbEmplName.Location = new System.Drawing.Point(104, 80);
            this.cbEmplName.Name = "cbEmplName";
            this.cbEmplName.Size = new System.Drawing.Size(193, 21);
            this.cbEmplName.TabIndex = 4;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEmployee.Location = new System.Drawing.Point(12, 83);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(89, 13);
            this.lblEmployee.TabIndex = 3;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.FormattingEnabled = true;
            this.cbWorkingUnit.Location = new System.Drawing.Point(104, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(193, 21);
            this.cbWorkingUnit.TabIndex = 2;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(6, 27);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(92, 13);
            this.lblWorkingUnit.TabIndex = 1;
            this.lblWorkingUnit.Text = "Working unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(509, 172);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 25;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(423, 172);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 23);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(636, 464);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(85, 13);
            this.lblTotal.TabIndex = 29;
            this.lblTotal.Text = "label1";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MealsAssigned
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvMealsAssigned);
            this.Controls.Add(this.gbSearch);
            this.Name = "MealsAssigned";
            this.Size = new System.Drawing.Size(734, 511);
            this.Load += new System.EventHandler(this.MealsAssigned_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyDailyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyDailyFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvMealsAssigned;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.ComboBox cbMealType;
        private System.Windows.Forms.Label lblMealType;
        private System.Windows.Forms.ComboBox cbEmplName;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblMoneyAmt;
        private System.Windows.Forms.Label lblQtyDaily;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMoneyAmtTo;
        private System.Windows.Forms.NumericUpDown numQtyTo;
        private System.Windows.Forms.NumericUpDown numQtyDailyTo;
        private System.Windows.Forms.NumericUpDown numQtyDailyFrom;
        private System.Windows.Forms.NumericUpDown numMoneyAmtFrom;
        private System.Windows.Forms.NumericUpDown numQtyFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.CheckBox cbUnlimited;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.CheckBox chbHierarhicly;
    }
}
