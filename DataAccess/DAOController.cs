using System;
using System.IO;
using Util;
using TransferObjects;


namespace DataAccess
{
	/// <summary>
	/// Summary description for DAOController.
	/// </summary>
	public class DAOController
	{
		public static DAOController instance;
		private static string logInUser = "";
		private static string applicationName = "";
		private static ApplUserLogTO log = null;
		DebugLog debug;

		protected DAOController()
		{
			// Debug
			string logFilePath = Constants.logFilePath + GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);
			log = new ApplUserLogTO();
		}

		/// <summary>
		/// Create instance of the class. 
		/// There must be only one instance of DAOController
		/// class.
		/// </summary>
		/// <returns>DAOController</returns>
		public static DAOController GetInstance()
		{
			if (instance == null)
			{
				instance = new DAOController();
			}
			return instance;
		}

		public static void SetLogInUser(string userID)
		{
			logInUser = userID;
		}

		public static string GetLogInUser()
		{
			return logInUser;
		}
		
		public static void SetLog(ApplUserLogTO userLog)
		{
			log = userLog;
		}
		
		
		public static ApplUserLogTO GetLog()
		{
			return log;
		}
		
		public static void SetApplicationName(string name)
		{
			applicationName = name;
		}
		
		public static string GetApplicationName()
		{
			return applicationName + "\\";
		}
	}
}
