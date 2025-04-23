using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for CameraDAO.
    /// </summary>
    public interface CameraDAO
    {
        int insert(int cameraID, string connAddress, string description,
            string type, bool doCommit);

        bool update(int cameraID, string connAddress, string description,
            string type, bool doCommit);

        bool delete(int cameraID, bool doCommit);

        /*ExtraHourTO find(int employeeID, DateTime dateEarned);*/

        ArrayList getCameras(int cameraID, string connAddress, string description,
            string type);

        int getCameraNextID();

        ArrayList getCamerasOnGate(int gateID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        void serialize(ArrayList cameraTOList);

        ArrayList getCamerasForMap(int mapID);

        ArrayList getCamerasForReaders(string readerID, string direction);
    }
}
