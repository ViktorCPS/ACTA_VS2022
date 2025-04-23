namespace UI
{
    partial class VacationAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VacationAdd));
            this.gbDefinitionType = new System.Windows.Forms.GroupBox();
            this.rbForWU = new System.Windows.Forms.RadioButton();
            this.rbForEmpl = new System.Windows.Forms.RadioButton();
            this.gbData = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.nudDays = new System.Windows.Forms.NumericUpDown();
            this.lblDays = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.dtpYearFrom = new System.Windows.Forms.DateTimePicker();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.gbUsingPlan = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblValidTo = new System.Windows.Forms.Label();
            this.dtpValidTo = new System.Windows.Forms.DateTimePicker();
            this.lvIntervals = new System.Windows.Forms.ListView();
            this.lblDblClick = new System.Windows.Forms.Label();
            this.btnAddInterval = new System.Windows.Forms.Button();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.chbMultiInterval = new System.Windows.Forms.CheckBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbDefinitionType.SuspendLayout();
            this.gbData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDays)).BeginInit();
            this.gbUsingPlan.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDefinitionType
            // 
            this.gbDefinitionType.Controls.Add(this.rbForWU);
            this.gbDefinitionType.Controls.Add(this.rbForEmpl);
            this.gbDefinitionType.Location = new System.Drawing.Point(253, 12);
            this.gbDefinitionType.Name = "gbDefinitionType";
            this.gbDefinitionType.Size = new System.Drawing.Size(309, 71);
            this.gbDefinitionType.TabIndex = 0;
            this.gbDefinitionType.TabStop = false;
            this.gbDefinitionType.Text = "Definition type";
            // 
            // rbForWU
            // 
            this.rbForWU.AutoSize = true;
            this.rbForWU.Location = new System.Drawing.Point(187, 31);
            this.rbForWU.Name = "rbForWU";
            this.rbForWU.Size = new System.Drawing.Size(100, 17);
            this.rbForWU.TabIndex = 1;
            this.rbForWU.TabStop = true;
            this.rbForWU.Text = "For working unit";
            this.rbForWU.UseVisualStyleBackColor = true;
            // 
            // rbForEmpl
            // 
            this.rbForEmpl.AutoSize = true;
            this.rbForEmpl.Checked = true;
            this.rbForEmpl.Location = new System.Drawing.Point(6, 31);
            this.rbForEmpl.Name = "rbForEmpl";
            this.rbForEmpl.Size = new System.Drawing.Size(88, 17);
            this.rbForEmpl.TabIndex = 0;
            this.rbForEmpl.TabStop = true;
            this.rbForEmpl.Text = "For employee";
            this.rbForEmpl.UseVisualStyleBackColor = true;
            this.rbForEmpl.CheckedChanged += new System.EventHandler(this.rbForEmpl_CheckedChanged);
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.label4);
            this.gbData.Controls.Add(this.label3);
            this.gbData.Controls.Add(this.label2);
            this.gbData.Controls.Add(this.label1);
            this.gbData.Controls.Add(this.lblNote);
            this.gbData.Controls.Add(this.richTextBox1);
            this.gbData.Controls.Add(this.nudDays);
            this.gbData.Controls.Add(this.lblDays);
            this.gbData.Controls.Add(this.lblYear);
            this.gbData.Controls.Add(this.dtpYearFrom);
            this.gbData.Controls.Add(this.chbHierarhicly);
            this.gbData.Controls.Add(this.btnWUTree);
            this.gbData.Controls.Add(this.cbEmployee);
            this.gbData.Controls.Add(this.lblEmployee);
            this.gbData.Controls.Add(this.cbWU);
            this.gbData.Controls.Add(this.lblWU);
            this.gbData.Location = new System.Drawing.Point(12, 102);
            this.gbData.Name = "gbData";
            this.gbData.Size = new System.Drawing.Size(385, 418);
            this.gbData.TabIndex = 1;
            this.gbData.TabStop = false;
            this.gbData.Text = "Data";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(168, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "*";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(168, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "*";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(315, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(315, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNote
            // 
            this.lblNote.Location = new System.Drawing.Point(27, 203);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(71, 23);
            this.lblNote.TabIndex = 35;
            this.lblNote.Text = "Note:";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(104, 205);
            this.richTextBox1.MaxLength = 500;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(202, 175);
            this.richTextBox1.TabIndex = 34;
            // 
            // nudDays
            // 
            this.nudDays.Location = new System.Drawing.Point(107, 160);
            this.nudDays.Name = "nudDays";
            this.nudDays.Size = new System.Drawing.Size(55, 20);
            this.nudDays.TabIndex = 33;
            this.nudDays.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // lblDays
            // 
            this.lblDays.Location = new System.Drawing.Point(35, 157);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(66, 23);
            this.lblDays.TabIndex = 32;
            this.lblDays.Text = "From:";
            this.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(33, 123);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(66, 23);
            this.lblYear.TabIndex = 30;
            this.lblYear.Text = "Year:";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpYearFrom
            // 
            this.dtpYearFrom.CustomFormat = "yyyy";
            this.dtpYearFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpYearFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpYearFrom.Location = new System.Drawing.Point(106, 124);
            this.dtpYearFrom.Name = "dtpYearFrom";
            this.dtpYearFrom.ShowUpDown = true;
            this.dtpYearFrom.Size = new System.Drawing.Size(56, 20);
            this.dtpYearFrom.TabIndex = 31;
            this.dtpYearFrom.ValueChanged += new System.EventHandler(this.dtpYearFrom_ValueChanged);
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(201, 56);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(87, 24);
            this.chbHierarhicly.TabIndex = 29;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(336, 27);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 28;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbEmployee
            // 
            this.cbEmployee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbEmployee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbEmployee.Location = new System.Drawing.Point(107, 86);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(202, 21);
            this.cbEmployee.TabIndex = 27;
            this.cbEmployee.Leave += new System.EventHandler(cbEmployee_Leave);
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(27, 84);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(72, 23);
            this.lblEmployee.TabIndex = 26;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.Location = new System.Drawing.Point(107, 29);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(202, 21);
            this.cbWU.TabIndex = 25;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblWU
            // 
            this.lblWU.Location = new System.Drawing.Point(11, 29);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(88, 23);
            this.lblWU.TabIndex = 24;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbUsingPlan
            // 
            this.gbUsingPlan.Controls.Add(this.label5);
            this.gbUsingPlan.Controls.Add(this.lblValidTo);
            this.gbUsingPlan.Controls.Add(this.dtpValidTo);
            this.gbUsingPlan.Controls.Add(this.lvIntervals);
            this.gbUsingPlan.Controls.Add(this.lblDblClick);
            this.gbUsingPlan.Controls.Add(this.btnAddInterval);
            this.gbUsingPlan.Controls.Add(this.gbTimeInterval);
            this.gbUsingPlan.Location = new System.Drawing.Point(414, 102);
            this.gbUsingPlan.Name = "gbUsingPlan";
            this.gbUsingPlan.Size = new System.Drawing.Size(385, 418);
            this.gbUsingPlan.TabIndex = 2;
            this.gbUsingPlan.TabStop = false;
            this.gbUsingPlan.Text = "Using plan";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(326, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 44;
            this.label5.Text = "*";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValidTo
            // 
            this.lblValidTo.Location = new System.Drawing.Point(6, 29);
            this.lblValidTo.Name = "lblValidTo";
            this.lblValidTo.Size = new System.Drawing.Size(66, 23);
            this.lblValidTo.TabIndex = 42;
            this.lblValidTo.Text = "Valid to:";
            this.lblValidTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpValidTo
            // 
            this.dtpValidTo.CustomFormat = "dd.MM.yyyy";
            this.dtpValidTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpValidTo.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpValidTo.Location = new System.Drawing.Point(78, 30);
            this.dtpValidTo.Name = "dtpValidTo";
            this.dtpValidTo.Size = new System.Drawing.Size(242, 20);
            this.dtpValidTo.TabIndex = 43;
            this.dtpValidTo.ValueChanged += new System.EventHandler(this.dtpValidTo_ValueChanged);
            // 
            // lvIntervals
            // 
            this.lvIntervals.FullRowSelect = true;
            this.lvIntervals.GridLines = true;
            this.lvIntervals.HideSelection = false;
            this.lvIntervals.Location = new System.Drawing.Point(41, 203);
            this.lvIntervals.Name = "lvIntervals";
            this.lvIntervals.Size = new System.Drawing.Size(305, 177);
            this.lvIntervals.TabIndex = 41;
            this.lvIntervals.UseCompatibleStateImageBehavior = false;
            this.lvIntervals.View = System.Windows.Forms.View.Details;
            this.lvIntervals.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvIntervals_MouseDoubleClick);
            // 
            // lblDblClick
            // 
            this.lblDblClick.Location = new System.Drawing.Point(41, 383);
            this.lblDblClick.Name = "lblDblClick";
            this.lblDblClick.Size = new System.Drawing.Size(305, 23);
            this.lblDblClick.TabIndex = 40;
            this.lblDblClick.Text = "Dbl click to remove from list";
            this.lblDblClick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddInterval
            // 
            this.btnAddInterval.Location = new System.Drawing.Point(136, 174);
            this.btnAddInterval.Name = "btnAddInterval";
            this.btnAddInterval.Size = new System.Drawing.Size(126, 23);
            this.btnAddInterval.TabIndex = 38;
            this.btnAddInterval.Text = "Add interval";
            this.btnAddInterval.Click += new System.EventHandler(this.btnAddInterval_Click);
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.chbMultiInterval);
            this.gbTimeInterval.Controls.Add(this.dtpTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(78, 64);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(242, 104);
            this.gbTimeInterval.TabIndex = 31;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time interval";
            // 
            // chbMultiInterval
            // 
            this.chbMultiInterval.AutoSize = true;
            this.chbMultiInterval.Location = new System.Drawing.Point(77, 81);
            this.chbMultiInterval.Name = "chbMultiInterval";
            this.chbMultiInterval.Size = new System.Drawing.Size(92, 17);
            this.chbMultiInterval.TabIndex = 31;
            this.chbMultiInterval.Text = "More intervals";
            this.chbMultiInterval.UseVisualStyleBackColor = true;
            this.chbMultiInterval.CheckedChanged += new System.EventHandler(this.chbMultiInterval_CheckedChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(77, 50);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(126, 20);
            this.dtpTo.TabIndex = 30;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 22);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(64, 23);
            this.lblFrom.TabIndex = 27;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(10, 48);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(61, 23);
            this.lblTo.TabIndex = 28;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(77, 22);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(126, 20);
            this.dtpFrom.TabIndex = 29;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(724, 537);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 537);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 21;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 538);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // VacationAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 573);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbUsingPlan);
            this.Controls.Add(this.gbData);
            this.Controls.Add(this.gbDefinitionType);
            this.KeyPreview = true;
            this.Name = "VacationAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Add vacation evidence";
            this.Load += new System.EventHandler(this.VacationAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VacationAdd_KeyUp);
            this.gbDefinitionType.ResumeLayout(false);
            this.gbDefinitionType.PerformLayout();
            this.gbData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDays)).EndInit();
            this.gbUsingPlan.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbTimeInterval.PerformLayout();
            this.ResumeLayout(false);

        }

        void cbEmployee_Leave(object sender, System.EventArgs e)
        {
            if (cbEmployee.SelectedIndex == -1) {
                cbEmployee.SelectedIndex = 0;
            }
        }

        #endregion

        private System.Windows.Forms.GroupBox gbDefinitionType;
        private System.Windows.Forms.RadioButton rbForWU;
        private System.Windows.Forms.RadioButton rbForEmpl;
        private System.Windows.Forms.GroupBox gbData;
        private System.Windows.Forms.GroupBox gbUsingPlan;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.DateTimePicker dtpYearFrom;
        private System.Windows.Forms.NumericUpDown nudDays;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.CheckBox chbMultiInterval;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnAddInterval;
        private System.Windows.Forms.Label lblDblClick;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvIntervals;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblValidTo;
        private System.Windows.Forms.DateTimePicker dtpValidTo;
    }
}