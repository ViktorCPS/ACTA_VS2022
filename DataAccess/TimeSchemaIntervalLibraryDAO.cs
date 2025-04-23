using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface TimeSchemaIntervalLibraryDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        Dictionary<int, List<TimeSchemaIntervalLibraryTO>> getTimeSchemaIntervalLibraryDictionary(TimeSchemaIntervalLibraryTO timeSchemaTO);
    }
}
