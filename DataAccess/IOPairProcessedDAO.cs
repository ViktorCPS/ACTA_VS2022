using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface IOPairProcessedDAO
    {
        uint insert(IOPairProcessedTO pair,bool doCommit);

        bool update(IOPairProcessedTO pair, bool doCommit);

        //natalija08112017
        bool updateManualCreatedProcessedPairs(IOPairProcessedTO processed, Dictionary<int, WorkTimeIntervalTO> workTimeInterval, Dictionary<int, WorkTimeIntervalTO> workTimeIntervalNextDay, bool is2DayShift, IDbTransaction trans);
        
        
        bool verify(string recIDs, string verifiedBy, string ptIDs);

        bool verify(string emplIDs, string verifiedBy, DateTime month, string passTypes, bool validateIsVerifiedDay);

        bool delete(uint recID);

        bool delete(string recIDs, bool doCommit);

        bool delete(int emplID, DateTime date, bool doCommit);

        IOPairProcessedTO find(uint recID);

        List<IOPairProcessedTO> search(IOPairProcessedTO pair);
        
        List<IOPairProcessedTO> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList, string ptIDs);

        List<IOPairProcessedTO> getIOPairsAllForEmpl(string employeeIDString, List<DateTime> datesList, string ptIDs, DateTime lastDate);

        List<IOPairProcessedTO> getIOPairsAllForEmpl(string employeeIDString, DateTime from, DateTime to, string ptIDs);

        List<IOPairProcessedTO> getIOPairsToVerifyForEmpl(string employeeIDString, List<DateTime> datesList, string ptIDs);

        int getPaidLeaveDays(DateTime from, DateTime to, DateTime date, int employeeID, int ptID, int limitCompositeID, int limitOccassionalyID, int limitElemetaryID);

        int getPaidLeaveDaysOutsidePeriod(DateTime from, DateTime to, DateTime periodStart, DateTime periodEnd, int employeeID, int ptID, int limitCompositeID, int limitOccassionalyID, int limitElemetaryID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        bool delete(Dictionary<int, List<DateTime>> emplDateList);

        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getManualCreatedPairs(Dictionary<int, List<DateTime>> emplDateList);

        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsForInterval(IOPairProcessedTO iOPairProcessedTO, DateTime startTime, DateTime endTime);

        //natalija 23.01.2018
        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsForIntervalForWU(DateTime startTime, DateTime endTime, string passTypeString, string wuIDs, bool isRetired);


        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsToVerifyEmplDaySet(string recIDs);

        void getDatesForEmplWithNoPairs(DateTime startIntervalTime, DateTime endIntervalTime, Dictionary<int, List<DateTime>> dict);

        bool DeleteUjustified(Dictionary<int, List<DateTime>> emplDict, bool doCommit);

        bool getManualCreatedPairsDayAfter(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated);

        bool getManualCreatedPairsDayBefore(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated);

        bool delete(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore);

        List<IOPairProcessedTO> getProcessedPairs(Dictionary<int, List<DateTime>> emplDateList);

        List<IOPairProcessedTO> getProcessedPairs(Dictionary<int, List<DateTime>> emplDateList, IDbTransaction trans);

        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getManualCreatedPairs(Dictionary<int, List<DateTime>> emplDateList, IDbTransaction trans);

        //natalija08112017
        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getManualCreatedPairsWholeDayAbsence(Dictionary<int, List<DateTime>> emplDateList, IDbTransaction trans);

        bool getManualCreatedPairsDayBefore(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated, IDbTransaction trans);

        bool getManualCreatedPairsDayAfter(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated, IDbTransaction trans);

        List<IOPairProcessedTO> getWeekPairs(int emplID, DateTime date, bool includeDate, string ptIDs, IDbTransaction trans);

        bool delete(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore, bool doCommit);

        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> getPairsForInterval(IOPairProcessedTO iOPairProcessedTO, DateTime startTime, DateTime endTime, string employeeIDs);

        DateTime getMaxDateOfPair(string employeeID, IDbTransaction trans);

        int getCollectiveAnnualLeaves(int emplID, int ptID, DateTime fromDate, List<DateTime> exceptDates);

        int getEarnedHours(int emplID, int ptID, DateTime fromDate, DateTime exceptDate);

        int getUsedHours(int emplID, int ptID, int ptRounding, DateTime fromDate, DateTime exceptDate);

        List<IOPairProcessedTO> getProcessedPairsTypesForMonthlyReports(DateTime from, DateTime to); //17.08.2017 MM

        bool DeleteDuplicates(int month); // VIKTOR 23012024
        List<IOPairProcessedTO> getProcessedPairsByPassType(DateTime from, DateTime to, string tipProlaska);
        //  24.05.2019  BOJAN
        List<IOPairProcessedTO> getIOPairsWithManualCreatedByEmployee(string employeeIDString, DateTime from, DateTime to);
        int BankHoursMonthly(int emplID,DateTime month);
        int BankHoursPeriodical(int emplID, DateTime fromDate, DateTime toDate);
        int BankHours6Months(int emplID, DateTime month);
        int radneSubote(int emplID, DateTime month);
        void BankHours6MonthsPay(int emplID, DateTime mesec);
        List<IOPairProcessedTO> pairsForPeriod(string emplIDs, DateTime from, DateTime to);
        List<IOPairProcessedTO> pairsForPeriod(string emplIDs, DateTime from, DateTime to, string passType);
        void deleteIoPairProcForAutoTimeSchema(int emplID, DateTime date, bool doCommit);
    }
}
