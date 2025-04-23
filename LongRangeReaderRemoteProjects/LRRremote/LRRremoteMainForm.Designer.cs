namespace LRRremote
{
    partial class LRRremoteMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LRRremoteMainForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.nudDebounceInterval = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.infoLine = new System.Windows.Forms.ColumnHeader();
            this.lvReadResults = new System.Windows.Forms.ListView();
            this.rNo = new System.Windows.Forms.ColumnHeader();
            this.Time = new System.Windows.Forms.ColumnHeader();
            this.Lane1 = new System.Windows.Forms.ColumnHeader();
            this.Lane2 = new System.Windows.Forms.ColumnHeader();
            this.pTime = new System.Windows.Forms.ColumnHeader();
            this.Linc = new System.Windows.Forms.ColumnHeader();
            this.L2Linc = new System.Windows.Forms.ColumnHeader();
            this.fullName = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.nudPairInterval = new System.Windows.Forms.NumericUpDown();
            this.lblNoPasses = new System.Windows.Forms.Label();
            this.tbPairs = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.resetTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudDebounceInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPairInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(418, 617);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(129, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // nudDebounceInterval
            // 
            this.nudDebounceInterval.Location = new System.Drawing.Point(884, 620);
            this.nudDebounceInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudDebounceInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDebounceInterval.Name = "nudDebounceInterval";
            this.nudDebounceInterval.Size = new System.Drawing.Size(55, 20);
            this.nudDebounceInterval.TabIndex = 3;
            this.nudDebounceInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudDebounceInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(784, 624);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Debounce interval";
            // 
            // lvReadResults
            // 
            this.lvReadResults.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvReadResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.rNo,
            this.Time,
            this.Lane1,
            this.Lane2,
            this.pTime,
            this.Linc,
            this.L2Linc,
            this.fullName});
            this.lvReadResults.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvReadResults.FullRowSelect = true;
            this.lvReadResults.GridLines = true;
            this.lvReadResults.HideSelection = false;
            this.lvReadResults.HotTracking = true;
            this.lvReadResults.HoverSelection = true;
            this.lvReadResults.Location = new System.Drawing.Point(29, 12);
            this.lvReadResults.Name = "lvReadResults";
            this.lvReadResults.Size = new System.Drawing.Size(911, 589);
            this.lvReadResults.TabIndex = 5;
            this.lvReadResults.UseCompatibleStateImageBehavior = false;
            this.lvReadResults.View = System.Windows.Forms.View.Details;
            // 
            // rNo
            // 
            this.rNo.Text = "rNo";
            this.rNo.Width = 57;
            // 
            // Time
            // 
            this.Time.Text = "Time";
            this.Time.Width = 126;
            // 
            // Lane1
            // 
            this.Lane1.Text = "Lane 1";
            this.Lane1.Width = 135;
            // 
            // Lane2
            // 
            this.Lane2.Text = "Lane 2";
            this.Lane2.Width = 135;
            // 
            // pTime
            // 
            this.pTime.Text = "Pair Time";
            this.pTime.Width = 81;
            // 
            // Linc
            // 
            this.Linc.Text = "1Linc";
            this.Linc.Width = 48;
            // 
            // L2Linc
            // 
            this.L2Linc.Text = "2Linc";
            this.L2Linc.Width = 48;
            // 
            // fullName
            // 
            this.fullName.Text = "Name";
            this.fullName.Width = 250;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(784, 656);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Pair interval";
            // 
            // nudPairInterval
            // 
            this.nudPairInterval.Location = new System.Drawing.Point(884, 652);
            this.nudPairInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudPairInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPairInterval.Name = "nudPairInterval";
            this.nudPairInterval.Size = new System.Drawing.Size(55, 20);
            this.nudPairInterval.TabIndex = 6;
            this.nudPairInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPairInterval.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblNoPasses
            // 
            this.lblNoPasses.AutoSize = true;
            this.lblNoPasses.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNoPasses.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoPasses.Location = new System.Drawing.Point(31, 608);
            this.lblNoPasses.Name = "lblNoPasses";
            this.lblNoPasses.Size = new System.Drawing.Size(52, 18);
            this.lblNoPasses.TabIndex = 8;
            this.lblNoPasses.Text = "000000";
            // 
            // tbPairs
            // 
            this.tbPairs.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPairs.Location = new System.Drawing.Point(955, 36);
            this.tbPairs.Multiline = true;
            this.tbPairs.Name = "tbPairs";
            this.tbPairs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbPairs.Size = new System.Drawing.Size(597, 565);
            this.tbPairs.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(956, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(577, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "                                      Pairs                                      " +
                "";
            // 
            // resetTimer
            // 
            this.resetTimer.Interval = 1000;
            this.resetTimer.Tick += new System.EventHandler(this.resetTimer_Tick);
            // 
            // LRRremoteMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1558, 678);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPairs);
            this.Controls.Add(this.lblNoPasses);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudPairInterval);
            this.Controls.Add(this.lvReadResults);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudDebounceInterval);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LRRremoteMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LRRremote";
            this.Load += new System.EventHandler(this.LRRremoteMainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LRRremoteMainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudDebounceInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPairInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NumericUpDown nudDebounceInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader infoLine;
        private System.Windows.Forms.ListView lvReadResults;
        private System.Windows.Forms.ColumnHeader rNo;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.ColumnHeader Lane1;
        private System.Windows.Forms.ColumnHeader L2Linc;
        private System.Windows.Forms.ColumnHeader Lane2;
        private System.Windows.Forms.ColumnHeader Linc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudPairInterval;
        private System.Windows.Forms.Label lblNoPasses;
        private System.Windows.Forms.ColumnHeader pTime;
        private System.Windows.Forms.TextBox tbPairs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader fullName;
        private System.Windows.Forms.Timer resetTimer;
    }
}

