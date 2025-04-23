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
	/// Address
	/// </summary>
	public class Address
	{
		private int _address_id = -1;
		private string _name = "";
		private string _address_line_1 = "";
		private string _address_line_2 = "";
		private string _address_line_3 = "";
		private string _city_name = "";
		private string _country = "";
		private string _state = "";
		private string _postal_zip_code = "";
		private string _tel_number_1 = "";
		private string _tel_number_2 = "";
		private string _tel_number_3 = "";
		private string _fax_number_1 = "";
		private string _fax_number_2 = "";
		private string _fax_number_3 = "";
		
		DAOFactory daoFactory = null;
		AddressDAO adao = null;

		DebugLog log;

		public int AddressID
		{
			get{ return _address_id; }
			set{ _address_id = value; }
		}

		public string Name
		{
			get{ return _name; }
			set{ _name = value; }
		}

		public string AddressLine1
		{
			get{ return _address_line_1; }
			set{ _address_line_1 = value; }
		}

		public string AddressLine2
		{
			get{ return _address_line_2; }
			set{ _address_line_2 = value; }
		}

		public string AddressLine3
		{
			get{ return _address_line_3; }
			set{ _address_line_3 = value; }
		}

		public string CityName
		{
			get{ return _city_name; }
			set{ _city_name = value; }
		}

		public string Country
		{
			get{ return _country; }
			set{ _country = value; }
		}

		public string State
		{
			get{ return _state; }
			set{ _state = value; }
		}

		public string PostalZipCode
		{
			get{ return _postal_zip_code; }
			set{ _postal_zip_code = value; }
		}

		public string TelNumber1
		{
			get{ return _tel_number_1; }
			set{ _tel_number_1 = value; }
		}

		public string TelNumber2
		{
			get{ return _tel_number_2; }
			set{ _tel_number_2 = value; }
		}

		public string TelNumber3
		{
			get{ return _tel_number_3; }
			set{ _tel_number_3 = value; }
		}

		public string FaxNumber1
		{
			get{ return _fax_number_1; }
			set{ _fax_number_1 = value; }
		}

		public string FaxNumber2
		{
			get{ return _fax_number_2; }
			set{ _fax_number_2 = value; }
		}

		public string FaxNumber3
		{
			get{ return _fax_number_3; }
			set{ _fax_number_3 = value; }
		}


		public Address()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			adao = daoFactory.getAddressDAO(null);
		}

        public Address(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            adao = daoFactory.getAddressDAO(dbConnection);
        }

		public Address(int addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			adao = daoFactory.getAddressDAO(null);

			this.AddressID = addressId;
			this.Name = name;
			this.AddressLine1 = addressLine1;
			this.AddressLine2 = addressLine2;
			this.AddressLine3 = addressLine3;
			this.CityName = cityName;
			this.Country = country;
			this.State = state;
			this.PostalZipCode = postalZipCode;
			this.TelNumber1 = telNumber1;
			this.TelNumber2 = telNumber2;
			this.TelNumber3 = telNumber3;
			this.FaxNumber1 = faxNumber1;
			this.FaxNumber2 = faxNumber2;
			this.FaxNumber3 = faxNumber3;
		}

		public int Save(string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
			int addressID = 0;
			try
			{
				// TODO: cerate TO and send to DAO
				addressID = adao.insert(name, addressLine1, addressLine2, addressLine3,
					cityName, country, state, postalZipCode, telNumber1,
					telNumber2, telNumber3, faxNumber1, faxNumber2, faxNumber3);

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Address.Save(): " + ex.Message + "\n");
				//throw new Exception(ex.Message);
				throw ex;
			}

			return addressID;

		}

		public bool Update(int addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
			bool isUpdated = false;

			try
			{
				// TODO: cerate TO and send to DAO
				isUpdated = adao.update(addressId, name, addressLine1, addressLine2, addressLine3,
					cityName, country, state, postalZipCode, telNumber1,
					telNumber2, telNumber3, faxNumber1, faxNumber2, faxNumber3);

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Address.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public ArrayList Search(string addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
			ArrayList addressTOList = new ArrayList();
			ArrayList addressList = new ArrayList();

			try
			{
				Address addrMember = new Address();
				addressTOList = adao.getAddresses(addressId, name, addressLine1, addressLine2, addressLine3,
					cityName, country, state, postalZipCode, telNumber1,
					telNumber2, telNumber3, faxNumber1, faxNumber2, faxNumber3);

				foreach(AddressTO addrTO in addressTOList)
				{
					addrMember = new Address();
					addrMember.receiveTransferObject(addrTO);

					addressList.Add(addrMember);
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Address.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			return addressList;

		}

		public bool Delete(int addressId)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = adao.delete(addressId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Address.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public AddressTO Find(int addressId)
		{
			AddressTO addressTO = new AddressTO();

			try
			{
				addressTO = adao.find(addressId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Address.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return addressTO;
		}

		public void Clear()
		{
			this.AddressID = -1;
			this.Name = "";
			this.AddressLine1 = "";
			this.AddressLine2 = "";
			this.AddressLine3 = "";
			this.CityName = "";
			this.Country = "";
			this.State = "";
			this.PostalZipCode = "";
			this.TelNumber1 = "";
			this.TelNumber2 = "";
			this.TelNumber3 = "";
			this.FaxNumber1 = "";
			this.FaxNumber2 = "";
			this.FaxNumber3 = "";
		}

		public void receiveTransferObject(AddressTO addressTO)
		{
			this.AddressID = addressTO.AddressID;
			this.Name = addressTO.Name;
			this.AddressLine1 = addressTO.AddressLine1;
			this.AddressLine2 = addressTO.AddressLine2;
			this.AddressLine3 = addressTO.AddressLine3;
			this.CityName = addressTO.CityName;
			this.Country = addressTO.Country;
			this.State = addressTO.State;
			this.PostalZipCode = addressTO.PostalZipCode;
			this.TelNumber1 = addressTO.TelNumber1;
			this.TelNumber2 = addressTO.TelNumber2;
			this.TelNumber3 = addressTO.TelNumber3;
			this.FaxNumber1 = addressTO.FaxNumber1;
			this.FaxNumber2 = addressTO.FaxNumber2;
			this.FaxNumber3 = addressTO.FaxNumber3;
		}

		public AddressTO sendTransferObject()
		{
			AddressTO addressTO = new AddressTO();

			addressTO.AddressID = this.AddressID;
			addressTO.Name = this.Name;
			addressTO.AddressLine1 = this.AddressLine1;
			addressTO.AddressLine2 = this.AddressLine2;
			addressTO.AddressLine3 = this.AddressLine3;
			addressTO.CityName = this.CityName;
			addressTO.Country = this.Country;
			addressTO.State = this.State;
			addressTO.PostalZipCode = this.PostalZipCode;
			addressTO.TelNumber1 = this.TelNumber1;
			addressTO.TelNumber2 = this.TelNumber2;
			addressTO.TelNumber3 = this.TelNumber3;
			addressTO.FaxNumber1 = this.FaxNumber1;
			addressTO.FaxNumber2 = this.FaxNumber2;
			addressTO.FaxNumber3 = this.FaxNumber3;
			return addressTO;
		}
	}
}
