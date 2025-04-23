namespace UI
{
    partial class MealsUsed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MealsUsed));
            this.gbMealsUsed = new System.Windows.Forms.GroupBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.numQtyTo = new System.Windows.Forms.NumericUpDown();
            this.numMoneyAmtFrom = new System.Windows.Forms.NumericUpDown();
            this.numMoneyAmtTo = new System.Windows.Forms.NumericUpDown();
            this.numQtyFrom = new System.Windows.Forms.NumericUpDown();
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMoneyAmt = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.lblMealType = new System.Windows.Forms.Label();
            this.lblPoint = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblEmpl = new System.Windows.Forms.Label();
            this.lblWU = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbMealType = new System.Windows.Forms.ComboBox();
            this.cbPoint = new System.Windows.Forms.ComboBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.cbEmpl = new System.Windows.Forms.ComboBox();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lvMealsUsed = new System.Windows.Forms.ListView();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.lblSummary = new System.Windows.Forms.Label();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.gbMealsUsed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).BeginInit();
            this.gbReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMealsUsed
            // 
            this.gbMealsUsed.Controls.Add(this.chbHierarhicly);
            this.gbMealsUsed.Controls.Add(this.btnWUTree);
            this.gbMealsUsed.Controls.Add(this.numQtyTo);
            this.gbMealsUsed.Controls.Add(this.numMoneyAmtFrom);
            this.gbMealsUsed.Controls.Add(this.numMoneyAmtTo);
            this.gbMealsUsed.Controls.Add(this.numQtyFrom);
            this.gbMealsUsed.Controls.Add(this.btnClear);
            this.gbMealsUsed.Controls.Add(this.label2);
            this.gbMealsUsed.Controls.Add(this.label1);
            this.gbMealsUsed.Controls.Add(this.lblMoneyAmt);
            this.gbMealsUsed.Controls.Add(this.lblQty);
            this.gbMealsUsed.Controls.Add(this.lblMealType);
            this.gbMealsUsed.Controls.Add(this.lblPoint);
            this.gbMealsUsed.Controls.Add(this.lblTo);
            this.gbMealsUsed.Controls.Add(this.lblFrom);
            this.gbMealsUsed.Controls.Add(this.lblEmpl);
            this.gbMealsUsed.Controls.Add(this.lblWU);
            this.gbMealsUsed.Controls.Add(this.btnSearch);
            this.gbMealsUsed.Controls.Add(this.cbMealType);
            this.gbMealsUsed.Controls.Add(this.cbPoint);
            this.gbMealsUsed.Controls.Add(this.dtpTo);
            this.gbMealsUsed.Controls.Add(this.dtpFrom);
            this.gbMealsUsed.Controls.Add(this.cbEmpl);
            this.gbMealsUsed.Controls.Add(this.cbWU);
            this.gbMealsUsed.Location = new System.Drawing.Point(12, 19);
            this.gbMealsUsed.Name = "gbMealsUsed";
            this.gbMealsUsed.Size = new System.Drawing.Size(595, 171);
            this.gbMealsUsed.TabIndex = 0;
            this.gbMealsUsed.TabStop = false;
            this.gbMealsUsed.Text = "Search parameters";
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(257, 17);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 25;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // numQtyTo
            // 
            this.numQtyTo.Location = new System.Drawing.Point(498, 77);
            this.numQtyTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyTo.Name = "numQtyTo";
            this.numQtyTo.Size = new System.Drawing.Size(65, 20);
            this.numQtyTo.TabIndex = 16;
            // 
            // numMoneyAmtFrom
            // 
            this.numMoneyAmtFrom.Location = new System.Drawing.Point(418, 105);
            this.numMoneyAmtFrom.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numMoneyAmtFrom.Name = "numMoneyAmtFrom";
            this.numMoneyAmtFrom.Size = new System.Drawing.Size(65, 20);
            this.numMoneyAmtFrom.TabIndex = 18;
            // 
            // numMoneyAmtTo
            // 
            this.numMoneyAmtTo.Location = new System.Drawing.Point(498, 105);
            this.numMoneyAmtTo.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numMoneyAmtTo.Name = "numMoneyAmtTo";
            this.numMoneyAmtTo.Size = new System.Drawing.Size(65, 20);
            this.numMoneyAmtTo.TabIndex = 20;
            // 
            // numQtyFrom
            // 
            this.numQtyFrom.Location = new System.Drawing.Point(418, 77);
            this.numQtyFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numQtyFrom.Name = "numQtyFrom";
            this.numQtyFrom.Size = new System.Drawing.Size(65, 20);
            this.numQtyFrom.TabIndex = 14;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(398, 135);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(486, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(486, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "-";
            // 
            // lblMoneyAmt
            // 
            this.lblMoneyAmt.Location = new System.Drawing.Point(312, 107);
            this.lblMoneyAmt.Name = "lblMoneyAmt";
            this.lblMoneyAmt.Size = new System.Drawing.Size(100, 13);
            this.lblMoneyAmt.TabIndex = 17;
            this.lblMoneyAmt.Text = "Money amount:";
            this.lblMoneyAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(346, 79);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(66, 13);
            this.lblQty.TabIndex = 13;
            this.lblQty.Text = "Qyantity:";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMealType
            // 
            this.lblMealType.Location = new System.Drawing.Point(343, 51);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(69, 13);
            this.lblMealType.TabIndex = 11;
            this.lblMealType.Text = "Meal type:";
            this.lblMealType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPoint
            // 
            this.lblPoint.Location = new System.Drawing.Point(355, 20);
            this.lblPoint.Name = "lblPoint";
            this.lblPoint.Size = new System.Drawing.Size(57, 16);
            this.lblPoint.TabIndex = 9;
            this.lblPoint.Text = "Point:";
            this.lblPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(21, 130);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(79, 13);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(21, 104);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(79, 13);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmpl
            // 
            this.lblEmpl.Location = new System.Drawing.Point(21, 74);
            this.lblEmpl.Name = "lblEmpl";
            this.lblEmpl.Size = new System.Drawing.Size(79, 13);
            this.lblEmpl.TabIndex = 3;
            this.lblEmpl.Text = "Employee:";
            this.lblEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(6, 22);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(94, 13);
            this.lblWU.TabIndex = 1;
            this.lblWU.Text = "Working unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(488, 135);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbMealType
            // 
            this.cbMealType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMealType.FormattingEnabled = true;
            this.cbMealType.Location = new System.Drawing.Point(418, 48);
            this.cbMealType.Name = "cbMealType";
            this.cbMealType.Size = new System.Drawing.Size(145, 21);
            this.cbMealType.TabIndex = 12;
            // 
            // cbPoint
            // 
            this.cbPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPoint.FormattingEnabled = true;
            this.cbPoint.Location = new System.Drawing.Point(418, 19);
            this.cbPoint.Name = "cbPoint";
            this.cbPoint.Size = new System.Drawing.Size(145, 21);
            this.cbPoint.TabIndex = 10;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(106, 126);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(145, 20);
            this.dtpTo.TabIndex = 8;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(106, 100);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(145, 20);
            this.dtpFrom.TabIndex = 6;
            // 
            // cbEmpl
            // 
            this.cbEmpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmpl.FormattingEnabled = true;
            this.cbEmpl.Location = new System.Drawing.Point(106, 71);
            this.cbEmpl.Name = "cbEmpl";
            this.cbEmpl.Size = new System.Drawing.Size(145, 21);
            this.cbEmpl.TabIndex = 4;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(106, 19);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(145, 21);
            this.cbWU.TabIndex = 2;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lvMealsUsed
            // 
            this.lvMealsUsed.FullRowSelect = true;
            this.lvMealsUsed.GridLines = true;
            this.lvMealsUsed.Location = new System.Drawing.Point(12, 205);
            this.lvMealsUsed.Name = "lvMealsUsed";
            this.lvMealsUsed.ShowItemToolTips = true;
            this.lvMealsUsed.Size = new System.Drawing.Size(705, 218);
            this.lvMealsUsed.TabIndex = 23;
            this.lvMealsUsed.UseCompatibleStateImageBehavior = false;
            this.lvMealsUsed.View = System.Windows.Forms.View.Details;
            this.lvMealsUsed.SelectedIndexChanged += new System.EventHandler(this.lvMealsUsed_SelectedIndexChanged);
            this.lvMealsUsed.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMealsUsed_ColumnClick);
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.btnGenerateReport);
            this.gbReport.Controls.Add(this.rbNo);
            this.gbReport.Controls.Add(this.rbYes);
            this.gbReport.Controls.Add(this.lblSummary);
            this.gbReport.Location = new System.Drawing.Point(12, 429);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(365, 57);
            this.gbReport.TabIndex = 24;
            this.gbReport.TabStop = false;
            this.gbReport.Text = "Report";
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(221, 19);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(126, 23);
            this.btnGenerateReport.TabIndex = 28;
            this.btnGenerateReport.Text = "Generate report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Location = new System.Drawing.Point(139, 22);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(39, 17);
            this.rbNo.TabIndex = 27;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "No";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Location = new System.Drawing.Point(80, 22);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(43, 17);
            this.rbYes.TabIndex = 26;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "Yes";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Location = new System.Drawing.Point(6, 22);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(53, 13);
            this.lblSummary.TabIndex = 25;
            this.lblSummary.Text = "Summary:";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(106, 41);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 34;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // MealsUsed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbReport);
            this.Controls.Add(this.lvMealsUsed);
            this.Controls.Add(this.gbMealsUsed);
            this.MaximumSize = new System.Drawing.Size(734, 511);
            this.MinimumSize = new System.Drawing.Size(734, 511);
            this.Name = "MealsUsed";
            this.Size = new System.Drawing.Size(734, 511);
            this.Load += new System.EventHandler(this.MealsUsed_Load);
            this.gbMealsUsed.ResumeLayout(false);
            this.gbMealsUsed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMoneyAmtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyFrom)).EndInit();
            this.gbReport.ResumeLayout(false);
            this.gbReport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMealsUsed;
        private System.Windows.Forms.ListView lvMealsUsed;
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbYes;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.ComboBox cbMealType;
        private System.Windows.Forms.ComboBox cbPoint;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.ComboBox cbEmpl;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblMoneyAmt;
        private System.Windows.Forms.Label lblMealType;
        private System.Windows.Forms.Label lblPoint;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblEmpl;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.NumericUpDown numQtyTo;
        private System.Windows.Forms.NumericUpDown numMoneyAmtFrom;
        private System.Windows.Forms.NumericUpDown numMoneyAmtTo;
        private System.Windows.Forms.NumericUpDown numQtyFrom;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.CheckBox chbHierarhicly;
    }
}