namespace UI
{
    partial class CustomVisitGraphAdv
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomVisitGraphAdv));
            this.graphPanel = new System.Windows.Forms.Panel();
            this.lblGraphName = new System.Windows.Forms.Label();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnLocationTree = new System.Windows.Forms.Button();
            this.cbGraphType = new System.Windows.Forms.ComboBox();
            this.lblGraphType = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.pbClientLogo = new System.Windows.Forms.PictureBox();
            this.gbDayInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblHourFrom = new System.Windows.Forms.Label();
            this.dtpHoursFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpHoursTo = new System.Windows.Forms.DateTimePicker();
            this.lblHourTo = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.gbDayOfWeek = new System.Windows.Forms.GroupBox();
            this.tbDayOfWeek = new System.Windows.Forms.TextBox();
            this.dtpDayOfWeek = new System.Windows.Forms.DateTimePicker();
            this.lblDateToDayOfWeek = new System.Windows.Forms.Label();
            this.dtpDateToDayOfWeek = new System.Windows.Forms.DateTimePicker();
            this.lblDayOfWeek = new System.Windows.Forms.Label();
            this.lblDateFromDayOfWeek = new System.Windows.Forms.Label();
            this.dtpDateFromDayOfWeek = new System.Windows.Forms.DateTimePicker();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphPanel.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).BeginInit();
            this.gbDayInterval.SuspendLayout();
            this.gbDayOfWeek.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphPanel
            // 
            this.graphPanel.AutoScroll = true;
            this.graphPanel.Controls.Add(this.lblGraphName);
            this.graphPanel.Location = new System.Drawing.Point(12, 12);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(963, 567);
            this.graphPanel.TabIndex = 32;
            // 
            // lblGraphName
            // 
            this.lblGraphName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblGraphName.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGraphName.Location = new System.Drawing.Point(0, 0);
            this.lblGraphName.Name = "lblGraphName";
            this.lblGraphName.Size = new System.Drawing.Size(960, 43);
            this.lblGraphName.TabIndex = 1;
            this.lblGraphName.Text = "text";
            this.lblGraphName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(273, 596);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(242, 104);
            this.gbTimeInterval.TabIndex = 39;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time interval";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(77, 50);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(126, 20);
            this.dtpTo.TabIndex = 30;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 22);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(64, 23);
            this.lblFrom.TabIndex = 27;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(10, 48);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(61, 23);
            this.lblTo.TabIndex = 28;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(77, 22);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(126, 20);
            this.dtpFrom.TabIndex = 29;
            // 
            // btnLocationTree
            // 
            this.btnLocationTree.Image = ((System.Drawing.Image)(resources.GetObject("btnLocationTree.Image")));
            this.btnLocationTree.Location = new System.Drawing.Point(242, 600);
            this.btnLocationTree.Name = "btnLocationTree";
            this.btnLocationTree.Size = new System.Drawing.Size(25, 24);
            this.btnLocationTree.TabIndex = 34;
            this.btnLocationTree.Click += new System.EventHandler(this.btnLocationTree_Click);
            // 
            // cbGraphType
            // 
            this.cbGraphType.Location = new System.Drawing.Point(96, 679);
            this.cbGraphType.Name = "cbGraphType";
            this.cbGraphType.Size = new System.Drawing.Size(140, 21);
            this.cbGraphType.TabIndex = 38;
            this.cbGraphType.SelectedIndexChanged += new System.EventHandler(this.cbGraphType_SelectedIndexChanged);
            // 
            // lblGraphType
            // 
            this.lblGraphType.Location = new System.Drawing.Point(13, 677);
            this.lblGraphType.Name = "lblGraphType";
            this.lblGraphType.Size = new System.Drawing.Size(77, 23);
            this.lblGraphType.TabIndex = 37;
            this.lblGraphType.Text = "Graph type:";
            this.lblGraphType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLocation
            // 
            this.cbLocation.Location = new System.Drawing.Point(96, 601);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(140, 21);
            this.cbLocation.TabIndex = 36;
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(19, 599);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(71, 23);
            this.lblLocation.TabIndex = 35;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(530, 677);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 33;
            this.btnShow.Text = "Show";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // pbClientLogo
            // 
            this.pbClientLogo.Location = new System.Drawing.Point(869, 670);
            this.pbClientLogo.Name = "pbClientLogo";
            this.pbClientLogo.Size = new System.Drawing.Size(106, 30);
            this.pbClientLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbClientLogo.TabIndex = 40;
            this.pbClientLogo.TabStop = false;
            // 
            // gbDayInterval
            // 
            this.gbDayInterval.Controls.Add(this.dtpToDate);
            this.gbDayInterval.Controls.Add(this.dtpFromDate);
            this.gbDayInterval.Controls.Add(this.label3);
            this.gbDayInterval.Controls.Add(this.label4);
            this.gbDayInterval.Controls.Add(this.lblHourFrom);
            this.gbDayInterval.Controls.Add(this.dtpHoursFrom);
            this.gbDayInterval.Controls.Add(this.dtpHoursTo);
            this.gbDayInterval.Controls.Add(this.lblHourTo);
            this.gbDayInterval.Controls.Add(this.lblFromDate);
            this.gbDayInterval.Controls.Add(this.lblDateTo);
            this.gbDayInterval.Location = new System.Drawing.Point(273, 596);
            this.gbDayInterval.Name = "gbDayInterval";
            this.gbDayInterval.Size = new System.Drawing.Size(242, 104);
            this.gbDayInterval.TabIndex = 40;
            this.gbDayInterval.TabStop = false;
            this.gbDayInterval.Text = "Time interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(74, 42);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(126, 20);
            this.dtpToDate.TabIndex = 47;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(74, 16);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(126, 20);
            this.dtpFromDate.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(121, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 23);
            this.label3.TabIndex = 45;
            this.label3.Text = "-";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(206, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 23);
            this.label4.TabIndex = 44;
            this.label4.Text = "h";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHourFrom
            // 
            this.lblHourFrom.Location = new System.Drawing.Point(4, 66);
            this.lblHourFrom.Name = "lblHourFrom";
            this.lblHourFrom.Size = new System.Drawing.Size(64, 23);
            this.lblHourFrom.TabIndex = 41;
            this.lblHourFrom.Text = "From:";
            this.lblHourFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpHoursFrom
            // 
            this.dtpHoursFrom.CustomFormat = "HH";
            this.dtpHoursFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursFrom.Location = new System.Drawing.Point(74, 68);
            this.dtpHoursFrom.Name = "dtpHoursFrom";
            this.dtpHoursFrom.ShowUpDown = true;
            this.dtpHoursFrom.Size = new System.Drawing.Size(41, 20);
            this.dtpHoursFrom.TabIndex = 40;
            // 
            // dtpHoursTo
            // 
            this.dtpHoursTo.CustomFormat = "HH";
            this.dtpHoursTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHoursTo.Location = new System.Drawing.Point(158, 67);
            this.dtpHoursTo.Name = "dtpHoursTo";
            this.dtpHoursTo.ShowUpDown = true;
            this.dtpHoursTo.Size = new System.Drawing.Size(42, 20);
            this.dtpHoursTo.TabIndex = 39;
            // 
            // lblHourTo
            // 
            this.lblHourTo.Location = new System.Drawing.Point(89, 65);
            this.lblHourTo.Name = "lblHourTo";
            this.lblHourTo.Size = new System.Drawing.Size(67, 23);
            this.lblHourTo.TabIndex = 38;
            this.lblHourTo.Text = "To:";
            this.lblHourTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Location = new System.Drawing.Point(4, 16);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(64, 23);
            this.lblFromDate.TabIndex = 27;
            this.lblFromDate.Text = "From:";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDateTo
            // 
            this.lblDateTo.Location = new System.Drawing.Point(7, 41);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(61, 23);
            this.lblDateTo.TabIndex = 28;
            this.lblDateTo.Text = "To:";
            this.lblDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbDayOfWeek
            // 
            this.gbDayOfWeek.Controls.Add(this.tbDayOfWeek);
            this.gbDayOfWeek.Controls.Add(this.dtpDayOfWeek);
            this.gbDayOfWeek.Controls.Add(this.lblDateToDayOfWeek);
            this.gbDayOfWeek.Controls.Add(this.dtpDateToDayOfWeek);
            this.gbDayOfWeek.Controls.Add(this.lblDayOfWeek);
            this.gbDayOfWeek.Controls.Add(this.lblDateFromDayOfWeek);
            this.gbDayOfWeek.Controls.Add(this.dtpDateFromDayOfWeek);
            this.gbDayOfWeek.Location = new System.Drawing.Point(273, 596);
            this.gbDayOfWeek.Name = "gbDayOfWeek";
            this.gbDayOfWeek.Size = new System.Drawing.Size(242, 104);
            this.gbDayOfWeek.TabIndex = 48;
            this.gbDayOfWeek.TabStop = false;
            this.gbDayOfWeek.Text = "Time interval";
            // 
            // tbDayOfWeek
            // 
            this.tbDayOfWeek.Location = new System.Drawing.Point(77, 69);
            this.tbDayOfWeek.Name = "tbDayOfWeek";
            this.tbDayOfWeek.Size = new System.Drawing.Size(108, 20);
            this.tbDayOfWeek.TabIndex = 0;
            this.tbDayOfWeek.TextChanged += new System.EventHandler(this.tbDayOfWeek_TextChanged);
            // 
            // dtpDayOfWeek
            // 
            this.dtpDayOfWeek.CustomFormat = "dd";
            this.dtpDayOfWeek.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDayOfWeek.Location = new System.Drawing.Point(77, 69);
            this.dtpDayOfWeek.Name = "dtpDayOfWeek";
            this.dtpDayOfWeek.ShowUpDown = true;
            this.dtpDayOfWeek.Size = new System.Drawing.Size(126, 20);
            this.dtpDayOfWeek.TabIndex = 40;
            this.dtpDayOfWeek.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // lblDateToDayOfWeek
            // 
            this.lblDateToDayOfWeek.Location = new System.Drawing.Point(10, 42);
            this.lblDateToDayOfWeek.Name = "lblDateToDayOfWeek";
            this.lblDateToDayOfWeek.Size = new System.Drawing.Size(61, 23);
            this.lblDateToDayOfWeek.TabIndex = 38;
            this.lblDateToDayOfWeek.Text = "Date to:";
            this.lblDateToDayOfWeek.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDateToDayOfWeek
            // 
            this.dtpDateToDayOfWeek.CustomFormat = "dd.MM.yyyy";
            this.dtpDateToDayOfWeek.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateToDayOfWeek.Location = new System.Drawing.Point(77, 43);
            this.dtpDateToDayOfWeek.Name = "dtpDateToDayOfWeek";
            this.dtpDateToDayOfWeek.Size = new System.Drawing.Size(126, 20);
            this.dtpDateToDayOfWeek.TabIndex = 39;
            // 
            // lblDayOfWeek
            // 
            this.lblDayOfWeek.Location = new System.Drawing.Point(6, 68);
            this.lblDayOfWeek.Name = "lblDayOfWeek";
            this.lblDayOfWeek.Size = new System.Drawing.Size(64, 23);
            this.lblDayOfWeek.TabIndex = 32;
            this.lblDayOfWeek.Text = "Day:";
            this.lblDayOfWeek.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDateFromDayOfWeek
            // 
            this.lblDateFromDayOfWeek.Location = new System.Drawing.Point(10, 16);
            this.lblDateFromDayOfWeek.Name = "lblDateFromDayOfWeek";
            this.lblDateFromDayOfWeek.Size = new System.Drawing.Size(61, 23);
            this.lblDateFromDayOfWeek.TabIndex = 27;
            this.lblDateFromDayOfWeek.Text = "Date from:";
            this.lblDateFromDayOfWeek.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDateFromDayOfWeek
            // 
            this.dtpDateFromDayOfWeek.CustomFormat = "dd.MM.yyyy";
            this.dtpDateFromDayOfWeek.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFromDayOfWeek.Location = new System.Drawing.Point(77, 18);
            this.dtpDateFromDayOfWeek.Name = "dtpDateFromDayOfWeek";
            this.dtpDateFromDayOfWeek.Size = new System.Drawing.Size(126, 20);
            this.dtpDateFromDayOfWeek.TabIndex = 29;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Location = new System.Drawing.Point(530, 600);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(75, 23);
            this.btnSwitch.TabIndex = 49;
            this.btnSwitch.Text = "Pie";
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.saveImageAsToolStripMenuItem,
            this.printSetupToolStripMenuItem,
            this.printToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(167, 92);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // saveImageAsToolStripMenuItem
            // 
            this.saveImageAsToolStripMenuItem.Name = "saveImageAsToolStripMenuItem";
            this.saveImageAsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.saveImageAsToolStripMenuItem.Text = "Save image as...";
            this.saveImageAsToolStripMenuItem.Click += new System.EventHandler(this.saveImageAsToolStripMenuItem_Click);
            // 
            // printSetupToolStripMenuItem
            // 
            this.printSetupToolStripMenuItem.Name = "printSetupToolStripMenuItem";
            this.printSetupToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.printSetupToolStripMenuItem.Text = "Page setup...";
            this.printSetupToolStripMenuItem.Click += new System.EventHandler(this.printSetupToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.printToolStripMenuItem.Text = "Print...";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // CustomVisitGraphAdv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 713);
            this.Controls.Add(this.btnSwitch);
            this.Controls.Add(this.gbDayOfWeek);
            this.Controls.Add(this.gbDayInterval);
            this.Controls.Add(this.pbClientLogo);
            this.Controls.Add(this.gbTimeInterval);
            this.Controls.Add(this.btnLocationTree);
            this.Controls.Add(this.cbGraphType);
            this.Controls.Add(this.lblGraphType);
            this.Controls.Add(this.cbLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.graphPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomVisitGraphAdv";
            this.ShowInTaskbar = false;
            this.Text = "CustomVisitGraphAdv";
            this.Load += new System.EventHandler(this.CustomVisitGraphAdv_Load);
            this.graphPanel.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).EndInit();
            this.gbDayInterval.ResumeLayout(false);
            this.gbDayOfWeek.ResumeLayout(false);
            this.gbDayOfWeek.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnLocationTree;
        private System.Windows.Forms.ComboBox cbGraphType;
        private System.Windows.Forms.Label lblGraphType;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.PictureBox pbClientLogo;
        private System.Windows.Forms.GroupBox gbDayInterval;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblHourFrom;
        private System.Windows.Forms.DateTimePicker dtpHoursFrom;
        private System.Windows.Forms.DateTimePicker dtpHoursTo;
        private System.Windows.Forms.Label lblHourTo;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.GroupBox gbDayOfWeek;
        private System.Windows.Forms.DateTimePicker dtpDayOfWeek;
        private System.Windows.Forms.Label lblDateToDayOfWeek;
        private System.Windows.Forms.DateTimePicker dtpDateToDayOfWeek;
        private System.Windows.Forms.Label lblDayOfWeek;
        private System.Windows.Forms.Label lblDateFromDayOfWeek;
        private System.Windows.Forms.DateTimePicker dtpDateFromDayOfWeek;
        private System.Windows.Forms.TextBox tbDayOfWeek;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.Label lblGraphName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
    }
}