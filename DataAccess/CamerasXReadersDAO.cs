using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for CamerasXReadersDAO.
    /// </summary>
    public interface CamerasXReadersDAO
    {
        int insert(int cameraID, int readerID, string directionCovered, bool doCommit);

        bool update(int cameraID, int readerID, string directionCovered, bool doCommit);

        bool delete(int cameraID, int readerID, bool doCommit);

        /*ExtraHourTO find(int employeeID, DateTime dateEarned);*/

        ArrayList getCamerasXReaders(int cameraID, int readerID, string directionCovered);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void serialize(ArrayList camerasXReadersTOList);
    }
}
