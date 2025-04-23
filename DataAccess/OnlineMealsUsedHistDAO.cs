using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
   public interface OnlineMealsUsedHistDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);


        int insert(OnlineMealsUsedHistTO onlineMealUsedTO, bool doCommit);


        List<OnlineMealsUsedHistTO> getMealsUsed(string employeeIDs, DateTime from, DateTime to, string restaurantIDs, string pointsIDs, string mealTypeIDs, int qtyFrom, int qtyTO, int manual_created, int online_validation, int auto_check, string reasonsReader, string reasonsAutoCheck, string status, string operater, DateTime approvedFrom, DateTime approvedTo, DateTime modifiedFrom, DateTime modifiedTo);

        List<OnlineMealsUsedHistTO> getMealsUsed(string transactionID);

    }
}
