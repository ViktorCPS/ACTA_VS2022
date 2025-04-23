using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for CameraSnapshotFileDAO.
    /// </summary>
    public interface CameraSnapshotFileDAO
    {
        int insert(string fileName, int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime, byte[] content, bool doCommit);

        /*bool update(int readerID, string type, string oldStatus, string newStatus,
            DateTime uploadStartTime, DateTime uploadEndTime, int delay, int recordID,
            string modifiedBy, bool doCommit);*/

        /*ExtraHourTO find(int employeeID, DateTime dateEarned);*/

        ArrayList getCameraSnapshotFiles(int cameraID,
            DateTime cameraCreatedTime, DateTime fileCreatedTime);

        int getCameraSnapshotFilesCount(int cameraID);

        ArrayList getCamSnapshotFilesForDates(DateTime DateFrom,DateTime DateTo);

        bool DeleteUntilDate(DateTime fileCreatedTime, bool doCommit);

        ArrayList getCSFilesForPass(int passID, DateTime fromDate, DateTime toDate, string direction);

        ArrayList getCSFilesForPassDisplay(string recordID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void serialize(ArrayList cameraSnapshotFileTOList);

        ArrayList getCameraSnapshotFiles(string cameraID,  DateTime dateFrom, DateTime dateTo, DateTime timeFrom, DateTime timeTo);
        int insert(CameraSnapshotFileTO cameraFileTO, bool doCommit);
    }
}
