using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
   public interface IOPairsProcessedHistDAO
    {
        uint insert(IOPairsProcessedHistTO pair, bool doCommit);

        int insert(int emplID, string modifiedBy, DateTime modifiedTime, DateTime date, int alertStatus, bool doCommit);

        int insert(string recIDs, string modifiedBy, DateTime modifiedTime, int alertStatus, bool doCommit);

        IOPairsProcessedHistTO find(uint recID);

        Dictionary<int, Dictionary<DateTime, Dictionary<string, List<IOPairsProcessedHistTO>>>> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList);

        List<IOPairsProcessedHistTO> search(IOPairsProcessedHistTO pair);

        Dictionary<DateTime, List<IOPairsProcessedHistTO>> getIOPairsSet(int emplID, DateTime date);

        bool isAlert(int emplID, DateTime date);

        Dictionary<int, Dictionary<DateTime, int>> getAlerts(string emplIDs, DateTime from, DateTime to);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
