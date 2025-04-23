namespace UI
{
    partial class ExitPermDaysSelection
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
            this.lvDays = new System.Windows.Forms.ListView();
            this.lblRemoveDay = new System.Windows.Forms.Label();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(155, 264);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvDays
            // 
            this.lvDays.FullRowSelect = true;
            this.lvDays.GridLines = true;
            this.lvDays.HideSelection = false;
            this.lvDays.Location = new System.Drawing.Point(259, 40);
            this.lvDays.Name = "lvDays";
            this.lvDays.Size = new System.Drawing.Size(114, 175);
            this.lvDays.TabIndex = 25;
            this.lvDays.UseCompatibleStateImageBehavior = false;
            this.lvDays.View = System.Windows.Forms.View.Details;
            this.lvDays.DoubleClick += new System.EventHandler(this.lvDays_DoubleClick);
            // 
            // lblRemoveDay
            // 
            this.lblRemoveDay.Location = new System.Drawing.Point(256, 218);
            this.lblRemoveDay.Name = "lblRemoveDay";
            this.lblRemoveDay.Size = new System.Drawing.Size(117, 69);
            this.lblRemoveDay.TabIndex = 31;
            this.lblRemoveDay.Text = "*Dbl click to remove day from list";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.FirstDayOfWeek = System.Windows.Forms.Day.Monday;
            this.monthCalendar1.Location = new System.Drawing.Point(31, 40);
            this.monthCalendar1.MaxSelectionCount = 1;
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 32;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // ExitPermDaysSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 299);
            this.ControlBox = false;
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.lblRemoveDay);
            this.Controls.Add(this.lvDays);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "ExitPermDaysSelection";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ExitPermDaysSelection";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExitPermDaysSelection_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvDays;
        private System.Windows.Forms.Label lblRemoveDay;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
    }
}