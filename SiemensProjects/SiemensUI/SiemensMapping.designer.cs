namespace SiemensUI
{
    partial class SiemensMapping
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
            this.btnSave = new System.Windows.Forms.Button();
            this.panelDevices = new System.Windows.Forms.Panel();
            this.gbLegend = new System.Windows.Forms.GroupBox();
            this.lblNewPoint = new System.Windows.Forms.Label();
            this.btnNewPoint = new System.Windows.Forms.Button();
            this.lblDeletedPoint = new System.Windows.Forms.Label();
            this.btnDeletedPoint = new System.Windows.Forms.Button();
            this.lblNotTAPoint = new System.Windows.Forms.Label();
            this.btnNotTAPoint = new System.Windows.Forms.Button();
            this.lblTAPoints = new System.Windows.Forms.Label();
            this.btnTAPoints = new System.Windows.Forms.Button();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.lblFound = new System.Windows.Forms.Label();
            this.cbPoints = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblPointName = new System.Windows.Forms.Label();
            this.tbPointName = new System.Windows.Forms.TextBox();
            this.gbLegend.SuspendLayout();
            this.gbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(911, 594);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 594);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelDevices
            // 
            this.panelDevices.AutoScroll = true;
            this.panelDevices.Location = new System.Drawing.Point(12, 90);
            this.panelDevices.Name = "panelDevices";
            this.panelDevices.Size = new System.Drawing.Size(974, 498);
            this.panelDevices.TabIndex = 26;
            // 
            // gbLegend
            // 
            this.gbLegend.Controls.Add(this.lblNewPoint);
            this.gbLegend.Controls.Add(this.btnNewPoint);
            this.gbLegend.Controls.Add(this.lblDeletedPoint);
            this.gbLegend.Controls.Add(this.btnDeletedPoint);
            this.gbLegend.Controls.Add(this.lblNotTAPoint);
            this.gbLegend.Controls.Add(this.btnNotTAPoint);
            this.gbLegend.Controls.Add(this.lblTAPoints);
            this.gbLegend.Controls.Add(this.btnTAPoints);
            this.gbLegend.Location = new System.Drawing.Point(31, 12);
            this.gbLegend.Name = "gbLegend";
            this.gbLegend.Size = new System.Drawing.Size(353, 72);
            this.gbLegend.TabIndex = 27;
            this.gbLegend.TabStop = false;
            this.gbLegend.Text = "Legend";
            // 
            // lblNewPoint
            // 
            this.lblNewPoint.Location = new System.Drawing.Point(235, 44);
            this.lblNewPoint.Name = "lblNewPoint";
            this.lblNewPoint.Size = new System.Drawing.Size(115, 23);
            this.lblNewPoint.TabIndex = 7;
            this.lblNewPoint.Text = "New point";
            this.lblNewPoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNewPoint
            // 
            this.btnNewPoint.BackColor = System.Drawing.Color.LightPink;
            this.btnNewPoint.Location = new System.Drawing.Point(197, 45);
            this.btnNewPoint.Name = "btnNewPoint";
            this.btnNewPoint.Size = new System.Drawing.Size(32, 20);
            this.btnNewPoint.TabIndex = 6;
            this.btnNewPoint.UseVisualStyleBackColor = false;
            this.btnNewPoint.Click += new System.EventHandler(this.btnNewPoint_Click);
            // 
            // lblDeletedPoint
            // 
            this.lblDeletedPoint.Location = new System.Drawing.Point(235, 15);
            this.lblDeletedPoint.Name = "lblDeletedPoint";
            this.lblDeletedPoint.Size = new System.Drawing.Size(115, 23);
            this.lblDeletedPoint.TabIndex = 5;
            this.lblDeletedPoint.Text = "Deleted";
            this.lblDeletedPoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDeletedPoint
            // 
            this.btnDeletedPoint.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeletedPoint.Location = new System.Drawing.Point(197, 16);
            this.btnDeletedPoint.Name = "btnDeletedPoint";
            this.btnDeletedPoint.Size = new System.Drawing.Size(32, 20);
            this.btnDeletedPoint.TabIndex = 4;
            this.btnDeletedPoint.UseVisualStyleBackColor = false;
            this.btnDeletedPoint.Click += new System.EventHandler(this.btnDeletedPoint_Click);
            // 
            // lblNotTAPoint
            // 
            this.lblNotTAPoint.Location = new System.Drawing.Point(44, 38);
            this.lblNotTAPoint.Name = "lblNotTAPoint";
            this.lblNotTAPoint.Size = new System.Drawing.Size(147, 29);
            this.lblNotTAPoint.TabIndex = 3;
            this.lblNotTAPoint.Text = "NOT Reading passes from device";
            this.lblNotTAPoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNotTAPoint
            // 
            this.btnNotTAPoint.BackColor = System.Drawing.Color.LightBlue;
            this.btnNotTAPoint.Location = new System.Drawing.Point(6, 45);
            this.btnNotTAPoint.Name = "btnNotTAPoint";
            this.btnNotTAPoint.Size = new System.Drawing.Size(32, 20);
            this.btnNotTAPoint.TabIndex = 2;
            this.btnNotTAPoint.UseVisualStyleBackColor = false;
            this.btnNotTAPoint.Click += new System.EventHandler(this.btnNotTAPoint_Click);
            // 
            // lblTAPoints
            // 
            this.lblTAPoints.Location = new System.Drawing.Point(44, 15);
            this.lblTAPoints.Name = "lblTAPoints";
            this.lblTAPoints.Size = new System.Drawing.Size(147, 23);
            this.lblTAPoints.TabIndex = 1;
            this.lblTAPoints.Text = "Reading passes from device";
            this.lblTAPoints.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTAPoints
            // 
            this.btnTAPoints.BackColor = System.Drawing.Color.LightGreen;
            this.btnTAPoints.Location = new System.Drawing.Point(6, 16);
            this.btnTAPoints.Name = "btnTAPoints";
            this.btnTAPoints.Size = new System.Drawing.Size(32, 20);
            this.btnTAPoints.TabIndex = 0;
            this.btnTAPoints.UseVisualStyleBackColor = false;
            this.btnTAPoints.Click += new System.EventHandler(this.btnTAPoints_Click);
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.lblFound);
            this.gbSearch.Controls.Add(this.cbPoints);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.lblPointName);
            this.gbSearch.Controls.Add(this.tbPointName);
            this.gbSearch.Location = new System.Drawing.Point(408, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(543, 72);
            this.gbSearch.TabIndex = 28;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search";
            // 
            // lblFound
            // 
            this.lblFound.Location = new System.Drawing.Point(344, 12);
            this.lblFound.Name = "lblFound";
            this.lblFound.Size = new System.Drawing.Size(76, 16);
            this.lblFound.TabIndex = 28;
            this.lblFound.Text = "Found:";
            this.lblFound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbPoints
            // 
            this.cbPoints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPoints.FormattingEnabled = true;
            this.cbPoints.Location = new System.Drawing.Point(347, 32);
            this.cbPoints.Name = "cbPoints";
            this.cbPoints.Size = new System.Drawing.Size(176, 21);
            this.cbPoints.TabIndex = 27;
            this.cbPoints.SelectedIndexChanged += new System.EventHandler(this.cbPoints_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(256, 30);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 26;
            this.btnSearch.Text = "Search ->";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblPointName
            // 
            this.lblPointName.Location = new System.Drawing.Point(6, 30);
            this.lblPointName.Name = "lblPointName";
            this.lblPointName.Size = new System.Drawing.Size(76, 23);
            this.lblPointName.TabIndex = 8;
            this.lblPointName.Text = "Part of name:";
            this.lblPointName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPointName
            // 
            this.tbPointName.Location = new System.Drawing.Point(88, 32);
            this.tbPointName.Name = "tbPointName";
            this.tbPointName.Size = new System.Drawing.Size(151, 20);
            this.tbPointName.TabIndex = 0;
            // 
            // SiemensMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 621);
            this.ControlBox = false;
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.gbLegend);
            this.Controls.Add(this.panelDevices);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SiemensMapping";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Mapping";
            this.Load += new System.EventHandler(this.SiemensMapping_Load);
            this.gbLegend.ResumeLayout(false);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panelDevices;
        private System.Windows.Forms.GroupBox gbLegend;
        private System.Windows.Forms.Button btnTAPoints;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.Label lblNotTAPoint;
        private System.Windows.Forms.Button btnNotTAPoint;
        private System.Windows.Forms.Label lblTAPoints;
        private System.Windows.Forms.Label lblNewPoint;
        private System.Windows.Forms.Button btnNewPoint;
        private System.Windows.Forms.Label lblDeletedPoint;
        private System.Windows.Forms.Button btnDeletedPoint;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblPointName;
        private System.Windows.Forms.TextBox tbPointName;
        private System.Windows.Forms.ComboBox cbPoints;
        private System.Windows.Forms.Label lblFound;
    }
}