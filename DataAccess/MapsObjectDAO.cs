using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
   public interface MapsObjectDAO
    {
       int insert(TransferObjects.MapsObjectHdrTO mapsObjectHdrTO);

       ArrayList getObjects(int mapID);

       int getObjectsCount(int mapID);

       ArrayList getPointsDetails(int objectID, string type);

       bool delete(int removeObjectID, object removeObjectTtpe);

       bool deleteOnMap(int mapID, bool doCommit);

       bool beginTransaction();

       void commitTransaction();

       void rollbackTransaction();

       IDbTransaction getTransaction();

       void setTransaction(IDbTransaction trans);
    }
}
