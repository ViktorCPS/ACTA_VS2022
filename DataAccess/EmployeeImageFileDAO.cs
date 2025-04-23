using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for EmployeeImageFileDAO.
    /// </summary>
    public interface EmployeeImageFileDAO
    {
        int insert(int employeeID, byte[] picture, bool doCommit);

        bool update(int employeeID, byte[] picture, bool doCommit);

        bool delete(int employeeID, bool doCommit);

        bool deleteAll(string employeeID, bool doCommit);

        /*ExtraHourTO find(int employeeID, DateTime dateEarned);*/

        ArrayList getEmployeeImageFiles(int employeeID);

        int getEmployeeImageFilesCount(int employeeID);

        ArrayList getEmployeeImageInfo(string[] statuses);

        ArrayList getEmployeeImageForSnapshots(string employeeID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void serialize(ArrayList employeeImageFileTOList);

        void SetDBConnection(Object dbConnection);
    }
}
