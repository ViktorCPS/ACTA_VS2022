namespace UI
{
    partial class MapObjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapObjects));
            this.cbMap = new System.Windows.Forms.ComboBox();
            this.lblMap = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lblObjectType = new System.Windows.Forms.Label();
            this.tbObjectType = new System.Windows.Forms.TextBox();
            this.lblPosition = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvObjects = new System.Windows.Forms.ListView();
            this.btnSave = new System.Windows.Forms.Button();
            this.lvPoints = new System.Windows.Forms.ListView();
            this.cmsRemove = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClearLast = new System.Windows.Forms.Button();
            this.mapControl1 = new UI.MapControl();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.cmsRemove.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbMap
            // 
            this.cbMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMap.FormattingEnabled = true;
            this.cbMap.Location = new System.Drawing.Point(118, 11);
            this.cbMap.Name = "cbMap";
            this.cbMap.Size = new System.Drawing.Size(168, 21);
            this.cbMap.TabIndex = 7;
            this.cbMap.SelectedIndexChanged += new System.EventHandler(this.cbMap_SelectedIndexChanged);
            // 
            // lblMap
            // 
            this.lblMap.Location = new System.Drawing.Point(12, 9);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(100, 23);
            this.lblMap.TabIndex = 6;
            this.lblMap.Text = "Map:";
            this.lblMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMode
            // 
            this.lblMode.Location = new System.Drawing.Point(362, 9);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(100, 23);
            this.lblMode.TabIndex = 8;
            this.lblMode.Text = "Mode:";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(468, 11);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(282, 21);
            this.cbMode.TabIndex = 9;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 247);
            this.trackBar1.Maximum = 3;
            this.trackBar1.Minimum = -3;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 155);
            this.trackBar1.TabIndex = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // lblObjectType
            // 
            this.lblObjectType.Location = new System.Drawing.Point(769, 73);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(87, 23);
            this.lblObjectType.TabIndex = 12;
            this.lblObjectType.Text = "Object type:";
            this.lblObjectType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbObjectType
            // 
            this.tbObjectType.Location = new System.Drawing.Point(862, 76);
            this.tbObjectType.Name = "tbObjectType";
            this.tbObjectType.Size = new System.Drawing.Size(130, 20);
            this.tbObjectType.TabIndex = 13;
            // 
            // lblPosition
            // 
            this.lblPosition.Location = new System.Drawing.Point(766, 326);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(100, 23);
            this.lblPosition.TabIndex = 14;
            this.lblPosition.Text = "Position:";
            this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(917, 637);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvObjects
            // 
            this.lvObjects.FullRowSelect = true;
            this.lvObjects.GridLines = true;
            this.lvObjects.HideSelection = false;
            this.lvObjects.Location = new System.Drawing.Point(769, 102);
            this.lvObjects.MultiSelect = false;
            this.lvObjects.Name = "lvObjects";
            this.lvObjects.Size = new System.Drawing.Size(223, 195);
            this.lvObjects.TabIndex = 17;
            this.lvObjects.UseCompatibleStateImageBehavior = false;
            this.lvObjects.View = System.Windows.Forms.View.Details;
            this.lvObjects.SelectedIndexChanged += new System.EventHandler(this.lvObjects_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(769, 637);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lvPoints
            // 
            this.lvPoints.Enabled = false;
            this.lvPoints.FullRowSelect = true;
            this.lvPoints.GridLines = true;
            this.lvPoints.HideSelection = false;
            this.lvPoints.Location = new System.Drawing.Point(769, 352);
            this.lvPoints.MultiSelect = false;
            this.lvPoints.Name = "lvPoints";
            this.lvPoints.Size = new System.Drawing.Size(223, 179);
            this.lvPoints.TabIndex = 19;
            this.lvPoints.UseCompatibleStateImageBehavior = false;
            this.lvPoints.View = System.Windows.Forms.View.Details;
            // 
            // cmsRemove
            // 
            this.cmsRemove.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.cmsRemove.Name = "cmsRemove";
            this.cmsRemove.Size = new System.Drawing.Size(125, 26);
            this.cmsRemove.MouseLeave += new System.EventHandler(this.cmsRemove_MouseLeave);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 317);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "zoom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "+";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(43, 382);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 24);
            this.label3.TabIndex = 22;
            this.label3.Text = "-";
            // 
            // btnClearLast
            // 
            this.btnClearLast.Location = new System.Drawing.Point(769, 547);
            this.btnClearLast.Name = "btnClearLast";
            this.btnClearLast.Size = new System.Drawing.Size(133, 23);
            this.btnClearLast.TabIndex = 23;
            this.btnClearLast.Text = "Clear last";
            this.btnClearLast.UseVisualStyleBackColor = true;
            this.btnClearLast.Click += new System.EventHandler(this.btnClearLast_Click);
            // 
            // mapControl1
            // 
            this.mapControl1.AutoScroll = true;
            this.mapControl1.Image = null;
            this.mapControl1.Location = new System.Drawing.Point(83, 56);
            this.mapControl1.Name = "mapControl1";
            //this.mapControl1.PathsArray = (List<LocationTO>)(resources.GetObject("mapControl1.PathsArray")));
            this.mapControl1.PrevPictureBoxHeight = 0;
            this.mapControl1.PrevPictureBoxWidth = 0;
            this.mapControl1.Size = new System.Drawing.Size(667, 599);
            this.mapControl1.TabIndex = 11;
            this.mapControl1.RightClick += new UI.MapControl.RightClickDelegate(this.rightClickOnObject);
            this.mapControl1.PictureClick += new UI.MapControl.PictureClickDelegate(this.AddingObject);
            // 
            // MapObjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 672);
            this.ControlBox = false;
            this.Controls.Add(this.btnClearLast);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvPoints);
            this.Controls.Add(this.lvObjects);
            this.Controls.Add(this.tbObjectType);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.mapControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblObjectType);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.cbMap);
            this.Controls.Add(this.lblMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "MapObjects";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MapObjects";
            this.Load += new System.EventHandler(this.MapObjects_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MapObjects_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.cmsRemove.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMap;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.TrackBar trackBar1;
        private MapControl mapControl1;
        private System.Windows.Forms.Label lblObjectType;
        private System.Windows.Forms.TextBox tbObjectType;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvObjects;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListView lvPoints;
        private System.Windows.Forms.ContextMenuStrip cmsRemove;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClearLast;
    }
}