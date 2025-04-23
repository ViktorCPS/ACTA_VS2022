
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using DataAccess;
using TransferObjects;
using Util;

namespace Common {
    [Serializable()]
    /// <summary>
    /// Employee
    /// </summary>
    public class Employee : ISerializable {
        DAOFactory daoFactory = null;
        EmployeeDAO edao = null;

        DebugLog log;

        EmployeeTO emplTO = new EmployeeTO();

        public EmployeeTO EmplTO {
            get { return emplTO; }
            set { emplTO = value; }
        }

        public Employee() {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeDAO(null);
        }

        public Employee(bool createDAO) {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO) {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getEmployeeDAO(null);
            }
        }

        public Employee(Object dbConnection) {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeDAO(dbConnection);
        }

        public Employee(int employeeID, string firstName, string lastName, int workingUnitId,
            string status, string password, int addressID, string picture, int workingGroupID, string type) {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getEmployeeDAO(null);

            this.EmplTO.EmployeeID = employeeID;
            this.EmplTO.FirstName = firstName;
            this.EmplTO.LastName = lastName;
            this.EmplTO.WorkingUnitID = workingUnitId;
            this.EmplTO.Status = status;
            this.EmplTO.Password = password;
            this.EmplTO.AddressID = addressID;
            this.EmplTO.Picture = picture;
            this.EmplTO.WorkingGroupID = workingGroupID;
            this.EmplTO.Type = type;
            this.EmplTO.TagID = 0;
        }

        public Employee(SerializationInfo info, StreamingContext ctxt) {
            //Get the values from info and assign them to the appropriate properties
            this.EmplTO.EmployeeID = (int)info.GetValue("EmployeeID", typeof(int));
            this.EmplTO.FirstName = (String)info.GetValue("FirstName", typeof(string));
            this.EmplTO.LastName = (String)info.GetValue("LastName", typeof(string));
            this.EmplTO.WorkingUnitID = (int)info.GetValue("WorkingUnitID", typeof(int));
            this.EmplTO.Status = (String)info.GetValue("Status", typeof(string));
            this.EmplTO.Password = (String)info.GetValue("Password", typeof(string));
            this.EmplTO.AddressID = (int)info.GetValue("AddressID", typeof(int));
            this.EmplTO.Picture = (String)info.GetValue("Picture", typeof(string));
            this.EmplTO.WorkingGroupID = (int)info.GetValue("WorkingGroupID", typeof(int));
            this.EmplTO.Type = (String)info.GetValue("Type", typeof(string));

            //this.AccessGroupID = (int)info.GetValue("AccessGroupID", typeof(int));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            info.AddValue("EmployeeID", this.EmplTO.EmployeeID);
            info.AddValue("FirstName", this.EmplTO.FirstName);
            info.AddValue("LastName", this.EmplTO.LastName);
            info.AddValue("WorkingUnitID", this.EmplTO.WorkingUnitID);
            info.AddValue("Status", this.EmplTO.Status);
            info.AddValue("Password", this.EmplTO.Password);
            info.AddValue("AddressID", this.EmplTO.AddressID);
            info.AddValue("Picture", this.EmplTO.Picture);
            info.AddValue("WorkingGroupID", this.EmplTO.WorkingGroupID);
            info.AddValue("Type", this.EmplTO.Type);

            //info.AddValue("AccessGroupID", this.AccessGroupID);
        }
        public int Save(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string typeID, string accessGroupID, int orgUnitID, bool doCommit) {
            int saved = 0;
            try {
                saved = edao.insert(EmployeeID, FirstName, LastName,
                    WorkingUnitID, Status, Password, AddressID, Picture, WorkingGroupID, typeID, accessGroupID, orgUnitID, doCommit);

            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Save(): " + ex.Message + EmployeeID.ToString() + "\n");
                throw ex;
            }

            return saved;
        }

