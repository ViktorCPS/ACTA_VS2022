using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface VisitorDocFileDAO
    {
        int insert(int visitID, int docType, byte[] content, bool doCommit);

        VisitorDocFileTO findVisitorDocFileByJMBG(string JMBG);

        VisitorDocFileTO findVisitorDocFileByID(string ID);

        VisitorDocFileTO findVisitorDocFileByVisitID(string visitID);

        ArrayList getVisitorDocFilesForDates(DateTime DateFrom, DateTime DateTo);

        bool deleteUntilDate(DateTime createdTime, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
