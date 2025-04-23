using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;
using System.Data;

namespace DataAccess
{
	/// <summary>
	/// Summary description for VisitDAO.
	/// </summary>
	public interface VisitDAO
	{
		int insert(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks);

		int insert(VisitTO visitTO);

		//int insert(PassTO passTO, bool doCommit);
         
		bool delete(string visitID);

		bool update(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks);

		bool update(VisitTO visitTO);
        bool updateVisit(VisitTO visitTO);
        ArrayList getVisitsUNIPROM(int visitID, int employeeID, string firstName,
            string lastName, string visitorJMBG, string visitorID, DateTime dateStart,
            DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr,
            int locationID, string remarks);

		int insert(VisitTO visitTo, bool doCommit);
		
		VisitTO find(string visitID);

        VisitTO findMAXVisitPIN(string PIN);

        VisitTO findMAXVisitIdCard(string idCard);

		ArrayList getVisits(int visitID, int employeeID, string firstName, string lastName, string visitorJMBG, string visitorID, 
			DateTime dateStart, DateTime dateEnd, int visitedPerson, int visitedWorkingUnit, string visitDescr, int locationID, string remarks);

        ArrayList getVisitsDateInterval(string employeeID, string visitedWorkingUnit, string visitedPerson, string visitorIdent,
            DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy,string visitAsco4, Dictionary<int, List<VisitAsco4TO>> ascoTO);

        ArrayList getVisitsDateInterval(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo,
            string visitDescr, string wUnits);

        int getVisitsDateIntervalCount(string employeeID, string visitedWorkingUnit,
            string visitedPerson, string visitorIdent, DateTime dateFrom, DateTime dateTo,
            string visitDescr, string wUnits);

        ArrayList getCurrentVisits(string wUnits);

        ArrayList getCurrentVisitsDetail(string wUnits);

        ArrayList getAllVisits(string wuString);

		bool serialize(ArrayList visitTO);
		bool serialize(ArrayList visitTOList, string filePath);
		
		ArrayList deserialize(string filePath);

        List<string> getDistinctVisitType();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        ArrayList getVisitsCompleted();

        ArrayList getVisitsNotCompleted(bool notCompleted);

        List<string> getDistinctName();

        List<string> getDistinctCompany();

        DataSet getVisitsCompletedDS();

        ArrayList getVisitsDateIntervalDistinct(string employeeID, string visitedWorkingUnit, string visitorID, string visitorIdent, DateTime dateFrom, DateTime dateTo, string visitDescr, string wUnits, string state, string company, string visitor, string privacy, string visitAsco4, Dictionary<int, List<VisitAsco4TO>> ascoTO);

        bool update(string FirstName, string lastName, string jmbg);
    }
}
