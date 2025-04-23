using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{/// <summary>
    /// Summary description for PassDAO.
    /// </summary>
    public interface PassHistDAO
    {
        int insert(int passID, int employeeID, string direction, DateTime eventTime, int passTypeID, int isWrkHrsCount, int locationID,
            int pairGenUsed, int manualCreated, string remarks, string createdBy, DateTime createdTime, bool doCommit);

        List<PassHistTO> getPassHistInterval(PassHistTO phTO, DateTime fromTime, DateTime toTime, bool eventTimeChecked, string wUnits, DateTime fromModifeidTime, DateTime toModifiedTime, bool modifiedTimeChecked);

        int getPassesIntervalCount(PassHistTO phTO, DateTime fromTime, DateTime toTime, bool eventTimeChecked, string wUnits, DateTime fromModifeidTime, DateTime toModifiedTime, bool modifiedTimeChecked);
        
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        List<PassHistTO> find(int passID);
    }
}
