using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface SystemClosingEventDAO
    {
        int insert(SystemClosingEventTO eventTO, bool doCommit);

        bool update(SystemClosingEventTO eventTO, bool doCommit);

        bool delete(int eventID, bool doCommit);

        List<SystemClosingEventTO> getClosingEvents(DateTime from, DateTime to);

        List<string> getClosingEventsMessages(string type);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
