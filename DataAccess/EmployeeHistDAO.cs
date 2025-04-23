using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
  public  interface EmployeeHistDAO
    {
        int insert(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID, string type, string accessGroupID, string EmployeeTypeID, string orgUnitID, string created_by, DateTime created_time,DateTime validTO, bool doCommit);

        int insert(EmployeeHistTO emplHistTO, bool doCommit);

        EmployeeHistTO getDateEmployee(DateTime date, EmployeeTO emplTO);

        DateTime getRetiredDate(int emplID);

        Dictionary<int, List<EmployeeHistTO>> getEmployeeChanges(DateTime from, DateTime to, string emplIDs);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
