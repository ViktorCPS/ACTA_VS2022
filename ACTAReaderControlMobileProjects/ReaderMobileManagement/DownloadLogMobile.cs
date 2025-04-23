using System;
using System.Collections;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Ionic.Zip;

using Common;
using TransferObjects;
using Util;

namespace ReaderMobileManagement
{
	/// <summary>
	/// DownloadLog
	/// </summary>
	public class DownloadLogMobile : IComparable
	{
		// File sufix
		private static string FILESUFFIX = "yyyyMMddHHmmss";
		private static string XMLEXT = ".xml";
        private static string TXTEXT = ".TXT";
        private static string JPGEXT = ".JPG";
        private static string JPEGEXT = ".JPEG";
		
		private string logReadingDirectory = "";
        private string logWrittingDirectory = "";
        private string trashDirectory = "";
        private string archiveDirectory = "";
        private string extractDirectory = "";
        private string extractTmpDirectory = "";
        private string snapshotsDirectory = "";
        		
        private DateTime nextTimeReading = new DateTime();
        private int _downloadingTimeout = -1;
		private int _numOfAttempts = 0;
		private int _totalReadingAttempts = 0;
		
		// Debug
		DebugLog debug;
        
		public DateTime NextTimeReading
		{
			get { return nextTimeReading; }
			set { nextTimeReading = value; }
		}

        public int DownloadingTimeout
        {
            get { return _downloadingTimeout; }
            set { _downloadingTimeout = value; }
        }

		public int NumOfAttempts
		{
			get { return _numOfAttempts; }
			set { _numOfAttempts = value; }
		}

		public int TotalReadingAttempts
		{
			get { return _totalReadingAttempts; }
			set { _totalReadingAttempts = value; }
		}

		// Observer initialization
		// Send message to other object that log downloading has started
		// Controller instance
		public NotificationController Controller;
		// Observer client instance
		public NotificationObserverClient observerClient;

		public DownloadLogMobile(string writtingDir)
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

            NextTimeReading = Constants.StartDownloadingTime;
            DownloadingTimeout = Constants.DownloadingTimeout;

            // set next time reading to first next reading moment
            if (NextTimeReading > DateTime.Now)
            {
                while (NextTimeReading.AddMinutes(-DownloadingTimeout) > DateTime.Now)
                {
                    NextTimeReading = NextTimeReading.AddMinutes(-DownloadingTimeout);
                }
            }
            else
            {
                while (NextTimeReading <= DateTime.Now)
                {
                    NextTimeReading = NextTimeReading.AddMinutes(DownloadingTimeout);
                }
            }            
            
            logReadingDirectory = Constants.MobileDownload;
            logWrittingDirectory = writtingDir;
            trashDirectory = Constants.trashMobileDownload;
            archiveDirectory = Constants.archivedMobileDownload;
            extractDirectory = Constants.MobileExtract;
            extractTmpDirectory = Constants.MobileExtractTmp;
            snapshotsDirectory = Constants.snapshots;

			InitializeObserverClient();
		}

        //****************?????
        public int CompareTo(object obj)
        {
            DownloadLogMobile log = (DownloadLogMobile)obj;
            return 0;
        }
		
