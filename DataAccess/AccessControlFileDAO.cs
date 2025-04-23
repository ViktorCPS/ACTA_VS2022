using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for AccessControlFileDAO.
    /// </summary>
    public interface AccessControlFileDAO
    {
        int insert(string type, int readerID, int delayed, string status, DateTime uploadStartTime,
            DateTime uploadEndTime, byte[] content, bool doCommit);

        bool deleteOld(int readerID, string type, bool doCommit);

        bool update(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit);

        bool updateOthers(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit);

        /*ExtraHourTO find(int employeeID, DateTime dateEarned);*/

        ArrayList getAccessControlFiles(string type, int reader_id, int delayed, string status,
            DateTime uploadStartTime, DateTime uploadEndTime);

        AccessControlFileTO getAccessControlFilesMax(string type, int readerID);

        int getAccessControlFilesCount(string type, string status);

        ArrayList getLastIssuedACFiles();

        ArrayList getLastUploadTime();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void serialize(ArrayList accessControlFileTOList);

        void CloseDBConnection();
    }
}
