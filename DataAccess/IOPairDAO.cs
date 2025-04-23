using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for IOPairsDAO
    /// </summary>
    public interface IOPairDAO
    {
        int insert(DateTime IOPairDate, int employeeID, int locationID, int isWrkHrsCounter, int passTypeID,
            DateTime startTime, DateTime endTime, int gateID, int manualyCreated, bool doCommit);

        int insert(IOPairTO pairTO, bool doCommit);

        int insertWholeDayAbsence(IOPairTO pairTO, bool doCommit);

        int insert(IOPairTO ioPairTO, PassTO firstPass, PassTO secondPass, DAOFactory factory);

        int insertWholeDayAbsence(DateTime IOPairDate, int employeeID, int locationID, int isWrkHrsCounter, int passTypeID,
            DateTime startTime, DateTime endTime, int manualyCreated);

        int insertExtraHourPair(DateTime IOPairDate, int employeeID, int locationID, int isWrkHrsCounter,
            int passTypeID, DateTime startTime, DateTime endTime, int manualyCreated, bool doCommit);

        bool delete(int IOPairID);

        bool delete(int IOPairID, bool doCommit);

        //bool delete(int employeeID, string date, bool doCommit);

        bool deletePairPasses(List<IOPairTO> iopairs, List<PassTO> passes, DAOFactory factory);

        bool delete(string IOPairID);

        bool delete(string IOPairID, bool doCommit);

        bool update(int IOPairID, DateTime IOPairDate, int employeeID, int locationID, int isWrkHrsCounter,
            int passTypeID, DateTime startTime, DateTime endTime, int manualyCreated, string createdBy);

        bool updateIOPairsPasses(int employeeID, string date, DAOFactory factory);

        IOPairTO find(int IOPairID);

        // Used in IOPairs maintenance
        List<IOPairTO> getIOPairs(IOPairTO pairTO);

        List<IOPairTO> getIOPairs(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int wuID);

        List<IOPairTO> getIOPairs(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int wuID, IDbTransaction trans);

        int getPairsCount(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int wuID);

        List<IOPairTO> getIOPairsForWU(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate);

        int getIOPairsForWUCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate);

        List<IOPairTO> getIOPairsNotInWUDate(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate);

        int getIOPairsNotInWUCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate);

        List<IOPairTO> getIOPairsForWUDate(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate, int locID, int isWrkHrs);

        List<IOPairTO> getIOPairsForWUWrkHrs(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate);

        int getIOPairsForWUWrkHrsCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate);

        List<IOPairTO> getIOPairsForExtraHours(DateTime fromDate, DateTime toDate, List<int> employeesList);

        List<IOPairTO> getIOPairsAll(DateTime fromDate, DateTime toDate, List<int> employeesList);

        int getIOPairsForEmplDateCount(DateTime fromDate, DateTime toDate, List<int> employeesList);

        List<IOPairTO> getIOPairsForEmplDate(DateTime fromDate, DateTime toDate, List<int> employeesList);

        List<IOPairTO> getIOPairsForEmplDateWithOpenPairs(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID, int isWrkHrs);

        List<IOPairTO> getIOPairsAllEmplDatePairs(string employeeIDString, List<DateTime> datesList);

        List<IOPairTO> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList, int locationID, int isWrkHrs);

        List<IOPairTO> getIOPairsEmplTimeInterval(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans);

        List<IOPairTO> getIOPairsForLoc(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate);

        int getIOPairsForLocCount(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate);

        DataSet getIOPairsForLocDataSet(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate);

        List<IOPairTO> getIOPairsForEmpl(DateTime fromDate, DateTime toDate, List<int> eployeesList, int locationID);

        List<IOPairTO> getIOPairsForEmplLoc(DateTime fromDate, DateTime toDate, List<int> eployeesList, string locationsID);

        int getIOPairsForEmplCount(DateTime fromDate, DateTime toDate, List<int> eployeesList, int locationID);

        int getIOPairsForEmplLocCount(DateTime fromDate, DateTime toDate, List<int> eployeesList, string locationsID);

        List<IOPairTO> getIOPairsForEmplWrkHrs(DateTime fromDate, DateTime toDate, List<int> eployeesList, int locationID);

        int getIOPairsForEmplWrkHrsCount(DateTime fromDate, DateTime toDate, List<int> eployeesList, int locationID);

        List<IOPairTO> getIOPairsForPresence(int wuID, string wUnitsString, DateTime form, DateTime to);

        List<IOPairTO> getIOPairsForOUPresence(int ouID, string oUnitsString, DateTime form, DateTime to);

        List<IOPairTO> getIOPairsForVisit(int visitID);

        List<EmployeeTO> getDistinctEmployees(DateTime from, DateTime to, int workingUnitID, string wUnits);

        List<EmployeeTO> getDistinctOUEmployees(DateTime from, DateTime to, int organizationalUnitID, string oUnits);

        int getDistinctEmployeesCount(DateTime from, DateTime to, int workingUnitID, string wUnits);

        ArrayList getEmpoloyeesByDate(DateTime fromDate);

        ArrayList getEmpoloyeesOpenPairsToday();

        List<IOPairTO> getOpenPairs(int employeeID, DateTime date);

        List<DateTime> getDatesWithOpenPairs(DateTime fromDate, DateTime toDate, int employeeID);

        List<DateTime> getDatesWithOpenPairsWrkHrs(DateTime fromDate, DateTime toDate, int employeeID);

        List<IOPairTO> getSpecialOutOpenPairs();

        List<IOPairTO> getPermissionPassPairs();

        List<IOPairTO> getPairsWithSpecialOut(DateTime start, DateTime end, DateTime date);

        bool existEmlpoyeeDatePair(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime);

        bool existEmlpoyeeDatePairNotWholeDayAbsences(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void serialize(List<IOPairTO> ioPairTO);

        DataSet getOpenPairs();

        List<IOPairTO> getOpenPairsFromDataSet(int employeeID, DateTime date, DataSet dsOpenPairs);

        ArrayList getEmployeesByDateAutoClose();

        List<IOPairTO> getIOPairsForEmplPerm(DateTime fromDate, DateTime toDate, List<int> eployeesList);

        int getIOPairsForEmplPermCount(DateTime fromDate, DateTime toDate, List<int> eployeesList);

        int getIOPairsForDaysArrayCount(List<DateTime> days, List<int> employeesList);

        List<IOPairTO> getIOPairsForDaysArray(List<DateTime> days, List<int> employeesList);

        int getIOPairsEmployeeDateCount(DateTime fromDate, DateTime toDate, List<int> eployeesList);

        List<IOPairTO> getIOPairsEmployeeDate(DateTime fromDate, DateTime toDate, List<int> employeesList);

        List<IOPairTO> getIOPairsForEmployees(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID);

        bool existEmlpoyeeDatePair(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime, int skipIOPairID, int isWrkHrs);

        DataSet getClosedPairs();

        List<IOPairTO> getPairsFromDataSet(int employeeID, DateTime date, DataSet dsOpenPairs);

        int getIOPairsCount(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int wuID);

        List<IOPairTO> getIOPairsWithType(IOPairTO pairTO, DateTime fromDate, DateTime toDate, string wUnits, int workingUnitID);

        List<IOPairTO> getNonEnteredIOPairs(int employeeID, DateTime startTime, DateTime endTime);

        List<IOPairTO> getWholeDayAbsenceIOPairs(List<int> emplIDs, List<int> passType, int year);

        bool update(IOPairTO iOPairTO, bool doCommit);

        List<IOPairTO> getIOPairsClosed(IOPairTO iOPairTO);

        DateTime getFirstArrivedTime(int emplID, DateTime date);

        bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList);

        List<IOPairTO> getIOPairsClosed(IOPairTO iOPairTO, DateTime fromTime, DateTime toTime);

        bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDict, bool doCommit);

        bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore);

        bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore, bool p);

        void getEmplOpenPairs(DateTime fromDate, Dictionary<int, Dictionary<DateTime, IOPairTO>> emplStartOpenPairs,
            Dictionary<int, Dictionary<DateTime, IOPairTO>> emplEndOpenPairs, ref string emplIDs, List<DateTime> dateList);

        bool updateToUnprocessedWorkCount(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore);



        List<IOPairTO> getIOPairsEmplTimeInterval2(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans);

        //  22.05.2019. BOJAN
        List<IOPairTO> getOpenPairsForReport(string employeeID, DateTime from, DateTime to, string orgUnitID, string workingUnitID);

        List<IOPairTO> getIOPairsForBreaks(DateTime from, DateTime to, string emplIDs, string ptIDs);   //  23.01.2020. BOJAN

        void deleteForDay(int emplID, DateTime date);

        void deleteForAutoTimeSchema(int emplID, DateTime date, bool doCommit);

    }
}
