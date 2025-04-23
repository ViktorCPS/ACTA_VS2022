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
    public class Map
    {
        private int _mapID;
        private int _parentMapID;
        private string _name;
        private string _description;
        private byte[] _content;

        DAOFactory daoFactory = null;
        MapDAO mdao = null;

        DebugLog log;

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int ParentMapID
        {
            get { return _parentMapID; }
            set { _parentMapID = value; }
        }

        public int MapID
        {
            get { return _mapID; }
            set { _mapID = value; }
        }

        public Map()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mdao = daoFactory.getMapDAO(null);

            this.MapID = -1;
            this.Name = "";
            this.Description = "";
            this.ParentMapID = -1;
            this.Content = null;
        }

        public Map(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mdao = daoFactory.getMapDAO(dbConnection);

            this.MapID = -1;
            this.Name = "";
            this.Description = "";
            this.ParentMapID = -1;
            this.Content = null;
        }
        public Map(int mapID, int parentMapID, string name, string description, byte[] content)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            mdao = daoFactory.getMapDAO(null);

            this.MapID = mapID;
            this.Name = name;
            this.Description = description;
            this.ParentMapID = parentMapID;
            this.Content = content;
        }

        public void ReceiveTransferObject(MapTO mapTO)
        {
            this.MapID = mapTO.MapID;
            this.Name = mapTO.Name;
            this.Description = mapTO.Description;
            this.ParentMapID = mapTO.ParentMapID;
            this.Content = mapTO.Content;            
        }

        public MapTO SendTransferObject()
        {
            MapTO mapTO = new MapTO();

            mapTO.MapID = this.MapID;
            mapTO.Name = this.Name;
            mapTO.Description = this.Description;
            mapTO.ParentMapID = this.ParentMapID;
            mapTO.Content = this.Content;
            
            return mapTO;
        }

        public bool Save(int mapID, string name, string description, byte[] content)
        {
            bool saved = false;

            try
            {
                saved = mdao.insert(mapID, name, description, content);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Map.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return saved;
        }

        public ArrayList Search(int mapID, int parentMapID, string name, string description)
        {
            ArrayList mapTOList = new ArrayList();
            ArrayList mapList = new ArrayList();

            try
            {
                Map mapMember = new Map();
                mapTOList = mdao.getMaps(mapID, parentMapID, name, description);

                foreach (MapTO mto in mapTOList)
                {
                    mapMember = new Map();
                    mapMember.ReceiveTransferObject(mto);

                    mapList.Add(mapMember);
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Holiday.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return mapList;
        }

        public Map getParentMap()
        {
            MapTO mapParentTO = new MapTO();
            Map map = new Map();

            try
            {
                mapParentTO = this.mdao.find(this.ParentMapID);
                map.ReceiveTransferObject(mapParentTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Map.getParentMap(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return map;
        }

        public bool Delete(int MapID,bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = mdao.delete(MapID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Map.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool Update(int mapID, int parentID, string name, string description, byte[] content)
        {
            bool isUpadated = false;

            try
            {
                isUpadated = mdao.update(mapID,parentID, name, description, content);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Map.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpadated;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = mdao.beginTransaction();
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
                mdao.commitTransaction();
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
                mdao.rollbackTransaction();
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
                return mdao.getTransaction();
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
                mdao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " PassHist.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int FindMAXMapID()
        {
            int mapID = 0;

            try
            {
                mapID = mdao.findMAXMapID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.FindMAXWUID(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return mapID;
        }
    }
}
