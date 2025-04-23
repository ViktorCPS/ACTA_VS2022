namespace UI
{
    partial class RestaurantIII
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
            this.tpMealsUsed = new System.Windows.Forms.TabPage();
            this.tpMealRestaurant = new System.Windows.Forms.TabPage();
            this.tpMealPoints = new System.Windows.Forms.TabPage();
            this.tpMealType = new System.Windows.Forms.TabPage();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpReports = new System.Windows.Forms.TabPage();
            this.mealTypes1 = new UI.OnlineMealTypes();
            this.onlineMealRestaurant1 = new UI.OnlineMealRestaurant();
            this.mealPoints1 = new UI.OnlineMealPoint();
            this.mealsUsed = new UI.OnlineMealUsed();
            this.mealRestaurant = new UI.OnlineMealRestaurant();
            this.onlineMealsReports = new UI.OnlineMealReports();
            this.tpMealsUsed.SuspendLayout();
            this.tpMealRestaurant.SuspendLayout();
            this.tpMealPoints.SuspendLayout();
            this.tpMealType.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(985, 727);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tpMealsUsed
            // 
            this.tpMealsUsed.Controls.Add(this.mealsUsed);
            this.tpMealsUsed.Location = new System.Drawing.Point(4, 22);
            this.tpMealsUsed.Name = "tpMealsUsed";
            this.tpMealsUsed.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealsUsed.Size = new System.Drawing.Size(1068, 687);
            this.tpMealsUsed.TabIndex = 3;
            this.tpMealsUsed.Text = "Meals used";
            this.tpMealsUsed.UseVisualStyleBackColor = true;
            // 
            // tpMealRestaurant
            // 
            this.tpMealRestaurant.Controls.Add(this.onlineMealRestaurant1);
            this.tpMealRestaurant.Location = new System.Drawing.Point(4, 22);
            this.tpMealRestaurant.Name = "tpMealRestaurant";
            this.tpMealRestaurant.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealRestaurant.Size = new System.Drawing.Size(1068, 687);
            this.tpMealRestaurant.TabIndex = 3;
            this.tpMealRestaurant.Text = "Restaurants";
            this.tpMealRestaurant.UseVisualStyleBackColor = true;
            // 
            // tpMealPoints
            // 
            this.tpMealPoints.Controls.Add(this.mealPoints1);
            this.tpMealPoints.Location = new System.Drawing.Point(4, 22);
            this.tpMealPoints.Name = "tpMealPoints";
            this.tpMealPoints.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealPoints.Size = new System.Drawing.Size(1068, 687);
            this.tpMealPoints.TabIndex = 2;
            this.tpMealPoints.Text = "Meal points";
            this.tpMealPoints.UseVisualStyleBackColor = true;
            // 
            // tpMealType
            // 
            this.tpMealType.Controls.Add(this.mealTypes1);
            this.tpMealType.Location = new System.Drawing.Point(4, 22);
            this.tpMealType.Name = "tpMealType";
            this.tpMealType.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealType.Size = new System.Drawing.Size(1068, 687);
            this.tpMealType.TabIndex = 0;
            this.tpMealType.Text = "Meal type";
            this.tpMealType.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpMealType);
            this.tabControl.Controls.Add(this.tpMealRestaurant);
            this.tabControl.Controls.Add(this.tpMealPoints);
            this.tabControl.Controls.Add(this.tpMealsUsed);
            this.tabControl.Controls.Add(this.tpReports);
            this.tabControl.Location = new System.Drawing.Point(11, 8);
            this.tabControl.MinimumSize = new System.Drawing.Size(944, 607);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1076, 713);
            this.tabControl.TabIndex = 2;
            // 
            // tpReports
            // 
            this.tpReports.Controls.Add(this.onlineMealsReports);
            this.tpReports.Location = new System.Drawing.Point(4, 22);
            this.tpReports.Name = "tpReports";
            this.tpReports.Padding = new System.Windows.Forms.Padding(3);
            this.tpReports.Size = new System.Drawing.Size(1068, 687);
            this.tpReports.TabIndex = 4;
            this.tpReports.Text = "Reports";
            this.tpReports.UseVisualStyleBackColor = true;
            // 
            // mealTypes1
            // 
            this.mealTypes1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealTypes1.Location = new System.Drawing.Point(6, 6);
            this.mealTypes1.Name = "mealTypes1";
            this.mealTypes1.Size = new System.Drawing.Size(845, 619);
            this.mealTypes1.TabIndex = 0;
            // 
            // onlineMealRestaurant1
            // 
            this.onlineMealRestaurant1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.onlineMealRestaurant1.Location = new System.Drawing.Point(6, 15);
            this.onlineMealRestaurant1.Name = "onlineMealRestaurant1";
            this.onlineMealRestaurant1.Size = new System.Drawing.Size(845, 613);
            this.onlineMealRestaurant1.TabIndex = 0;
            // 
            // mealPoints1
            // 
            this.mealPoints1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealPoints1.Location = new System.Drawing.Point(6, 6);
            this.mealPoints1.Name = "mealPoints1";
            this.mealPoints1.Size = new System.Drawing.Size(934, 593);
            this.mealPoints1.TabIndex = 0;
            // 
            // mealsUsed
            // 
            this.mealsUsed.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealsUsed.Location = new System.Drawing.Point(6, -3);
            this.mealsUsed.MaximumSize = new System.Drawing.Size(1041, 647);
            this.mealsUsed.MinimumSize = new System.Drawing.Size(1063, 689);
            this.mealsUsed.Name = "mealsUsed";
            this.mealsUsed.Size = new System.Drawing.Size(1063, 689);
            this.mealsUsed.TabIndex = 0;
            // 
            // mealRestaurant
            // 
            this.mealRestaurant.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealRestaurant.Location = new System.Drawing.Point(0, 0);
            this.mealRestaurant.Name = "mealRestaurant";
            this.mealRestaurant.Size = new System.Drawing.Size(719, 531);
            this.mealRestaurant.TabIndex = 0;

            this.onlineMealsReports.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.onlineMealsReports.Location = new System.Drawing.Point(0, 0);
            this.onlineMealsReports.Name = "onlineMealsReports";
            this.onlineMealsReports.Size = new System.Drawing.Size(845, 613);
            this.onlineMealsReports.TabIndex = 0;
            // 
            // RestaurantIII
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 758);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RestaurantIII";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RestaurantIII";
            this.Load += new System.EventHandler(this.Restaurant_Load);
            this.tpMealsUsed.ResumeLayout(false);
            this.tpMealRestaurant.ResumeLayout(false);
            this.tpMealPoints.ResumeLayout(false);
            this.tpMealType.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OnlineMealRestaurant mealRestaurant;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tpMealsUsed;
        private OnlineMealUsed mealsUsed;
        private System.Windows.Forms.TabPage tpMealRestaurant;
        private System.Windows.Forms.TabPage tpMealPoints;
        private OnlineMealPoint mealPoints1;
        private System.Windows.Forms.TabPage tpMealType;
        private OnlineMealTypes mealTypes1;
        private System.Windows.Forms.TabControl tabControl;
        private OnlineMealRestaurant onlineMealRestaurant1;
        private System.Windows.Forms.TabPage tpReports;
        private OnlineMealReports onlineMealsReports;


    }
}