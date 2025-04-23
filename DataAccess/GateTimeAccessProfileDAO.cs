using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for GateTimeAccessProfileDAO.
	/// </summary>
	public interface GateTimeAccessProfileDAO
	{
		int insert(string name, string description, string gateTAProfile0, string gateTAProfile1,
			string gateTAProfile2, string gateTAProfile3, string gateTAProfile4, string gateTAProfile5,
			string gateTAProfile6, string gateTAProfile7, string gateTAProfile8, string gateTAProfile9,
			string gateTAProfile10, string gateTAProfile11, string gateTAProfile12,
			string gateTAProfile13, string gateTAProfile14, string gateTAProfile15);

		bool update(string gateTimeAccessProfileId, string name, string description, 
			string gateTAProfile0, string gateTAProfile1, string gateTAProfile2, 
			string gateTAProfile3, string gateTAProfile4, string gateTAProfile5,
			string gateTAProfile6, string gateTAProfile7, string gateTAProfile8, 
			string gateTAProfile9, string gateTAProfile10, string gateTAProfile11, 
			string gateTAProfile12, string gateTAProfile13, string gateTAProfile14, 
			string gateTAProfile15, bool doCommit);

		bool delete(string gateTimeAccessProfileId);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);

		GateTimeAccessProfileTO find(string gateTimeAccessProfileId);

		ArrayList getGateTimeAccessProfile(string name);

		void serialize(ArrayList gateTimeAccessProfileTOList);
	}
}
