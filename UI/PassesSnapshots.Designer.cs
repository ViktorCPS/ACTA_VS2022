namespace UI
{
    partial class PassesSnapshots
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
            this.gbCurrentPass = new System.Windows.Forms.GroupBox();
            this.lblOfficialEmplPhoto = new System.Windows.Forms.Label();
            this.lblPhotoNum = new System.Windows.Forms.Label();
            this.nudOffset = new System.Windows.Forms.NumericUpDown();
            this.pbEmployee = new System.Windows.Forms.PictureBox();
            this.gbPass = new System.Windows.Forms.GroupBox();
            this.lblPassIsWrkHrsVal = new System.Windows.Forms.Label();
            this.lblPassTypeVal = new System.Windows.Forms.Label();
            this.lblPassLocationVal = new System.Windows.Forms.Label();
            this.lblPassDirectionVal = new System.Windows.Forms.Label();
            this.lblPassTimeVal = new System.Windows.Forms.Label();
            this.lblPassIsWrkHrs = new System.Windows.Forms.Label();
            this.lblPassType = new System.Windows.Forms.Label();
            this.lblPassLocation = new System.Windows.Forms.Label();
            this.lblPassDirection = new System.Windows.Forms.Label();
            this.lblPassTime = new System.Windows.Forms.Label();
            this.gbEmployee = new System.Windows.Forms.GroupBox();
            this.cbEmplWTVal = new System.Windows.Forms.ComboBox();
            this.lblEmplWUVal = new System.Windows.Forms.Label();
            this.lblEmplNameVal = new System.Windows.Forms.Label();
            this.lblEmplWT = new System.Windows.Forms.Label();
            this.lblEmplWU = new System.Windows.Forms.Label();
            this.lblEmplName = new System.Windows.Forms.Label();
            this.lblSec = new System.Windows.Forms.Label();
            this.lblOffset = new System.Windows.Forms.Label();
            this.gbPhotoNavigation = new System.Windows.Forms.GroupBox();
            this.btnPhotoFirst = new System.Windows.Forms.Button();
            this.btnPhotoPrev = new System.Windows.Forms.Button();
            this.btnPhotoNext = new System.Windows.Forms.Button();
            this.btnPhotoLast = new System.Windows.Forms.Button();
            this.gbPhotos = new System.Windows.Forms.GroupBox();
            this.lblFileCTime = new System.Windows.Forms.Label();
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.pb2 = new System.Windows.Forms.PictureBox();
            this.pb5 = new System.Windows.Forms.PictureBox();
            this.pb4 = new System.Windows.Forms.PictureBox();
            this.pb3 = new System.Windows.Forms.PictureBox();
            this.gbPassNavigation = new System.Windows.Forms.GroupBox();
            this.btnPassFirst = new System.Windows.Forms.Button();
            this.btnPassPrev = new System.Windows.Forms.Button();
            this.btnPassNext = new System.Windows.Forms.Button();
            this.btnPassLast = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblPassNum = new System.Windows.Forms.Label();
            this.gbCurrentPass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployee)).BeginInit();
            this.gbPass.SuspendLayout();
            this.gbEmployee.SuspendLayout();
            this.gbPhotoNavigation.SuspendLayout();
            this.gbPhotos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb3)).BeginInit();
            this.gbPassNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCurrentPass
            // 
            this.gbCurrentPass.Controls.Add(this.lblOfficialEmplPhoto);
            this.gbCurrentPass.Controls.Add(this.lblPhotoNum);
            this.gbCurrentPass.Controls.Add(this.nudOffset);
            this.gbCurrentPass.Controls.Add(this.pbEmployee);
            this.gbCurrentPass.Controls.Add(this.gbPass);
            this.gbCurrentPass.Controls.Add(this.gbEmployee);
            this.gbCurrentPass.Controls.Add(this.lblSec);
            this.gbCurrentPass.Controls.Add(this.lblOffset);
            this.gbCurrentPass.Controls.Add(this.gbPhotoNavigation);
            this.gbCurrentPass.Controls.Add(this.gbPhotos);
            this.gbCurrentPass.Location = new System.Drawing.Point(11, 8);
            this.gbCurrentPass.Name = "gbCurrentPass";
            this.gbCurrentPass.Size = new System.Drawing.Size(870, 546);
            this.gbCurrentPass.TabIndex = 1;
            this.gbCurrentPass.TabStop = false;
            this.gbCurrentPass.Text = "Current pass";
            // 
            // lblOfficialEmplPhoto
            // 
            this.lblOfficialEmplPhoto.Location = new System.Drawing.Point(275, 150);
            this.lblOfficialEmplPhoto.Name = "lblOfficialEmplPhoto";
            this.lblOfficialEmplPhoto.Size = new System.Drawing.Size(320, 23);
            this.lblOfficialEmplPhoto.TabIndex = 37;
            this.lblOfficialEmplPhoto.Text = "Official employee photo";
            this.lblOfficialEmplPhoto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPhotoNum
            // 
            this.lblPhotoNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhotoNum.Location = new System.Drawing.Point(654, 498);
            this.lblPhotoNum.Name = "lblPhotoNum";
            this.lblPhotoNum.Size = new System.Drawing.Size(70, 23);
            this.lblPhotoNum.TabIndex = 1;
            this.lblPhotoNum.Text = "i/n";
            this.lblPhotoNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudOffset
            // 
            this.nudOffset.BackColor = System.Drawing.SystemColors.Window;
            this.nudOffset.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudOffset.Location = new System.Drawing.Point(88, 501);
            this.nudOffset.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudOffset.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudOffset.Name = "nudOffset";
            this.nudOffset.ReadOnly = true;
            this.nudOffset.Size = new System.Drawing.Size(42, 20);
            this.nudOffset.TabIndex = 4;
            this.nudOffset.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // pbEmployee
            // 
            this.pbEmployee.Location = new System.Drawing.Point(390, 12);
            this.pbEmployee.Name = "pbEmployee";
            this.pbEmployee.Size = new System.Drawing.Size(90, 135);
            this.pbEmployee.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEmployee.TabIndex = 36;
            this.pbEmployee.TabStop = false;
            // 
            // gbPass
            // 
            this.gbPass.Controls.Add(this.lblPassIsWrkHrsVal);
            this.gbPass.Controls.Add(this.lblPassTypeVal);
            this.gbPass.Controls.Add(this.lblPassLocationVal);
            this.gbPass.Controls.Add(this.lblPassDirectionVal);
            this.gbPass.Controls.Add(this.lblPassTimeVal);
            this.gbPass.Controls.Add(this.lblPassIsWrkHrs);
            this.gbPass.Controls.Add(this.lblPassType);
            this.gbPass.Controls.Add(this.lblPassLocation);
            this.gbPass.Controls.Add(this.lblPassDirection);
            this.gbPass.Controls.Add(this.lblPassTime);
            this.gbPass.Location = new System.Drawing.Point(505, 17);
            this.gbPass.Name = "gbPass";
            this.gbPass.Size = new System.Drawing.Size(350, 122);
            this.gbPass.TabIndex = 2;
            this.gbPass.TabStop = false;
            this.gbPass.Text = "Pass info";
            // 
            // lblPassIsWrkHrsVal
            // 
            this.lblPassIsWrkHrsVal.Location = new System.Drawing.Point(174, 94);
            this.lblPassIsWrkHrsVal.Name = "lblPassIsWrkHrsVal";
            this.lblPassIsWrkHrsVal.Size = new System.Drawing.Size(165, 20);
            this.lblPassIsWrkHrsVal.TabIndex = 10;
            this.lblPassIsWrkHrsVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPassTypeVal
            // 
            this.lblPassTypeVal.Location = new System.Drawing.Point(174, 74);
            this.lblPassTypeVal.Name = "lblPassTypeVal";
            this.lblPassTypeVal.Size = new System.Drawing.Size(165, 20);
            this.lblPassTypeVal.TabIndex = 8;
            this.lblPassTypeVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPassLocationVal
            // 
            this.lblPassLocationVal.Location = new System.Drawing.Point(174, 54);
            this.lblPassLocationVal.Name = "lblPassLocationVal";
            this.lblPassLocationVal.Size = new System.Drawing.Size(165, 20);
            this.lblPassLocationVal.TabIndex = 6;
            this.lblPassLocationVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPassDirectionVal
            // 
            this.lblPassDirectionVal.Location = new System.Drawing.Point(174, 34);
            this.lblPassDirectionVal.Name = "lblPassDirectionVal";
            this.lblPassDirectionVal.Size = new System.Drawing.Size(165, 20);
            this.lblPassDirectionVal.TabIndex = 4;
            this.lblPassDirectionVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPassTimeVal
            // 
            this.lblPassTimeVal.Location = new System.Drawing.Point(174, 14);
            this.lblPassTimeVal.Name = "lblPassTimeVal";
            this.lblPassTimeVal.Size = new System.Drawing.Size(165, 20);
            this.lblPassTimeVal.TabIndex = 2;
            this.lblPassTimeVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPassIsWrkHrs
            // 
            this.lblPassIsWrkHrs.Location = new System.Drawing.Point(6, 94);
            this.lblPassIsWrkHrs.Name = "lblPassIsWrkHrs";
            this.lblPassIsWrkHrs.Size = new System.Drawing.Size(160, 20);
            this.lblPassIsWrkHrs.TabIndex = 9;
            this.lblPassIsWrkHrs.Text = "Is working hours:";
            this.lblPassIsWrkHrs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassType
            // 
            this.lblPassType.Location = new System.Drawing.Point(6, 74);
            this.lblPassType.Name = "lblPassType";
            this.lblPassType.Size = new System.Drawing.Size(160, 20);
            this.lblPassType.TabIndex = 7;
            this.lblPassType.Text = "Pass type:";
            this.lblPassType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassLocation
            // 
            this.lblPassLocation.Location = new System.Drawing.Point(6, 54);
            this.lblPassLocation.Name = "lblPassLocation";
            this.lblPassLocation.Size = new System.Drawing.Size(160, 20);
            this.lblPassLocation.TabIndex = 5;
            this.lblPassLocation.Text = "Location:";
            this.lblPassLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassDirection
            // 
            this.lblPassDirection.Location = new System.Drawing.Point(6, 34);
            this.lblPassDirection.Name = "lblPassDirection";
            this.lblPassDirection.Size = new System.Drawing.Size(160, 20);
            this.lblPassDirection.TabIndex = 3;
            this.lblPassDirection.Text = "Direction:";
            this.lblPassDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassTime
            // 
            this.lblPassTime.Location = new System.Drawing.Point(6, 14);
            this.lblPassTime.Name = "lblPassTime";
            this.lblPassTime.Size = new System.Drawing.Size(160, 20);
            this.lblPassTime.TabIndex = 1;
            this.lblPassTime.Text = "Time:";
            this.lblPassTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbEmployee
            // 
            this.gbEmployee.Controls.Add(this.cbEmplWTVal);
            this.gbEmployee.Controls.Add(this.lblEmplWUVal);
            this.gbEmployee.Controls.Add(this.lblEmplNameVal);
            this.gbEmployee.Controls.Add(this.lblEmplWT);
            this.gbEmployee.Controls.Add(this.lblEmplWU);
            this.gbEmployee.Controls.Add(this.lblEmplName);
            this.gbEmployee.Location = new System.Drawing.Point(15, 17);
            this.gbEmployee.Name = "gbEmployee";
            this.gbEmployee.Size = new System.Drawing.Size(350, 122);
            this.gbEmployee.TabIndex = 1;
            this.gbEmployee.TabStop = false;
            this.gbEmployee.Text = "Employee info";
            // 
            // cbEmplWTVal
            // 
            this.cbEmplWTVal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmplWTVal.Location = new System.Drawing.Point(99, 75);
            this.cbEmplWTVal.Name = "cbEmplWTVal";
            this.cbEmplWTVal.Size = new System.Drawing.Size(100, 21);
            this.cbEmplWTVal.TabIndex = 6;
            // 
            // lblEmplWUVal
            // 
            this.lblEmplWUVal.Location = new System.Drawing.Point(96, 54);
            this.lblEmplWUVal.Name = "lblEmplWUVal";
            this.lblEmplWUVal.Size = new System.Drawing.Size(245, 20);
            this.lblEmplWUVal.TabIndex = 4;
            this.lblEmplWUVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEmplNameVal
            // 
            this.lblEmplNameVal.Location = new System.Drawing.Point(96, 34);
            this.lblEmplNameVal.Name = "lblEmplNameVal";
            this.lblEmplNameVal.Size = new System.Drawing.Size(245, 20);
            this.lblEmplNameVal.TabIndex = 2;
            this.lblEmplNameVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEmplWT
            // 
            this.lblEmplWT.Location = new System.Drawing.Point(6, 74);
            this.lblEmplWT.Name = "lblEmplWT";
            this.lblEmplWT.Size = new System.Drawing.Size(82, 20);
            this.lblEmplWT.TabIndex = 5;
            this.lblEmplWT.Text = "Work time:";
            this.lblEmplWT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmplWU
            // 
            this.lblEmplWU.Location = new System.Drawing.Point(6, 54);
            this.lblEmplWU.Name = "lblEmplWU";
            this.lblEmplWU.Size = new System.Drawing.Size(82, 20);
            this.lblEmplWU.TabIndex = 3;
            this.lblEmplWU.Text = "Working unit:";
            this.lblEmplWU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmplName
            // 
            this.lblEmplName.Location = new System.Drawing.Point(6, 34);
            this.lblEmplName.Name = "lblEmplName";
            this.lblEmplName.Size = new System.Drawing.Size(82, 20);
            this.lblEmplName.TabIndex = 1;
            this.lblEmplName.Text = "Name:";
            this.lblEmplName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSec
            // 
            this.lblSec.Location = new System.Drawing.Point(136, 498);
            this.lblSec.Name = "lblSec";
            this.lblSec.Size = new System.Drawing.Size(30, 23);
            this.lblSec.TabIndex = 6;
            this.lblSec.Text = "sec";
            this.lblSec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOffset
            // 
            this.lblOffset.Location = new System.Drawing.Point(12, 498);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(70, 23);
            this.lblOffset.TabIndex = 4;
            this.lblOffset.Text = "Offset:";
            this.lblOffset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbPhotoNavigation
            // 
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoFirst);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoPrev);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoNext);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoLast);
            this.gbPhotoNavigation.Location = new System.Drawing.Point(235, 480);
            this.gbPhotoNavigation.Name = "gbPhotoNavigation";
            this.gbPhotoNavigation.Size = new System.Drawing.Size(400, 50);
            this.gbPhotoNavigation.TabIndex = 7;
            this.gbPhotoNavigation.TabStop = false;
            this.gbPhotoNavigation.Text = "Photo navigation";
            // 
            // btnPhotoFirst
            // 
            this.btnPhotoFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoFirst.Location = new System.Drawing.Point(10, 18);
            this.btnPhotoFirst.Name = "btnPhotoFirst";
            this.btnPhotoFirst.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoFirst.TabIndex = 1;
            this.btnPhotoFirst.Text = "|<";
            this.btnPhotoFirst.Click += new System.EventHandler(this.btnPhotoFirst_Click);
            // 
            // btnPhotoPrev
            // 
            this.btnPhotoPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoPrev.Location = new System.Drawing.Point(110, 18);
            this.btnPhotoPrev.Name = "btnPhotoPrev";
            this.btnPhotoPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoPrev.TabIndex = 2;
            this.btnPhotoPrev.Text = "<<";
            this.btnPhotoPrev.Click += new System.EventHandler(this.btnPhotoPrev_Click);
            // 
            // btnPhotoNext
            // 
            this.btnPhotoNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoNext.Location = new System.Drawing.Point(210, 18);
            this.btnPhotoNext.Name = "btnPhotoNext";
            this.btnPhotoNext.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoNext.TabIndex = 3;
            this.btnPhotoNext.Text = ">>";
            this.btnPhotoNext.Click += new System.EventHandler(this.btnPhotoNext_Click);
            // 
            // btnPhotoLast
            // 
            this.btnPhotoLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoLast.Location = new System.Drawing.Point(310, 18);
            this.btnPhotoLast.Name = "btnPhotoLast";
            this.btnPhotoLast.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoLast.TabIndex = 4;
            this.btnPhotoLast.Text = ">|";
            this.btnPhotoLast.Click += new System.EventHandler(this.btnPhotoLast_Click);
            // 
            // gbPhotos
            // 
            this.gbPhotos.Controls.Add(this.lblFileCTime);
            this.gbPhotos.Controls.Add(this.pb1);
            this.gbPhotos.Controls.Add(this.pb2);
            this.gbPhotos.Controls.Add(this.pb5);
            this.gbPhotos.Controls.Add(this.pb4);
            this.gbPhotos.Controls.Add(this.pb3);
            this.gbPhotos.Location = new System.Drawing.Point(15, 179);
            this.gbPhotos.Name = "gbPhotos";
            this.gbPhotos.Size = new System.Drawing.Size(840, 295);
            this.gbPhotos.TabIndex = 3;
            this.gbPhotos.TabStop = false;
            this.gbPhotos.Text = "Camera photos that match pass time";
            // 
            // lblFileCTime
            // 
            this.lblFileCTime.Location = new System.Drawing.Point(260, 263);
            this.lblFileCTime.Name = "lblFileCTime";
            this.lblFileCTime.Size = new System.Drawing.Size(320, 23);
            this.lblFileCTime.TabIndex = 2;
            this.lblFileCTime.Text = "File created time:";
            this.lblFileCTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pb1
            // 
            this.pb1.Location = new System.Drawing.Point(6, 95);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(120, 90);
            this.pb1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb1.TabIndex = 4;
            this.pb1.TabStop = false;
            this.pb1.Click += new System.EventHandler(this.pb1_Click);
            // 
            // pb2
            // 
            this.pb2.Location = new System.Drawing.Point(133, 95);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(120, 90);
            this.pb2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb2.TabIndex = 3;
            this.pb2.TabStop = false;
            this.pb2.Click += new System.EventHandler(this.pb2_Click);
            // 
            // pb5
            // 
            this.pb5.Location = new System.Drawing.Point(714, 95);
            this.pb5.Name = "pb5";
            this.pb5.Size = new System.Drawing.Size(120, 90);
            this.pb5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb5.TabIndex = 2;
            this.pb5.TabStop = false;
            this.pb5.Click += new System.EventHandler(this.pb5_Click);
            // 
            // pb4
            // 
            this.pb4.Location = new System.Drawing.Point(587, 95);
            this.pb4.Name = "pb4";
            this.pb4.Size = new System.Drawing.Size(120, 90);
            this.pb4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb4.TabIndex = 1;
            this.pb4.TabStop = false;
            this.pb4.Click += new System.EventHandler(this.pb4_Click);
            // 
            // pb3
            // 
            this.pb3.Location = new System.Drawing.Point(260, 20);
            this.pb3.Name = "pb3";
            this.pb3.Size = new System.Drawing.Size(320, 240);
            this.pb3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb3.TabIndex = 0;
            this.pb3.TabStop = false;
            this.pb3.Click += new System.EventHandler(this.pb3_Click);
            // 
            // gbPassNavigation
            // 
            this.gbPassNavigation.Controls.Add(this.btnPassFirst);
            this.gbPassNavigation.Controls.Add(this.btnPassPrev);
            this.gbPassNavigation.Controls.Add(this.btnPassNext);
            this.gbPassNavigation.Controls.Add(this.btnPassLast);
            this.gbPassNavigation.Location = new System.Drawing.Point(246, 560);
            this.gbPassNavigation.Name = "gbPassNavigation";
            this.gbPassNavigation.Size = new System.Drawing.Size(400, 50);
            this.gbPassNavigation.TabIndex = 2;
            this.gbPassNavigation.TabStop = false;
            this.gbPassNavigation.Text = "Pass navigation";
            // 
            // btnPassFirst
            // 
            this.btnPassFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassFirst.Location = new System.Drawing.Point(10, 18);
            this.btnPassFirst.Name = "btnPassFirst";
            this.btnPassFirst.Size = new System.Drawing.Size(75, 23);
            this.btnPassFirst.TabIndex = 1;
            this.btnPassFirst.Text = "|<";
            this.btnPassFirst.Click += new System.EventHandler(this.btnPassFirst_Click);
            // 
            // btnPassPrev
            // 
            this.btnPassPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassPrev.Location = new System.Drawing.Point(110, 18);
            this.btnPassPrev.Name = "btnPassPrev";
            this.btnPassPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPassPrev.TabIndex = 2;
            this.btnPassPrev.Text = "<<";
            this.btnPassPrev.Click += new System.EventHandler(this.btnPassPrev_Click);
            // 
            // btnPassNext
            // 
            this.btnPassNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassNext.Location = new System.Drawing.Point(210, 18);
            this.btnPassNext.Name = "btnPassNext";
            this.btnPassNext.Size = new System.Drawing.Size(75, 23);
            this.btnPassNext.TabIndex = 3;
            this.btnPassNext.Text = ">>";
            this.btnPassNext.Click += new System.EventHandler(this.btnPassNext_Click);
            // 
            // btnPassLast
            // 
            this.btnPassLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPassLast.Location = new System.Drawing.Point(310, 19);
            this.btnPassLast.Name = "btnPassLast";
            this.btnPassLast.Size = new System.Drawing.Size(75, 23);
            this.btnPassLast.TabIndex = 4;
            this.btnPassLast.Text = ">|";
            this.btnPassLast.Click += new System.EventHandler(this.btnPassLast_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(806, 579);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(11, 579);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblPassNum
            // 
            this.lblPassNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassNum.Location = new System.Drawing.Point(665, 578);
            this.lblPassNum.Name = "lblPassNum";
            this.lblPassNum.Size = new System.Drawing.Size(70, 23);
            this.lblPassNum.TabIndex = 5;
            this.lblPassNum.Text = "i/n";
            this.lblPassNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PassesSnapshots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 616);
            this.ControlBox = false;
            this.Controls.Add(this.lblPassNum);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbPassNavigation);
            this.Controls.Add(this.gbCurrentPass);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 650);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 650);
            this.Name = "PassesSnapshots";
            this.ShowInTaskbar = false;
            this.Text = "Camera photos for passes";
            this.Load += new System.EventHandler(this.PassesSnapshots_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PassesSnapshots_KeyUp);
            this.gbCurrentPass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEmployee)).EndInit();
            this.gbPass.ResumeLayout(false);
            this.gbEmployee.ResumeLayout(false);
            this.gbPhotoNavigation.ResumeLayout(false);
            this.gbPhotos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb3)).EndInit();
            this.gbPassNavigation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCurrentPass;
        private System.Windows.Forms.GroupBox gbPassNavigation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbPhotoNavigation;
        private System.Windows.Forms.GroupBox gbPhotos;
        private System.Windows.Forms.PictureBox pb3;
        private System.Windows.Forms.PictureBox pb1;
        private System.Windows.Forms.PictureBox pb2;
        private System.Windows.Forms.PictureBox pb5;
        private System.Windows.Forms.PictureBox pb4;
        private System.Windows.Forms.Button btnPassFirst;
        private System.Windows.Forms.Button btnPassPrev;
        private System.Windows.Forms.Button btnPassNext;
        private System.Windows.Forms.Button btnPassLast;
        private System.Windows.Forms.Button btnPhotoFirst;
        private System.Windows.Forms.Button btnPhotoPrev;
        private System.Windows.Forms.Button btnPhotoNext;
        private System.Windows.Forms.Button btnPhotoLast;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.Label lblSec;
        private System.Windows.Forms.Label lblFileCTime;
        private System.Windows.Forms.Label lblPhotoNum;
        private System.Windows.Forms.GroupBox gbPass;
        private System.Windows.Forms.GroupBox gbEmployee;
        private System.Windows.Forms.Label lblEmplName;
        private System.Windows.Forms.Label lblEmplWU;
        private System.Windows.Forms.Label lblEmplWT;
        private System.Windows.Forms.Label lblEmplNameVal;
        private System.Windows.Forms.Label lblEmplWUVal;
        private System.Windows.Forms.PictureBox pbEmployee;
        private System.Windows.Forms.Label lblPassIsWrkHrs;
        private System.Windows.Forms.Label lblPassType;
        private System.Windows.Forms.Label lblPassLocation;
        private System.Windows.Forms.Label lblPassDirection;
        private System.Windows.Forms.Label lblPassTime;
        private System.Windows.Forms.Label lblPassTimeVal;
        private System.Windows.Forms.Label lblPassIsWrkHrsVal;
        private System.Windows.Forms.Label lblPassTypeVal;
        private System.Windows.Forms.Label lblPassLocationVal;
        private System.Windows.Forms.Label lblPassDirectionVal;
        private System.Windows.Forms.NumericUpDown nudOffset;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblPassNum;
        private System.Windows.Forms.Label lblOfficialEmplPhoto;
        private System.Windows.Forms.ComboBox cbEmplWTVal;
    }
}