namespace UI
{
    partial class RestaurantII
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpMealType = new System.Windows.Forms.TabPage();
            this.mealTypesII1 = new UI.MealTypesII();
            this.tpMealsSchedule = new System.Windows.Forms.TabPage();
            this.mealTypeSchedules1 = new UI.MealTypeSchedules();
            this.tpSetingSchedule = new System.Windows.Forms.TabPage();
            this.mealsWorkingUnitSchedules1 = new UI.MealsWorkingUnitSchedules();
            this.tpReports = new System.Windows.Forms.TabPage();
            this.mealEmployeeSchedulePreview1 = new UI.MealEmployeeSchedulePreview();
            this.tpMealPoints = new System.Windows.Forms.TabPage();
            this.mealPointsII1 = new UI.MealPointsII();
            this.tpOrder = new System.Windows.Forms.TabPage();
            this.mealOrderControl1 = new UI.MealOrderControl();
            this.rptDataSet1 = new DataAccess.RptDataSet();
            this.tabControl.SuspendLayout();
            this.tpMealType.SuspendLayout();
            this.tpMealsSchedule.SuspendLayout();
            this.tpSetingSchedule.SuspendLayout();
            this.tpReports.SuspendLayout();
            this.tpMealPoints.SuspendLayout();
            this.tpOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rptDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(896, 696);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpMealType);
            this.tabControl.Controls.Add(this.tpMealsSchedule);
            this.tabControl.Controls.Add(this.tpSetingSchedule);
            this.tabControl.Controls.Add(this.tpReports);
            this.tabControl.Controls.Add(this.tpMealPoints);
            this.tabControl.Controls.Add(this.tpOrder);
            this.tabControl.Location = new System.Drawing.Point(4, 7);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(979, 682);
            this.tabControl.TabIndex = 3;
            // 
            // tpMealType
            // 
            this.tpMealType.Controls.Add(this.mealTypesII1);
            this.tpMealType.Location = new System.Drawing.Point(4, 22);
            this.tpMealType.Name = "tpMealType";
            this.tpMealType.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealType.Size = new System.Drawing.Size(971, 634);
            this.tpMealType.TabIndex = 0;
            this.tpMealType.Text = "Meal type";
            this.tpMealType.UseVisualStyleBackColor = true;
            this.tpMealType.Click += new System.EventHandler(this.tpMealType_Click);
            // 
            // mealTypesII1
            // 
            this.mealTypesII1.Location = new System.Drawing.Point(6, 6);
            this.mealTypesII1.Name = "mealTypesII1";
            this.mealTypesII1.Size = new System.Drawing.Size(638, 355);
            this.mealTypesII1.TabIndex = 0;
            // 
            // tpMealsSchedule
            // 
            this.tpMealsSchedule.Controls.Add(this.mealTypeSchedules1);
            this.tpMealsSchedule.Location = new System.Drawing.Point(4, 22);
            this.tpMealsSchedule.Name = "tpMealsSchedule";
            this.tpMealsSchedule.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealsSchedule.Size = new System.Drawing.Size(971, 634);
            this.tpMealsSchedule.TabIndex = 1;
            this.tpMealsSchedule.Text = "Meals schedule";
            this.tpMealsSchedule.UseVisualStyleBackColor = true;
            // 
            // mealTypeSchedules1
            // 
            this.mealTypeSchedules1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealTypeSchedules1.Location = new System.Drawing.Point(0, 0);
            this.mealTypeSchedules1.Name = "mealTypeSchedules1";
            this.mealTypeSchedules1.Size = new System.Drawing.Size(949, 605);
            this.mealTypeSchedules1.TabIndex = 0;
            // 
            // tpSetingSchedule
            // 
            this.tpSetingSchedule.Controls.Add(this.mealsWorkingUnitSchedules1);
            this.tpSetingSchedule.Location = new System.Drawing.Point(4, 22);
            this.tpSetingSchedule.Name = "tpSetingSchedule";
            this.tpSetingSchedule.Padding = new System.Windows.Forms.Padding(3);
            this.tpSetingSchedule.Size = new System.Drawing.Size(971, 656);
            this.tpSetingSchedule.TabIndex = 2;
            this.tpSetingSchedule.Text = "Meal points";
            this.tpSetingSchedule.UseVisualStyleBackColor = true;
            // 
            // mealsWorkingUnitSchedules1
            // 
            this.mealsWorkingUnitSchedules1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealsWorkingUnitSchedules1.Location = new System.Drawing.Point(0, 3);
            this.mealsWorkingUnitSchedules1.Name = "mealsWorkingUnitSchedules1";
            this.mealsWorkingUnitSchedules1.Size = new System.Drawing.Size(963, 652);
            this.mealsWorkingUnitSchedules1.TabIndex = 0;
            // 
            // tpReports
            // 
            this.tpReports.Controls.Add(this.mealEmployeeSchedulePreview1);
            this.tpReports.Location = new System.Drawing.Point(4, 22);
            this.tpReports.Name = "tpReports";
            this.tpReports.Padding = new System.Windows.Forms.Padding(3);
            this.tpReports.Size = new System.Drawing.Size(971, 634);
            this.tpReports.TabIndex = 3;
            this.tpReports.Text = "Meal type employees";
            this.tpReports.UseVisualStyleBackColor = true;
            // 
            // mealEmployeeSchedulePreview1
            // 
            this.mealEmployeeSchedulePreview1.Location = new System.Drawing.Point(6, 6);
            this.mealEmployeeSchedulePreview1.Name = "mealEmployeeSchedulePreview1";
            this.mealEmployeeSchedulePreview1.Size = new System.Drawing.Size(882, 640);
            this.mealEmployeeSchedulePreview1.TabIndex = 0;
            // 
            // tpMealPoints
            // 
            this.tpMealPoints.Controls.Add(this.mealPointsII1);
            this.tpMealPoints.Location = new System.Drawing.Point(4, 22);
            this.tpMealPoints.Name = "tpMealPoints";
            this.tpMealPoints.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealPoints.Size = new System.Drawing.Size(971, 634);
            this.tpMealPoints.TabIndex = 3;
            this.tpMealPoints.Text = "Meal points";
            this.tpMealPoints.UseVisualStyleBackColor = true;
            // 
            // mealPointsII1
            // 
            this.mealPointsII1.Location = new System.Drawing.Point(6, 6);
            this.mealPointsII1.Name = "mealPointsII1";
            this.mealPointsII1.Size = new System.Drawing.Size(694, 370);
            this.mealPointsII1.TabIndex = 0;
            // 
            // tpOrder
            // 
            this.tpOrder.Controls.Add(this.mealOrderControl1);
            this.tpOrder.Location = new System.Drawing.Point(4, 22);
            this.tpOrder.Name = "tpOrder";
            this.tpOrder.Padding = new System.Windows.Forms.Padding(3);
            this.tpOrder.Size = new System.Drawing.Size(971, 634);
            this.tpOrder.TabIndex = 4;
            this.tpOrder.Text = "Order";
            this.tpOrder.UseVisualStyleBackColor = true;
            // 
            // mealOrderControl1
            // 
            this.mealOrderControl1.Location = new System.Drawing.Point(0, 0);
            this.mealOrderControl1.Name = "mealOrderControl1";
            this.mealOrderControl1.Size = new System.Drawing.Size(320, 150);
            this.mealOrderControl1.TabIndex = 0;
            // 
            // rptDataSet1
            // 
            this.rptDataSet1.DataSetName = "RptDataSet";
            this.rptDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // RestaurantII
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 731);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RestaurantII";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "RestaurantII";
            this.Load += new System.EventHandler(this.RestaurantII_Load);
            this.tabControl.ResumeLayout(false);
            this.tpMealType.ResumeLayout(false);
            this.tpMealsSchedule.ResumeLayout(false);
            this.tpSetingSchedule.ResumeLayout(false);
            this.tpReports.ResumeLayout(false);
            this.tpMealPoints.ResumeLayout(false);
            this.tpOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rptDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpMealType;
        private System.Windows.Forms.TabPage tpMealsSchedule;
        private System.Windows.Forms.TabPage tpSetingSchedule;
        private System.Windows.Forms.TabPage tpReports;
        private DataAccess.RptDataSet rptDataSet1;
        private MealTypesII mealTypesII1;
        private MealTypeSchedules mealTypeSchedules1;
        private MealsWorkingUnitSchedules mealsWorkingUnitSchedules1;
        private System.Windows.Forms.TabPage tpMealPoints;
        private MealPointsII mealPointsII1;
        private System.Windows.Forms.TabPage tpOrder;
        private MealEmployeeSchedulePreview mealEmployeeSchedulePreview1;
        private MealOrderControl mealOrderControl1;
    }
}