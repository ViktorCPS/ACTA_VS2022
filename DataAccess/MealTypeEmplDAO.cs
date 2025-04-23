using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface MealTypeEmplDAO
    {
        int insert(string name, string description);

        ArrayList getMealsTypeEmpl(int mealTypeEmplID, string name, string description);

        int getMealsTypeEmplCount(int mealTypeEmplID, string name, string description);

        bool delete(string mealsTypeEmplID);

        bool update(int mealTypeEmplID, string name, string description);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
