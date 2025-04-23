using System;

namespace TransferObjects
{
	/// <summary>
	/// This is used by the AddressDAO to send and receive 
	/// data from the Address class.
	/// </summary>
	public class AddressTO
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

		public AddressTO()
		{
		
		}

		public AddressTO(int addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3)
		{
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
	}
}
