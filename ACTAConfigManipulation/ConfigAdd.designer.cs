namespace ACTAConfigManipulation
{
    partial class ConfigAdd
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGates = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblReaders = new System.Windows.Forms.Label();
            this.lblAuxPort = new System.Windows.Forms.Label();
            this.lblGateDescription = new System.Windows.Forms.Label();
            this.lblGateName = new System.Windows.Forms.Label();
            this.lblGateID = new System.Windows.Forms.Label();
            this.btnGatesSave = new System.Windows.Forms.Button();
            this.tabPageVisitors = new System.Windows.Forms.TabPage();
            this.gbOfflineVisits = new System.Windows.Forms.GroupBox();
            this.lblOfflineVisits = new System.Windows.Forms.Label();
            this.cbOfflineViists = new System.Windows.Forms.ComboBox();
            this.gbUpdateEmplLoc = new System.Windows.Forms.GroupBox();
            this.lblUpdateEmplLoc = new System.Windows.Forms.Label();
            this.cbUpdEmplLoc = new System.Windows.Forms.ComboBox();
            this.gbVisitorsCode = new System.Windows.Forms.GroupBox();
            this.lblWU = new System.Windows.Forms.Label();
            this.cbWU = new System.Windows.Forms.ComboBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.btnVisitorsSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageGates.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageVisitors.SuspendLayout();
            this.gbOfflineVisits.SuspendLayout();
            this.gbUpdateEmplLoc.SuspendLayout();
            this.gbVisitorsCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGates);
            this.tabControl1.Controls.Add(this.tabPageVisitors);
            this.tabControl1.Location = new System.Drawing.Point(-3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(997, 628);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGates
            // 
            this.tabPageGates.Controls.Add(this.panel1);
            this.tabPageGates.Controls.Add(this.btnGatesSave);
            this.tabPageGates.Location = new System.Drawing.Point(4, 22);
            this.tabPageGates.Name = "tabPageGates";
            this.tabPageGates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGates.Size = new System.Drawing.Size(989, 602);
            this.tabPageGates.TabIndex = 0;
            this.tabPageGates.Text = "Gates";
            this.tabPageGates.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.lblReaders);
            this.panel1.Controls.Add(this.lblAuxPort);
            this.panel1.Controls.Add(this.lblGateDescription);
            this.panel1.Controls.Add(this.lblGateName);
            this.panel1.Controls.Add(this.lblGateID);
            this.panel1.Location = new System.Drawing.Point(5, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 518);
            this.panel1.TabIndex = 1;
            // 
            // lblReaders
            // 
            this.lblReaders.BackColor = System.Drawing.Color.LightYellow;
            this.lblReaders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReaders.Location = new System.Drawing.Point(330, 5);
            this.lblReaders.Name = "lblReaders";
            this.lblReaders.Size = new System.Drawing.Size(123, 15);
            this.lblReaders.TabIndex = 4;
            this.lblReaders.Text = "Reader/Aux Ports";
            // 
            // lblAuxPort
            // 
            this.lblAuxPort.BackColor = System.Drawing.Color.LightYellow;
            this.lblAuxPort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAuxPort.Location = new System.Drawing.Point(280, 5);
            this.lblAuxPort.Name = "lblAuxPort";
            this.lblAuxPort.Size = new System.Drawing.Size(50, 15);
            this.lblAuxPort.TabIndex = 3;
            this.lblAuxPort.Text = "Aux Port";
            this.lblAuxPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGateDescription
            // 
            this.lblGateDescription.BackColor = System.Drawing.Color.LightYellow;
            this.lblGateDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGateDescription.Location = new System.Drawing.Point(180, 5);
            this.lblGateDescription.Name = "lblGateDescription";
            this.lblGateDescription.Size = new System.Drawing.Size(100, 15);
            this.lblGateDescription.TabIndex = 2;
            this.lblGateDescription.Text = "Description";
            this.lblGateDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGateName
            // 
            this.lblGateName.BackColor = System.Drawing.Color.LightYellow;
            this.lblGateName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGateName.Location = new System.Drawing.Point(80, 5);
            this.lblGateName.Name = "lblGateName";
            this.lblGateName.Size = new System.Drawing.Size(100, 15);
            this.lblGateName.TabIndex = 1;
            this.lblGateName.Text = "Name";
            this.lblGateName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGateID
            // 
            this.lblGateID.BackColor = System.Drawing.Color.LightYellow;
            this.lblGateID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblGateID.Location = new System.Drawing.Point(35, 5);
            this.lblGateID.Name = "lblGateID";
            this.lblGateID.Size = new System.Drawing.Size(45, 15);
            this.lblGateID.TabIndex = 0;
            this.lblGateID.Text = "ID";
            this.lblGateID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGatesSave
            // 
            this.btnGatesSave.Location = new System.Drawing.Point(11, 560);
            this.btnGatesSave.Name = "btnGatesSave";
            this.btnGatesSave.Size = new System.Drawing.Size(75, 23);
            this.btnGatesSave.TabIndex = 0;
            this.btnGatesSave.Text = "Save";
            this.btnGatesSave.UseVisualStyleBackColor = true;
            this.btnGatesSave.Click += new System.EventHandler(this.btnGatesSave_Click);
            // 
            // tabPageVisitors
            // 
            this.tabPageVisitors.Controls.Add(this.gbOfflineVisits);
            this.tabPageVisitors.Controls.Add(this.gbUpdateEmplLoc);
            this.tabPageVisitors.Controls.Add(this.gbVisitorsCode);
            this.tabPageVisitors.Controls.Add(this.btnVisitorsSave);
            this.tabPageVisitors.Location = new System.Drawing.Point(4, 22);
            this.tabPageVisitors.Name = "tabPageVisitors";
            this.tabPageVisitors.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVisitors.Size = new System.Drawing.Size(989, 602);
            this.tabPageVisitors.TabIndex = 1;
            this.tabPageVisitors.Text = "Visitors";
            this.tabPageVisitors.UseVisualStyleBackColor = true;
            // 
            // gbOfflineVisits
            // 
            this.gbOfflineVisits.Controls.Add(this.lblOfflineVisits);
            this.gbOfflineVisits.Controls.Add(this.cbOfflineViists);
            this.gbOfflineVisits.Location = new System.Drawing.Point(38, 343);
            this.gbOfflineVisits.Name = "gbOfflineVisits";
            this.gbOfflineVisits.Size = new System.Drawing.Size(366, 100);
            this.gbOfflineVisits.TabIndex = 8;
            this.gbOfflineVisits.TabStop = false;
            this.gbOfflineVisits.Text = "Offline Visits";
            // 
            // lblOfflineVisits
            // 
            this.lblOfflineVisits.AutoSize = true;
            this.lblOfflineVisits.Location = new System.Drawing.Point(32, 29);
            this.lblOfflineVisits.Name = "lblOfflineVisits";
            this.lblOfflineVisits.Size = new System.Drawing.Size(66, 13);
            this.lblOfflineVisits.TabIndex = 9;
            this.lblOfflineVisits.Text = "Offline visits:";
            this.lblOfflineVisits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbOfflineViists
            // 
            this.cbOfflineViists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOfflineViists.FormattingEnabled = true;
            this.cbOfflineViists.Location = new System.Drawing.Point(124, 26);
            this.cbOfflineViists.Name = "cbOfflineViists";
            this.cbOfflineViists.Size = new System.Drawing.Size(73, 21);
            this.cbOfflineViists.TabIndex = 10;
            // 
            // gbUpdateEmplLoc
            // 
            this.gbUpdateEmplLoc.Controls.Add(this.lblUpdateEmplLoc);
            this.gbUpdateEmplLoc.Controls.Add(this.cbUpdEmplLoc);
            this.gbUpdateEmplLoc.Location = new System.Drawing.Point(38, 199);
            this.gbUpdateEmplLoc.Name = "gbUpdateEmplLoc";
            this.gbUpdateEmplLoc.Size = new System.Drawing.Size(366, 100);
            this.gbUpdateEmplLoc.TabIndex = 5;
            this.gbUpdateEmplLoc.TabStop = false;
            this.gbUpdateEmplLoc.Text = "Update Employee Location";
            // 
            // lblUpdateEmplLoc
            // 
            this.lblUpdateEmplLoc.AutoSize = true;
            this.lblUpdateEmplLoc.Location = new System.Drawing.Point(32, 29);
            this.lblUpdateEmplLoc.Name = "lblUpdateEmplLoc";
            this.lblUpdateEmplLoc.Size = new System.Drawing.Size(138, 13);
            this.lblUpdateEmplLoc.TabIndex = 6;
            this.lblUpdateEmplLoc.Text = "Update Employee Location:";
            this.lblUpdateEmplLoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbUpdEmplLoc
            // 
            this.cbUpdEmplLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUpdEmplLoc.FormattingEnabled = true;
            this.cbUpdEmplLoc.Location = new System.Drawing.Point(198, 26);
            this.cbUpdEmplLoc.Name = "cbUpdEmplLoc";
            this.cbUpdEmplLoc.Size = new System.Drawing.Size(73, 21);
            this.cbUpdEmplLoc.TabIndex = 7;
            // 
            // gbVisitorsCode
            // 
            this.gbVisitorsCode.Controls.Add(this.lblWU);
            this.gbVisitorsCode.Controls.Add(this.cbWU);
            this.gbVisitorsCode.Controls.Add(this.lblDesc);
            this.gbVisitorsCode.Controls.Add(this.tbDesc);
            this.gbVisitorsCode.Location = new System.Drawing.Point(38, 23);
            this.gbVisitorsCode.Name = "gbVisitorsCode";
            this.gbVisitorsCode.Size = new System.Drawing.Size(366, 143);
            this.gbVisitorsCode.TabIndex = 0;
            this.gbVisitorsCode.TabStop = false;
            this.gbVisitorsCode.Text = "Visitors Code";
            // 
            // lblWU
            // 
            this.lblWU.AutoSize = true;
            this.lblWU.Location = new System.Drawing.Point(32, 36);
            this.lblWU.Name = "lblWU";
            this.lblWU.Size = new System.Drawing.Size(72, 13);
            this.lblWU.TabIndex = 1;
            this.lblWU.Text = "Working Unit:";
            this.lblWU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbWU
            // 
            this.cbWU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWU.FormattingEnabled = true;
            this.cbWU.Location = new System.Drawing.Point(124, 33);
            this.cbWU.Name = "cbWU";
            this.cbWU.Size = new System.Drawing.Size(174, 21);
            this.cbWU.TabIndex = 2;
            this.cbWU.SelectedIndexChanged += new System.EventHandler(this.cbWU_SelectedIndexChanged);
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(32, 88);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(63, 13);
            this.lblDesc.TabIndex = 3;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(124, 85);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.ReadOnly = true;
            this.tbDesc.Size = new System.Drawing.Size(174, 20);
            this.tbDesc.TabIndex = 4;
            // 
            // btnVisitorsSave
            // 
            this.btnVisitorsSave.Location = new System.Drawing.Point(14, 558);
            this.btnVisitorsSave.Name = "btnVisitorsSave";
            this.btnVisitorsSave.Size = new System.Drawing.Size(75, 23);
            this.btnVisitorsSave.TabIndex = 11;
            this.btnVisitorsSave.Text = "Save";
            this.btnVisitorsSave.UseVisualStyleBackColor = true;
            this.btnVisitorsSave.Click += new System.EventHandler(this.btnVisitorsSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(905, 638);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ConfigAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 673);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.MaximumSize = new System.Drawing.Size(1000, 700);
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "ConfigAdd";
            this.ShowInTaskbar = false;
            this.Text = "ConfigAdd";
            this.Load += new System.EventHandler(this.ConfigAdd_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGates.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabPageVisitors.ResumeLayout(false);
            this.gbOfflineVisits.ResumeLayout(false);
            this.gbOfflineVisits.PerformLayout();
            this.gbUpdateEmplLoc.ResumeLayout(false);
            this.gbUpdateEmplLoc.PerformLayout();
            this.gbVisitorsCode.ResumeLayout(false);
            this.gbVisitorsCode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGates;
        private System.Windows.Forms.TabPage tabPageVisitors;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnGatesSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblReaders;
        private System.Windows.Forms.Label lblAuxPort;
        private System.Windows.Forms.Label lblGateDescription;
        private System.Windows.Forms.Label lblGateName;
        private System.Windows.Forms.Label lblGateID;
        private System.Windows.Forms.Button btnVisitorsSave;
        private System.Windows.Forms.ComboBox cbUpdEmplLoc;
        private System.Windows.Forms.Label lblUpdateEmplLoc;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.ComboBox cbWU;
        private System.Windows.Forms.Label lblWU;
        private System.Windows.Forms.GroupBox gbUpdateEmplLoc;
        private System.Windows.Forms.GroupBox gbVisitorsCode;
        private System.Windows.Forms.GroupBox gbOfflineVisits;
        private System.Windows.Forms.Label lblOfflineVisits;
        private System.Windows.Forms.ComboBox cbOfflineViists;
    }
}