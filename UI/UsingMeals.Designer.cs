namespace UI
{
    partial class UsingMeals
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
            this.btnClose = new System.Windows.Forms.Button();
            this.gbEmpl = new System.Windows.Forms.GroupBox();
            this.pbEmplPhoto = new System.Windows.Forms.PictureBox();
            this.tbEmpl = new System.Windows.Forms.TextBox();
            this.tbType = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblEmpl = new System.Windows.Forms.Label();
            this.gbMeals = new System.Windows.Forms.GroupBox();
            this.gbMealsII = new System.Windows.Forms.GroupBox();
            this.lblToII = new System.Windows.Forms.Label();
            this.lblFromII = new System.Windows.Forms.Label();
            this.dtpToII = new System.Windows.Forms.DateTimePicker();
            this.dtpFromII = new System.Windows.Forms.DateTimePicker();
            this.tbMealTypeII = new System.Windows.Forms.TextBox();
            this.lblIntervalII = new System.Windows.Forms.Label();
            this.lblMealTypeII = new System.Windows.Forms.Label();
            this.lblMealPrice = new System.Windows.Forms.Label();
            this.tbPrice = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblUnlimitedTo = new System.Windows.Forms.Label();
            this.lblUnlimitedFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.tbUsed = new System.Windows.Forms.TextBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.tbAvailable = new System.Windows.Forms.TextBox();
            this.lblMealUsedCount = new System.Windows.Forms.Label();
            this.lblMealCount = new System.Windows.Forms.Label();
            this.lblUsed = new System.Windows.Forms.Label();
            this.lblInterval = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.btnUse = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFailed = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tbQty = new System.Windows.Forms.TextBox();
            this.tbMealType = new System.Windows.Forms.TextBox();
            this.lblMeals = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.lblMealType = new System.Windows.Forms.Label();
            this.gbEmpl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmplPhoto)).BeginInit();
            this.gbMeals.SuspendLayout();
            this.gbMealsII.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(205, 427);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbEmpl
            // 
            this.gbEmpl.Controls.Add(this.pbEmplPhoto);
            this.gbEmpl.Controls.Add(this.tbEmpl);
            this.gbEmpl.Controls.Add(this.tbType);
            this.gbEmpl.Controls.Add(this.lblType);
            this.gbEmpl.Controls.Add(this.lblEmpl);
            this.gbEmpl.Location = new System.Drawing.Point(12, 12);
            this.gbEmpl.Name = "gbEmpl";
            this.gbEmpl.Size = new System.Drawing.Size(268, 222);
            this.gbEmpl.TabIndex = 0;
            this.gbEmpl.TabStop = false;
            this.gbEmpl.Text = "Employee";
            // 
            // pbEmplPhoto
            // 
            this.pbEmplPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbEmplPhoto.Location = new System.Drawing.Point(95, 17);
            this.pbEmplPhoto.Name = "pbEmplPhoto";
            this.pbEmplPhoto.Size = new System.Drawing.Size(90, 135);
            this.pbEmplPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEmplPhoto.TabIndex = 4;
            this.pbEmplPhoto.TabStop = false;
            // 
            // tbEmpl
            // 
            this.tbEmpl.BackColor = System.Drawing.SystemColors.Info;
            this.tbEmpl.Location = new System.Drawing.Point(108, 165);
            this.tbEmpl.Name = "tbEmpl";
            this.tbEmpl.ReadOnly = true;
            this.tbEmpl.Size = new System.Drawing.Size(144, 20);
            this.tbEmpl.TabIndex = 2;
            this.tbEmpl.TabStop = false;
            // 
            // tbType
            // 
            this.tbType.BackColor = System.Drawing.SystemColors.Info;
            this.tbType.Location = new System.Drawing.Point(108, 192);
            this.tbType.Name = "tbType";
            this.tbType.ReadOnly = true;
            this.tbType.Size = new System.Drawing.Size(144, 20);
            this.tbType.TabIndex = 4;
            this.tbType.TabStop = false;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(17, 191);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(85, 21);
            this.lblType.TabIndex = 3;
            this.lblType.Text = "Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmpl
            // 
            this.lblEmpl.Location = new System.Drawing.Point(20, 166);
            this.lblEmpl.Name = "lblEmpl";
            this.lblEmpl.Size = new System.Drawing.Size(82, 17);
            this.lblEmpl.TabIndex = 1;
            this.lblEmpl.Text = "Employee:";
            this.lblEmpl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbMeals
            // 
            this.gbMeals.Controls.Add(this.lblMealPrice);
            this.gbMeals.Controls.Add(this.tbPrice);
            this.gbMeals.Controls.Add(this.lblPrice);
            this.gbMeals.Controls.Add(this.lblUnlimitedTo);
            this.gbMeals.Controls.Add(this.lblUnlimitedFrom);
            this.gbMeals.Controls.Add(this.lblTo);
            this.gbMeals.Controls.Add(this.lblFrom);
            this.gbMeals.Controls.Add(this.tbUsed);
            this.gbMeals.Controls.Add(this.dtpTo);
            this.gbMeals.Controls.Add(this.dtpFrom);
            this.gbMeals.Controls.Add(this.tbAvailable);
            this.gbMeals.Controls.Add(this.lblMealUsedCount);
            this.gbMeals.Controls.Add(this.lblMealCount);
            this.gbMeals.Controls.Add(this.lblUsed);
            this.gbMeals.Controls.Add(this.lblInterval);
            this.gbMeals.Controls.Add(this.lblAvailable);
            this.gbMeals.Location = new System.Drawing.Point(12, 240);
            this.gbMeals.Name = "gbMeals";
            this.gbMeals.Size = new System.Drawing.Size(268, 176);
            this.gbMeals.TabIndex = 5;
            this.gbMeals.TabStop = false;
            this.gbMeals.Text = "Meals";
            // 
            // gbMealsII
            // 
            this.gbMealsII.Controls.Add(this.lblToII);
            this.gbMealsII.Controls.Add(this.lblFromII);
            this.gbMealsII.Controls.Add(this.dtpToII);
            this.gbMealsII.Controls.Add(this.dtpFromII);
            this.gbMealsII.Controls.Add(this.tbMealTypeII);
            this.gbMealsII.Controls.Add(this.lblIntervalII);
            this.gbMealsII.Controls.Add(this.lblMealTypeII);
            this.gbMealsII.Location = new System.Drawing.Point(12, 240);
            this.gbMealsII.Name = "gbMealsII";
            this.gbMealsII.Size = new System.Drawing.Size(268, 176);
            this.gbMealsII.TabIndex = 24;
            this.gbMealsII.TabStop = false;
            this.gbMealsII.Text = "Meals";
            // 
            // lblToII
            // 
            this.lblToII.Location = new System.Drawing.Point(96, 122);
            this.lblToII.Name = "lblToII";
            this.lblToII.Size = new System.Drawing.Size(33, 13);
            this.lblToII.TabIndex = 12;
            this.lblToII.Text = "To:";
            this.lblToII.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromII
            // 
            this.lblFromII.Location = new System.Drawing.Point(93, 88);
            this.lblFromII.Name = "lblFromII";
            this.lblFromII.Size = new System.Drawing.Size(36, 23);
            this.lblFromII.TabIndex = 10;
            this.lblFromII.Text = "From:";
            this.lblFromII.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToII
            // 
            this.dtpToII.CalendarMonthBackground = System.Drawing.SystemColors.Info;
            this.dtpToII.CustomFormat = "HH:mm";
            this.dtpToII.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToII.Location = new System.Drawing.Point(132, 118);
            this.dtpToII.Name = "dtpToII";
            this.dtpToII.Size = new System.Drawing.Size(115, 20);
            this.dtpToII.TabIndex = 13;
            this.dtpToII.TabStop = false;
            // 
            // dtpFromII
            // 
            this.dtpFromII.CalendarMonthBackground = System.Drawing.SystemColors.Info;
            this.dtpFromII.CustomFormat = "HH:mm";
            this.dtpFromII.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromII.Location = new System.Drawing.Point(132, 89);
            this.dtpFromII.Name = "dtpFromII";
            this.dtpFromII.Size = new System.Drawing.Size(115, 20);
            this.dtpFromII.TabIndex = 11;
            this.dtpFromII.TabStop = false;
            // 
            // tbMealTypeII
            // 
            this.tbMealTypeII.BackColor = System.Drawing.SystemColors.Info;
            this.tbMealTypeII.Location = new System.Drawing.Point(92, 25);
            this.tbMealTypeII.Name = "tbMealTypeII";
            this.tbMealTypeII.ReadOnly = true;
            this.tbMealTypeII.Size = new System.Drawing.Size(155, 20);
            this.tbMealTypeII.TabIndex = 7;
            this.tbMealTypeII.TabStop = false;
            this.tbMealTypeII.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblIntervalII
            // 
            this.lblIntervalII.Location = new System.Drawing.Point(6, 90);
            this.lblIntervalII.Name = "lblIntervalII";
            this.lblIntervalII.Size = new System.Drawing.Size(80, 18);
            this.lblIntervalII.TabIndex = 9;
            this.lblIntervalII.Text = "Interval:";
            this.lblIntervalII.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMealTypeII
            // 
            this.lblMealTypeII.Location = new System.Drawing.Point(6, 25);
            this.lblMealTypeII.Name = "lblMealTypeII";
            this.lblMealTypeII.Size = new System.Drawing.Size(80, 18);
            this.lblMealTypeII.TabIndex = 6;
            this.lblMealTypeII.Text = "Meal type:";
            this.lblMealTypeII.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMealPrice
            // 
            this.lblMealPrice.AutoSize = true;
            this.lblMealPrice.Location = new System.Drawing.Point(213, 145);
            this.lblMealPrice.Name = "lblMealPrice";
            this.lblMealPrice.Size = new System.Drawing.Size(34, 13);
            this.lblMealPrice.TabIndex = 19;
            this.lblMealPrice.Text = "meals";
            this.lblMealPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPrice
            // 
            this.tbPrice.Location = new System.Drawing.Point(132, 142);
            this.tbPrice.Name = "tbPrice";
            this.tbPrice.Size = new System.Drawing.Size(75, 20);
            this.tbPrice.TabIndex = 18;
            this.tbPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPrice
            // 
            this.lblPrice.Location = new System.Drawing.Point(20, 140);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(109, 23);
            this.lblPrice.TabIndex = 17;
            this.lblPrice.Text = "Meal price:";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUnlimitedTo
            // 
            this.lblUnlimitedTo.AutoSize = true;
            this.lblUnlimitedTo.Location = new System.Drawing.Point(129, 82);
            this.lblUnlimitedTo.Name = "lblUnlimitedTo";
            this.lblUnlimitedTo.Size = new System.Drawing.Size(50, 13);
            this.lblUnlimitedTo.TabIndex = 30;
            this.lblUnlimitedTo.Text = "Unlimited";
            this.lblUnlimitedTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUnlimitedFrom
            // 
            this.lblUnlimitedFrom.AutoSize = true;
            this.lblUnlimitedFrom.Location = new System.Drawing.Point(129, 53);
            this.lblUnlimitedFrom.Name = "lblUnlimitedFrom";
            this.lblUnlimitedFrom.Size = new System.Drawing.Size(50, 13);
            this.lblUnlimitedFrom.TabIndex = 29;
            this.lblUnlimitedFrom.Text = "Unlimited";
            this.lblUnlimitedFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(96, 84);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(33, 13);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(93, 50);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(36, 23);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbUsed
            // 
            this.tbUsed.BackColor = System.Drawing.SystemColors.Info;
            this.tbUsed.Location = new System.Drawing.Point(92, 106);
            this.tbUsed.Name = "tbUsed";
            this.tbUsed.ReadOnly = true;
            this.tbUsed.Size = new System.Drawing.Size(115, 20);
            this.tbUsed.TabIndex = 15;
            this.tbUsed.TabStop = false;
            this.tbUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarMonthBackground = System.Drawing.SystemColors.Info;
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(132, 80);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(115, 20);
            this.dtpTo.TabIndex = 13;
            this.dtpTo.TabStop = false;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarMonthBackground = System.Drawing.SystemColors.Info;
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(132, 51);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(115, 20);
            this.dtpFrom.TabIndex = 11;
            this.dtpFrom.TabStop = false;
            // 
            // tbAvailable
            // 
            this.tbAvailable.BackColor = System.Drawing.SystemColors.Info;
            this.tbAvailable.Location = new System.Drawing.Point(92, 25);
            this.tbAvailable.Name = "tbAvailable";
            this.tbAvailable.ReadOnly = true;
            this.tbAvailable.Size = new System.Drawing.Size(115, 20);
            this.tbAvailable.TabIndex = 7;
            this.tbAvailable.TabStop = false;
            this.tbAvailable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMealUsedCount
            // 
            this.lblMealUsedCount.AutoSize = true;
            this.lblMealUsedCount.Location = new System.Drawing.Point(213, 109);
            this.lblMealUsedCount.Name = "lblMealUsedCount";
            this.lblMealUsedCount.Size = new System.Drawing.Size(34, 13);
            this.lblMealUsedCount.TabIndex = 16;
            this.lblMealUsedCount.Text = "meals";
            this.lblMealUsedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMealCount
            // 
            this.lblMealCount.AutoSize = true;
            this.lblMealCount.Location = new System.Drawing.Point(213, 28);
            this.lblMealCount.Name = "lblMealCount";
            this.lblMealCount.Size = new System.Drawing.Size(34, 13);
            this.lblMealCount.TabIndex = 8;
            this.lblMealCount.Text = "meals";
            this.lblMealCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsed
            // 
            this.lblUsed.Location = new System.Drawing.Point(19, 106);
            this.lblUsed.Name = "lblUsed";
            this.lblUsed.Size = new System.Drawing.Size(67, 18);
            this.lblUsed.TabIndex = 14;
            this.lblUsed.Text = "Used:";
            this.lblUsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInterval
            // 
            this.lblInterval.Location = new System.Drawing.Point(6, 52);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(80, 18);
            this.lblInterval.TabIndex = 9;
            this.lblInterval.Text = "Interval:";
            this.lblInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAvailable
            // 
            this.lblAvailable.Location = new System.Drawing.Point(6, 25);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(80, 18);
            this.lblAvailable.TabIndex = 6;
            this.lblAvailable.Text = "Available:";
            this.lblAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUse
            // 
            this.btnUse.Location = new System.Drawing.Point(12, 427);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(75, 23);
            this.btnUse.TabIndex = 20;
            this.btnUse.Text = "Use";
            this.btnUse.UseVisualStyleBackColor = true;
            this.btnUse.Click += new System.EventHandler(this.btnUse_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(108, 427);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 21;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // gbStatus
            // 
            this.gbStatus.BackColor = System.Drawing.SystemColors.Control;
            this.gbStatus.Controls.Add(this.panel1);
            this.gbStatus.Location = new System.Drawing.Point(12, 459);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(268, 122);
            this.gbStatus.TabIndex = 23;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Status";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.lblFailed);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.tbQty);
            this.panel1.Controls.Add(this.tbMealType);
            this.panel1.Controls.Add(this.lblMeals);
            this.panel1.Controls.Add(this.lblQty);
            this.panel1.Controls.Add(this.lblMealType);
            this.panel1.Location = new System.Drawing.Point(8, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 100);
            this.panel1.TabIndex = 24;
            // 
            // lblFailed
            // 
            this.lblFailed.Location = new System.Drawing.Point(6, 33);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(244, 53);
            this.lblFailed.TabIndex = 26;
            this.lblFailed.Text = "Used service failed.";
            this.lblFailed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(6, 11);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(115, 13);
            this.lblStatus.TabIndex = 25;
            this.lblStatus.Text = "Status of used service.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbQty
            // 
            this.tbQty.BackColor = System.Drawing.SystemColors.Info;
            this.tbQty.Location = new System.Drawing.Point(92, 65);
            this.tbQty.Name = "tbQty";
            this.tbQty.ReadOnly = true;
            this.tbQty.Size = new System.Drawing.Size(115, 20);
            this.tbQty.TabIndex = 30;
            this.tbQty.TabStop = false;
            this.tbQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbMealType
            // 
            this.tbMealType.BackColor = System.Drawing.SystemColors.Info;
            this.tbMealType.Location = new System.Drawing.Point(92, 33);
            this.tbMealType.Name = "tbMealType";
            this.tbMealType.ReadOnly = true;
            this.tbMealType.Size = new System.Drawing.Size(115, 20);
            this.tbMealType.TabIndex = 28;
            this.tbMealType.TabStop = false;
            // 
            // lblMeals
            // 
            this.lblMeals.AutoSize = true;
            this.lblMeals.Location = new System.Drawing.Point(213, 68);
            this.lblMeals.Name = "lblMeals";
            this.lblMeals.Size = new System.Drawing.Size(34, 13);
            this.lblMeals.TabIndex = 31;
            this.lblMeals.Text = "meals";
            this.lblMeals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(19, 65);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(67, 18);
            this.lblQty.TabIndex = 29;
            this.lblQty.Text = "Quantity:";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMealType
            // 
            this.lblMealType.Location = new System.Drawing.Point(6, 33);
            this.lblMealType.Name = "lblMealType";
            this.lblMealType.Size = new System.Drawing.Size(80, 18);
            this.lblMealType.TabIndex = 27;
            this.lblMealType.Text = "Meal type:";
            this.lblMealType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UsingMeals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 586);
            this.ControlBox = false;
            this.Controls.Add(this.gbMealsII);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnUse);
            this.Controls.Add(this.gbMeals);
            this.Controls.Add(this.gbEmpl);
            this.Controls.Add(this.btnClose);
            this.MaximumSize = new System.Drawing.Size(300, 620);
            this.MinimumSize = new System.Drawing.Size(300, 620);
            this.Name = "UsingMeals";
            this.ShowInTaskbar = false;
            this.Text = "Using meals";
            this.Load += new System.EventHandler(this.UsingMeals_Load);
            this.Closed += new System.EventHandler(this.UsingMeals_Closed);
            this.gbEmpl.ResumeLayout(false);
            this.gbEmpl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmplPhoto)).EndInit();
            this.gbMeals.ResumeLayout(false);
            this.gbMeals.PerformLayout();
            this.gbMealsII.ResumeLayout(false);
            this.gbMealsII.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbEmpl;
        private System.Windows.Forms.TextBox tbEmpl;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblEmpl;
        private System.Windows.Forms.PictureBox pbEmplPhoto;
        private System.Windows.Forms.GroupBox gbMeals;
        private System.Windows.Forms.Label lblUsed;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.TextBox tbAvailable;
        private System.Windows.Forms.Label lblMealUsedCount;
        private System.Windows.Forms.Label lblMealCount;
        private System.Windows.Forms.TextBox tbUsed;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Button btnUse;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox tbQty;
        private System.Windows.Forms.TextBox tbMealType;
        private System.Windows.Forms.Label lblMeals;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label lblMealType;
        private System.Windows.Forms.Label lblUnlimitedTo;
        private System.Windows.Forms.Label lblUnlimitedFrom;
        private System.Windows.Forms.Label lblMealPrice;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.TextBox tbPrice;
        private System.Windows.Forms.GroupBox gbMealsII;
        private System.Windows.Forms.Label lblToII;
        private System.Windows.Forms.Label lblFromII;
        private System.Windows.Forms.DateTimePicker dtpToII;
        private System.Windows.Forms.DateTimePicker dtpFromII;
        private System.Windows.Forms.TextBox tbMealTypeII;
        private System.Windows.Forms.Label lblIntervalII;
        private System.Windows.Forms.Label lblMealTypeII;
    }
}