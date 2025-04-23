using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for AccessGroupXGateDAO.
	/// </summary>
	public interface AccessGroupXGateDAO
	{
		int insert(string accessGroupID, string gateID, string gateTimeAccessProfile, string readerAccessGroupOrdNum, bool doCommit);

		bool update(string accessGroupID, string gateID, string gateTimeAccessProfile, string readerAccessGroupOrdNum, bool doCommit);

		bool delete(string accessGroupID, string gateID, bool doCommit);

		bool deleteGates(string gateID, bool doCommit);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);

		AccessGroupXGateTO find(string accessGroupID, string gateID);

		ArrayList getAccessGroupXGate(string accessGroupID, string gateID);

		void serialize(ArrayList accessGroupXGateTOList);
	}
}
