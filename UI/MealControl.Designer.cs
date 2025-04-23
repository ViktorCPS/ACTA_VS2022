namespace UI
{
    partial class MealControl
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
            this.btnMeal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMeal
            // 
            this.btnMeal.Location = new System.Drawing.Point(0, 0);
            this.btnMeal.Name = "btnMeal";
            this.btnMeal.Size = new System.Drawing.Size(173, 37);
            this.btnMeal.TabIndex = 0;
            this.btnMeal.Text = "Meal";
            this.btnMeal.UseVisualStyleBackColor = true;
            this.btnMeal.Click += new System.EventHandler(this.btnMeal_Click);
            // 
            // MealControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnMeal);
            this.Name = "MealControl";
            this.Size = new System.Drawing.Size(173, 37);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMeal;
    }
}
