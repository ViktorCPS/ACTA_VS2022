using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface OnlineMealsUsedDailyDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        int getOnlineMealUsedCount(int employeeID, DateTime from, DateTime to);

        int insert(OnlineMealsUsedTO onlineMealUsedTO, bool doCommit, int transfer_flag);

        List<OnlineMealsUsedTO> getMealsToTransfer();

        bool updateToTransfered(string transIDs, bool doCommit);

        bool deletePreviousDay(bool doCommit);

        int findByStatus(int online_validation, DateTime date, int pointID);
    }
}
