using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface MealTypeDAO
    {
        int insert(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo);

        int insert(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo, bool doCommit);

        ArrayList getMealsType(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo);

        int getMealsTypeCount(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo);

        bool update(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo);

        bool update(int mealTypeID, string name, string description, DateTime hoursFrom, DateTime hoursTo, bool doCommit);

        bool delete(int mealTypeID);

        bool delete(int mealTypeID, bool doCommit);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        MealTypeTO getMealType(int mealTypeID);
        int getMaxID();
    }
}
