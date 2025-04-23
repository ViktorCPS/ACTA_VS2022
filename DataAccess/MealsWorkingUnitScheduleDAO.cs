using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace DataAccess
{
   public interface MealsWorkingUnitScheduleDAO
    {
        int insert(int mealTypeID, DateTime date, int WUID);

        ArrayList getMealsWUSchedule(int mealTypeID, DateTime date, int WUID);

        bool delete(int mealTypeID, DateTime date, int WUID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

       ArrayList getMealsWUSchedule(int mealTypeID, DateTime datefrom, DateTime dateTo, int workingUnitID);

       int insert(int MealTypeID, DateTime date, int WUID, bool doCommit);

       bool delete(int mealTypeId, DateTime date, int workingUnitID, bool doCommit);
   }
}