		public void GetNextReading()
		{
			try
			{
                NextTimeReading = NextTimeReading.AddMinutes(DownloadingTimeout);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ex.Message);
			}
		}

		/// <summary>
		/// Read log from ftp folder, pushing to the log file
		/// </summary>
		/// <returns>true if succeeded, false otherways</returns>
		public bool GetLogFiles(Dictionary<string, int> readersIMEIDict, Dictionary<int, List<CameraTO>> readersCamerasDict)
		{
            try
            {
                LogTmpAdditionalInfoTO logMobileTOData = new LogTmpAdditionalInfoTO();
                Dictionary<int, List<LogTmpAdditionalInfoTO>> logTODict = new Dictionary<int, List<LogTmpAdditionalInfoTO>>();
                ArrayList logMobileLineList = new ArrayList();

                if (!Directory.Exists(logReadingDirectory))
                    Directory.CreateDirectory(logReadingDirectory);

                DirectoryInfo redingInfo = new DirectoryInfo(logReadingDirectory);

                FileInfo[] ftpFileList = redingInfo.GetFiles();
                foreach (FileInfo ftpFile in ftpFileList)
                {
                    FileStream str = null;
                    StreamReader reader = null;

                    try
                    {
                        // extract files
                        if (!ExtractFile(ftpFile))
                        {
                            MoveFileToTrash(ftpFile.FullName);
                            DeleteExtract();
                            continue;
                        }

                        // get extracted files
                        DirectoryInfo extractInfo = new DirectoryInfo(extractDirectory);

                        FileInfo txtFile = null;
                        FileInfo imgFile = null;
                        foreach (FileInfo file in extractInfo.GetFiles())
                        {
                            if (file.Extension.Trim().ToUpper() == TXTEXT)
                                txtFile = file;
                            else if (file.Extension.Trim().ToUpper() == JPEGEXT || file.Extension.Trim().ToUpper() == JPGEXT)
                                imgFile = file;
                        }

                        if (txtFile == null && imgFile == null)
                        {
                            MoveFileToTrash(ftpFile.FullName);
                            DeleteExtract();
                            continue;
                        }

                        int camID = -1;
                        DateTime passTime = new DateTime();
                        if (txtFile != null)
                        {
                            str = txtFile.OpenRead();
                            reader = new StreamReader(str);

                            string logLine = reader.ReadLine();

                            while (logLine != null)
                            {
                                LogTmpAdditionalInfoTO logTO = new LogTmpAdditionalInfoTO();
                                logTO = logTO.CreateLogFromFileLine(logLine, readersIMEIDict);

                                if (logTO != null)
                                {
                                    if (!logTODict.ContainsKey(logTO.ReaderID))
                                        logTODict.Add(logTO.ReaderID, new List<LogTmpAdditionalInfoTO>());

                                    logTODict[logTO.ReaderID].Add(logTO);
                                    passTime = logTO.EventTime;
                                    if (readersCamerasDict.ContainsKey(logTO.ReaderID) && readersCamerasDict[logTO.ReaderID].Count > 0)
                                        camID = readersCamerasDict[logTO.ReaderID][0].CameraID;
                                }
                                else
                                    debug.writeLog(DateTime.Now + this.ToString() + ".GetLogFiles(): Invalid log data in file " + txtFile.Name.Trim() + "; line: " + logLine.Trim() + " \n");

                                logLine = reader.ReadLine();
                            }

                            reader.Dispose();
                            reader.Close();
                            str.Dispose();
                            str.Close();
                        }

                        // move img file to shapshots folder
                        if (imgFile != null && camID != -1 && passTime != new DateTime())
                        {
                            MoveFileToSnapshots(imgFile.FullName, camID, passTime);
                        }

                        // move original ftp file to archive
                        MoveFileToArchived(ftpFile.FullName);

                        // delete files from extraction folders
                        DeleteExtract();
                    }
                    catch (Exception ex)
                    {
                        debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".GetLogFiles(): " + ex.Message + " \n");

                        if (reader != null)
                        {
                            reader.Dispose();
                            reader.Close();
                        }

                        if (str != null)
                        {
                            str.Dispose();
                            str.Close();
                        }

                        MoveFileToTrash(ftpFile.FullName);
                        DeleteExtract();
                    }
                }

                // serialize to XML
                CreateXML(logTODict);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".GetLogFiles(): " + ex.Message + " \n");

                return false;
            }
		
			return true;
		}

        private void CreateXML(Dictionary<int, List<LogTmpAdditionalInfoTO>> logTODict)
        {
            try
            {
                Log currentLog = new Log();

                foreach (int readerID in logTODict.Keys)
                {
                    try
                    {
                        if (!Directory.Exists(logWrittingDirectory))
                            Directory.CreateDirectory(logWrittingDirectory);

                        // Send diff log XML file to unprocessed
                        string path = this.logWrittingDirectory
                            + Constants.ReaderXMLLogFile
                            + "_" + readerID.ToString().Trim()
                            + "_" + DateTime.Now.ToString(FILESUFFIX)
                            + XMLEXT;

                        bool succeed = false;

                        if (logTODict[readerID].Count > 0)
                        {
                            succeed = currentLog.SaveToFileMobile(logTODict[readerID], path);
                        }
                    }
                    catch (Exception ex)
                    {
                        debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".CreateXML() save logs to file: " + ex.Message + " \n");
                    }
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".CreateXML(): " + ex.Message + " \n");
            }
        }

        private bool ExtractFile(FileInfo file)
        {
            bool succeded = false;
            ZipFile zip = null;
            try
            {
                if (!Directory.Exists(extractDirectory))
                    Directory.CreateDirectory(extractDirectory);

                if (!Directory.Exists(extractTmpDirectory))
                    Directory.CreateDirectory(extractTmpDirectory);

                // first zip file must be unzip with password. Unzipped file contains regulary zipped file (txt and jpg files are zipped in this one)
                // get password from file_name
                string password = GetZipPassword(Path.GetFileNameWithoutExtension(file.FullName));

                zip = ZipFile.Read(file.FullName);
                zip.Password = password;
                zip.ExtractAll(extractTmpDirectory, ExtractExistingFileAction.OverwriteSilently);
                zip.Dispose();

                foreach (string filePath in Directory.GetFiles(extractTmpDirectory))
                {
                    zip = ZipFile.Read(filePath);
                    zip.ExtractAll(extractDirectory, ExtractExistingFileAction.OverwriteSilently);
                }

                zip.Dispose();

                succeded = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".ExtractFile() : " + ex.Message);

                if (zip != null)
                    zip.Dispose();
            }

            return succeded;
        }

        private string GetZipPassword(string fileName)
        {
            // file name is R_<ID>_<yyyyMMddHHmmss>.zip - password is <ID><ss>B
            string password = "";
            try
            {
                if (fileName.Length > 2)
                {
                    fileName = fileName.Substring(2);

                    int index = fileName.IndexOf("_");

                    if (index >= 0 && fileName.Length > 2)
                        password = fileName.Substring(0, index) + fileName.Substring(fileName.Length - 2);
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".GetZipPassword() : " + ex.Message);
                password = "";
            }

            return password;
        }

        private bool DeleteExtract()
        {
            bool isSucceeded = false;

            try
            {
                if (Directory.Exists(extractDirectory))
                {
                    foreach (string path in Directory.GetFiles(extractDirectory))
                    {
                        File.Delete(path);
                    }
                }

                if (Directory.Exists(extractTmpDirectory))
                {
                    foreach (string path in Directory.GetFiles(extractTmpDirectory))
                    {
                        File.Delete(path);
                    }
                }

                isSucceeded = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".DeleteExtract() : " + ex.Message);
            }

            return isSucceeded;
        }

        private bool MoveFileToArchived(string filepath)
        {
            string targetPath = "";
            bool isSucceeded = false;

            try
            {
                if (!Directory.Exists(archiveDirectory))
                    Directory.CreateDirectory(archiveDirectory);

                targetPath = archiveDirectory + Path.GetFileNameWithoutExtension(filepath) + "_Copy" + DateTime.Now.ToString(FILESUFFIX) + Path.GetExtension(filepath);
                File.Move(filepath, targetPath);
                isSucceeded = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".MoveFileToArchived() : " + ex.Message);
            }

            return isSucceeded;
        }

        private bool MoveFileToTrash(string filepath)
        {
            bool isSucceeded = false;

            try
            {
                if (!Directory.Exists(trashDirectory))
                    Directory.CreateDirectory(trashDirectory);

                string targetPath = trashDirectory + Path.GetFileNameWithoutExtension(filepath) + "_Copy" + DateTime.Now.ToString(FILESUFFIX) + Path.GetExtension(filepath);
                File.Move(filepath, targetPath);
                isSucceeded = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".MoveFileToArchived() : " + ex.Message);
            }

            return isSucceeded;
        }

        private bool MoveFileToSnapshots(string filePath, int camID, DateTime passTime)
        {
            bool isSucceeded = false;

            try
            {
                if (!Directory.Exists(snapshotsDirectory))
                    Directory.CreateDirectory(snapshotsDirectory);

                //fileName is C_three digits camera ID_yy-MM-dd_HH-mm-ss-msms.jpg
                //for example C_123_08-02-12_16-38-14-88.jpg

                string targetPath = snapshotsDirectory + "C_" + camID.ToString().PadLeft(3, '0') + "_" + passTime.ToString("yy-MM-dd_HH-mm-ss-ff") + Path.GetExtension(filePath);
                File.Move(filePath, targetPath);
                isSucceeded = true;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " + this.ToString() + ".MoveFileToSnapshots() : " + ex.Message);
            }

            return isSucceeded;
        }

        private void InitializeObserverClient()
        {
            observerClient = new NotificationObserverClient(this.ToString());
            Controller = NotificationController.GetInstance();
            //Controller.AttachToNotifier(observerClient);
            //this.observerClient.Notification += new NotificationEventHandler(this.MonitorEvent);
        }
	}
}
