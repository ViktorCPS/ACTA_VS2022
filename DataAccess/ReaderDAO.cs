using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// ReaderDAO interface is implemented by 
	/// database specific PassTypes DAO classes
	/// </summary>
	public interface ReaderDAO
	{
		int insert(ReaderTO readerData);

		bool delete(int readerID);

		bool update(ReaderTO readerData);

		ReaderTO find(string readerID);

		int findMAXReaderID();

		List<ReaderTO> getReaders(ReaderTO readerData);

        List<ReaderTO> getReaders(string[] gateArray);

        List<ReaderTO> getAllReaders();

        List<ReaderTO> getReadersOnAntenna0();

        List<ReaderTO> getReadersLastReadTime();

		DateTime getAllReadersLastReadTime();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void serialize(List<ReaderTO> ReadersTO);

        List<ReaderTO> getReaders(int locID, int gateID);

        List<ReaderTO> getReadersForMap(int mapID);

        bool update(ReaderTO readerTO, bool doCommit);

        int insert(ReaderTO readerTO, bool doCommit);

        DateTime getLastLogUsed(int readerID, string direction);

        Dictionary<int, ReaderTO> getReadersDictionary(ReaderTO readerTO);

        List<ReaderTO> searchForIDs(string readerID);
    }
}
