using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for TagDAO.
	/// </summary>
	public interface TagDAO
	{
		int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO);

		bool delete(int recordID);

		bool update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, bool doCommit);

		TagTO find(int recordID);

		List<TagTO> getTags(TagTO tag);

		Dictionary<ulong, TagTO> getActiveTags();

		Dictionary<ulong, TagTO> getActiveTagsWithAccessGroup();

		List<TagTO> getInactiveTags(string wUnits, DateTime from, DateTime to);

		TagTO findActive(int ownerID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

		void serialize(List<TagTO> TagsTO);

		void serialize(List<TagTO> TagTOList, String filename);

        void serialize(List<TagTO> TagTOList, Stream stream);

		List<TagTO> deserialize(string filePath);

        List<TagTO> deserialize(Stream stream);

        void SetDBConnection(Object dbConnection);

        int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy);

        int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy, bool doCommit);

        bool update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string user, bool doCommit);

        int searchTagsCount(int emplID, string status, string wUnits, DateTime from, DateTime to, string tagID);

        List<TagTO> searchTags(int emplID, string status, string wuString, DateTime from, DateTime to, string tagID);
    }
}
