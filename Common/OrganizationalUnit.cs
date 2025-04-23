using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class OrganizationalUnit
    {
        DAOFactory daoFactory = null;
        OrganizationalUnitDAO dao = null;
		
		DebugLog log;

        OrganizationalUnitTO orgUnitTO = new OrganizationalUnitTO();

        public OrganizationalUnitTO OrgUnitTO
        {
            get { return orgUnitTO; }
            set { orgUnitTO = value; }
        }
        		
		public OrganizationalUnit()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			dao = daoFactory.getOrganizationalUnitDAO(null);
		}

        public OrganizationalUnit(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            dao = daoFactory.getOrganizationalUnitDAO(dbConnection);
        }

        public OrganizationalUnit(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                dao = daoFactory.getOrganizationalUnitDAO(null);
            }
        }

        public int Save(bool doCommit)
        {
            try
            {
                return dao.insert(this.OrgUnitTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Save(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public bool Update(bool doCommit)
        {
            try
            {
                return dao.update(this.OrgUnitTO, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int FindMAXOUID()
        {
            try
            {
                return dao.findMAXOUID();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.FindMAXOUID(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
       
        public OrganizationalUnitTO Find(int ouID)
        {
            try
            {
                return dao.find(ouID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<OrganizationalUnitTO> Search()
        {           
            try
            {
                return dao.getOrgUnits(this.OrgUnitTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, OrganizationalUnitTO> SearchDictionary()
        {
            try
            {
                return dao.getOrgUnitsDictionary();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
        public DataSet getOrganizationUnits(string oUnits)
        {
            try
            {
                return dao.getOrgUnitsDS(oUnits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<OrganizationalUnitTO> Search(string oUnits)
        {
            try
            {
                return dao.getOrgUnits(oUnits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int ouID)
        {
            try
            {
                return dao.delete(ouID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(int ouID, bool doCommit)
        {
            try
            {
                return dao.delete(ouID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        List<OrganizationalUnitTO> tmpList = new List<OrganizationalUnitTO>();

        public List<OrganizationalUnitTO> FindAllChildren(List<OrganizationalUnitTO> orgUnitIdList)
        {
            List<OrganizationalUnitTO> workingunitTOList = new List<OrganizationalUnitTO>();

            try
            {
                for (int i = 0; i < orgUnitIdList.Count; i++)
                {
                    tmpList.Add(orgUnitIdList[i]);
                    OrganizationalUnitTO orgUnitTO = new OrganizationalUnitTO();
                    orgUnitTO.ParentOrgUnitID = orgUnitIdList[i].OrgUnitID;
                    orgUnitTO.Status = Constants.DefaultStateActive;
                    workingunitTOList = dao.getOrgUnits(orgUnitTO);

                    for (int j = 0; j < workingunitTOList.Count; j++)
                    {
                        if (workingunitTOList[j].OrgUnitID == workingunitTOList[j].ParentOrgUnitID)
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
                log.writeLog(DateTime.Now + " OrganizationalUnit.FindAllChildren(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return tmpList;
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = dao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                dao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                dao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return dao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                dao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " OrganizationalUnit.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<OrganizationalUnitTO> SearchByName(string name)
        {
            return dao.getOUByName(name);
        }



        public List<OrganizationalUnitTO> SearchChildOU(string parentID)
        {
            return dao.getChildOU(parentID);
        }        

    }
}
