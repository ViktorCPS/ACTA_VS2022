using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

using DataAccess;
using Util;

namespace Common
{
    public class EmployeeAsco4
    {
        DAOFactory daoFactory = null;
        EmployeeAsco4DAO edao = null;

        DebugLog log;

        EmployeeAsco4TO emplAsco4TO = new EmployeeAsco4TO();

        public EmployeeAsco4TO EmplAsco4TO
        {
            get { return emplAsco4TO; }
            set { emplAsco4TO = value; }
        }
        
        public EmployeeAsco4()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			edao = daoFactory.getEmployeeAsco4DAO(null);
		}

        public EmployeeAsco4(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeAsco4DAO(dbConnection);
        }

        public bool save()
        {
            bool saved = false;
            try
            {
                saved = edao.insert(this.EmplAsco4TO.EmployeeID,this.EmplAsco4TO.IntegerValue1, this.EmplAsco4TO.IntegerValue2, this.EmplAsco4TO.IntegerValue3, this.EmplAsco4TO.IntegerValue4, this.EmplAsco4TO.IntegerValue5,
                    this.EmplAsco4TO.IntegerValue6, this.EmplAsco4TO.IntegerValue7, this.EmplAsco4TO.IntegerValue8, this.EmplAsco4TO.IntegerValue9, this.EmplAsco4TO.IntegerValue10, this.EmplAsco4TO.DatetimeValue1,
                     this.EmplAsco4TO.DatetimeValue2, this.EmplAsco4TO.DatetimeValue3, this.EmplAsco4TO.DatetimeValue4, this.EmplAsco4TO.DatetimeValue5, this.EmplAsco4TO.DatetimeValue6, this.EmplAsco4TO.DatetimeValue7,
                     this.EmplAsco4TO.DatetimeValue8, this.EmplAsco4TO.DatetimeValue9, this.EmplAsco4TO.DatetimeValue10, this.EmplAsco4TO.NVarcharValue1, this.EmplAsco4TO.NVarcharValue2, this.EmplAsco4TO.NVarcharValue3,
                     this.EmplAsco4TO.NVarcharValue4, this.EmplAsco4TO.NVarcharValue5, this.EmplAsco4TO.NVarcharValue6, this.EmplAsco4TO.NVarcharValue7, this.EmplAsco4TO.NVarcharValue8, this.EmplAsco4TO.NVarcharValue9,
                      this.EmplAsco4TO.NVarcharValue10);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.save(): " + ex.Message + this.EmplAsco4TO.EmployeeID.ToString() + "\n");
                throw ex;
            }

            return saved;
        }

