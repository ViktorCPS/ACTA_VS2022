namespace UI
{
    partial class MealDayControl
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
            this.lblDay = new System.Windows.Forms.Label();
            this.lblDayNum = new System.Windows.Forms.Label();
            this.lblMeal = new System.Windows.Forms.Label();
            this.lblNotWorking = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDay
            // 
            this.lblDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.Location = new System.Drawing.Point(12, 0);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(76, 15);
            this.lblDay.TabIndex = 1;
            this.lblDay.Text = "Day";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDay.Click += new System.EventHandler(this.lblDay_Click);
            // 
            // lblDayNum
            // 
            this.lblDayNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayNum.Location = new System.Drawing.Point(-4, -7);
            this.lblDayNum.Name = "lblDayNum";
            this.lblDayNum.Size = new System.Drawing.Size(40, 38);
            this.lblDayNum.TabIndex = 2;
            this.lblDayNum.Text = "31";
            this.lblDayNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDayNum.Click += new System.EventHandler(this.lblDayNum_Click);
            // 
            // lblMeal
            // 
            this.lblMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMeal.Location = new System.Drawing.Point(5, 33);
            this.lblMeal.Name = "lblMeal";
            this.lblMeal.Size = new System.Drawing.Size(76, 15);
            this.lblMeal.TabIndex = 3;
            this.lblMeal.Text = "Meal";
            this.lblMeal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMeal.Click += new System.EventHandler(this.lblMeal_Click);
            // 
            // lblNotWorking
            // 
            this.lblNotWorking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotWorking.Location = new System.Drawing.Point(0, 31);
            this.lblNotWorking.Name = "lblNotWorking";
            this.lblNotWorking.Size = new System.Drawing.Size(101, 38);
            this.lblNotWorking.TabIndex = 4;
            this.lblNotWorking.Text = "Not working day";
            this.lblNotWorking.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblNotWorking.Visible = false;
            this.lblNotWorking.Click += new System.EventHandler(this.lblNotWorking_Click);
            // 
            // MealDayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNotWorking);
            this.Controls.Add(this.lblMeal);
            this.Controls.Add(this.lblDayNum);
            this.Controls.Add(this.lblDay);
            this.Name = "MealDayControl";
            this.Size = new System.Drawing.Size(101, 71);
            this.Load += new System.EventHandler(this.MealDayControl_Load);
            this.Click += new System.EventHandler(this.MealDayControl_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblDayNum;
        private System.Windows.Forms.Label lblMeal;
        private System.Windows.Forms.Label lblNotWorking;
    }
}
