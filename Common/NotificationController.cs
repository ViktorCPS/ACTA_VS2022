using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Util;
using DataAccess;
using TransferObjects;

namespace Common
{
	/// <summary>
	/// NotificationController class is used to manage messages that are passed between 
	/// classes (most of them are Windows Forms). After getting an instance of the NotificationController 
	/// and Observer Client, an object is enable to receive and to send messages. So if one object made some 
	/// changes that maybe important to other objects(win forms) it do notification about the changes.
	/// FormNotifier contain an array of ObserverCilent that are used as an interface 
	/// to classes that we need to notify. When Observer Cilent receive message, it fires proper event.
	/// NotificationObserverClient instance, is interface that provide events to send and receive
	/// notification. After receiving this messege, other classes can perform further actions.
	/// </summary>
	/// <seealso>NotificationObserverClient, FaxNotifier, NotificationEventArgs</seealso>
	public class NotificationController
	{
		private static NotificationController instance;
		private static Notifier notifier;
		private static ApplUserTO logInUser;
		private static ApplUserLogTO log;
		private static List<ApplRoleTO> currentRoles = new List<ApplRoleTO>();
		private static string currentMenuItemID;
		private static Hashtable menuItemsPermissions;
		private static string applName = "";
		private static bool changePassword;
        private static Lock lastLocking;

		DebugLog debug;

		protected NotificationController()
		{
			// Debug
			string logFilePath = Constants.logFilePath + GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);
			
			notifier = Notifier.GetInstance();
			DAOController.GetInstance();
		}

		/// <summary>
		/// Create instance of the class. 
		/// There must be only one instance of DmController
		/// class.
		/// </summary>
		/// <returns>DmController</returns>
		public static NotificationController GetInstance()
		{
			if (instance == null)
			{
				instance = new NotificationController();
			}
			return instance;
		}

		public static void SetChangePassword(bool change)
		{
			changePassword = change;
		}

		public static bool GetChangePassword()
		{
			return changePassword;
		}

		public static void SetLogInUser(ApplUserTO user)
		{
			logInUser = new ApplUserTO(user.UserID, user.Password, user.Name, user.Description, user.PrivilegeLvl,
				user.Status, user.NumOfTries, user.LangCode);
			logInUser.ExitPermVerification = user.ExitPermVerification;
            logInUser.ExtraHoursAdvancedAmt = user.ExtraHoursAdvancedAmt;

			DAOController.SetLogInUser(logInUser.UserID);
		}

		public static ApplUserTO GetLogInUser()
		{
			return logInUser;
		}

        public static void SetLastLocking()
        {
            ArrayList list = new Lock().Search();
            if (list.Count > 0)
                lastLocking = (Lock)list[0];
            else
                lastLocking = new Lock();
        }

        public static Lock GetLastLocking()
        {
            return lastLocking;
        }
		
		public static void SetLog(ApplUserLogTO userLog)
		{	
			DAOController.SetLog(userLog);
			log = userLog;
		}
		
		public static ApplUserLogTO GetLog()
		{
			return log;
		}

