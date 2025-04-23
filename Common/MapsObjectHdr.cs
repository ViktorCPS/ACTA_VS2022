using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using DataAccess;
using TransferObjects;
using Util;


namespace Common
{
   public class MapsObjectHdr
    {
        private int _objectID;
        private string _type;
        private int _mapID;

        public Hashtable Points;

        private DAOFactory daoFactory;
        private MapsObjectDAO mapsObjectDAO;
        DebugLog log;

        public int MapID
        {
            get { return _mapID; }
            set { _mapID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        
        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        public MapsObjectHdr()
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mapsObjectDAO = daoFactory.getMapsObjectDAO(null);

            // Init properties
            MapID = -1;
            Type = "";
            ObjectID = -1;
            Points = new Hashtable();
        }

        public MapsObjectHdr(object dbConnection)
        {
            // Debug
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mapsObjectDAO = daoFactory.getMapsObjectDAO(dbConnection);

            // Init properties
            MapID = -1;
            Type = "";
            ObjectID = -1;
            Points = new Hashtable();
        }
        public void ReceiveTransferObject(MapsObjectHdrTO mapsObjectHdrTO)
        {
            try
            {
                this.ObjectID = mapsObjectHdrTO.ObjectID;
                this.Type = mapsObjectHdrTO.Type;
                this.MapID = mapsObjectHdrTO.MapID;

                Hashtable pointsTO = new Hashtable();
                pointsTO = mapsObjectHdrTO.Points;

                MapsObjectDtl mapsObjectDtl = new MapsObjectDtl();

                foreach (int key in pointsTO.Keys)
                {
                    mapsObjectDtl = new MapsObjectDtl();
                    mapsObjectDtl.ReceiveTransferObject((MapsObjectDtlTO)pointsTO[key]);

                    this.Points.Add(key, mapsObjectDtl);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapsObjectHdr.ReceiveTransferObject(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public MapsObjectHdrTO SendTransferObject()
        {
            MapsObjectHdrTO mapsObjectTO = new MapsObjectHdrTO();

            try
            {
                mapsObjectTO.ObjectID = this.ObjectID;
                mapsObjectTO.Type = this.Type;
                mapsObjectTO.MapID = this.MapID;

                Hashtable points = new Hashtable();
                points = (Hashtable)this.Points;

                MapsObjectDtlTO mapsObjectDtlTO = new MapsObjectDtlTO();

                foreach (int key in points.Keys)
                {
                    mapsObjectDtlTO = ((MapsObjectDtl)points[key]).SendTransferObject();

                    mapsObjectTO.Points.Add(key, mapsObjectDtlTO);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MapsObjectHdr.SendTransferObject(): " + ex.Message + "\n");
                throw ex;
            }

            return mapsObjectTO;
        }


       public int Save()
       {
           int affectedRows;
           try
           {
               affectedRows = this.mapsObjectDAO.insert(this.SendTransferObject());
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " SecurityRouteHdr.Save(): " + ex.Message + "\n");
               throw ex;
           }

           return affectedRows;
       }

       public ArrayList Search(int mapID)
       {
           // List that contins TO object
           ArrayList objectsTO = new ArrayList();
           ArrayList objects = new ArrayList();

           try
           {
               objectsTO = mapsObjectDAO.getObjects(mapID);
               MapsObjectHdr member;

               foreach (MapsObjectHdrTO objectTO in objectsTO)
               {
                   member = new MapsObjectHdr();
                   member.ReceiveTransferObject(objectTO);

                   objects.Add(member);
               }

           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MapsObjectHdr.Search(): " + ex.Message + "\n");
               throw ex;
           }

           return objects;
       }

       public int SearchCount(int mapID)
       {
           int count = 0;

           try
           {
               count = mapsObjectDAO.getObjectsCount(mapID);
              
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MapsObjectHdr.Search(): " + ex.Message + "\n");
               throw ex;
           }

           return count;
       }

       public ArrayList SearchDetails(int objectID,string type)
       {
           // List that contins TO object
           ArrayList objectsTO = new ArrayList();
           ArrayList objects = new ArrayList();

           try
           {
               objectsTO = mapsObjectDAO.getPointsDetails(objectID,type);
               MapsObjectDtl member;

               foreach (MapsObjectDtlTO objestPointTO in objectsTO)
               {
                   member = new MapsObjectDtl();
                   member.ReceiveTransferObject(objestPointTO);

                   objects.Add(member);
               }

           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MapsObjectHdr.SearchDetailsTerminal(): " + ex.Message + "\n");
               throw ex;
           }

           return objects;
       }

       public bool Remove(int removeObjectID, string removeObjectTtpe)
       {
           bool isDeleted = false;

           try
           {
               isDeleted = this.mapsObjectDAO.delete(removeObjectID, removeObjectTtpe);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MapsObjectHdr.Remove(): " + ex.Message + "\n");
               throw ex;
           }

           return isDeleted;
       }
       public bool Remove(int mapID, bool doCommit)
       {
           bool isDeleted = false;

           try
           {
               isDeleted = this.mapsObjectDAO.deleteOnMap(mapID,doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " MapsObjectHdr.Remove(): " + ex.Message + "\n");
               throw ex;
           }

           return isDeleted;
       }

       public bool BeginTransaction()
       {
           bool isStarted = false;

           try
           {
               isStarted = mapsObjectDAO.beginTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " PassHist.BeginTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return isStarted;
       }

       public void CommitTransaction()
       {
           try
           {
               mapsObjectDAO.commitTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " PassHist.CommitTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public void RollbackTransaction()
       {
           try
           {
               mapsObjectDAO.rollbackTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " PassHist.CommitTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public IDbTransaction GetTransaction()
       {
           try
           {
               return mapsObjectDAO.getTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " PassHist.GetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public void SetTransaction(IDbTransaction trans)
       {
           try
           {
               mapsObjectDAO.setTransaction(trans);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " PassHist.SetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }
   }
}
