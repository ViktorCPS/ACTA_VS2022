using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public  interface MealsEmployeeScheduleDAO
    {
        int insert(int mealTypeID, DateTime date, int employeeID, string shift);

        bool update(int mealTypeID, DateTime date, int employeeID, string shift);

        ArrayList getMealsEmployeeSchedule(int mealTypeID, DateTime date, int employeeID);

        ArrayList getMealsEmployeeSchedule(DateTime from, DateTime to, int WUID);

        bool delete(int mealTypeID, DateTime date,int employeeID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        int insert(int mealTypeID, DateTime date, int employeeID, string shift, bool doCommit);

        bool delete(int mealTypeID, DateTime date, int employeeID, bool doCommit);

        ArrayList getNumberOfScheduledMeals(DateTime from, DateTime to, string wuString);

        ArrayList getMealsEmployeeScheduleForEmpl(DateTime from, DateTime to, int employeeID);

        ArrayList getScheduleForDateEmplMeal(DateTime from, DateTime to, string WUID, int emplID, int mealTypeID);

        Dictionary<DateTime, List<EmployeeTO>> getDateEndEmployeesWhoOrdered(DateTime from, DateTime to, string WUID);
    }
}
