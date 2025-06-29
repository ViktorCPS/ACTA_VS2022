using System;
using System.Configuration;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace Util
{
    /// <summary>
    /// 
    /// </summary>
    public class Constants
    {
        private static string rootDirectory = "";
        private static string _logFilePath = "appllogs\\";
        private static string _pdfDocPath = "report\\pdf\\";
        private static string _pdfTemplatePath = "report\\templates\\";
        private static string _csvDocPath = "report\\csv\\";
        private static string _txtDocPath = "report\\txt\\";
        private static string _XMLDataSourceDir = "data\\XMLSource\\";
        private static string _XMLUpdateFilesDir = "data\\XMLUpdate\\";
        private static string _unprocessed = "unprocessed\\";
        private static string _archived = "archived\\";
        private static string _trash = "trash\\";
        private static string _cards = "cards\\";
        private static string _tmpScan = "tmpScan\\";
        private static string _timeaccessprofiles = "timeaccessprofiles\\";
        private static string _snapshots = "snapshots\\";
        private static string _EmployeePhotoDirectory = "resources\\photos\\";
        private static string _LocalEmployeePhotoDirectory = "photos\\";
        private static string _logoPath = "resources\\logos\\ClientLogo.jpg";
        private static string _logoReportPath = "resources\\logos\\ClientLogo-Report.jpg";
        private static string _LocalReaderLogDir = "data\\LOG\\";
        private static string _RegistryDataApplication = "RWD\\RegistryDataApplication.exe";
        private static string _helpManualPath = "Help\\ACTAAdmin Manual.pdf";
        private static string _helpManualHtmlPath = "Help\\ACTAAdmin Manual.html";
        private static string _helpACTAGateManualPath = "Help\\ACTAGate Manual.pdf";
        private static string _objectImagePath = "resources\\icons\\";
        private static string _siPass = "SiPass";
        private static string _siPassConnPath = "connection.txt";
        private static string _siPassConnPathBreza = "connectionBreza.txt";
        private static string _siPassMappingPath = "mapping.xml";
        private static string _siPassMappingOLDPath = "mappingOLD.xml";
        private static string _siPassDiffLogPath = "DiffLogPointer.xml";
        private static string _siPassPostProcessing = "PostProcessing\\";
        private static string _siPassDepartmentMappingPath = "departmentMapping.xml";
        private static string _siPassTruckMappingPath = "truckMapping.xml";
        private static string _snapshotsFilePath = "snapshots\\";
        private static string _tempLog = "temp";
        private static string _XKPointerFilePath = "appllogs\\XKPointer.txt";
        private static string _NotificationFilePath = "EmailNotification";
        private static string _LastNotifTimeFilePath = "LastNotifTime.txt";
        private static string _sdditgLogoPath = "resources\\logos\\SDDITGLogo.jpg";
        private static string _fiatServicesLogoPath = "resources\\logos\\FiatLogo.jpg";
        private static string _infoPath = "\\CommonWeb\\files\\";
        private static string _mobile = "mobile\\";
        private static string _mobileDownload = "ftp\\";
        private static string _mobileExtract = "extract\\";
        private static string _defaultMealImagePath = "resources\\photos\\DefaultMeal.jpg";
        private static string _keteringPricaFirstPage = "resources\\photos\\KeteringPricaFirstPage.jpg";
        private static string _keteringPricaBackground = "resources\\photos\\KeteringPricaBackground.jpg";
        

        public static string FiatServicesLogoPath
        {
            get
            {
                return RootDirectory + "\\" + _fiatServicesLogoPath;
            }
        }

        //max insert into Log table
        private static int _recordsToProcess = 100;

        public static int RecordsToProcess
        {
            get
            {
                //if (_recordsToProcess == -1)
                //{
                //    try
                //    {
                //        _recordsToProcess = int.Parse(ConfigurationManager.AppSettings["RecordsToProcess"]);
                //    }
                //    catch
                //    {
                //        _recordsToProcess = -1;
                //    }
                //}
                return Constants._recordsToProcess;
            }
        }

        public static string tempLog
        {
            get
            {
                string folder = "";
                folder = RootDirectory + "\\" + _tempLog;
                if (Directory.Exists(RootDirectory) && !Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public static string SnapshotsFilePath
        {
            get
            {
                string folder = "";
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    folder = RootDirectory + "\\L\\" + _snapshotsFilePath;
                }
                else
                {
                    folder = ConfigurationManager.AppSettings["L"] + "\\" + _snapshotsFilePath;
                }
                if (Directory.Exists(RootDirectory + "\\L") && !Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public static string NotificationFilePath
        {
            get
            {
                string folder = "";
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    folder = RootDirectory + "\\L\\" + _NotificationFilePath;
                }
                else
                {
                    folder = ConfigurationManager.AppSettings["L"] + "\\" + _NotificationFilePath;
                }
                if (Directory.Exists(RootDirectory + "\\L") && !Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }


        public static string SiPassPostProcessing
        {
            get
            {
                string folder = SiPass + "\\" + _siPassPostProcessing;
                if (Directory.Exists(SiPass) && !Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public static string LastNotifTimeFilePath
        {
            get { return NotificationFilePath + "\\" + Constants._LastNotifTimeFilePath; }
        }

        public static string SiPassDiffLogPath
        {
            get { return SiPass + "\\" + Constants._siPassDiffLogPath; }
        }

        public static string SiPassMappingPath
        {
            get { return SiPass + "\\" + Constants._siPassMappingPath; }
        }

        public static string SiPassMappingOLDPath
        {
            get { return SiPass + "\\" + Constants._siPassMappingOLDPath; }
        }

        public static string SiPassDepartmentMappingPath
        {
            get { return SiPass + "\\" + Constants._siPassDepartmentMappingPath; }
        }

        public static string SiPassTruckMappingPath
        {
            get { return SiPass + "\\" + Constants._siPassTruckMappingPath; }
        }

        public static string SiPassConnPath
        {
            get { return SiPass + "\\" + Constants._siPassConnPath; }
        }

        public static string SiPassConnPathBreza
        {
            get { return SiPass + "\\" + Constants._siPassConnPathBreza; }
        }

        public static string SiemensAuditTrailPath
        {
            get
            {
                string folder = "";
                if (ConfigurationManager.AppSettings["AuditTrailPath"] != null)
                {
                    folder = ConfigurationManager.AppSettings["AuditTrailPath"].Trim() + "\\";
                }                
                return folder;
            }
        }

        public static string SiPass
        {
            get
            {
                string Lfolder = "";
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    Lfolder = RootDirectory + "\\L";
                }
                else
                {
                    Lfolder = ConfigurationManager.AppSettings["L"];
                }
                string folder = Lfolder + "\\" + _siPass;
                if (Directory.Exists(Lfolder) && !Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        // Properties
        public static string logFilePath
        {
            get { return RootDirectory + "\\" + _logFilePath; }
        }

        public static string infoFilePath
        {
            get { return RootDirectory + "\\" + _infoPath; }
        }

        public static string XKPointerFilePath
        {
            get { return RootDirectory + "\\" + _XKPointerFilePath; }
        }

        public static string pdfDocPath
        {
            get { return RootDirectory + "\\" + _pdfDocPath; }
        }

        public static string pdfTemplatePath
        {
            get { return RootDirectory + "\\" + _pdfTemplatePath; }
        }

        public static string csvDocPath
        {
            get { return RootDirectory + "\\" + _csvDocPath; }
        }

        public static string txtDocPath
        {
            get { return RootDirectory + "\\" + _txtDocPath; }
        }

        public static string XMLDataSourceDir
        {
            get { return RootDirectory + "\\" + _XMLDataSourceDir; }
        }

        public static string XMLUpdateFilesDir
        {
            get { return RootDirectory + "\\" + _XMLUpdateFilesDir; }
        }

        public static string MonitoringPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\monitoring\\";
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\monitoring\\";
                }
            }
        }

        public static string unprocessed
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _unprocessed;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _unprocessed;
                }

            }
        }

        public static string archived
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _archived;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _archived;
                }

            }
        }

        public static string trash
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _trash;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _trash;
                }

            }
        }

        public static string unprocessedMobile
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _unprocessed + "\\" + _mobile;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _unprocessed + "\\" + _mobile;
                }

            }
        }

        public static string archivedMobile
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _archived + "\\" + _mobile;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _archived + "\\" + _mobile;
                }

            }
        }

        public static string archivedMobileDownload
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _archived + "\\" + _mobileDownload;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _archived + "\\" + _mobileDownload;
                }

            }
        }

        public static string trashMobile
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _trash + "\\" + _mobile;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _trash + "\\" + _mobile;
                }

            }
        }

        public static string trashMobileDownload
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _trash + "\\" + _mobileDownload;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _trash + "\\" + _mobileDownload;
                }

            }
        }

        public static string MobileDownload
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _mobileDownload;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _mobileDownload;
                }

            }
        }

        public static string MobileExtract
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _mobileDownload + "\\" + _mobileExtract;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _mobileDownload + "\\" + _mobileExtract;
                }

            }
        }

        public static string MobileExtractTmp
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _mobileDownload + "\\" + _mobileExtract + "tmp\\";
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _mobileDownload + "\\" + _mobileExtract + "tmp\\";
                }

            }
        }

        public static string cards
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _cards;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _cards;
                }

            }
        }

        public static string TmpScan
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _tmpScan;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _tmpScan;
                }

            }
        }

        public static string timeaccessprofiles
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _timeaccessprofiles;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _timeaccessprofiles;
                }
            }
        }

        public static string snapshots
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L\\" + _snapshots;
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"] + "\\" + _snapshots;
                }
            }
        }

        public static string EmployeePhotoDirectory
        {
            get
            {
                if (ConfigurationManager.AppSettings["photos"] == null || ConfigurationManager.AppSettings["photos"].Equals(""))
                {
                    return RootDirectory + "\\" + _EmployeePhotoDirectory;
                }
                else
                {
                    return ConfigurationManager.AppSettings["photos"] + "\\";
                }
            }
        }

        public static string DatabaseInfoString
        {
            get
            {
                string connectionString = "";
                string server = "";
                string database = "";
                string dataProvider = "";
                string port = Constants.mysqlPortDefault;
                int startIndex = -1;
                int endIndex = -1;

                connectionString = ConfigurationManager.AppSettings["connectionString"];

                byte[] buffer = Convert.FromBase64String(connectionString);
                connectionString = Util.Misc.decrypt(buffer);


                if (connectionString.Length > 0)
                {
                    //calculate data provider
                    startIndex = connectionString.ToLower().IndexOf("data provider=");
                    if (startIndex >= 0)
                    {
                        endIndex = connectionString.IndexOf(";", startIndex);
                        if (endIndex >= startIndex)
                            dataProvider = connectionString.Substring(startIndex, endIndex - startIndex + 1);
                    }

                    //calculate server
                    startIndex = connectionString.ToLower().IndexOf("server=");
                    if (startIndex >= 0)
                    {
                        endIndex = connectionString.IndexOf(";", startIndex);
                        if (endIndex >= startIndex)
                            server = connectionString.Substring(startIndex, endIndex - startIndex + 1);
                    }

                    //calculate port
                    startIndex = connectionString.ToLower().IndexOf("port=");
                    if (startIndex >= 0)
                    {
                        endIndex = connectionString.IndexOf(";", startIndex);
                        if (endIndex >= startIndex)
                            port = connectionString.Substring(startIndex, endIndex - startIndex + 1);
                    }

                    //calculatedatabase
                    startIndex = connectionString.ToLower().IndexOf("database=");
                    if (startIndex >= 0)
                    {
                        endIndex = connectionString.IndexOf(";", startIndex);
                        if (endIndex >= startIndex)
                            database = connectionString.Substring(startIndex, endIndex - startIndex + 1);
                    }
                }

                return dataProvider + " " + server + (dataProvider.ToLower().Equals("data provider=mysql;") ? " " + port + " " : " ") + database;
            }
        }

        public static string GetDatabaseString
        {
            get
            {
                string connectionString = "";
                string database = "";
                int startIndex = -1;
                int endIndex = -1;

                connectionString = ConfigurationManager.AppSettings["connectionString"];

                byte[] buffer = Convert.FromBase64String(connectionString);
                connectionString = Util.Misc.decrypt(buffer);

                if (connectionString.Length > 0)
                {
                    //calculatedatabase
                    startIndex = connectionString.IndexOf("database");
                    if (startIndex >= 0)
                    {
                        endIndex = connectionString.IndexOf(";", startIndex);
                        if (endIndex > startIndex)
                        {
                            database = connectionString.Substring(startIndex, endIndex - startIndex);
                            startIndex = database.IndexOf("=");
                            if ((startIndex >= 0) && ((startIndex + 1) < database.Length))
                            {
                                database = database.Substring(startIndex + 1);
                            }
                        }
                    }
                }

                return database;
            }
        }

        public static string RootDirectory
        {
            get
            {
                try
                {
                    try
                    {
                        rootDirectory = ConfigurationManager.AppSettings["rootDirectory"];
                    }
                    catch
                    {
                        rootDirectory = "";
                    }

                    if ((rootDirectory != null) && (rootDirectory != ""))
                    {
                        return rootDirectory;
                    }
                    else
                    {
                        rootDirectory = Environment.CurrentDirectory;

                        if (rootDirectory.EndsWith("bin\\Debug") || rootDirectory.EndsWith("bin\\Release"))
                        {
                            return Directory.GetParent(Directory.GetParent(Directory.GetParent(rootDirectory).ToString()).ToString()).ToString();
                        }
                        else
                        {
                            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                            string currentDir = "";

                            if (assembly.CodeBase.Contains(@"bin/Debug") || assembly.CodeBase.Contains(@"bin/Release"))
                            {
                                currentDir = assembly.CodeBase.Substring(8, assembly.CodeBase.LastIndexOf(@"/bin") - 8);
                            }
                            else
                            {
                                currentDir = assembly.CodeBase.Substring(8, assembly.CodeBase.LastIndexOf(@"/") - 8);
                            }
                            return Directory.GetParent(currentDir).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting root directory: " + ex.Message);
                }
            }
        }

        public static string LPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["L"] == null || ConfigurationManager.AppSettings["L"].Equals(""))
                {
                    return RootDirectory + "\\L";
                }
                else
                {
                    return ConfigurationManager.AppSettings["L"];
                }
            }
        }

        public static string LocalEmployeePhotoDirectory
        {
            get
            {
                return _LocalEmployeePhotoDirectory;
            }
        }

        public static string LogoPath
        {
            get
            {
                return RootDirectory + "\\" + _logoPath;
            }
        }

        public static string SDDITGLogoPath
        {
            get
            {
                return RootDirectory + "\\" + _sdditgLogoPath;
            }
        }

        public static byte[] LogoForReport
        {
            get
            {
                //if there is no logo, return array that represent white logo whit only one pixel
                byte[] whiteLogo = new byte[58];
                for (int i = 0; i < whiteLogo.Length; i++)
                {
                    whiteLogo[i] = (byte)0;
                }
                whiteLogo[0] = (byte)66;
                whiteLogo[1] = (byte)77;
                whiteLogo[2] = (byte)58;
                whiteLogo[10] = (byte)54;
                whiteLogo[14] = (byte)40;
                whiteLogo[18] = (byte)1;
                whiteLogo[22] = (byte)1;
                whiteLogo[26] = (byte)1;
                whiteLogo[28] = (byte)24;
                whiteLogo[34] = (byte)4;
                whiteLogo[54] = (byte)255;
                whiteLogo[55] = (byte)255;
                whiteLogo[56] = (byte)255;

                try
                {
                    string logoReportPath = RootDirectory + "\\" + _logoReportPath;

                    FileStream FilStr;

                    FilStr = new FileStream(logoReportPath, FileMode.Open);
                    if (FilStr != null)
                    {
                        BinaryReader BinRed = new BinaryReader(FilStr);

                        byte[] imgbyte = new byte[FilStr.Length + 1];

                        // Here you use ReadBytes method to add a byte array of the image stream.
                        //so the image column will hold a byte array.
                        imgbyte = BinRed.ReadBytes(Convert.ToInt32(BinRed.BaseStream.Length));

                        BinRed.Close();
                        FilStr.Close();

                        return imgbyte;
                    }
                    else
                        return whiteLogo;
                }
                catch
                {
                    return whiteLogo;
                }
            }
        }

        public static string LocalReaderLogDir
        {
            get { return RootDirectory + "\\" + _LocalReaderLogDir; }
        }

        public static string RegistryDataApplication
        {
            get
            {
                return RootDirectory + "\\" + _RegistryDataApplication;
            }
        }

        public static string HelpManualPath
        {
            get { return RootDirectory + "\\" + _helpManualPath; }
        }

        public static string HelpManualHtmlPath
        {
            get { return RootDirectory + "\\" + _helpManualHtmlPath; }
        }

        public static string HelpACTAGateManualPath
        {
            get { return RootDirectory + "\\" + _helpACTAGateManualPath; }
        }
        public static string ObjectImagePath
        {
            get { return RootDirectory + "\\" + _objectImagePath; }
        }

        public static string DefaultMealImagePath
        {
            get
            {
                return RootDirectory + "\\" + _defaultMealImagePath;
            }
        }

        public static string KeteringPricaFirstImagePath
        {
            get
            {
                return RootDirectory + "\\" + _keteringPricaFirstPage;
            }
        }

        public static string KeteringPricaBackgroundImagePath
        {
            get
            {
                return RootDirectory + "\\" + _keteringPricaBackground;
            }
        }

        public static string PINforMealOrder
        {
            get
            {
                string PIN = "";
                if (ConfigurationManager.AppSettings["PIN"] != null)
                {
                    PIN = ConfigurationManager.AppSettings["PIN"].Trim();
                }
                return PIN;
            }
        }

        public static string IdleTimeKeteringPrica
        {
            get
            {
                string idleTime = "";
                if (ConfigurationManager.AppSettings["idleTimeInSec"] != null)
                {
                    idleTime = ConfigurationManager.AppSettings["idleTimeInSec"].Trim();
                }
                return idleTime;
            }
        }       

        public enum FiatCompanies
        {
            FAS = -2,
            MMdoo = -3,
            MMauto = -4,
            FS = -5
        }

        // enums
        public enum EventTag
        {
            eventTagUnrecognized = 1,
            eventUnknown = 2,
            eventTagDenied = 3,
            eventTagAllowed = 4,
            eventButtonPressed = 255
        }

        public enum PassGenUsed
        {
            Unused = 0,
            Used = 1,
            Unexpected = 2
        }

        public enum PassType
        {
            Work = 0,
            WholeDayAbsences = 2
        }

        public enum PassTypeAll
        {
            OtherPaymentCode = 0,
            PassOnReader = 1,
            WholeDayAbsences = 2,
            OverTime = 3,
        }

        public enum IsWrkCount
        {
            IsNotCounter = 0,
            IsCounter = 1
        }

        public enum PairGenUsed
        {
            Unused = 0,
            Used = 1,
            Obsolete = 2,
        }

        public enum recordCreated
        {
            Automaticaly = 0,
            Manualy = 1,
        }

        public enum Used
        {
            No = 0, //this is now verified but unused
            Yes = 1, //this is now used, as it was			
            Error = 2,
            Unverified = 3, //this is new, meaning unverified

        }

        public enum PermVerification
        {
            No = 0, //user doesn't have the right to verificate permission
            Yes = 1, //user has the right to verificate permission
        }

        public enum AutoClose
        {
            WithoutClose = 0,
            StartClose = 1,
            EndClose = 2,
            StartEndClose = 3,
        }

        public enum HasTag
        {
            yes = 0,
            no = 1,
            all = 2,
        }

        public enum Moduls
        {
            TimeTable1 = 1,
            TimeTable2 = 2,
            Absences = 3,
            ExitPermits = 4,
            ExtraHours = 5,
            AccessControl = 6,
            Snapshots = 7,
            Monitoring = 8,
            Visitors = 9,
            GraficalReports = 10,
            Restaurant = 11,
            RoutesTag = 12,
            RoutesTerminal = 13,
            PeopleCounterBasic = 14,
            PeopleCounterStandard = 15,
            PeopleCounterAdvance = 16,
            Vacation = 17,
            ProcessAllTags = 18,
            SiemensCompatibility = 19,
            RecordsToProcess = 20,
            UNIPROM = 21,
            SelfService = 22,
            RestaurantI = 23,
            RestaurantII = 24,
            MedicalCheck = 25
        }

        // Pri menjanju, menjati i kod za popunjavanje combo boxa u formi za generisanje licence!
        public enum Customers
        {
            Mittal = 10,
            AAC = 11,
            ConfezioniAndrea = 12,
            GSK = 15,
            Grundfos = 16,
            Hyatt = 17,
            Magna = 18,
            PIO = 20,
            Geox = 22,
            ATB = 23,
            Niksic = 25,
            Millennium = 30,
            Sinvoz = 35,
            FOD = 40,
            Vlatacom = 45,
            JUBMES = 50,
            FIAT = 55,
            JEEP = 60,
            Lames = 65,
            UNIPROM = 70,
            DSF = 75,
            ZIN = 80,
            WAKERNEUSON = 85,
            EUNET = 90,
            Ministarstvo = 95,
            PMC = 97,
            NikolaTesla=100,
        }

        public const int actionWhenButtonPressed = 127;

        // Constants
        public const int RestaurantReaderID = 1; //Tamara 5.2.2020.
        // NEDAP
        public const int actionCommitedAllowed = 20;
        public const int actionCommitedDenied = 24;
        public const string nedapDirectory = "NEDAP\\";

        // Gate
        public const int downloadStartHour = 3;
        public const int downloadInterval = 1440;
        public const int downloadEraseCounter = 90;

        public const int offset = 30;
        public const int officialOutOffset = 20;

        // Status
        public const string statusActive = "ACTIVE";
        public const string statusBlocked = "BLOCKED";
        public const string statusRetired = "RETIRED";
        public const string statusLost = "LOST";
        public const string statusDamaged = "DAMAGED";
        public const string statusReturned = "RETURNED";
        public const string statusDisabled = "DISABLED";

        //Type of card
        public const string typeManager = "Manager";
        public const string typeSupervisor = "Supervisor";
        public const string typeEmployee = "Employee";

        // ExtraHoursUsed
        public const string extraHoursUsedRegular = "REGULAR_WORK";
        public const string extraHoursUsedOvertime = "OVERTIME_WORK";
        public const string extraHoursUsedRejected = "REJECTED_HOURS";
        public const string extraHoursUsedRegularAdvanced = "REGULAR_WORK_ADV";

        // Sort
        public const int sortAsc = 0;
        public const int sortDesc = 1;

        // max records shown number
        public const int maxRecords = 10000;
        public const int recordsPerPage = 500;

        //number of records in graph view
        public const int recordsPerGraph = 31;

        public const int PaymentCodeLength = 4;

        // Reader's connections types
        public const string ConnTypeIP = "IP";
        public const string ConnTypeSerial = "SERIAL";
        public const string ConnTypeGSM = "GSM";

        // Technlogy Types
        public const string TechTypeHitag1 = "HITAG1";
        public const string TechTypeHitag2 = "HITAG2";
        public const string TechTypeMifare = "MIFARE";
        public const string TechTypeICode = "ICODE";
        public const string DefaultTechType = "MIFARE";

        // Antenna Directions
        public const string DirectionIn = "IN";
        public const string DirectionOut = "OUT";
        public const string DirectionInOut = "INOUT";

        // Auto Close Users
        public const string AutoCloseUser = "DP SERVICE";//"AUTO_CLOSE";
        public const string AutoCloseSpecialOutUser = "AUTO_CLOSE_SPEC_OUT";
        public const string PermPassUser = "PERMISSION_PASS";
        public const string ReaderControlUser = "READER_CONTROL";
        public const string AutoCheckUser = "AUTO CHECK";

        // Purpose
        public const string ReportPurpose = "REPORT";
        public const string PassPurpose = "PASS";
        public const string IOPairPurpose = "IOPAIR";
        public const string LocationPurpose = "LOCATION";
        public const string PermissionPurpose = "PERMISSION";
        public const string AbsencesPurpose = "ABSENCES";
        public const string EmployeesPurpose = "EMPLOYEES";
        public const string ExtraHoursPurpose = "EXTRA HOURS";
        public const string PermVerificationPurpose = "PERM VERIFICATION";
        public const string RestaurantPurpose = "RESTAURANT";
        public const string VacationPurpose = "VACATION";

        // USERS
        public const string sysUser = "SYS";
        public const int numOfTries = 3;

        // Calendar
        public const int calendarHSpace = 2;
        public const int calendarVSpace = 2;
        public const int calendarLeft = 5;
        public const int calendarTop = 5;

        // PDF document
        public const string pdfFont = "Arial";
        public const int pdfFontSize = 9;
        public const int pdfFontSize12 = 12;
        public const int pdfFontSize14 = 14;
        public const int pdfFontSize16 = 16;
        public const int pdfFontSize9 = 9;
        public const int pdfFontSize11 = 11;
        public const int pdfFontSize10 = 10;

        public const int HeaderHeight = 100;
        public const int LeftBoxWidth = 70;
        public const int RightBoxWidth = 70;

        public const int TopMargine = 50;
        public const int LeftMargine = 50;
        public const int RightMargine = 50;
        public const int BottomMargine = 50;

        public const int PDFStandardPageWidth = 612;
        public const int PDFStandardPageHeight = 792;

        // PDF table
        public const int RowHeightMax = 20;
        public const int RowHeightMin = 15;

        // Report records
        public const int warningLocReportRecords = 5000;
        public const int maxLocReportRecords = 17000;
        public const int warningWUReportRecords = 5000;
        public const int maxWUReportRecords = 17000;
        public const int warningEmplReportRecords = 5000;
        public const int maxEmplReportRecords = 17000;
        public const int warningEmplAnaliticReportRecords = 5000;
        public const int maxEmplAnaliticReportRecords = 17000;
        public const int warningMonthlyReportRecords = 5000;
        public const int maxMonthlyReportRecords = 17000;
        public const int warningPaymentReportRecords = 5000;
        public const int maxPaymentReportRecords = 17000;

        //Log types
        public const int passAntenna1 = 1;
        public const int passAntenna0 = 0;

        // Pass Types
        public const int otherPaymentCode = 0;
        public const int passOnReader = 1;
        public const int wholeDayAbsence = 2;
        public const int overtimePassType = 3;
        public const int overtimeUnjustified = -1000;
        public const int absence = -100;
        public const int extraHoursIOPairInsertPassType = -101;
        public const int automaticPausePassType = -102;
        public const int automaticShortBreakPassType = -103;
        public const int extraHoursAheadIOPairInsertPassType = -104;
        public const int nightShiftWork = 6; // Nenad
        public const int overTimeSundayID = 99; // Nenad
        public const int overTimeID = 4; // Nenad
        public const int earnedHoursID = 48; // Nenad
        public const int workOnHoliday1 = 5;
        public const int pause1 = 9;
        public const int justifiedAbsence1 = 13;
        public const int usedBankHours1 = 42;
        public const int bankHours1 = 48;
        public const int homeWork1 = 98;
        public const int workHolidayPlusNightWOrk1 = 106;
        public const int nightWorkOvertime1 = 108;
        public const int nightWorkOvertimeUnjustified1 = 88;
        public const int neopravdaniPrekovremeni = 60;

        // Payment Report
        public const int pause = 1;
        public const int nightWork = -1;
        public const int late = -2;
        public const int holiday = -3;
        public const int workOnHoliday = -4;
        public const int weekendWork = -7;
        public const int overTime = -8;
        public const int regularWork = 0;
        public const int officialOut = 2;
        public const int tickets = -9;
        public const int vacation = 5;
        public const int privateOut = 3;
        public const int privateOutExtra = 10;
        public const int extraHours = -101;
        public const int automaticPause = -102;
        public const int shortBreak = -103;
        public const int otherOut = 1006;
        public const int officialTravel = 7;
        public const int dayOff = 4;
        public const int delay = -2000;

        public const int YUBMESSortBreakNum = 1000;

        //Graph report of employee presence
        public const int sickLeave = 6;

        //Statistic graphic report types
        public const int physicalAttendanceGraphType = 0;
        public const int wholeDayAbsenceGraphType = 1;
        public const int absenceDuringWorkingTimeGraphType = 2;
        public const int extraHoursGraphType = 3;

        //ATBFOD report's
        public static Hashtable sickLeaveTable
        {
            get
            {
                Hashtable table = new Hashtable();
                table.Add(6, 6);
                table.Add(10, 10);
                table.Add(11, 11);
                table.Add(12, 12);
                table.Add(13, 13);
                table.Add(14, 14);
                table.Add(15, 15);
                table.Add(17, 17);
                return table;
            }
        }
        public static Hashtable vacationTable
        {
            get
            {
                Hashtable table = new Hashtable();
                table.Add(5, 5);
                return table;
            }
        }
        public static Hashtable overTimeTable
        {
            get
            {
                Hashtable table = new Hashtable();
                table.Add(-101, -101);
                return table;
            }
        }
        public static Hashtable payedAbsenceTable
        {
            get
            {
                Hashtable table = new Hashtable();
                table.Add(9, 9);
                return table;
            }
        }
        public static Hashtable businessTripTable
        {
            get
            {
                Hashtable table = new Hashtable();
                table.Add(7, 7);
                table.Add(19, 19);
                return table;
            }
        }
        public static Hashtable extraHoursTable
        {
            get
            {
                Hashtable table = new Hashtable();
                table.Add(-101, -101);
                return table;
            }
        }

        public const string regularTimeSchema = "NORMAL";

        // Night work
        public const int startNightHour = 22;
        public const int endNightHour = 6;

        // Default State Active
        public const string DefaultStateActive = "ACTIVE";

        public const string yes = "YES";
        public const string no = "NO";

        public const string yesL = "Yes";
        public const string noL = "No";

        // Employee type
        public const string emplOrdinary = "ORDINARY";
        public const string emplExtraOrdinary = "EXTRAORDINARY";
        public const string emplSpecial = "SPECIAL";

        // Default location
        public const int defaultLocID = 0;

        // Default working group
        public const int defaultWorkingGroupID = 0;

        // Default working group
        public const int defaultWorkingUnitID = 0;

        // Default access group
        public const int defaultAccessGroupID = 0;

        // Default time schema
        public const int defaultSchemaID = 0;

        // Default time schema start cycle day
        public const int defaultStartDay = 0;

        public const int basicVisitorCode = 999;

        //error Codes
        public const int SQLExceptionPrimaryKeyViolation = 2627;

        // Log Processing
        public const int MAXATTEMPTSNUM = 3;
        public const int ATTEMPTINTERVAL = 15;
        public const int DOWNLOADSLEEPINTERVAL = 30;
        public const int PROCESSINGSLEEPINTERVAL = 15;

        public enum Report
        {
            ByLocation = 0,
            ByWorkingUnit = 1,
            ByEmployee = 2,
            ByEmployeeAnalytical = 3,
            ByWorkingUnitSummary = 4,
        }

        // Application Locale
        public const string Lang_sr = "sr-SP-Latn";
        public const string Lang_en = "en-US";
        public const string Lang_fi = "fi";
        public const string DisplayLangSR = "SR / HR / BS";
        public const string DisplayLangEN = "EN";
        public const string DisplayLangFI = "ACTA School";


        // Pass Types sr
        public const string passTypeRO = "RO";	// Rad na dane odmora
        public const string passTypeRP = "RP";	// Rad na praznik
        public const string passTypeP = "P";	// Praznik
        public const string passTypeR = "+";	// Redovan rad
        public const string passTypeR1 = "H:m";	// Redovan rad
        public const string passTypeSD = "SD";	// Slobodan dan
        public const string passTypeO = "O";	// Odmor
        public const string passTypeB = "B";	// Bolovanje
        public const string passTypeSP = "SP";	// Sluzbeni put
        public const string passTypeNO = "NO";	// Neplaceno odsustvo
        public const string passTypeS = "S";	// Suspenzija
        public const string passTypeB42 = "B42"; // Bolovanje preko 42 dana
        public const string passTypeBP = "BP";	// Bolovanje porodiljsko
        public const string passTypeI = "I";	// Invalidi
        public const string passTypeC = "C";	// Cekanje
        public const string passTypePO = "PO";	// Placeno odsustvo
        public const string passTypeCO = "CO";	// Ostala celodnevna odsustva


        public const string LoginFailed = "Login failed!";

        // Reader Control action names
        public const string ActionLog = "LOG";
        public const string ActionTags = "TAGS";
        public const string ActionTimeAccess = "TIMEACCESS";

        // Time constants
        public const string Hour = "Hrs";
        public const string Minute = "Min";
        public const string Day = "Day";

        // Reader Status
        public const string readerStatusEnabled = "ENABLED";
        public const string readerStatusDisabled = "DISABLED";

        //Created By
        public const string extraHoursCreatedBy = "EXTRA_HRS";

        //Pass deleted
        public const string PassDeleted = "DELETED";

        //Schema Types
        public const string schemaTypeNormal = "NORMAL";
        public const string schemaTypeFlexi = "FLEXI";
        public const string schemaTypeNightFlexi = "NIGHT FLEXI";
        public const string schemaTypeIndustrial = "INDUSTRIAL";

        //Visitors purpose
        public const string visitorPrivate = "PRIVATE";
        public const string visitorOfficial = "OFFICIAL";
        public const string visitorOther = "OTHER";

        // Access Control Files constants for database field type
        public const string ACFilesTypeCards = "CARDS";
        public const string ACFilesTypeTAProfile = "TIME_ACCESS_PROFILE";

        // Access Control Files constants for database field delay
        public enum ACFilesDelay
        {
            DontDelay = 0,
            Delay = 1,
        }

        // Access Control Files constants for database field status
        public const string ACFilesStatusUnused = "UNUSED";
        public const string ACFilesStatusInProgress = "IN_PROGRESS";
        public const string ACFilesStatusOverwritten = "OVERWRITTEN";
        public const string ACFilesStatusUsed = "USED";

        public const string whiteheadImage = "whitehead.jpg";

        // Data processing service
        public const string fileInProcessing = "File in processing:";

        // IOPair status
        public const string Complete = "COMPLETE";
        public const string Incomplete = "INCOMPLETE";

        // XML Files constants
        public const string XMLEmployeesFile = "Employees.xml";
        public const string XMLWUsFile = "WorkingUnits.xml";
        public const string XMLTagsFile = "Tags.xml";
        public const string XMLLocationFile = "Location.xml";
        public const string XMLPassTypesFile = "PassTypes.xml";
        public const string XMLReadersFile = "Readers.xml";
        public const string XMLVisitsFile = "Visits.xml";
        public const string XMLPassesFile = "Passes.xml";
        public const string XMLIOPairsFile = "IOPairs.xml";
        public const string XMLVisitsUpdateFile = "UpdateVisits.xml";
        public const string XMLPassesUpdateFile = "UpdatePasses.xml";

        // Current Reader
        public const string ReaderXMLLogFile = "Log";

        //Camera snapshots constants
        public static TimeSpan minutesOffsetForFileTime = new TimeSpan(0, 10, 0);
        public static TimeSpan difoltAverrageError = new TimeSpan(0, 0, 3);
        public static TimeSpan diffFileCameraCTforAvg = new TimeSpan(0, 0, 10);
        public static TimeSpan diffFileCameraCTforEfc = new TimeSpan(0, 1, 0);

        public const int MAXNUMANTENNAS = 3;

        // XML Data Provider
        public const int XMLDataProvider = 4;

        // connection string not found
        public const string connStringNotFound = "Connection string not found";

        // server and port default
        public const string mysqlServerDefault = "localhost";
        public const string sqlServerDefault = "(local)";
        public const string mysqlPortDefault = "3306";
        public const string dataBaseDefault = "ACTA";

        // connection string
        public const string dataProvider = "data provider=";
        public const string dataProviderMySQL = "mysql;";
        public const string dataProviderSQLServer = "sqlserver;";
        public const string server = "server=";
        public const string port = " port=";
        public const string dataBase = "database=";
        public const string sqlServerUID = "uid=actamgr;";
        public const string sqlServerPwd = "pwd=actamgr2005";
        public const string mysqlUID = " user id=root;";
        public const string mysqlPwd = " password=password;";
        public const string pooling = " pooling=false;";

        //siemens connection properties
        public const string sqlServerSiemensUID = "uid=";
        public const string sqlServerSiemensPwd = "pwd=";
        public const string sqlServerSiemensTable = "table=";
        public const string sqlServerSiemensUser = "siPassUser=";


        // cryption constants
        public const string DESKey = "77TXjh+Stx0=";
        public const string DESIV = "fTTHDkEQ6kM=";

        //Days of week
        public const string Monday = "Mon";
        public const string Sunday = "Sun";
        public const string Saturday = "Sat";

        // licence constants
        public const int noLicence = 0;
        public const int invalidLicence = 1;
        public const int maxConnNumExceeeded = 2;
        public const int functionalityNotValid = 3;
        public const int menuItemsNotUpdated = 4;
        public const int serverPortLength = 6;
        public const int serverNameLength = 63;
        public const int noSessionsLength = 6;
        public const int modulLength = 6;
        public const int modulNum = 24;
        public const int customerLength = 6;
        public const int ADMINRoleID = 0;
        public const string licenceKeyValue = "VOID";
        public const string licencePassword = "licgen";
        public const string timeTable1Pos1 = "010_020";
        public const string timeTable1Pos2 = "020_010_020";
        public const string timeTable2Pos1 = "050_050";
        public const string absencePos1 = "030_030";
        public const string exitPermitsPos1 = "030_040";
        public const string extraHoursPos1 = "030_050";
        public const string extraHoursPos2 = "020_010_020_040";
        public const string accessControlPos1 = "050_030";
        public const string snapshotsPos1 = "010_080";
        public const string snapshotsPos2 = "050_040";
        public const string snapshotsPos3 = "020_010_040";
        public const string snapshotsPos4 = "040_050_020";
        public const string monitoringPos1 = "060";
        public const string visitorsPos1 = "070";
        public const string visitorsPos2 = "020_010_050";
        public const string visitorsPos3 = "040_050_030";
        public const string customizedReportsPos = "020_020";
        public const string graficalReportsPos = "020_015";
        public const string restaurantPos1 = "010_085";
        public const string restaurantPos2 = "075";
        public const string routesTagPos1 = "010_087";
        public const string routesTagPos2 = "020_010_070";
        public const string routesTerminalPos1 = "010_088";
        public const string routesTerminalPos2 = "020_010_071";
        public const string peopleCounterPos1 = "020_050";
        public const string peopleCounterPos2 = "020_050_010";
        public const string peopleCounterPos3 = "020_050_020";
        public const string peopleCounterPos4 = "020_050_030";
        public const string SiemensCompatibilityPos1 = "040_070";
        public const string SiemensCompatibilityPos2 = "040_080";
        public const string vacationPos1 = "030_060";
        public const string unipromPos1 = "040_090";
        public const string selfServ = "030_070";
        public const string MC = "010_091";
        public const string MCDisabilities = "010_091_030";
        public const string MCRisks = "010_091_010";
        public const string MCRisksXPositions = "010_091_040";
        public const string MCVaccines = "010_091_020";
	
        public const int noPermition = 0;
        public const int allPermition = 15;
        public const int rolesNum = 40;
        public const string noCustomer = "000";
        public const string autoUser = "AUTO";

        public static int LicenceLength
        {
            get
            {
                // no_sessions (number of concurrent sessions)			6 characters
                // database_server_port						6 characters
                // database_server_name						63 characters
                // no_sessions_ctrl (same as no_sessions – for later control)	6 characters
                // check sum                                1 character
                // 18 moduls                                6 characters each
                // customer                                 6 characters
                return noSessionsLength + serverPortLength + serverNameLength + noSessionsLength
                    + 1 + modulNum * modulLength + customerLength;
            }
        }
        public static int LicenceReqLength
        {
            get
            {
                // server port contains 6 characters, server name contains 63 characters, sessions num contains 6 characters
                // 18 moduls contains 6 characters each and customer contains 6 characters
                return serverPortLength + serverNameLength + noSessionsLength
                    + modulNum * modulLength + customerLength;
            }
        }

        public static List<string> MinVisibilityPos
        {
            get
            {
                List<string> minVisibilityPos = new List<string>();

                minVisibilityPos.Add("080");
                minVisibilityPos.Add("080_010");
                minVisibilityPos.Add("080_020");
                minVisibilityPos.Add("010");
                minVisibilityPos.Add("010_500");

                return minVisibilityPos;
            }
        }

        public static List<string> TempNonVisibilePos
        {
            get
            {
                List<string> tempNonVisiblePos = new List<string>();

                tempNonVisiblePos.Add("010_020_040");
                tempNonVisiblePos.Add("040_010");
                tempNonVisiblePos.Add("040_020");
                tempNonVisiblePos.Add("040_030");
                tempNonVisiblePos.Add("040_030_010");
                tempNonVisiblePos.Add("040_030_020");
                tempNonVisiblePos.Add("040_030_030");
                tempNonVisiblePos.Add("080_030");
                tempNonVisiblePos.Add("040_060");

                return tempNonVisiblePos;
            }
        }

        public static List<string> SiemensUnvisible
        {
            get
            {
                List<string> tempNonVisiblePos = new List<string>();

                tempNonVisiblePos.Add("010_050");
                tempNonVisiblePos.Add("010_060");
                tempNonVisiblePos.Add("010_070");
                tempNonVisiblePos.Add("010_090");
                tempNonVisiblePos.Add("010_085");
                tempNonVisiblePos.Add("040_040");
                tempNonVisiblePos.Add("040_050");
                tempNonVisiblePos.Add("040_060");
                tempNonVisiblePos.Add("060");
                tempNonVisiblePos.Add("075");
                tempNonVisiblePos.Add("020_010_010");
                tempNonVisiblePos.Add("020_010_030");
                tempNonVisiblePos.Add("020_040");

                return tempNonVisiblePos;
            }
        }


        public static List<string> PeopleCounterBasicVisiblePos
        {
            get
            {
                List<string> peopleCounterVisiblePos = new List<string>();

                peopleCounterVisiblePos.Add("010");
                peopleCounterVisiblePos.Add("010_050");
                peopleCounterVisiblePos.Add("010_060");
                peopleCounterVisiblePos.Add("010_070");
                peopleCounterVisiblePos.Add("010_500");
                peopleCounterVisiblePos.Add("020");
                peopleCounterVisiblePos.Add("020_050");
                peopleCounterVisiblePos.Add("020_050_030");
                peopleCounterVisiblePos.Add("040");
                peopleCounterVisiblePos.Add("040_040");
                peopleCounterVisiblePos.Add("040_060");
                peopleCounterVisiblePos.Add("080");
                peopleCounterVisiblePos.Add("080_020");
                peopleCounterVisiblePos.Add("080_030");

                return peopleCounterVisiblePos;
            }
        }

        public static List<string> SelfServiceVisiblePos
        {
            get
            {
                List<string> selfServiceVisiblePos = new List<string>();

                selfServiceVisiblePos.Add("030_070");

                return selfServiceVisiblePos;
            }
        }

        public static List<string> PeopleCounterStandardVisiblePos
        {
            get
            {
                List<string> peopleCounterVisiblePos = new List<string>();

                peopleCounterVisiblePos.Add("010");
                peopleCounterVisiblePos.Add("010_050");
                peopleCounterVisiblePos.Add("010_060");
                peopleCounterVisiblePos.Add("010_070");
                peopleCounterVisiblePos.Add("010_500");
                peopleCounterVisiblePos.Add("020");
                peopleCounterVisiblePos.Add("020_050");
                peopleCounterVisiblePos.Add("020_050_010");
                peopleCounterVisiblePos.Add("020_050_030");
                peopleCounterVisiblePos.Add("040");
                peopleCounterVisiblePos.Add("040_040");
                peopleCounterVisiblePos.Add("040_060");
                peopleCounterVisiblePos.Add("080");
                peopleCounterVisiblePos.Add("080_020");
                peopleCounterVisiblePos.Add("080_030");

                return peopleCounterVisiblePos;
            }
        }

        public static List<string> PeopleCounterAdvanceVisiblePos
        {
            get
            {
                List<string> peopleCounterVisiblePos = new List<string>();

                peopleCounterVisiblePos.Add("010");
                peopleCounterVisiblePos.Add("010_050");
                peopleCounterVisiblePos.Add("010_060");
                peopleCounterVisiblePos.Add("010_070");
                peopleCounterVisiblePos.Add("010_500");
                peopleCounterVisiblePos.Add("020");
                peopleCounterVisiblePos.Add("020_050");
                peopleCounterVisiblePos.Add("020_050_010");
                peopleCounterVisiblePos.Add("020_050_020");
                peopleCounterVisiblePos.Add("020_050_030");
                peopleCounterVisiblePos.Add("040");
                peopleCounterVisiblePos.Add("040_040");
                peopleCounterVisiblePos.Add("040_060");
                peopleCounterVisiblePos.Add("080");
                peopleCounterVisiblePos.Add("080_020");
                peopleCounterVisiblePos.Add("080_030");

                return peopleCounterVisiblePos;
            }
        }

        // Hashtable - key is modul number, and value is ArrayList of positions of corresponding menu items
        public static Dictionary<int, List<string>> ModulsMenuItems
        {
            get
            {
                Dictionary<int, List<string>> modMenuItems = new Dictionary<int, List<string>>();
                List<string> positions = new List<string>();

                positions.Add(timeTable1Pos1);
                positions.Add(timeTable1Pos2);
                modMenuItems.Add((int)Moduls.TimeTable1, positions);

                positions = new List<string>();
                positions.Add(timeTable2Pos1);
                modMenuItems.Add((int)Moduls.TimeTable2, positions);

                positions = new List<string>();
                positions.Add(absencePos1);
                modMenuItems.Add((int)Moduls.Absences, positions);

                positions = new List<string>();
                positions.Add(exitPermitsPos1);
                modMenuItems.Add((int)Moduls.ExitPermits, positions);

                positions = new List<string>();
                positions.Add(extraHoursPos1);
                positions.Add(extraHoursPos2);
                modMenuItems.Add((int)Moduls.ExtraHours, positions);

                positions = new List<string>();
                positions.Add(accessControlPos1);
                modMenuItems.Add((int)Moduls.AccessControl, positions);

                positions = new List<string>();
                positions.Add(snapshotsPos1);
                positions.Add(snapshotsPos2);
                positions.Add(snapshotsPos3);
                positions.Add(snapshotsPos4);
                modMenuItems.Add((int)Moduls.Snapshots, positions);

                positions = new List<string>();
                positions.Add(monitoringPos1);
                modMenuItems.Add((int)Moduls.Monitoring, positions);

                positions = new List<string>();
                positions.Add(visitorsPos1);
                positions.Add(visitorsPos2);
                positions.Add(visitorsPos3);
                modMenuItems.Add((int)Moduls.Visitors, positions);

                positions = new List<string>();
                positions.Add(graficalReportsPos);
                modMenuItems.Add((int)Moduls.GraficalReports, positions);

                positions = new List<string>();
                positions.Add(restaurantPos1);
                positions.Add(restaurantPos2);
                modMenuItems.Add((int)Moduls.Restaurant, positions);

                positions = new List<string>();
                modMenuItems.Add((int)Moduls.RestaurantII, positions);

                positions = new List<string>();
                modMenuItems.Add((int)Moduls.RestaurantI, positions);

                positions = new List<string>();
                positions.Add(routesTagPos1);
                positions.Add(routesTagPos2);
                modMenuItems.Add((int)Moduls.RoutesTag, positions);

                positions = new List<string>();
                positions.Add(routesTerminalPos1);
                positions.Add(routesTerminalPos2);
                modMenuItems.Add((int)Moduls.RoutesTerminal, positions);

                positions = new List<string>();
                positions.Add(peopleCounterPos1);
                positions.Add(peopleCounterPos4);
                modMenuItems.Add((int)Moduls.PeopleCounterBasic, positions);

                positions = new List<string>();
                positions.Add(peopleCounterPos1);
                positions.Add(peopleCounterPos2);
                positions.Add(peopleCounterPos4);
                modMenuItems.Add((int)Moduls.PeopleCounterStandard, positions);

                positions = new List<string>();
                positions.Add(peopleCounterPos1);
                positions.Add(peopleCounterPos2);
                positions.Add(peopleCounterPos3);
                positions.Add(peopleCounterPos4);
                modMenuItems.Add((int)Moduls.PeopleCounterAdvance, positions);

                positions = new List<string>();
                positions.Add(SiemensCompatibilityPos1);
                positions.Add(SiemensCompatibilityPos2);
                modMenuItems.Add((int)Moduls.SiemensCompatibility, positions);

                positions = new List<string>();
                positions.Add(vacationPos1);
                modMenuItems.Add((int)Moduls.Vacation, positions);

                positions = new List<string>();
                positions.Add(unipromPos1);
                modMenuItems.Add((int)Moduls.UNIPROM, positions);

                positions = new List<string>();
                positions.Add(selfServ);
                modMenuItems.Add((int)Moduls.SelfService, positions);

                positions = new List<string>();
                positions.Add(MC);
                positions.Add(MCDisabilities);
                positions.Add(MCRisks);
                positions.Add(MCRisksXPositions);
                positions.Add(MCVaccines);
                modMenuItems.Add((int)Moduls.MedicalCheck, positions);

                return modMenuItems;
            }
        }

        // config add form constants
        public const int gateAuxPortWidth = 325;
        public const int gateAuxPortHeight = 30;
        public const int readerAuxPortWidth = 190;
        public const int readerAuxPortHeight = 30;
        public const int toolTipDuration = 1000;

        public const string offlineWork = "NO";

        // privileges constants
        public const string PrivilegesSR = "Privilegije";
        public const string PrivilegesEN = "Privileges";
        public const string PrivilegesFI = "Privilegije";

        public const string addForm = "ADD";
        public const string updateForm = "UPDATE";

        // web report constants
        public const string trueValue = "true";
        public const string falseValue = "false";
        public const string analytical = "analytical";
        public const string summary = "summary";
        public const int objectTransferCount = 4000;

        //html help constants, key is Form name, member is link
        public static Hashtable HelpLinks
        {
            get
            {
                Hashtable helpLinks = new Hashtable();
                helpLinks.Add("ACTA", "#_Top");
                helpLinks.Add("AccessControlStatus", "#_Toc205183164");
                helpLinks.Add("AccessGroupsXGates", "#_Toc205183158");
                helpLinks.Add("Adresses", "#_Toc205183112");
                helpLinks.Add("ApplRoles", "#_Toc205183153");
                helpLinks.Add("ApplRolesAdd", "#_Toc205183153");
                helpLinks.Add("ApplUsers", "#_Toc205183152");
                helpLinks.Add("ApplUsersAdd", "#_Toc205183152");
                helpLinks.Add("ApplUsersLog", "#_Toc205183152");
                helpLinks.Add("ApplUsersXRoles", "#_Toc205183154");
                helpLinks.Add("ApplUsersXWU", "#_Toc205183144");
                helpLinks.Add("CameraAdd", "#_Toc205183125");
                helpLinks.Add("Cameras", "#_Toc205183125");
                helpLinks.Add("ChangePassword", "#_Toc205183108");
                helpLinks.Add("EmployeeAbsencesAdd", "#_Toc205183143");
                helpLinks.Add("EmployeeAbsences", "#_Toc205183143");
                helpLinks.Add("EmployeeAccessGroupAdd", "#_Toc205183156");
                helpLinks.Add("EmployeeAccessGroups", "#_Toc205183156");
                helpLinks.Add("EmployeeAdd", "#_Toc205183112");
                helpLinks.Add("EmployeeLocations", "#_Toc205183137");
                helpLinks.Add("EmployeePresenceGraphicReports", "#_Toc205183173");
                helpLinks.Add("Employees", "#_Toc205183112");
                helpLinks.Add("EmployeesXAccessGroups", "#_Toc205183156");
                helpLinks.Add("EmployeesXWorkingUnits", "#_Toc205183119");
                helpLinks.Add("EmplWorkingGroupChanged", "#_Toc205183112");
                helpLinks.Add("ExitPermissionsWorkingDayBeginning", "#_Toc205183144");
                helpLinks.Add("ExitPermissionPasses", "#_Toc205183144");
                helpLinks.Add("ExitPermissions", "#_Toc205183144");
                helpLinks.Add("ExitPermissionsAdd", "#_Toc205183144");
                helpLinks.Add("ExitPermissionsVerification", "#_Toc205183144");
                helpLinks.Add("ExtraHours", "#_Toc205183145");
                helpLinks.Add("ExtraHoursCalculation", "#_Toc205183145");
                helpLinks.Add("Gates", "#_Toc205183123");
                helpLinks.Add("GatesAdd", "#_Toc205183123");
                helpLinks.Add("GatesXGateTimeProfile", "#_Toc205183162");
                helpLinks.Add("GatesXGateTimeProfileUpd", "#_Toc205183162");
                helpLinks.Add("GateTimeAccessProfileAdd", "#_Toc205183161");
                helpLinks.Add("GateTimeAccessProfiles", "#_Toc205183161");
                helpLinks.Add("Holidays", "#_Toc205183115");
                helpLinks.Add("HolidaysAdd", "#_Toc205183115");
                helpLinks.Add("IOPairs", "#_Toc205183142");
                helpLinks.Add("IOPairsAdd", "#_Toc205183142");
                helpLinks.Add("Locations", "#_Toc205183122");
                helpLinks.Add("LocationsAdd", "#_Toc205183122");
                helpLinks.Add("logInForm", "#_Toc205183107");
                helpLinks.Add("Monitor", "#_Toc205183165");
                helpLinks.Add("Passes", "#_Toc205183141");
                helpLinks.Add("PassesAdd", "#_Toc205183141");
                helpLinks.Add("PassesHist", "#_Toc205183141");
                helpLinks.Add("PassesSnapshots", "#_Toc205183141");
                helpLinks.Add("PassTypeAdd", "#_Toc205183122");
                helpLinks.Add("PassTypes", "#_Toc205183122");
                helpLinks.Add("ProlongTS", "#_Toc205183117");
                helpLinks.Add("ReaderAdvanceSettings", "#_Toc205183125");
                helpLinks.Add("Readers", "#_Toc205183125");
                helpLinks.Add("ReadersAdd", "#_Toc205183125");
                helpLinks.Add("RolesPermissions", "#_Toc205183152");
                helpLinks.Add("StatisticGraphicReports", "#_Toc205183174");
                helpLinks.Add("Tags", "#_Toc205183112");
                helpLinks.Add("ApplUserXWU", "#_Toc205183150");
                helpLinks.Add("TimeAccessProfileAdd", "#_Toc205183160");
                helpLinks.Add("TimeAccessProfiles", "#_Toc205183160");
                helpLinks.Add("TimeSchemaPauseAdd", "#_Toc205183118");
                helpLinks.Add("TimeSchemaPauses", "#_Toc205183118");
                helpLinks.Add("Visitors", "#_Toc205183167");
                helpLinks.Add("VisitorsView", "#_Toc205183169");
                helpLinks.Add("WorkingUnits", "#_Toc205183119");
                helpLinks.Add("WorkingUnitsAdd", "#_Toc205183119");
                helpLinks.Add("WTEmployeesTimeSchedule", "#_Toc205183112");
                helpLinks.Add("WTGroups", "#_Toc205183116");
                helpLinks.Add("WTGroupsAdd", "#_Toc205183116");
                helpLinks.Add("WTSchema", "#_Toc205183114");
                helpLinks.Add("WTSchemaAdd", "#_Toc205183114");
                helpLinks.Add("WTSchemaAdvance", "#_Toc205183114");
                helpLinks.Add("WTSchemaPeriod", "#_Toc205183114");
                helpLinks.Add("WTWorkingGroupTimeSchedules", "#_Toc205183116");
                helpLinks.Add("WorkingUnitsReports", "#_Toc205183130");
                helpLinks.Add("EmployeePresenceType", "#_Toc205183133");
                helpLinks.Add("EmployeesReports", "#_Toc205183131");
                helpLinks.Add("EmpoyeeAnaliticReport", "#_Toc205183135");
                helpLinks.Add("LocationsReports", "#_Toc205183129");
                helpLinks.Add("PresenceTracking", "#_Toc205183132");
                helpLinks.Add("Video_surveillance", "#_Toc205183138");
                helpLinks.Add("VisitorReports", "#_Toc205183139");
                helpLinks.Add("VisitorReportsDetails", "#_Toc205183139");
                helpLinks.Add("EmployeeAnaliticalWU", "#_Toc205183130");
                helpLinks.Add("CamSnapshotMaintenance", "#_Toc205183170");
                helpLinks.Add("EmplPhotosMaintenance", "#_Toc205183169");
                helpLinks.Add("TrespassReports", "#_Toc205183171");
                helpLinks.Add("VisitorsMaintenance", "#_Toc205183175");
                helpLinks.Add("Restaurant", "#_Toc205183177");
                helpLinks.Add("MealTypesAdd", "#_Toc205183177");
                helpLinks.Add("MealTypeEmplAdd", "#_Toc205183177");
                helpLinks.Add("MealPointAdd", "#_Toc205183177");
                helpLinks.Add("MealAssignedAdd", "#_Toc205183177");
                helpLinks.Add("SecurityRoutes", "#_Toc205183178");
                helpLinks.Add("SecurityRoutesAdd", "#_Toc205183178");
                helpLinks.Add("SecurityRoutesReadersAdd", "#_Toc205183178");
                helpLinks.Add("SecurityRoutesScheduleAdd", "#_Toc205183178");
                helpLinks.Add("SecurityRoutesPointsAdd", "#_Toc205183178");
                helpLinks.Add("Maps", "#_Toc205183182");
                helpLinks.Add("MapAdd", "#_Toc205183182");
                helpLinks.Add("MapObjects", "#_Toc205183183");
                helpLinks.Add("MapView", "#_Toc205183184");
                helpLinks.Add("ExitPermissionsAddAdvanced", "#_Toc205183185");
                helpLinks.Add("ExitPermissionsPreview", "#_Toc205183185");
                helpLinks.Add("ExitPermDaysSelection", "#_Toc205183185");
                helpLinks.Add("VacationEvidence", "#_Toc205183186");
                helpLinks.Add("VacationDetails", "#_Toc205183186");
                helpLinks.Add("VacationAdd", "#_Toc205183186");
                helpLinks.Add("Lockings", "_Toc205183190");
                helpLinks.Add("EmplPersonalRecordsMDI", "#_Toc205183140");
                helpLinks.Add("TagsPreview", "#_Toc205183191");
                return helpLinks;
            }
        }

        //menuItemID constants, key is Form name, member is menu item ID
        public static Hashtable ItemIDsSerbian
        {
            get
            {
                Hashtable ItemIDs = new Hashtable();

                ItemIDs.Add("Passes", "Intervencije_Prolasci");
                ItemIDs.Add("IOPairs", "Intervencije_Parovi ulaz/izlaz");
                ItemIDs.Add("EmployeeAbsences", "Intervencije_Odsustva zaposlenih");
                ItemIDs.Add("ExitPermissions", "Intervencije_Propusnice");
                ItemIDs.Add("ExtraHours", "Intervencije_Preraspodela");
                ItemIDs.Add("VacationEvidence", "Intervencije_Evidencija godišnjih odmora");
                ItemIDs.Add("EmployeesReports", "Izveštaji_Standardni izveštaji_Izveštaji o prisutnosti_Izveštaji po zaposlenima");
                ItemIDs.Add("EmployeeAnaliticReport", "Izveštaji_Standardni izveštaji_Izveštaji o radnom vremenu_Izveštaj o dolascima zaposlenog");
                return ItemIDs;
            }
        }

        public static Hashtable ItemIDsEnglish
        {
            get
            {
                Hashtable ItemIDs = new Hashtable();

                ItemIDs.Add("Passes", "Interventions_Passes");
                ItemIDs.Add("IOPairs", "Interventions_IO Pairs");
                ItemIDs.Add("EmployeeAbsences", "Interventions_Employee absences");
                ItemIDs.Add("ExitPermissions", "Interventions_Exit permissions");
                ItemIDs.Add("ExtraHours", "Interventions_Extra hours");
                ItemIDs.Add("VacationEvidence", "Interventions_Vacation evidence");
                ItemIDs.Add("EmployeesReports", "Reports_Standard reports_Presence reports_Reports for employees");
                ItemIDs.Add("EmployeeAnaliticReport", "Reports_Standard reports_Time schedule reports_Reports for employee - analytic");
                return ItemIDs;
            }
        }
        //menuItemID constants, key is Form name, member is menu item ID
        public static Hashtable ItemIDsFi
        {
            get
            {
                Hashtable ItemIDs = new Hashtable();

                ItemIDs.Add("Passes", "Intervencije_Prolasci");
                ItemIDs.Add("IOPairs", "Intervencije_Parovi ulaz/izlaz");
                ItemIDs.Add("EmployeeAbsences", "Intervencije_Odsustva zaposlenih");
                ItemIDs.Add("ExitPermissions", "Intervencije_Propusnice");
                ItemIDs.Add("ExtraHours", "Intervencije_Preraspodela");
                ItemIDs.Add("VacationEvidence", "Intervencije_Evidencija godišnjih odmora");
                ItemIDs.Add("EmployeesReports", "Izveštaji_Standardni izveštaji_Izveštaji o prisutnosti_Izveštaji po zaposlenima");
                ItemIDs.Add("EmployeeAnaliticReport", "Izveštaji_Standardni izveštaji_Izveštaji o radnom vremenu_Izveštaj o dolascima zaposlenog");
                return ItemIDs;
            }
        }
        //Camera snapshot maintenance
        public const double megabytes = 1048576;

        // Meals assigned constants
        public const int unlimited = -1;
        public const int undefined = 0;

        // Log event constants
        public const int unknownCard = 1;
        public const int cardDisapproved = 3;

        // Role permission cell constants
        public const string read = "READ";
        public const string add = "ADD";
        public const string update = "UPDATE";
        public const string delete = "DELETE";

        // scan document constants
        public const double mm2inch = 25.4;
        public const int scanDocWidth = 140;
        public const int scanDocHeight = 90;
        public const int defaultDocType = 0;

        //scanDocZIN
        public const int scanDocWidthZIN = 112;
        public const int scanDocHeightZIN = 72;

        // horizontal resolution
        public static float DpiX
        {
            get
            {
                Bitmap b = new Bitmap(1, 1);
                Graphics g = System.Drawing.Graphics.FromImage(b);

                return g.DpiX;
            }
        }

        // vertical resolution
        public static float DpiY
        {
            get
            {
                Bitmap b = new Bitmap(1, 1);
                Graphics g = System.Drawing.Graphics.FromImage(b);

                return g.DpiY;
            }
        }

        // Security routes constants
        public const int defaultNextVisit = 60;
        public const string statusCompleted = "COMPLETED";
        public const string statusNotCompleted = "NOT COMPLETED";
        public const string statusPartially = "PARTIALLY";
        public const string statusScheduled = "SCHEDULED";
        public const string routeTag = "TAG";
        public const string routeTerminal = "TERMINAL";

        //Map object types
        public const string readerObjectType = "READER";
        public const string gateObjectType = "GATE";
        public const string cameraObjectType = "CAMERA";
        public const string locationObjectType = "LOCATION";

        public const string dbSetupACTA = "ACTA";
        public const string dbSetupInfo = "Info";

        //Graph settings strings
        public const string GraphType = "GraphType";
        public const string GraphTimeFrom = "GraphTimeFrom";
        public const string GraphTimeTo = "GraphTimeTo";
        public const string GraphPeriod = "GraphPeriod";
        public const string GraphDevide = "GraphDevide";
        public const string GraphDEMO = "GraphDEMO";

        public const int GraphfromHour = 8;
        public const int GraphtoHour = 17;

        public const int NumberOfPasses = 256;

        // Reports
        public const int maxPassTypes = 25;
        public const int maxLegendDesc = 2;

        // Siemens
        public const string SiemensDataBase = "SYNC";
        public const string SiemensUser = "dbo";
        public const string SiemensTableName = "AuditTrail";
        public const string SiemensDataBaseDesc = "Asco4";
        public const string SiemensACTAUser = "ACTAReaderInterfaceS";
        public const int SiemensWorkingUnitID = 0;
        public const int SiemensGroupID = 0;
        public const int SiemensAccessGroupID = 0;
        public const int SiemensPointCounter = 1;
        public const int SiemensPointNOTCounter = 0;
        public const int SiemensPointPassReading = 1;
        public const int SiemensPointNOTPassReading = 0;
        public const int SiemensType = 21;
        public const int SiemensLogID = -1;
        public const int SiemensReaderID = 0;
        public const int SiemensAntenna = 0;
        public const int SiemensEventHappened = 4;
        public const int SiemensActionCommited = 20;
        public const int SiemensPassGanUsed = 0;
        public const int SiemensDepartmentLevel = 3;
        public const int SiemensWorkgroupMaxNameLenght = 40;
        public const string SiemensRegistrationSkipMessage = "Valid Card Presented";
        public const int SiemensWGLine1Max = 20;
        public const int SiemensWGLine2Max = 17;
        public const int SiemensWGLine3Max = 17;

        public const string SiemensStatusActive = "A";
        public const string SiemensStatusRetired = "R";
        public const string SiemensStatusNotActive = "M";
        public const string SiemensStatusPassive = "P";

        public const string siemensAddFlag = "N";
        public const string siemensDeleteFlag = "D";
        public const string siemensUpdateFlag = "C";
        
        public const string SiemensUnproccessXML = "Log";
        public const string SiemensNoWrkXML = "LogPP";

        public const int SiemensDirectionIn = 0;
        public const int SiemensDirectionOut = 1;

        public const string SiemensRegDirectionIn = "I";
        public const string SiemensRegDirectionOut = "O";

        public const int SiemensDefaultReadStatus = 0;
        public const int SiemensManualCreatedLoc = 0;

        public const int SiemensCategory1 = 0;
        public const int SiemensCategory2 = 1;
        public const int SiemensCategory3 = 2;
        public const int SiemensCategory4 = 3;
        public const int SiemensCategory5 = 4;
        public const int SiemensNoCategory = 1000;

        public const string SiemensEmployeeType = "E";
        public const string SiemensVisitorType = "V";
        public const int SiemensEmployeeInt = 0;
        public const int SiemensVisitorInt = 1;

        public const string SiemensVisitorTypeTruck = "KAMION";
        public const string SiemensVisitorTypeTruckDriver = "VOZAČ KAMIONA";
        public const string SiemensVisitorTypeContractorA = "IZVOĐAČ_A";
        public const string SiemensVisitorTypeContractorB = "IZVOĐAČ_B";
        public const string SiemensVisitorTypeVehicleOfficial = "SLUŽBENO VOZILO";
        public const string SiemensVisitorTypeVehiclePrivate = "PRIVATNO VOZILO";
        public const string SiemensVisitorTypeVisitor = "POSETIOC";

        private const string _siemensStatesAntiPassBack = "39";

        public static string SiemensStatesAntiPassBack
        {
            get
            {
                if (ConfigurationManager.AppSettings["SiemensAntiPassback"] == null)
                {
                    return _siemensStatesAntiPassBack;
                }
                else
                {
                    return ConfigurationManager.AppSettings["SiemensAntiPassback"];
                }
            }
        }

        public static string SiemensAuditTrailValidStates
        {
            get
            {
                if (ConfigurationManager.AppSettings["SiemensAuditTrailValidStates"] == null)
                {
                    return "";
                }
                else
                {
                    return ConfigurationManager.AppSettings["SiemensAuditTrailValidStates"];
                }
            }
        }

        // Hashtable - key is number of category, and value is category name
        public static Hashtable SiemensCategories
        {
            get
            {
                Hashtable categories = new Hashtable();
                categories.Add(SiemensCategory1, "Kategorija 1");
                categories.Add(SiemensCategory2, "Kategorija 2");
                categories.Add(SiemensCategory3, "Kategorija 3");
                categories.Add(SiemensCategory4, "Kategorija 4");
                categories.Add(SiemensCategory5, "Kategorija 5");

                return categories;
            }
        }

        public enum SiemensCardStatus
        {
            Valid = 0,
            Void = 1,
            Expired = 2,
            BeforeStart = 3,
            WorkGroupVoid = 4,
            CardLost = 5,
            Temporary = 6
        }

        //Reader control ping reader interval in seucnds
        public const int pingInterval = 60;

        //Monitor-camera user and password hardcoded
        public const string CammeraUser = "actamgr";
        public const string CammeraPass = "actamgr";

        //Credentail reading constants
        public const int EID_MAX_DocRegNo = 9;
        public const int EID_MAX_IssuingDate = 10;
        public const int EID_MAX_ExpiryDate = 10;
        public const int EID_MAX_IssuingAuthority = 30;

        public const int EID_MAX_PersonalNumber = 13;
        public const int EID_MAX_Surname = 60;
        public const int EID_MAX_GivenName = 40;
        public const int EID_MAX_ParentGivenName = 25;
        public const int EID_MAX_Sex = 2;
        public const int EID_MAX_PlaceOfBirth = 25;
        public const int EID_MAX_StateOfBirth = 25;
        public const int EID_MAX_DateOfBirth = 10;
        public const int EID_MAX_CommunityOfBirth = 25;

        public const int EID_MAX_State = 3;
        public const int EID_MAX_Community = 25;
        public const int EID_MAX_Place = 25;
        public const int EID_MAX_Street = 36;
        public const int EID_MAX_HouseNumber = 5;
        public const int EID_MAX_HouseLetter = 2;
        public const int EID_MAX_Entrance = 3;
        public const int EID_MAX_Floor = 3;
        public const int EID_MAX_ApartmentNumber = 6;

        public const int EID_MAX_Portrait = 7700;

        public const int EID_OK = 0;
        public const int EID_E_GENERAL_ERROR = -1;
        public const int EID_E_INVALID_PARAMETER = -2;
        public const int EID_E_VERSION_NOT_SUPPORTED = -3;
        public const int EID_E_NOT_INITIALIZED = -4;
        public const int EID_E_UNABLE_TO_EXECUTE = -5;
        public const int EID_E_READER_ERROR = -6;
        public const int EID_E_CARD_MISSING = -7;
        public const int EID_E_CARD_UNKNOWN = -8;
        public const int EID_E_CARD_MISMATCH = -9;
        public const int EID_E_UNABLE_TO_OPEN_SESSION = -10;
        public const int EID_E_DATA_MISSING = -11;
        public const int EID_E_CARD_SECFORMAT_CHECK_ERROR = -12;
        public const int EID_E_SECFORMAT_CHECK_CERT_ERROR = -13;

        public const int CelikV = 1;

        public static Hashtable CelikReturnValues
        {
            get
            {
                Hashtable ItemIDs = new Hashtable();

                ItemIDs.Add(0, "OK");
                ItemIDs.Add(-1, "EID_E_GENERAL_ERROR");
                ItemIDs.Add(-2, "EID_E_INVALID_PARAMETER");
                ItemIDs.Add(-3, "EID_E_VERSION_NOT_SUPPORTED");
                ItemIDs.Add(-4, "EID_E_NOT_INITIALIZED");
                ItemIDs.Add(-5, "EID_E_UNABLE_TO_EXECUTE");
                ItemIDs.Add(-6, "EID_E_READER_ERROR");
                ItemIDs.Add(-7, "EID_E_CARD_MISSING");
                ItemIDs.Add(-8, "EID_E_CARD_UNKNOWN");
                ItemIDs.Add(-9, "EID_E_CARD_MISMATCH");
                ItemIDs.Add(-10, "EID_E_UNABLE_TO_OPEN_SESSION");
                ItemIDs.Add(-11, "EID_E_DATA_MISSING");
                ItemIDs.Add(-12, "EID_E_CARD_SECFORMAT_CHECK_ERROR");
                ItemIDs.Add(-13, "EID_E_SECFORMAT_CHECK_CERT_ERROR");
                return ItemIDs;
            }
        }
        # region convert
        public static string ConvertCyrillicToLatin(string cyrillic)
        {
            string c = "";
            string slovo = "";
            int i;
            for (i = 0; i < cyrillic.Length; i++)
            {

                slovo = cyrillic.Substring(i, 1);


                switch (slovo)
                {
                    case "a": c += Convert.ToChar(97); break;
                    case "A": c += Convert.ToChar(65); break;
                    case "б": c += "b"; break;
                    case "Б": c += "B"; break;
                    case "ц": c += "c"; break;
                    case "Ц": c += "C"; break;
                    case "ч": c += "č"; break;
                    case "Ч": c += "Č"; break;
                    case "ћ": c += "ć"; break;
                    case "Ћ": c += "Ć"; break;
                    case "д": c += "d"; break;
                    case "Д": c += "D"; break;
                    case "ђ": c += "đ"; break;
                    case "Ђ": c += "Đ"; break;
                    case "е": c += "e"; break;
                    case "Е": c += "E"; break;
                    case "ф": c += "f"; break;
                    case "Ф": c += "F"; break;
                    case "г": c += "g"; break;
                    case "Г": c += "G"; break;
                    case "х": c += "h"; break;
                    case "Х": c += "H"; break;
                    case "и": c += "i"; break;
                    case "И": c += "I"; break;
                    case "ј": c += "j"; break;
                    case "Ј": c += "J"; break;
                    case "к": c += "k"; break;
                    case "К": c += "K"; break;
                    case "л": c += "l"; break;
                    case "Л": c += "L"; break;
                    case "љ": c += "lj"; break;
                    case "Љ": c += "Lj"; break;
                    case "м": c += "m"; break;
                    case "М": c += "M"; break;
                    case "н": c += "n"; break;
                    case "Н": c += "N"; break;
                    case "о": c += "o"; break;
                    case "О": c += "O"; break;
                    case "п": c += "p"; break;
                    case "П": c += "P"; break;
                    case "р": c += "r"; break;
                    case "Р": c += "R"; break;
                    case "с": c += "s"; break;
                    case "С": c += "S"; break;
                    case "ш": c += "š"; break;
                    case "Ш": c += "Š"; break;
                    case "т": c += "t"; break;
                    case "Т": c += "T"; break;
                    case "у": c += "u"; break;
                    case "У": c += "U"; break;
                    case "в": c += "v"; break;
                    case "В": c += "V"; break;
                    case "з": c += "z"; break;
                    case "З": c += "Z"; break;
                    case "ж": c += "ž"; break;
                    case "Ж": c += "Ž"; break;
                    case "њ": c += "nj"; break;
                    case "Њ": c += "Nj"; break;
                    case "џ": c += "dž"; break;
                    case "Џ": c += "Dž"; break;
                    default:
                        c += slovo;
                        break;
                }
            }
            return c;
        }

        # endregion

        //locking properties
        public const string lockTypeLock = "LOCK";
        public const string lockTypeUnlock = "UNLOCK";

        //control string's
        public const string CONTROL_GROUP_BOX = "System.Windows.Forms.GroupBox";
        public const string CONTROL_TAB_PAGE = "System.Windows.Forms.TabPage";
        public const string CONTROL_TAB_CONTROL = "System.Windows.Forms.TabControl";
        public const string CONTROL_PANEL = "System.Windows.Forms.Panel";
        public const string CONTROL_TEXT_BOX = "System.Windows.Forms.TextBox";
        public const string CONTROL_RICH_TEXT_BOX = "System.Windows.Forms.RichTextBox";
        public const string CONTROL_COMBO_BOX = "System.Windows.Forms.ComboBox";
        public const string CONTROL_DATE_TIME_PICKER = "System.Windows.Forms.DateTimePicker";
        public const string CONTROL_LIST_VIEW = "System.Windows.Forms.ListView";
        public const string CONTROL_NUMERIC_UP_DOWN = "System.Windows.Forms.NumericUpDown";
        public const string CONTROL_RADIO_BUTTON = "System.Windows.Forms.RadioButton";
        public const string CONTROL_CHECK_BOX = "System.Windows.Forms.CheckBox";

        //Filterable control
        public const string CONTROL_Filterable = "FILTERABLE";
        public const string CONTROL_NOT_Filterable = "NOTFILTERABLE";

        //default filter
        public const int filterDefault = 1;
        public const int filterNotDefault = 0;

        //SANYO Camera type
        public const string cameraTypeSANYO = "SANYO HD4000P";

        //UNIPROM working units(drivers and tracks)
        public const int tracksWURootID = 5000;
        public const int driversWURootID = 5001;

        //UNIPROM RAMP Employee and tag
        public const int UNIPROMRampEmployeeID = 100000;
        public const int UNIPROMRampTagID = 1;

        //delay in secunds
        public const int UNIPROMPreDelay = 10;
        public const int UNIPROMPostDelay = 0;

        public const int readerRefreshTime = 10;

        public const int msInSec = 1000;
        public const int secInMin = 60;

        public const int XPocketDelPoint = 12800;

        public const string selfServUser = "SelfService";

        public const int restaurantJUBMESLocID = 5;
        public const string restaurantJUBMESPaymentCode = "9999";
        public const int restaurantJUBMESPassTypeID = 99;

        public const int commandTimeout = 1200; //seconds        

        public const int invalidWU = 6117;
        public const int waitWU = 6118;
        public const int invalidPT = -6;
        public const int waitPT = -5;

        public const int maxLogsForReaderNum = 100;

        public const string cameraTypeAxis = "AXIS";

        // ACTAWeb constants
        public const string dateFormat = "dd.MM.yyyy.";
        public const string yearFormat = "yyyy";
        public const string monthFormat = "MM.yyyy.";
        public const string timeFormat = "HH:mm";
        public const string dateMonthFormat = "dd.MM.";
        public const string doubleFormat = "F2";
        public const int recPerPage = 100;
        //public const string hdrTableColor = "#9999FF";
        //public const string tabTableColor = "#D6E0CC";        
        public const string selItemColor = "#FFFFD1"; //#D1E0FF
        public const string colBorderColor = "#E6E6E6";
        public const string nightWorkLblColor = "#7393C4"; //#A3C2FF
        public const string hourLineColor = "#B2B2B2";
        public const string emplDayViewAltColor = "#E6E6E6";
        public const string calendarDayColor = "#FFFFB8"; //#FFFF99
        public const string calendarWeekEndDayColor = "#FFE6CC"; //#FFCC99
        public const string calendarNationalHolidayDayColor = "#FFCCCC"; //#FFCCCC
        public const string calendarPersonalHolidayDayColor = "#CCB2FF"; //#CCCCFF
        public const int colBorderWidth = 1;
        public const string sortASC = "ASC";
        public const string sortDESC = "DESC";
        public const int chbColWidth = 30;
        public const int scrollWidth = 20;
        public const int updNoSelected = -1;
        public const string dpMySql = "mysql";
        public const string dpSqlServer = "sqlserver";
        public const char delimiter = '|';
        public const char rowDelimiter = '~';
        public const double minutWidth = 0.5;
        public const double hourLineWidth = 1;
        public const string dayStartTime = "00:00";
        public const string dayEndTime = "23:59";
        //public const string verificationClientScriptArg = "verifyPair:";
        //public const string confirmationClientScriptArg = "confirmPair:";
        public const string pairsSavedArg = "pairsSavedArg";
        public const string undoVerificationClientScriptArg = "undoVerify:"; // followed by employee_id, date for pairs, modified time and modified by from hist
        public const int fullWeek10hShift = 4;
        public const int ptEmptyInterval = -10000;
        public const int dayDurationStandardShift = 8;
        public const int dayDuration10hShift = 10;
        public const string extXLS = ".xls";
        public const string extXLSX = ".xlsx";

        // pass type limits - limit types
        public const string ptLimitTypeElementary = "ELEMENTARY";
        public const string ptLimitTypeComposite = "COMPOSITE";
        public const string ptLimitTypeOccassionaly = "OCCASSIONALY";

        // Session parameters
        public const string sessionConnection = "Connection"; //Object
        public const string sessionLogInUser = "LogInUser"; //ApplUserTO
        public const string sessionLogInUserLog = "LogInUserLog"; //ApplUserLogTO
        public const string sessionLogInEmployee = "LogInEmployee"; // EmployeeTO
        public const string sessionDataProvider = "DataProvider"; // string
        public const string sessionDayPairs = "DayPairs"; // List<IOPairProcessedTO>
        public const string sessionEmplCounters = "EmplCounters"; // Dictionary<int, EmployeeCounterValueTO>
        public const string sessionFilterState = "FilterState"; // Dictionary<string, string>
        public const string sessionUserCategories = "UserCategories"; // List<ApplUserCategoryTO>
        public const string sessionLoginCategory = "LoginCategory"; // ApplUserCategoryTO
        public const string sessionLoginCategoryPassTypes = "LoginCategoryPassTypes"; // List<int>
        public const string sessionLoginCategoryWUnits = "LoginCategoryWUnits"; // List<int>
        public const string sessionLoginCategoryOUnits = "LoginCategoryOUnits"; // List<int>
        public const string sessionHeader = "Header"; // string
        public const string sessionFields = "Fields"; // string
        public const string sessionFieldsFormating = "FieldsFormating"; // Dictionary<int, int>
        public const string sessionFieldsFormatedValues = "FieldsFormatedValues"; // Dictionary<int, Dictionary<string, string>>
        public const string sessionTables = "Tables"; // string
        public const string sessionFilter = "Filter"; // string
        public const string sessionSortCol = "SortCol"; // string
        public const string sessionSortDir = "SortDir"; // string
        public const string sessionKey = "Key"; // string
        public const string sessionItemsColors = "ItemsColor"; // Dictionary<string, Color>        
        public const string sessionColTypes = "ColTypes"; // string
        public const string sessionSamePage = "SamePage"; // bool
        public const string sessionSelectedKeys = "SelectedKeys"; // List<string>
        public const string sessionChangedKeys = "ChangedKeys"; // Dictionary<string, Dictionary<int, string>>
        public const string sessionReportName = "ReportName"; // string
        public const string sessionSelectedWUID = "SelectedWUID"; // int
        public const string sessionSelectedTempWUID = "SelectedTempWUID"; // int
        public const string sessionSelectedOUID = "SelectedOUID"; // int
        public const string sessionWU = "SessionWU"; // int
        public const string sessionOU = "SessionOU"; // int
        public const string sessionSelectedEmplIDs = "SessionSelectedEmplIDs"; // List<string>
        public const string sessionFromDate = "SessionFromDate"; // DateTime
        public const string sessionToDate = "SessionToDate"; // DateTime
        public const string sessionResultCurrentPage = "ResultCurrentPage"; // int
        public const string sessionDataTableList = "DataTableList"; // List<List<object>>
        public const string sessionDataTableColumns = "DataTableColumns"; // List<DataColumn>        
        public const string sessionCounters = "Counters"; // Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>
        public const string sessionInfoMessage = "InfoMessage"; // string
        public const string sessionVisibleEmplTypes = "sessionVisibleEmplTypes"; // Dictionary<int, List<int>>
        public const string sessionCountersEmployees = "sessionCountersEmployees"; // string
        public const string sessionMinChangingDate = "MinChangingDate"; // DateTime

        // ViewState parameters
        public const string vsResultCurrentPage = "ResultCurrentPage"; // int
        public const string vsResultNumPages = "ResultNumPages"; // int
        public const string vsResultRowCount = "ResultRowCount"; // int
        public const string vsResultWidth = "ResultWidth"; // double

        // defaultCategory
        public const int categoryDefault = 1;
        public const int categoryNotDefault = 0;

        // tabs for login user category, key is category id, value is list of tabs (menu items values)
        public static Dictionary<int, List<string>> CategoryTabs
        {
            get
            {
                Dictionary<int, List<string>> catTabs = new Dictionary<int, List<string>>();
                List<string> tabs = new List<string>();

                // TL tabs
                tabs.Add("TLTMData");
                tabs.Add("TLReports");
                tabs.Add("TLAnnualLeave");
                tabs.Add("TLMassiveInput");
                tabs.Add("TLClockData");
                tabs.Add("TLLoans");
                tabs.Add("TLDetails");
                tabs.Add("TLWUStatisticalReports");

                catTabs.Add((int)Categories.TL, tabs);

                // WC tabs
                tabs = new List<string>();
                tabs.Add("WCTMData");
                tabs.Add("WCAnnualLeave");
                tabs.Add("WCReports");
                tabs.Add("WCForms");
                tabs.Add("WCClockData");
                tabs.Add("WCDetails");
                tabs.Add("WCPayslips");

                catTabs.Add((int)Categories.WC, tabs);

                // WC Manager tabs
                tabs = new List<string>();
                tabs.Add("WCManagerTMData");
                tabs.Add("WCManagerVerification");
                tabs.Add("WCManagerClockData");
          //      tabs.Add("WCManagerWUStatisticalReports");                

                catTabs.Add((int)Categories.WCManager, tabs);

                // HRSSC tabs
                tabs = new List<string>();
                tabs.Add("HRSSCTMData");
                tabs.Add("HRSSCAnnualLeave");
                tabs.Add("HRSSCMassiveInput");
                tabs.Add("HRSSCCounters");
                tabs.Add("HRSSCConfirmationAbsences");
                tabs.Add("HRSSCVerification");
                tabs.Add("HRSSCClockData");
                tabs.Add("HRSSCOutstandingData");
                tabs.Add("HRSSCDetails");                

                catTabs.Add((int)Categories.HRSSC, tabs);

                // HR Legal entity tabs
                tabs = new List<string>();
                tabs.Add("HRLegalEntityTMData");
                tabs.Add("HRLegalEntityReports");
                tabs.Add("HRLegalEntityClockData");
                tabs.Add("HRLegalEntityLoans");
                tabs.Add("HRLegalEntityDetails");
                tabs.Add("HRLegalEntityWUStatisticalReports");
                tabs.Add("HRLegalEntityWorkAnalyzeReports");
                tabs.Add("HRLegalEntityBufferReport");
                tabs.Add("HRLegalEntityOutstandingData");

                catTabs.Add((int)Categories.HRLegalEntity, tabs);

                // Medical Check tabs
                tabs = new List<string>();
                tabs.Add("MCScheduling");
                tabs.Add("MCVisitsSearch");
                tabs.Add("MCEmployeeData");
                tabs.Add("MCReports");

                catTabs.Add((int)Categories.MedicalCheck, tabs);

                // Medical Check supervisors tabs
                tabs = new List<string>();
                tabs.Add("MCScheduling");
                tabs.Add("MCVisitsSearch");
                tabs.Add("MCEmployeeData");
                tabs.Add("MCReports");

                catTabs.Add((int)Categories.MedicalCheckSupervisor, tabs);

                // Medical Check LE tabs
                tabs = new List<string>();
                tabs.Add("MCScheduling");
                tabs.Add("MCVisitsSearch");
                tabs.Add("MCEmployeeData");
                tabs.Add("MCReports");

                catTabs.Add((int)Categories.MedicalCheckLE, tabs);

                // Medical Check wcdr tabs
                tabs = new List<string>();
                tabs.Add("MCScheduling");
                tabs.Add("MCVisitsSearch");
                tabs.Add("MCEmployeeData");
                tabs.Add("MCReports");

                catTabs.Add((int)Categories.MedicalCheckWCDR, tabs);

                // BCSelf tabs
                tabs = new List<string>();
                tabs.Add("BCTMData");
                tabs.Add("BCAnnualLeave");
                tabs.Add("BCReports");
                tabs.Add("BCForms");
                tabs.Add("BCClockData");
                tabs.Add("BCDetails");

                catTabs.Add((int)Categories.BCSelfService, tabs);
                
                return catTabs;
            }
        }

        public static List<string> GrundfosUnvisibleTabs
        {
            get
            {
                List<string> tabs = new List<string>();
                tabs.Add("TLLoans");
                tabs.Add("WCForms");
                tabs.Add("HRLegalEntityLoans");
                return tabs;
            }
        }

        public enum Categories
        {
            TL = 1,
            WC = 2,
            WCManager = 3,
            HRSSC = 4,
            HRLegalEntity = 5,
            MedicalCheck = 6,
            MedicalCheckSupervisor = 7,
            MedicalCheckLE = 8,
            MedicalCheckWCDR = 9,
            BCSelfService = 10
        }

        public static List<int> MCCategories
        {
            get
            {
                List<int> catList = new List<int>();
                catList.Add((int)Constants.Categories.MedicalCheck);
                catList.Add((int)Constants.Categories.MedicalCheckLE);
                catList.Add((int)Constants.Categories.MedicalCheckSupervisor);
                catList.Add((int)Constants.Categories.MedicalCheckWCDR);
                return catList;
            }
        }

        public enum EmplCounterTypes
        {
            AnnualLeaveCounter = 1,
            PrevAnnualLeaveCounter = 2,
            UsedAnnualLeaveCounter = 3,
            PaidLeaveCounter = 4,
            BankHoursCounter = 5,
            OvertimeCounter = 6,
            StopWorkingCounter = 7,
            NotJustifiedOvertime = 8
        }

        public static Dictionary<string, int> CounterTypesForRuleTypes
        {
            get
            {
                Dictionary<string, int> ItemIDs = new Dictionary<string, int>();

                ItemIDs.Add(Constants.RuleCompanyOvertimePaid, (int)Constants.EmplCounterTypes.OvertimeCounter);
                ItemIDs.Add(Constants.RuleCompanyBankHour, (int)Constants.EmplCounterTypes.BankHoursCounter);
                ItemIDs.Add(Constants.RuleCompanyBankHourUsed, (int)Constants.EmplCounterTypes.BankHoursCounter);
                ItemIDs.Add(Constants.RuleCompanyStopWorking, (int)Constants.EmplCounterTypes.StopWorkingCounter);
                ItemIDs.Add(Constants.RuleCompanyStopWorkingDone, (int)Constants.EmplCounterTypes.StopWorkingCounter);
                ItemIDs.Add(Constants.RuleCompanyAnnualLeave, (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter);
                ItemIDs.Add(Constants.RuleCompanyCollectiveAnnualLeave, (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter);
                ItemIDs.Add(Constants.RuleCompanyInitialOvertimeUsed, (int)Constants.EmplCounterTypes.NotJustifiedOvertime);
                return ItemIDs;
            }
        }

        public static Dictionary<int, string> MeasureForCounterType
        {
            get
            {
                Dictionary<int, string> ItemIDs = new Dictionary<int, string>();

                ItemIDs.Add((int)Constants.EmplCounterTypes.OvertimeCounter, Constants.emplCounterMesureMinute);
                ItemIDs.Add((int)Constants.EmplCounterTypes.BankHoursCounter, Constants.emplCounterMesureMinute);
                ItemIDs.Add((int)Constants.EmplCounterTypes.StopWorkingCounter, Constants.emplCounterMesureMinute);
                ItemIDs.Add((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter, Constants.emplCounterMesureDay);
                ItemIDs.Add((int)Constants.EmplCounterTypes.PaidLeaveCounter, Constants.emplCounterMesureDay);
                ItemIDs.Add((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter, Constants.emplCounterMesureDay);
                ItemIDs.Add((int)Constants.EmplCounterTypes.AnnualLeaveCounter, Constants.emplCounterMesureDay);
                ItemIDs.Add((int)Constants.EmplCounterTypes.NotJustifiedOvertime, Constants.emplCounterMesureMinute);
                return ItemIDs;
            }
        }

        public enum Confirmation
        {
            Confirmed = 0,
            NotConfirmed = 1,
        }

        public enum Verification
        {
            Verified = 0,
            NotVerified = 1,
        }

        public enum MassiveInput
        {
            No = 0,
            Yes = 1,
        }

        public enum FormatTypes
        {
            None = 0,
            DateFormat = 1,
            DateTimeFormat = 2,
            YearFormat = 3,
            DoubleFormat = 4,
            TimeFormat = 5,
        }

        public enum AnomalyCategories
        {
            NotVerified = 0,
            NotConfirmed = 1,
            Unjustified = 2,
            OvertimeToJusitify = 3,
            NegativeBuffers = 4,
            GroupHours = 5,
            ScheduleHours = 6,
            PairsHours = 7,
            SickLeave = 8,
            Canteen = 9,
            NightOvertimeToJusitify = 10,
            LaborLaw = 11,
            LaborLawShift = 12
        }

        //rules constants
        public const string RuleDelayMax = "DELAY RULE MAX";
        public const string RuleDelayRounding = "DELAY RULE ROUNDING";
        public const string RuleMinPresence = "PRESENCE MIN";
        public const string RulePresenceRounding = "PRESENCE/ABSENCE ROUNDING RULE";
        public const string RuleOvertimeRounding = "OVERTIME ROUNDING RULE";
        public const string RuleOvertimeMinimum = "OVERTIME MINIMUM";
        public const string RuleOvertimeShiftStart = "OVERTIME SHIFT START";
        public const string RuleOvertimeShiftEnd = "OVERTIME SHIFT END";
        public const string RuleMinOvertimeOutWS = "OVERTIME OUT OF WS MINIMUM";
        public const string RuleOvertimeRoundingOutWS = "OVERTIME ROUNDING RULE OUT OF WS";
        public const string RuleNightWork = "NIGHT WORK";
        public const string RuleBankHrsDayLimit = "BANK HOURS DAY LIMIT";
        public const string RuleBankHrsWeekLimit = "BANK HOURS WEEK LIMIT";
        public const string RuleBankHoursUsedRounding  = "BANK HOURS USED ROUNDING RULE";
        public const string RuleBankHrsCutOffDate1 = "BANK HOURS CUT OFF DATE 1";
        public const string RuleBankHrsCutOffDate2 = "BANK HOURS CUT OFF DATE 2";
        public const string RuleMealMinPresence = "MEAL RULE - MINIMAL PRESENCE";
        public const string RuleClockInOut = "CLOCK IN/OUT MINIMAL PRESENCE";
        public const string RuleAnnualLeaveCutOffDate = "ANNUAL LEAVE CUTOFF DATE";
        public const string RuleCutOffDate = "CUTOFF DATE";
        public const string RuleHRSSCCutOffDate = "HRSSC CUTOFF DATE";
        public const string RuleCompanyRegularWork = "COMPANY REGULAR WORK";
        public const string RuleCompanyAnnualLeave = "COMPANY ANNUAL LEAVE";
        public const string RuleCompanyBankHour = "COMPANY BANK HOUR";
        public const string RuleCompanyBankHourUsed = "COMPANY BANK HOUR USED";
        public const string RuleCompanyOvertimePaid = "COMPANY OVERTIME PAID";
        public const string RuleCompanyOvertimeRejected = "COMPANY OVERTIME REJECTED";
        public const string RuleCompanyStopWorking = "COMPANY STOP WORKING";
        public const string RuleCompanyStopWorkingDone = "COMPANY STOP WORKING DONE";
        public const string RuleCompanyDelay = "COMPANY DELAY";
        public const string RuleCompanySickLeaveNCF = "COMPANY SICK LEAVE NCF";
        public const string RuleCompanyInitialOvertime = "COMPANY INITIAL OVERTIME";
        public const string RuleCompanyInitialNightOvertime = "COMPANY INITIAL NIGHT OVERTIME";
        public const string RuleCompanyInitialOvertimeUsed = "COMPANY INITIAL OVERTIME USED";
        public const string RuleInitialOvertimeUsedRounding = "INITIAL OVERTIME USED ROUNDING";
        public const string RuleCompanyOfficialTrip = "COMPANY OFFICIAL TRIP";
        public const string RuleOvertimeDayLimit = "OVERTIME HOURS DAY LIMIT";
        public const string RuleOvertimeWeekLimit = "OVERTIME HOURS WEEK LIMIT";
        public const string RuleHolidayPassType = "HOLIDAY PASS TYPE";
        public const string RulePersonalHolidayPassType = "PERSONAL HOLIDAY PASS TYPE";
        public const string RuleOvertimeDayLimitWeekend = "OVERTIME HOURS DAY LIMIT WEEKEND"; //tamara 21.10.2019.
        public const string RuleCompanyOvertimePaidSatur = "COMPANY OVERTIME PAID SATURDAY"; //tamara
        public const string RuleBankHoursDayLimitWeekend = "BANK HOURS DAY LIMIT WEEKEND"; //Viktor 22.05.2024
        public const string RuleCompanyOvertimePaidSun = "COMPANY OVERTIME PAID SUNDAY"; //tamara
        public const string RuleWorkOnPersonalHolidayPassType = "WORK ON PERSONAL HOLIDAY PASS TYPE";  //mm      
        public const string RuleWorkOnHolidayPassType = "WORK ON HOLIDAY PASS TYPE";
        public const string RuleEmplLoanMaxPeriod = "EMPLOYEE LOANS MAXIMUM PERIOD";
        public const string RuleWCDRCutOffDate = "WCDR CUT OFF DATE";
        public const string RuleCompanySickLeave30Days = "COMPANY SICK LEAVE 30 DAYS";
        public const string RuleCompanyLunchBreak = "COMPANY LUNCH BREAK";
        public const string RuleCompanyOfficialOut = "COMPANY OFFICIAL OUT";
        public const string RuleCompanyPeriodicalMedicalCheckUp = "COMPANY PERIODICAL MEDICAL CHECK UP";
        public const string RuleCompanyUnionActivities = "COMPANY UNION ACTIVITIES";
        public const string RuleComanyRotaryShift = "ROTARY SHIFT";        
        public const string RuleCompanyTrening = "TRENING";
        public const string RuleCompanyManagers = "MANAGERS";
        public const string RuleCompanyBankHourMonthly = "COMPANY BANK HOUR MONTHLY";
        public const string RuleCompanyStopWorkingMonthly = "COMPANY STOP WORKING MONTHLY";
        public const string RuleCompensationForUnusedVacation = "COMPENSATION FOR UNUSED VACATION";
        public const string RulePaidUnusedBankedHours = "PAID UNUSED BANKED HOURS";
        public const string RuleDefaultShift = "DEFAULT SHIFT";
        public const string RuleOfficialTripDuration = "OFFICIAL TRIP DURATION";
        public const string RuleCompanyPaymentDay = "PAYMENT DAY";
        public const string RulePayslipOffsetVisibility = "PAYSLIP OFFSET VISIBILITY";
        public const string RuleBankHrsLimit = "BANK HOURS LIMIT";
        public const string RuleOvertimeBeforeBankHours = "USE OVERTIME BEFORE BANK HOURS";
        public const string RuleSickLeavesRefundationType = "SICK LEAVE REFUNDATION TYPE";
        public const string RuleRestaurant = "RESTAURANT";
        public const string RuleExpatOutType = "EXPATRIAT OUT TYPE";
        public const string RuleCompanyPaidLeave = "COMPANY PAID LEAVE";
        public const string RuleCompanyPaidLeave65Percent = "COMPANY PAID LEAVE 65 PERCENT";
        public const string RuleCompanyPayslipEmailNotification = "COMPANY PAYSLIP EMAIL NOTIFICATION";
        public const string RuleMCVisitsSchedulingBorderDay = "MC SCHEDULING BORDER DAY";
        public const string RuleLaborLaw = "LABOR LAW";
        public const string RuleCompanyCollectiveAnnualLeave = "COMPANY COLLECTIVE ANNUAL LEAVE";
        public const string RuleCompanyCollectiveAnnualLeaveReservation = "COMPANY COLLECTIVE ANNUAL LEAVE RESERVATION";
        public const string RuleCompanyStrike = "STRIKE";
        public const string RuleCompanyJustifiedAbsence = "JUSTIFIED ABSENCE";
        public const string RuleBankHoursBeforeVacation = "BANK HOURS BEFORE VACATION";
        public const string RuleOvertimeNJHoursBeforeVacation = "OVERTIME UNJUSTIFIED HOURS BEFORE VACATION";
        public const string RuleOvertimeMonthLimit = "OVERTIME MONTH LIMIT";
        public const string RulePrintAnnualLeaveReport = "PRINT ANNUAL LEAVE REPORT";
        public const string RulePrintPaidLeaveReport = "PRINT PAID LEAVE REPORT";
        public const string RulePrintPaidLeaveReportOffset = "PAID LEAVE REPORT OFFSET";
        public const string RulePrintAnnualLeaveReportOffset = "PRINT ANNUAL LEAVE REPORT OFFSET";
        public const string RuleShiftBegginingOffset =  "SHIFT BEGGINING OFFSET";
        public const string RuleShiftEndingOffset = "SHIFT ENDING OFFSET";
        public const string RuleInitialAbsence = "INITIAL ABSENCE";
        public const string RulePairsByLocation = "SYSTEM PAIRS BY LOCATION";
        public const string RuleCompanyNotJustifiedAbsence = "NOTJUSTIFIED ABSENCE";
        public const string RuleHideBH = "SYSTEM HIDE SUPERVISORS BANK HOURS";
        public const string RuleWholeDayAbsenceByHour = "WHOLE DAY ABSENCE BY HOUR";
        public const string RuleMaxLocationAbsence = "MAX ABSENCE BETWEEN LOCATIONS";
        public const string RuleWrittingDataToTag = "SYSTEM WRITTING DATA TO TAG";
        public const string RuleNegativeBHPayment = "NEGATIVE BANK HOURS";
        public const string RuleSWPayment = "STOP WORKING PAYMENT";
        public const string RuleCompanySickLeave30DaysContinuation = "COMPANY SICK LEAVE 30 DAYS CONTINUATION";
        public const string RuleCompanySickLeaveIndustrialInjury = "COMPANY SICK LEAVE INDUSTRIAL INJURY";
        public const string RuleCompanySickLeaveIndustrialInjuryContinuation = "COMPANY SICK LEAVE INDUSTRIAL INJURY CONTINUATION";
        public const string RuleCompanyAnnualLeaveReservation = "COMPANY ANNUAL LEAVE RESERVATION";
        public const string RuleCompanyBreastFeeding = "COMPANY BREAST FEEDING";
        public const string RuleOvertimeWSLimit = "OVERTIME HOURS WORKING DAY LIMIT";
        public const string RuleVacationBeforeBankHours = "VACATION BEFORE BANK HOURS";
        public const string RuleReaderForRestaurant = "READER FOR RESTAURANT"; //Tamara 11.2.2020.
        public const string RuleHolidayPlusNightWork = "HOLIDAY NIGHT WORK"; //Viktor 10.06.2024.

        //employee types
        public const string emplTypeBlueColor = "BC";
        public const string emplTypeWhiteColor = "WC";
        public const string emplTypeProffesional = "Professional";
        public const string emplTypeProfessionalExpert = "Professional Expert";
        public const string emplTypeManager = "Manager";

        public const string SiemensTruckReaders = "";

        public static List<string> effectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            list.Add(RuleCompanyPeriodicalMedicalCheckUp);
            list.Add(RuleCompanyStopWorkingDone);
            list.Add(RuleCompanyUnionActivities);
            list.Add(RuleCompanyTrening);
            list.Add(RuleCompanyManagers);
            list.Add(RuleCompanyBreastFeeding);
            return list;
        }

        public static List<string> effectiveWorkLunchBreakWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            list.Add(RuleCompanyPeriodicalMedicalCheckUp);
            list.Add(RuleCompanyUnionActivities);
            list.Add(RuleCompanyTrening);
            list.Add(RuleCompanyManagers);
            list.Add(RuleCompanyBreastFeeding);
            return list;
        }


        public static List<string> effectiveWorkRotaryWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            list.Add(RuleCompanyUnionActivities);
            list.Add(RuleCompanyTrening);
            list.Add(RuleCompanyBreastFeeding);
            return list;
        }

        public const int LunchBreakDuration = 30;

        //IOPair processed
        public const int ioPairProcessed = 1;
        public const int ioPairUnprocessed = 0;

        //processing io pairs
        public const int roundingPairMinutes = 15;
        public const int unjustifiedIOPairID = 0;

        //alerts
        public const int alertStatusNoAlert = 0;
        public const int alertStatus = 1;
        public const int alertStatusAgreeToChange = 2;
        public const int alertStatusLeavePair = 3;

        public const int yesInt = 1;
        public const int noInt = 0;

        public const int locationOut = -100;

        //holidays categories
        public const string nationalHoliday = "NATIONAL";
        public const string personalHoliday = "PERSONAL";

        //holidays types
        public const string holidayTypeI = "2";
        public const string holidayTypeII = "3";
        public const string holidayTypeIII = "4";
        public const string holidayTypeIV = "1";
        public const string holidayTypeDefault = "N/A";
        public const int holidayMaxCategory = 4;

        //pass type limit types
        public const string limitComposite = "COMPOSITE";
        public const string limitOccassionaly = "OCCASSIONALY";
        public const string limitElementary = "ELEMENTARY";

        //counters mesure units
        public const string emplCounterMesureDay = "DAY";
        public const string emplCounterMesureMinute = "MINUTE";

        public const string dataProcessingUser = "DP SERVICE";
        public const string massiveAdminUser = "Massive ADMIN";
        public const string syncUser = "TM sync";
        public const int resultSucc = 1;
        public const int resultFaild = 0;

        public const int fsCompanyCodeLenght = 4;
        public const int fsPlantCodeLenght = 3;
        public const int fsCostCenterCodeLenght = 4;
        public const int fsWorkShopCodeLenght = 2;
        public const int fsTeamCodeLenght = 2;
        public const int fsUnitStringoneLenght = 14;

        // info files
        public const string infoSRName = "InfoSR.txt";
        public const string infoENName = "InfoEN.txt";
        public const string infoAppSRName = "InfoAppSR.txt";
        public const string infoAppENName = "InfoAppEN.txt";

        public enum EmployeeTypesFIAT
        {
            BC = 1,
            WC = 2,
            Professional = 3,
            ProfessionalExpert = 4,
            Manager = 5,
            Expat = 6,
            TaskForce = 7,
            ExpatOut = 8,
            Agency = 9
        }

        public enum EmployeeTypesWN
        {
            DIRECT = 1,
            ADMINISTRATION = 2,
            INDIRECT = 3,
            MANAGEMENT = 4,
            TRAINING = 5            
        }

        public enum EmployeeTypesPMC
        {
            MOD_PMC = 1,
            MOD_AGENCY = 2,
            MOI_PMC = 3,
            MOI_AGENCY = 4,
            PROFESSIONALS_PMC = 5,
            PROFESSIONALS_AGENCY = 6,
            INTERNATIONAL_STAFF = 7,
            VILLANOVA_LC = 8,
            VILLANOVA_INTERIM = 9
        }

        public enum UserCategoriesFIAT
        {
            Supervisor = 1,
            WCSelfService = 2,
            WCDirectRisponsible = 3
        }

        public const string langSRB_FIAT = "SRB";
        public const string langENG_FIAT = "ENG";

        public static Dictionary<int, int> ExpatWorkingUnits
        {
            get
            {
                Dictionary<int, int> WUIDs = new Dictionary<int, int>();

                WUIDs.Add(-2, -6);
                WUIDs.Add(-3, -7);
                WUIDs.Add(-4, -8);
                WUIDs.Add(-5, -9);
                return WUIDs;
            }
        }

        public static Dictionary<int, int> TaskWorkingUnits
        {
            get
            {
                Dictionary<int, int> WUIDs = new Dictionary<int, int>();

                WUIDs.Add(-2, 100000);
                WUIDs.Add(-3, 100001);
                WUIDs.Add(-4, 100002);
                WUIDs.Add(-5, 100003);
                return WUIDs;
            }
        }

        public static Dictionary<int, int> AgencyWorkingUnits
        {
            get
            {
                Dictionary<int, int> WUIDs = new Dictionary<int, int>();

                WUIDs.Add(-2, 100004);
                WUIDs.Add(-3, 100005);
                WUIDs.Add(-4, 100006);
                WUIDs.Add(-5, 100007);
                return WUIDs;
            }
        }

        public const string syncResFSType = "F";
        public const string syncResOSType = "O";

        public const string emplResTypeWU = "WU";
        public const string emplResTypeOU = "OU";
        
        public static Dictionary<string, string> SyncRespTypes
        {
            get
            {
                Dictionary<string, string> WUIDs = new Dictionary<string, string>();

                WUIDs.Add(syncResFSType, emplResTypeWU);
                WUIDs.Add(syncResOSType, emplResTypeOU);
                return WUIDs;
            }
        }

        public const string syncStringNullValue = "DELETED";
        public const int syncIntNullValue = -100;
        public static DateTime syncDateTimeNullValue()
        {
            return new DateTime(1900, 1, 1);
        }

        public static DateTime dateTimeNullValue()
        {
            return new DateTime(1900, 1, 1);
        }

        public const string PYTypeEstimated = "E";
        public const string PYTypeReal = "R";

        public const string payRollUser = "PY sync";

        public const string notApplicable = "N/A";

        private static string sharedAreaFiat = @"\\SRSKRA01APCP030\";

        public static string SharedAreaFiat
        {
            get { return Constants.sharedAreaFiat; }
            set { Constants.sharedAreaFiat = value; }
        }

        private static string _FiatPayslipsPath = @"C:\TM Payslips\";

        public static string FiatPayslipsPath
        {
            get { return Constants._FiatPayslipsPath; }
            set { Constants._FiatPayslipsPath = value; }
        }

        //work analysis
        //FAS      
        private static string _fiatSharedAreaFASReport400 = @"C:\TM WorkAnalysis\FAS\TM FAS\Work Analisys\Report_400\";
        private static string _fiatSharedAreaFASReport500 = @"C:\TM WorkAnalysis\FAS\TM FAS\Work Analisys\Report_500\";
        private static string _fiatSharedAreaFASReportWageType = @"C:\TM WorkAnalysis\FAS\TM FAS\Work Analisys\Wage Type\";
        private static string _fiatSharedAreaFASReportPA = @"C:\TM WorkAnalysis\FAS\TM FAS\Work Analisys\Presence and Absence\";
        private static string _fiatSharedAreaFASReportShifts = @"C:\TM WorkAnalysis\FAS\TM FAS\Work Analisys\Shifts\";
        private static string _fiatSharedAreaFASReportAnomalies = @"C:\TM WorkAnalysis\FAS\TM FAS\Anomalies\";    
        private static string _fiatSharedWAReportFAS = @"\\Srskra01apcp030\fas\TM FAS\PY export\";
        private static string _fiatSharedWAAnomaliesReportFAS = @"\\Srskra01apcp030\FAS\TM FAS\Anomalies\";

        //MMA
        private static string _fiatSharedAreaMMAReport400 = @"C:\TM WorkAnalysis\mmauto\TM MMAUTO\Work Analisys\Report_400\";
        private static string _fiatSharedAreaMMAReport500 = @"C:\TM WorkAnalysis\mmauto\TM MMAUTO\Work Analisys\Report_500\";
        private static string _fiatSharedAreaMMAReportWageType = @"C:\TM WorkAnalysis\mmauto\TM MMAUTO\Work Analisys\Wage Type\";
        private static string _fiatSharedAreaMMAReportPA = @"C:\TM WorkAnalysis\mmauto\TM MMAUTO\Work Analisys\Presence and Absence\";
        private static string _fiatSharedAreaMMAReportDecisions = @"C:\TM WorkAnalysis\mmauto\TM MMAUTO\BANK HOURS LETTER\";
        private static string _fiatSharedAreaMMAReportShifts = @"C:\TM WorkAnalysis\mmauto\TM MMAUTO\Work Analisys\Shifts\";
        private static string _fiatSharedAreaMMAReportAnomalies = @"C:\TM WorkAnalysis\mmauto\TM MMAUTO\Anomalies\";
        private static string _fiatSharedWAReportMMA = @"\\Srskra01apcp030\mmauto\TM MMAUTO\PY export\";
        private static string _fiatSharedWAAnomaliesReportMMA = @"\\Srskra01apcp030\MMAUTO\TM MMAUTO\Anomalies\";

        //mm
        private static string _fiatSharedAreaMMReport400 = @"C:\TM WorkAnalysis\mmdoo\TM MM DOO\Work Analisys\Report_400\";
        private static string _fiatSharedAreaMMReport500 = @"C:\TM WorkAnalysis\mmdoo\TM MM DOO\Work Analisys\Report_500\";
        private static string _fiatSharedAreaMMReportWageType = @"C:\TM WorkAnalysis\mmdoo\TM MM DOO\Work Analisys\Wage Type\";
        private static string _fiatSharedAreaMMReportPA = @"C:\TM WorkAnalysis\mmdoo\TM MM DOO\Work Analisys\Presence and Absence\";
        private static string _fiatSharedAreaMMReportDecisions = @"C:\TM WorkAnalysis\mmdoo\TM MM DOO\BANK HOURS LETTER\";
        private static string _fiatSharedAreaMMReportShifts = @"C:\TM WorkAnalysis\mmdoo\TM MM DOO\Work Analisys\Shifts\";
        private static string _fiatSharedAreaMMReportAnomalies = @"C:\TM WorkAnalysis\mmdoo\TM MM DOO\Anomalies\";
        private static string _fiatSharedWAReportMM = @"\\Srskra01apcp030\mmdoo\TM MM DOO\PY export\";
        private static string _fiatSharedWAAnomaliesReportMM = @"\\Srskra01apcp030\MMDOO\TM MM DOO\Anomalies\";

        //fs
        private static string _fiatSharedAreaFSReport400 = @"C:\TM WorkAnalysis\fs\TM FS\Work Analisys\Report_400\";
        private static string _fiatSharedAreaFSReport500 = @"C:\TM WorkAnalysis\fs\TM FS\Work Analisys\Report_500\";
        private static string _fiatSharedAreaFSReportWageType = @"C:\TM WorkAnalysis\fs\TM FS\Work Analisys\Wage Type\";
        private static string _fiatSharedAreaFSReportPA = @"C:\TM WorkAnalysis\fs\TM FS\Work Analisys\Presence and Absence\";
        private static string _fiatSharedAreaFSReportShifts = @"C:\TM WorkAnalysis\fs\TM FS\Work Analisys\Shifts\";
        private static string _fiatSharedAreaFSReportAnomalies = @"C:\TM WorkAnalysis\fs\TM FS\Anomalies\";
        private static string _fiatSharedWAReportFS = @"\\Srskra01apcp030\fs\TM FS\PY export\";
        private static string _fiatSharedWAAnomaliesReportFS = @"\\Srskra01apcp030\FS\TM FS\Anomalies\";

        // ISSE file paths
        private static string _fiatSharedAreaFASISSE = @"C:\TM ISSE\FS\FAS - Interface for ISSE\";

        public static string FiatSharedAreaFASISSE
        {
            get { return Constants._fiatSharedAreaFASISSE; }
        }

        // PASS ALLOWANCE file paths
        private static string _fiatSharedAreaPassAllowance = @"C:\TMPassAllowance\";

        public static string FiatSharedAreaPassAllowance
        {
            get { return Constants._fiatSharedAreaPassAllowance; }
        }

        // absence        
        private static string _fiatSharedAbsencePath = @"\\Srskra01apcp030\HRSSC\System HRSSC\Flow FAS - Absenteeism\";        

        public static string FiatSharedAbsencePath
        {
            get { return Constants._fiatSharedAbsencePath; }            
        }

        public static Dictionary<string, string> FiatSharedAreaReport400
        {
            get
            {
                Dictionary<string, string> dictionaryCompaniesFilePath = new Dictionary<string, string>();

                dictionaryCompaniesFilePath.Add("FAS", _fiatSharedAreaFASReport400);
                dictionaryCompaniesFilePath.Add("MMdoo", _fiatSharedAreaMMReport400);
                dictionaryCompaniesFilePath.Add("MMauto", _fiatSharedAreaMMAReport400);
                dictionaryCompaniesFilePath.Add("FS", _fiatSharedAreaFSReport400);
                return dictionaryCompaniesFilePath;
            }
        }

        public static Dictionary<string, string> FiatSharedAreaReport500
        {
            get
            {
                Dictionary<string, string> dictionaryCompaniesFilePath = new Dictionary<string, string>();

                dictionaryCompaniesFilePath.Add("FAS", _fiatSharedAreaFASReport500);
                dictionaryCompaniesFilePath.Add("MMdoo", _fiatSharedAreaMMReport500);
                dictionaryCompaniesFilePath.Add("MMauto", _fiatSharedAreaMMAReport500);
                dictionaryCompaniesFilePath.Add("FS", _fiatSharedAreaFSReport500);
                return dictionaryCompaniesFilePath;
            }
        }

        public static Dictionary<string, string> FiatSharedAreaReportWageType
        {
            get
            {
                Dictionary<string, string> dictionaryCompaniesFilePath = new Dictionary<string, string>();

                dictionaryCompaniesFilePath.Add("FAS", _fiatSharedAreaFASReportWageType);
                dictionaryCompaniesFilePath.Add("MMdoo", _fiatSharedAreaMMReportWageType);
                dictionaryCompaniesFilePath.Add("MMauto", _fiatSharedAreaMMAReportWageType);
                dictionaryCompaniesFilePath.Add("FS", _fiatSharedAreaFSReportWageType);
                return dictionaryCompaniesFilePath;
            }
        }

        public static Dictionary<string, string> FiatSharedAreaReportPA
        {
            get
            {
                Dictionary<string, string> dictionaryCompaniesFilePath = new Dictionary<string, string>();

                dictionaryCompaniesFilePath.Add("FAS", _fiatSharedAreaFASReportPA);
                dictionaryCompaniesFilePath.Add("MMdoo", _fiatSharedAreaMMReportPA);
                dictionaryCompaniesFilePath.Add("MMauto", _fiatSharedAreaMMAReportPA);
                dictionaryCompaniesFilePath.Add("FS", _fiatSharedAreaFSReportPA);
                return dictionaryCompaniesFilePath;
            }
        }

        public static Dictionary<string, string> FiatSharedAreaReportDecisions
        {
            get
            {
                Dictionary<string, string> dictionaryCompaniesFilePath = new Dictionary<string, string>();

                dictionaryCompaniesFilePath.Add("MMdoo", _fiatSharedAreaMMReportDecisions);
                dictionaryCompaniesFilePath.Add("MMauto", _fiatSharedAreaMMAReportDecisions);
                return dictionaryCompaniesFilePath;
            }
        }
        public static Dictionary<string, string> FiatSharedAreaReportShifts
        {
            get
            {
                Dictionary<string, string> dictionaryCompaniesFilePath = new Dictionary<string, string>();

                dictionaryCompaniesFilePath.Add("FAS", _fiatSharedAreaFASReportShifts);
                dictionaryCompaniesFilePath.Add("MMdoo", _fiatSharedAreaMMReportShifts);
                dictionaryCompaniesFilePath.Add("MMauto", _fiatSharedAreaMMAReportShifts);
                dictionaryCompaniesFilePath.Add("FS", _fiatSharedAreaFSReportShifts);
                return dictionaryCompaniesFilePath;
            }
        }
        public static Dictionary<string, string> FiatSharedAreaReportAnomalies
        {
            get
            {
                Dictionary<string, string> dictionaryCompaniesFilePath = new Dictionary<string, string>();

                dictionaryCompaniesFilePath.Add("FAS", _fiatSharedAreaFASReportAnomalies);
                dictionaryCompaniesFilePath.Add("MMdoo", _fiatSharedAreaMMReportAnomalies);
                dictionaryCompaniesFilePath.Add("MMauto", _fiatSharedAreaMMAReportAnomalies);
                dictionaryCompaniesFilePath.Add("FS", _fiatSharedAreaFSReportAnomalies);
                return dictionaryCompaniesFilePath;
            }
        }

        public static Dictionary<int, string> FiatSharedAreaWAReports
        {
            get
            {
                Dictionary<int, string> dictionaryCompaniesFilePath = new Dictionary<int, string>();

                dictionaryCompaniesFilePath.Add(-2, _fiatSharedWAReportFAS);
                dictionaryCompaniesFilePath.Add(-3, _fiatSharedWAReportMM);
                dictionaryCompaniesFilePath.Add(-4, _fiatSharedWAReportMMA);
                dictionaryCompaniesFilePath.Add(-5, _fiatSharedWAReportFS);
                return dictionaryCompaniesFilePath;
            }
        }

        public static Dictionary<int, string> FiatSharedAreaWAAnomaliesReports
        {
            get
            {
                Dictionary<int, string> dictionaryCompaniesFilePath = new Dictionary<int, string>();

                dictionaryCompaniesFilePath.Add(-2, _fiatSharedWAAnomaliesReportFAS);
                dictionaryCompaniesFilePath.Add(-3, _fiatSharedWAAnomaliesReportMM);
                dictionaryCompaniesFilePath.Add(-4, _fiatSharedWAAnomaliesReportMMA);
                dictionaryCompaniesFilePath.Add(-5, _fiatSharedWAAnomaliesReportFS);
                return dictionaryCompaniesFilePath;
            }
        }

        public enum LoginType
        {
            LOG_ON = 0,
            LOG_OFF = 1
        }

        public enum LoginStatus
        {
            FAILED_INCORRECT_USER = 0,
            FAILED_INCORRECT_PASSWORD = 1,
            FAILED_INACTIVE_USER = 2,
            ADAM_FAILED = 3,
            SUCCESSFULL_TM = 4,
            SUCCESSFULL_ADAM = 5,            
            SUCCESSFULL = 6,
            FAILED_UNKNOWN_TAG = 7,
            FAILED_INACTIVE_TAG = 8
        }

        //Reader remote control const.
        public const int MESSAGE_TAG_DETECTED = 1;
        public const int MESSAGE_PASS_STATUS = 2;
        public const int MESSAGE_BUTTON_PRESSED = 3;

        public const int app_response_show_buttons = 2;
        public const int app_response_pass_permitted = 1;
        public const int app_response_pass_unpermitted = 0;


        public const int buttonsScreen = 1;
        public const int buttonRegular = 4;
        public const int buttonCancel = 5;

        public const int passIsDetected = 1;
        public const int passNotDetected = 0;

        public const int basicDebugLevel = 0;
        public const int extendedDebugLevel = 1;
        public const int detailedDebugLevel = 2;

        public const string technologyType = "MIFARE";

        public const int dbRefreshTime = 10000;
        public const int SundayDay = 7;

        public enum MealDenied
        {
            Approved = 0,
            CompanyRestaurant = 1,
            TagNotActive = 2,
            EmployeeNotActive = 3,
            NotWorkingSchedule = 4,
            WholeDayAbsence = 5,
            MealUsed = 6,
            Location = 7,
            TagUnknown = 8
        }

        //reasons for denied meal
        public const string RestaurantCompany = "KOMPANIJA NEAK.";
        public const string RestaurantLocation = "LOKACIJA POGRESNA";
        public const string RestaurantEmployeeNotActive = "ZAPOSLENI NEAKT.";
        public const string RestaurantTagNotActive = "KARTICA NEAK.";
        public const string RestaurantTagUnknown = "KARTICA NEPOZ.";
        public const string RestaurantUsed = "ISKORISCEN OBROK";
        public const string RestaurantApproved = "ODOBREN";

        //MEALS APPROVAL
        public const string MealApproved = "APPROVED";
        public const string MealNotApproved = "NOT APPROVED";
        public const string MealDeleted = "DELETED";
        public const string MealBusinessTrip = "BUSINESS TRIP";
        public enum MealOnlineValidation
        {
            NotValid = 0,
            Valid = 1
        }

        public enum MealAutoCheck
        {
            checkNotValid = 0,
            checkValid = 1
        }
        public const string MealReportTypeCumulative = "CUMULATIVE";
        public const string MealReportTypeDaily = "DAILY";

        public const int TCPErrorNumber = 64;
        public const int ConnectionErrorNumber = -1;
        public const int TimeoutNumber = -2;
        public const string ConnectionErrorString = "conn";

        public const string noConnectionSerb = "Nema konekcije sa bazom podataka.";
        public const string noConnectionEng = "No DB connection.";

        public enum FiatMealsTypes
        {
            KG = 1,
            BG = 2
        }

        public enum AutoCheckFailureReason
        {
            DOUBLE_REGISTRATION = 0,
            NO_MINIMAL_PRESENCE = 1,
            NOT_WORKING_DAY = 2,
            WHOLE_DAY_ABSENCE = 3,
            UNJUSTIFED_DAY = 4,
            NO_EFFECTIVE_WORK = 5
        }

        public static DateTime SyncStartTime
        {
            get
            {
                DateTime syncStartTime = new DateTime();
                if (ConfigurationManager.AppSettings["syncStartTime"] == null || ConfigurationManager.AppSettings["syncStartTime"].Equals("")
                    || (!DateTime.TryParseExact(ConfigurationManager.AppSettings["syncStartTime"].ToString().Trim(), Constants.timeFormat, null, DateTimeStyles.None, out syncStartTime)))
                        syncStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 2, 0, 0);
                                
                return syncStartTime;
            }
        }

        public static int SyncTimeout
        {
            get
            {
                int syncTimeout = -1;
                if (ConfigurationManager.AppSettings["syncTimeout"] == null || ConfigurationManager.AppSettings["syncTimeout"].Equals("")
                    || (!int.TryParse(ConfigurationManager.AppSettings["syncTimeout"].ToString().Trim(), out syncTimeout)))
                    syncTimeout = 1440;

                return syncTimeout;
            }
        }

        //Boris, za obradu podataka sa gate-ova, 20170112

        public static List<String> GetInfoFromGates
        {
            get
            {

                List<String> infoListFromGates = new List<String>();
                if (ConfigurationManager.AppSettings["Gates"] != null || !ConfigurationManager.AppSettings["Gates"].Equals(""))
                {
                    string[] gates = ConfigurationManager.AppSettings["Gates"].Trim().Split(',');

                    foreach (string gatesForProcess in gates)
                    {

                        infoListFromGates.Add(gatesForProcess);
                    }
                }



                return infoListFromGates;
            }
        }


        public static DateTime MedicalCheckStartTime
        {
            get
            {
                DateTime mcStartTime = new DateTime();
                if (ConfigurationManager.AppSettings["medicalCheckStartTime"] == null || ConfigurationManager.AppSettings["medicalCheckStartTime"].Equals("")
                    || (!DateTime.TryParseExact(ConfigurationManager.AppSettings["medicalCheckStartTime"].ToString().Trim(), Constants.timeFormat, null, DateTimeStyles.None, out mcStartTime)))
                    mcStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 5, 0, 0);

                return mcStartTime;
            }
        }

        public static int MedicalCheckTimeout
        {
            get
            {
                int mcTimeout = -1;
                if (ConfigurationManager.AppSettings["medicalCheckTimeout"] == null || ConfigurationManager.AppSettings["medicalCheckTimeout"].Equals("")
                    || (!int.TryParse(ConfigurationManager.AppSettings["medicalCheckTimeout"].ToString().Trim(), out mcTimeout)))
                    mcTimeout = 1440;

                return mcTimeout;
            }
        }

        public static DateTime MonthlyCheckStartTime
        {
            get
            {
                DateTime monthlyCheckStartTime = new DateTime();
                if (ConfigurationManager.AppSettings["monthlyCheckStartTime"] == null || ConfigurationManager.AppSettings["monthlyCheckStartTime"].Equals("")
                    || (!DateTime.TryParseExact(ConfigurationManager.AppSettings["monthlyCheckStartTime"].ToString().Trim(), Constants.timeFormat, null, DateTimeStyles.None, out monthlyCheckStartTime)))
                    monthlyCheckStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 2, 0, 0);

                return monthlyCheckStartTime;
            }
        }

        public static int MonthlyCheckTimeout
        {
            get
            {
                int monthlyCheckTimeout = -1;
                if (ConfigurationManager.AppSettings["monthlyCheckTimeout"] == null || ConfigurationManager.AppSettings["monthlyCheckTimeout"].Equals("")
                    || (!int.TryParse(ConfigurationManager.AppSettings["monthlyCheckTimeout"].ToString().Trim(), out monthlyCheckTimeout)))
                    monthlyCheckTimeout = 1440;

                return monthlyCheckTimeout;
            }
        }

        public static bool DoRecalculation 
        {
            get
            {                
                return ConfigurationManager.AppSettings["recalculateAnnualLeave"] != null && ConfigurationManager.AppSettings["recalculateAnnualLeave"].Trim().ToUpper().Equals(Constants.yes.Trim().ToUpper());
            }
        }

        public static string SiPassServer
        {
            get
            {
                string server = "";
                if (ConfigurationManager.AppSettings["SiPassServer"] != null)
                    server = ConfigurationManager.AppSettings["SiPassServer"].Trim();
                return server;
            }
        }

        public static string SiPassUsername
        {
            get
            {
                string user = "";
                if (ConfigurationManager.AppSettings["SiPassUsername"] != null)
                    user = ConfigurationManager.AppSettings["SiPassUsername"].Trim();
                return user;
            }
        }

        public static string SiPassPassword
        {
            get
            {
                string password = "";
                if (ConfigurationManager.AppSettings["SiPassPassword"] != null)
                    password = ConfigurationManager.AppSettings["SiPassPassword"].Trim();
                return password;
            }
        }

        public const int expatOutOU = 999999;

        public enum UserLoginStatus
        {
            FAILED = 0,
            SUCCESSFUL = 1            
        }

        public enum UserLoginChanel
        {
            WEB = 0,
            DESKTOP = 1
        }

        public enum UserLoginType
        {
            TM = 0,
            FIAT = 1
        }

        public static int defaultSYSOU = 1000000;

        public enum MassiveInputActions
        {
            CHANGE_REGULAR = 0,
            CHANGE_OVERTIME = 1,
            CHANGE_UNCONFIRMED = 2,
            ENTERING_REGULAR = 3,
            ENTERING_OVERTIME = 4
        }

        public enum ChangeRegularTypes
        {
            ABSENCE = 0,
            ANNUAL_LEAVE = 1,
            COLLECTIVE_ANNUAL_LEAVE = 2,
            BANK_HOURS_USED = 3,
            STOP_WORKING = 4,
            TRAINING = 5,
            REGULAR_WORK = 6,
            OVERTIME_NOTJUSTIFIED_USED = 7,
            STRIKE = 8,
            JUSTIFIED_ABSENCE = 9,
            PAID_LEAVE = 10,
            PAID_LEAVE_65 = 11,
            MEDICAL_CHECK = 12,
            DELAY = 13
        }

        public enum ChangeRegularTypesAdditional
        {
            BLOOD_DONATION = 0,
            DEATH_CLOSE_FAMILY = 1,
            DEATH_MARITIAL_FAMILY = 2,
            DEATH_GRANDPARENTS = 3,
            CHILD_BEARING_FAMILY = 4,
            CHILD_BEARING = 5,
            WEDDING = 6,
            SUSPENSION_3 = 7,
            DRILL_SUMMONS = 8,
            SICK_LEAVE_30_START = 9,
            INDUSTRIAL_INJURY = 10,
            SICK_LEAVE_30_EXC = 11,
            MATERNITY_LEAVE = 12,
            JUSTIFIED_ABSENCE = 13,
            UNJUSTIFIED_ABSENCE = 14,
            MOVING_EMPLOYEE = 15,
            REPAIR_OF_DEMAGE = 16,
            EXAMINATION = 17,
            DISEASE_FAMILY = 18,
            SUSPENSION_4 = 19,
            INDUSTRIAL_INJURY_CONT = 20,
            SICK_LEAVE_30_CONT = 21,
            CARE_OF_CHILD = 22,
            UNPAID_LEAVE = 23,
            PREGNANCY_LEAVE = 24,
            TISSUE_DONATION = 25,
            PERSONAL_LEAVE = 26,
            DEATH_FAMILY = 27,
            ILLNESS_FAMILY = 28,
            WEDDING_CHILDREN = 29,
            RECREATIONAL_HOLIDAY = 30,
            DISCIPLINARY_SUSPENSION = 31,
            SICK_LEAVE_NO_PAYMENT = 32,
            CARE_OF_CHILD_3 = 33,
            PREGNENCY_LEAVE_30 = 34,
            STRIKE = 35,
            SICK_LEAVE = 36,
            NCF_CHILD_BEARING_FAMILY = 37,
            NCF_CHILD_BEARING = 38,
            NCF_DEATH_CLOSE_FAMILY = 39,
            NCF_DEATH_MARITIAL_FAMILY = 40,
            NCF_DRILL_SUMMONS = 41,
            NCF_ILLNESS_FAMILY = 42,
            NCF_UNPAID_LEAVE = 43,
            NCF_MOVING_EMPLOYEE = 44,
            NCF_RECREATIONAL_HOLIDAY = 45,
            NCF_REPAIR_OF_DEMAGE = 46,
            NCF_DISEASE_FAMILY = 47,
            NCF_EXAMINATION = 48,
            NCF_WEDDING_CHILDREN = 49,
            NCF_WEDDING = 50,
            NCF_BLOOD_DONATION = 51,
            NCF_DEATH_GRANDPARENTS = 52,
            ABSENCE = 53
        }

        public enum ChangeOvertimeTypes
        {
            OVER_TIME_PAID = 0,
            BANK_HOURS = 1,
            STOP_WORKING_DONE = 2,
            OVER_TIME_REJECTED = 3,            
            OVER_TIME_TO_JUSTIFY = 4,
            OVER_TIME_TO_JUSTIFY_NIGHT = 5
        }

        public static Dictionary<int, string> ChangeRegularTypesAdditionalDict()
        {
            Dictionary<int, string> dict = new Dictionary<int,string>();            
            dict.Add((int)Constants.ChangeRegularTypesAdditional.BLOOD_DONATION, Constants.ChangeRegularTypesAdditional.BLOOD_DONATION.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.DEATH_CLOSE_FAMILY, Constants.ChangeRegularTypesAdditional.DEATH_CLOSE_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.DEATH_MARITIAL_FAMILY, Constants.ChangeRegularTypesAdditional.DEATH_MARITIAL_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.CHILD_BEARING_FAMILY, Constants.ChangeRegularTypesAdditional.CHILD_BEARING_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.CHILD_BEARING, Constants.ChangeRegularTypesAdditional.CHILD_BEARING.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.WEDDING, Constants.ChangeRegularTypesAdditional.WEDDING.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.SUSPENSION_3, Constants.ChangeRegularTypesAdditional.SUSPENSION_3.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.DRILL_SUMMONS, Constants.ChangeRegularTypesAdditional.DRILL_SUMMONS.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_START, Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_START.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.INDUSTRIAL_INJURY, Constants.ChangeRegularTypesAdditional.INDUSTRIAL_INJURY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_EXC, Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_EXC.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.MATERNITY_LEAVE, Constants.ChangeRegularTypesAdditional.MATERNITY_LEAVE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.JUSTIFIED_ABSENCE, Constants.ChangeRegularTypesAdditional.JUSTIFIED_ABSENCE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.UNJUSTIFIED_ABSENCE, Constants.ChangeRegularTypesAdditional.UNJUSTIFIED_ABSENCE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.REPAIR_OF_DEMAGE, Constants.ChangeRegularTypesAdditional.REPAIR_OF_DEMAGE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.EXAMINATION, Constants.ChangeRegularTypesAdditional.EXAMINATION.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.DISEASE_FAMILY, Constants.ChangeRegularTypesAdditional.DISEASE_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.SUSPENSION_4, Constants.ChangeRegularTypesAdditional.SUSPENSION_4.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.INDUSTRIAL_INJURY_CONT, Constants.ChangeRegularTypesAdditional.INDUSTRIAL_INJURY_CONT.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_CONT, Constants.ChangeRegularTypesAdditional.SICK_LEAVE_30_CONT.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.CARE_OF_CHILD, Constants.ChangeRegularTypesAdditional.CARE_OF_CHILD.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.UNPAID_LEAVE, Constants.ChangeRegularTypesAdditional.UNPAID_LEAVE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.PREGNANCY_LEAVE, Constants.ChangeRegularTypesAdditional.PREGNANCY_LEAVE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.TISSUE_DONATION, Constants.ChangeRegularTypesAdditional.TISSUE_DONATION.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.PERSONAL_LEAVE, Constants.ChangeRegularTypesAdditional.PERSONAL_LEAVE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.DEATH_GRANDPARENTS, Constants.ChangeRegularTypesAdditional.DEATH_GRANDPARENTS.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.MOVING_EMPLOYEE, Constants.ChangeRegularTypesAdditional.MOVING_EMPLOYEE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.DEATH_FAMILY, Constants.ChangeRegularTypesAdditional.DEATH_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.ILLNESS_FAMILY, Constants.ChangeRegularTypesAdditional.ILLNESS_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.WEDDING_CHILDREN, Constants.ChangeRegularTypesAdditional.WEDDING_CHILDREN.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.RECREATIONAL_HOLIDAY, Constants.ChangeRegularTypesAdditional.RECREATIONAL_HOLIDAY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.DISCIPLINARY_SUSPENSION, Constants.ChangeRegularTypesAdditional.DISCIPLINARY_SUSPENSION.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE_NO_PAYMENT, Constants.ChangeRegularTypesAdditional.SICK_LEAVE_NO_PAYMENT.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.CARE_OF_CHILD_3, Constants.ChangeRegularTypesAdditional.CARE_OF_CHILD_3.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.PREGNENCY_LEAVE_30, Constants.ChangeRegularTypesAdditional.PREGNENCY_LEAVE_30.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.STRIKE, Constants.ChangeRegularTypesAdditional.STRIKE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.SICK_LEAVE, Constants.ChangeRegularTypesAdditional.SICK_LEAVE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_CHILD_BEARING_FAMILY, Constants.ChangeRegularTypesAdditional.NCF_CHILD_BEARING_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_CHILD_BEARING, Constants.ChangeRegularTypesAdditional.NCF_CHILD_BEARING.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_DEATH_CLOSE_FAMILY, Constants.ChangeRegularTypesAdditional.NCF_DEATH_CLOSE_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_DEATH_MARITIAL_FAMILY, Constants.ChangeRegularTypesAdditional.NCF_DEATH_MARITIAL_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_DRILL_SUMMONS, Constants.ChangeRegularTypesAdditional.NCF_DRILL_SUMMONS.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_ILLNESS_FAMILY, Constants.ChangeRegularTypesAdditional.NCF_ILLNESS_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_UNPAID_LEAVE, Constants.ChangeRegularTypesAdditional.NCF_UNPAID_LEAVE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_MOVING_EMPLOYEE, Constants.ChangeRegularTypesAdditional.NCF_MOVING_EMPLOYEE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_RECREATIONAL_HOLIDAY, Constants.ChangeRegularTypesAdditional.NCF_RECREATIONAL_HOLIDAY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_REPAIR_OF_DEMAGE, Constants.ChangeRegularTypesAdditional.NCF_REPAIR_OF_DEMAGE.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_DISEASE_FAMILY, Constants.ChangeRegularTypesAdditional.NCF_DISEASE_FAMILY.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_EXAMINATION, Constants.ChangeRegularTypesAdditional.NCF_EXAMINATION.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_WEDDING_CHILDREN, Constants.ChangeRegularTypesAdditional.NCF_WEDDING_CHILDREN.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_WEDDING, Constants.ChangeRegularTypesAdditional.NCF_WEDDING.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_BLOOD_DONATION, Constants.ChangeRegularTypesAdditional.NCF_BLOOD_DONATION.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.NCF_DEATH_GRANDPARENTS, Constants.ChangeRegularTypesAdditional.NCF_DEATH_GRANDPARENTS.ToString());
            dict.Add((int)Constants.ChangeRegularTypesAdditional.ABSENCE, Constants.ChangeRegularTypesAdditional.ABSENCE.ToString());

            return dict;
        }

        public const string notConfirmedPrefix = "NCF_";

        public enum AdditionalTypes
        {
            NONE = 0,
            CLOSURE = 1,
            LAYOFF = 2,
            STOPPAGE = 3,
            PUBLIC_HOLIDAY = 4
        }

        public static Dictionary<string, string> MassiveInputTypeCodes()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ABSENCE", "(0000) ");
            dict.Add("ANNUAL_LEAVE", "(0040) ");
            dict.Add("COLLECTIVE_ANNUAL_LEAVE", "(0040) ");
            dict.Add("BANK_HOURS_USED", "(0212) ");
            dict.Add("STOP_WORKING", "(0512) ");
            dict.Add("TRAINING", "(1406) ");
            dict.Add("REGULAR_WORK", "(0012) ");
            dict.Add("OVERTIME_NOTJUSTIFIED_USED", "(2212) ");
            dict.Add("STRIKE", "(1407) ");
            dict.Add("JUSTIFIED_ABSENCE", "(0069) ");
            dict.Add("PAID_LEAVE", "(0053) ");
            dict.Add("PAID_LEAVE_65", "(0153) ");
            dict.Add("MEDICAL_CHECK", "(0075) ");
            dict.Add("DELAY", "(0171) ");
            dict.Add("OVER_TIME_PAID", "(0030) ");
            dict.Add("BANK_HOURS", "(0312) ");
            dict.Add("STOP_WORKING_DONE", "(0612) ");
            dict.Add("OVER_TIME_REJECTED", "(0130) ");
            dict.Add("OVER_TIME_TO_JUSTIFY", "(9999) ");
            dict.Add("OVER_TIME_TO_JUSTIFY_NIGHT", "(8888) ");

            return dict;
        }

        public static Dictionary<string, string> MassiveInputTypeAdditionalCodes()
        {            
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("BLOOD_DONATION", "(0043) ");
            dict.Add("DEATH_CLOSE_FAMILY", "(0045) ");
            dict.Add("DEATH_MARITIAL_FAMILY", "(0046) ");            
            dict.Add("CHILD_BEARING_FAMILY", "(0047) ");
            dict.Add("CHILD_BEARING", "(0048) ");
            dict.Add("WEDDING", "(0049) ");
            dict.Add("SUSPENSION_3", "(0055) ");
            dict.Add("DRILL_SUMMONS", "(0056) ");
            dict.Add("SICK_LEAVE_30_START", "(0057) ");
            dict.Add("INDUSTRIAL_INJURY", "(0058) ");
            dict.Add("SICK_LEAVE_30_EXC", "(0060) ");
            dict.Add("MATERNITY_LEAVE", "(0061) ");
            dict.Add("JUSTIFIED_ABSENCE", "(0069) ");
            dict.Add("UNJUSTIFIED_ABSENCE", "(0070) ");            
            dict.Add("REPAIR_OF_DEMAGE", "(0146) ");
            dict.Add("EXAMINATION", "(0147) ");
            dict.Add("DISEASE_FAMILY", "(0148) ");
            dict.Add("SUSPENSION_4", "(0155) ");
            dict.Add("INDUSTRIAL_INJURY_CONT", "(0156) ");
            dict.Add("SICK_LEAVE_30_CONT", "(0157) ");
            dict.Add("CARE_OF_CHILD", "(0160) ");
            dict.Add("UNPAID_LEAVE", "(0169) ");
            dict.Add("PREGNANCY_LEAVE", "(0257) ");
            dict.Add("TISSUE_DONATION", "(0357) ");
            dict.Add("PERSONAL_LEAVE", "(0369) ");
            dict.Add("DEATH_GRANDPARENTS", "(1045) ");
            dict.Add("MOVING_EMPLOYEE", "(1144) ");
            dict.Add("DEATH_FAMILY", "(1145) ");
            dict.Add("ILLNESS_FAMILY", "(1148) ");
            dict.Add("WEDDING_CHILDREN", "(1149) ");
            dict.Add("RECREATIONAL_HOLIDAY", "(1150) ");
            dict.Add("DISCIPLINARY_SUSPENSION", "(1155) ");
            dict.Add("SICK_LEAVE_NO_PAYMENT", "(1157) ");
            dict.Add("CARE_OF_CHILD_3", "(1160) ");
            dict.Add("PREGNENCY_LEAVE_30", "(1257) ");
            dict.Add("STRIKE", "(1407) ");
            dict.Add("SICK_LEAVE", "(9999) ");
            dict.Add("NCF_CHILD_BEARING_FAMILY", "(9999) ");
            dict.Add("NCF_CHILD_BEARING", "(9999) ");
            dict.Add("NCF_DEATH_CLOSE_FAMILY", "(9999) ");
            dict.Add("NCF_DEATH_MARITIAL_FAMILY", "(9999) ");
            dict.Add("NCF_DRILL_SUMMONS", "(9999) ");
            dict.Add("NCF_ILLNESS_FAMILY", "(9999) ");
            dict.Add("NCF_UNPAID_LEAVE", "(9999) ");
            dict.Add("NCF_MOVING_EMPLOYEE", "(9999) ");
            dict.Add("NCF_RECREATIONAL_HOLIDAY", "(9999) ");
            dict.Add("NCF_REPAIR_OF_DEMAGE", "(9999) ");
            dict.Add("NCF_DISEASE_FAMILY", "(9999) ");
            dict.Add("NCF_EXAMINATION", "(9999) ");
            dict.Add("NCF_WEDDING_CHILDREN", "(9999) ");
            dict.Add("NCF_WEDDING", "(9999) ");
            dict.Add("NCF_BLOOD_DONATION", "(9999) ");
            dict.Add("NCF_DEATH_GRANDPARENTS", "(9999) ");
            dict.Add("ABSENCE", "(0000) ");

            return dict;
        }

        public enum LockedDayType
        {
            MASSIVE_INPUT = 0            
        }

        public enum EmplTypesVisibility
        {
            WCDR = 0,
            SUPERVISOR = 1
        }

        public static List<string> WNEffectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyStopWorkingDone);
            list.Add(RuleCompanyBankHour);
            list.Add(RuleCompanyOvertimePaid);
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            list.Add(RuleCompanyPeriodicalMedicalCheckUp);
            
            return list;
        }

        public static List<string> WNEffectiveMealWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyStopWorkingDone);
            list.Add(RuleCompanyBankHour);
            list.Add(RuleCompanyOvertimePaid);
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            list.Add(RuleCompanyPeriodicalMedicalCheckUp);
            list.Add(RuleCompanyBankHourUsed);

            return list;
        }

        public static List<string> WNSundayBHWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyBankHour);

            return list;
        }

        public static List<string> WNSaturdayBHWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyBankHour);

            return list;
        }

        public static List<string> WNSundayWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyOvertimePaid);
            
            return list;
        }

        public static List<string> WNSaturdayWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyOvertimePaid);

            return list;
        }

        public static List<int> WNWeekendWorkEmplTypes()
        {
            List<int> types = new List<int>();
            types.Add((int)Constants.EmployeeTypesWN.DIRECT);
            types.Add((int)Constants.EmployeeTypesWN.INDIRECT);

            return types;
        }

        public static List<int> WNBHWeekendWorkEmplTypes()
        {
            List<int> types = new List<int>();
            types.Add((int)Constants.EmployeeTypesWN.DIRECT);

            return types;
        }

        public const int WNMinMealPresence = 300; // minimal presence for meals in minutes

        public static List<string> CAEffectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyStopWorkingDone);
            list.Add(RuleCompanyBankHour);
            list.Add(RuleCompanyOvertimePaid);
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            list.Add(RuleCompanyPeriodicalMedicalCheckUp);
            list.Add(RuleCompanyUnionActivities);
            list.Add(RuleCompanyTrening);
            list.Add(RuleCompanyManagers);

            return list;
        }

        public static List<string> PMCAgencyEffectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyRegularWork);

            return list;
        }

        public static List<string> PMCEffectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyOvertimePaid);
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            
            return list;
        }

        public static List<string> PMCMealEffectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyOvertimePaid);
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);
            list.Add(RuleCompanyStopWorking);

            return list;
        }

        public const int unpaidLeave = 80;
        public const int unjustifiedLeave = 89;
        public static List<int> PMCUnpaidLeavesTypes()
        {
            List<int> list = new List<int>();
            //list.Add(unpaidLeave);
            //list.Add(unjustifiedLeave);

            return list;
        }

        public const int PMCWorkInjurySickLeave = 26;
        public const int PMCWorkInjurySickLeaveCont = 40;
        private const int PMCCareOfChildUnder3Years = 63;
        public static List<int> PMCSickLeaves100Types()
        {
            List<int> list = new List<int>();
            list.Add(PMCWorkInjurySickLeave);
            list.Add(PMCWorkInjurySickLeaveCont);
            list.Add(PMCCareOfChildUnder3Years);

            return list;
        }

        private const int PMCSickLeaveUntil30Days = 25;
        private const int PMCSickLeaveUntil30DaysCont = 39;
        public static List<int> PMCSickLeaves65Types()
        {
            List<int> list = new List<int>();
            list.Add(PMCSickLeaveUntil30Days);
            list.Add(PMCSickLeaveUntil30DaysCont);

            return list;
        }

        private const int PMCSickLeaveOver30Days = 27;
        public static List<int> PMCSickLeaves30DaysTypes()
        {
            List<int> list = new List<int>();
            list.Add(PMCSickLeaveOver30Days);
            
            return list;
        }

        private const int PMCSickLeavePregnancy = 43;
        private const int PMCSickLeaveMaternity = 28;
        public static List<int> PMCSickLeavesPregnancyTypes()
        {
            List<int> list = new List<int>();
            list.Add(PMCSickLeavePregnancy);
            list.Add(PMCSickLeaveMaternity);

            return list;
        }

        private const int PMCPaidLeave60 = 95;
        public static List<int> PMCPaidLeave60Types()
        {
            List<int> list = new List<int>();
            list.Add(PMCPaidLeave60);

            return list;
        }

        private const int HyattSickLeaveUntil30Days = 25;
        private const int HyattSickLeaveUntil30DaysCont = 39;
        private const int HyattPregnancyLeaveUpTo30Days = 90;
        public static List<int> HyattSickLeavesUntil30DaysTypes()
        {
            List<int> list = new List<int>();
            list.Add(HyattSickLeaveUntil30Days);
            list.Add(HyattSickLeaveUntil30DaysCont);
            list.Add(HyattPregnancyLeaveUpTo30Days);

            return list;
        }

        private const int HyattSickLeaveOver30Days = 27;
        public static List<int> HyattSickLeavesOver30DaysTypes()
        {
            List<int> list = new List<int>();

            list.Add(HyattSickLeaveOver30Days);
            return list;
        }

        private const int HyattIndustrialInjury = 26;
        private const int HyattIndustrialInjuryCont = 40;
        public static List<int> HyattIndustrialInjuryTypes()
        {
            List<int> list = new List<int>();
            list.Add(HyattIndustrialInjury);
            list.Add(HyattIndustrialInjuryCont);

            return list;
        }


        private const int HyattPregnancyLeave = 43;
        private const int HyattMaternityLeave = 28;
        //private const int HyattSpecialCareOfChild = 62;
        //private const int HyattCareOfChildUnder3Years = 63;
        public static List<int> HyattPregnancyLeaveTypes()
        {
            List<int> list = new List<int>();
            list.Add(HyattPregnancyLeave);
            list.Add(HyattMaternityLeave);
            //list.Add(HyattSpecialCareOfChild);
            //list.Add(HyattCareOfChildUnder3Years);

            return list;
        }

        private const int HyattSickLeaveWithoutPayment = 91;
        public static List<int> HyattSickLeaveWithoutPaymentTypes()
        {
            List<int> list = new List<int>();
            list.Add(HyattSickLeaveWithoutPayment);

            return list;
        }


        public static Dictionary<string, string> MassiveInputFailedMessages()
        {
            Dictionary<string, string> messages = new Dictionary<string, string>();
            messages.Add("overtimeLessThenMinimum", "OVERTIME PRESENCE LESS THEN MINIMAL PRESENCE");
            messages.Add("overtimeNotValidRoundingRule", "NOT VALID OVERTIME ROUNDING");
            messages.Add("overtimeBeforeShiftEndRule", "NOT VALID OVERTIME AFTER SHIFT END");
            messages.Add("overtimeBeforeShiftStartRule", "NOT VALID OVERTIME BEFORE SHIFT START");
            messages.Add("overtimeNegative", "NEGATIVE OVERTIME COUNTER");
            messages.Add("bankHourNegative", "NEGATIVE BANK HOURS COUNTER");
            messages.Add("stopWorkingNegative", "NEGATIVE STOP WORKING COUNTER");
            messages.Add("notEnoughStopWorkingHours", "NOT ENOUGH STOP WORKING HOURS");
            messages.Add("notEnoughBankHour", "NOT ENOUGH BANK HOURS");
            messages.Add("overtimeDayLimitReached", "OVERTIME DAY LIMIT REACHED");
            messages.Add("overtimeWeekLimitReached", "OVERTIME WEEK LIMIT REACHED");
            messages.Add("bankHourWeekLimitReached", "BANK HOURS WEEK LIMIT REACHED");
            messages.Add("overtimeLeftForUse", "MUST USE OVERTIME FIRST");
            messages.Add("bankHourDayLimitReached", "BANK HOURS DAY LIMIT REACHED");
            messages.Add("noRightToUseAnnualLeave", "NO RIGHT TO USE ANUAL LEAVE");
            messages.Add("noMoreAnnualLeave", "NO MORE FREE DAYS OF ANNUAL LEAVE FOR USING");
            messages.Add("ptElementaryLimitReached", "PASS TYPE ELEMENTARY LIMIT REACHED");
            messages.Add("ptCompositeLimitReached", "PASS TYPE COMPOSITE LIMIT REACHED");
            messages.Add("bankHourLimitReached", "BANK HOURS LIMIT REACHED");
            messages.Add("notEnoughOvertimeNotJustifiedHours", "NOT ENOUGH OVERTIME NOT JUSTIFIED HOURS");
            messages.Add("noMoreReservedCollectiveAnnualLeave", "NOT ENOUGH RESERVED DAYS FOR COLLECTIVE ANNUAL LEAVE");
            messages.Add("pairLessThenMinimum", "PAIR DURATION IS LESS THEN MINIMAL ALLOWED");
            messages.Add("delayMaxDurationExceeded", "MAXIMAL ALLOWED DELAY DURATION IS EXCEEDED");
            messages.Add("notEnoughFreeHoursBetweenWorkingHours", "NOT ENOUGH FREE HOURS BETWEEN WORKING HOURS");
            messages.Add("useBankHoursBeforeVacation", "CAN NOT USE VACATION BEFORE USING BANK HOURS");
            messages.Add("useNotjustifedOvertimeHoursBeforeVacation", "CAN NOT USE VACATION BEFORE USING NOT JUSTIFIED OVERTIME HOURS");
            messages.Add("useNotjustifedOvertimeBankHoursBeforeVacation", "CAN NOT USE VACATION BEFORE USING NOT JUSTIFIED OVERTIME AND BANK HOURS");
            messages.Add("useAnnualLeaveBeforeBankHours", "CAN NOT USE BANK HOURS BEFORE USING VACATION");
            return messages;
        }

        public enum MassiveInputStatus
        {
            DONE = 0,
            FAILED = 1,
            NO_CHANGES = 2
        }

        public enum ColumnTypes
        {
            TEXT = 0,
            EDIT_TEXT = 1,
            VISIT_HIST = 2,
            CHANGE_TEXT = 3,
            TOOLTIP = 4
        }

        public enum MedicalCheckVisitStatus
        {
            WR = 0,
            RND = 1,
            DONE = 2,
            DEMANDED = 3,
            DELETED = 4
        }

        public enum VisitType
        {
            V = 0,
            R = 1,
            D = 2
        }

        public enum MedicalCheckDisabilityType
        {
            PERMANENT = 0,
            TEMPORARY = 1            
        }

        public enum VisitResult
        {
            OK = 0,
            NOK = 1
        }
        public enum EmailFlag
        {
            OK = 1,
            NOK = 0
        }

        public const int defaultMedicalCheckPointId = 0;

        public const int MCSchedulingIntervalsColIndex = 11;
        public const int MCSchedulingPointColIndex = 13;
        public const int MCSchedulingTerminColIndex = 14;
        public const int MCSchedulingDColIndex = 15;
        public const int MCSchedulingMColIndex = 16;
        public const int MCSchedulingYColIndex = 17;

        public enum SyncPreviewType
        {
            FS = 0,
            OU = 1,
            CC = 2,
            POS = 3,
            EMPL = 4,
            RES = 5,
            AL = 6
        }

        public const int acfNumOfSetsLeave = 2;
        public const int defaultClockINOffset = 30; // in minutes
        public const string male = "M";
        public const string female = "F";

        public const string closingEventTypeDemanded = "DEMANDED";
        public const string closingEventTypeRegularPeriodical = "REGULAR PERIODICAL";

        public enum DPEngineState
        {
            STARTED = 0,
            FINISHED = 1            
        }

        public static List<string> GeoxEffectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyStopWorkingDone);
            list.Add(RuleCompanyBankHour);
            list.Add(RuleCompanyOvertimePaid);
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);

            return list;
        }

        public static List<int> GeoxUnpaidLeaves()
        {
            List<int> list = new List<int>();
            list.Add(unpaidLeave);

            return list;
        }

        private const int geoxSL30_65 = 25;
        private const int geoxSL30_100 = 27;        
        public static List<int> GeoxSickLeaves30Days()
        {
            List<int> list = new List<int>();
            list.Add(geoxSL30_65);
            list.Add(geoxSL30_100);

            return list;
        }

        private const int geoxSLRefundation_65 = 39;
        private const int geoxSLRefundation_100 = 16;
        private const int geoxSLMaternity = 28;        
        public static List<int> GeoxSickLeavesRefundation()
        {
            List<int> list = new List<int>();
            list.Add(geoxSLRefundation_65);
            list.Add(geoxSLRefundation_100);
            list.Add(geoxSLMaternity);

            return list;
        }

        public static List<string> GrundfosEffectiveWorkWageTypes()
        {
            List<string> list = new List<string>();
            list.Add(RuleCompanyStopWorkingDone);
            list.Add(RuleCompanyBankHour);
            list.Add(RuleCompanyOvertimePaid);
            list.Add(RuleCompanyRegularWork);
            list.Add(RuleCompanyOfficialOut);

            return list;
        }

        public const int ptFiatSWPay = -500;

        public const int maxVisibleTabs = 10;

        public static Dictionary<string, string> FiatWorkLocations()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("0445K", "Kosovska 4, Kragujevac");
            dict.Add("0987K", "Kosovska 4, Kragujevac");
            dict.Add("3056B", "Despota Stefana 12, Beograd");
            dict.Add("3056J", "JCMM");
            dict.Add("3056K", "Kosovska 4, Kragujevac");
            dict.Add("RS03", "Kosovska 4, Kragujevac");
            dict.Add("RS04", "Despota Stefana 12, Beograd");

            return dict;
        }

        public static List<string> WeekDays()
        {
            List<string> days = new List<string>();

            days.Add(DayOfWeek.Monday.ToString());
            days.Add(DayOfWeek.Tuesday.ToString());
            days.Add(DayOfWeek.Wednesday.ToString());
            days.Add(DayOfWeek.Thursday.ToString());
            days.Add(DayOfWeek.Friday.ToString());
            days.Add(DayOfWeek.Saturday.ToString());
            days.Add(DayOfWeek.Sunday.ToString());

            return days;
        }

        public const string FiatCollectiveAnnualLeavePaymentCode = "CANL";
        public const string FiatClosurePaymentCode = "C";
        public const string FiatLayOffPaymentCode = "L";
        public const string FiatStoppagePaymentCode = "S";
        public const string FiatPublicHollidayPaymentCode = "PH";
        public const string FiatClosure = "closure";
        public const string FiatLayOff = "layoff";
        public const string FiatStoppage = "stoppage";
        public const string FiatPublicHolliday = "public holiday";

        public static List<int> FiatClosureTypes()
        {
            // only for FAS!!!!!
            List<int> closureTypes = new List<int>();
            closureTypes.Add(94);
            closureTypes.Add(98);
            closureTypes.Add(102);
            closureTypes.Add(106);
            closureTypes.Add(110);
            closureTypes.Add(114);
            closureTypes.Add(118);
            closureTypes.Add(122);
            closureTypes.Add(126);
            closureTypes.Add(130);
            closureTypes.Add(134);
            closureTypes.Add(138);
            closureTypes.Add(142);
            closureTypes.Add(146);
            closureTypes.Add(150);
            closureTypes.Add(154);
            closureTypes.Add(158);
            closureTypes.Add(162);
            closureTypes.Add(166);
            closureTypes.Add(170);
            closureTypes.Add(174);
            closureTypes.Add(178);
            closureTypes.Add(182);
            closureTypes.Add(186);
            closureTypes.Add(190);
            closureTypes.Add(194);
            closureTypes.Add(198);
            closureTypes.Add(202);
            closureTypes.Add(206);
            closureTypes.Add(210);
            closureTypes.Add(214);
            closureTypes.Add(218);
            closureTypes.Add(222);
            closureTypes.Add(226);
            closureTypes.Add(230);
            closureTypes.Add(234);
            closureTypes.Add(238);
            closureTypes.Add(242);
            closureTypes.Add(246);
            closureTypes.Add(250);
            closureTypes.Add(254);
            closureTypes.Add(258);
            closureTypes.Add(262);
            closureTypes.Add(266);
            closureTypes.Add(270);
            closureTypes.Add(274);
            closureTypes.Add(278);
            closureTypes.Add(282);
            closureTypes.Add(286);
            closureTypes.Add(290);
            closureTypes.Add(295);
            closureTypes.Add(299);
            closureTypes.Add(303);
            closureTypes.Add(311);
            closureTypes.Add(315);
            closureTypes.Add(319);
            closureTypes.Add(323);
            closureTypes.Add(328);
            return closureTypes;
        }

        public static List<int> FiatLayOffTypes()
        {
            List<int> layoffTypes = new List<int>();
            layoffTypes.Add(95);
            layoffTypes.Add(99);
            layoffTypes.Add(103);
            layoffTypes.Add(107);
            layoffTypes.Add(111);
            layoffTypes.Add(115);
            layoffTypes.Add(119);
            layoffTypes.Add(123);
            layoffTypes.Add(127);
            layoffTypes.Add(131);
            layoffTypes.Add(135);
            layoffTypes.Add(139);
            layoffTypes.Add(143);
            layoffTypes.Add(147);
            layoffTypes.Add(151);
            layoffTypes.Add(155);
            layoffTypes.Add(159);
            layoffTypes.Add(163);
            layoffTypes.Add(167);
            layoffTypes.Add(171);
            layoffTypes.Add(175);
            layoffTypes.Add(179);
            layoffTypes.Add(183);
            layoffTypes.Add(187);
            layoffTypes.Add(191);
            layoffTypes.Add(195);
            layoffTypes.Add(199);
            layoffTypes.Add(203);
            layoffTypes.Add(207);
            layoffTypes.Add(211);
            layoffTypes.Add(215);
            layoffTypes.Add(219);
            layoffTypes.Add(223);
            layoffTypes.Add(227);
            layoffTypes.Add(231);
            layoffTypes.Add(235);
            layoffTypes.Add(239);
            layoffTypes.Add(243);
            layoffTypes.Add(247);
            layoffTypes.Add(251);
            layoffTypes.Add(255);
            layoffTypes.Add(259);
            layoffTypes.Add(263);
            layoffTypes.Add(267);
            layoffTypes.Add(271);
            layoffTypes.Add(275);
            layoffTypes.Add(279);
            layoffTypes.Add(283);
            layoffTypes.Add(287);
            layoffTypes.Add(291);
            layoffTypes.Add(296);
            layoffTypes.Add(300);
            layoffTypes.Add(304);
            layoffTypes.Add(312);
            layoffTypes.Add(316);
            layoffTypes.Add(320);
            layoffTypes.Add(324);
            layoffTypes.Add(329);
            return layoffTypes;
        }

        public static List<int> FiatStoppageTypes()
        {
            List<int> stoppageTypes = new List<int>();
            stoppageTypes.Add(97);
            stoppageTypes.Add(101);
            stoppageTypes.Add(105);
            stoppageTypes.Add(109);
            stoppageTypes.Add(113);
            stoppageTypes.Add(117);
            stoppageTypes.Add(121);
            stoppageTypes.Add(125);
            stoppageTypes.Add(129);
            stoppageTypes.Add(133);
            stoppageTypes.Add(137);
            stoppageTypes.Add(141);
            stoppageTypes.Add(145);
            stoppageTypes.Add(149);
            stoppageTypes.Add(153);
            stoppageTypes.Add(157);
            stoppageTypes.Add(161);
            stoppageTypes.Add(165);
            stoppageTypes.Add(169);
            stoppageTypes.Add(173);
            stoppageTypes.Add(177);
            stoppageTypes.Add(181);
            stoppageTypes.Add(185);
            stoppageTypes.Add(189);
            stoppageTypes.Add(193);
            stoppageTypes.Add(197);
            stoppageTypes.Add(201);
            stoppageTypes.Add(205);
            stoppageTypes.Add(209);
            stoppageTypes.Add(213);
            stoppageTypes.Add(217);
            stoppageTypes.Add(221);
            stoppageTypes.Add(225);
            stoppageTypes.Add(229);
            stoppageTypes.Add(233);
            stoppageTypes.Add(237);
            stoppageTypes.Add(241);
            stoppageTypes.Add(245);
            stoppageTypes.Add(249);
            stoppageTypes.Add(253);
            stoppageTypes.Add(257);
            stoppageTypes.Add(261);
            stoppageTypes.Add(265);
            stoppageTypes.Add(269);
            stoppageTypes.Add(273);
            stoppageTypes.Add(277);
            stoppageTypes.Add(281);
            stoppageTypes.Add(285);
            stoppageTypes.Add(289);
            stoppageTypes.Add(293);
            stoppageTypes.Add(298);
            stoppageTypes.Add(302);
            stoppageTypes.Add(306);
            stoppageTypes.Add(314);
            stoppageTypes.Add(318);
            stoppageTypes.Add(322);
            stoppageTypes.Add(326);
            stoppageTypes.Add(331);
            return stoppageTypes;
        }

        public static List<int> FiatHolidayTypes()
        {
            List<int> holidayTypes = new List<int>();
            holidayTypes.Add(96);
            holidayTypes.Add(100);
            holidayTypes.Add(104);
            holidayTypes.Add(108);
            holidayTypes.Add(112);
            holidayTypes.Add(116);
            holidayTypes.Add(120);
            holidayTypes.Add(124);
            holidayTypes.Add(128);
            holidayTypes.Add(132);
            holidayTypes.Add(136);
            holidayTypes.Add(140);
            holidayTypes.Add(144);
            holidayTypes.Add(148);
            holidayTypes.Add(152);
            holidayTypes.Add(156);
            holidayTypes.Add(160);
            holidayTypes.Add(164);
            holidayTypes.Add(168);
            holidayTypes.Add(172);
            holidayTypes.Add(176);
            holidayTypes.Add(180);
            holidayTypes.Add(184);
            holidayTypes.Add(188);
            holidayTypes.Add(192);
            holidayTypes.Add(196);
            holidayTypes.Add(200);
            holidayTypes.Add(204);
            holidayTypes.Add(208);
            holidayTypes.Add(212);
            holidayTypes.Add(216);
            holidayTypes.Add(220);
            holidayTypes.Add(224);
            holidayTypes.Add(228);
            holidayTypes.Add(232);
            holidayTypes.Add(236);
            holidayTypes.Add(240);
            holidayTypes.Add(244);
            holidayTypes.Add(248);
            holidayTypes.Add(252);
            holidayTypes.Add(256);
            holidayTypes.Add(260);
            holidayTypes.Add(264);
            holidayTypes.Add(268);
            holidayTypes.Add(272);
            holidayTypes.Add(276);
            holidayTypes.Add(280);
            holidayTypes.Add(284);
            holidayTypes.Add(288);
            holidayTypes.Add(292);
            holidayTypes.Add(297);
            holidayTypes.Add(301);
            holidayTypes.Add(305);
            holidayTypes.Add(313);
            holidayTypes.Add(317);
            holidayTypes.Add(321);
            holidayTypes.Add(325);
            holidayTypes.Add(330);
            return holidayTypes;
        }
        
        public const int FiatInactiveUsersMonths = 6;

        public const double standardFooterTableWidth = 990;
    
        public static string EMailAddress
        {
            get
            {
                string emailAddress = "";
                if (ConfigurationManager.AppSettings["emailAddress"] != null)
                    emailAddress = ConfigurationManager.AppSettings["emailAddress"].Trim();
                return emailAddress;
            }
        }

        public static string Host
        {
            get
            {
                string host = "";
                if (ConfigurationManager.AppSettings["host"] != null)
                    host = ConfigurationManager.AppSettings["host"].Trim();
                return host;
            }
        }

        public static string UserName
        {
            get
            {
                // *********** there is \ symbol in string
                string userName = "";
                if (ConfigurationManager.AppSettings["userName"] != null)
                    userName = ConfigurationManager.AppSettings["userName"].Trim();
                return userName;
            }
        }

        public static string Password
        {
            get
            {
                string password = "";
                if (ConfigurationManager.AppSettings["password"] != null)
                    password = ConfigurationManager.AppSettings["password"].Trim();
                return password;
            }
        }

        public static string MailsToSent
        {
            get
            {
                string mailsToSent = "";
                if (ConfigurationManager.AppSettings["mailsToSent"] != null)
                    mailsToSent = ConfigurationManager.AppSettings["mailsToSent"].Trim();
                return mailsToSent;
            }
        }

        public static DateTime StartDownloadingTime
        {
            get
            {
                DateTime startDownloadingTime = new DateTime();
                if (ConfigurationManager.AppSettings["startDownloadingTime"] == null || ConfigurationManager.AppSettings["startDownloadingTime"].Equals("")
                    || (!DateTime.TryParseExact(ConfigurationManager.AppSettings["startDownloadingTime"].ToString().Trim(), Constants.timeFormat, null, DateTimeStyles.None, out startDownloadingTime)))
                    startDownloadingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 2, 0, 0);

                return startDownloadingTime;
            }
        }

        public static int DownloadingTimeout
        {
            get
            {
                int downloadingTimeout = -1;
                if (ConfigurationManager.AppSettings["downloadingTimeout"] == null || ConfigurationManager.AppSettings["downloadingTimeout"].Equals("")
                    || (!int.TryParse(ConfigurationManager.AppSettings["downloadingTimeout"].ToString().Trim(), out downloadingTimeout)))
                    downloadingTimeout = 15;

                return downloadingTimeout;
            }
        }

        public static List<int> VechiclePoints
        {
            get
            {
                List<int> vechiclePoints = new List<int>();
                if (ConfigurationManager.AppSettings["vechiclePoints"] != null && !ConfigurationManager.AppSettings["vechiclePoints"].Trim().Equals(""))
                {
                    string[] points = ConfigurationManager.AppSettings["vechiclePoints"].Trim().Split(',');

                    foreach (string point in points)
                    {
                        int pID = -1;
                        if (int.TryParse(point.Trim(), out pID))
                            vechiclePoints.Add(pID);
                    }
                }

                return vechiclePoints;
            }
        }

        // minimal duration between shifts in Fiat in hours
        public const int FiatShiftMinimalGap = 12;
        public const int FiatShiftMinimalFreeDayGap = 7; 

        //restoran prica meals type for Saturday and Sunday
        public const int saturdayMealType = -6;
        public const int sundayMealType = -7;

        public enum GrundfosAddCodes
        {
            OB = 0,
            ON = 1,
            REM = 2,
            MEAL = 3
        }

        public static List<string> ReadTagIPList
        {
            get
            {
                List<string> list = new List<string>();
                if (ConfigurationManager.AppSettings["readTagIP"] != null && !ConfigurationManager.AppSettings["readTagIP"].Trim().Equals(""))
                {
                    string[] ipList = ConfigurationManager.AppSettings["readTagIP"].Trim().Split(',');

                    foreach (string ip in ipList)
                    {
                        list.Add(ip.Trim());
                    }
                }

                return list;
            }
        }

        public const int rfidLoginTimeSec = 10;

        public const int defaultBHBalanceMonth = 9;
        public const int defaultSWBalanceMonth = 9;
    }
}
