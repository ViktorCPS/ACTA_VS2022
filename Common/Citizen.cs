using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Configuration;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Summary description for Citizen.
	/// </summary>
	public class Citizen
	{
		private string _jmbg;
		private string _firstName;
		private string _lastName;

		DAOFactory daoFactory = null;
		CitizenDAO cdao = null;

		DebugLog debug;

		public string JMBG
		{
			get{ return _jmbg; }
			set{ _jmbg = value; }
		}

		public string FirstName
		{
			get{ return _firstName; }
			set{ _firstName = value; }
		}

		public string LastName
		{
			get{ return _lastName; }
			set{ _lastName = value; }
		}

		public Citizen()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			cdao = daoFactory.getCitizenDAO(null);

			this.JMBG = "";
			this.FirstName = "";
			this.LastName = "";
			
		}
        public Citizen(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            debug = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            cdao = daoFactory.getCitizenDAO(dbConnection);

            this.JMBG = "";
            this.FirstName = "";
            this.LastName = "";

        }
		public Citizen(string jmbg, string firstName, string lastName)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			cdao = daoFactory.getCitizenDAO(null);

			this.JMBG = jmbg;
			this.FirstName = firstName;
			this.LastName = lastName;		
		}

		public ArrayList Search(string jmbg, string firstName, string lastName)
		{
			ArrayList citizenTOList = new ArrayList();
			ArrayList citizenList = new ArrayList();

			try
			{
				Citizen citizenMember = new Citizen();
				citizenTOList = cdao.getCitizens(jmbg, firstName, lastName);

				if (citizenTOList != null )
				{
					foreach(CitizenTO citizenTO in citizenTOList)
					{
						citizenMember = new Citizen();
						citizenMember.receiveTransferObject(citizenTO);

						citizenList.Add(citizenMember);
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Citizen.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return citizenList;
		}

		public void receiveTransferObject(CitizenTO citizenTo)
		{
			this.JMBG = citizenTo.JMBG;	
			this.FirstName = citizenTo.FirstName;
			this.LastName = citizenTo.LastName;					
		}

		public CitizenTO sendTransferObject()
		{
			CitizenTO citizenTo = new CitizenTO();
			
			citizenTo.FirstName = this.FirstName;
			citizenTo.LastName = this.LastName;
			citizenTo.JMBG = this.JMBG;

			return citizenTo;
		}

		public void Clear()
		{
			this.JMBG = "";
			this.FirstName = "";
			this.LastName = "";
		}
	}
}
