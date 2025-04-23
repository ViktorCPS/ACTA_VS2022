namespace UI
{
    partial class MonthlyMenu
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
            this.gbSchedule = new System.Windows.Forms.GroupBox();
            this.lvMonthlyMenu = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.gbSchedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSchedule
            // 
            this.gbSchedule.Controls.Add(this.lvMonthlyMenu);
            this.gbSchedule.Location = new System.Drawing.Point(12, 12);
            this.gbSchedule.Name = "gbSchedule";
            this.gbSchedule.Size = new System.Drawing.Size(430, 433);
            this.gbSchedule.TabIndex = 0;
            this.gbSchedule.TabStop = false;
            this.gbSchedule.Text = "Meals Type Schedule";
            // 
            // lvMonthlyMenu
            // 
            this.lvMonthlyMenu.FullRowSelect = true;
            this.lvMonthlyMenu.GridLines = true;
            this.lvMonthlyMenu.HideSelection = false;
            this.lvMonthlyMenu.Location = new System.Drawing.Point(16, 37);
            this.lvMonthlyMenu.Name = "lvMonthlyMenu";
            this.lvMonthlyMenu.Size = new System.Drawing.Size(398, 390);
            this.lvMonthlyMenu.TabIndex = 0;
            this.lvMonthlyMenu.UseCompatibleStateImageBehavior = false;
            this.lvMonthlyMenu.View = System.Windows.Forms.View.Details;
            this.lvMonthlyMenu.SelectedIndexChanged += new System.EventHandler(this.lvMonthlyMenu_SelectedIndexChanged);
            this.lvMonthlyMenu.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMonthlyMenu_ColumnClick_1);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(190, 463);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(351, 463);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // MonthlyMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 498);
            this.ControlBox = false;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.gbSchedule);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MonthlyMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MonthlyMenu";
            this.Load += new System.EventHandler(this.MonthlyMenu_Load);
            this.gbSchedule.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSchedule;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView lvMonthlyMenu;
        private System.Windows.Forms.Button btnExit;
    }
}