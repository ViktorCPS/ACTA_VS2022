namespace UI
{
    partial class MealsWorkingUnitSchedules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MealsWorkingUnitSchedules));
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.btnWUTree = new System.Windows.Forms.Button();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cbEmployee = new System.Windows.Forms.ComboBox();
            this.btnByMeals = new System.Windows.Forms.Button();
            this.btnByDays = new System.Windows.Forms.Button();
            this.cbOverrideSch = new System.Windows.Forms.CheckBox();
            this.panelMeals = new System.Windows.Forms.Panel();
            this.lblMeals = new System.Windows.Forms.Label();
            this.tbNote = new System.Windows.Forms.RichTextBox();
            this.gbLegend = new System.Windows.Forms.GroupBox();
            this.lblDaysWithMeals = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblMealAvaiable = new System.Windows.Forms.Label();
            this.lblNotWorkingDays = new System.Windows.Forms.Label();
            this.lblWorkingDays = new System.Windows.Forms.Label();
            this.lblDiseabled = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelMeals.SuspendLayout();
            this.gbLegend.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(635, 80);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(33, 41);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = "->";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(470, 80);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(33, 41);
            this.btnPrev.TabIndex = 7;
            this.btnPrev.Text = "<-";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // dtpMonth
            // 
            this.dtpMonth.CustomFormat = "MMMM yyyy";
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(509, 92);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.Size = new System.Drawing.Size(120, 20);
            this.dtpMonth.TabIndex = 6;
            this.dtpMonth.ValueChanged += new System.EventHandler(this.dtpMonth_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Location = new System.Drawing.Point(193, 126);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 459);
            this.panel1.TabIndex = 9;
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(404, 12);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(89, 24);
            this.chbHierarhicly.TabIndex = 13;
            this.chbHierarhicly.Text = "Hierarchy ";
            this.chbHierarhicly.CheckedChanged += new System.EventHandler(this.chbHierarhicly_CheckedChanged);
            // 
            // btnWUTree
            // 
            this.btnWUTree.Image = ((System.Drawing.Image)(resources.GetObject("btnWUTree.Image")));
            this.btnWUTree.Location = new System.Drawing.Point(373, 12);
            this.btnWUTree.Name = "btnWUTree";
            this.btnWUTree.Size = new System.Drawing.Size(25, 23);
            this.btnWUTree.TabIndex = 12;
            this.btnWUTree.Click += new System.EventHandler(this.btnWUTree_Click);
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(121, 14);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(246, 21);
            this.cbWorkingUnit.TabIndex = 10;
            this.cbWorkingUnit.SelectedIndexChanged += new System.EventHandler(this.cbWorkingUnit_SelectedIndexChanged);
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(9, 14);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(104, 23);
            this.lblWorkingUnit.TabIndex = 11;
            this.lblWorkingUnit.Text = "Working Unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(10, 53);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(104, 23);
            this.lblEmployee.TabIndex = 15;
            this.lblEmployee.Text = "First Name:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbEmployee
            // 
            this.cbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmployee.Location = new System.Drawing.Point(121, 53);
            this.cbEmployee.Name = "cbEmployee";
            this.cbEmployee.Size = new System.Drawing.Size(246, 21);
            this.cbEmployee.TabIndex = 14;
            this.cbEmployee.SelectedIndexChanged += new System.EventHandler(this.cbEmployee_SelectedIndexChanged);
            // 
            // btnByMeals
            // 
            this.btnByMeals.Location = new System.Drawing.Point(3, 80);
            this.btnByMeals.Name = "btnByMeals";
            this.btnByMeals.Size = new System.Drawing.Size(184, 40);
            this.btnByMeals.TabIndex = 18;
            this.btnByMeals.Text = "By meals";
            this.btnByMeals.UseVisualStyleBackColor = true;
            this.btnByMeals.Click += new System.EventHandler(this.btnByMeals_Click);
            // 
            // btnByDays
            // 
            this.btnByDays.Location = new System.Drawing.Point(199, 80);
            this.btnByDays.Name = "btnByDays";
            this.btnByDays.Size = new System.Drawing.Size(203, 41);
            this.btnByDays.TabIndex = 19;
            this.btnByDays.Text = "By days";
            this.btnByDays.UseVisualStyleBackColor = true;
            this.btnByDays.Click += new System.EventHandler(this.btnByDays_Click);
            // 
            // cbOverrideSch
            // 
            this.cbOverrideSch.Checked = true;
            this.cbOverrideSch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOverrideSch.Location = new System.Drawing.Point(373, 53);
            this.cbOverrideSch.Name = "cbOverrideSch";
            this.cbOverrideSch.Size = new System.Drawing.Size(239, 24);
            this.cbOverrideSch.TabIndex = 20;
            this.cbOverrideSch.Text = "Override schedules";
            // 
            // panelMeals
            // 
            this.panelMeals.AutoScroll = true;
            this.panelMeals.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelMeals.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelMeals.Controls.Add(this.lblMeals);
            this.panelMeals.Location = new System.Drawing.Point(3, 129);
            this.panelMeals.Name = "panelMeals";
            this.panelMeals.Size = new System.Drawing.Size(184, 456);
            this.panelMeals.TabIndex = 0;
            // 
            // lblMeals
            // 
            this.lblMeals.Location = new System.Drawing.Point(1, 0);
            this.lblMeals.Name = "lblMeals";
            this.lblMeals.Size = new System.Drawing.Size(179, 23);
            this.lblMeals.TabIndex = 0;
            this.lblMeals.Text = "Meals";
            this.lblMeals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbNote
            // 
            this.tbNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNote.Location = new System.Drawing.Point(0, 0);
            this.tbNote.Name = "tbNote";
            this.tbNote.Size = new System.Drawing.Size(963, 44);
            this.tbNote.TabIndex = 21;
            this.tbNote.Visible = false;
            // 
            // gbLegend
            // 
            this.gbLegend.Controls.Add(this.lblDaysWithMeals);
            this.gbLegend.Controls.Add(this.panel6);
            this.gbLegend.Controls.Add(this.lblMealAvaiable);
            this.gbLegend.Controls.Add(this.lblNotWorkingDays);
            this.gbLegend.Controls.Add(this.lblWorkingDays);
            this.gbLegend.Controls.Add(this.lblDiseabled);
            this.gbLegend.Controls.Add(this.panel5);
            this.gbLegend.Controls.Add(this.panel4);
            this.gbLegend.Controls.Add(this.panel3);
            this.gbLegend.Controls.Add(this.panel2);
            this.gbLegend.Location = new System.Drawing.Point(7, 586);
            this.gbLegend.Name = "gbLegend";
            this.gbLegend.Size = new System.Drawing.Size(908, 49);
            this.gbLegend.TabIndex = 22;
            this.gbLegend.TabStop = false;
            this.gbLegend.Text = "Legend";
            // 
            // lblDaysWithMeals
            // 
            this.lblDaysWithMeals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDaysWithMeals.Location = new System.Drawing.Point(688, 17);
            this.lblDaysWithMeals.Name = "lblDaysWithMeals";
            this.lblDaysWithMeals.Size = new System.Drawing.Size(150, 26);
            this.lblDaysWithMeals.TabIndex = 9;
            this.lblDaysWithMeals.Text = "- Meal avaiable days";
            this.lblDaysWithMeals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Gray;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Location = new System.Drawing.Point(653, 19);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(29, 20);
            this.panel6.TabIndex = 8;
            // 
            // lblMealAvaiable
            // 
            this.lblMealAvaiable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMealAvaiable.Location = new System.Drawing.Point(499, 17);
            this.lblMealAvaiable.Name = "lblMealAvaiable";
            this.lblMealAvaiable.Size = new System.Drawing.Size(150, 26);
            this.lblMealAvaiable.TabIndex = 7;
            this.lblMealAvaiable.Text = "- Meal avaiable days";
            this.lblMealAvaiable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNotWorkingDays
            // 
            this.lblNotWorkingDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotWorkingDays.Location = new System.Drawing.Point(349, 17);
            this.lblNotWorkingDays.Name = "lblNotWorkingDays";
            this.lblNotWorkingDays.Size = new System.Drawing.Size(109, 26);
            this.lblNotWorkingDays.TabIndex = 6;
            this.lblNotWorkingDays.Text = "- Holidays and not working days";
            this.lblNotWorkingDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWorkingDays
            // 
            this.lblWorkingDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkingDays.Location = new System.Drawing.Point(189, 19);
            this.lblWorkingDays.Name = "lblWorkingDays";
            this.lblWorkingDays.Size = new System.Drawing.Size(119, 23);
            this.lblWorkingDays.TabIndex = 5;
            this.lblWorkingDays.Text = "- Working days";
            this.lblWorkingDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiseabled
            // 
            this.lblDiseabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiseabled.Location = new System.Drawing.Point(51, 19);
            this.lblDiseabled.Name = "lblDiseabled";
            this.lblDiseabled.Size = new System.Drawing.Size(97, 23);
            this.lblDiseabled.TabIndex = 4;
            this.lblDiseabled.Text = "- Past days";
            this.lblDiseabled.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Location = new System.Drawing.Point(464, 19);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(29, 20);
            this.panel5.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Pink;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(314, 19);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(29, 20);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Yellow;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(154, 19);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(29, 20);
            this.panel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(16, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(29, 18);
            this.panel2.TabIndex = 0;
            // 
            // MealsWorkingUnitSchedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbLegend);
            this.Controls.Add(this.tbNote);
            this.Controls.Add(this.panelMeals);
            this.Controls.Add(this.cbOverrideSch);
            this.Controls.Add(this.btnByDays);
            this.Controls.Add(this.btnByMeals);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.cbEmployee);
            this.Controls.Add(this.chbHierarhicly);
            this.Controls.Add(this.btnWUTree);
            this.Controls.Add(this.cbWorkingUnit);
            this.Controls.Add(this.lblWorkingUnit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.dtpMonth);
            this.Name = "MealsWorkingUnitSchedules";
            this.Size = new System.Drawing.Size(963, 639);
            this.Load += new System.EventHandler(this.MealsWorkingUnitSchedules_Load);
            this.panelMeals.ResumeLayout(false);
            this.gbLegend.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.DateTimePicker dtpMonth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chbHierarhicly;
        private System.Windows.Forms.Button btnWUTree;
        private System.Windows.Forms.ComboBox cbWorkingUnit;
        private System.Windows.Forms.Label lblWorkingUnit;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cbEmployee;
        private System.Windows.Forms.Button btnByMeals;
        private System.Windows.Forms.Button btnByDays;
        private System.Windows.Forms.CheckBox cbOverrideSch;
        private System.Windows.Forms.Panel panelMeals;
        private System.Windows.Forms.Label lblMeals;
        private System.Windows.Forms.RichTextBox tbNote;
        private System.Windows.Forms.GroupBox gbLegend;
        private System.Windows.Forms.Label lblMealAvaiable;
        private System.Windows.Forms.Label lblNotWorkingDays;
        private System.Windows.Forms.Label lblWorkingDays;
        private System.Windows.Forms.Label lblDiseabled;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblDaysWithMeals;
        private System.Windows.Forms.Panel panel6;
    }
}
