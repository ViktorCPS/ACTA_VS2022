namespace RestaurantSelfService
{
    partial class EmployeeSchedules
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
            this.mealsWorkingUnitSchedules1 = new UI.MealsWorkingUnitSchedules();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mealsWorkingUnitSchedules1
            // 
            this.mealsWorkingUnitSchedules1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mealsWorkingUnitSchedules1.Location = new System.Drawing.Point(18, 12);
            this.mealsWorkingUnitSchedules1.Name = "mealsWorkingUnitSchedules1";
            this.mealsWorkingUnitSchedules1.Size = new System.Drawing.Size(963, 656);
            this.mealsWorkingUnitSchedules1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(884, 674);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 50);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // EmployeeSchedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 736);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.mealsWorkingUnitSchedules1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EmployeeSchedules";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "EmployeeSchedules";
            this.ResumeLayout(false);

        }

        #endregion

        private UI.MealsWorkingUnitSchedules mealsWorkingUnitSchedules1;
        private System.Windows.Forms.Button btnClose;
    }
}