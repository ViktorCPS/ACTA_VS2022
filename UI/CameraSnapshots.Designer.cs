namespace UI
{
    partial class CameraSnapshots
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
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.cbCamera = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblReader = new System.Windows.Forms.Label();
            this.lblGate = new System.Windows.Forms.Label();
            this.cbReader = new System.Windows.Forms.ComboBox();
            this.cbGate = new System.Windows.Forms.ComboBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTimeForm = new System.Windows.Forms.Label();
            this.lblTimeTo = new System.Windows.Forms.Label();
            this.gbDate = new System.Windows.Forms.GroupBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblCamera = new System.Windows.Forms.Label();
            this.lblDirection = new System.Windows.Forms.Label();
            this.gbPhotos = new System.Windows.Forms.GroupBox();
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.pb2 = new System.Windows.Forms.PictureBox();
            this.pb5 = new System.Windows.Forms.PictureBox();
            this.pb4 = new System.Windows.Forms.PictureBox();
            this.pb3 = new System.Windows.Forms.PictureBox();
            this.gbPhotoNavigation = new System.Windows.Forms.GroupBox();
            this.btnPhotoFirst = new System.Windows.Forms.Button();
            this.btnPhotoPrev = new System.Windows.Forms.Button();
            this.btnPhotoNext = new System.Windows.Forms.Button();
            this.btnPhotoLast = new System.Windows.Forms.Button();
            this.lblPhotoNum = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbPhotoInfo = new System.Windows.Forms.GroupBox();
            this.tbPhotoCamera = new System.Windows.Forms.TextBox();
            this.cbPhotoDirections = new System.Windows.Forms.ComboBox();
            this.cbPhotoReaders = new System.Windows.Forms.ComboBox();
            this.cbPhotoGates = new System.Windows.Forms.ComboBox();
            this.cbPhotoLocations = new System.Windows.Forms.ComboBox();
            this.lblPhotoGate = new System.Windows.Forms.Label();
            this.lblPhotoTerminal = new System.Windows.Forms.Label();
            this.lblPhotoLocation = new System.Windows.Forms.Label();
            this.lblPhotoDirection = new System.Windows.Forms.Label();
            this.lblPhotoCamera = new System.Windows.Forms.Label();
            this.gbSearch.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbTime.SuspendLayout();
            this.gbDate.SuspendLayout();
            this.gbPhotos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb3)).BeginInit();
            this.gbPhotoNavigation.SuspendLayout();
            this.gbPhotoInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.gbFilter);
            this.gbSearch.Controls.Add(this.cbCamera);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.lblReader);
            this.gbSearch.Controls.Add(this.lblGate);
            this.gbSearch.Controls.Add(this.cbReader);
            this.gbSearch.Controls.Add(this.cbGate);
            this.gbSearch.Controls.Add(this.cbLocation);
            this.gbSearch.Controls.Add(this.lblLocation);
            this.gbSearch.Controls.Add(this.cbDirection);
            this.gbSearch.Controls.Add(this.gbTime);
            this.gbSearch.Controls.Add(this.gbDate);
            this.gbSearch.Controls.Add(this.lblCamera);
            this.gbSearch.Controls.Add(this.lblDirection);
            this.gbSearch.Location = new System.Drawing.Point(12, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(549, 309);
            this.gbSearch.TabIndex = 27;
            this.gbSearch.TabStop = false;
            this.gbSearch.Tag = "FILTERABLE";
            this.gbSearch.Text = "Search";
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(385, 16);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 100);
            this.gbFilter.TabIndex = 28;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 16;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 17;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // cbCamera
            // 
            this.cbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCamera.Location = new System.Drawing.Point(105, 141);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Size = new System.Drawing.Size(215, 21);
            this.cbCamera.TabIndex = 10;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(447, 270);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblReader
            // 
            this.lblReader.Location = new System.Drawing.Point(9, 77);
            this.lblReader.Name = "lblReader";
            this.lblReader.Size = new System.Drawing.Size(79, 23);
            this.lblReader.TabIndex = 5;
            this.lblReader.Text = "Reader:";
            this.lblReader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGate
            // 
            this.lblGate.Location = new System.Drawing.Point(9, 43);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(79, 23);
            this.lblGate.TabIndex = 3;
            this.lblGate.Text = "Gate:";
            this.lblGate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbReader
            // 
            this.cbReader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReader.Location = new System.Drawing.Point(105, 79);
            this.cbReader.Name = "cbReader";
            this.cbReader.Size = new System.Drawing.Size(215, 21);
            this.cbReader.TabIndex = 6;
            this.cbReader.SelectedIndexChanged += new System.EventHandler(this.cbReader_SelectedIndexChanged);
            // 
            // cbGate
            // 
            this.cbGate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGate.Location = new System.Drawing.Point(105, 47);
            this.cbGate.Name = "cbGate";
            this.cbGate.Size = new System.Drawing.Size(215, 21);
            this.cbGate.TabIndex = 4;
            this.cbGate.SelectedIndexChanged += new System.EventHandler(this.cbGate_SelectedIndexChanged);
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.Location = new System.Drawing.Point(105, 18);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(215, 21);
            this.cbLocation.TabIndex = 2;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(6, 16);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(82, 23);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Location = new System.Drawing.Point(105, 110);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(156, 21);
            this.cbDirection.TabIndex = 8;
            this.cbDirection.SelectedIndexChanged += new System.EventHandler(this.cbDirection_SelectedIndexChanged);
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.dtTo);
            this.gbTime.Controls.Add(this.dtFrom);
            this.gbTime.Controls.Add(this.lblTimeForm);
            this.gbTime.Controls.Add(this.lblTimeTo);
            this.gbTime.Location = new System.Drawing.Point(327, 164);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(195, 85);
            this.gbTime.TabIndex = 11;
            this.gbTime.TabStop = false;
            this.gbTime.Text = "Time";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "HH:mm";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(80, 48);
            this.dtTo.Name = "dtTo";
            this.dtTo.ShowUpDown = true;
            this.dtTo.Size = new System.Drawing.Size(56, 20);
            this.dtTo.TabIndex = 18;
            this.dtTo.Value = new System.DateTime(2008, 3, 5, 23, 59, 0, 0);
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "HH:mm";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(80, 16);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.ShowUpDown = true;
            this.dtFrom.Size = new System.Drawing.Size(56, 20);
            this.dtFrom.TabIndex = 16;
            this.dtFrom.Value = new System.DateTime(2008, 3, 5, 0, 0, 0, 0);
            // 
            // lblTimeForm
            // 
            this.lblTimeForm.Location = new System.Drawing.Point(29, 16);
            this.lblTimeForm.Name = "lblTimeForm";
            this.lblTimeForm.Size = new System.Drawing.Size(45, 20);
            this.lblTimeForm.TabIndex = 15;
            this.lblTimeForm.Text = "From:";
            this.lblTimeForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimeTo
            // 
            this.lblTimeTo.Location = new System.Drawing.Point(42, 48);
            this.lblTimeTo.Name = "lblTimeTo";
            this.lblTimeTo.Size = new System.Drawing.Size(32, 20);
            this.lblTimeTo.TabIndex = 17;
            this.lblTimeTo.Text = "To:";
            this.lblTimeTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbDate
            // 
            this.gbDate.Controls.Add(this.dtpTo);
            this.gbDate.Controls.Add(this.dtpFrom);
            this.gbDate.Controls.Add(this.lblFrom);
            this.gbDate.Controls.Add(this.lblTo);
            this.gbDate.Location = new System.Drawing.Point(106, 164);
            this.gbDate.Name = "gbDate";
            this.gbDate.Size = new System.Drawing.Size(215, 85);
            this.gbDate.TabIndex = 10;
            this.gbDate.TabStop = false;
            this.gbDate.Text = "Date";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(55, 48);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(132, 20);
            this.dtpTo.TabIndex = 14;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dtpFrom.Location = new System.Drawing.Point(55, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(132, 20);
            this.dtpFrom.TabIndex = 12;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(6, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(45, 20);
            this.lblFrom.TabIndex = 11;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(11, 46);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 20);
            this.lblTo.TabIndex = 13;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCamera
            // 
            this.lblCamera.Location = new System.Drawing.Point(6, 140);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(82, 20);
            this.lblCamera.TabIndex = 8;
            this.lblCamera.Text = "Camera:";
            this.lblCamera.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(6, 109);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(82, 20);
            this.lblDirection.TabIndex = 7;
            this.lblDirection.Text = "Direction:";
            this.lblDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbPhotos
            // 
            this.gbPhotos.Controls.Add(this.pb1);
            this.gbPhotos.Controls.Add(this.pb2);
            this.gbPhotos.Controls.Add(this.pb5);
            this.gbPhotos.Controls.Add(this.pb4);
            this.gbPhotos.Controls.Add(this.pb3);
            this.gbPhotos.Location = new System.Drawing.Point(12, 327);
            this.gbPhotos.Name = "gbPhotos";
            this.gbPhotos.Size = new System.Drawing.Size(843, 295);
            this.gbPhotos.TabIndex = 25;
            this.gbPhotos.TabStop = false;
            this.gbPhotos.Text = "Camera photos that match pass time";
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
            // gbPhotoNavigation
            // 
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoFirst);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoPrev);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoNext);
            this.gbPhotoNavigation.Controls.Add(this.btnPhotoLast);
            this.gbPhotoNavigation.Location = new System.Drawing.Point(229, 629);
            this.gbPhotoNavigation.Name = "gbPhotoNavigation";
            this.gbPhotoNavigation.Size = new System.Drawing.Size(400, 50);
            this.gbPhotoNavigation.TabIndex = 8;
            this.gbPhotoNavigation.TabStop = false;
            this.gbPhotoNavigation.Text = "Photo navigation";
            // 
            // btnPhotoFirst
            // 
            this.btnPhotoFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoFirst.Location = new System.Drawing.Point(10, 18);
            this.btnPhotoFirst.Name = "btnPhotoFirst";
            this.btnPhotoFirst.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoFirst.TabIndex = 20;
            this.btnPhotoFirst.Text = "|<";
            this.btnPhotoFirst.Click += new System.EventHandler(this.btnPhotoFirst_Click);
            // 
            // btnPhotoPrev
            // 
            this.btnPhotoPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoPrev.Location = new System.Drawing.Point(110, 18);
            this.btnPhotoPrev.Name = "btnPhotoPrev";
            this.btnPhotoPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoPrev.TabIndex = 21;
            this.btnPhotoPrev.Text = "<<";
            this.btnPhotoPrev.Click += new System.EventHandler(this.btnPhotoPrev_Click);
            // 
            // btnPhotoNext
            // 
            this.btnPhotoNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoNext.Location = new System.Drawing.Point(210, 18);
            this.btnPhotoNext.Name = "btnPhotoNext";
            this.btnPhotoNext.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoNext.TabIndex = 22;
            this.btnPhotoNext.Text = ">>";
            this.btnPhotoNext.Click += new System.EventHandler(this.btnPhotoNext_Click);
            // 
            // btnPhotoLast
            // 
            this.btnPhotoLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhotoLast.Location = new System.Drawing.Point(310, 18);
            this.btnPhotoLast.Name = "btnPhotoLast";
            this.btnPhotoLast.Size = new System.Drawing.Size(75, 23);
            this.btnPhotoLast.TabIndex = 23;
            this.btnPhotoLast.Text = ">|";
            this.btnPhotoLast.Click += new System.EventHandler(this.btnPhotoLast_Click);
            // 
            // lblPhotoNum
            // 
            this.lblPhotoNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhotoNum.Location = new System.Drawing.Point(635, 647);
            this.lblPhotoNum.Name = "lblPhotoNum";
            this.lblPhotoNum.Size = new System.Drawing.Size(70, 23);
            this.lblPhotoNum.TabIndex = 9;
            this.lblPhotoNum.Text = "i/n";
            this.lblPhotoNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(780, 647);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbPhotoInfo
            // 
            this.gbPhotoInfo.Controls.Add(this.tbPhotoCamera);
            this.gbPhotoInfo.Controls.Add(this.cbPhotoDirections);
            this.gbPhotoInfo.Controls.Add(this.cbPhotoReaders);
            this.gbPhotoInfo.Controls.Add(this.cbPhotoGates);
            this.gbPhotoInfo.Controls.Add(this.cbPhotoLocations);
            this.gbPhotoInfo.Controls.Add(this.lblPhotoGate);
            this.gbPhotoInfo.Controls.Add(this.lblPhotoTerminal);
            this.gbPhotoInfo.Controls.Add(this.lblPhotoLocation);
            this.gbPhotoInfo.Controls.Add(this.lblPhotoDirection);
            this.gbPhotoInfo.Controls.Add(this.lblPhotoCamera);
            this.gbPhotoInfo.Location = new System.Drawing.Point(577, 12);
            this.gbPhotoInfo.Name = "gbPhotoInfo";
            this.gbPhotoInfo.Size = new System.Drawing.Size(278, 309);
            this.gbPhotoInfo.TabIndex = 26;
            this.gbPhotoInfo.TabStop = false;
            this.gbPhotoInfo.Text = "Photo info";
            // 
            // tbPhotoCamera
            // 
            this.tbPhotoCamera.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbPhotoCamera.Enabled = false;
            this.tbPhotoCamera.Location = new System.Drawing.Point(101, 171);
            this.tbPhotoCamera.Name = "tbPhotoCamera";
            this.tbPhotoCamera.Size = new System.Drawing.Size(168, 20);
            this.tbPhotoCamera.TabIndex = 30;
            // 
            // cbPhotoDirections
            // 
            this.cbPhotoDirections.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbPhotoDirections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPhotoDirections.Location = new System.Drawing.Point(101, 128);
            this.cbPhotoDirections.Name = "cbPhotoDirections";
            this.cbPhotoDirections.Size = new System.Drawing.Size(168, 21);
            this.cbPhotoDirections.TabIndex = 30;
            // 
            // cbPhotoReaders
            // 
            this.cbPhotoReaders.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbPhotoReaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPhotoReaders.Location = new System.Drawing.Point(101, 91);
            this.cbPhotoReaders.Name = "cbPhotoReaders";
            this.cbPhotoReaders.Size = new System.Drawing.Size(168, 21);
            this.cbPhotoReaders.TabIndex = 30;
            // 
            // cbPhotoGates
            // 
            this.cbPhotoGates.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbPhotoGates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPhotoGates.Location = new System.Drawing.Point(101, 57);
            this.cbPhotoGates.Name = "cbPhotoGates";
            this.cbPhotoGates.Size = new System.Drawing.Size(168, 21);
            this.cbPhotoGates.TabIndex = 30;
            // 
            // cbPhotoLocations
            // 
            this.cbPhotoLocations.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cbPhotoLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPhotoLocations.Location = new System.Drawing.Point(101, 25);
            this.cbPhotoLocations.Name = "cbPhotoLocations";
            this.cbPhotoLocations.Size = new System.Drawing.Size(168, 21);
            this.cbPhotoLocations.TabIndex = 30;
            // 
            // lblPhotoGate
            // 
            this.lblPhotoGate.Location = new System.Drawing.Point(16, 56);
            this.lblPhotoGate.Name = "lblPhotoGate";
            this.lblPhotoGate.Size = new System.Drawing.Size(79, 20);
            this.lblPhotoGate.TabIndex = 31;
            this.lblPhotoGate.Text = "Gate:";
            this.lblPhotoGate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPhotoTerminal
            // 
            this.lblPhotoTerminal.Location = new System.Drawing.Point(13, 90);
            this.lblPhotoTerminal.Name = "lblPhotoTerminal";
            this.lblPhotoTerminal.Size = new System.Drawing.Size(82, 20);
            this.lblPhotoTerminal.TabIndex = 31;
            this.lblPhotoTerminal.Text = "Reder:";
            this.lblPhotoTerminal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPhotoLocation
            // 
            this.lblPhotoLocation.Location = new System.Drawing.Point(10, 26);
            this.lblPhotoLocation.Name = "lblPhotoLocation";
            this.lblPhotoLocation.Size = new System.Drawing.Size(85, 20);
            this.lblPhotoLocation.TabIndex = 31;
            this.lblPhotoLocation.Text = "Location:";
            this.lblPhotoLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPhotoDirection
            // 
            this.lblPhotoDirection.Location = new System.Drawing.Point(16, 127);
            this.lblPhotoDirection.Name = "lblPhotoDirection";
            this.lblPhotoDirection.Size = new System.Drawing.Size(79, 20);
            this.lblPhotoDirection.TabIndex = 31;
            this.lblPhotoDirection.Text = "Direction:";
            this.lblPhotoDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPhotoCamera
            // 
            this.lblPhotoCamera.Location = new System.Drawing.Point(9, 169);
            this.lblPhotoCamera.Name = "lblPhotoCamera";
            this.lblPhotoCamera.Size = new System.Drawing.Size(86, 20);
            this.lblPhotoCamera.TabIndex = 31;
            this.lblPhotoCamera.Text = "Camera:";
            this.lblPhotoCamera.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CameraSnapshots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 690);
            this.ControlBox = false;
            this.Controls.Add(this.gbPhotoInfo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblPhotoNum);
            this.Controls.Add(this.gbPhotoNavigation);
            this.Controls.Add(this.gbPhotos);
            this.Controls.Add(this.gbSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CameraSnapshots";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CameraSnapshots";
            this.Load += new System.EventHandler(this.CameraSnapshots_Load);
            this.gbSearch.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbTime.ResumeLayout(false);
            this.gbDate.ResumeLayout(false);
            this.gbPhotos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb3)).EndInit();
            this.gbPhotoNavigation.ResumeLayout(false);
            this.gbPhotoInfo.ResumeLayout(false);
            this.gbPhotoInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.GroupBox gbPhotos;
        private System.Windows.Forms.PictureBox pb1;
        private System.Windows.Forms.PictureBox pb2;
        private System.Windows.Forms.PictureBox pb5;
        private System.Windows.Forms.PictureBox pb4;
        private System.Windows.Forms.PictureBox pb3;
        private System.Windows.Forms.GroupBox gbPhotoNavigation;
        private System.Windows.Forms.Button btnPhotoFirst;
        private System.Windows.Forms.Button btnPhotoPrev;
        private System.Windows.Forms.Button btnPhotoNext;
        private System.Windows.Forms.Button btnPhotoLast;
        private System.Windows.Forms.Label lblPhotoNum;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbTime;
        private System.Windows.Forms.Label lblTimeForm;
        private System.Windows.Forms.Label lblTimeTo;
        private System.Windows.Forms.GroupBox gbDate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblCamera;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label lblReader;
        private System.Windows.Forms.Label lblGate;
        private System.Windows.Forms.ComboBox cbReader;
        private System.Windows.Forms.ComboBox cbGate;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbCamera;
        private System.Windows.Forms.GroupBox gbPhotoInfo;
        private System.Windows.Forms.Label lblPhotoGate;
        private System.Windows.Forms.Label lblPhotoTerminal;
        private System.Windows.Forms.Label lblPhotoLocation;
        private System.Windows.Forms.Label lblPhotoDirection;
        private System.Windows.Forms.Label lblPhotoCamera;
        private System.Windows.Forms.ComboBox cbPhotoDirections;
        private System.Windows.Forms.ComboBox cbPhotoReaders;
        private System.Windows.Forms.ComboBox cbPhotoGates;
        private System.Windows.Forms.ComboBox cbPhotoLocations;
        private System.Windows.Forms.TextBox tbPhotoCamera;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSaveCriteria;
        private System.Windows.Forms.ComboBox cbFilter;
    }
}