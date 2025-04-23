using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using TransferObjects;

namespace DataAccess
{
    public interface OnlineMealsUsedDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        List<OnlineMealsUsedTO> getMealsUsed(string employeeIDs, DateTime from, DateTime to);

        List<OnlineMealsUsedTO> getMealsUsed(int employeeID, DateTime from, DateTime to);

        List<OnlineMealsUsedTO> getMealsUsed(string IDs);

        int getOnlineMealUsedCount(int employeeID, DateTime from, DateTime to);

        Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> getMealsUsedDict(string emplIDs, DateTime from, DateTime to);

        OnlineMealsUsedTO find(string transactionID);

        int insert(OnlineMealsUsedTO onlineMealUsedTO, bool doCommit);

        List<OnlineMealsUsedTO> getMealsUsed(int employeeId, DateTime from, DateTime to, int pointId, int mealTypeId, int qtyFrom, int qtyTo, string wUnits);

        List<OnlineMealsUsedTO> getMealsUsed(string employeeIDs, DateTime from, DateTime to, string restaurantIDs, string pointsIDs, string mealTypeIDs, int qtyFrom, int qtyTO, int manual_created, int online_validation, int auto_check, string reasonsReader, string reasonsAutoCheck, string status, string operater, DateTime approvedFrom, DateTime approvedTo);

        bool update(OnlineMealsUsedTO onlineMealsUsedTO, bool doCommit);

        Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> getMealsForValidation(DateTime from, DateTime to, string emplIDs);

        Dictionary<int, Dictionary<DateTime, List<OnlineMealsUsedTO>>> getMealsValidated(DateTime from, DateTime to, DateTime fromValidation, DateTime toValidation, string emplIDs);

        int findByStatus(int online_validation, DateTime date, int pointID);
    }
}
