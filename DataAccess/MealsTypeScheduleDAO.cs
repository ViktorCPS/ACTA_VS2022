using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace DataAccess
{
   public interface MealsTypeScheduleDAO
    {
        int insert(int mealTypeID, DateTime  date, DateTime hoursFrom, DateTime hoursTo);

       ArrayList getMealsTypeSchedule(int mealTypeID, DateTime dateFrom, DateTime dateTo);

       bool update(int mealTypeID, DateTime date, DateTime hoursFrom, DateTime hoursTo);

        bool delete(int mealTypeID, DateTime date);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        int insert(int MealTypeID, DateTime date, DateTime hoursFrom, DateTime hoursTo, bool doCommit);

        bool delete(int mealTypeId, DateTime date, bool doCommit);
    }
}
