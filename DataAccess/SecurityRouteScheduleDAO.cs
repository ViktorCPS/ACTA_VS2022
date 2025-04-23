using System;
using System.Collections;
using System.Text;
using System.Data;

namespace DataAccess
{
    public interface SecurityRouteScheduleDAO
    {
        int insert(int emplID, int routeID, DateTime date, bool doCommit);

        ArrayList getRoutesSch(int emplID, int routeID, DateTime from, DateTime to);

        bool delete(int emplID, int routeID, DateTime date);

        bool delete(int emplID, DateTime date, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
