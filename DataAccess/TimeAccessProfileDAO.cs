using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for TimeAccessProfileDAO.
	/// </summary>
	public interface TimeAccessProfileDAO
	{
		int insert(string name, string description, bool doCommit);

		bool update(string timeAccessProfileId, string name, string description, bool doCommit);

		bool delete(string timeAccessProfileId, bool doCommit);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);

		TimeAccessProfileTO find(string timeAccessProfileId);

		ArrayList getTimeAccessProfile(string name);

		void serialize(ArrayList TimeAccessProfileTOList);
	}
}
