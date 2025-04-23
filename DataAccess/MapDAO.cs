using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface MapDAO
    {
        bool insert(int mapID, string name, string description, byte[] content);

        ArrayList getMaps(int mapID, int parentMapID, string name, string description);

        TransferObjects.MapTO find(int parentMapID);

        bool delete(int MapID,bool doCommit);

        bool update(int mapID, int parentID, string name, string description, byte[] content);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        int findMAXMapID();
    }
}
