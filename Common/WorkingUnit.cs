using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// WorkingUnit.
	/// </summary>
    public class WorkingUnit
    {
        private WorkingUnitsDAO workingUnitDAO;
        private DAOFactory daoFactory;

        DebugLog log;

        WorkingUnitTO wuTO = new WorkingUnitTO();

        public WorkingUnitTO WUTO
        {
            get { return wuTO; }
            set { wuTO = value; }
        }

        public WorkingUnit()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            workingUnitDAO = daoFactory.getWorkingUnitsDAO(null);
        }


        public WorkingUnit(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            workingUnitDAO = daoFactory.getWorkingUnitsDAO(dbConnection);
        }

        public WorkingUnit(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                workingUnitDAO = daoFactory.getWorkingUnitsDAO(null);
            }
        }

        public WorkingUnit(int working_unit_id, int parentWUID, string description, string name, string status, int addressID)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            workingUnitDAO = daoFactory.getWorkingUnitsDAO(null);

            WUTO = new WorkingUnitTO(working_unit_id, parentWUID, description, name, status, addressID);
        }

        public WorkingUnit(int working_unit_id, int parentWUID, string description, string name, string status,
            int addressID, bool createDAO)
        {
            try
            {
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
            }
            catch { }

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                workingUnitDAO = daoFactory.getWorkingUnitsDAO(null);
            }

            WUTO = new WorkingUnitTO(working_unit_id, parentWUID, description, name, status, addressID);
        }

        public bool Find(int workingUnitID)
        {
            bool isFounded = false;

            try
            {
                this.WUTO = workingUnitDAO.find(workingUnitID);

                if (this.WUTO.WorkingUnitID != -1)
                {
                    isFounded = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isFounded;
        }

        public bool Find(int workingUnitID, bool useTrans)
        {
            bool isFounded = false;

            try
            {
                this.WUTO = workingUnitDAO.find(workingUnitID, useTrans);

                if (this.WUTO.WorkingUnitID != -1)
                {
                    isFounded = true;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isFounded;
        }

        List<WorkingUnitTO> tmpList = new List<WorkingUnitTO>();

        public List<WorkingUnitTO> FindAllChildren(List<WorkingUnitTO> workingUnitIdList)
        {
            List<WorkingUnitTO> workingunitTOList = new List<WorkingUnitTO>();

            try
            {
                for (int i = 0; i < workingUnitIdList.Count; i++)
                {
                    tmpList.Add(workingUnitIdList[i]);
                    WorkingUnitTO workingUnitTO = new WorkingUnitTO();
                    workingUnitTO.ParentWorkingUID = workingUnitIdList[i].WorkingUnitID;
                    workingUnitTO.Status = Constants.DefaultStateActive;
                    workingunitTOList = workingUnitDAO.getWorkingUnits(workingUnitTO);

                    for (int j = 0; j < workingunitTOList.Count; j++)
                    {
                        if (workingunitTOList[j].WorkingUnitID == workingunitTOList[j].ParentWorkingUID)
                        {
                            workingunitTOList.RemoveAt(j);
                            j--;
                        }
                    }

                    if (workingunitTOList.Count > 0)
                    {
                        FindAllChildren(workingunitTOList);
                        //for ( int j=0; j< al.Count; j++ )
                        //	tmpList.Add(al[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.FindAllChildren(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return tmpList;
        }

        public List<WorkingUnitTO> FindAllChildrenAllStatuses(List<WorkingUnitTO> workingUnitIdList)
        {
            List<WorkingUnitTO> workingunitTOList = new List<WorkingUnitTO>();

            try
            {
                for (int i = 0; i < workingUnitIdList.Count; i++)
                {
                    tmpList.Add(workingUnitIdList[i]);
                    WorkingUnitTO workingUnitTO = new WorkingUnitTO();
                    workingUnitTO.ParentWorkingUID = workingUnitIdList[i].WorkingUnitID;
                    workingunitTOList = workingUnitDAO.getWorkingUnits(workingUnitTO);

                    for (int j = 0; j < workingunitTOList.Count; j++)
                    {
                        if (workingunitTOList[j].WorkingUnitID == workingunitTOList[j].ParentWorkingUID)
                        {
                            workingunitTOList.RemoveAt(j);
                            j--;
                        }
                    }
                    if (workingunitTOList.Count > 0)
                    {
                        FindAllChildrenAllStatuses(workingunitTOList);
                        //for ( int j=0; j< al.Count; j++ )
                        //	tmpList.Add(al[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.FindAllChildren(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return tmpList;
        }


        public List<WorkingUnitTO> FindAllChildren(List<WorkingUnitTO> workingUnitIdList, bool createDAO)
        {
            List<WorkingUnitTO> workingunitTOList = new List<WorkingUnitTO>();

            try
            {
                for (int i = 0; i < workingUnitIdList.Count; i++)
                {
                    tmpList.Add(workingUnitIdList[i]);
                    WorkingUnitTO workingUnitTO = new WorkingUnitTO();
                    workingUnitTO.ParentWorkingUID = workingUnitIdList[i].WorkingUnitID;
                    workingUnitTO.Status = Constants.DefaultStateActive;
                    workingunitTOList = workingUnitDAO.getWorkingUnits(workingUnitTO);

                    for (int j = 0; j < workingunitTOList.Count; j++)
                    {
                        if (workingunitTOList[j].WorkingUnitID == workingunitTOList[j].ParentWorkingUID)
                        {
                            workingunitTOList.RemoveAt(j);
                            j--;
                        }
                    }
                    if (workingunitTOList.Count > 0)
                    {
                        FindAllChildren(workingunitTOList);
                        //for ( int j=0; j< al.Count; j++ )
                        //	tmpList.Add(al[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.FindAllChildren(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return tmpList;
        }

        public int FindMAXWUID()
        {
            try
            {
                return workingUnitDAO.findMAXWUID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.FindMAXWUID(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public int FindMINWUID()
        {
            int wuID = -1;
            try
            {
                wuID = workingUnitDAO.findMINWUID();
                if (wuID >= 0)
                {
                    wuID = -1;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.FindMAXWUID(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return wuID;
        }

        /// <summary>
        /// Search Working Units that approve given condition 
        /// </summary>
        /// <param name="workingUnitID"></param>
        /// <param name="parentWUID"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <returns>List of Working Units object</returns>
        public List<WorkingUnitTO> Search()
        {
            try
            {
                return workingUnitDAO.getWorkingUnits(this.WUTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Search Working Units that approve given condition 
        /// </summary>
        /// <param name="workingUnitID"></param>
        /// <param name="parentWUID"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <returns>List of Working Units object</returns>
        public List<WorkingUnitTO> SearchExact()
        {
            try
            {
                return workingUnitDAO.getWorkingUnitsExact(this.WUTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }


        public List<WorkingUnitTO> Search(string wunits)
        {
            try
            {
                return workingUnitDAO.getWUnits(wunits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<WorkingUnitTO> SearchByName(string name) 
        {
            return workingUnitDAO.getWUByName(name);
        }

        //public List<WorkingUnitTO> SearchByName(string wunit)
        //{
        //    try
        //    {
        //        return workingUnitDAO.getWUnitByName(wunit);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " WorkingUnit.Search(): " + ex.Message + "\n");
        //        throw new Exception(ex.Message);
        //    }
        //}

        public List<WorkingUnitTO> Search(bool createDAO)
        {
            try
            {
                return workingUnitDAO.getWorkingUnits(this.WUTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<WorkingUnitTO> getWorkingUnitsByOU(string orgUnitID)
        {
            try
            {
                return workingUnitDAO.getWorkingUnitsForOU(orgUnitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getWorkingUnitsByOU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Save Working Unit using DAO object
        /// </summary>
        /// <param name="workingUnitID"></param>
        /// <param name="parentWUID"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <returns>number of affected rows</returns>
        public int Save()
        {
            try
            {
                return workingUnitDAO.insert(this.WUTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public int Save(bool doCommit)
        {
            try
            {
                return workingUnitDAO.insert(this.WUTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Save(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Save this Working Unit
        /// </summary>
        /// <returns>number of affected rows</returns>
        //public int Save()
        //{
        //    int inserted = 0;

        //    try
        //    {
        //        inserted = workingUnitDAO.insert(this.WUTO);
        //    }
        //    catch(Exception ex)
        //    {
        //        log.writeLog(DateTime.Now + " WorkingUnit.Save(): " + ex.Message + "\n");
        //        throw ex;
        //    }

        //    return inserted;
        //}


        /// <summary>
        /// Update Working Unit using DAO
        /// </summary>
        /// <param name="workingUnitID"></param>
        /// <param name="parentWUID"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <returns>true if succedeed, false otherwise</returns>
        public bool Update()
        {
            try
            {
                return workingUnitDAO.update(this.WUTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update Working Unit using DAO
        /// </summary>
        /// <param name="workingUnitID"></param>
        /// <param name="parentWUID"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="doCommit"></param>
        /// <returns>true if succedeed, false otherwise</returns>
        public bool Update(bool doCommit)
        {
            try
            {
                return workingUnitDAO.update(this.WUTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int workingUnitsID)
        {
            try
            {
                this.Find(workingUnitsID);
                this.WUTO.Status = Constants.statusRetired;
                return Update();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int workingUnitsID, bool doCommit)
        {
            try
            {
                this.Find(workingUnitsID, !doCommit);
                this.WUTO.Status = Constants.statusRetired;
                return Update(doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public WorkingUnitTO getParentWorkingUnit()
        {
            try
            {
                return workingUnitDAO.find(this.WUTO.ParentWorkingUID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getParentWorkingUnit(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = workingUnitDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                workingUnitDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                workingUnitDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return workingUnitDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                workingUnitDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void Clear()
        {
            this.WUTO = new WorkingUnitTO();
        }


        /// <summary>
        /// Send list of WorkingUnitTO objects to serialization
        /// </summary>
        /// <param name="workingUnitsTOList">List of WorkingUnitTO</param>
        private void CacheData(List<WorkingUnitTO> workingUnitsTOList)
        {
            try
            {
                workingUnitDAO.serialize(workingUnitsTOList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.CacheData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Search data form database with given criteria and then create cache,
        /// XML file 
        /// </summary>
        /// <param name="workingUnitID"></param>
        /// <param name="parentWUID"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        public void CacheData()
        {
            List<WorkingUnitTO> wuTOList = new List<WorkingUnitTO>();

            try
            {
                wuTOList = workingUnitDAO.getWorkingUnits(this.WUTO);
                this.CacheData(wuTOList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.CacheData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Push all of Working Units to XML data source
        /// </summary>
        public void CacheAllData()
        {
            try
            {
                this.CacheData(workingUnitDAO.getWorkingUnits(new WorkingUnitTO()));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.CacheAllData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Change current DAO Factory. Switch to XML data source.
        /// </summary>
        public void SwitchToXMLDAO()
        {
            try
            {
                daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
                workingUnitDAO = daoFactory.getWorkingUnitsDAO(null);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.SwitchToXMLDAO(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public DataSet getWorkingUnits(string workingUnitID)
        {
            DataSet dataSet = new DataSet();

            try
            {
                dataSet = workingUnitDAO.getWorkingUnits(workingUnitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getWorkingUnits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return dataSet;
        }

        public DataSet getRootWorkingUnits(string workingUnitID)
        {
            DataSet dataSet = new DataSet();

            try
            {
                dataSet = workingUnitDAO.getRootWorkingUnits(workingUnitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getWorkingUnits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return dataSet;
        }

        public List<WorkingUnitTO> getRootWorkingUnitsList(string workingUnitID)
        {
            List<WorkingUnitTO> list = new List<WorkingUnitTO>();

            try
            {
                list = workingUnitDAO.getRootWorkingUnitsList(workingUnitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getWorkingUnits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return list;
        }

        public WorkingUnitTO FindWU(int workingUnitID)
        {
            try
            {
                return workingUnitDAO.find(workingUnitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public Dictionary<int, WorkingUnitTO> getWUDictionary(IDbTransaction trans)
        {
            try
            {
                return workingUnitDAO.getWUDictionary(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getWUDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public Dictionary<int, WorkingUnitTO> getWUDictionary()
        {
            try
            {
                return workingUnitDAO.getWUDictionary();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getWUDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<WorkingUnitTO> SearchRootWU(string wuIDs)
        {
            try
            {
                return workingUnitDAO.getRootWU(wuIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.SearchRootWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<WorkingUnitTO> SearchChildWU(string parentID)
        {
            try
            {
                return workingUnitDAO.getChildWU(parentID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.SearchChildWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, WorkingUnitTO> SearchAllWU()
        {
            try
            {
                Dictionary<int, WorkingUnitTO> WUall = new Dictionary<int, WorkingUnitTO>();
                List<WorkingUnitTO> rootWU = workingUnitDAO.getWorkingUnits(new WorkingUnitTO());


                foreach (WorkingUnitTO wu in rootWU)
                {
                    if (!WUall.ContainsKey(wu.WorkingUnitID))
                        WUall.Add(wu.WorkingUnitID, wu);
                }

                return WUall;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.SearchAllWUHierarchicly(): " + ex.Message + "\n");
                throw ex;
            }
        }
        Dictionary<WorkingUnitTO, int> allWU = new Dictionary<WorkingUnitTO, int>(); //<working unit, working unit level>

        public Dictionary<WorkingUnitTO, int> SearchAllWUHierarchicly()
        {
            try
            {
                allWU = new Dictionary<WorkingUnitTO, int>();
                List<WorkingUnitTO> rootWU = SearchRootWU("");
                getChildWU(rootWU, 0);

                foreach (WorkingUnitTO wu in allWU.Keys)
                {
                    wu.Name = wu.Name.PadLeft(wu.Name.Length + allWU[wu] * 5, ' ');
                }

                return allWU;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.SearchAllWUHierarchicly(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void getChildWU(List<WorkingUnitTO> wuList, int level)
        {
            try
            {
                foreach (WorkingUnitTO wu in wuList)
                {
                    allWU.Add(wu, level);
                    List<WorkingUnitTO> wuChildren = SearchChildWU(wu.WorkingUnitID.ToString().Trim());

                    if (wuChildren.Count > 0)
                    {
                        getChildWU(wuChildren, level + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getChildWU(): " + ex.Message + "\n");
                throw ex;
            }
        }
        public List<WorkingUnitTO> getAllWU()
        {
            try
            {
                return workingUnitDAO.getAllWU();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WorkingUnit.getChildWU(): " + ex.Message + "\n");
                throw ex;
            }
        }


       
    }
        
}