        public int Save(bool doCommit) {
            try {
                return edao.insert(this.EmplTO, doCommit);

            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Save(): " + ex.Message + this.EmplTO.EmployeeID.ToString() + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Save Employee 
        /// </summary>
        /// <returns></returns>
        public int Save(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID, bool doCommit) {
            int saved = 0;
            try {
                saved = edao.insert(EmployeeID, FirstName, LastName,
                    WorkingUnitID, Status, Password, AddressID, Picture, WorkingGroupID, type, accessGroupID, doCommit);

            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Save(): " + ex.Message + EmployeeID.ToString() + "\n");
                throw ex;
            }

            return saved;
        }

        /// <summary>
        /// Save Employee 
        /// </summary>
        /// <returns></returns>
        public int Save(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID, string user, bool doCommit) {
            int saved = 0;
            try {
                saved = edao.insert(EmployeeID, FirstName, LastName,
                    WorkingUnitID, Status, Password, AddressID, Picture, WorkingGroupID, type, accessGroupID, user, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        /// <summary>
        /// Update Employee data using DAO instance
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="WorkingUnitID"></param>
        /// <param name="Status"></param>
        /// <param name="Password"></param>
        /// <param name="AddressID"></param>
        /// <param name="Picture"></param>
        /// <returns></returns>
        public bool Update(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID) {
            bool isUpdated = false;

            try {
                // TODO: cerate TO and send to DAO
                isUpdated = edao.update(EmployeeID, FirstName, LastName,
                    WorkingUnitID, Status, Password, AddressID, Picture, WorkingGroupID, type, accessGroupID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Update(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID, int orgUnitID, bool doCommit) {
            bool isUpdated = false;

            try {
                // TODO: cerate TO and send to DAO
                isUpdated = edao.update(EmployeeID, FirstName, LastName,
                    WorkingUnitID, Status, Password, AddressID, Picture, WorkingGroupID, type, accessGroupID, orgUnitID, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        //natalija07112017
        public bool Update(EmployeeTO EmplTO, int emplGroupID, IDbTransaction trans) {
            bool isUpdated = false;

            try {
                // TODO: cerate TO and send to DAO
                isUpdated = edao.update(EmplTO, emplGroupID, trans);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Update(bool doCommit) {
            bool isUpdated = false;

            try {
                // TODO: cerate TO and send to DAO
                isUpdated = edao.update(this.EmplTO, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Update(string EmployeeID, string FirstName, string LastName, string WorkingUnitID,
            string Status, string Password, string AddressID, string Picture, string WorkingGroupID,
            string type, string accessGroupID, bool doCommit) {
            bool isUpdated = false;

            try {
                // TODO: cerate TO and send to DAO
                isUpdated = edao.update(EmployeeID, FirstName, LastName,
                    WorkingUnitID, Status, Password, AddressID, Picture, WorkingGroupID, type, accessGroupID, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        /// <summary>
        /// Search Employees that approve given condition, receive TO and map to Employee object
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="WorkingUnitID"></param>
        /// <param name="Status"></param>
        /// <param name="Password"></param>
        /// <param name="AddressID"></param>
        /// <param name="Picture"></param>
        /// <returns></returns>
        // Same as method Search(string  EmployeeID, string FirstName, string LastName, string WorkingUnitID, string Status, string Password, string AddressID, string Picture, string WorkingGroupID, string type, IdbTransaction trans)
        public List<EmployeeTO> Search() {
            try {
                return edao.getEmployees(this.EmplTO);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public string SearchActiveIDsForScheduling() {
            try {
                return edao.getActiveIDsForScheduling();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchActiveIDsForScheduling(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> Search(string emplIDs) {
            try {
                return edao.getEmployees(emplIDs);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<int> SearchIDs() {
            try {
                return edao.getEmployeesIDs();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchIDs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, int> SearchEmployeesGroups(string emplIDs, IDbTransaction trans) {
            try {
                return edao.getEmployeesGroups(emplIDs, trans);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchEmployeesGroups(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchEmployeesWUResponsible(string wuID, List<int> typesVisible, DateTime from, DateTime to) {
            try {
                return edao.getEmployeesWUResponsible(wuID, typesVisible, from, to);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchEmployeesWUResponsible(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchEmployeesOUResponsible(string ouID, List<int> typesVisible, DateTime from, DateTime to) {
            try {
                return edao.getEmployeesOUResponsible(ouID, typesVisible, from, to);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchEmployeesOUResponsible(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, EmployeeTO> SearchDictionaryWithASCO() {
            try {
                return edao.getEmployeesDictionaryWithASCO(this.EmplTO);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public Dictionary<int, EmployeeTO> SearchDictionary() {
            try {
                return edao.getEmployeesDictionary(this.EmplTO);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, EmployeeTO> SearchDictionary(string emplIDs) {
            try {
                return edao.getEmployeesDictionary(emplIDs);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, EmployeeTO> SearchDictionaryWCSelfService() {
            try {
                return edao.getEmployeesDictionaryWCSelfService();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchDictionary(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeeTO> getAllEmployees() {
            try {
                return edao.getAllEmployees();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        // Same as method Search(string  EmployeeID, string FirstName, string LastName, string WorkingUnitID, string Status, string Password, string AddressID, string Picture, string WorkingGroupID, string type)
        public List<EmployeeTO> Search(IDbTransaction trans) {
            try {
                return edao.getEmployees(this.EmplTO, trans);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByWU(string wUnits) {
            try {
                return edao.getEmployeesByWU(wUnits);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByWULoans(string wUnits, int emplID, List<int> typesVisible, DateTime from, DateTime to) {
            try {
                return edao.getEmployeesByWULoans(wUnits, emplID, typesVisible, from, to);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByWULoans(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByOU(string oUnits, int emplID, List<int> typesVisible, DateTime from, DateTime to) {
            try {
                return edao.getEmployeesByOU(oUnits, emplID, typesVisible, from, to);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByOU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByOU(string oUnits) {
            try {
                return edao.getEmployeesByOU(oUnits);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByOU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        // 18.06.2019.  BOJAN
        public List<EmployeeTO> SearchByOUandWU(int orgUnitID, int workingUnitID) {
            try {
                return edao.getEmployeesByOUandWU(orgUnitID, workingUnitID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByOUandWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchNotInWU(string wUnits) {
            try {
                return edao.getEmployeesNotInWU(wUnits);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByWU(string wUnits, bool createDAO) {
            try {
                return edao.getEmployeesByWU(wUnits);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByWUWithStatuses(string wUnits, List<string> statuses) {
            try {
                return edao.getEmployeesByWUWithStatuses(wUnits, statuses);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByWUWithStatuses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Search Employees that approve given condition, receive TO and map to Employee object
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="WorkingUnitID"></param>
        /// <param name="Status"></param>
        /// <param name="Password"></param>
        /// <param name="AddressID"></param>
        /// <param name="Picture"></param>
        /// <returns></returns>
        public List<EmployeeTO> SearchWithStatuses(List<string> Statuses, string wuString) {
            try {
                return edao.getEmployesWithStatus(this.EmplTO, Statuses, wuString);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchWithStatuses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchWithStatuses(List<string> Statuses, string wuString, bool createDAO) {
            try {
                return edao.getEmployesWithStatus(this.EmplTO, Statuses, wuString);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchWithStatuses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchTagsWithStatuses(List<string> Statuses, string wUnits, int hasTag) {
            try {
                return edao.getEmployesTagsWithStatus(this.EmplTO, Statuses, wUnits, hasTag);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchTagsWithStatuses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchWithStatusesNotInGroup(List<string> Statuses, string wuString, int groupID) {
            try {
                return edao.getEmployesWithStatusNotInGroup(Statuses, wuString, groupID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchWithStatuses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Search Employees that approve given condition, receive TO and map to Employee object
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="WorkingUnitID"></param>
        /// <param name="Status"></param>
        /// <param name="Password"></param>
        /// <param name="AddressID"></param>
        /// <param name="Picture"></param>
        /// <returns></returns>
        public Dictionary<int, EmployeeTO> SearchHash() {
            List<EmployeeTO> employeeTOList = new List<EmployeeTO>();
            Dictionary<int, EmployeeTO> employeeHashtable = new Dictionary<int, EmployeeTO>();

            try {
                employeeTOList = edao.getEmployees(this.EmplTO);

                foreach (EmployeeTO emplTO in employeeTOList) {
                    employeeHashtable.Add(emplTO.EmployeeID, emplTO);
                }
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeHashtable;
        }

        public List<EmployeeTO> SearchByWUGetAccessGroup(string wUnits) {
            try {
                return edao.getEmployeesByWUGetAccessGroup(wUnits);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByWUGetAccessGroup(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByAccessGroup(string accessGroupID) {
            try {
                return edao.getEmployeesByAccessGroup(accessGroupID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByAccessGroup(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchNotInAccessGroup(string accessGroupID) {
            try {
                return edao.getEmployeesNotInAccessGroup(accessGroupID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchNotInAccessGroup(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchVisitors(string wUnits, List<string> statuses, string type) {
            try {
                return edao.getEmployeesVisitors(wUnits, statuses, type);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchVisitors(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByTags(string tags) {
            try {
                return edao.getEmployeesByTags(tags);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByTags(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public Dictionary<uint, EmployeeTO> SearchByTagsDictionary(string tags) {
            try {
                return edao.getEmployeesByTagsDictionary(tags);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByTags(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchByBlockedTags(string tags) {
            try {
                return edao.getEmployeesByTags(tags);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByBlockedTags(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public EmployeeTO SearchByTag(string tags) {
            try {
                return edao.getEmployeesByTag(tags);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchByTag(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public bool BeginTransaction() {
            bool isStarted = false;

            try {
                isStarted = edao.beginTransaction();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction() {
            try {
                edao.commitTransaction();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction() {
            try {
                edao.rollbackTransaction();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction() {
            try {
                return edao.getTransaction();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans) {
            try {
                edao.setTransaction(trans);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateAccessGroup(string EmployeeID, string AccessGroupID, bool doCommit) {
            bool isUpdated = false;

            try {
                isUpdated = edao.updateAccessGroup(EmployeeID, AccessGroupID, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.UpdateAccessGroup(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool UpdateWU(string EmployeeID, string WorkingUnitID, bool doCommit) {
            bool isUpdated = false;

            try {
                isUpdated = edao.updateWU(EmployeeID, WorkingUnitID, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.UpdateWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool UpdateOU(string EmployeeID, string ouID, bool doCommit) {
            try {
                return edao.updateOU(EmployeeID, ouID, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.UpdateOU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool UpdatePassword(string employeeID, string password) {
            bool isUpdated = false;

            try {
                isUpdated = edao.updatePassword(employeeID, password);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.UpdatePassword(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Delete(string employeeId) {
            bool isDeleted = false;

            try {
                isDeleted = edao.delete(employeeId);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;

        }

        // Same as method Find(string employeeId, IDBTransaction trans)
        public EmployeeTO Find(string employeeId) {
            EmployeeTO employeeTO = new EmployeeTO();

            try {
                employeeTO = edao.find(employeeId);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeTO;
        }

        // Same as method Find(string employeeId)
        public EmployeeTO Find(string employeeId, IDbTransaction trans) {
            EmployeeTO employeeTO = new EmployeeTO();

            try {
                employeeTO = edao.find(employeeId, trans);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeTO;
        }

        public EmployeeTO FindUserEmployee(string userID) {
            try {
                return edao.findUserEmployee(userID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.FindUserEmployee(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public EmployeeTO FindEmplMealType(uint tagID) {
            EmployeeTO employeeTO = new EmployeeTO();

            try {
                employeeTO = edao.findEmplMealType(tagID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.FindEmplMealType(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employeeTO;
        }

        public void Clear() {
            this.EmplTO = new EmployeeTO();
        }

        public TagTO FindActive(int ownerID) {
            TagTO tag = new TagTO();

            try {
                tag = edao.findActive(ownerID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.FindActive(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return tag;
        }

        public ArrayList findTimeSchema(DateTime date) {
            // first member is Time Schema for that date and second is day of Time Schema
            ArrayList emplSchema = new ArrayList();

            // Find Time Schema and day that was scheduled to Employee for that date
            List<EmployeeTimeScheduleTO> schedule = new EmployeesTimeSchedule().SearchMonthSchedule(this.EmplTO.EmployeeID, date);

            TimeSchema schema = new TimeSchema();
            EmployeeTimeScheduleTO emplTimeSchedule = new EmployeeTimeScheduleTO();
            int timeScheduleIndex = -1;
            int dayNum = 0;

            for (int scheduleIndex = 0; scheduleIndex < schedule.Count; scheduleIndex++) {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= schedule[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (schedule[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0) {
                emplTimeSchedule = schedule[timeScheduleIndex];
                schema.TimeSchemaTO.TimeSchemaID = emplTimeSchedule.TimeSchemaID;
                List<WorkTimeSchemaTO> schemaList = schema.Search();
                if (schemaList.Count > 0) {
                    emplSchema.Add(schemaList[0]);
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    //dayNum = (emplTimeSchedule.StartCycleDay + date.Day - emplTimeSchedule.Date.Day) % schema.CycleDuration;
                    TimeSpan ts = new TimeSpan(date.Date.Ticks - emplTimeSchedule.Date.Date.Ticks);
                    dayNum = (emplTimeSchedule.StartCycleDay + (int)ts.TotalDays) % schemaList[0].CycleDuration;
                    emplSchema.Add(dayNum);
                }
            }

            return emplSchema;
        }

        public ArrayList findTimeSchema(DateTime date, Dictionary<int, WorkTimeSchemaTO> timeSchemaDict) {
            // first member is Time Schema for that date and second is day of Time Schema
            ArrayList emplSchema = new ArrayList();

            // Find Time Schema and day that was scheduled to Employee for that date
            List<EmployeeTimeScheduleTO> schedule = new EmployeesTimeSchedule().SearchMonthSchedule(this.EmplTO.EmployeeID, date);

            EmployeeTimeScheduleTO emplTimeSchedule = new EmployeeTimeScheduleTO();
            int timeScheduleIndex = -1;
            int dayNum = 0;

            for (int scheduleIndex = 0; scheduleIndex < schedule.Count; scheduleIndex++) {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= schedule[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (schedule[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0) {
                emplTimeSchedule = schedule[timeScheduleIndex];

                if (timeSchemaDict.ContainsKey(emplTimeSchedule.TimeSchemaID)) {
                    emplSchema.Add(timeSchemaDict[emplTimeSchedule.TimeSchemaID]);
                    /* 2008-03-14
                     * From now one, take the last existing time schedule, don't expect that every month has 
                     * time schedule*/
                    //dayNum = (emplTimeSchedule.StartCycleDay + date.Day - emplTimeSchedule.Date.Day) % schema.CycleDuration;
                    TimeSpan ts = new TimeSpan(date.Date.Ticks - emplTimeSchedule.Date.Date.Ticks);
                    dayNum = (emplTimeSchedule.StartCycleDay + (int)ts.TotalDays) % timeSchemaDict[emplTimeSchedule.TimeSchemaID].CycleDuration;
                    emplSchema.Add(dayNum);
                }
            }

            return emplSchema;
        }

        public ArrayList findTimeSchemaFromDataSet(DateTime date, DataSet dsTimeSchedules) {
            // first member is Time Schema for that date and second is day of Time Schema
            ArrayList emplSchema = new ArrayList();

            // Find Time Schema and day that was scheduled to Employee for that date
            List<EmployeeTimeScheduleTO> schedule = new EmployeesTimeSchedule().SearchMonthScheduleFromDataSet(this.EmplTO.EmployeeID, date, dsTimeSchedules);

            TimeSchema schema = new TimeSchema();
            EmployeeTimeScheduleTO emplTimeSchedule = new EmployeeTimeScheduleTO();
            int timeScheduleIndex = -1;
            int dayNum = 0;

            for (int scheduleIndex = 0; scheduleIndex < schedule.Count; scheduleIndex++) {
                if ((date >= schedule[scheduleIndex].Date)
                    && (date.Month == schedule[scheduleIndex].Date.Month)) {
                    timeScheduleIndex = scheduleIndex;
                }
            }

            if (timeScheduleIndex >= 0) {
                emplTimeSchedule = schedule[timeScheduleIndex];
                schema.TimeSchemaTO.TimeSchemaID = emplTimeSchedule.TimeSchemaID;
                List<WorkTimeSchemaTO> schemaList = schema.Search();
                if (schemaList.Count > 0) {
                    emplSchema.Add(schemaList[0]);
                    dayNum = (emplTimeSchedule.StartCycleDay + date.Day - emplTimeSchedule.Date.Day) % schemaList[0].CycleDuration;
                    emplSchema.Add(dayNum);
                }
            }

            return emplSchema;
        }

        /// <summary>
        /// Send list of EmployeeTO objects to serialization
        /// </summary>
        /// <param name="employeeTOList">List of EmployeeTO</param>
        public void CacheData(List<EmployeeTO> employeeTOList) {
            try {
                edao.serialize(employeeTOList);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.CacheData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CacheAllData() {
            try {
                edao.serialize(edao.getEmployees(new EmployeeTO()));
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.CacheData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void CacheData() {
            try {
                List<EmployeeTO> EmployeeTOList = edao.getEmployees(this.EmplTO);

                this.CacheData(EmployeeTOList);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.CacheData(): " + ex.Message + "\n");
                throw ex;
            }
        }

        /// <summary>
        /// Change current DAO Factory. Switch to XML data source.
        /// </summary>
        public void SwitchToXMLDAO() {
            try {
                daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
                edao = daoFactory.getEmployeeDAO(null);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SwitchToXMLDAO(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool UpdatePicture(int employeeID, string picture, bool doCommit) {
            bool isUpdated;

            try {
                isUpdated = edao.updatePicture(employeeID, picture, doCommit);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.UpdatePicture(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public List<EmployeeTO> SearchEmplNumByWUnits() {
            try {
                return edao.getEmplNumByWUnits();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SearchEmplNumByWUnits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateSiemens(int employeeID, string firstName, string lastName, int wuID, int addressID) {
            bool isUpdated;

            try {
                isUpdated = edao.updateSiemens(employeeID, firstName, lastName, wuID, addressID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.UpdateSiemens(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public string getAddressLine3(int employeeID) {
            try {
                return edao.getAddressLine3(employeeID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.getAddressLine3(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }



        public List<int> PrebaciPodatkeSaNAV() {
            try {
                return edao.SyncDataWithNav();
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.SyncDataWithNav(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> ProlasciZaForecast(DateTime dateTime) {
            try {
                return edao.ProlasciZaForKast(dateTime);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.ProlasciZaForecast(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> ForecastProlasci(DateTime dateTime) {
            try {
                return edao.ForKastProlasci(dateTime);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.ForKastProlasci(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeeTO> ProlasciZaForecast(DateTime fromTime, DateTime toTime)
        {
            try
            {
                return edao.ProlasciZaForKast(fromTime,toTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.ProlasciZaForecast(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //  10.06.2019. BOJAN za izvestaj za godisnje odmore zaposlenih
        public List<EmployeeTO> GetNumberOfDaysVacationPerEmployees(DateTime dateTime) {
            try {
                return edao.getNumberOfDaysVacationPerEmployees(dateTime);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.GetNumberOfDaysVacationPerEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int BrZaposlenihDoOvogMeseca(DateTime mesec) {
            try {
                return edao.BrZaposlenih(mesec);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " Employee.BrZaposlenihDoOvogMeseca(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public int BrZaposlenihPeriodicno(DateTime fromTime, DateTime toTime)
        {
            try
            {
                return edao.BrZaposlenih(fromTime, toTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.BrZaposlenihDoOvogMeseca(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<int> getEmplIDsForBankHours(DateTime mesec)
        {
            try
            {
                return edao.getEmplIDsForBankHours(mesec);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.BrZaposlenihDoOvogMeseca(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeeTO> listaRadnikaZaWU(int wuID)
        {
            try
            {
                return edao.listaRadnikaZaWU(wuID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.listaRadnikaZaWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeeTO> listaRadnikaZaWU(string name)
        {
            try
            {
                return edao.listaRadnikaZaWU(name);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Employee.listaRadnikaZaWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
