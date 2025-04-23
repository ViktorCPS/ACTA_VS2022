using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// Summary description for GateSyncDAO.
    /// </summary>
    public interface GateSyncDAO
    {
        bool Insert(string gateID, string readerControlRequest, string monitoring);

        bool Update(string gateID, string readerControlRequest, string monitoring);

        bool Delete(string gateID);

        ArrayList Search(string gateID);

        int Count(string gateID);

        void SetDBConnection(Object dbConnection);
    }
}
