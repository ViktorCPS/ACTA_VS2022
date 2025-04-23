using System;
using System.Collections;
using System.Data;
using System.IO;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for TimeAccessProfileDtlDAO.
	/// </summary>
	public interface TimeAccessProfileDtlDAO
	{
		int insert(string timeAccessProfileId, string dayOfWeek, string direction, string hrs0,
			string hrs1, string hrs2, string hrs3, string hrs4, string hrs5, string hrs6, string hrs7, string hrs8, string hrs9, 
			string hrs10, string hrs11, string hrs12, string hrs13, string hrs14, string hrs15, string hrs16, string hrs17, string hrs18, 
			string hrs19, string hrs20, string hrs21, string hrs22, string hrs23, bool doCommit);

		bool update(string timeAccessProfileId, string dayOfWeek, string direction, string hrs0,
			string hrs1, string hrs2, string hrs3, string hrs4, string hrs5, string hrs6, string hrs7, string hrs8, string hrs9, 
			string hrs10, string hrs11, string hrs12, string hrs13, string hrs14, string hrs15, string hrs16, string hrs17, string hrs18, 
			string hrs19, string hrs20, string hrs21, string hrs22, string hrs23, bool doCommit);

		bool delete(string timeAccessProfileId, string dayOfWeek, string direction, bool doCommit);

		bool beginTransaction();

		void commitTransaction();

		void rollbackTransaction();

		IDbTransaction getTransaction();

		void setTransaction(IDbTransaction trans);

		TimeAccessProfileDtlTO find(string timeAccessProfileId, string dayOfWeek, string direction);

		ArrayList getTimeAccessProfileDtl(string timeAccessProfileId);

		void serialize(ArrayList TimeAccessProfileDtlTOList);

		void serialize(ArrayList TimeAccessProfileDtlTOList, String filename);

        void serialize(ArrayList TimeAccessProfileDtlTOList, Stream stream);

		ArrayList deserialize(string filePath);

        ArrayList deserialize(Stream stream);
	}
}
