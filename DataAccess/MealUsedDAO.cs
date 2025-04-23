using System;
using System.Collections;
using System.Data;

using TransferObjects;
namespace DataAccess
{
    public interface MealUsedDAO
    {
        int insert(string transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount);

        ArrayList getMealsUsed(int transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount);

        ArrayList getMealsUsed(int employeeId, DateTime from, DateTime to, int pointId, int mealTypeId, int qtyFrom, int qtyTo, int moneyAmtFrom, int moneyAmtTo, string wUnits);

        ArrayList getMealsUsed(int employeeID, DateTime from, DateTime to);

        ArrayList getNumerOfUsedMeals(DateTime from, DateTime to, string wuString);

        int getMealsUsedCount(int transId, int employeeId, DateTime eventTime, int pointId, int mealTypeId, int quantity, int moneyAmount);

        bool delete(string mealTypeID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        int getTransID();
    }
}
