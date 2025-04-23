using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for EmployeeAbsenceDAO.
	/// </summary>
	public interface EmployeeAbsenceDAO
	{
		int insert(int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, DateTime vacationYear,string description);

		bool delete(int recID);

		bool deleteEAdeleteIOP(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, DAOFactory factory);

		bool update(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used);

		bool update(int recID, int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, bool doCommit);

		EmployeeAbsenceTO find(int recID);

		bool updateEAdeleteIOP(int recID, int employeeID, int passTypeIDOld, int passTypeID, DateTime dateStartOld,
            DateTime dateEndOld, DateTime dateStart, DateTime dateEnd, int used, DAOFactory factory, IDbTransaction trans, string description);
       
        bool updateEAdeleteIOP(int recID, int employeeID, int passTypeIDOld, int passTypeID, DateTime dateStartOld,
            DateTime dateEndOld, DateTime dateStart, DateTime dateEnd, int used, DAOFactory factory, IDbTransaction tran);

		List<EmployeeAbsenceTO> getEmployeeAbsences(EmployeeAbsenceTO eaTO, string wUnits);
        List<EmployeeAbsenceTO> getEmployeeAbsences(EmployeeAbsenceTO eaTO);


        List<EmployeeAbsenceTO> getEmployeeAbsences(EmployeeAbsenceTO eaTO, string wUnits, IDbTransaction trans);

		//Added this function because Search algorithm is changed for Absences form, 
		//and original Search function is used in IOPair, for insertWholeDayAbsences
		List<EmployeeAbsenceTO> getEmployeeAbsencesForAbsences(EmployeeAbsenceTO eaTO, string wUnits);

		int getExistingEmployeeAbsences(EmployeeAbsenceTO eaTO, string wUnits);

        List<EmployeeAbsenceTO> getEmployeeAbsencesForVacEvid(EmployeeAbsenceTO eaTO, string employees, string wUnits, DateTime relatedYearFrom, DateTime relatedYearTo);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        int insert(int employeeID, int passTypeID, DateTime dateStart, DateTime dateEnd, int used, DateTime vacationYear, bool doCommit);
    }
}
