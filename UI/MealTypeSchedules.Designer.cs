namespace UI
{
    partial class MealTypeSchedules
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
            this.gbMealData = new System.Windows.Forms.GroupBox();
            this.cbMeal = new System.Windows.Forms.ComboBox();
            this.lblMeal = new System.Windows.Forms.Label();
            this.gbGroupChange = new System.Windows.Forms.GroupBox();
            this.btnChange = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbLegend = new System.Windows.Forms.GroupBox();
            this.lblSelectedDays = new System.Windows.Forms.Label();
            this.lblHolidays = new System.Windows.Forms.Label();
            this.lblChangableDays = new System.Windows.Forms.Label();
            this.lblDiseabled = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnMonthlyMenu = new System.Windows.Forms.Button();
            this.gbMealData.SuspendLayout();
            this.gbGroupChange.SuspendLayout();
            this.gbLegend.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMealData
            // 
            this.gbMealData.Controls.Add(this.cbMeal);
            this.gbMealData.Controls.Add(this.lblMeal);
            this.gbMealData.Location = new System.Drawing.Point(13, 12);
            this.gbMealData.Name = "gbMealData";
            this.gbMealData.Size = new System.Drawing.Size(240, 74);
            this.gbMealData.TabIndex = 0;
            this.gbMealData.TabStop = false;
            this.gbMealData.Text = "Meal data";
            // 
            // cbMeal
            // 
            this.cbMeal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMeal.Location = new System.Drawing.Point(60, 31);
            this.cbMeal.Name = "cbMeal";
            this.cbMeal.Size = new System.Drawing.Size(171, 21);
            this.cbMeal.TabIndex = 13;
            this.cbMeal.SelectedIndexChanged += new System.EventHandler(this.cbMeal_SelectedIndexChanged);
            // 
            // lblMeal
            // 
            this.lblMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMeal.Location = new System.Drawing.Point(12, 29);
            this.lblMeal.Name = "lblMeal";
            this.lblMeal.Size = new System.Drawing.Size(42, 23);
            this.lblMeal.TabIndex = 3;
            this.lblMeal.Text = "Meal:";
            this.lblMeal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbGroupChange
            // 
            this.gbGroupChange.Controls.Add(this.btnChange);
            this.gbGroupChange.Controls.Add(this.dtpTo);
            this.gbGroupChange.Controls.Add(this.lblTo);
            this.gbGroupChange.Controls.Add(this.dtpFrom);
            this.gbGroupChange.Controls.Add(this.lblFrom);
            this.gbGroupChange.Location = new System.Drawing.Point(575, 12);
            this.gbGroupChange.Name = "gbGroupChange";
            this.gbGroupChange.Size = new System.Drawing.Size(227, 74);
            this.gbGroupChange.TabIndex = 1;
            this.gbGroupChange.TabStop = false;
            this.gbGroupChange.Text = "Group change";
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(132, 40);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 10;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(55, 43);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowUpDown = true;
            this.dtpTo.Size = new System.Drawing.Size(59, 20);
            this.dtpTo.TabIndex = 9;
            // 
            // lblTo
            // 
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(7, 42);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(42, 23);
            this.lblTo.TabIndex = 8;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(55, 17);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowUpDown = true;
            this.dtpFrom.Size = new System.Drawing.Size(59, 20);
            this.dtpFrom.TabIndex = 7;
            // 
            // lblFrom
            // 
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(7, 16);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(42, 23);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(3, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(821, 455);
            this.panel1.TabIndex = 2;
            // 
            // dtpMonth
            // 
            this.dtpMonth.CustomFormat = "MMMM yyyy";
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(355, 29);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.Size = new System.Drawing.Size(120, 20);
            this.dtpMonth.TabIndex = 3;
            this.dtpMonth.ValueChanged += new System.EventHandler(this.dtpMonth_ValueChanged);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(316, 28);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(33, 22);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.Text = "<-";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(481, 28);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(33, 22);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "->";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(749, 564);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbLegend
            // 
            this.gbLegend.Controls.Add(this.lblSelectedDays);
            this.gbLegend.Controls.Add(this.lblHolidays);
            this.gbLegend.Controls.Add(this.lblChangableDays);
            this.gbLegend.Controls.Add(this.lblDiseabled);
            this.gbLegend.Controls.Add(this.panel5);
            this.gbLegend.Controls.Add(this.panel4);
            this.gbLegend.Controls.Add(this.panel3);
            this.gbLegend.Controls.Add(this.panel2);
            this.gbLegend.Location = new System.Drawing.Point(3, 548);
            this.gbLegend.Name = "gbLegend";
            this.gbLegend.Size = new System.Drawing.Size(686, 49);
            this.gbLegend.TabIndex = 7;
            this.gbLegend.TabStop = false;
            this.gbLegend.Text = "Legend";
            // 
            // lblSelectedDays
            // 
            this.lblSelectedDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedDays.Location = new System.Drawing.Point(499, 17);
            this.lblSelectedDays.Name = "lblSelectedDays";
            this.lblSelectedDays.Size = new System.Drawing.Size(150, 26);
            this.lblSelectedDays.TabIndex = 7;
            this.lblSelectedDays.Text = "- Selected days";
            this.lblSelectedDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHolidays
            // 
            this.lblHolidays.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHolidays.Location = new System.Drawing.Point(349, 16);
            this.lblHolidays.Name = "lblHolidays";
            this.lblHolidays.Size = new System.Drawing.Size(109, 26);
            this.lblHolidays.TabIndex = 6;
            this.lblHolidays.Text = "- Holidays";
            this.lblHolidays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblChangableDays
            // 
            this.lblChangableDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangableDays.Location = new System.Drawing.Point(189, 19);
            this.lblChangableDays.Name = "lblChangableDays";
            this.lblChangableDays.Size = new System.Drawing.Size(119, 23);
            this.lblChangableDays.TabIndex = 5;
            this.lblChangableDays.Text = "- Days can change";
            this.lblChangableDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.panel5.BackColor = System.Drawing.Color.Blue;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Location = new System.Drawing.Point(464, 19);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(29, 20);
            this.panel5.TabIndex = 3;
            this.panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.panel5_Paint);
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
            this.panel3.BackColor = System.Drawing.Color.LightBlue;
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
            // btnMonthlyMenu
            // 
            this.btnMonthlyMenu.Location = new System.Drawing.Point(355, 56);
            this.btnMonthlyMenu.Name = "btnMonthlyMenu";
            this.btnMonthlyMenu.Size = new System.Drawing.Size(120, 23);
            this.btnMonthlyMenu.TabIndex = 8;
            this.btnMonthlyMenu.Text = "View monthly menu";
            this.btnMonthlyMenu.UseVisualStyleBackColor = true;
            this.btnMonthlyMenu.Click += new System.EventHandler(this.btnViewMonthSchedule_Click);
            // 
            // MealTypeSchedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnMonthlyMenu);
            this.Controls.Add(this.gbLegend);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.dtpMonth);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbGroupChange);
            this.Controls.Add(this.gbMealData);
            this.Name = "MealTypeSchedules";
            this.Size = new System.Drawing.Size(964, 601);
            this.Load += new System.EventHandler(this.MealTypeSchedules_Load);
            this.gbMealData.ResumeLayout(false);
            this.gbGroupChange.ResumeLayout(false);
            this.gbLegend.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMealData;
        private System.Windows.Forms.Label lblMeal;
        private System.Windows.Forms.ComboBox cbMeal;
        private System.Windows.Forms.GroupBox gbGroupChange;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpMonth;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbLegend;
        private System.Windows.Forms.Label lblHolidays;
        private System.Windows.Forms.Label lblChangableDays;
        private System.Windows.Forms.Label lblDiseabled;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblSelectedDays;
        private System.Windows.Forms.Button btnMonthlyMenu;
    }
}
