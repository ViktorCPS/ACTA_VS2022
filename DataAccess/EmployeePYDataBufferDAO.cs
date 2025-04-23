using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface EmployeePYDataBufferDAO
    {
        int insert(EmployeePYDataBufferTO emplPYDataAnalitical, bool doCommit);

        int insertExpat(EmployeePYDataBufferTO emplPYDataAnalitical, bool doCommit);

        List<EmployeePYDataBufferTO> getEmployeeBuffers(uint calcID);

        uint getMaxCalcID();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}


