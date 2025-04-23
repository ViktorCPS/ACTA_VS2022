using System;
using System.Collections;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// AddressDAO interface is implemented by 
	/// database specific PassTypes DAO classes
	/// </summary>
	public interface AddressDAO
	{
		int insert(string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3);

		bool delete(int addressID);

		bool update(int addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3);

		AddressTO find(int addressID);

		ArrayList getAddresses(string addressId, string name, string addressLine1, string addressLine2, string addressLine3,
			string cityName, string country, string state, string postalZipCode, string telNumber1,
			string telNumber2, string telNumber3, string faxNumber1, string faxNumber2, string faxNumber3);
	}
}