		public static void SetApplicationName(string name)
		{
			applName = name;
			DAOController.SetApplicationName(applName);
			
			try
			{
				// Create Debug directory
				if (Directory.Exists(Constants.logFilePath))
				{
					if (!Directory.Exists(Constants.logFilePath + "\\" + applName))
					{
						Directory.CreateDirectory(Constants.logFilePath + "\\" + applName);
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        public static void SetApplicationName(string name, bool createLogDirectory)
        {
            applName = name;
            DAOController.SetApplicationName(applName);

            if (createLogDirectory)
            {
                try
                {
                    // Create Debug directory
                    if (Directory.Exists(Constants.logFilePath))
                    {
                        if (!Directory.Exists(Constants.logFilePath + "\\" + applName))
                        {
                            Directory.CreateDirectory(Constants.logFilePath + "\\" + applName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

		public static string GetApplicationName()
		{
			return applName + "\\";
		}

		public static void SetCurrentRoles(List<ApplRoleTO> roles)
		{
			currentRoles = roles;
		}

		public static List<ApplRoleTO> GetCurrentRoles()
		{			
			return currentRoles;
		}

		public static void SetCurrentMenuItemID(string menuItemID)
		{
			currentMenuItemID = menuItemID;
		}

		public static string GetCurrentMenuItemID()
		{
			return currentMenuItemID;
		}

		public static void SetMenuItemsPermissions(Hashtable menuItemsPermissionsTable)
		{
			menuItemsPermissions = (Hashtable) menuItemsPermissionsTable.Clone();
		}

		public static Hashtable GetMenuItemsPermissions()
		{
			Hashtable menuItemsPermissionsTable = new Hashtable();
			
			if (menuItemsPermissions != null)
			{
				menuItemsPermissionsTable = (Hashtable) menuItemsPermissions.Clone();
			}

			return menuItemsPermissionsTable;
		}

		public static void MonitorEventHappened(ReaderTO currentReader, int currentAntennaNum, UInt64 currentTagID)
		{
			notifier.MonitorEvent(currentReader, currentAntennaNum, currentTagID);
		}

		public static string GetLanguage()
		{
			
			if ((logInUser == null) || 
				(logInUser.LangCode == null) || 
				(logInUser.LangCode.Trim().Equals("")))
			{
				return Constants.Lang_sr;
			}
			else
			{
				return logInUser.LangCode;
			}
		}

		/// <summary>
		/// Enable object to send and receive 
		/// notifications, putting it's observer instance to notification List
		/// </summary>
		/// <param name="obs">NotificationObserverClient</param>
		/// <seealso>NotificationObserverClient, FaxNotifier</seealso>
		public void AttachToNotifier(NotificationObserverClient obs)
		{
			notifier.Attach(obs);
		}
			
		/// <summary>
		/// Remove reference of an Observer from Notifier array and
		/// disable object from further notifications.
		/// </summary>
		/// <param name="obs">NotificationObserverClient</param>
		/// <seealso>NotificationObserverClient, FaxNotifier</seealso>
		/// <remarks>When object is Disposed, it must be dettach from 
		/// notifications first. 
		/// </remarks>
		public void DettachFromNotifier(NotificationObserverClient obs)
		{
			notifier.Detach(obs);
		}

		public void TestMessage(string message)
		{
			notifier.Message = message;
		}

		public void SettingsChanged(ReaderTO currentReader)
		{
			notifier.CurrentReader = currentReader;
		}

		public void EmpoliyeeChanged(bool isChanged)
		{
			notifier.EmployeeChanged = isChanged;
		}

        public void DBChanged(bool isChanged)
        {
            notifier.dbChanged = isChanged;
        }

		public void PassTypeChanged(bool isChanged)
		{
			notifier.PassTypeHaveChanged = isChanged;
		}

        public void TimeShemaChanged(WorkTimeSchemaTO schema)
		{
			notifier.Schema = schema;
		}

		public void DaySelected(DateTime date)
		{
			notifier.SelectedDay = date;
		}

        public void AuxPortChanged(ArrayList readersAuxPorts)
        {
            notifier.ReadersAuxPorts = readersAuxPorts;
        }

        public void CloseGraphicReportsClick(bool closeGraphicReports)
        {
            notifier.CloseGraphicReports = closeGraphicReports;
        }
        public void IOPairDateChanged(bool showIOPairsDateChanged)
        {
            notifier.ShowIOPairsDateChanged = showIOPairsDateChanged;
        }
        public void setTagID(string tagID)
        {
            notifier.ShowUNIPROMTagIDChanged =tagID;
        }

        public void setStatus(string status)
        {
            notifier.ShowUNIPROMStatusChanged= status;
        }

        public void setTransactionStatus(string transStatus)
        {
            notifier.ShowUNIPROMTransactionStatusChanged = transStatus;
        }
        public void AuxPortChanged(int gateID)
        {
            notifier.GateID = gateID;
        }

        public void RolePermissionCellChecked(int roleID, string position, string cellBoxChecked)
        {
            notifier.RoleID = roleID;
            notifier.Position = position;
            notifier.CellBoxChecked = cellBoxChecked;
        }

		public void LogIn(ApplUserTO currentUser)
		{
			logInUser = currentUser;
			notifier.LogInUser = currentUser;
			
		}

		// Test database connection.
		public bool TestDataBaseConnection()
		{
            bool connExists = false;

            try
            {

                DAOFactory daoFactory = DAOFactory.getDAOFactory();

                if (daoFactory == null)
                {
                    connExists = true;
                }
                else
                {
                    if (!daoFactory.TestDataSourceConnection())
                    {
                        connExists = true;
                    }
                }
            }
            catch
            {
                connExists = true;
            }

			return connExists;
		}

        /// <summary>
        /// Test existence and validity of product licence.
        /// Returns -1 if licence is valid
        /// NO LICENCE (0) if there is no licence
        /// INVALID LICENCE (1) if there is licence but it is not valid
        /// MAXIMUM CONN NUMBER EXCEEDED (2) if user has no permission to connect to database
        /// FUNCTIONALITY NOT VALID (3) if existing functionality does not correspond to licenced
        /// MENU ITEMS NOT CHANGED (4) if existing functionality does not correspond to licenced and could not be set to licenced
        /// </summary>
        public int testLicence()
        {
            Licence licence = new Licence();

            try
            {
                int validity = -1;

                LicenceTO licTO = new LicenceTO();

                // if log in user has ADMIN role, he has permisson to import licence
                if (Common.Misc.isADMINRole(GetLogInUser().UserID))
                {

                    // check if there is licence.txt file in current directory
                    string filePath = Application.StartupPath + "\\licence.txt";

                    if (File.Exists(filePath))
                    {
                        // get licence from licence.txt
                        // licence structure:
                        // no_sessions (number of concurrent sessions)			        6 characters
                        // database_server_port						                    6 characters
                        // database_server_name						                    63 characters
                        // no_sessions_ctrl (same as no_sessions – for later control)	6 characters
                        // 18 moduls                                                    6 characters each
                        // customer                                                     6 characters
                        // check sum                                                    1 character
                        FileStream stream = new FileStream(filePath, FileMode.Open);
                        stream.Close();

                        StreamReader reader = new StreamReader(filePath);

                        string licenceText = reader.ReadLine();
                        reader.Close();

                        // write licence to licence_key field of licence table
                        // if licence_key of maximal rec_id value is 'void' update it else insert new record
                        licence.BeginTransaction();
                        licTO = licence.FindMAX();

                        int i = licenceText.Length;
                        bool isChanged = true;
                        if (licTO.LicenceKey.ToUpper().Equals(Constants.licenceKeyValue))
                        {
                            isChanged = licence.Update(licTO.RecID, licenceText, false) && isChanged;
                        }
                        else
                        {
                            isChanged = (licence.Save(licenceText, false) > 0 ? true : false) && isChanged;
                        }

                        // delete licence.txt
                        if (isChanged)
                        {
                            bool isDeleted = false;
                            try
                            {
                                File.Delete(filePath);
                                isDeleted = true;
                            }
                            catch
                            {
                                isDeleted = false;
                            }

                            if (isDeleted)
                            {
                                licence.CommitTransaction();
                            }
                            else
                            {
                                licence.RollbackTransaction();
                            }
                        }
                        else
                        {
                            licence.RollbackTransaction();
                        }
                    }
                }

                // read licence_key
                licTO = licence.FindMAX();

                licence.LicTO = licTO;

                // check if there is licence
                if (licence.LicTO.LicenceKey.ToUpper().Equals(Constants.licenceKeyValue))
                {
                    // set to minimum visibility
                    ApplMenuItem mItem = new ApplMenuItem();
                    mItem.MenuItemTO.LangCode = NotificationController.GetLanguage().Trim();
                    List<ApplMenuItemTO> menuItems = mItem.Search();

                    // key is position, value is menu item
                    Dictionary<string, ApplMenuItemTO> mItems = new Dictionary<string,ApplMenuItemTO>();
                    foreach (ApplMenuItemTO item in menuItems)
                    {
                        mItems.Add(item.Position, item);
                    }

                    // List of position of minimum visible menu items
                    List<string> minVisibilityPos = Constants.MinVisibilityPos;

                    bool isSet = setMinimumVisibility(mItems, minVisibilityPos);

                    if (isSet)
                    {
                        validity = Constants.noLicence;
                    }
                    else
                    {
                        validity = Constants.menuItemsNotUpdated;
                    }
                }
                else
                {
                    //+++++++++++++++++
                    //debug.writeLog(DateTime.Now + " LICENCE CRYPTED: " + licence.LicTO.LicenceKey.Trim() + "\n");
                    // decrypt licence key
                    byte[] buffer = Convert.FromBase64String(licence.LicTO.LicenceKey);
                    string licenceKeyValue = Util.Misc.decrypt(buffer);
                    //+++++++++++++++++
                    //debug.writeLog(DateTime.Now + " LICENCE DECRYPTED: " + licenceKeyValue.Trim() + "\n");
                    // check licence_key value validity
                    if (licenceKeyValue.Length != Constants.LicenceLength)
                    {
                        validity = Constants.invalidLicence;
                    }
                    else
                    {
                        int numSessions = Int32.Parse(licenceKeyValue.Substring(0, Constants.noSessionsLength).Trim());
                        //+++++++++++++++++
                        //debug.writeLog(DateTime.Now + " LICENCE NUM SESSION: " + numSessions.ToString().Trim() + "\n");
                        string serverPort = licenceKeyValue.Substring(Constants.noSessionsLength, Constants.serverPortLength).Trim();
                        string serverName = licenceKeyValue.Substring(Constants.noSessionsLength + Constants.serverPortLength, Constants.serverNameLength).Trim();
                        int numSessionsCtrl = Int32.Parse(licenceKeyValue.Substring(Constants.noSessionsLength + Constants.serverPortLength + Constants.serverNameLength, Constants.noSessionsLength).Trim());
                        string moduls = licenceKeyValue.Substring(Constants.serverPortLength + Constants.serverNameLength + Constants.noSessionsLength * 2,
                                Constants.modulLength * Constants.modulNum);
                        string customer = licenceKeyValue.Substring(Constants.serverPortLength + Constants.serverNameLength + Constants.noSessionsLength * 2
                            + Constants.modulLength * Constants.modulNum, Constants.customerLength);

                        string licenceKey = licenceKeyValue.Substring(0, licenceKeyValue.Length - 1);
                        byte[] licenceKeyBytes = System.Text.Encoding.ASCII.GetBytes(licenceKey);
                        string licenceKeyCS = licenceKeyValue.Substring(licenceKeyValue.Length - 1);
                        byte[] licenceKeyCSBytes = System.Text.Encoding.ASCII.GetBytes(licenceKeyCS);
                        byte keyCS = (byte)0;

                        if (licenceKeyBytes.Length > 0)
                        {
                            for (int i = 0; i < licenceKeyBytes.Length; i++)
                            {
                                keyCS ^= licenceKeyBytes[i];
                            }
                        }

                        // validate licence
                        if (licenceKeyCSBytes.Length <= 0)
                        {
                            // validete length
                            validity = Constants.invalidLicence;
                        }
                        else if (licenceKeyCSBytes[0].CompareTo(keyCS) != 0)
                        {
                            // validate CRC
                            validity = Constants.invalidLicence;
                        }
                        else if (numSessions != numSessionsCtrl || !serverPort.ToUpper().Equals(getDBServerPort().ToUpper()) || !serverName.ToUpper().Equals(getDBServerName().ToUpper()))
                        {
                            // validate sessions number, server, and port
                            validity = Constants.invalidLicence;
                        }
                        else
                        {
                            List<ApplUserLogTO> usersLog = new ApplUserLog().SearchOpenSessions(NotificationController.GetLogInUser().UserID, System.Net.Dns.GetHostName(), 
                                Constants.UserLoginChanel.DESKTOP.ToString().Trim());

                            foreach (ApplUserLogTO userLog in usersLog)
                            {
                                ApplUserLog log = new ApplUserLog();
                                log.UserLogTO = userLog;
                                log.Update("", Constants.autoUser);
                            }

                            usersLog = new ApplUserLog().SearchOpenSessions("", "", Constants.UserLoginChanel.DESKTOP.ToString().Trim());
                            //+++++++++++++++++
                            debug.writeLog(DateTime.Now + " OPEN SESSIONS NUM: " + usersLog.Count.ToString().Trim() + "\n");
                            if (usersLog.Count >= numSessions)
                            {
                                // validate connections num
                                validity = Constants.maxConnNumExceeeded;
                            }
                            else
                            {
                                // validate functionality
                                ApplMenuItem mItem = new ApplMenuItem();
                                mItem.MenuItemTO.LangCode = NotificationController.GetLanguage();
                                List<ApplMenuItemTO> menuItems = mItem.Search();

                                // list of moduls allowed
                                List<string> modList = new List<string>();

                                if (!moduls.Trim().Equals("") && moduls.Length == Constants.modulNum * Constants.modulLength)
                                {
                                    for (int i = 0; i < Constants.modulNum; i++)
                                    {
                                        modList.Add(moduls.Substring(i * Constants.modulLength, Constants.modulLength).Trim());
                                    }
                                }

                                // key is position, value is menu item
                                /*Hashtable mItems = new Hashtable();
                                foreach (ApplMenuItem item in menuItems)
                                {
                                    mItems.Add(item.Position, item);
                                }*/

                                if ((modList.Contains(((int)Constants.Moduls.PeopleCounterBasic).ToString()))
                                    || (modList.Contains(((int)Constants.Moduls.PeopleCounterStandard).ToString()))
                                    || (modList.Contains(((int)Constants.Moduls.PeopleCounterAdvance).ToString())))
                                {
                                    // key is position, value is menu item
                                    Dictionary<string, ApplMenuItemTO> mItems = new Dictionary<string,ApplMenuItemTO>();
                                    foreach (ApplMenuItemTO item in menuItems)
                                    {
                                        mItems.Add(item.Position, item);
                                    }

                                    // List of position of people counter visible menu items
                                    List<string> peopleCounterVisibilityPos = new List<string>();

                                    if (modList.Contains(((int)Constants.Moduls.PeopleCounterAdvance).ToString()))
                                    {
                                        peopleCounterVisibilityPos = Constants.PeopleCounterAdvanceVisiblePos;
                                    }
                                    else if (modList.Contains(((int)Constants.Moduls.PeopleCounterStandard).ToString()))
                                    {
                                        peopleCounterVisibilityPos = Constants.PeopleCounterStandardVisiblePos;
                                    }
                                    else
                                    {
                                        peopleCounterVisibilityPos = Constants.PeopleCounterBasicVisiblePos;
                                    }

                                    bool isSet = setPeopleCounterVisibility(mItems, peopleCounterVisibilityPos);

                                    if (!isSet)
                                    {
                                        validity = Constants.menuItemsNotUpdated;
                                    }
                                }
                                    //if ACTA is siemens compatibile items visibility specific
                                else if (modList.Contains(((int)Constants.Moduls.SiemensCompatibility).ToString()))
                                {
                                    // key is modul number, value is ArrayList of corresponding menu items positions
                                    Dictionary<int, List<string>> modulsMenuItems = Constants.ModulsMenuItems;

                                    // List of all menu items positions that correspond to not allowed moduls
                                    List<string> itemsPos = new List<string>();

                                    foreach (int key in modulsMenuItems.Keys)
                                    {
                                        if (!modList.Contains(key.ToString().Trim()))
                                        {
                                            foreach (string itemPos in modulsMenuItems[key])
                                            {
                                                itemsPos.Add(itemPos);
                                            }
                                        }
                                    }

                                    bool isValid = compareSiemensLicence(menuItems, itemsPos, customer);

                                    if (!isValid)
                                    {
                                        bool isUpdated = setSiemensVisibility(menuItems, itemsPos, customer);

                                        if (isUpdated)
                                        {
                                            validity = Constants.functionalityNotValid;
                                        }
                                        else
                                        {
                                            validity = Constants.menuItemsNotUpdated;
                                        }
                                    }

                                }
                                else
                                {
                                    // key is modul number, value is ArrayList of corresponding menu items positions
                                    Dictionary<int, List<string>> modulsMenuItems = Constants.ModulsMenuItems;

                                    // ArrayList of all menu items positions that correspond to not allowed moduls
                                    List<string> itemsPos = new List<string>();

                                    foreach (int key in modulsMenuItems.Keys)
                                    {
                                        if (!modList.Contains(key.ToString().Trim()))
                                        {
                                            foreach (string itemPos in modulsMenuItems[key])
                                            {
                                                itemsPos.Add(itemPos);
                                            }
                                        }
                                    }

                                    bool isValid = compareToLicence(menuItems, itemsPos, customer);

                                    if (!isValid)
                                    {
                                        bool isUpdated = setMenuItemsPermitionRole0(menuItems, itemsPos, customer);

                                        if (isUpdated)
                                        {
                                            validity = Constants.functionalityNotValid;
                                        }
                                        else
                                        {
                                            validity = Constants.menuItemsNotUpdated;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return validity;
            }
            catch (Exception ex)
            {
                if (licence.GetTransaction() != null)
                {
                    licence.RollbackTransaction();
                }

                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".testLicence() : " + ex.StackTrace + "\n");
                
                MessageBox.Show(ex.Message);
                return Constants.invalidLicence;
            }
        }       

        public string getDBServerName()
        {
            try
            {
                string serverName = "";

                DAOFactory daoFactory = DAOFactory.getDAOFactory();

                serverName = daoFactory.getDBServerName().Trim();

                return serverName;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".getDBServerName() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        public string getDBName()
        {
            try
            {
                string dbName = "";

                DAOFactory daoFactory = DAOFactory.getDAOFactory();

                dbName = daoFactory.getDBName().Trim();

                return dbName;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".getDBName() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        public string getDBServerPort()
        {
            try
            {
                string serverPort = "";

                DAOFactory daoFactory = DAOFactory.getDAOFactory();

                serverPort = daoFactory.getDBServerPort();

                if (serverPort != null)
                {
                    serverPort = serverPort.Trim();
                }
                else
                {
                    serverPort = "";
                }

                return serverPort;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".getDBServerPort() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        public int getDBConnectedHosts()
        {
            try
            {
                DAOFactory daoFactory = DAOFactory.getDAOFactory();

                int connectedHosts = daoFactory.getDBConnectedHosts();

                return connectedHosts;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".getDBConnectedHosts() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

		public void Downloading(ReaderTO reader, bool isDownloadState)
		{
			notifier.Downloading(reader, isDownloadState);
		}

		/// <summary>
		/// In the moment and XML file start to be processed, this method will be called 
		/// to notify other classes about event and file name.  
		/// </summary>
		/// <param name="filename">current file path</param>
		/// <param name="isProcessingNow">true if file start with processing, false if it ends processing</param>
		public void FileProcessing(string filePath, bool isProcessingNow)
		{
			notifier.FileProcessing(filePath, isProcessingNow);
		}

		public void DataProcessingStateChanged(string state)
		{
			notifier.DataProcessingStateChanged(state);
		}

        public void NotificationStateChanged(string state)
        {
            notifier.NotificationStateChanged(state);
        }

		public void DownloadStarted(bool isDownloadingNow, DateTime nextDownloadingAt)
		{
			notifier.DownloadStarted(isDownloadingNow, nextDownloadingAt);
		}

		public void ReaderAction(ReaderTO currentReader, bool isStarted, bool isSucceeded, 
			bool isPingSucceeded, DateTime nextDownloadingAt)
		{
			notifier.ReaderAction(currentReader,isStarted,isSucceeded, 
			isPingSucceeded, nextDownloadingAt);
		}

		/// <summary>
		/// Send event notification when download or upload data 
		/// has started or stoped.
		/// </summary>
		/// <param name="readerDownload">DownloadLog instance that contain current reader</param>
		/// <param name="isStarted">true if download has started, false if it is stopped</param>
		public void ReaderDataExchange(ReaderTO reader, bool isStarted, DateTime nextReading, string actionName)
		{
			try
			{
				notifier.ReaderDataExchange(reader, isStarted, nextReading, actionName);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ReaderDataExchange() : " + ex.StackTrace + "\n");
			}

		}

		/// <summary>
		/// Send event notification when connection to the reader device has failed 
		/// </summary>
		/// <param name="readerDownload">DownloadLog instance that contain current reader</param>
		/// <param name="isNetFailed">true if ping to the device failed</param>
		/// <param name="isDataTranFailed">true if data transfer for some reason failed</param>
		public void ReaderFailed(ReaderTO reader, bool isNetFailed, bool isDataTranFailed)
		{
			try
			{
				notifier.ReaderFailed(reader, isNetFailed, isDataTranFailed);
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".ReaderFailed() : " + ex.StackTrace + "\n");
			}

		}

        public void PingFailed(ReaderTO reader, bool isNetFailed)
        {
            try
            {
                notifier.PingFailed(reader, isNetFailed);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".ReaderFailed() : " + ex.StackTrace + "\n");
            }

        }

		public void CardOwnerObserved(CardOwner cardOwner)
		{
			notifier.CardOwnerObserved(cardOwner);
		}

        private bool compareToLicence(List<ApplMenuItemTO> menuItems, List<string> itemsPos, string customer)
        {
            try
            {
                bool isValid = true;

                foreach (ApplMenuItemTO item in menuItems)
                {
                    if (!item.Position.StartsWith(Constants.customizedReportsPos))
                    {
                        // all but customized reports
                        if (Constants.TempNonVisibilePos.Contains(item.Position))
                        {
                            if (item.PermitionRole0 != Constants.noPermition)
                            {
                                // has permition over not temporary not visible menu item
                                isValid = false;
                                break;
                            }
                        }
                        else if (itemsPos.Contains(item.Position) && item.PermitionRole0 != Constants.noPermition)
                        {
                            // has permition over not allowed menu item
                            isValid = false;
                            break;
                        }
                        else if (!itemsPos.Contains(item.Position))
                        {
                            bool isLicenced = true;

                            foreach (string pos in itemsPos)
                            {
                                if (item.Position.StartsWith(pos)) 
                                {
                                    isLicenced = false;
                                    if (item.PermitionRole0 != Constants.noPermition)
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                            }

                            if (!isValid)
                            {
                                break;
                            }

                            if (isLicenced && item.PermitionRole0 == Constants.noPermition)
                            {
                                // no permition over allowed menu item
                                isValid = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // customized reports
                        if (customer.Trim().Equals(Constants.noCustomer))
                        {
                            // no customized reports allowed
                            if (item.PermitionRole0 != Constants.noPermition)
                            {
                                // has permition over some customized report menu item
                                isValid = false;
                                break;
                            }
                        }
                        else
                        {
                            // specific customized reports allowed
                            if (item.Position.Equals(Constants.customizedReportsPos))
                            {
                                // main customized reports menu item
                                if (item.PermitionRole0 == Constants.noPermition)
                                {
                                    // no permition over customized reports main menu item
                                    isValid = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (!item.Position.StartsWith(Constants.customizedReportsPos + "_" + customer.Trim())
                                    && item.PermitionRole0 != Constants.noPermition)
                                {
                                    // has permition over not allowed customized report
                                    isValid = false;
                                    break;
                                }
                                else if (item.Position.StartsWith(Constants.customizedReportsPos + "_" + customer.Trim())
                                    && item.PermitionRole0 == Constants.noPermition)
                                {
                                    // no permition over allowed customized report
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                return isValid;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".compareToLicence() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool compareSiemensLicence(List<ApplMenuItemTO> menuItems, List<string> itemsPos, string customer)
        {
            try
            {
                bool isValid = true;

                foreach (ApplMenuItemTO item in menuItems)
                {
                    if (!item.Position.StartsWith(Constants.customizedReportsPos))
                    {
                        // all but customized reports
                        if (Constants.TempNonVisibilePos.Contains(item.Position))
                        {
                            if (item.PermitionRole0 != Constants.noPermition)
                            {
                                // has permition over not temporary not visible menu item
                                isValid = false;
                                break;
                            }
                        }
                        else if (Constants.SiemensUnvisible.Contains(item.Position))
                        {
                            if (item.PermitionRole0 != Constants.noPermition)
                            {
                                // has permition over forbiden items
                                isValid = false;
                                break;
                            }
                        }
                        else if (itemsPos.Contains(item.Position) && item.PermitionRole0 != Constants.noPermition)
                        {
                            // has permition over not allowed menu item
                            isValid = false;
                            break;
                        }
                        else if (!itemsPos.Contains(item.Position))
                        {
                            bool isLicenced = true;

                            foreach (string pos in itemsPos)
                            {
                                if (item.Position.StartsWith(pos))
                                {
                                    isLicenced = false;
                                    if (item.PermitionRole0 != Constants.noPermition)
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                            }

                            if (!isValid)
                            {
                                break;
                            }

                            if (isLicenced && item.PermitionRole0 == Constants.noPermition)
                            {
                                // no permition over allowed menu item
                                isValid = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // customized reports
                        if (customer.Trim().Equals(Constants.noCustomer))
                        {
                            // no customized reports allowed
                            if (item.PermitionRole0 != Constants.noPermition)
                            {
                                // has permition over some customized report menu item
                                isValid = false;
                                break;
                            }
                        }
                        else
                        {
                            // specific customized reports allowed
                            if (item.Position.Equals(Constants.customizedReportsPos))
                            {
                                // main customized reports menu item
                                if (item.PermitionRole0 == Constants.noPermition)
                                {
                                    // no permition over customized reports main menu item
                                    isValid = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (!item.Position.StartsWith(Constants.customizedReportsPos + "_" + customer.Trim())
                                    && item.PermitionRole0 != Constants.noPermition)
                                {
                                    // has permition over not allowed customized report
                                    isValid = false;
                                    break;
                                }
                                else if (item.Position.StartsWith(Constants.customizedReportsPos + "_" + customer.Trim())
                                    && item.PermitionRole0 == Constants.noPermition)
                                {
                                    // no permition over allowed customized report
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                return isValid;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".compareToLicence() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool setSiemensVisibility(List<ApplMenuItemTO> menuItems, List<string> itemsPos, string customer)
        {
            try
            {
                bool isUpdated = true;

                foreach (ApplMenuItemTO item in menuItems)
                {
                    ApplMenuItemTO mItemTO = item;

                    if (Constants.SiemensUnvisible.Contains(item.Position))
                    {
                        mItemTO.PermitionRole0 = Constants.noPermition;
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                        if (isUpdated)
                            continue;
                    }

                    if (Constants.TempNonVisibilePos.Contains(item.Position))
                    {
                        // set to no permition over temporary not visible menu item
                        mItemTO.PermitionRole0 = Constants.noPermition;
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }
                    else
                    {
                        // set to has permition over current menu item
                        mItemTO.PermitionRole0 = Constants.allPermition;
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }

                    if (!isUpdated)
                        break;

                    // if it is not customized reports
                    if (!item.Position.StartsWith(Constants.customizedReportsPos))
                    {
                        // set to no permition over not allowed menu items
                        if (itemsPos.Contains(item.Position))
                        {
                            mItemTO = item;
                            mItemTO.PermitionRole0 = Constants.noPermition;
                            isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                            if (!isUpdated)
                                break;
                        }
                        else
                        {
                            foreach (string pos in itemsPos)
                            {
                                if (item.Position.StartsWith(pos))
                                {
                                    mItemTO = item;
                                    mItemTO.PermitionRole0 = Constants.noPermition;
                                    isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                                    if (!isUpdated)
                                        break;
                                }
                            }

                            if (!isUpdated)
                                break;
                        }
                    }
                    else
                    {
                        // customized reports
                        if (customer.Trim().Equals(Constants.noCustomer))
                        {
                            // no customized reports allowed
                            mItemTO = item;
                            mItemTO.PermitionRole0 = Constants.noPermition;
                            isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                            if (!isUpdated)
                                break;
                        }
                        else
                        {
                            // other customized reports are not allowed
                            if (!item.Position.Equals(Constants.customizedReportsPos)
                                && !item.Position.ToString().StartsWith(Constants.customizedReportsPos + "_" + customer.Trim()))
                            {
                                mItemTO = item;
                                mItemTO.PermitionRole0 = Constants.noPermition;
                                isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                                if (!isUpdated)
                                    break;
                            }
                        }
                    }
                }

                return isUpdated;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".setMenuItemsPermitionRole0() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool setMenuItemsPermitionRole0(List<ApplMenuItemTO> menuItems, List<string> itemsPos, string customer)
        {
            try
            {
                bool isUpdated = true;

                foreach (ApplMenuItemTO item in menuItems)
                {
                    ApplMenuItemTO mItemTO = item;
                    
                    if (Constants.TempNonVisibilePos.Contains(item.Position))
                    {
                        // set to no permition over temporary not visible menu item
                        mItemTO.PermitionRole0 = Constants.noPermition;
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }
                    else
                    {
                        // set to has permition over current menu item
                        mItemTO.PermitionRole0 = Constants.allPermition;
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }

                    if (!isUpdated)
                        break;

                    // if it is not customized reports
                    if (!item.Position.StartsWith(Constants.customizedReportsPos))
                    {
                        // set to no permition over not allowed menu items
                        if (itemsPos.Contains(item.Position))
                        {
                            mItemTO = item;
                            mItemTO.PermitionRole0 = Constants.noPermition;
                            isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                            if (!isUpdated)
                                break;
                        }
                        else
                        {
                            foreach (string pos in itemsPos)
                            {
                                if (item.Position.StartsWith(pos))
                                {
                                    mItemTO = item;
                                    mItemTO.PermitionRole0 = Constants.noPermition;
                                    isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                                    if (!isUpdated)
                                        break;
                                }
                            }

                            if (!isUpdated)
                                break;
                        }
                    }
                    else
                    {
                        // customized reports
                        if (customer.Trim().Equals(Constants.noCustomer))
                        {
                            // no customized reports allowed
                            mItemTO = item;
                            mItemTO.PermitionRole0 = Constants.noPermition;
                            isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                            if (!isUpdated)
                                break;
                        }
                        else
                        {
                            // other customized reports are not allowed
                            if (!item.Position.Equals(Constants.customizedReportsPos) 
                                && !item.Position.ToString().StartsWith(Constants.customizedReportsPos + "_" + customer.Trim()))
                            {
                                mItemTO = item;
                                mItemTO.PermitionRole0 = Constants.noPermition;
                                isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;

                                if (!isUpdated)
                                    break;
                            }
                        }
                    }
                }

                return isUpdated;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".setMenuItemsPermitionRole0() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool setMinimumVisibility(Dictionary<string, ApplMenuItemTO> mItems, List<string> minVisibilityPos)
        {
            try
            {
                bool isUpdated = true;

                foreach (string key in mItems.Keys)
                {
                    ApplMenuItemTO mItemTO = mItems[key];
                    
                    if (minVisibilityPos.Contains(key))
                    {
                        // if is minimum visibility menu item, set all permition roles to 15
                        //ApplMenuItem mItem = new ApplMenuItem();
                        //mItem.ReceiveTransferObject(mItemTO);

                        int[] permitions = new int[Constants.rolesNum];
                        for (int i = 0; i < Constants.rolesNum; i++)
                        {
                            permitions[i] = Constants.allPermition;
                        }

                        mItemTO.ArrayToPermissions(permitions);
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }
                    else
                    {
                        // if is not minimum visibility menu item, set all permition roles to 0
                        mItemTO.PermitionRole0 = Constants.noPermition;
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }
                    
                    if (!isUpdated)
                        break;
                }

                return isUpdated;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".setMinimumVisibility() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }

        private bool setPeopleCounterVisibility(Dictionary<string, ApplMenuItemTO> mItems, List<string> peopleCounterVisibilityPos)
        {
            try
            {
                bool isUpdated = true;

                foreach (string key in mItems.Keys)
                {
                    ApplMenuItemTO mItemTO = mItems[key];

                    if (peopleCounterVisibilityPos.Contains(key))
                    {
                        // if is people counter visibility menu item, set all permition roles to 15
                        //ApplMenuItem mItem = new ApplMenuItem();
                        //mItem.ReceiveTransferObject(mItemTO);

                        int[] permitions = new int[Constants.rolesNum];
                        for (int i = 0; i < Constants.rolesNum; i++)
                        {
                            permitions[i] = Constants.allPermition;
                        }

                        mItemTO.ArrayToPermissions(permitions);
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }
                    else
                    {
                        // if is not people counter visibility menu item, set all permition roles to 0
                        mItemTO.PermitionRole0 = Constants.noPermition;
                        isUpdated = new ApplMenuItem().UpdateSamePosition(mItemTO) && isUpdated;
                    }

                    if (!isUpdated)
                        break;
                }

                return isUpdated;
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + ".setMinimumVisibility() : " + ex.StackTrace + "\n");
                throw ex;
            }
        }       
    }
}
