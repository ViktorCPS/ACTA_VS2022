namespace UI
{
    partial class SecurityRoutesAdd
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
            this.gbAddRoute = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gbRoutes = new System.Windows.Forms.GroupBox();
            this.lvRoutes = new System.Windows.Forms.ListView();
            this.btnRemove = new System.Windows.Forms.Button();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.numNextVisit = new System.Windows.Forms.NumericUpDown();
            this.lblMin = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblNextVisit = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.gbGate = new System.Windows.Forms.GroupBox();
            this.lvGates = new System.Windows.Forms.ListView();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbAddRoute.SuspendLayout();
            this.gbRoutes.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNextVisit)).BeginInit();
            this.gbGate.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(571, 414);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbAddRoute
            // 
            this.gbAddRoute.Controls.Add(this.label1);
            this.gbAddRoute.Controls.Add(this.btnAdd);
            this.gbAddRoute.Controls.Add(this.gbRoutes);
            this.gbAddRoute.Controls.Add(this.gbTimeInterval);
            this.gbAddRoute.Controls.Add(this.gbGate);
            this.gbAddRoute.Controls.Add(this.tbDesc);
            this.gbAddRoute.Controls.Add(this.tbName);
            this.gbAddRoute.Controls.Add(this.lblDesc);
            this.gbAddRoute.Controls.Add(this.lblName);
            this.gbAddRoute.Location = new System.Drawing.Point(12, 12);
            this.gbAddRoute.Name = "gbAddRoute";
            this.gbAddRoute.Size = new System.Drawing.Size(634, 396);
            this.gbAddRoute.TabIndex = 0;
            this.gbAddRoute.TabStop = false;
            this.gbAddRoute.Text = "Add new route";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(248, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(300, 206);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(32, 23);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gbRoutes
            // 
            this.gbRoutes.Controls.Add(this.lvRoutes);
            this.gbRoutes.Controls.Add(this.btnRemove);
            this.gbRoutes.Location = new System.Drawing.Point(338, 87);
            this.gbRoutes.Name = "gbRoutes";
            this.gbRoutes.Size = new System.Drawing.Size(281, 294);
            this.gbRoutes.TabIndex = 18;
            this.gbRoutes.TabStop = false;
            this.gbRoutes.Text = "Routes";
            // 
            // lvRoutes
            // 
            this.lvRoutes.FullRowSelect = true;
            this.lvRoutes.GridLines = true;
            this.lvRoutes.HideSelection = false;
            this.lvRoutes.Location = new System.Drawing.Point(6, 19);
            this.lvRoutes.Name = "lvRoutes";
            this.lvRoutes.Size = new System.Drawing.Size(269, 240);
            this.lvRoutes.TabIndex = 19;
            this.lvRoutes.UseCompatibleStateImageBehavior = false;
            this.lvRoutes.View = System.Windows.Forms.View.Details;
            this.lvRoutes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvRoutes_ColumnClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(6, 265);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(114, 23);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "Delete selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.numNextVisit);
            this.gbTimeInterval.Controls.Add(this.lblMin);
            this.gbTimeInterval.Controls.Add(this.dtpTo);
            this.gbTimeInterval.Controls.Add(this.dtpFrom);
            this.gbTimeInterval.Controls.Add(this.lblNextVisit);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(22, 299);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(272, 82);
            this.gbTimeInterval.TabIndex = 8;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time interval";
            // 
            // numNextVisit
            // 
            this.numNextVisit.Location = new System.Drawing.Point(186, 49);
            this.numNextVisit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numNextVisit.Name = "numNextVisit";
            this.numNextVisit.Size = new System.Drawing.Size(49, 20);
            this.numNextVisit.TabIndex = 14;
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new System.Drawing.Point(241, 51);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(23, 13);
            this.lblMin.TabIndex = 15;
            this.lblMin.Text = "min";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(186, 19);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowUpDown = true;
            this.dtpTo.Size = new System.Drawing.Size(75, 20);
            this.dtpTo.TabIndex = 12;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(61, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowUpDown = true;
            this.dtpFrom.Size = new System.Drawing.Size(75, 20);
            this.dtpFrom.TabIndex = 10;
            // 
            // lblNextVisit
            // 
            this.lblNextVisit.Location = new System.Drawing.Point(18, 51);
            this.lblNextVisit.Name = "lblNextVisit";
            this.lblNextVisit.Size = new System.Drawing.Size(162, 13);
            this.lblNextVisit.TabIndex = 13;
            this.lblNextVisit.Text = "Next visit:";
            this.lblNextVisit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(145, 23);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(35, 13);
            this.lblTo.TabIndex = 11;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(9, 19);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(44, 20);
            this.lblFrom.TabIndex = 9;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbGate
            // 
            this.gbGate.Controls.Add(this.lvGates);
            this.gbGate.Location = new System.Drawing.Point(22, 87);
            this.gbGate.Name = "gbGate";
            this.gbGate.Size = new System.Drawing.Size(272, 206);
            this.gbGate.TabIndex = 6;
            this.gbGate.TabStop = false;
            this.gbGate.Text = "Gate";
            // 
            // lvGates
            // 
            this.lvGates.FullRowSelect = true;
            this.lvGates.GridLines = true;
            this.lvGates.HideSelection = false;
            this.lvGates.Location = new System.Drawing.Point(6, 19);
            this.lvGates.MultiSelect = false;
            this.lvGates.Name = "lvGates";
            this.lvGates.ShowItemToolTips = true;
            this.lvGates.Size = new System.Drawing.Size(260, 181);
            this.lvGates.TabIndex = 7;
            this.lvGates.UseCompatibleStateImageBehavior = false;
            this.lvGates.View = System.Windows.Forms.View.Details;
            this.lvGates.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGates_ColumnClick);
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(97, 52);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(146, 20);
            this.tbDesc.TabIndex = 5;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(97, 23);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(146, 20);
            this.tbName.TabIndex = 2;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(19, 55);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(72, 13);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(19, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(72, 17);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 414);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SecurityRoutesAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.KeyPreview = true;
            this.ClientSize = new System.Drawing.Size(658, 449);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbAddRoute);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SecurityRoutesAdd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "SecurityRoutesAdd";
            this.Load += new System.EventHandler(this.SecurityRoutesAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SecurityRoutesAdd_KeyUp);
            this.gbAddRoute.ResumeLayout(false);
            this.gbAddRoute.PerformLayout();
            this.gbRoutes.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbTimeInterval.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNextVisit)).EndInit();
            this.gbGate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbAddRoute;
        private System.Windows.Forms.GroupBox gbGate;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbTimeInterval;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox gbRoutes;
        private System.Windows.Forms.Label lblNextVisit;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.ListView lvGates;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.NumericUpDown numNextVisit;
        private System.Windows.Forms.ListView lvRoutes;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label1;
    }
}