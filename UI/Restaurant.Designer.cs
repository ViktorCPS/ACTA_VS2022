namespace UI
{
    partial class Restaurant
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpMealType = new System.Windows.Forms.TabPage();
            this.mealTypes1 = new UI.MealTypes();
            this.tpMealsAssigned = new System.Windows.Forms.TabPage();
            this.mealsAssigned1 = new UI.MealsAssigned();
            this.tpMealPoints = new System.Windows.Forms.TabPage();
            this.mealPoints1 = new UI.MealPoints();
            this.tpMealTypeEmpl = new System.Windows.Forms.TabPage();
            this.mealTypeEmployees1 = new UI.MealTypeEmployees();
            this.tpMealsUsed = new System.Windows.Forms.TabPage();
            this.mealsUsed = new UI.MealsUsed();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tpMealType.SuspendLayout();
            this.tpMealsAssigned.SuspendLayout();
            this.tpMealPoints.SuspendLayout();
            this.tpMealTypeEmpl.SuspendLayout();
            this.tpMealsUsed.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpMealType);
            this.tabControl.Controls.Add(this.tpMealsAssigned);
            this.tabControl.Controls.Add(this.tpMealPoints);
            this.tabControl.Controls.Add(this.tpMealTypeEmpl);
            this.tabControl.Controls.Add(this.tpMealsUsed);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(748, 535);
            this.tabControl.TabIndex = 0;
            // 
            // tpMealType
            // 
            this.tpMealType.Controls.Add(this.mealTypes1);
            this.tpMealType.Location = new System.Drawing.Point(4, 22);
            this.tpMealType.Name = "tpMealType";
            this.tpMealType.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealType.Size = new System.Drawing.Size(740, 509);
            this.tpMealType.TabIndex = 0;
            this.tpMealType.Text = "Meal type";
            this.tpMealType.UseVisualStyleBackColor = true;
            // 
            // mealTypes1
            // 
            this.mealTypes1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealTypes1.Location = new System.Drawing.Point(0, 2);
            this.mealTypes1.Name = "mealTypes1";
            this.mealTypes1.Size = new System.Drawing.Size(734, 511);
            this.mealTypes1.TabIndex = 0;
            // 
            // tpMealsAssigned
            // 
            this.tpMealsAssigned.Controls.Add(this.mealsAssigned1);
            this.tpMealsAssigned.Location = new System.Drawing.Point(4, 22);
            this.tpMealsAssigned.Name = "tpMealsAssigned";
            this.tpMealsAssigned.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealsAssigned.Size = new System.Drawing.Size(740, 509);
            this.tpMealsAssigned.TabIndex = 1;
            this.tpMealsAssigned.Text = "Meals assigned";
            this.tpMealsAssigned.UseVisualStyleBackColor = true;
            // 
            // mealsAssigned1
            // 
            this.mealsAssigned1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealsAssigned1.Location = new System.Drawing.Point(3, 3);
            this.mealsAssigned1.Name = "mealsAssigned1";
            this.mealsAssigned1.Size = new System.Drawing.Size(734, 511);
            this.mealsAssigned1.TabIndex = 0;
            // 
            // tpMealPoints
            // 
            this.tpMealPoints.Controls.Add(this.mealPoints1);
            this.tpMealPoints.Location = new System.Drawing.Point(4, 22);
            this.tpMealPoints.Name = "tpMealPoints";
            this.tpMealPoints.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealPoints.Size = new System.Drawing.Size(740, 509);
            this.tpMealPoints.TabIndex = 2;
            this.tpMealPoints.Text = "Meal points";
            this.tpMealPoints.UseVisualStyleBackColor = true;
            // 
            // mealPoints1
            // 
            this.mealPoints1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealPoints1.Location = new System.Drawing.Point(0, 0);
            this.mealPoints1.Name = "mealPoints1";
            this.mealPoints1.Size = new System.Drawing.Size(734, 511);
            this.mealPoints1.TabIndex = 0;
            // 
            // tpMealTypeEmpl
            // 
            this.tpMealTypeEmpl.Controls.Add(this.mealTypeEmployees1);
            this.tpMealTypeEmpl.Location = new System.Drawing.Point(4, 22);
            this.tpMealTypeEmpl.Name = "tpMealTypeEmpl";
            this.tpMealTypeEmpl.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealTypeEmpl.Size = new System.Drawing.Size(740, 509);
            this.tpMealTypeEmpl.TabIndex = 3;
            this.tpMealTypeEmpl.Text = "Meal type employees";
            this.tpMealTypeEmpl.UseVisualStyleBackColor = true;
            // 
            // mealTypeEmployees1
            // 
            this.mealTypeEmployees1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealTypeEmployees1.Location = new System.Drawing.Point(0, -5);
            this.mealTypeEmployees1.Name = "mealTypeEmployees1";
            this.mealTypeEmployees1.Size = new System.Drawing.Size(734, 511);
            this.mealTypeEmployees1.TabIndex = 0;
            // 
            // tpMealsUsed
            // 
            this.tpMealsUsed.Controls.Add(this.mealsUsed);
            this.tpMealsUsed.Location = new System.Drawing.Point(4, 22);
            this.tpMealsUsed.Name = "tpMealsUsed";
            this.tpMealsUsed.Padding = new System.Windows.Forms.Padding(3);
            this.tpMealsUsed.Size = new System.Drawing.Size(740, 509);
            this.tpMealsUsed.TabIndex = 3;
            this.tpMealsUsed.Text = "Meals used";
            this.tpMealsUsed.UseVisualStyleBackColor = true;
            // 
            // mealsUsed
            // 
            this.mealsUsed.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealsUsed.Location = new System.Drawing.Point(0, -10);
            this.mealsUsed.MaximumSize = new System.Drawing.Size(734, 511);
            this.mealsUsed.MinimumSize = new System.Drawing.Size(734, 511);
            this.mealsUsed.Name = "mealsUsed";
            this.mealsUsed.Size = new System.Drawing.Size(734, 511);
            this.mealsUsed.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(685, 554);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Restaurant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 583);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl);
            this.KeyPreview = true;
            this.Name = "Restaurant";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Restaurant";
            this.Load += new System.EventHandler(this.Restaurant_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Restaurant_KeyUp);
            this.tabControl.ResumeLayout(false);
            this.tpMealType.ResumeLayout(false);
            this.tpMealsAssigned.ResumeLayout(false);
            this.tpMealPoints.ResumeLayout(false);
            this.tpMealTypeEmpl.ResumeLayout(false);
            this.tpMealsUsed.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpMealType;
        private System.Windows.Forms.TabPage tpMealsAssigned;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tpMealPoints;
        private System.Windows.Forms.TabPage tpMealTypeEmpl;
        private System.Windows.Forms.TabPage tpMealsUsed;
        private MealsUsed mealsUsed;
        private MealsAssigned mealsAssigned1;
        private MealTypes mealTypes1;
        private MealPoints mealPoints1;
        private MealTypeEmployees mealTypeEmployees1;
    }
}