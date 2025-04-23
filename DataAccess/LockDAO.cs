using System;
using System.Collections;
using System.Text;
using System.Data;

namespace DataAccess
{
    public interface LockDAO
    {
       int insert(DateTime lockDate, string type, string comment, bool doCommit);
       ArrayList search();
       bool beginTransaction();

       void commitTransaction();

       void rollbackTransaction();

       void setTransaction(IDbTransaction trans);

       IDbTransaction getTransaction();
    }
}
