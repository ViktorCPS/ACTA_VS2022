using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface MealAssignedDAO
    {
        bool insert(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo, int quantity, int quantityDaily, int moneyAmount);

        ArrayList getMealsAssigned(string employeeID, string mealTypeID, DateTime validFrom, DateTime validTo);

        ArrayList getMealsAssigned(string employeeID);

        int getMealsAssignedCount(string employeeID, string mealTypeID, DateTime validFrom, DateTime validTo);

        ArrayList getMealsAssigned(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo,
            int qtyFrom, int qtyTo, int qtyDailyFrom, int qtyDailyTo, int mAmtFrom, int mAmtTo, bool unlimited);

        int getMealsAssignedCount(int employeeID, int mealTypeID, DateTime validFrom, DateTime validTo,
            int qtyFrom, int qtyTo, int qtyDailyFrom, int qtyDailyTo, int mAmtFrom, int mAmtTo, bool unlimited);

        bool delete(string employeeID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