        public bool save(bool doCommit)
        {
            bool saved = false;
            try
            {
                saved = edao.insert(this.EmplAsco4TO.EmployeeID, this.EmplAsco4TO.IntegerValue1, this.EmplAsco4TO.IntegerValue2, this.EmplAsco4TO.IntegerValue3, this.EmplAsco4TO.IntegerValue4, this.EmplAsco4TO.IntegerValue5,
                    this.EmplAsco4TO.IntegerValue6, this.EmplAsco4TO.IntegerValue7, this.EmplAsco4TO.IntegerValue8, this.EmplAsco4TO.IntegerValue9, this.EmplAsco4TO.IntegerValue10, this.EmplAsco4TO.DatetimeValue1,
                     this.EmplAsco4TO.DatetimeValue2, this.EmplAsco4TO.DatetimeValue3, this.EmplAsco4TO.DatetimeValue4, this.EmplAsco4TO.DatetimeValue5, this.EmplAsco4TO.DatetimeValue6, this.EmplAsco4TO.DatetimeValue7,
                     this.EmplAsco4TO.DatetimeValue8, this.EmplAsco4TO.DatetimeValue9, this.EmplAsco4TO.DatetimeValue10, this.EmplAsco4TO.NVarcharValue1, this.EmplAsco4TO.NVarcharValue2, this.EmplAsco4TO.NVarcharValue3,
                     this.EmplAsco4TO.NVarcharValue4, this.EmplAsco4TO.NVarcharValue5, this.EmplAsco4TO.NVarcharValue6, this.EmplAsco4TO.NVarcharValue7, this.EmplAsco4TO.NVarcharValue8, this.EmplAsco4TO.NVarcharValue9,
                      this.EmplAsco4TO.NVarcharValue10, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.save(): " + ex.Message + this.EmplAsco4TO.EmployeeID.ToString() + "\n");
                throw ex;
            }

            return saved;
        }

        public bool update()
        {
            bool update = false;
            try
            {
                update = edao.update(this.EmplAsco4TO.EmployeeID, this.EmplAsco4TO.IntegerValue1, this.EmplAsco4TO.IntegerValue2, this.EmplAsco4TO.IntegerValue3, this.EmplAsco4TO.IntegerValue4, this.EmplAsco4TO.IntegerValue5,
                    this.EmplAsco4TO.IntegerValue6, this.EmplAsco4TO.IntegerValue7, this.EmplAsco4TO.IntegerValue8, this.EmplAsco4TO.IntegerValue9, this.EmplAsco4TO.IntegerValue10, this.EmplAsco4TO.DatetimeValue1,
                     this.EmplAsco4TO.DatetimeValue2, this.EmplAsco4TO.DatetimeValue3, this.EmplAsco4TO.DatetimeValue4, this.EmplAsco4TO.DatetimeValue5, this.EmplAsco4TO.DatetimeValue6, this.EmplAsco4TO.DatetimeValue7,
                     this.EmplAsco4TO.DatetimeValue8, this.EmplAsco4TO.DatetimeValue9, this.EmplAsco4TO.DatetimeValue10, this.EmplAsco4TO.NVarcharValue1, this.EmplAsco4TO.NVarcharValue2, this.EmplAsco4TO.NVarcharValue3,
                     this.EmplAsco4TO.NVarcharValue4, this.EmplAsco4TO.NVarcharValue5, this.EmplAsco4TO.NVarcharValue6, this.EmplAsco4TO.NVarcharValue7, this.EmplAsco4TO.NVarcharValue8, this.EmplAsco4TO.NVarcharValue9,
                      this.EmplAsco4TO.NVarcharValue10);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.update(): " + ex.Message + this.EmplAsco4TO.EmployeeID.ToString() + "\n");
                throw ex;
            }

            return update;
        }

        public bool update(bool doCommit)
        {
            bool update = false;
            try
            {
                update = edao.update(this.EmplAsco4TO.EmployeeID, this.EmplAsco4TO.IntegerValue1, this.EmplAsco4TO.IntegerValue2, this.EmplAsco4TO.IntegerValue3, this.EmplAsco4TO.IntegerValue4, this.EmplAsco4TO.IntegerValue5,
                    this.EmplAsco4TO.IntegerValue6, this.EmplAsco4TO.IntegerValue7, this.EmplAsco4TO.IntegerValue8, this.EmplAsco4TO.IntegerValue9, this.EmplAsco4TO.IntegerValue10, this.EmplAsco4TO.DatetimeValue1,
                     this.EmplAsco4TO.DatetimeValue2, this.EmplAsco4TO.DatetimeValue3, this.EmplAsco4TO.DatetimeValue4, this.EmplAsco4TO.DatetimeValue5, this.EmplAsco4TO.DatetimeValue6, this.EmplAsco4TO.DatetimeValue7,
                     this.EmplAsco4TO.DatetimeValue8, this.EmplAsco4TO.DatetimeValue9, this.EmplAsco4TO.DatetimeValue10, this.EmplAsco4TO.NVarcharValue1, this.EmplAsco4TO.NVarcharValue2, this.EmplAsco4TO.NVarcharValue3,
                     this.EmplAsco4TO.NVarcharValue4, this.EmplAsco4TO.NVarcharValue5, this.EmplAsco4TO.NVarcharValue6, this.EmplAsco4TO.NVarcharValue7, this.EmplAsco4TO.NVarcharValue8, this.EmplAsco4TO.NVarcharValue9,
                      this.EmplAsco4TO.NVarcharValue10, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.update(): " + ex.Message + this.EmplAsco4TO.EmployeeID.ToString() + "\n");
                throw ex;
            }

            return update;
        }

        public bool delete(int employeeID)
        {
            bool deleted = false;
            try
            {
                deleted = edao.delete(employeeID);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.delete(): " + ex.Message + this.EmplAsco4TO.EmployeeID.ToString() + "\n");
                throw ex;
            }

            return deleted;
        }

        public bool delete(int employeeID,bool doCommit)
        {
            bool deleted = false;
            try
            {
                deleted = edao.delete(employeeID,doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.delete(): " + ex.Message + this.emplAsco4TO.EmployeeID.ToString() + "\n");
                throw ex;
            }

            return deleted;
        }

        public List<EmployeeAsco4TO> Search()
        {
            try
            {
                return edao.getEmployeesAsco(this.EmplAsco4TO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeAsco4TO> Search(string emplIDs)
        {
            try
            {
                return edao.getEmployeesAsco(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, EmployeeAsco4TO> SearchDictionary(string emplIDs)
        {
            try
            {
                return edao.getEmployeesAscoDictionary(emplIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<string, EmployeeTO> SearchICodeData()
        {
            try
            {
                return edao.getICodeData();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = edao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                edao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                edao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return edao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                edao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeeAsco4.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

    }
}
