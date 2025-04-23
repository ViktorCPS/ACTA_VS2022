using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface WorkingUnitXOrganizationalUnitDAO
    {
        int insert(WorkingUnitXOrganizationalUnitTO wuXouTO);

        int insert(WorkingUnitXOrganizationalUnitTO wuXouTO, bool doCommit);

        bool delete(int orgUnitID, int wuID);

        bool delete(int orgUnitID, int wuID, bool doCommit);

        List<WorkingUnitXOrganizationalUnitTO> getWUXOU(WorkingUnitXOrganizationalUnitTO auXwuTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
