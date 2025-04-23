using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using System.Text;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
    /// <summary>
    /// Managing IOPairs data
    /// </summary>
    public class IOPair
    {
        private DAOFactory daoFactory;
        private IOPairDAO ioPairDAO;

        DebugLog log;

        IOPairTO pairTO = new IOPairTO();

        // Hashtable that contains special and extra ordinaru employees
        // Key is employee_id, value is Employee
        Dictionary<int, EmployeeTO> specialEmployees = new Dictionary<int, EmployeeTO>();

        public IOPairTO PairTO
        {
            get { return pairTO; }
            set { pairTO = value; }
        }

        public IOPair()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            ioPairDAO = daoFactory.getIOPairDAO(null);
        }

        public IOPair(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            ioPairDAO = daoFactory.getIOPairDAO(dbConnection);
        }

        public IOPair(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                ioPairDAO = daoFactory.getIOPairDAO(null);
            }
        }

        public int Save()
        {
            int rowsInserted;

            try
            {
                rowsInserted = ioPairDAO.insert(this.PairTO.IOPairDate,
                    this.PairTO.EmployeeID,
                    this.PairTO.LocationID,
                    this.PairTO.IsWrkHrsCount,
                    this.PairTO.PassTypeID,
                    this.PairTO.StartTime,
                    this.PairTO.EndTime,
                    this.PairTO.GateID, //TAMARA 6.2.2020.
                    this.PairTO.ManualCreated,                    
                    true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Save(): employee " + PairTO.EmployeeID.ToString() + " start time " + PairTO.StartTime.ToString("dd.MM.yyyy HH:mm")
                    + " end time " + PairTO.EndTime.ToString("dd.MM.yyyy HH:mm") + " exception: " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return rowsInserted;
        }

        public int SaveIOPairs()
        {
            int rowsInserted;

            try
            {
                if (Misc.isLockedDate(this.PairTO.IOPairDate.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(this.PairTO.IOPairDate.Date);
                    throw new Exception(exceptionString);
                }
                rowsInserted = ioPairDAO.insert(this.PairTO.IOPairDate,
                    this.PairTO.EmployeeID,
                    this.PairTO.LocationID,
                    this.PairTO.IsWrkHrsCount,
                    this.PairTO.PassTypeID,
                    this.PairTO.StartTime,
                    this.PairTO.EndTime,
                    this.PairTO.GateID, //Tamara 6.2.2020.
                    this.PairTO.ManualCreated,
                    true);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Save(): employee " + PairTO.EmployeeID.ToString() + " start time " + PairTO.StartTime.ToString("dd.MM.yyyy HH:mm")
                    + " end time " + PairTO.EndTime.ToString("dd.MM.yyyy HH:mm") + " exception: " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return rowsInserted;
        }

        public int Save(int ioPairId,
            DateTime ioPairDate,
            int employeeId,
            int locationId,
            int passTypeId,
            int isWrkHrsCount,
            DateTime startTime,
            DateTime endTime,
            int gateID,
            int manualCreated,
            bool doCommit)
        {
            int rowsInserted;

            try
            {
                rowsInserted = ioPairDAO.insert(ioPairDate,
                    employeeId,
                    locationId,
                    isWrkHrsCount,
                    passTypeId,
                    startTime,
                    endTime,
                    gateID, //TAMARA 6.2.2020.
                    manualCreated,
                    doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Save(): employee " + employeeId.ToString() + " start time " + startTime.ToString("dd.MM.yyyy HH:mm")
                    + " end time " + endTime.ToString("dd.MM.yyyy HH:mm") + " exception: " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return rowsInserted;
        }

        public int SaveExtraHourPair(DateTime ioPairDate, int employeeId, int locationId,
            int isWrkHrsCount, int passTypeId, DateTime startTime, DateTime endTime,
            int manualCreated, bool doCommit)
        {
            int rowsInserted;

            try
            {
                if (Misc.isLockedDate(ioPairDate.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(ioPairDate.Date);
                    throw new Exception(exceptionString);
                }
                rowsInserted = ioPairDAO.insertExtraHourPair(ioPairDate, employeeId, locationId,
                    isWrkHrsCount, passTypeId, startTime, endTime, manualCreated, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Save(): employee " + employeeId.ToString() + " start time " + startTime.ToString("dd.MM.yyyy HH:mm")
                    + " end time " + endTime.ToString("dd.MM.yyyy HH:mm") + " exception: " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return rowsInserted;
        }

        public bool Update(int ioPairId,
            DateTime ioPairDate,
            int employeeId,
            int locationId,
            int passTypeId,
            int isWrkHrsCount,
            DateTime startTime,
            DateTime endTime,
            int manualCreated)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = ioPairDAO.update(ioPairId,
                    ioPairDate,
                    employeeId,
                    locationId,
                    passTypeId,
                    isWrkHrsCount,
                    startTime,
                    endTime,
                    manualCreated,
                    "");
            }
            catch (DataProcessingException dpex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + dpex.Message + "\n");
                if (dpex.Number != 12) throw new Exception(dpex.Message);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;

        }

        public bool Update(IOPairTO pairTO, string createdBy)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = ioPairDAO.update(pairTO.IOPairID,
                    pairTO.IOPairDate,
                    pairTO.EmployeeID,
                    pairTO.LocationID,
                    pairTO.PassTypeID,
                    pairTO.IsWrkHrsCount,
                    pairTO.StartTime,
                    pairTO.EndTime,
                    pairTO.ManualCreated,
                    createdBy);
            }
            catch (DataProcessingException dpex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + dpex.Message + "\n");
                log.writeLog(DateTime.Now + " IOPair.Update() Error for: IOPairID:" + pairTO.IOPairID.ToString() + " EmplID:" + pairTO.EmployeeID.ToString() + " IOPairDate:" + pairTO.IOPairDate.ToString("yyyy-MM-dd") + "\n");

                if (dpex.Number != 12) throw new Exception(dpex.Message);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + ex.Message + "\n");
                log.writeLog(DateTime.Now + " IOPair.Update() Error for: IOPairID:" + pairTO.IOPairID.ToString() + " EmplID:" + pairTO.EmployeeID.ToString() + " IOPairDate:" + pairTO.IOPairDate.ToString("yyyy-MM-dd") + "\n");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }
        public bool Update(bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                isUpdated = ioPairDAO.update(this.PairTO, doCommit);
            }
            catch (DataProcessingException dpex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + dpex.Message + "\n");
                log.writeLog(DateTime.Now + " IOPair.Update() Error for: IOPairID:" + pairTO.IOPairID.ToString() + " EmplID:" + pairTO.EmployeeID.ToString() + " IOPairDate:" + pairTO.IOPairDate.ToString("yyyy-MM-dd") + "\n");

                if (dpex.Number != 12) throw new Exception(dpex.Message);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + ex.Message + "\n");
                log.writeLog(DateTime.Now + " IOPair.Update() Error for: IOPairID:" + pairTO.IOPairID.ToString() + " EmplID:" + pairTO.EmployeeID.ToString() + " IOPairDate:" + pairTO.IOPairDate.ToString("yyyy-MM-dd") + "\n");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool UpdateIOPairs(IOPairTO pairTO, string createdBy)
        {
            bool isUpdated = false;

            try
            {
                if (Misc.isLockedDate(pairTO.IOPairDate.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(pairTO.IOPairDate.Date);
                    throw new Exception(exceptionString);
                }
                isUpdated = ioPairDAO.update(pairTO.IOPairID,
                    pairTO.IOPairDate,
                    pairTO.EmployeeID,
                    pairTO.LocationID,
                    pairTO.PassTypeID,
                    pairTO.IsWrkHrsCount,
                    pairTO.StartTime,
                    pairTO.EndTime,
                    pairTO.ManualCreated,
                    createdBy);
            }
            catch (DataProcessingException dpex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + dpex.Message + "\n");
                log.writeLog(DateTime.Now + " IOPair.Update() Error for: IOPairID:" + pairTO.IOPairID.ToString() + " EmplID:" + pairTO.EmployeeID.ToString() + " IOPairDate:" + pairTO.IOPairDate.ToString("yyyy-MM-dd") + "\n");

                if (dpex.Number != 12) throw new Exception(dpex.Message);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Update(): " + ex.Message + "\n");
                log.writeLog(DateTime.Now + " IOPair.Update() Error for: IOPairID:" + pairTO.IOPairID.ToString() + " EmplID:" + pairTO.EmployeeID.ToString() + " IOPairDate:" + pairTO.IOPairDate.ToString("yyyy-MM-dd") + "\n");

                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        // TODO: To be implemented
        public bool Delete(int ioPairID, DateTime ioPairDate)
        {
            bool isDeleted = false;

            try
            {
                if (Misc.isLockedDate(ioPairDate.Date))
                {
                    string exceptionString = Misc.dataLockedMessage(ioPairDate.Date);
                    throw new Exception(exceptionString);
                }

                isDeleted = ioPairDAO.delete(ioPairID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool Delete(string IOPairID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = ioPairDAO.delete(IOPairID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public bool Delete(string IOPairID, bool doCommit)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = ioPairDAO.delete(IOPairID, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public IOPairTO Find(int ioPairID)
        {
            IOPairTO pairTO = new IOPairTO();

            try
            {
                pairTO = ioPairDAO.find(ioPairID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return pairTO;
        }

        public void SearchEmplOpenPairs(DateTime fromDate, Dictionary<int, Dictionary<DateTime, IOPairTO>> emplStartOpenPairs,
            Dictionary<int, Dictionary<DateTime, IOPairTO>> emplEndOpenPairs, ref string emplIDs, List<DateTime> dateList)
        {
            try
            {
                ioPairDAO.getEmplOpenPairs(fromDate, emplStartOpenPairs, emplEndOpenPairs, ref emplIDs, dateList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmplOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchClosedPairs()
        {
            try
            {
                return ioPairDAO.getIOPairsClosed(this.PairTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchClosedPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchClosedPairs(DateTime fromTime, DateTime toTime)
        {
            try
            {
                return ioPairDAO.getIOPairsClosed(this.PairTO, fromTime, toTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchClosedPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> Search()
        {
            try
            {
                return ioPairDAO.getIOPairs(this.PairTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void Clear()
        {
            PairTO = new IOPairTO();
        }

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = ioPairDAO.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                ioPairDAO.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                ioPairDAO.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return ioPairDAO.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                ioPairDAO.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Arrange all passes by employee and by date.
        /// Create list of passes for one Employee by one day. 
        /// </summary>
        public void ClassifyPasses(Dictionary<int, EmployeeTO> specialEmpl, Dictionary<int, EmployeeTO> extraOrdEmpl, NotificationController Controller, Dictionary<int, WorkTimeSchemaTO> dictTimeSchema)
        {
            Pass pass = new Pass();
            try
            {
                // First insert records for Employees that have been whole day absent
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Insert whole day absences: STARTED!");
                log.writeBenchmarkLog(" ClassifyPasses(): insert whole day absences: STARTED! +++++\n");

                insertWholeDayAbsences(dictTimeSchema);

                log.writeBenchmarkLog(" ClassifyPasses(): insert whole day absences: FINISHED! +++++\n");
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Insert whole day absences: FINISHED!");
                //log.writeBenchmarkLog(" ClassifyPasses(): get employees: STARTED! +++++\n");

                specialEmployees = specialEmpl;
                // 27.06.2013. Sanja - only SPECIAL employees are treated in old way
                // for EXTRAORDINARY employees, only absence is changed to regular work at the end
                //foreach (int key in extraOrdEmpl.Keys)
                //{
                //    if (!specialEmployees.ContainsKey(key))
                //        specialEmployees.Add(key, extraOrdEmpl[key]);
                //}
                // 27.06.2013. Sanja

                // get employees and distinct days they were worked from unused passes instead of
                // employees and distinct days they were worked                
                // get all unused passes
                ArrayList emplByDay = pass.getEmployeesByDate();

                //log.writeBenchmarkLog(" ClassifyPasses(): get employees: FINISHED! +++++\n");
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Processing passes: STARTED!");
                log.writeBenchmarkLog(" ClassifyPasses(): update io pairs passes: STARTED! +++++\n");

                List<PassTO> allPasessForEmpl = new List<PassTO>();
                int counter = 0;
                foreach (ArrayList emplDate in emplByDay)
                {
                    counter++;
                    if (Controller != null)
                        Controller.DataProcessingStateChanged(" Classifying Passes - Processing passes: " + counter.ToString() + "/" + emplByDay.Count.ToString());
                    // update passes for this employee and date as unused and
                    // delete io pairs for this employee and this day, all in same transaction
                    bool isUpdated = ioPairDAO.updateIOPairsPasses((int)emplDate[0], emplDate[1].ToString(), daoFactory);

                    if (isUpdated)
                    {
                        // process unused passes
                        allPasessForEmpl = pass.getPassesForEmployee((int)emplDate[0], emplDate[1].ToString());
                        //ProcessingPasses(allPasessForEmpl, DateTime.Parse(emplDate[1].ToString(), new CultureInfo("en-US"), DateTimeStyles.NoCurrentDateDefault), dictTimeSchema);
                        
                        //MM 06.09.2017

                        DateTime datum;//= DateTime.Parse(emplDate[1].ToString());
                        try
                        {
                            datum = DateTime.ParseExact(emplDate[1].ToString(), "MM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                            
                        if(DateTime.TryParse(emplDate[1].ToString(),out datum))
                        {
                           
                            ProcessingPasses(allPasessForEmpl, datum, dictTimeSchema);
                        }
                         
                        //mm
                        //mm ja komentarisao ProcessingPasses(allPasessForEmpl,DateTime.Parse(emplDate[1].ToString()+ "00:00:00"), dictTimeSchema);
                    }
                }

                log.writeBenchmarkLog(" ClassifyPasses(): update io pairs passes: FINISHED! +++++\n");
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Processing passes: FINISHED!");

                // Process pairs that are derived from passes that are derived from start working day exit permissions
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Permission passes processing: STARTED!");
                log.writeBenchmarkLog(" ClassifyPasses(): permission passes processing: STARTED! +++++\n");

                permissionPassesProcessing();

                log.writeBenchmarkLog(" ClassifyPasses(): permission passes processing: FINISHED! +++++\n");
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Permission passes processing: FINISHED!");

                // auto close opened pairs
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Auto close pairs: STARTED!");
                log.writeBenchmarkLog(" ClassifyPasses(): auto close pairs: STARTED! +++++\n");

                autoClosePairs(dictTimeSchema);

                log.writeBenchmarkLog(" ClassifyPasses(): auto close pairs: FINISHED! +++++\n");
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Auto close pairs: FINISHED!");

                // auto close opened pairs with special out started
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Auto close special out: STARTED!");
                log.writeBenchmarkLog(" ClassifyPasses(): auto close special out: STARTED! +++++\n");

                autoCloseSpecialOut(dictTimeSchema);

                log.writeBenchmarkLog(" ClassifyPasses(): auto close special out: FINISHED! +++++\n");
                if (Controller != null)
                    Controller.DataProcessingStateChanged(" Classifying Passes - Auto close special out: FINISHED!");

                // calculate and insert pauses for employees and dates that need recalculating
                if ((new TimeSchema()).SearchPauses() > 0)
                {
                    if (Controller != null)
                        Controller.DataProcessingStateChanged(" Classifying Passes - Processing pauses: STARTED!");
                    log.writeBenchmarkLog(" ClassifyPasses(): processing pauses: STARTED! +++++\n");

                    processingPauses(emplByDay, dictTimeSchema);

                    log.writeBenchmarkLog(" ClassifyPasses(): processing pauses: FINISHED! +++++\n");
                    if (Controller != null)
                        Controller.DataProcessingStateChanged(" Classifying Passes - Processing pauses: FINISHED!");
                }
                else
                    log.writeBenchmarkLog(" ClassifyPasses(): no need for processing pauses! +++++\n");
            }
            catch (Exception ex)
            {
                if (ex is Util.DataProcessingException)
                {
                    DataProcessingException pdex = (DataProcessingException)ex;

                    if (pdex.Number.Equals(12))
                    {

                        DataProcessingException pde = new DataProcessingException(pdex.message, 12);
                        log.writeLog(DateTime.Now + " Exception in: "
                            + this.ToString() + ".ProcessingPasses() : \n "
                            + pdex.message + pdex.TrackMessage(2) + "\n");
                    }
                    else
                    {
                        log.writeLog(DateTime.Now + " Exception in: "
                            + this.ToString() + ".ProcessingPasses() : \n "
                            + pdex.message + pdex.TrackMessage(2) + "\n");
                    }
                }
                else
                {
                    log.writeLog(DateTime.Now + " IOPair.ClassifyPasses(): " + ex.Message + "\n");
                    throw ex;
                }
            }
        }




        private void ProcessingPasses(List<PassTO> passList, DateTime date, Dictionary<int, WorkTimeSchemaTO> timeSchema)
        {
            PassTO pairPass = new PassTO();
            WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
            int dayNum = 0;
            ArrayList emplSchema = new ArrayList();
            // Key is interval number, value is boolean
            Hashtable flags = new Hashtable();

            Dictionary<int, WorkTimeIntervalTO> intervals = new Dictionary<int, WorkTimeIntervalTO>();
            // get first pass for that employee and day that is before pair start time

            TimeSpan intervalsDuration = new TimeSpan();
            // find start and end time of working day
            DateTime endTime = new DateTime(0);
            DateTime latestEndTime = new DateTime(0);
            DateTime earliestStartTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
            DateTime startTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);

            if (passList.Count > 0)
            {
                Employee empl = new Employee();
                empl.EmplTO.EmployeeID = passList[0].EmployeeID;
                // if employee is special or extraordinary

                if (specialEmployees.ContainsKey(passList[0].EmployeeID))
                {
                    emplSchema = empl.findTimeSchema(date);
                    // find his time schema for pass date
                    empl.EmplTO = specialEmployees[passList[0].EmployeeID];
                    ArrayList schemaDay = empl.findTimeSchema(passList[0].EventTime, timeSchema);
                    
                    if (schemaDay.Count > 1)
                    {
                        //Boris, test1 IMLEK
                        //EmployeesTimeSchedule ets = new EmployeesTimeSchedule();
                        //ets.Update(passList[0].EmployeeID, passList[0].EventTime, 1, 1);

                        schema = (WorkTimeSchemaTO)schemaDay[0];
                        dayNum = (int)schemaDay[1];

                        intervals = schema.Days[dayNum];

                        foreach (int intNum in intervals.Keys)
                        {
                            intervalsDuration += intervals[intNum].EndTime.TimeOfDay - intervals[intNum].StartTime.TimeOfDay;
                            flags.Add(intNum, false);

                            if (intervals[intNum].EarliestArrived.TimeOfDay < earliestStartTime.TimeOfDay)
                            {
                                earliestStartTime = intervals[intNum].EarliestArrived;
                            }

                            if (intervals[intNum].StartTime.TimeOfDay < startTime.TimeOfDay)
                            {
                                startTime = intervals[intNum].StartTime;
                            }

                            if (intervals[intNum].EndTime.TimeOfDay > endTime.TimeOfDay)
                            {
                                endTime = intervals[intNum].EndTime;
                            }

                            if (intervals[intNum].LatestLeft.TimeOfDay > latestEndTime.TimeOfDay)
                            {
                                latestEndTime = intervals[intNum].LatestLeft;
                            }
                        }
                    }
                }
            }

            try
            {
                int currentIndex = 0;
                foreach (PassTO currentPass in passList)
                {
                    if (currentPass.Direction.Equals(Constants.DirectionIn))
                    {
                        // if employee is special or extraordinary
                        if (specialEmployees.ContainsKey(currentPass.EmployeeID))
                        {
                            foreach (int intNum in intervals.Keys)
                            {
                                WorkTimeIntervalTO interval = intervals[intNum];

                                if (flags.ContainsKey(intNum) && !((bool)flags[intNum]))
                                {
                                    // if pass is during the interval, create official out pair (start_time, event_time)
                                    if (currentPass.EventTime.TimeOfDay > interval.LatestArrivaed.TimeOfDay
                                        && currentPass.EventTime.TimeOfDay < interval.EarliestLeft.TimeOfDay)
                                    {
                                        this.Clear();
                                        this.PairTO.IOPairDate = currentPass.EventTime.Date;
                                        this.PairTO.EmployeeID = currentPass.EmployeeID;
                                        this.PairTO.LocationID = currentPass.LocationID;
                                        this.PairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                                        this.PairTO.PassTypeID = Constants.officialOut;
                                        //this.StartTime = new DateTime(this.IOPairDate.Year, this.IOPairDate.Month,
                                        //    this.IOPairDate.Day, interval.StartTime.Hour, interval.StartTime.Minute,
                                        //    interval.StartTime.Second);
                                        //30.04.2010 Natasa create out for last_arrived_time
                                        this.PairTO.StartTime = new DateTime(this.PairTO.IOPairDate.Year, this.PairTO.IOPairDate.Month,
                                            this.PairTO.IOPairDate.Day, interval.LatestArrivaed.Hour, interval.LatestArrivaed.Minute,
                                            interval.LatestArrivaed.Second);
                                        this.PairTO.EndTime = currentPass.EventTime;
                                        this.PairTO.ManualCreated = 0;
                                        this.PairTO.GateID = currentPass.GateID; //TAMARA 6.2.2020.
                                        this.Save();

                                        //// set corresponding flag for his interval on true
                                        // flags[intNum] = true;	// DC 2007.03.09. eo user in auto close issue
                                    }
                                    // set corresponding flag for his interval on true
                                    flags[intNum] = true;	// DC 2007.03.09. eo user in auto close issue
                                }
                            }
                        }
                        //Tamara 30.1.2020.
                        pairPass = findPass(currentIndex, currentPass.IsWrkHrsCount, currentPass.LocationID, passList, currentPass.GateID);

                        if (pairPass.Direction.Equals(Constants.DirectionOut))
                        {
                            GenerateValidPair(currentPass, pairPass, date);
                        }
                        else if ((pairPass.Direction.Equals(Constants.DirectionIn)) ||
                            (pairPass.PassID == -1))
                        {
                            GenerateStartOpenPair(currentPass, date);
                        }
                    }
                    else if (currentPass.Direction.Equals(Constants.DirectionOut))
                    {

                        if (currentPass.PairGenUsed == (int)Constants.PassGenUsed.Unused)
                        {
                            GenerateEndOpenPair(currentPass, date);
                        }

                        if (currentPass.PassTypeID != (int)Constants.PassType.Work)
                        {
                            if (currentIndex + 1 < passList.Count)
                            {
                                // find next pass
                                //pairPass = (PassTO)passList[currentIndex + 1];

                                // find next pass which is working hours count
                                pairPass = new PassTO();

                                int index = currentIndex + 1;
                                bool pairFound = false;

                                while (index < passList.Count)
                                {
                                    pairPass = (PassTO)passList[index];

                                    if (pairPass.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                    {
                                        pairFound = true;
                                        break;
                                    }

                                    index++;
                                }

                                if (pairFound)
                                {
                                    if (pairPass.Direction.Equals(Constants.DirectionIn))
                                    {
                                        GenerateValidPair(currentPass, pairPass, date);
                                    }
                                    else if (pairPass.Direction.Equals(Constants.DirectionOut))
                                    {
                                        GenerateStartOpenPair(currentPass, date);
                                    }
                                    else
                                    {
                                        // TODO:
                                    }
                                }
                                else
                                {
                                    GenerateStartOpenPair(currentPass, date);
                                }
                            }
                            else
                            {
                                GenerateStartOpenPair(currentPass, date);
                            }
                        }
                        //30.04.2010. Natasa
                        //if out is regular work and special employee add official out if event time is before end of shift
                        else if (specialEmployees.ContainsKey(currentPass.EmployeeID))//15.07.2011 add condition that employee has to be special
                        {
                            //shift end time
                            DateTime officileLimit = new DateTime();
                            //03.10.2011 Natasa if empolyee time shema is flexy considare first pass to calculate end shift time
                            if (emplSchema.Count > 0 && ((WorkTimeSchemaTO)emplSchema[0]).Type.Trim() == Constants.schemaTypeFlexi)
                            {
                                // get first pass for that employee and day that is before pair start time
                                PassTO pass = new Pass().SearchPassBeforePermissionPass(currentPass.EmployeeID, currentPass.EventTime);

                                if (pass.PassID != -1)
                                {
                                    if (pass.EventTime.TimeOfDay < earliestStartTime.TimeOfDay)
                                        officileLimit = earliestStartTime.AddMinutes(intervalsDuration.TotalMinutes);
                                    else if (pass.EventTime.AddMinutes(intervalsDuration.TotalMinutes).TimeOfDay > latestEndTime.TimeOfDay)
                                        officileLimit = latestEndTime;
                                    else
                                        officileLimit = pass.EventTime.AddMinutes(intervalsDuration.TotalMinutes);
                                }
                                else
                                {
                                    officileLimit = endTime;
                                }
                            }
                            //03.10.2011 Natasa if employee time shema is normal no need to calculate end time
                            else
                            {
                                officileLimit = endTime;
                            }
                            if (currentPass.EventTime.TimeOfDay >= officileLimit.TimeOfDay)
                            {
                                if (specialEmployees.ContainsKey(currentPass.EmployeeID))
                                {
                                    currentIndex++;
                                }
                                continue;
                            }

                            if (currentIndex + 1 < passList.Count)
                            {
                                // find next pass
                                //pairPass = (PassTO)passList[currentIndex + 1];

                                // find next pass which is working hours count
                                pairPass = new PassTO();

                                int index = currentIndex + 1;
                                bool pairFound = false;

                                while (index < passList.Count)
                                {
                                    pairPass = (PassTO)passList[index];

                                    if (pairPass.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter)
                                    {
                                        pairFound = true;
                                        break;
                                    }

                                    index++;
                                }

                                if (pairFound)
                                {
                                    if (pairPass.Direction.Equals(Constants.DirectionIn))
                                    {
                                        GenerateValidPair(currentPass, pairPass, date);
                                    }
                                    else if (pairPass.Direction.Equals(Constants.DirectionOut))
                                    {
                                        currentPass.PassTypeID = Constants.officialOut;
                                        GenerateStartOpenPair(currentPass, date);
                                    }
                                    else
                                    {
                                        // TODO:
                                    }
                                }
                                else
                                {
                                    currentPass.PassTypeID = Constants.officialOut;
                                    GenerateStartOpenPair(currentPass, date);
                                }
                            }
                            else
                            {
                                currentPass.PassTypeID = Constants.officialOut;

                                GenerateStartOpenPair(currentPass, date);
                            }
                        }
                        //30.04.2010. Natasa
                    }
                    else
                    {
                        // TODO:
                    }

                    currentIndex++;
                }
            }
            catch (Exception ex)
            {
                if (ex is Util.DataProcessingException)
                {
                    DataProcessingException pdex = (DataProcessingException)ex;
                    if (pdex.Number.Equals(12))
                    {
                        throw ex;
                    }
                    else
                    {
                        DataProcessingException pde = new DataProcessingException(pdex.message, 13);
                        log.writeLog(DateTime.Now + " Exception in: "
                            + this.ToString() + ".ProcessingPasses() : \n "
                            + pde.message + pdex.TrackMessage(2) + "\n");
                    }
                }
                else
                {
                    DataProcessingException pde = new DataProcessingException(ex.Message, 11);
                    log.writeLog(DateTime.Now + " Exception in: "
                        + this.ToString() + ".ProcessingPasses() : \n "
                        + pde.message + pde.TrackMessage(1) + "\n");
                    throw ex;
                }
            }
        }

        private PassTO findPass(int currentPassIndex, int isWrkHrsCount, int locationID, List<PassTO> currentPassEmplList, int gateID)
        {
            PassTO pairPass = new PassTO();

            try
            {
                if (currentPassIndex + 1 < currentPassEmplList.Count)
                {
                    for (int i = currentPassIndex + 1; i < currentPassEmplList.Count; i++)
                    {
                        // TAMARA 17.4.2018. dodala uslov za isWrkHrsCount
                        if (currentPassEmplList[i].LocationID == locationID && currentPassEmplList[i].IsWrkHrsCount == isWrkHrsCount)
                        {
                            pairPass = currentPassEmplList[i];
                            return pairPass;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.findPair(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return pairPass;
        }

        private void GenerateStartOpenPair(PassTO passTOFirst, DateTime date)
        {
            try
            {
                this.Clear();

                this.PairTO.IOPairDate = date;
                this.PairTO.EmployeeID = passTOFirst.EmployeeID;
                this.PairTO.LocationID = passTOFirst.LocationID;
                this.PairTO.IsWrkHrsCount = passTOFirst.IsWrkHrsCount;
                this.PairTO.PassTypeID = passTOFirst.PassTypeID;
                this.PairTO.StartTime = passTOFirst.EventTime;
                this.PairTO.ManualCreated = 0;
                this.PairTO.GateID = passTOFirst.GateID; //Tamara 6.2.2020.

                passTOFirst.PairGenUsed = (int)Constants.PairGenUsed.Used;
                this.PairTO.ProcessedGenUsed = (int)Constants.PairGenUsed.Used; //////////// tamara 27.6.2018.

                try
                {
                    this.ioPairDAO.insert(this.PairTO, passTOFirst, new PassTO(), this.daoFactory);
                }
                catch (Exception ex)
                {
                    // Check is it DataProcessingException 
                    if (ex is DataProcessingException)
                    {
                        DataProcessingException dataProcEx = (DataProcessingException)ex;

                        if (dataProcEx.Number.Equals(12))
                        {
                            log.writeLog(DateTime.Now + " IOPair.GenerateValidPair(): " + dataProcEx.message + "\n"
                                + dataProcEx.TrackMessage(2) + "\n");
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GenerateStartOpenPair(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void GenerateEndOpenPair(PassTO passTOFirst, DateTime date)
        {
            try
            {
                this.Clear();

                this.PairTO.IOPairDate = date;
                this.PairTO.EmployeeID = passTOFirst.EmployeeID;
                this.PairTO.LocationID = passTOFirst.LocationID;
                this.PairTO.IsWrkHrsCount = passTOFirst.IsWrkHrsCount;
                this.PairTO.PassTypeID = passTOFirst.PassTypeID;
                this.PairTO.EndTime = passTOFirst.EventTime;
                this.PairTO.ManualCreated = 0;
                this.PairTO.GateID = passTOFirst.GateID; //Tamara 6.2.2020.

                passTOFirst.PairGenUsed = (int)Constants.PairGenUsed.Used;
                this.PairTO.ProcessedGenUsed = (int)Constants.PairGenUsed.Used; //////////// tamara 27.6.2018.

                try
                {
                    ioPairDAO.insert(this.PairTO, new PassTO(), passTOFirst, this.daoFactory);
                }
                catch (Exception ex)
                {
                    // Check is it DataProcessingException 
                    if (ex is DataProcessingException)
                    {
                        DataProcessingException dataProcEx = (DataProcessingException)ex;

                        if (dataProcEx.Number.Equals(12))
                        {
                            log.writeLog(DateTime.Now + " IOPair.GenerateValidPair(): " + dataProcEx.message + "\n"
                                + dataProcEx.TrackMessage(2) + "\n");
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GenerateEndOpenPair(): " + ex.Message + "\n");
                throw ex;
            }

        }

        private void GenerateValidPair(PassTO passTOFirst, PassTO passTOSecond, DateTime date)
        {
            try
            {
                this.Clear();

                this.PairTO.IOPairDate = date;
                this.PairTO.EmployeeID = passTOFirst.EmployeeID;
                this.PairTO.LocationID = passTOFirst.LocationID;

                if ((passTOFirst.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter) ||
                    (passTOSecond.IsWrkHrsCount == (int)Constants.IsWrkCount.IsCounter))
                {
                    this.PairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                }
                else
                {
                    this.PairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsNotCounter;
                    this.PairTO.ProcessedGenUsed = 1; // tamara 30.4.2018 postavlja se na 
                    //1 jer nema potrebe da se dalje procesira
                }

                this.PairTO.PassTypeID = passTOFirst.PassTypeID;
                this.PairTO.StartTime = passTOFirst.EventTime;
                this.PairTO.EndTime = passTOSecond.EventTime;
                this.PairTO.ManualCreated = 0;
                this.PairTO.GateID = passTOSecond.GateID; //Tamara 6.2.2020.

                passTOFirst.PairGenUsed = (int)Constants.PairGenUsed.Used;
                passTOSecond.PairGenUsed = (int)Constants.PairGenUsed.Used;

                try
                {
                    ioPairDAO.insert(this.PairTO, passTOFirst, passTOSecond, this.daoFactory);
                }
                catch (Exception ex)
                {
                    // Check is it DataProcessingException 
                    if (ex is DataProcessingException)
                    {
                        DataProcessingException dataProcEx = (DataProcessingException)ex;

                        if (dataProcEx.Number.Equals(12))
                        {
                            log.writeLog(DateTime.Now + " IOPair.GenerateValidPair(): " + dataProcEx.message + "\n"
                                + dataProcEx.TrackMessage(2) + "\n");
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GenerateValidPair(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<IOPairTO> SearchForWU(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsForWU(workingUnitID, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchNotInWUCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsNotInWUCount(workingUnitID, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForWUCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchNotInWUDate(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsNotInWUDate(workingUnitID, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForWUDate(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate, int locID, int isWrkHrs)
        {
            try
            {
                return ioPairDAO.getIOPairsForWUDate(workingUnitID, wUnits, fromDate, toDate, locID, isWrkHrs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForWU(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchForWUCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsForWUCount(workingUnitID, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForWUCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForWUWrkHrs(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsForWUWrkHrs(workingUnitID, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForWUWrkHrs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchForWUWrkHrsCount(int workingUnitID, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsForWUWrkHrsCount(workingUnitID, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForWUWrkHrsCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForExtraHours(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsForExtraHours(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForExtraHours(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchAll(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsAll(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchEmplDate(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplDate(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmplDate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchWholeDayAbsenceIOPairs(List<int> employeesList, List<int> passTypes, int year)
        {
            try
            {
                return ioPairDAO.getWholeDayAbsenceIOPairs(employeesList, passTypes, year);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmplDate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchEmplDateWithOpenPairs(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID, int isWrkHrs)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplDateWithOpenPairs(fromDate, toDate, employeesList, locationID, isWrkHrs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmplDateWithOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchEmplDate(DateTime fromDate, DateTime toDate, List<int> employeesList, bool createDAO)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplDate(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmplDate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchEmplDateCount(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplDateCount(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmplDateCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchAllEmplDatePairs(string employeeIDString, List<DateTime> datesList)
        {
            try
            {
                return ioPairDAO.getIOPairsAllEmplDatePairs(employeeIDString, datesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchAllPairsForEmpl(string employeeIDString, List<DateTime> datesList, int locationID, int isWrkHrs)
        {
            try
            {
                return ioPairDAO.getIOPairsAllForEmpl(employeeIDString, datesList, locationID, isWrkHrs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchEmplTimeInterval(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            try
            {
                return ioPairDAO.getIOPairsEmplTimeInterval(employeeIDString, fromDate, toDate, trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchAllEmplDatePairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchEmplTimeInterval2(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            try
            {
                return ioPairDAO.getIOPairsEmplTimeInterval2(employeeIDString, fromDate, toDate, trans);
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchAllEmplDatePairs2(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }



        public List<IOPairTO> SearchLocation(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsForLoc(locationID, locations, wUnits, fromDate, toDate);
                //ioPairDAO.serialize(arrList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchLocation(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchLocationCount(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsForLocCount(locationID, locations, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchLocationCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public DataSet SearchLocationDataSet(int locationID, string locations, string wUnits, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return ioPairDAO.getIOPairsForLocDataSet(locationID, locations, wUnits, fromDate, toDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchLocation(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForEmployees(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmpl(fromDate, toDate, employeesList, locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForEmpl(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmployees(fromDate, toDate, employeesList, locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmpl(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForEmployeesLocation(DateTime fromDate, DateTime toDate, List<int> employeesList, string locationsID)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplLoc(fromDate, toDate, employeesList, locationsID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchForEmployeesCount(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplCount(fromDate, toDate, employeesList, locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchForEmployeesLocCount(DateTime fromDate, DateTime toDate, List<int> employeesList, string locationsID)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplLocCount(fromDate, toDate, employeesList, locationsID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForEmployeesWrkHrs(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplWrkHrs(fromDate, toDate, employeesList, locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployeesWrkHrs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchForEmployeesWrkHrsCount(DateTime fromDate, DateTime toDate, List<int> employeesList, int locationID)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplWrkHrsCount(fromDate, toDate, employeesList, locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployeesWrkHrsCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<DateTime> SearchDatesWithOpenPairs(DateTime fromDate, DateTime toDate, int employeeID)
        {
            try
            {
                return ioPairDAO.getDatesWithOpenPairs(fromDate, toDate, employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchDatesWithOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<DateTime> SearchDatesWithOpenPairsWrkHrs(DateTime fromDate, DateTime toDate, int employeeID)
        {
            try
            {
                return ioPairDAO.getDatesWithOpenPairsWrkHrs(fromDate, toDate, employeeID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchDatesWithOpenPairsWrkHrs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> Search(DateTime fromDate, DateTime toDate, string wUnits, int wuID)
        {
            try
            {
                return ioPairDAO.getIOPairs(this.PairTO, fromDate, toDate, wUnits, wuID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.getIOPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchCountPairs(DateTime fromDate, DateTime toDate, string wUnits, int wuID)
        {
            try
            {
                return ioPairDAO.getPairsCount(this.PairTO, fromDate, toDate, wUnits, wuID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchCountPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool ExistEmlpoyeeDatePair(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime)
        {
            try
            {
                return ioPairDAO.existEmlpoyeeDatePair(employeeID, ioPairDate, startTime, endTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.ExistEmlpoyeeDatePair(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool ExistEmlpoyeeDatePairNotWholeDayAbsences(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime)
        {
            try
            {
                return ioPairDAO.existEmlpoyeeDatePairNotWholeDayAbsences(employeeID, ioPairDate, startTime, endTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.ExistEmlpoyeeDatePairNotWholeDayAbsences(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void insertWholeDayAbsences(Dictionary<int, WorkTimeSchemaTO> timeSchemaDict)
        {
            try
            {
                DateTime date = DateTime.Today.Date;
                DateTime startTime = new DateTime(0);
                DateTime endTime = new DateTime(0);
                int i;

                // Get all whole day absences that are unused
                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.Used = (int)Constants.Used.No;
                List<EmployeeAbsenceTO> wholeDayAbsList = ea.Search("");

                foreach (EmployeeAbsenceTO emplAbs in wholeDayAbsList)
                {
                    date = emplAbs.DateStart.Date;
                    List<IOPairTO> absPairs = new List<IOPairTO>();

                    while (date.Date <= emplAbs.DateEnd.Date)
                    {
                        // if there is no Time Schema defined for current day, Start and End Time will be 00:00:00
                        startTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        endTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

                        // get Time Schedule for current month
                        EmployeeTimeScheduleTO emplTimeSchedule = new EmployeeTimeScheduleTO();
                        List<EmployeeTimeScheduleTO> emplTimeScheduleList = new EmployeesTimeSchedule().SearchMonthSchedule(emplAbs.EmployeeID, date.Date);

                        // find Time Shema for current day
                        i = 0;
                        while ((i < emplTimeScheduleList.Count) && (emplTimeScheduleList[i].Date <= date))
                        {
                            i++;
                        }

                        if (i > 0)
                        {
                            emplTimeSchedule = emplTimeScheduleList[i - 1];
                        }

                        List<WorkTimeSchemaTO> timeSchemaList = new List<WorkTimeSchemaTO>();

                        if (emplTimeSchedule.TimeSchemaID >= 0)
                        {
                            if (timeSchemaDict.ContainsKey(emplTimeSchedule.TimeSchemaID))
                            {
                                timeSchemaList.Add(timeSchemaDict[emplTimeSchedule.TimeSchemaID]);
                            }
                        }

                        int cycleDuration = 0;
                        int day = 0;
                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        Dictionary<int, WorkTimeIntervalTO> schemaIntervals = new Dictionary<int, WorkTimeIntervalTO>();
                        Dictionary<int, WorkTimeIntervalTO> schemaIntervalsPrevious = new Dictionary<int, WorkTimeIntervalTO>();
                        Dictionary<int, WorkTimeIntervalTO> schemaIntervalsNext = new Dictionary<int, WorkTimeIntervalTO>();
                        int dayPrevious = 0;
                        int dayNext = 0;
                        TimeSpan tsPrevious = new TimeSpan();
                        TimeSpan tsNext = new TimeSpan();
                        WorkTimeIntervalTO interval = new WorkTimeIntervalTO();

                        // if there is Time Schema defined for current day
                        if (timeSchemaList.Count > 0)
                        {
                            schema = timeSchemaList[0];
                            cycleDuration = schema.CycleDuration;
                            /* 2008-03-14
                             * From now one, take the last existing time schedule, don't expect that every month has 
                             * time schedule*/
                            //day = (emplTimeSchedule.StartCycleDay + date.Day - emplTimeSchedule.Date.Day) % cycleDuration;
                            TimeSpan ts = new TimeSpan(date.Date.Ticks - emplTimeSchedule.Date.Date.Ticks);
                            day = (emplTimeSchedule.StartCycleDay + (int)ts.TotalDays) % cycleDuration;
                            schemaIntervals = schema.Days[day];

                            if (date.Date == emplAbs.DateStart.Date)
                            {
                                tsPrevious = new TimeSpan(date.Date.AddDays(-1).Ticks - emplTimeSchedule.Date.Date.Ticks);
                                dayPrevious = (emplTimeSchedule.StartCycleDay + (int)tsPrevious.TotalDays) % cycleDuration;
                                if (dayPrevious == -1)
                                    dayPrevious = cycleDuration - 1;
                                schemaIntervalsPrevious = schema.Days[dayPrevious];
                            }

                            if (date.Date == emplAbs.DateEnd.Date)
                            {
                                tsNext = new TimeSpan(date.Date.AddDays(1).Ticks - emplTimeSchedule.Date.Date.Ticks);
                                dayNext = (emplTimeSchedule.StartCycleDay + (int)tsNext.TotalDays) % cycleDuration;
                                if (dayNext == -1)
                                {
                                    dayNext = cycleDuration - 1;
                                }
                                schemaIntervalsNext = schema.Days[dayNext];
                            }

                            // if current day has more intervals for defined Time Schema, record is inserted
                            // to io_pairs table foreach interval
                            foreach (int key in schemaIntervals.Keys)
                            {
                                interval = schemaIntervals[key];
                                startTime = new DateTime(date.Year, date.Month, date.Day, interval.StartTime.Hour, interval.StartTime.Minute, interval.StartTime.Second);
                                endTime = new DateTime(date.Year, date.Month, date.Day, interval.EndTime.Hour, interval.EndTime.Minute, interval.EndTime.Second);

                                // interval 0-7h from night shift of absence starting day is not absence!
                                if (date.Date == emplAbs.DateStart.Date && interval.StartTime.Hour == 0 && interval.StartTime.Minute == 0)
                                {
                                    bool isNightShift = false;
                                    foreach (int keyPrevious in schemaIntervalsPrevious.Keys)
                                    {
                                        WorkTimeIntervalTO intervalPrevious = schemaIntervalsPrevious[keyPrevious];

                                        if (intervalPrevious.EndTime.Hour == 23 && intervalPrevious.EndTime.Minute == 59)
                                        {
                                            isNightShift = true;
                                            break;
                                        }
                                    }

                                    if (isNightShift)
                                        continue;
                                }

                                try
                                {
                                    IOPairTO pairTO = new IOPairTO();
                                    pairTO.IOPairDate = date.Date;
                                    pairTO.EmployeeID = emplAbs.EmployeeID;
                                    pairTO.LocationID = Constants.defaultLocID;
                                    pairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                                    pairTO.PassTypeID = emplAbs.PassTypeID;
                                    pairTO.StartTime = startTime;
                                    pairTO.EndTime = endTime;
                                    pairTO.ManualCreated = (int)Constants.recordCreated.Automaticaly;

                                    absPairs.Add(pairTO);
                                }
                                catch (Exception ex)
                                {
                                    // Check is it DataProcessingException 
                                    if (ex is DataProcessingException)
                                    {
                                        DataProcessingException dataProcEx = (DataProcessingException)ex;

                                        if (dataProcEx.Number.Equals(12))
                                        {
                                            log.writeLog(DateTime.Now + " IOPair.GenerateValidPair(): " + dataProcEx.message + "\n");
                                        }
                                        else
                                        {
                                            throw ex;
                                        }
                                    }
                                    else
                                    {
                                        throw ex;
                                    }
                                }

                                // interval 0-7h from night shift of absence end day + 1 is absence!

                                if (date.Date.Equals(emplAbs.DateEnd.Date) && interval.EndTime.Hour == 23 && interval.EndTime.Minute == 59)
                                {
                                    bool isNightShift = false;
                                    foreach (int keyNext in schemaIntervalsNext.Keys)
                                    {
                                        WorkTimeIntervalTO intervalNext = schemaIntervalsNext[keyNext];

                                        if (intervalNext.StartTime.Hour == 0 && intervalNext.StartTime.Minute == 0)
                                        {
                                            isNightShift = true;
                                            break;
                                        }
                                    }

                                    if (isNightShift)
                                    {
                                        try
                                        {
                                            IOPairTO pairTO = new IOPairTO();
                                            pairTO.IOPairDate = date.Date.AddDays(1);
                                            pairTO.EmployeeID = emplAbs.EmployeeID;
                                            pairTO.LocationID = Constants.defaultLocID;
                                            pairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                                            pairTO.PassTypeID = emplAbs.PassTypeID;
                                            pairTO.StartTime = new DateTime(date.AddDays(1).Year, date.AddDays(1).Month, date.AddDays(1).Day, 0, 0, 0);
                                            pairTO.EndTime = new DateTime(date.AddDays(1).Year, date.AddDays(1).Month, date.AddDays(1).Day, 7, 0, 0);
                                            pairTO.ManualCreated = (int)Constants.recordCreated.Automaticaly;

                                            absPairs.Add(pairTO);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Check is it DataProcessingException 
                                            if (ex is DataProcessingException)
                                            {
                                                DataProcessingException dataProcEx = (DataProcessingException)ex;

                                                if (dataProcEx.Number.Equals(12))
                                                {
                                                    log.writeLog(DateTime.Now + " IOPair.GenerateValidPair(): " + dataProcEx.message + "\n");
                                                }
                                                else
                                                {
                                                    throw ex;
                                                }
                                            }
                                            else
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                IOPairTO pairTO = new IOPairTO();
                                pairTO.IOPairDate = date.Date;
                                pairTO.EmployeeID = emplAbs.EmployeeID;
                                pairTO.LocationID = Constants.defaultLocID;
                                pairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                                pairTO.PassTypeID = emplAbs.PassTypeID;
                                pairTO.StartTime = startTime;
                                pairTO.EndTime = endTime;
                                pairTO.ManualCreated = (int)Constants.recordCreated.Automaticaly;

                                absPairs.Add(pairTO);
                            }
                            catch (Exception ex)
                            {
                                // Check is it DataProcessingException 
                                if (ex is DataProcessingException)
                                {
                                    DataProcessingException dataProcEx = (DataProcessingException)ex;

                                    if (dataProcEx.Number.Equals(12))
                                    {
                                        log.writeLog(DateTime.Now + " IOPair.GenerateValidPair(): " + dataProcEx.message + "\n");
                                    }
                                    else
                                    {
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    throw ex;
                                }
                            }
                        }

                        // goto next day
                        date = date.AddDays(1);
                    }

                    bool inserted = true;
                    ioPairDAO.beginTransaction();

                    foreach (IOPairTO absPair in absPairs)
                    {
                        inserted = (ioPairDAO.insertWholeDayAbsence(absPair, false) == 1 ? true : false) && inserted;
                        if (!inserted)
                        {
                            break;
                        }
                    }

                    // if all absences days are successufully inserted to io_pairs,
                    // absence is updated to used and transaction is commited
                    if (inserted)
                    {
                        ioPairDAO.commitTransaction();
                        emplAbs.Used = (int)Constants.Used.Yes;
                        new EmployeeAbsence().Update(emplAbs.RecID, emplAbs.EmployeeID, emplAbs.PassTypeID,
                            emplAbs.DateStart, emplAbs.DateEnd, emplAbs.Used);
                    }
                    else
                    {
                        // absence is updated to error state
                        ioPairDAO.rollbackTransaction();
                        emplAbs.Used = (int)Constants.Used.Error;
                        new EmployeeAbsence().Update(emplAbs.RecID, emplAbs.EmployeeID, emplAbs.PassTypeID,
                            emplAbs.DateStart, emplAbs.DateEnd, emplAbs.Used);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.insertWholeDayAbsences(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<IOPairTO> SearchForPresence(int wuID, string wUnitsString, DateTime from, DateTime to)
        {
            try
            {
                return ioPairDAO.getIOPairsForPresence(wuID, wUnitsString, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetIOPairsForPresence(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForOUPresence(int ouID, string oUnitsString, DateTime from, DateTime to)
        {
            try
            {
                return ioPairDAO.getIOPairsForOUPresence(ouID, oUnitsString, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetIOPairsForPresence(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<IOPairTO> SearchForPresence(int wuID, string wUnitsString, DateTime from, DateTime to, bool createDAO)
        {
            try
            {
                return ioPairDAO.getIOPairsForPresence(wuID, wUnitsString, from, to);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetIOPairsForPresence(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForVisit(int visitID)
        {
            try
            {
                return ioPairDAO.getIOPairsForVisit(visitID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForVisit(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchDistincEmployees(DateTime from, DateTime to, int workingUnitID, string wUnits)
        {
            try
            {
                return this.ioPairDAO.getDistinctEmployees(from, to, workingUnitID, wUnits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetIOPairsForPresence(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<EmployeeTO> SearchDistincOUEmployees(DateTime from, DateTime to, int organizationalUnitID, string oUnits)
        {
            try
            {
                return this.ioPairDAO.getDistinctEmployees(from, to, organizationalUnitID, oUnits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetIOPairsForPresence(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<EmployeeTO> SearchDistincEmployees(DateTime from, DateTime to, int workingUnitID, string wUnits, bool createDAO)
        {
            try
            {
                return this.ioPairDAO.getDistinctEmployees(from, to, workingUnitID, wUnits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetIOPairsForPresence(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchDistincEmployeesCount(DateTime from, DateTime to, int workingUnitID, string wUnits)
        {
            try
            {
                return this.ioPairDAO.getDistinctEmployeesCount(from, to, workingUnitID, wUnits);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetIOPairsForPresence(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public ArrayList SearchEmployeesByDate(DateTime fromDate)
        {
            try
            {
                return ioPairDAO.getEmpoloyeesByDate(fromDate);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmployeesByDate(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public ArrayList SearchEmployeesOpenPairsToday()
        {
            try
            {
                return ioPairDAO.getEmpoloyeesOpenPairsToday();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmployeesOpenPairsToday(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetOpenPairs()
        {
            try
            {
                return ioPairDAO.getOpenPairs();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchOpenPairs(int employeeID, DateTime date)
        {
            try
            {
                return ioPairDAO.getOpenPairs(employeeID, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchOpenPairsFromDataSet(int employeeID, DateTime date, DataSet dsOpenPairs)
        {
            try
            {
                return ioPairDAO.getOpenPairsFromDataSet(employeeID, date, dsOpenPairs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchOpenPairsFromDataSet(): " + ex.Message + "\n");
                log.writeLog(DateTime.Now + " StackTrace: " + ex.StackTrace + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchPairsFromDataSet(int employeeID, DateTime date, DataSet dsOpenPairs)
        {
            try
            {
                return ioPairDAO.getPairsFromDataSet(employeeID, date, dsOpenPairs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchPairsFromDataSet(): " + ex.Message + "\n");
                log.writeLog(DateTime.Now + " StackTrace: " + ex.StackTrace + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchPairsWithSpecialOut(DateTime start, DateTime end, DateTime date)
        {
            try
            {
                return ioPairDAO.getPairsWithSpecialOut(start, end, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchPairsWithSpecialOut(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchSpecialOutOpenPairs()
        {
            try
            {
                return ioPairDAO.getSpecialOutOpenPairs();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchSpecialOutOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchPermissionPassPairs()
        {
            try
            {
                return ioPairDAO.getPermissionPassPairs();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchPermissionPassPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void autoClosePairs(Dictionary<int, WorkTimeSchemaTO> dictTimeSchema)
        {
            try
            {
                // if there is no auto close intervals among Time Schema, do not implement auto close				
                bool isAutoClose = false;

                //log.writeBenchmarkLog(" autoClosePairs(): get time schemas: STARTED! +++++\n");
                foreach (WorkTimeSchemaTO schema in dictTimeSchema.Values)
                {
                    if (isAutoClose)
                    {
                        break;
                    }

                    Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> days = schema.Days;
                    foreach (int dayNum in days.Keys)
                    {
                        if (isAutoClose)
                        {
                            break;
                        }

                        Dictionary<int, WorkTimeIntervalTO> intervals = days[dayNum];
                        foreach (int intNum in intervals.Keys)
                        {
                            WorkTimeIntervalTO interval = intervals[intNum];
                            if (interval.AutoClose != (int)Constants.AutoClose.WithoutClose)
                            {
                                isAutoClose = true;
                                break;
                            }
                        }
                    }
                }
                //log.writeBenchmarkLog(" autoClosePairs(): get time schemas: FINISHED! +++++\n");

                if (isAutoClose)
                {
                    // Get all distinct pairs of dates and employees that have opened IO Pairs
                    // order by date and employee ascending.
                    // emplDays contain ArrayLists (first member is date, second member is employee ID)
                    //log.writeBenchmarkLog(" autoClosePairs(): search employee by date: STARTED! +++++\n");

                    // Sanja 23.09.2011. get only pairs from this and previous month                    
                    DateTime lastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).Date;

                    ArrayList emplDays = new IOPair().SearchEmployeesByDate(lastMonth);

                    //log.writeBenchmarkLog(" autoClosePairs(): search employee by date: FINISHED! +++++\n");

                    // work with data sets instead of working with database
                    //DataSet dsTimeSchedules = new EmployeesTimeSchedule().GetTimeSchedules();
                    DataSet dsOpenPairs = new IOPair().GetOpenPairs();
                    //Natasa 25.06.2009
                    //take all autoClosedPairs in dataset
                    DataSet dsAutoClosedPairs = new IOPair().GetClosedPairs();

                    //log.writeBenchmarkLog(" autoClosePairs(): foreach: STARTED! +++++\n");

                    // get schedules for all employees for current and previous month
                    string emplIDs = "";
                    foreach (ArrayList member in emplDays)
                    {
                        emplIDs += ((int)member[1]).ToString().Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(emplIDs, lastMonth, lastMonth.AddMonths(2), null);

                    foreach (ArrayList member in emplDays)
                    {
                        DateTime date = (DateTime)member[0];
                        int employeeID = (int)member[1];

                        // Find Time Schema and day that was scheduled to Employee for that date
                        //List<EmployeeTimeScheduleTO> schedule = new EmployeesTimeSchedule().SearchMonthSchedule(employeeID, date);
                        List<EmployeeTimeScheduleTO> schedule = new List<EmployeeTimeScheduleTO>();

                        if (emplSchedules.ContainsKey(employeeID))
                            schedule = emplSchedules[employeeID];

                        // work with data set instead of working with database
                        //ArrayList schedule = new EmployeesTimeSchedule().SearchMonthScheduleFromDataSet(employeeID, date, dsTimeSchedules);

                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        EmployeeTimeScheduleTO emplTimeSchedule = new EmployeeTimeScheduleTO();
                        Dictionary<int, WorkTimeIntervalTO> intervals = new Dictionary<int, WorkTimeIntervalTO>();
                        int timeScheduleIndex = -1;
                        int dayNum = 0;

                        for (int scheduleIndex = 0; scheduleIndex < schedule.Count; scheduleIndex++)
                        {
                            /* 2008-03-14
                             * From now one, take the last existing time schedule, don't expect that every month has 
                             * time schedule*/
                            if (date >= schedule[scheduleIndex].Date)
                            //&& (date.Month == ((EmployeesTimeSchedule) (schedule[scheduleIndex])).Date.Month))
                            {
                                timeScheduleIndex = scheduleIndex;
                            }
                        }

                        if (timeScheduleIndex >= 0)
                        {
                            emplTimeSchedule = schedule[timeScheduleIndex];

                            List<WorkTimeSchemaTO> schemaList = new List<WorkTimeSchemaTO>();
                            if (dictTimeSchema.ContainsKey(emplTimeSchedule.TimeSchemaID))
                            {
                                schemaList.Add(dictTimeSchema[emplTimeSchedule.TimeSchemaID]);
                            }

                            if (schemaList.Count > 0)
                            {
                                schema = schemaList[0];
                                /* 2008-03-14
                                 * From now one, take the last existing time schedule, don't expect that every month has 
                                 * time schedule*/
                                //dayNum = (emplTimeSchedule.StartCycleDay + date.Day - emplTimeSchedule.Date.Day) % schema.CycleDuration;
                                TimeSpan ts = new TimeSpan(date.Date.Ticks - emplTimeSchedule.Date.Date.Ticks);
                                dayNum = (emplTimeSchedule.StartCycleDay + (int)ts.TotalDays) % schema.CycleDuration;

                                // Determine if any of interval for that day is autoclose
                                intervals = schema.Days[dayNum];
                                isAutoClose = false;
                                List<WorkTimeIntervalTO> autoCloseIntervals = new List<WorkTimeIntervalTO>();

                                foreach (int intNum in intervals.Keys)
                                {
                                    if (intervals[intNum].AutoClose != (int)Constants.AutoClose.WithoutClose)
                                    {
                                        autoCloseIntervals.Add(intervals[intNum]);
                                    }
                                }

                                // If auto close intervals exists for that day, implement automatic IO Pairs closing
                                if (autoCloseIntervals.Count > 0)
                                {
                                    // Find all opened IO Pairs for particular date and employee
                                    //ArrayList iopairs = new IOPair().SearchOpenPairs(employeeID, date);
                                    // work with data set instead of working with database

                                    List<IOPairTO> iopairs = new IOPair().SearchOpenPairsFromDataSet(employeeID, date, dsOpenPairs);
                                    List<IOPairTO> autoClosedPairs = new IOPair().SearchPairsFromDataSet(employeeID, date, dsAutoClosedPairs);

                                    foreach (WorkTimeIntervalTO interval in autoCloseIntervals)
                                    {
                                        //Natasa 25.06.2009.
                                        //don't close interval again if it's allready closed
                                        bool alredyProccess = false;

                                        foreach (IOPairTO pair in autoClosedPairs)
                                        {
                                            if (pair.StartTime.TimeOfDay == interval.StartTime.TimeOfDay || pair.EndTime.TimeOfDay == interval.EndTime.TimeOfDay)
                                            {
                                                alredyProccess = true;
                                                break;
                                            }
                                        }

                                        // Sanja 06.08.2012. if there is auto closed pair for current interval, move to next interval
                                        if (alredyProccess)
                                            continue;
                                        //Natasa 25.06.2009

                                        if (interval.AutoClose == (int)Constants.AutoClose.StartClose)
                                        {
                                            updatePairStart(iopairs, interval);
                                        }
                                        else if (interval.AutoClose == (int)Constants.AutoClose.EndClose)
                                        {
                                            updatePairEnd(iopairs, interval);
                                        }
                                        else if (interval.AutoClose == (int)Constants.AutoClose.StartEndClose)
                                        {
                                            updatePairStartEnd(iopairs, interval);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //log.writeBenchmarkLog(" autoClosePairs(): foreach: FINISHED! +++++\n");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.autoClosePairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void autoCloseTodayPairs()
        {
            try
            {
                // if there is no auto close intervals among Time Schema, do not implement auto close
                List<WorkTimeSchemaTO> schemas = new TimeSchema().Search();
                bool isAutoClose = false;

                log.writeBenchmarkLog(" autoCloseTodayPairs(): get time schemas: STARTED! +++++\n");
                foreach (WorkTimeSchemaTO schema in schemas)
                {
                    if (isAutoClose)
                    {
                        break;
                    }

                    Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> days = schema.Days;
                    foreach (int dayNum in days.Keys)
                    {
                        if (isAutoClose)
                        {
                            break;
                        }

                        Dictionary<int, WorkTimeIntervalTO> intervals = days[dayNum];
                        foreach (int intNum in intervals.Keys)
                        {
                            WorkTimeIntervalTO interval = intervals[intNum];
                            if (interval.AutoClose != (int)Constants.AutoClose.WithoutClose)
                            {
                                isAutoClose = true;
                                break;
                            }
                        }
                    }
                }
                log.writeBenchmarkLog(" autoCloseTodayPairs(): get time schemas: FINISHED! +++++\n");

                if (isAutoClose)
                {
                    // Get all distinct pairs of dates and employees that have opened IO Pairs
                    // order by date and employee ascending for today.
                    // emplDays contain ArrayLists (first member is date, second member is employee ID)
                    log.writeBenchmarkLog(" autoCloseTodayPairs(): search employee by date: STARTED! +++++\n");
                    ArrayList emplDays = new IOPair().SearchEmployeesOpenPairsToday();
                    log.writeBenchmarkLog(" autoCloseTodayPairs(): search employee by date: FINISHED! +++++\n");

                    // work with data sets instead of working with database
                    //DataSet dsTimeSchedules = new EmployeesTimeSchedule().GetTimeSchedules();
                    DataSet dsOpenPairs = new IOPair().GetOpenPairs();
                    //Natasa 25.06.2009
                    //take all autoClosedPairs in dataset
                    DataSet dsAutoClosedPairs = new IOPair().GetClosedPairs();

                    // Sanja 23.09.2011. get only pairs from this and previous month                    
                    DateTime lastMonth = DateTime.Now.Date.AddMonths(-1); // last month
                    lastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1); // first day of last month

                    log.writeBenchmarkLog(" autoCloseTodayPairs(): foreach: STARTED! +++++\n");
                    foreach (ArrayList member in emplDays)
                    {
                        DateTime date = (DateTime)member[0];
                        int employeeID = (int)member[1];

                        // Sanja 23.09.2011. get only pairs from this and previous month
                        if (date.Date < lastMonth)
                            continue;

                        // Find Time Schema and day that was scheduled to Employee for that date
                        List<EmployeeTimeScheduleTO> schedule = new EmployeesTimeSchedule().SearchMonthSchedule(employeeID, date);
                        // work with data set instead of working with database
                        //ArrayList schedule = new EmployeesTimeSchedule().SearchMonthScheduleFromDataSet(employeeID, date, dsTimeSchedules);

                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        EmployeeTimeScheduleTO emplTimeSchedule = new EmployeeTimeScheduleTO();
                        Dictionary<int, WorkTimeIntervalTO> intervals = new Dictionary<int, WorkTimeIntervalTO>();
                        int timeScheduleIndex = -1;
                        int dayNum = 0;

                        for (int scheduleIndex = 0; scheduleIndex < schedule.Count; scheduleIndex++)
                        {
                            /* 2008-03-14
                             * From now one, take the last existing time schedule, don't expect that every month has 
                             * time schedule*/
                            if (date >= schedule[scheduleIndex].Date)
                            //&& (date.Month == ((EmployeesTimeSchedule) (schedule[scheduleIndex])).Date.Month))
                            {
                                timeScheduleIndex = scheduleIndex;
                            }
                        }

                        if (timeScheduleIndex >= 0)
                        {
                            emplTimeSchedule = schedule[timeScheduleIndex];

                            List<WorkTimeSchemaTO> schemaList = new List<WorkTimeSchemaTO>();
                            foreach (WorkTimeSchemaTO timeSchema in schemas)
                            {
                                if (timeSchema.TimeSchemaID == emplTimeSchedule.TimeSchemaID)
                                {
                                    schemaList.Add(timeSchema);
                                }
                            }

                            if (schemaList.Count > 0)
                            {
                                schema = schemaList[0];
                                /* 2008-03-14
                                 * From now one, take the last existing time schedule, don't expect that every month has 
                                 * time schedule*/
                                //dayNum = (emplTimeSchedule.StartCycleDay + date.Day - emplTimeSchedule.Date.Day) % schema.CycleDuration;
                                TimeSpan ts = new TimeSpan(date.Date.Ticks - emplTimeSchedule.Date.Date.Ticks);
                                dayNum = (emplTimeSchedule.StartCycleDay + (int)ts.TotalDays) % schema.CycleDuration;

                                // Determine if any of interval for that day is autoclose
                                intervals = schema.Days[dayNum];
                                isAutoClose = false;
                                List<WorkTimeIntervalTO> autoCloseIntervals = new List<WorkTimeIntervalTO>();

                                foreach (int intNum in intervals.Keys)
                                {
                                    if (intervals[intNum].AutoClose != (int)Constants.AutoClose.WithoutClose)
                                    {
                                        autoCloseIntervals.Add(intervals[intNum]);
                                    }
                                }

                                // If auto close intervals exists for that day, implement automatic IO Pairs closing
                                if (autoCloseIntervals.Count > 0)
                                {
                                    // Find all opened IO Pairs for particular date and employee
                                    //ArrayList iopairs = new IOPair().SearchOpenPairs(employeeID, date);
                                    // work with data set instead of working with database

                                    List<IOPairTO> iopairs = new IOPair().SearchOpenPairsFromDataSet(employeeID, date, dsOpenPairs);
                                    List<IOPairTO> autoClosedPairs = new IOPair().SearchPairsFromDataSet(employeeID, date, dsAutoClosedPairs);

                                    foreach (WorkTimeIntervalTO interval in autoCloseIntervals)
                                    {
                                        //Natasa 25.06.2009.
                                        //don't close interval again if it's allready sclosed
                                        bool alredyProccess = false;

                                        foreach (IOPairTO pair in autoClosedPairs)
                                        {
                                            if (pair.StartTime.TimeOfDay == interval.StartTime.TimeOfDay || pair.EndTime.TimeOfDay == interval.EndTime.TimeOfDay)
                                            {
                                                alredyProccess = true;
                                                break;
                                            }
                                        }

                                        if (alredyProccess)
                                            break;
                                        //Natasa 25.06.2009

                                        if (interval.AutoClose == (int)Constants.AutoClose.StartClose)
                                        {
                                            updatePairStart(iopairs, interval);
                                        }
                                        else if (interval.AutoClose == (int)Constants.AutoClose.EndClose)
                                        {
                                            updatePairEnd(iopairs, interval);
                                        }
                                        else if (interval.AutoClose == (int)Constants.AutoClose.StartEndClose)
                                        {
                                            updatePairStartEnd(iopairs, interval);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    log.writeBenchmarkLog(" autoCloseTodayPairs(): foreach: FINISHED! +++++\n");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.autoCloseTodayPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private DataSet GetClosedPairs()
        {
            try
            {
                return ioPairDAO.getClosedPairs();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.GetOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private bool compareSchedules(List<EmployeeTimeScheduleTO> sc1, List<EmployeeTimeScheduleTO> sc2)
        {
            if (sc1.Count != sc2.Count) return false;
            for (int i = 0; i < sc1.Count; i++)
            {
                if ((sc1[i].EmployeeID != sc2[i].EmployeeID) ||
                    (sc1[i].TimeSchemaID != sc2[i].TimeSchemaID) ||
                    (sc1[i].StartCycleDay != sc2[i].StartCycleDay))
                    return false;
            }
            return true;
        }

        private bool compareIOPairs(List<IOPairTO> io1, List<IOPairTO> io2)
        {
            if (io1.Count != io2.Count) return false;
            for (int i = 0; i < io1.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < io2.Count; j++)
                {
                    if (io2[j].IOPairID == io1[i].IOPairID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            return true;
        }

        private void updatePairStart(List<IOPairTO> iopairs, WorkTimeIntervalTO interval)
        {
            IOPairTO pair = new IOPairTO();
            try
            {
                pair.EndTime = DateTime.Now;
                bool condition = false;

                // find pair without start time and earleast end time
                foreach (IOPairTO iop in iopairs)
                {
                    // if interval is part of night shift, pair is closed whenever employee left job
                    // else, pair is closed only if pair end is during the interval
                    if (interval.StartTime.TimeOfDay.Equals(new DateTime(1, 1, 1, 0, 0, 0).TimeOfDay))
                    {
                        condition = iop.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && iop.EndTime < pair.EndTime;
                    }
                    else
                    {
                        condition = iop.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && iop.EndTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay
                            && iop.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay && iop.EndTime < pair.EndTime;
                    }

                    if (condition)
                    {
                        pair = iop;
                    }
                }

                if (pair.IOPairID != -1)
                {
                    pair.StartTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                        interval.StartTime.Hour, interval.StartTime.Minute, interval.StartTime.Second);
                    pair.ManualCreated = (int)Constants.recordCreated.Automaticaly;

                    new IOPair().Update(pair, Constants.AutoCloseUser);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updatePairStart(): " + "io_pair_id = " + pair.IOPairID.ToString() + ", " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void updatePairEnd(List<IOPairTO> iopairs, WorkTimeIntervalTO interval)
        {
            IOPairTO pair = new IOPairTO();
            try
            {
                bool condition = false;

                // find pair without end time and latest start time
                foreach (IOPairTO iop in iopairs)
                {
                    // if interval is part of night shift, pair is closed whenever employee arrived to job
                    // else, pair is closed only if pair start is during the interval
                    if (interval.EndTime.TimeOfDay.Equals(new DateTime(1, 1, 1, 23, 59, 0).TimeOfDay))
                    {
                        condition = iop.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && iop.StartTime > pair.StartTime;
                    }
                    else
                    {
                        condition = iop.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && iop.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay
                            && iop.StartTime.TimeOfDay <= interval.LatestLeft.TimeOfDay && iop.StartTime > pair.StartTime;
                    }
                    if (condition)
                    {
                        pair = iop;
                    }
                }

                if (pair.IOPairID != -1)
                {
                    pair.EndTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                        interval.EndTime.Hour, interval.EndTime.Minute, interval.EndTime.Second);
                    pair.ManualCreated = (int)Constants.recordCreated.Automaticaly;

                    new IOPair().Update(pair, Constants.AutoCloseUser);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updatePairEnd(): " + "io_pair_id = " + pair.IOPairID.ToString() + ", " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void updatePairStartEnd(List<IOPairTO> iopairs, WorkTimeIntervalTO interval)
        {
            IOPairTO pair = new IOPairTO();
            try
            {
                pair.EndTime = DateTime.Now;

                // find pair without start time and earleast end time
                foreach (IOPairTO iop in iopairs)
                {
                    if (iop.StartTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && iop.EndTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay
                        && iop.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay && iop.EndTime < pair.EndTime)
                    {
                        pair = iop;
                    }
                }

                if (pair.IOPairID != -1)
                {
                    pair.StartTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                        interval.StartTime.Hour, interval.StartTime.Minute, interval.StartTime.Second);
                    pair.ManualCreated = (int)Constants.recordCreated.Automaticaly;

                    new IOPair().Update(pair, Constants.AutoCloseUser);
                }

                pair = new IOPairTO();

                // find pair without end time and latest start time
                foreach (IOPairTO iop in iopairs)
                {
                    if (iop.EndTime.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && iop.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay
                        && iop.StartTime.TimeOfDay <= interval.LatestLeft.TimeOfDay && iop.StartTime > pair.StartTime)
                    {
                        pair = iop;
                    }
                }

                if (pair.IOPairID != -1)
                {
                    pair.EndTime = new DateTime(pair.IOPairDate.Year, pair.IOPairDate.Month, pair.IOPairDate.Day,
                        interval.EndTime.Hour, interval.EndTime.Minute, interval.EndTime.Second);
                    pair.ManualCreated = (int)Constants.recordCreated.Automaticaly;

                    new IOPair().Update(pair, Constants.AutoCloseUser);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updatePairStartEnd(): " + "io_pair_id = " + pair.IOPairID.ToString() + ", " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void autoCloseSpecialOut(Dictionary<int, WorkTimeSchemaTO> timeSchema)
        {
            try
            {
                // get all open IO Pairs where end time is unkonown and pass type is special out
                List<IOPairTO> pairs = SearchSpecialOutOpenPairs();

                // Sanja 23.09.2011. get only pairs from this and previous month                    
                DateTime lastMonth = DateTime.Now.Date.AddMonths(-1); // last month
                lastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1); // first day of last month

                // process each pair and auto close it if it is needed
                foreach (IOPairTO iop in pairs)
                {
                    // Sanja 23.09.2011. get only pairs from this and previous month
                    if (iop.IOPairDate.Date < lastMonth)
                        continue;

                    Employee empl = new Employee();
                    empl.EmplTO.EmployeeID = iop.EmployeeID;
                    ArrayList emplSchema = empl.findTimeSchema(iop.IOPairDate, timeSchema);

                    // get first pass for that employee and day that is before pair start time
                    //Pass pass = new Pass().SearchPassBeforePermissionPass(iop.EmployeeID, iop.StartTime);
                    CultureInfo ci = CultureInfo.InvariantCulture;
                    List<PassTO> passList = new Pass().getPassesForEmployeeAll(empl.EmplTO.EmployeeID, iop.IOPairDate);
                    PassTO pass = new PassTO();
                    foreach (PassTO p in passList)
                    {
                        if (pass.PassID == -1 || pass.EventTime.TimeOfDay > p.EventTime.TimeOfDay)
                            pass = p;
                    }
                    // pair wiil be auto closed only if employee has defined time schema for that date
                    if (emplSchema.Count > 1)
                    {
                        // find start and end time of working day
                        DateTime endTime = new DateTime(0);
                        DateTime latestEndTime = new DateTime(0);
                        DateTime earliestStartTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
                        DateTime startTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
                        TimeSpan shiftDuration = new TimeSpan();

                        Dictionary<int, WorkTimeIntervalTO> intervals = ((WorkTimeSchemaTO)emplSchema[0]).Days[(int)emplSchema[1]];
                        foreach (int intNum in intervals.Keys)
                        {
                            if (intervals[intNum].EarliestArrived.TimeOfDay < earliestStartTime.TimeOfDay)
                            {
                                earliestStartTime = intervals[intNum].EarliestArrived;
                            }

                            if (intervals[intNum].StartTime.TimeOfDay < startTime.TimeOfDay)
                            {
                                startTime = intervals[intNum].StartTime;
                            }

                            if (intervals[intNum].EndTime.TimeOfDay > endTime.TimeOfDay)
                            {
                                endTime = intervals[intNum].EndTime;
                            }

                            if (intervals[intNum].LatestLeft.TimeOfDay > latestEndTime.TimeOfDay)
                            {
                                latestEndTime = intervals[intNum].LatestLeft;
                            }
                            shiftDuration += endTime.TimeOfDay - startTime.TimeOfDay;
                        }

                        //if first pass with direction IN is before earliest start time set event time = earliest start time
                        if (pass.EventTime.TimeOfDay < earliestStartTime.TimeOfDay)
                            pass.EventTime = earliestStartTime;

                        if (pass.EventTime.AddMinutes(shiftDuration.TotalMinutes).TimeOfDay < iop.StartTime.TimeOfDay)
                        {
                            pass = new PassTO();
                        }

                        // if there is more than one interval for working day
                        if (intervals.Count > 1)
                        {
                            // and out is not before earliest start
                            if (iop.StartTime.TimeOfDay >= earliestStartTime.TimeOfDay)
                            {
                                bool found = false;

                                foreach (int intNum in intervals.Keys)
                                {
                                    DateTime time = intervals[intNum].StartTime.AddSeconds(1);
                                    if (iop.StartTime.TimeOfDay.Equals(time.TimeOfDay))
                                    {
                                        List<IOPairTO> pairsSpecialOut = SearchPairsWithSpecialOut(intervals[intNum].EarliestArrived,
                                            intervals[intNum].LatestLeft, iop.IOPairDate);
                                        if (pairsSpecialOut.Count > 0)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }

                                    DateTime intEnd;
                                    if (((WorkTimeSchemaTO)emplSchema[0]).Type.Trim() != Constants.schemaTypeFlexi)
                                    {
                                        intEnd = endTime;
                                    }
                                    // if flexi time schema set end time for the permission to the Latest Left time of the schema <--> Darko 7.12.2007.
                                    else
                                    {
                                        if (pass.PassID == -1 || pass.EventTime.AddMinutes(shiftDuration.TotalMinutes).TimeOfDay > latestEndTime.TimeOfDay)
                                        {
                                            intEnd = latestEndTime;
                                        }
                                        else
                                        {
                                            intEnd = pass.EventTime.AddMinutes(shiftDuration.TotalMinutes);
                                        }
                                    }

                                    if (iop.StartTime.TimeOfDay >= intervals[intNum].EarliestArrived.TimeOfDay
                                        && iop.StartTime.TimeOfDay <= intEnd.TimeOfDay)
                                    {
                                        iop.EndTime = new DateTime(iop.StartTime.Year, iop.StartTime.Month, iop.StartTime.Day,
                                            intEnd.Hour, intEnd.Minute, intEnd.Second);
                                        //check if there is overlap with some existing io pair.If it is, do not do anything.
                                        if (ExistEmlpoyeeDatePairNotWholeDayAbsences(iop.EmployeeID, iop.IOPairDate, iop.StartTime, iop.EndTime))
                                            break;
                                        new IOPair().Update(iop, Constants.AutoCloseSpecialOutUser);
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    iop.EndTime = iop.StartTime;
                                    //check if there is overlap with some existing io pair.If it is, do not do anything.
                                    if (!ExistEmlpoyeeDatePairNotWholeDayAbsences(iop.EmployeeID, iop.IOPairDate, iop.StartTime, iop.EndTime))
                                        new IOPair().Update(iop, Constants.AutoCloseSpecialOutUser);
                                }
                            }
                        }
                        else
                        {
                            DateTime time = startTime.AddSeconds(1);
                            if (iop.StartTime.TimeOfDay.Equals(time.TimeOfDay))
                            {
                                List<IOPairTO> pairsSpecialOut = SearchPairsWithSpecialOut(earliestStartTime, latestEndTime, iop.IOPairDate);
                                if (pairsSpecialOut.Count > 0)
                                {
                                    continue;
                                }
                            }

                            DateTime intEnd;
                            if (((WorkTimeSchemaTO)emplSchema[0]).Type.Trim() != Constants.schemaTypeFlexi)
                            {
                                intEnd = endTime;
                            }
                            // if flexi time schema set end time for the permission to the Latest Left time of the schema <--> Darko 7.12.2007.
                            else
                            {
                                if (pass.PassID == -1 || pass.EventTime.AddMinutes(shiftDuration.TotalMinutes).TimeOfDay > latestEndTime.TimeOfDay)
                                {
                                    intEnd = latestEndTime;
                                }
                                else
                                {
                                    intEnd = pass.EventTime.AddMinutes(shiftDuration.TotalMinutes);
                                }
                            }

                            // pair will be auto closed only if out is not before start time for that day
                            if (iop.StartTime.TimeOfDay >= earliestStartTime.TimeOfDay)
                            {
                                // if out is during the working day, it is closed with end time for that day
                                if (iop.StartTime.TimeOfDay <= intEnd.TimeOfDay)
                                {
                                    iop.EndTime = new DateTime(iop.StartTime.Year, iop.StartTime.Month, iop.StartTime.Day,
                                        intEnd.Hour, intEnd.Minute, intEnd.Second);
                                    //check if there is overlap with some existing io pair.If it is, do not do anything.
                                    if (!ExistEmlpoyeeDatePairNotWholeDayAbsences(iop.EmployeeID, iop.IOPairDate, iop.StartTime, iop.EndTime))
                                        new IOPair().Update(iop, Constants.AutoCloseSpecialOutUser);
                                }
                                // if out is after the working day, it is closed with time from pair
                                else
                                {
                                    iop.EndTime = iop.StartTime;
                                    //check if there is overlap with some existing io pair.If it is, do not do anything.
                                    if (!ExistEmlpoyeeDatePairNotWholeDayAbsences(iop.EmployeeID, iop.IOPairDate, iop.StartTime, iop.EndTime))
                                        new IOPair().Update(iop, Constants.AutoCloseSpecialOutUser);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.autoCloseSpecialOut(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void permissionPassesProcessing()
        {
            try
            {
                // get all pairs that last 1sec 
                // they are created from passes that are created for start working day exit permissions
                List<IOPairTO> pairs = SearchPermissionPassPairs();

                foreach (IOPairTO pair in pairs)
                {
                    // get first pass for that employee and day that is before pair start time
                    PassTO pass = new Pass().SearchPassBeforePermissionPass(pair.EmployeeID, pair.StartTime);

                    // only if pass is found and direction is IN and passes that pair is created from
                    // are created from start working day exit permissions, pair and passes are deleted
                    if (pass.PassID >= 0 && pass.Direction == Constants.DirectionIn)
                    {
                        List<PassTO> passesTOForPair = new Pass().SearchPermPassesForPair(pair.EmployeeID, pair.StartTime, pair.EndTime);
                        //ArrayList passesTOForPair = new ArrayList();
                        //foreach (PassTO passForPair in passesForPair)
                        //{
                        //    passesTOForPair.Add(passForPair.sendTransferObject());
                        //}
                        // if both passes are found do deletion
                        if (passesTOForPair.Count == 2)
                        {
                            // first find pair that start with special out from pair
                            this.PairTO = new IOPairTO();
                            this.PairTO.IOPairDate = pair.IOPairDate;
                            this.PairTO.EmployeeID = pair.EmployeeID;
                            this.PairTO.LocationID = pair.LocationID;
                            this.PairTO.IsWrkHrsCount = (int)Constants.IsWrkCount.IsCounter;
                            this.PairTO.StartTime = pair.EndTime;
                            this.PairTO.ManualCreated = (int)Constants.recordCreated.Automaticaly;
                            List<IOPairTO> pairsToDel = Search();
                            pairsToDel.Add(pair);
                            List<IOPairTO> pairsTOList = new List<IOPairTO>();
                            foreach (IOPairTO iop in pairsToDel)
                            {
                                pairsTOList.Add(iop);
                            }

                            int emplID = pair.EmployeeID;
                            DateTime date = pair.IOPairDate;

                            // delete passes created from permissions and pair and found pairs
                            ioPairDAO.deletePairPasses(pairsTOList, passesTOForPair, daoFactory);

                            // do recreation: for employee and date, set all passes to not used for creating pairs 
                            // and delete all pairs, so in next processing pairs from passes, 
                            // regular pairs will be created
                            ioPairDAO.updateIOPairsPasses(emplID, date.ToString("MM/dd/yyy"), daoFactory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.permissionPassesProcessing(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //emplByDay - list of employees and days for them, that need recalculation
        private void processingPauses(ArrayList emplByDay, Dictionary<int, WorkTimeSchemaTO> dictTimeSchema)
        {
            try
            {
                //list of union of all days for all employees from emplByDay
                //and, for every day in the list, also contains previous they 
                List<DateTime> datesList = new List<DateTime>();
                //Key is employeeID, value is Hashtable with dates from emplByDay for that employee
                Hashtable employees = new Hashtable();
                //string of all employees from emplByDay
                string employeeIDString = "";

                //get list of employee and dates, for days that do not have pause calculated, 
                //no open pairs, and there is at least one pair with auto_close
                ArrayList emplByDayAutoClose = ioPairDAO.getEmployeesByDateAutoClose();

                createEmplAndDateLists(emplByDay, ref employeeIDString, ref datesList,
                    ref employees);
                createEmplAndDateLists(emplByDayAutoClose, ref employeeIDString, ref datesList,
                    ref employees);
                if (employeeIDString.Length > 0)
                {
                    employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);
                }

                //get all ioPairs (including open) for all employees in employeeIDString and all dates in datesList
                List<IOPairTO> ioPairsList = SearchAllEmplDatePairs(employeeIDString, datesList);

                //calculate pause for this ioPairs
                calculatePause(ioPairsList, employees, dictTimeSchema, null);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.processingPauses(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //emplByDay - list of employees and days for them, that need recalculation
        //employeeIDString - string of all employees from emplByDay
        //datesList - list of union of all days for all employees from emplByDay
        //and, for every day in the list, also contains previous they
        //employees - Key is employeeID, value is Hashtable with dates from emplByDay for that employee
        private void createEmplAndDateLists(ArrayList emplByDay, ref string employeeIDString,
            ref List<DateTime> datesList, ref Hashtable employees)
        {
            foreach (ArrayList emplDate in emplByDay)
            {
                int currentEmpl = (int)emplDate[0];
                DateTime currentEmplDate = DateTime.Parse(emplDate[1].ToString(), new CultureInfo("en-US"), DateTimeStyles.NoCurrentDateDefault);

                DateTime previousDay = currentEmplDate.Subtract(new TimeSpan(1, 0, 0, 0));
                if (!datesList.Contains(previousDay))
                {
                    datesList.Add(previousDay);
                }

                if (!datesList.Contains(currentEmplDate))
                {
                    datesList.Add(currentEmplDate);
                }

                if (!employees.ContainsKey(currentEmpl))
                {
                    employees.Add(currentEmpl, new Hashtable());
                    employeeIDString += currentEmpl.ToString().Trim() + ",";
                }

                Hashtable datesForEmpl = (Hashtable)employees[currentEmpl];
                if (!datesForEmpl.ContainsKey(currentEmplDate))
                {
                    datesForEmpl.Add(currentEmplDate, "");
                }
            }
        }

        //ioPairList must be Sort by employee_id, io_pair_date, start_time
        //and contains all io_pairs (including open pairs)
        //for each employee and date, ioPairList contains all pairs for that day and day before (for night shift)
        //employeesAndDates - key is employeeID, value is Hashtable with dates for that employee
        private void calculatePause(List<IOPairTO> ioPairList, Hashtable employeesAndDates, Dictionary<int, WorkTimeSchemaTO> timeSchemaTable, IDbTransaction transaction)
        {
            int currEmplID = 0;
            DateTime currentDay = new DateTime();
            try
            {
                if (ioPairList.Count > 0)
                {
                    //list of time schedules for all employees, for selected interval
                    List<EmployeeTimeScheduleTO> timeSchedules = new List<EmployeeTimeScheduleTO>();

                    // all employee time schedules for selected Time Interval, key is employee ID
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();

                    //all pauses
                    List<TimeSchemaPauseTO> pauses = new TimeSchemaPause().Search("", transaction);

                    DateTime fromDay = new DateTime(0);
                    DateTime toDay = new DateTime(0);

                    List<int> employeesID = new List<int>();
                    string employeeIDString = "";

                    // Key is Employee ID, value is List of valid IO Pairs for that Employee
                    Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>();

                    // io pairs for particular employee are sorted by io_pair_date ascending
                    for (int i = 0; i < ioPairList.Count; i++)
                    {
                        int currentEmployeeID = ioPairList[i].EmployeeID;
                        if (!emplPairs.ContainsKey(currentEmployeeID))
                        {
                            employeesID.Add(currentEmployeeID);
                            employeeIDString += currentEmployeeID.ToString().Trim() + ",";

                            emplPairs.Add(currentEmployeeID, new List<IOPairTO>());
                            emplTimeSchedules.Add(currentEmployeeID, new List<EmployeeTimeScheduleTO>());
                        }
                        if ((fromDay == new DateTime(0)) || (fromDay.Date > ioPairList[i].IOPairDate.Date))
                            fromDay = ioPairList[i].IOPairDate;
                        if ((toDay == new DateTime(0)) || (toDay.Date < ioPairList[i].IOPairDate.Date))
                            toDay = ioPairList[i].IOPairDate;

                        emplPairs[currentEmployeeID].Add(ioPairList[i]);
                    }
                    if (employeeIDString.Length > 0)
                    {
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);
                    }

                    //in ioPairList, for each day, there are pairs from previous day also, so,
                    //from day is not minimum day, but minimum day + 1
                    //but, if there were no ioPairs from previous day, then, from day is as it should be
                    //the best is not to add 1 to from day, anyway, calculation is done only for the employee's days
                    //fromDay = fromDay.AddDays(1);

                    //NOTE: this is changed now, pairs from the previous day are also in ioPairList (not get from database here)
                    //and everything is sorted ok
                    //for the first day, in case it is night shift, pairs from the previous day are needed
                    //for the last day ignore night shift, because pair for the next day do not exist yet
                    /*TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
                    ArrayList firstDayPairs = (new IOPair()).SearchAll(fromDay.Subtract(oneDay), fromDay.Subtract(oneDay), employeesID);
                    for (int i = 0; i < firstDayPairs.Count; i++)
                    {
                        int currentEmployeeID = ((TransferObjects.IOPairTO) firstDayPairs[i]).EmployeeID;
				
                        ((ArrayList) (emplPairs[currentEmployeeID])).Add((TransferObjects.IOPairTO) firstDayPairs[i]);
                    }*/

                    //get time schemas for selected Employees, for selected Time Interval
                    timeSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeeIDString, fromDay, toDay, transaction);
                    for (int i = 0; i < timeSchedules.Count; i++)
                    {
                        emplTimeSchedules[timeSchedules[i].EmployeeID].Add(timeSchedules[i]);
                    }

                    //Start calculation
                    //for each employee, day, interval in that day
                    foreach (int employeeID in employeesID)
                    {
                        currEmplID = employeeID;
                        if (employeeID == 340) 
                        {
                        }
                        if (!emplPairs.ContainsKey(employeeID) || (emplPairs.ContainsKey(employeeID) && emplPairs[employeeID].Count <= 0))
                            continue;

                        if (!employeesAndDates.ContainsKey(employeeID))
                            continue;

                        Hashtable employeeDates = (Hashtable)employeesAndDates[employeeID];

                        for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                        {
                            currentDay = day;
                            if (employeeID == 340 && day == new DateTime(2024, 11, 20))
                            {

                            }
                            //calculate pause only for employee's dates
                            if (!employeeDates.ContainsKey(day))
                                continue;

                            bool is2DaysShift = false;
                            bool is2DaysShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref is2DaysShift,
                                ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemaTable);

                            if (dayIntervals == null)
                                continue;

                            for (int i = 0; i < dayIntervals.Count; i++)
                            {
                                WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i];
                                if (timeSchemaTable.ContainsKey(currentTimeSchemaInterval.TimeSchemaID))
                                    if (timeSchemaTable[currentTimeSchemaInterval.TimeSchemaID].Type.Equals(Constants.schemaTypeFlexi))
                                    {
                                        currentTimeSchemaInterval.StartTime = currentTimeSchemaInterval.EarliestArrived;
                                        currentTimeSchemaInterval.EndTime = currentTimeSchemaInterval.LatestLeft;
                                    }
                            }

                            //if previous day is night shift day, take that night shift interval
                            WorkTimeIntervalTO lastIntervalPreviousDay = new WorkTimeIntervalTO();
                            if (is2DaysShiftPrevious)
                                lastIntervalPreviousDay = getLastIntervalPrevDay(emplTimeSchedules[employeeID], day, timeSchemaTable);

                            //if this day is night shift day, find index of night shift interval
                            int lastIntervalIndex = -1;
                            if (is2DaysShift)
                            {
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                    {
                                        lastIntervalIndex = i;
                                        break;
                                    }
                                }
                            }

                            //if previous day is night shift day, find index of interval in this day that is part of
                            //previous day night shift
                            int partOfNightShiftIndex = -1;
                            if (is2DaysShiftPrevious)
                            {
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                    {
                                        partOfNightShiftIndex = i;
                                        break;
                                    }
                                }
                            }

                            //Take only IO Pairs for specific day, considering night shifts
                            List<IOPairTO> dayIOPairList = getEmployeeExactDayPairs(emplPairs[employeeID], day, is2DaysShiftPrevious, lastIntervalPreviousDay);

                            if (dayIOPairList.Count <= 0)
                                continue;

                            //do not calculate pause if day has at least one open interval
                            bool openPairExist = existEmployeeDayOpenPairs(dayIOPairList);
                            if (openPairExist)
                                continue;

                            for (int i = 0; i < dayIntervals.Count; i++)
                            {
                                WorkTimeIntervalTO tsInterval = dayIntervals[i]; //key is interval_num which is 0, 1...

                                //if this is night shift day, skip night shift interval, it will 
                                //be calculated together with next day
                                if (is2DaysShift && (i == lastIntervalIndex))
                                    continue;

                                //use pairs from previous day only if previous day is night shift
                                //and current interval is part of night shift
                                bool addPairsFromPrevDay = false;
                                if (is2DaysShiftPrevious && (lastIntervalPreviousDay != null)
                                    && (i == partOfNightShiftIndex))
                                    addPairsFromPrevDay = true;

                                //get pause informations (for night shift, informations are with first interval)
                                int duration = -1;
                                int tolerancy = -1;
                                int shortBreak = -1;
                                DateTime pauseEarliestTime = new DateTime(0);
                                DateTime pauseLatestTime = new DateTime(0);
                                int pauseID = tsInterval.PauseID;
                                if (addPairsFromPrevDay)
                                    pauseID = lastIntervalPreviousDay.PauseID;

                                if (pauseID == -1)
                                    continue;

                                foreach (TimeSchemaPauseTO currentPause in pauses)
                                {
                                    if (pauseID == currentPause.PauseID)
                                    {
                                        duration = currentPause.PauseDuration;
                                        tolerancy = currentPause.PauseOffset;
                                        shortBreak = currentPause.ShortBreakDuration;

                                        WorkTimeIntervalTO newInterval = new WorkTimeIntervalTO();

                                        //calculate pause latest use time, it is always in tsInterval
                                        newInterval.EndTime = new DateTime(day.Year, day.Month, day.Day);
                                        newInterval.EndTime = newInterval.EndTime.Add(tsInterval.EndTime.TimeOfDay);
                                        pauseLatestTime = newInterval.EndTime.Subtract(new TimeSpan(0, currentPause.LatestUseTime, 0));

                                        //calculate pause earliest use time, it is in lastIntervalPreviousDay for night shift
                                        //otherwise, it is in tsInterval
                                        newInterval.StartTime = new DateTime(day.Year, day.Month, day.Day);
                                        if (addPairsFromPrevDay)
                                        {
                                            //if it is night shift
                                            newInterval.StartTime = newInterval.StartTime.Subtract(new TimeSpan(1, 0, 0, 0));
                                            newInterval.StartTime = newInterval.StartTime.Add(lastIntervalPreviousDay.StartTime.TimeOfDay);
                                        }
                                        else
                                        {
                                            newInterval.StartTime = newInterval.StartTime.Add(tsInterval.StartTime.TimeOfDay);
                                        }
                                        pauseEarliestTime = newInterval.StartTime.AddMinutes(currentPause.EarliestUseTime);
                                        break;
                                    }
                                }

                                //get only interval pairs, considering night shift
                                List<IOPairTO> intervalIoPairList = getEmployeeIntervalPairs(dayIOPairList, tsInterval, lastIntervalPreviousDay,
                                    addPairsFromPrevDay, day);
                                if (intervalIoPairList.Count < 2)
                                    continue;

                                List<IOPairTO> intervalIoPairListClone = new List<IOPairTO>();
                                foreach (IOPairTO iop in intervalIoPairList)
                                {
                                    intervalIoPairListClone.Add(iop);
                                }

                                //make new ioPairList (trimmedIoPairs) considering overlaping of intervals
                                List<IOPairTO> trimmedIoPairs = new List<IOPairTO>();
                                List<int> equalPairsID = new List<int>();
                                foreach (IOPairTO iopairTO in intervalIoPairList)
                                {
                                    if (!equalPairsID.Contains(iopairTO.IOPairID))
                                    {
                                        IOPairTO newioPair = new IOPairTO(iopairTO);
                                        newioPair.IOPairID = 0;
                                        newioPair.IsWrkHrsCount = 0;
                                        newioPair.LocationID = 0;
                                        newioPair.LocationName = "";
                                        newioPair.ManualCreated = 0;
                                        newioPair.PassType = "";
                                        newioPair.PassTypeID = 0;
                                        newioPair.WUName = "";

                                        bool insideOtherPair = false;
                                        foreach (IOPairTO iopairTOClone in intervalIoPairListClone)
                                        {
                                            if (iopairTOClone.IOPairID != iopairTO.IOPairID)
                                            {
                                                if ((iopairTOClone.StartTime.TimeOfDay == iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay == iopairTO.EndTime.TimeOfDay))
                                                {
                                                    //register that for iopairTO pair exist iopairTOClone pair with the same
                                                    //start and end time, so, do not do calculation again for iopairTOClone
                                                    equalPairsID.Add(iopairTOClone.IOPairID);
                                                }

                                                if ((iopairTOClone.StartTime.TimeOfDay <= iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay >= iopairTO.EndTime.TimeOfDay)
                                                    && !((iopairTOClone.StartTime.TimeOfDay == iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay == iopairTO.EndTime.TimeOfDay)))
                                                {
                                                    //iopairTO pair is inside samo other iopairTOClone pair
                                                    insideOtherPair = true;
                                                    break;
                                                }

                                                if ((iopairTOClone.StartTime.TimeOfDay < iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay >= iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.StartTime.TimeOfDay < newioPair.StartTime.TimeOfDay))
                                                {
                                                    //iopairTO and iopairTOClone pairs are overlaping, adjust start time
                                                    newioPair.StartTime = iopairTOClone.StartTime;
                                                }

                                                if ((iopairTOClone.StartTime.TimeOfDay <= iopairTO.EndTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay > iopairTO.EndTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay > newioPair.EndTime.TimeOfDay))
                                                {
                                                    //iopairTO and iopairTOClone pairs are overlaping, adjust end time
                                                    newioPair.EndTime = iopairTOClone.EndTime;
                                                }
                                            } //if (iopairTOClone.IOPairID != iopairTO.IOPairID)
                                        } //foreach (TransferObjects.IOPairTO iopairTOClone in intervalIoPairListClone)

                                        if (insideOtherPair)
                                            continue;

                                        if (trimmedIoPairs.Contains(newioPair))
                                            continue;

                                        //check if this new pair already exist in trimmedIoPairs, or maybe it
                                        //overlaps with some pair there
                                        //Do necesery changes acording to that
                                        bool existInTrimStart = false;
                                        bool existInTrimEnd = false;
                                        foreach (IOPairTO iopairTOTrimm in trimmedIoPairs)
                                        {
                                            if ((iopairTOTrimm.StartTime.TimeOfDay > newioPair.StartTime.TimeOfDay)
                                                && (iopairTOTrimm.StartTime.TimeOfDay <= newioPair.EndTime.TimeOfDay))
                                            {
                                                iopairTOTrimm.StartTime = newioPair.StartTime;
                                                existInTrimStart = true;
                                            }

                                            if ((iopairTOTrimm.EndTime.TimeOfDay >= newioPair.StartTime.TimeOfDay)
                                                && (iopairTOTrimm.EndTime.TimeOfDay < newioPair.EndTime.TimeOfDay))
                                            {
                                                iopairTOTrimm.EndTime = newioPair.EndTime;
                                                existInTrimEnd = true;
                                            }

                                            if ((iopairTOTrimm.StartTime.TimeOfDay <= newioPair.StartTime.TimeOfDay)
                                                && (iopairTOTrimm.EndTime.TimeOfDay >= newioPair.EndTime.TimeOfDay))
                                            {
                                                existInTrimStart = true;
                                                existInTrimEnd = true;
                                            }
                                        }

                                        if (existInTrimStart || existInTrimEnd)
                                            continue;

                                        trimmedIoPairs.Add(newioPair);
                                    } //if (!equalPairsID.Contains(iopairTO.IOPairID))
                                } //foreach (TransferObjects.IOPairTO iopairTO in intervalIoPairList)

                                if (trimmedIoPairs.Count < 2)
                                    continue;

                                //since iopairs are sorted by start_time, trimmedIOPAirs are also sorted, so, holes are between them
                                //holes will be new IOPairs

                                //create short breaks first
                                if (shortBreak > 0)
                                    trimmedIoPairs = insertShortBreaks(employeeID, day, trimmedIoPairs, shortBreak, addPairsFromPrevDay, transaction);

                                if (trimmedIoPairs.Count < 2)
                                    continue;

                                if (duration <= 0)
                                    continue;

                                //create candidates for holes
                                List<IOPairTO> holesList = new List<IOPairTO>();
                                for (int j = 1; j < trimmedIoPairs.Count; j++)
                                {
                                    IOPairTO newioPair = new IOPairTO();
                                    newioPair.StartTime = trimmedIoPairs[j - 1].EndTime;
                                    newioPair.EndTime = trimmedIoPairs[j].StartTime;

                                    //if it is night shift, ignore hole (23:59 - 00:00)
                                    if (addPairsFromPrevDay &&
                                        (newioPair.StartTime == new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month,
                                        newioPair.StartTime.Day, 23, 59, 0))
                                        && (newioPair.EndTime == new DateTime(newioPair.EndTime.Year, newioPair.EndTime.Month,
                                        newioPair.EndTime.Day, 0, 0, 0))
                                        )
                                        continue;

                                    if (addPairsFromPrevDay && (newioPair.StartTime == new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month,
                                        newioPair.StartTime.Day, 23, 59, 0)))
                                        newioPair.StartTime = newioPair.StartTime.AddMinutes(1);

                                    if (addPairsFromPrevDay && (newioPair.EndTime == new DateTime(newioPair.EndTime.Year, newioPair.EndTime.Month,
                                        newioPair.EndTime.Day, 0, 0, 0)))
                                        newioPair.EndTime = newioPair.EndTime.Subtract(new TimeSpan(0, 1, 0));

                                    if (newioPair.StartTime.Date < pauseEarliestTime.Date)
                                        continue;
                                    if (newioPair.EndTime.Date > pauseLatestTime.Date)
                                        continue;

                                    //adjust start time according to pause earliest use time
                                    if ((newioPair.StartTime.TimeOfDay < pauseEarliestTime.TimeOfDay)
                                        && (newioPair.EndTime.TimeOfDay > pauseEarliestTime.TimeOfDay)
                                        && (newioPair.StartTime.Date == pauseEarliestTime.Date))
                                        newioPair.StartTime = pauseEarliestTime;

                                    //adjust end time according to pause latest use time
                                    if ((newioPair.EndTime.TimeOfDay > pauseLatestTime.TimeOfDay)
                                        && (newioPair.StartTime.TimeOfDay < pauseLatestTime.TimeOfDay)
                                        && (newioPair.EndTime.Date == pauseLatestTime.Date))
                                        newioPair.EndTime = pauseLatestTime;

                                    if ((newioPair.StartTime.TimeOfDay < pauseEarliestTime.TimeOfDay)
                                        && (newioPair.StartTime.Date == pauseEarliestTime.Date))
                                        continue;

                                    /*DateTime newioPairStartTime = newioPair.StartTime.Subtract(new TimeSpan(0, 0, newioPair.StartTime.Second));
                                    DateTime newioPairEndTime = newioPair.EndTime.Subtract(new TimeSpan(0, 0, newioPair.EndTime.Second));
                                    TimeSpan newioPairDuration = (new DateTime(newioPairEndTime.Ticks - newioPairStartTime.Ticks)).TimeOfDay;*/
                                    /*TimeSpan newioPairDuration = (new DateTime(newioPair.EndTime.Ticks - newioPair.StartTime.Ticks)).TimeOfDay;

                                    DateTime newioPairEndDate = new DateTime();
                                    if (newioPairDuration.TotalMinutes > (double)duration)
                                        newioPairEndDate = newioPair.StartTime.AddMinutes((double) duration);
                                    else
                                        newioPairEndDate = newioPair.EndTime;
                                    if ((newioPairEndDate.TimeOfDay > pauseLatestTime.TimeOfDay)
                                        && (newioPairEndDate.Date == pauseLatestTime.Date))
                                        continue;*/
                                    if ((newioPair.EndTime.TimeOfDay > pauseLatestTime.TimeOfDay)
                                        && (newioPair.EndTime.Date == pauseLatestTime.Date))
                                        continue;

                                    holesList.Add(newioPair);
                                }

                                if (holesList.Count <= 0)
                                    continue;

                                //find appropriate hole for pause
                                DateTime startPause = new DateTime(0);
                                DateTime endPause = new DateTime(0);
                                double curentDuration = 0;
                                foreach (IOPairTO iopairHole in holesList)
                                {
                                    /*DateTime startTime = iopairHole.StartTime.Subtract(new TimeSpan(0, 0, iopairHole.StartTime.Second));
                                    DateTime endTime = iopairHole.EndTime.Subtract(new TimeSpan(0, 0, iopairHole.EndTime.Second));
                                    TimeSpan holeDuration = (new DateTime(endTime.Ticks - startTime.Ticks)).TimeOfDay;*/
                                    TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                                    if ((holeDuration.TotalMinutes >= (double)duration)
                                        && (holeDuration.TotalMinutes <= (double)(duration + tolerancy)))
                                    {
                                        if (holeDuration.TotalMinutes > curentDuration)
                                        {
                                            startPause = iopairHole.StartTime;
                                            endPause = iopairHole.EndTime;
                                            curentDuration = holeDuration.TotalMinutes;
                                        }
                                    }
                                }

                                if (curentDuration == 0)
                                {
                                    foreach (IOPairTO iopairHole in holesList)
                                    {
                                        /*DateTime startTime = iopairHole.StartTime.Subtract(new TimeSpan(0, 0, iopairHole.StartTime.Second));
                                        DateTime endTime = iopairHole.EndTime.Subtract(new TimeSpan(0, 0, iopairHole.EndTime.Second));
                                        TimeSpan holeDuration = (new DateTime(endTime.Ticks - startTime.Ticks)).TimeOfDay;*/
                                        TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                                        if (holeDuration.TotalMinutes > (double)(duration + tolerancy))
                                        {
                                            if ((curentDuration == 0) || (holeDuration.TotalMinutes < curentDuration))
                                            {
                                                startPause = iopairHole.StartTime;
                                                endPause = iopairHole.StartTime.AddMinutes((double)duration);
                                                curentDuration = holeDuration.TotalMinutes;
                                            }
                                        }
                                    }
                                }

                                if (curentDuration == 0)
                                {
                                    foreach (IOPairTO iopairHole in holesList)
                                    {
                                        /*DateTime startTime = iopairHole.StartTime.Subtract(new TimeSpan(0, 0, iopairHole.StartTime.Second));
                                        DateTime endTime = iopairHole.EndTime.Subtract(new TimeSpan(0, 0, iopairHole.EndTime.Second));
                                        TimeSpan holeDuration = (new DateTime(endTime.Ticks - startTime.Ticks)).TimeOfDay;*/
                                        TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                                        if (holeDuration.TotalMinutes > curentDuration)
                                        {
                                            startPause = iopairHole.StartTime;
                                            endPause = iopairHole.EndTime;
                                            curentDuration = holeDuration.TotalMinutes;
                                        }
                                    }
                                }

                                int insertedCount = -1;
                                bool inserted = false;
                                if (addPairsFromPrevDay)
                                {
                                    //it is night shift

                                    if (startPause.Date < endPause.Date)
                                    {
                                        //if pause is in 2 days (arround midnight), insert 2 pairs
                                        //one ended with 23:59, another started with 00:00
                                        DateTime endDate = new DateTime(startPause.Year, startPause.Month,
                                            startPause.Day, 23, 59, 0);
                                        DateTime startDate = new DateTime(endPause.Year, endPause.Month,
                                            endPause.Day, 0, 0, 0);

                                        IOPair tempIOPair = new IOPair();
                                        bool trans = false;
                                        if (transaction != null)
                                        {
                                            tempIOPair.SetTransaction(transaction);
                                            trans = true;
                                        }
                                        else
                                        {
                                            trans = tempIOPair.BeginTransaction();
                                        }

                                        if (trans)
                                        {
                                            insertedCount = tempIOPair.Save(-1, day.Date.Subtract(new TimeSpan(1, 0, 0, 0)), employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                startPause, endDate,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                            inserted = ((insertedCount > 0) ? true : false);

                                            if (inserted)
                                            {
                                                insertedCount = tempIOPair.Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                    startDate, endPause,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                                inserted = ((insertedCount > 0) ? true : false) && inserted;
                                            }

                                            if (transaction == null)
                                            {
                                                if (inserted)
                                                {
                                                    tempIOPair.CommitTransaction();
                                                }
                                                else
                                                {
                                                    tempIOPair.RollbackTransaction();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Debug
                                            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                                            DebugLog debug = new DebugLog(logFilePath);
                                            debug.writeLog(DateTime.Now + " Error in: calculatePause() : cannot start transaction for "
                                                + "employee: " + employeeID.ToString() + ", day: " + day.ToString("yyyy/MM/dd") + ", interval: " + i.ToString() + "\n");
                                        }
                                    }
                                    else
                                    {
                                        //otherwise insert one pair
                                        if (transaction != null)
                                        {
                                            IOPair tempIOPair = new IOPair();
                                            tempIOPair.SetTransaction(transaction);
                                            insertedCount = tempIOPair.Save(-1, startPause.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                startPause, endPause,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                        }
                                        else
                                        {
                                            insertedCount = (new IOPair()).Save(-1, startPause.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                startPause, endPause,-1, (int)Constants.recordCreated.Automaticaly, true);
                                        }
                                        inserted = ((insertedCount > 0) ? true : false);
                                    }
                                }
                                else
                                {
                                    //it is not a night shift, insert one pair
                                    if (transaction != null)
                                    {
                                        IOPair tempIOPair = new IOPair();
                                        tempIOPair.SetTransaction(transaction);
                                        insertedCount = tempIOPair.Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                            startPause, endPause,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                    }
                                    else
                                    {
                                        insertedCount = (new IOPair()).Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                            startPause, endPause,-1, (int)Constants.recordCreated.Automaticaly, true);
                                    }
                                    inserted = ((insertedCount > 0) ? true : false);
                                }

                                if (!inserted)
                                {
                                    // Debug
                                    string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                                    DebugLog debug = new DebugLog(logFilePath);
                                    debug.writeLog(DateTime.Now + " Error in: calculatePause() : Can't insert pause for "
                                        + "employee: " + employeeID.ToString() + ", day: " + day.ToString("yyyy/MM/dd") + ", interval: " + i.ToString() + "\n");
                                }

                            } //for (int i = 0; i < dayIntervals.Count; i++)
                        } //for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                    } //foreach(int employeeID in employeesID)
                } //if (ioPairList.Count > 0)
            } // try
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.calculatePause(): " + ex.Message + "--- For employeeID "+currEmplID.ToString()+" and day "+currentDay.ToString("dd.MM.yyyy.")+"\n");
                throw new Exception(ex.Message);
            }
        }
        //ioPairList must be Sort by employee_id, io_pair_date, start_time
        //and contains all io_pairs (including open pairs)
        //for each employee and date, ioPairList contains all pairs for that day and day before (for night shift)
        //employeesAndDates - key is employeeID, value is Hashtable with dates for that employee
        private void calculatePause(List<IOPairTO> ioPairList, Hashtable employeesAndDates, Dictionary<int, WorkTimeSchemaTO> timeSchemaTable, IDbTransaction transaction, object dbConnection)
        {
            try
            {
                if (ioPairList.Count > 0)
                {
                    //list of time schedules for all employees, for selected interval
                    List<EmployeeTimeScheduleTO> timeSchedules = new List<EmployeeTimeScheduleTO>();

                    // all employee time schedules for selected Time Interval, key is employee ID
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();

                    //all pauses
                    List<TimeSchemaPauseTO> pauses = new TimeSchemaPause(dbConnection).Search("", transaction);

                    DateTime fromDay = new DateTime(0);
                    DateTime toDay = new DateTime(0);

                    List<int> employeesID = new List<int>();
                    string employeeIDString = "";

                    // Key is Employee ID, value is List of valid IO Pairs for that Employee
                    Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>();

                    // io pairs for particular employee are sorted by io_pair_date ascending
                    for (int i = 0; i < ioPairList.Count; i++)
                    {
                        int currentEmployeeID = ioPairList[i].EmployeeID;
                        if (!emplPairs.ContainsKey(currentEmployeeID))
                        {
                            employeesID.Add(currentEmployeeID);
                            employeeIDString += currentEmployeeID.ToString().Trim() + ",";

                            emplPairs.Add(currentEmployeeID, new List<IOPairTO>());
                            emplTimeSchedules.Add(currentEmployeeID, new List<EmployeeTimeScheduleTO>());
                        }
                        if ((fromDay == new DateTime(0)) || (fromDay.Date > ioPairList[i].IOPairDate.Date))
                            fromDay = ioPairList[i].IOPairDate;
                        if ((toDay == new DateTime(0)) || (toDay.Date < ioPairList[i].IOPairDate.Date))
                            toDay = ioPairList[i].IOPairDate;

                        emplPairs[currentEmployeeID].Add(ioPairList[i]);
                    }
                    if (employeeIDString.Length > 0)
                    {
                        employeeIDString = employeeIDString.Substring(0, employeeIDString.Length - 1);
                    }

                    //in ioPairList, for each day, there are pairs from previous day also, so,
                    //from day is not minimum day, but minimum day + 1
                    //but, if there were no ioPairs from previous day, then, from day is as it should be
                    //the best is not to add 1 to from day, anyway, calculation is done only for the employee's days
                    //fromDay = fromDay.AddDays(1);

                    //NOTE: this is changed now, pairs from the previous day are also in ioPairList (not get from database here)
                    //and everything is sorted ok
                    //for the first day, in case it is night shift, pairs from the previous day are needed
                    //for the last day ignore night shift, because pair for the next day do not exist yet
                    /*TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
                    ArrayList firstDayPairs = (new IOPair()).SearchAll(fromDay.Subtract(oneDay), fromDay.Subtract(oneDay), employeesID);
                    for (int i = 0; i < firstDayPairs.Count; i++)
                    {
                        int currentEmployeeID = ((TransferObjects.IOPairTO) firstDayPairs[i]).EmployeeID;
				
                        ((ArrayList) (emplPairs[currentEmployeeID])).Add((TransferObjects.IOPairTO) firstDayPairs[i]);
                    }*/

                    //get time schemas for selected Employees, for selected Time Interval
                    timeSchedules = new EmployeesTimeSchedule(dbConnection).SearchEmployeesSchedules(employeeIDString, fromDay, toDay, transaction);
                    for (int i = 0; i < timeSchedules.Count; i++)
                    {
                        emplTimeSchedules[timeSchedules[i].EmployeeID].Add(timeSchedules[i]);
                    }

                    //Start calculation
                    //for each employee, day, interval in that day
                    foreach (int employeeID in employeesID)
                    {
                        if (!emplPairs.ContainsKey(employeeID) || (emplPairs.ContainsKey(employeeID) && emplPairs[employeeID].Count <= 0))
                            continue;

                        if (!employeesAndDates.ContainsKey(employeeID))
                            continue;

                        Hashtable employeeDates = (Hashtable)employeesAndDates[employeeID];

                        for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                        {
                            //calculate pause only for employee's dates
                            if (!employeeDates.ContainsKey(day))
                                continue;

                            bool is2DaysShift = false;
                            bool is2DaysShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref is2DaysShift,
                                ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemaTable);

                            if (dayIntervals == null)
                                continue;

                            for (int i = 0; i < dayIntervals.Count; i++)
                            {
                                WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i];
                                if (timeSchemaTable.ContainsKey(currentTimeSchemaInterval.TimeSchemaID))
                                    if (timeSchemaTable[currentTimeSchemaInterval.TimeSchemaID].Type.Equals(Constants.schemaTypeFlexi))
                                    {
                                        currentTimeSchemaInterval.StartTime = currentTimeSchemaInterval.EarliestArrived;
                                        currentTimeSchemaInterval.EndTime = currentTimeSchemaInterval.LatestLeft;
                                    }
                            }


                            //if previous day is night shift day, take that night shift interval
                            WorkTimeIntervalTO lastIntervalPreviousDay = new WorkTimeIntervalTO();
                            if (is2DaysShiftPrevious)
                                lastIntervalPreviousDay = getLastIntervalPrevDay(emplTimeSchedules[employeeID], day, timeSchemaTable);

                            //if this day is night shift day, find index of night shift interval
                            int lastIntervalIndex = -1;
                            if (is2DaysShift)
                            {
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                    {
                                        lastIntervalIndex = i;
                                        break;
                                    }
                                }
                            }

                            //if previous day is night shift day, find index of interval in this day that is part of
                            //previous day night shift
                            int partOfNightShiftIndex = -1;
                            if (is2DaysShiftPrevious)
                            {
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                    if ((currentTimeSchemaInterval.StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                    {
                                        partOfNightShiftIndex = i;
                                        break;
                                    }
                                }
                            }

                            //Take only IO Pairs for specific day, considering night shifts
                            List<IOPairTO> dayIOPairList = getEmployeeExactDayPairs(emplPairs[employeeID], day, is2DaysShiftPrevious, lastIntervalPreviousDay);

                            if (dayIOPairList.Count <= 0)
                                continue;

                            //do not calculate pause if day has at least one open interval
                            bool openPairExist = existEmployeeDayOpenPairs(dayIOPairList);
                            if (openPairExist)
                                continue;

                            for (int i = 0; i < dayIntervals.Count; i++)
                            {
                                WorkTimeIntervalTO tsInterval = dayIntervals[i]; //key is interval_num which is 0, 1...

                                //if this is night shift day, skip night shift interval, it will 
                                //be calculated together with next day
                                if (is2DaysShift && (i == lastIntervalIndex))
                                    continue;

                                //use pairs from previous day only if previous day is night shift
                                //and current interval is part of night shift
                                bool addPairsFromPrevDay = false;
                                if (is2DaysShiftPrevious && (lastIntervalPreviousDay != null)
                                    && (i == partOfNightShiftIndex))
                                    addPairsFromPrevDay = true;

                                //get pause informations (for night shift, informations are with first interval)
                                int duration = -1;
                                int tolerancy = -1;
                                int shortBreak = -1;
                                DateTime pauseEarliestTime = new DateTime(0);
                                DateTime pauseLatestTime = new DateTime(0);
                                int pauseID = tsInterval.PauseID;
                                if (addPairsFromPrevDay)
                                    pauseID = lastIntervalPreviousDay.PauseID;

                                if (pauseID == -1)
                                    continue;

                                foreach (TimeSchemaPauseTO currentPause in pauses)
                                {
                                    if (pauseID == currentPause.PauseID)
                                    {
                                        duration = currentPause.PauseDuration;
                                        tolerancy = currentPause.PauseOffset;
                                        shortBreak = currentPause.ShortBreakDuration;

                                        WorkTimeIntervalTO newInterval = new WorkTimeIntervalTO();

                                        //calculate pause latest use time, it is always in tsInterval
                                        newInterval.EndTime = new DateTime(day.Year, day.Month, day.Day);
                                        newInterval.EndTime = newInterval.EndTime.Add(tsInterval.EndTime.TimeOfDay);
                                        pauseLatestTime = newInterval.EndTime.Subtract(new TimeSpan(0, currentPause.LatestUseTime, 0));

                                        //calculate pause earliest use time, it is in lastIntervalPreviousDay for night shift
                                        //otherwise, it is in tsInterval
                                        newInterval.StartTime = new DateTime(day.Year, day.Month, day.Day);
                                        if (addPairsFromPrevDay)
                                        {
                                            //if it is night shift
                                            newInterval.StartTime = newInterval.StartTime.Subtract(new TimeSpan(1, 0, 0, 0));
                                            newInterval.StartTime = newInterval.StartTime.Add(lastIntervalPreviousDay.StartTime.TimeOfDay);
                                        }
                                        else
                                        {
                                            newInterval.StartTime = newInterval.StartTime.Add(tsInterval.StartTime.TimeOfDay);
                                        }
                                        pauseEarliestTime = newInterval.StartTime.AddMinutes(currentPause.EarliestUseTime);
                                        break;
                                    }
                                }

                                //get only interval pairs, considering night shift
                                List<IOPairTO> intervalIoPairList = getEmployeeIntervalPairs(dayIOPairList, tsInterval, lastIntervalPreviousDay,
                                    addPairsFromPrevDay, day);
                                if (intervalIoPairList.Count < 2)
                                    continue;

                                List<IOPairTO> intervalIoPairListClone = new List<IOPairTO>();
                                foreach (IOPairTO iop in intervalIoPairList)
                                {
                                    intervalIoPairListClone.Add(iop);
                                }

                                //make new ioPairList (trimmedIoPairs) considering overlaping of intervals
                                List<IOPairTO> trimmedIoPairs = new List<IOPairTO>();
                                List<int> equalPairsID = new List<int>();
                                foreach (IOPairTO iopairTO in intervalIoPairList)
                                {
                                    if (!equalPairsID.Contains(iopairTO.IOPairID))
                                    {
                                        IOPairTO newioPair = new IOPairTO(iopairTO);
                                        newioPair.IOPairID = 0;
                                        newioPair.IsWrkHrsCount = 0;
                                        newioPair.LocationID = 0;
                                        newioPair.LocationName = "";
                                        newioPair.ManualCreated = 0;
                                        newioPair.PassType = "";
                                        newioPair.PassTypeID = 0;
                                        newioPair.WUName = "";

                                        bool insideOtherPair = false;
                                        foreach (IOPairTO iopairTOClone in intervalIoPairListClone)
                                        {
                                            if (iopairTOClone.IOPairID != iopairTO.IOPairID)
                                            {
                                                if ((iopairTOClone.StartTime.TimeOfDay == iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay == iopairTO.EndTime.TimeOfDay))
                                                {
                                                    //register that for iopairTO pair exist iopairTOClone pair with the same
                                                    //start and end time, so, do not do calculation again for iopairTOClone
                                                    equalPairsID.Add(iopairTOClone.IOPairID);
                                                }

                                                if ((iopairTOClone.StartTime.TimeOfDay <= iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay >= iopairTO.EndTime.TimeOfDay)
                                                    && !((iopairTOClone.StartTime.TimeOfDay == iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay == iopairTO.EndTime.TimeOfDay)))
                                                {
                                                    //iopairTO pair is inside samo other iopairTOClone pair
                                                    insideOtherPair = true;
                                                    break;
                                                }

                                                if ((iopairTOClone.StartTime.TimeOfDay < iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay >= iopairTO.StartTime.TimeOfDay)
                                                    && (iopairTOClone.StartTime.TimeOfDay < newioPair.StartTime.TimeOfDay))
                                                {
                                                    //iopairTO and iopairTOClone pairs are overlaping, adjust start time
                                                    newioPair.StartTime = iopairTOClone.StartTime;
                                                }

                                                if ((iopairTOClone.StartTime.TimeOfDay <= iopairTO.EndTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay > iopairTO.EndTime.TimeOfDay)
                                                    && (iopairTOClone.EndTime.TimeOfDay > newioPair.EndTime.TimeOfDay))
                                                {
                                                    //iopairTO and iopairTOClone pairs are overlaping, adjust end time
                                                    newioPair.EndTime = iopairTOClone.EndTime;
                                                }
                                            } //if (iopairTOClone.IOPairID != iopairTO.IOPairID)
                                        } //foreach (TransferObjects.IOPairTO iopairTOClone in intervalIoPairListClone)

                                        if (insideOtherPair)
                                            continue;

                                        if (trimmedIoPairs.Contains(newioPair))
                                            continue;

                                        //check if this new pair already exist in trimmedIoPairs, or maybe it
                                        //overlaps with some pair there
                                        //Do necesery changes acording to that
                                        bool existInTrimStart = false;
                                        bool existInTrimEnd = false;
                                        foreach (IOPairTO iopairTOTrimm in trimmedIoPairs)
                                        {
                                            if ((iopairTOTrimm.StartTime.TimeOfDay > newioPair.StartTime.TimeOfDay)
                                                && (iopairTOTrimm.StartTime.TimeOfDay <= newioPair.EndTime.TimeOfDay))
                                            {
                                                iopairTOTrimm.StartTime = newioPair.StartTime;
                                                existInTrimStart = true;
                                            }

                                            if ((iopairTOTrimm.EndTime.TimeOfDay >= newioPair.StartTime.TimeOfDay)
                                                && (iopairTOTrimm.EndTime.TimeOfDay < newioPair.EndTime.TimeOfDay))
                                            {
                                                iopairTOTrimm.EndTime = newioPair.EndTime;
                                                existInTrimEnd = true;
                                            }

                                            if ((iopairTOTrimm.StartTime.TimeOfDay <= newioPair.StartTime.TimeOfDay)
                                                && (iopairTOTrimm.EndTime.TimeOfDay >= newioPair.EndTime.TimeOfDay))
                                            {
                                                existInTrimStart = true;
                                                existInTrimEnd = true;
                                            }
                                        }

                                        if (existInTrimStart || existInTrimEnd)
                                            continue;

                                        trimmedIoPairs.Add(newioPair);
                                    } //if (!equalPairsID.Contains(iopairTO.IOPairID))
                                } //foreach (TransferObjects.IOPairTO iopairTO in intervalIoPairList)

                                if (trimmedIoPairs.Count < 2)
                                    continue;

                                //since iopairs are sorted by start_time, trimmedIOPAirs are also sorted, so, holes are between them
                                //holes will be new IOPairs

                                //create short breaks first
                                if (shortBreak > 0)
                                    trimmedIoPairs = insertShortBreaks(employeeID, day, trimmedIoPairs, shortBreak, addPairsFromPrevDay, transaction);

                                if (trimmedIoPairs.Count < 2)
                                    continue;

                                if (duration <= 0)
                                    continue;

                                //create candidates for holes
                                List<IOPairTO> holesList = new List<IOPairTO>();
                                for (int j = 1; j < trimmedIoPairs.Count; j++)
                                {
                                    IOPairTO newioPair = new IOPairTO();
                                    newioPair.StartTime = trimmedIoPairs[j - 1].EndTime;
                                    newioPair.EndTime = trimmedIoPairs[j].StartTime;

                                    //if it is night shift, ignore hole (23:59 - 00:00)
                                    if (addPairsFromPrevDay &&
                                        (newioPair.StartTime == new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month,
                                        newioPair.StartTime.Day, 23, 59, 0))
                                        && (newioPair.EndTime == new DateTime(newioPair.EndTime.Year, newioPair.EndTime.Month,
                                        newioPair.EndTime.Day, 0, 0, 0))
                                        )
                                        continue;

                                    if (addPairsFromPrevDay && (newioPair.StartTime == new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month,
                                        newioPair.StartTime.Day, 23, 59, 0)))
                                        newioPair.StartTime = newioPair.StartTime.AddMinutes(1);

                                    if (addPairsFromPrevDay && (newioPair.EndTime == new DateTime(newioPair.EndTime.Year, newioPair.EndTime.Month,
                                        newioPair.EndTime.Day, 0, 0, 0)))
                                        newioPair.EndTime = newioPair.EndTime.Subtract(new TimeSpan(0, 1, 0));

                                    if (newioPair.StartTime.Date < pauseEarliestTime.Date)
                                        continue;
                                    if (newioPair.EndTime.Date > pauseLatestTime.Date)
                                        continue;

                                    //adjust start time according to pause earliest use time
                                    if ((newioPair.StartTime.TimeOfDay < pauseEarliestTime.TimeOfDay)
                                        && (newioPair.EndTime.TimeOfDay > pauseEarliestTime.TimeOfDay)
                                        && (newioPair.StartTime.Date == pauseEarliestTime.Date))
                                        newioPair.StartTime = pauseEarliestTime;

                                    //adjust end time according to pause latest use time
                                    if ((newioPair.EndTime.TimeOfDay > pauseLatestTime.TimeOfDay)
                                        && (newioPair.StartTime.TimeOfDay < pauseLatestTime.TimeOfDay)
                                        && (newioPair.EndTime.Date == pauseLatestTime.Date))
                                        newioPair.EndTime = pauseLatestTime;

                                    if ((newioPair.StartTime.TimeOfDay < pauseEarliestTime.TimeOfDay)
                                        && (newioPair.StartTime.Date == pauseEarliestTime.Date))
                                        continue;

                                    /*DateTime newioPairStartTime = newioPair.StartTime.Subtract(new TimeSpan(0, 0, newioPair.StartTime.Second));
                                    DateTime newioPairEndTime = newioPair.EndTime.Subtract(new TimeSpan(0, 0, newioPair.EndTime.Second));
                                    TimeSpan newioPairDuration = (new DateTime(newioPairEndTime.Ticks - newioPairStartTime.Ticks)).TimeOfDay;*/
                                    /*TimeSpan newioPairDuration = (new DateTime(newioPair.EndTime.Ticks - newioPair.StartTime.Ticks)).TimeOfDay;

                                    DateTime newioPairEndDate = new DateTime();
                                    if (newioPairDuration.TotalMinutes > (double)duration)
                                        newioPairEndDate = newioPair.StartTime.AddMinutes((double) duration);
                                    else
                                        newioPairEndDate = newioPair.EndTime;
                                    if ((newioPairEndDate.TimeOfDay > pauseLatestTime.TimeOfDay)
                                        && (newioPairEndDate.Date == pauseLatestTime.Date))
                                        continue;*/
                                    if ((newioPair.EndTime.TimeOfDay > pauseLatestTime.TimeOfDay)
                                        && (newioPair.EndTime.Date == pauseLatestTime.Date))
                                        continue;

                                    holesList.Add(newioPair);
                                }

                                if (holesList.Count <= 0)
                                    continue;

                                //find appropriate hole for pause
                                DateTime startPause = new DateTime(0);
                                DateTime endPause = new DateTime(0);
                                double curentDuration = 0;
                                foreach (IOPairTO iopairHole in holesList)
                                {
                                    /*DateTime startTime = iopairHole.StartTime.Subtract(new TimeSpan(0, 0, iopairHole.StartTime.Second));
                                    DateTime endTime = iopairHole.EndTime.Subtract(new TimeSpan(0, 0, iopairHole.EndTime.Second));
                                    TimeSpan holeDuration = (new DateTime(endTime.Ticks - startTime.Ticks)).TimeOfDay;*/
                                    TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                                    if ((holeDuration.TotalMinutes >= (double)duration)
                                        && (holeDuration.TotalMinutes <= (double)(duration + tolerancy)))
                                    {
                                        if (holeDuration.TotalMinutes > curentDuration)
                                        {
                                            startPause = iopairHole.StartTime;
                                            endPause = iopairHole.EndTime;
                                            curentDuration = holeDuration.TotalMinutes;
                                        }
                                    }
                                }

                                if (curentDuration == 0)
                                {
                                    foreach (IOPairTO iopairHole in holesList)
                                    {
                                        /*DateTime startTime = iopairHole.StartTime.Subtract(new TimeSpan(0, 0, iopairHole.StartTime.Second));
                                        DateTime endTime = iopairHole.EndTime.Subtract(new TimeSpan(0, 0, iopairHole.EndTime.Second));
                                        TimeSpan holeDuration = (new DateTime(endTime.Ticks - startTime.Ticks)).TimeOfDay;*/
                                        TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                                        if (holeDuration.TotalMinutes > (double)(duration + tolerancy))
                                        {
                                            if ((curentDuration == 0) || (holeDuration.TotalMinutes < curentDuration))
                                            {
                                                startPause = iopairHole.StartTime;
                                                endPause = iopairHole.StartTime.AddMinutes((double)duration);
                                                curentDuration = holeDuration.TotalMinutes;
                                            }
                                        }
                                    }
                                }

                                if (curentDuration == 0)
                                {
                                    foreach (IOPairTO iopairHole in holesList)
                                    {
                                        /*DateTime startTime = iopairHole.StartTime.Subtract(new TimeSpan(0, 0, iopairHole.StartTime.Second));
                                        DateTime endTime = iopairHole.EndTime.Subtract(new TimeSpan(0, 0, iopairHole.EndTime.Second));
                                        TimeSpan holeDuration = (new DateTime(endTime.Ticks - startTime.Ticks)).TimeOfDay;*/
                                        TimeSpan holeDuration = (new DateTime(iopairHole.EndTime.Ticks - iopairHole.StartTime.Ticks)).TimeOfDay;

                                        if (holeDuration.TotalMinutes > curentDuration)
                                        {
                                            startPause = iopairHole.StartTime;
                                            endPause = iopairHole.EndTime;
                                            curentDuration = holeDuration.TotalMinutes;
                                        }
                                    }
                                }

                                int insertedCount = -1;
                                bool inserted = false;
                                if (addPairsFromPrevDay)
                                {
                                    //it is night shift

                                    if (startPause.Date < endPause.Date)
                                    {
                                        //if pause is in 2 days (arround midnight), insert 2 pairs
                                        //one ended with 23:59, another started with 00:00
                                        DateTime endDate = new DateTime(startPause.Year, startPause.Month,
                                            startPause.Day, 23, 59, 0);
                                        DateTime startDate = new DateTime(endPause.Year, endPause.Month,
                                            endPause.Day, 0, 0, 0);

                                        IOPair tempIOPair = new IOPair(dbConnection);
                                        bool trans = false;
                                        if (transaction != null)
                                        {
                                            tempIOPair.SetTransaction(transaction);
                                            trans = true;
                                        }
                                        else
                                        {
                                            trans = tempIOPair.BeginTransaction();
                                        }

                                        if (trans)
                                        {
                                            insertedCount = tempIOPair.Save(-1, day.Date.Subtract(new TimeSpan(1, 0, 0, 0)), employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                startPause, endDate,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                            inserted = ((insertedCount > 0) ? true : false);

                                            if (inserted)
                                            {
                                                insertedCount = tempIOPair.Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                    startDate, endPause,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                                inserted = ((insertedCount > 0) ? true : false) && inserted;
                                            }

                                            if (transaction == null)
                                            {
                                                if (inserted)
                                                {
                                                    tempIOPair.CommitTransaction();
                                                }
                                                else
                                                {
                                                    tempIOPair.RollbackTransaction();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Debug
                                            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                                            DebugLog debug = new DebugLog(logFilePath);
                                            debug.writeLog(DateTime.Now + " Error in: calculatePause() : cannot start transaction for "
                                                + "employee: " + employeeID.ToString() + ", day: " + day.ToString("yyyy/MM/dd") + ", interval: " + i.ToString() + "\n");
                                        }
                                    }
                                    else
                                    {
                                        //otherwise insert one pair
                                        if (transaction != null)
                                        {
                                            IOPair tempIOPair = new IOPair(dbConnection);
                                            tempIOPair.SetTransaction(transaction);
                                            insertedCount = tempIOPair.Save(-1, startPause.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                startPause, endPause,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                        }
                                        else
                                        {
                                            insertedCount = (new IOPair(dbConnection)).Save(-1, startPause.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                                startPause, endPause,-1, (int)Constants.recordCreated.Automaticaly, true);
                                        }
                                        inserted = ((insertedCount > 0) ? true : false);
                                    }
                                }
                                else
                                {
                                    //it is not a night shift, insert one pair
                                    if (transaction != null)
                                    {
                                        IOPair tempIOPair = new IOPair(dbConnection);
                                        tempIOPair.SetTransaction(transaction);
                                        insertedCount = tempIOPair.Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                            startPause, endPause,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                    }
                                    else
                                    {
                                        insertedCount = (new IOPair(dbConnection)).Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticPausePassType, (int)Constants.IsWrkCount.IsCounter,
                                            startPause, endPause,-1, (int)Constants.recordCreated.Automaticaly, true);
                                    }
                                    inserted = ((insertedCount > 0) ? true : false);
                                }

                                if (!inserted)
                                {
                                    // Debug
                                    string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                                    DebugLog debug = new DebugLog(logFilePath);
                                    debug.writeLog(DateTime.Now + " Error in: calculatePause() : Can't insert pause for "
                                        + "employee: " + employeeID.ToString() + ", day: " + day.ToString("yyyy/MM/dd") + ", interval: " + i.ToString() + "\n");
                                }

                            } //for (int i = 0; i < dayIntervals.Count; i++)
                        } //for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                    } //foreach(int employeeID in employeesID)
                } //if (ioPairList.Count > 0)
            } // try
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.calculatePause(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        //calculate and insert short breaks for current employee and day
        //intervalIoPairList must have at least 2 members
        //employeeID is current employee
        //day is current day
        //intervalIoPairList are io pairs for current employee, day and interval, considering night shift
        //shortBreak is durration of short break
        //addPairsFromPrevDay - logical, true if previous day is a night shift and lastIntervalPreviousDay != null
        public List<IOPairTO> insertShortBreaks(int employeeID, DateTime day, List<IOPairTO> intervalIoPairList,
            int shortBreak, bool addPairsFromPrevDay, IDbTransaction transaction)
        {
            try
            {
                //contains all ioPairs from intervalIoPairList, but also new one created 
                //for short breaks, taking care that it is still sorted by start time and no overlaps of intervals
                List<IOPairTO> newIOPairList = new List<IOPairTO>();
                if (employeeID == 339)
                {
                }
                //intervalIoPairList has at least 2 members
                newIOPairList.Add(intervalIoPairList[0]);
                for (int j = 1; j < intervalIoPairList.Count; j++)
                {
                    IOPairTO newioPair = new IOPairTO();
                    newioPair.StartTime = intervalIoPairList[j - 1].EndTime;
                    newioPair.EndTime = intervalIoPairList[j].StartTime;

                    //VIKOR DODAO ZATO STO CEO SERVIS PUCA KADA JE StartTime VECI OD 23:00
                    if (newioPair.StartTime > new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month, newioPair.StartTime.Day, 23, 59, 0))
                    {
                        newioPair.StartTime = new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month, newioPair.StartTime.Day, 23, 59, 0);
                    }
                    /*DateTime startTime = newioPair.StartTime.Subtract(new TimeSpan(0, 0, newioPair.StartTime.Second));
                    DateTime endTime = newioPair.EndTime.Subtract(new TimeSpan(0, 0, newioPair.EndTime.Second));
                    TimeSpan holeDuration = (new DateTime(endTime.Ticks - startTime.Ticks)).TimeOfDay;*/
                    TimeSpan holeDuration = (new DateTime(newioPair.EndTime.Ticks - newioPair.StartTime.Ticks)).TimeOfDay;

                    bool adjustStartTime = false;
                    if (addPairsFromPrevDay && (newioPair.StartTime == new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month,
                        newioPair.StartTime.Day, 23, 59, 0)))
                    {
                        //for night shift, if ioPair starts at 23:59:00, adjust it to 00:00:00
                        adjustStartTime = true;
                        //substract one minute from 23:59 to 00:00
                        holeDuration = holeDuration.Subtract(new TimeSpan(0, 1, 0));
                    }

                    bool adjustEndTime = false;
                    if (addPairsFromPrevDay && (newioPair.EndTime == new DateTime(newioPair.EndTime.Year, newioPair.EndTime.Month,
                        newioPair.EndTime.Day, 0, 0, 0)))
                    {
                        //for night shift, if ioPair ends at 00:00:00, adjust it to 23:59:00
                        adjustEndTime = true;
                    }

                    //check if this is hole for short break
                    //if it is night shift, ignore hole (23:59 - 00:00)
                    if ((holeDuration.TotalMinutes > 0)
                        && (holeDuration.TotalMinutes <= (double)shortBreak)
                        &&
                        ((!addPairsFromPrevDay)
                        || (addPairsFromPrevDay &&
                        ((newioPair.StartTime != new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month,
                        newioPair.StartTime.Day, 23, 59, 0))
                        || (newioPair.EndTime != new DateTime(newioPair.EndTime.Year, newioPair.EndTime.Month,
                        newioPair.EndTime.Day, 0, 0, 0))
                        ))
                        ))
                    {
                        int insertedCount = -1;
                        bool inserted = false;
                        if (addPairsFromPrevDay)
                        {
                            //it is night shift
                            if ((newioPair.StartTime.Date < newioPair.EndTime.Date)
                                && (!adjustStartTime) && (!adjustEndTime))
                            {
                                //if short break is in 2 days (arround midnight), insert 2 pairs
                                //one ended with 23:59, another started with 00:00
                                DateTime endDate = new DateTime(newioPair.StartTime.Year, newioPair.StartTime.Month,
                                    newioPair.StartTime.Day, 23, 59, 0);
                                DateTime startDate = new DateTime(newioPair.EndTime.Year, newioPair.EndTime.Month,
                                    newioPair.EndTime.Day, 0, 0, 0);

                                IOPair tempIOPair = new IOPair();
                                bool trans = false;
                                if (transaction != null)
                                {
                                    tempIOPair.SetTransaction(transaction);
                                    trans = true;
                                }
                                else
                                {
                                    trans = tempIOPair.BeginTransaction();
                                }
                                if (trans)
                                {
                                    insertedCount = tempIOPair.Save(-1, day.Date.Subtract(new TimeSpan(1, 0, 0, 0)), employeeID, Constants.defaultLocID, Constants.automaticShortBreakPassType, (int)Constants.IsWrkCount.IsCounter,
                                        newioPair.StartTime, endDate,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                    inserted = ((insertedCount > 0) ? true : false);

                                    if (inserted)
                                    {
                                        insertedCount = tempIOPair.Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticShortBreakPassType, (int)Constants.IsWrkCount.IsCounter,
                                            startDate, newioPair.EndTime,-1, (int)Constants.recordCreated.Automaticaly, false);
                                        inserted = ((insertedCount > 0) ? true : false) && inserted;
                                    }

                                    if (transaction == null)
                                    {
                                        if (inserted)
                                        {
                                            tempIOPair.CommitTransaction();
                                        }
                                        else
                                        {
                                            tempIOPair.RollbackTransaction();
                                        }
                                    }
                                }
                                else
                                {
                                    // Debug
                                    string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                                    DebugLog debug = new DebugLog(logFilePath);
                                    debug.writeLog(DateTime.Now + " Error in: insertShortBreaks() : cannot start transaction for "
                                        + "employee: " + employeeID.ToString() + ", day: " + day.ToString("yyyy/MM/dd") + "\n");
                                }
                            }
                            else
                            {
                                //otherwise insert one pair

                                if (adjustStartTime)
                                    //adjust start time from 23:59 to 00:00
                                    newioPair.StartTime = newioPair.StartTime.AddMinutes(1);

                                if (adjustEndTime)
                                    //adjust end time from 00:00 to 23:59
                                    newioPair.EndTime = newioPair.EndTime.Subtract(new TimeSpan(0, 1, 0));

                                if (transaction != null)
                                {
                                    IOPair tempIOPair = new IOPair();
                                    tempIOPair.SetTransaction(transaction);
                                    insertedCount = tempIOPair.Save(-1, newioPair.StartTime.Date, employeeID, Constants.defaultLocID, Constants.automaticShortBreakPassType, (int)Constants.IsWrkCount.IsCounter,
                                        newioPair.StartTime, newioPair.EndTime,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                                }
                                else
                                {
                                    insertedCount = (new IOPair()).Save(-1, newioPair.StartTime.Date, employeeID, Constants.defaultLocID, Constants.automaticShortBreakPassType, (int)Constants.IsWrkCount.IsCounter,
                                        newioPair.StartTime, newioPair.EndTime,-1, (int)Constants.recordCreated.Automaticaly, true);
                                }
                                inserted = ((insertedCount > 0) ? true : false);
                            }
                        }
                        else
                        {
                            //it is not a night shift, insert one pair
                            if (transaction != null)
                            {
                                IOPair tempIOPair = new IOPair();
                                tempIOPair.SetTransaction(transaction);
                                insertedCount = tempIOPair.Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticShortBreakPassType, (int)Constants.IsWrkCount.IsCounter,
                                   newioPair.StartTime, newioPair.EndTime,tempIOPair.PairTO.GateID, (int)Constants.recordCreated.Automaticaly, false);
                            }
                            else
                            {
                                insertedCount = (new IOPair()).Save(-1, day.Date, employeeID, Constants.defaultLocID, Constants.automaticShortBreakPassType, (int)Constants.IsWrkCount.IsCounter,
                                    newioPair.StartTime, newioPair.EndTime,-1, (int)Constants.recordCreated.Automaticaly, true);
                            }
                            inserted = ((insertedCount > 0) ? true : false);
                        }

                        if (!inserted)
                        {
                            // Debug
                            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                            DebugLog debug = new DebugLog(logFilePath);
                            debug.writeLog(DateTime.Now + " Error in: insertShortBreaks() : Can't insert short break for "
                                + "employee: " + employeeID.ToString() + ", day: " + day.ToString("yyyy/MM/dd") + "\n");

                            //short break is not inserted, just add interval to the list of intervals
                            newIOPairList.Add((TransferObjects.IOPairTO)intervalIoPairList[j]);
                        }
                        else
                        {
                            //short break is inserted, adjust end time of last interval in the list of intervals
                            ((TransferObjects.IOPairTO)newIOPairList[newIOPairList.Count - 1]).EndTime =
                                ((TransferObjects.IOPairTO)intervalIoPairList[j]).EndTime;
                        }
                    }
                    else
                    {
                        //it is not a hole for short break, just add interval to the list of intervals
                        newIOPairList.Add((TransferObjects.IOPairTO)intervalIoPairList[j]);
                    }
                } //for

                return newIOPairList;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.insertShortBreaks(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //get all iopairs for specific employee, for specific day, including night shift 
        //ioPairList contains all iopairs for selected employee, for selected time interval
        //day is specific day
        //is2DaysShiftPrevious - logical, is previous day a night shift or not
        //lastIntervalPreviousDay is last interval of previous day (night shift interval)
        public List<IOPairTO> getEmployeeExactDayPairs(List<IOPairTO> ioPairList, DateTime day, bool is2DaysShiftPrevious,
            WorkTimeIntervalTO lastIntervalPreviousDay)
        {
            List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
            TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
            foreach (IOPairTO iopair in ioPairList)
            {
                //iopair.StartTime = iopair.StartTime.Subtract(new TimeSpan(0, 0, iopair.StartTime.Second));
                //iopair.EndTime = iopair.EndTime.Subtract(new TimeSpan(0, 0, iopair.EndTime.Second));

                //if previous day is night shift day, take also pairs from previous day
                //but only pairs that belong to night shift interval
                if ((iopair.IOPairDate.Date == day.Date)
                    ||
                    (is2DaysShiftPrevious
                    && (lastIntervalPreviousDay != null)
                    && (iopair.IOPairDate.Date == day.Date.Subtract(oneDay))
                    &&
                    ((iopair.StartTime.TimeOfDay <= lastIntervalPreviousDay.StartTime.TimeOfDay
                    && iopair.EndTime.TimeOfDay > lastIntervalPreviousDay.StartTime.TimeOfDay)
                    ||
                    (iopair.StartTime.TimeOfDay > lastIntervalPreviousDay.StartTime.TimeOfDay
                    && iopair.StartTime.TimeOfDay < lastIntervalPreviousDay.EndTime.TimeOfDay)
                    ||
                    ((iopair.StartTime == new DateTime(0)) && (iopair.EndTime.TimeOfDay > lastIntervalPreviousDay.StartTime.TimeOfDay))
                    ||
                    ((iopair.EndTime == new DateTime(0)) && (iopair.StartTime.TimeOfDay >= lastIntervalPreviousDay.EarliestArrived.TimeOfDay)))
                    ))
                    employeeDayPairs.Add(iopair);
            }

            return employeeDayPairs;
        }

        //get all iopairs for specific employee, for specific day interval  
        //dayIOPairList contains all iopairs for selected employee, for selected day
        //tsInterval is specific day interval
        //lastIntervalPreviousDay is last interval of previous day (night shift interval)
        //addPairsFromPrevDay - logical, true if previous day is a night shift and lastIntervalPreviousDay != null
        public List<IOPairTO> getEmployeeIntervalPairs(List<IOPairTO> dayIOPairList, WorkTimeIntervalTO tsInterval,
            WorkTimeIntervalTO lastIntervalPreviousDay, bool addPairsFromPrevDay, DateTime day)
        {
            List<IOPairTO> employeeIntervalPairs = new List<IOPairTO>();

            //first add pairs from previous day, so, everything will be sorted as it should be
            //because pairs from the previous day are added to the end of the list

            //NOTE: this is changed now, pairs from the previous day are also in ioPairList (not get from database here)
            //and everything is sorted ok
            if (addPairsFromPrevDay)
            {
                DateTime ioPairDate = day.Subtract(new TimeSpan(1, 0, 0, 0));
                foreach (IOPairTO iopair in dayIOPairList)
                {
                    if (iopair.IOPairDate == ioPairDate)
                    {
                        //iopair.StartTime = iopair.StartTime.Subtract(new TimeSpan(0, 0, iopair.StartTime.Second));
                        //iopair.EndTime = iopair.EndTime.Subtract(new TimeSpan(0, 0, iopair.EndTime.Second));

                        if ((iopair.StartTime.TimeOfDay <= lastIntervalPreviousDay.StartTime.TimeOfDay
                            && iopair.EndTime.TimeOfDay > lastIntervalPreviousDay.StartTime.TimeOfDay)
                            ||
                            (iopair.StartTime.TimeOfDay > lastIntervalPreviousDay.StartTime.TimeOfDay
                            && iopair.StartTime.TimeOfDay < lastIntervalPreviousDay.EndTime.TimeOfDay))
                            employeeIntervalPairs.Add(iopair);
                    }
                }
            }

            foreach (IOPairTO iopair in dayIOPairList)
            {
                if (iopair.IOPairDate == day)
                {
                    //iopair.StartTime = iopair.StartTime.Subtract(new TimeSpan(0, 0, iopair.StartTime.Second));
                    //iopair.EndTime = iopair.EndTime.Subtract(new TimeSpan(0, 0, iopair.EndTime.Second));

                    if ((iopair.StartTime.TimeOfDay <= tsInterval.StartTime.TimeOfDay
                        && iopair.EndTime.TimeOfDay > tsInterval.StartTime.TimeOfDay)
                        ||
                        (iopair.StartTime.TimeOfDay > tsInterval.StartTime.TimeOfDay
                        && iopair.StartTime.TimeOfDay < tsInterval.EndTime.TimeOfDay)
                        )
                        employeeIntervalPairs.Add(iopair);
                }
            }

            return employeeIntervalPairs;
        }

        //get all iopairs for specific employee, for specific day
        //dayIOPairList contains all iopairs for selected employee, for selected day, including night shift pairs
        public bool existEmployeeDayOpenPairs(List<IOPairTO> dayIOPairList)
        {
            bool openPairExist = false;
            foreach (IOPairTO iopair in dayIOPairList)
            {
                if ((iopair.StartTime == new DateTime(0)) ||
                    (iopair.EndTime == new DateTime(0)))
                {
                    openPairExist = true;
                    break;
                }
            }

            return openPairExist;
        }

        //get last interval of previous day (night shift interval)
        //timeScheduleList contains Time Schemas for selected Employee and selected Time Interval
        //date is specific day
        //timeSchemas - all time shemas
        public WorkTimeIntervalTO getLastIntervalPrevDay(List<EmployeeTimeScheduleTO> timeScheduleList, DateTime date, List<WorkTimeSchemaTO> timeSchemas)
        {
            WorkTimeIntervalTO lastIntervalPreviousDay = new WorkTimeIntervalTO();

            // find actual time schedule for the day
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = timeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            WorkTimeSchemaTO actualTimeSchema = null;
            foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
            {
                if (currentTimeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
                {
                    actualTimeSchema = currentTimeSchema;
                    break;
                }
            }
            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //int dayNum = (employeeTimeSchedule.StartCycleDay + date.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(date.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            int previousDay = dayNum - 1;
            if (previousDay < 0)
                previousDay = actualTimeSchema.CycleDuration - 1;
            for (int i = 0; i < actualTimeSchema.Days[previousDay].Count; i++)
            {
                WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[previousDay][i]; //key is interval_num which is 0, 1...
                if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                {
                    lastIntervalPreviousDay = currentTimeSchemaInterval;
                    break;
                }
            }

            return lastIntervalPreviousDay;
        }

        //get last interval of previous day (night shift interval)
        //timeScheduleList contains Time Schemas for selected Employee and selected Time Interval
        //date is specific day
        //timeSchemas - all time shemas
        public WorkTimeIntervalTO getLastIntervalPrevDay(List<EmployeeTimeScheduleTO> timeScheduleList, DateTime date, Dictionary<int, WorkTimeSchemaTO> timeSchemas)
        {
            WorkTimeIntervalTO lastIntervalPreviousDay = new WorkTimeIntervalTO();

            // find actual time schedule for the day
            WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
            int timeScheduleIndex = -1;
            for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
            {
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                if (date >= timeScheduleList[scheduleIndex].Date)
                //&& (date.Month == ((EmployeesTimeSchedule) (timeScheduleList[scheduleIndex])).Date.Month))
                {
                    timeScheduleIndex = scheduleIndex;
                }
            }
            if (timeScheduleIndex == -1) return null;

            EmployeeTimeScheduleTO employeeTimeSchedule = timeScheduleList[timeScheduleIndex];

            // find actual time schema for the day
            WorkTimeSchemaTO actualTimeSchema = null;
            if (timeSchemas.ContainsKey(employeeTimeSchedule.TimeSchemaID))
                actualTimeSchema = timeSchemas[employeeTimeSchedule.TimeSchemaID];

            if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
            //int dayNum = (employeeTimeSchedule.StartCycleDay + date.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(date.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            int previousDay = dayNum - 1;
            if (previousDay < 0)
                previousDay = actualTimeSchema.CycleDuration - 1;
            for (int i = 0; i < actualTimeSchema.Days[previousDay].Count; i++)
            {
                WorkTimeIntervalTO currentTimeSchemaInterval = actualTimeSchema.Days[previousDay][i]; //key is interval_num which is 0, 1...
                if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                    && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                {
                    lastIntervalPreviousDay = currentTimeSchemaInterval;
                    break;
                }
            }

            return lastIntervalPreviousDay;
        }

        public ArrayList getEmployeesByDateAutoClose()
        {
            try
            {
                return ioPairDAO.getEmployeesByDateAutoClose();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.getEmployeesByDateAutoClose(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void recalculatePause(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans)
        {
            try
            {
                //all dates in selected interval
                Hashtable datesList = new Hashtable();
                //Key is employeeID, value is Hashtable with dates for that employee
                Hashtable employees = new Hashtable();

                for (DateTime day = fromDate; day <= toDate; day = day.AddDays(1))
                {
                    if (!datesList.ContainsKey(day))
                    {
                        datesList.Add(day, "");
                    }
                }

                string[] employeeIDS = employeeIDString.Split(',');
                for (int j = 0; j < employeeIDS.Length; j++)
                {
                    int emplID = Int32.Parse(employeeIDS[j]);

                    if (!employees.ContainsKey(emplID))
                    {
                        employees.Add(emplID, datesList);
                    }
                }
                //get all time Schemas
                Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new TimeSchema().getDictionary(trans);

                bool doRecalculation = false;
                StringBuilder deletePairs = new StringBuilder();
                string pairsToDelete = "";
                List<IOPairTO> ioPairList = SearchEmplTimeInterval(employeeIDString, fromDate.AddDays(-1), toDate, trans);
                if (ioPairList.Count > 0)
                {
                    //list of time schedules for all employees, for selected interval
                    List<EmployeeTimeScheduleTO> timeSchedules = new List<EmployeeTimeScheduleTO>();

                    // all employee time schedules for selected Time Interval, key is employee ID
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();


                    List<int> employeesID = new List<int>();

                    // Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
                    Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>();

                    // io pairs for particular employee are sorted by io_pair_date ascending
                    for (int i = 0; i < ioPairList.Count; i++)
                    {
                        int currentEmployeeID = ioPairList[i].EmployeeID;
                        if (!emplPairs.ContainsKey(currentEmployeeID))
                        {
                            employeesID.Add(currentEmployeeID);

                            emplPairs.Add(currentEmployeeID, new List<IOPairTO>());
                            emplTimeSchedules.Add(currentEmployeeID, new List<EmployeeTimeScheduleTO>());
                        }

                        emplPairs[currentEmployeeID].Add(ioPairList[i]);
                    }

                    //get time schemas for selected Employees, for selected Time Interval
                    timeSchedules = new EmployeesTimeSchedule().SearchEmployeesSchedules(employeeIDString, fromDate.AddDays(-1), toDate, trans);
                    for (int i = 0; i < timeSchedules.Count; i++)
                    {
                        if (emplTimeSchedules.ContainsKey(timeSchedules[i].EmployeeID))
                            emplTimeSchedules[timeSchedules[i].EmployeeID].Add(timeSchedules[i]);
                    }

                    //Start calculation
                    //for each employee, day, interval in that day
                    foreach (int employeeID in employeesID)
                    {
                        if (!emplPairs.ContainsKey(employeeID) && (emplPairs.ContainsKey(employeeID) && emplPairs[employeeID].Count <= 0))
                            continue;

                        if (!employees.ContainsKey(employeeID))
                            continue;

                        Hashtable employeeDates = (Hashtable)employees[employeeID];

                        for (DateTime day = fromDate; day <= toDate; day = day.AddDays(1))
                        {
                            //calculate pause only for employee's dates
                            if (!employeeDates.ContainsKey(day))
                                continue;

                            bool is2DaysShift = false;
                            bool is2DaysShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref is2DaysShift,
                                ref is2DaysShiftPrevious, ref firstIntervalNextDay, dictTimeSchema);

                            if (dayIntervals == null)
                                continue;

                            //if previous day is night shift day, take that night shift interval
                            WorkTimeIntervalTO lastIntervalPreviousDay = new WorkTimeIntervalTO();
                            if (is2DaysShiftPrevious)
                                lastIntervalPreviousDay = getLastIntervalPrevDay(emplTimeSchedules[employeeID], day, dictTimeSchema);

                            //if this day is night shift day, find index of night shift interval
                            int lastIntervalIndex = -1;
                            if (is2DaysShift)
                            {
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                    {
                                        lastIntervalIndex = i;
                                        break;
                                    }
                                }
                            }

                            //Take only IO Pairs for specific day, considering night shifts
                            List<IOPairTO> dayIOPairList = getEmployeeExactDayPairs(emplPairs[employeeID], day, is2DaysShiftPrevious, lastIntervalPreviousDay);

                            if (dayIOPairList.Count <= 0)
                                continue;

                            //do not calculate pause if day has at least one open interval
                            bool openPairExist = existEmployeeDayOpenPairs(dayIOPairList);
                            if (openPairExist)
                                continue;

                            foreach (IOPairTO ioPairTO in dayIOPairList)
                            {
                                if ((day.Date == toDate.Date) && is2DaysShift)
                                {
                                    WorkTimeIntervalTO tsInterval = dayIntervals[lastIntervalIndex]; //key is interval_num which is 0, 1...
                                    if ((ioPairTO.IOPairDate == day.Date) && (ioPairTO.StartTime.TimeOfDay >= tsInterval.StartTime.TimeOfDay))
                                        continue;
                                }

                                if ((ioPairTO.PassTypeID != Constants.automaticPausePassType)
                                    && (ioPairTO.PassTypeID != Constants.automaticShortBreakPassType))
                                    continue;

                                if (
                                        (day.Date != fromDate.Date)
                                        ||
                                        (
                                            (day.Date == fromDate.Date)
                                            &&
                                            (
                                                (!is2DaysShiftPrevious)
                                                || (
                                                    is2DaysShiftPrevious && (lastIntervalPreviousDay != null) &&
                                                    ((ioPairTO.IOPairDate == day.Date) ||
                                                    (ioPairTO.StartTime.TimeOfDay >= lastIntervalPreviousDay.StartTime.TimeOfDay))
                                                   )
                                            )
                                        )
                                    )
                                    deletePairs.Append(ioPairTO.IOPairID.ToString().Trim() + ",");
                            }
                        } //for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                    } //foreach(int employeeID in employeesID)
                } //if (ioPairList.Count > 0)

                pairsToDelete = deletePairs.ToString();
                if (!pairsToDelete.Equals(""))
                {
                    pairsToDelete = pairsToDelete.Substring(0, pairsToDelete.Length - 1);

                    if (trans != null)
                    {
                        ioPairDAO.setTransaction(trans);
                    }

                    if (trans != null)
                    {
                        doRecalculation = Delete(pairsToDelete, false);
                    }
                    else
                    {
                        doRecalculation = Delete(pairsToDelete);
                    }

                    if (!doRecalculation)
                        log.writeLog(DateTime.Now + " IOPair.recalculatePause(): Can't delete existing pauses. IO Pair ID's are: " + pairsToDelete + "\n");
                }
                else if (ioPairList.Count > 0)
                {
                    doRecalculation = true;
                }

                if (doRecalculation)
                {
                    //get all ioPairs (including open) for all employees in employeeIDString for given time interval
                    List<IOPairTO> ioPairsList = SearchEmplTimeInterval(employeeIDString, fromDate.AddDays(-1), toDate, trans);

                    //calculate pause for this ioPairs
                    calculatePause(ioPairsList, employees, dictTimeSchema, trans);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.recalculatePause(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public void recalculatePause(string employeeIDString, DateTime fromDate, DateTime toDate, IDbTransaction trans, object dbConnection)
        {
            try
            {
                //all dates in selected interval
                Hashtable datesList = new Hashtable();
                //Key is employeeID, value is Hashtable with dates for that employee
                Hashtable employees = new Hashtable();

                for (DateTime day = fromDate; day <= toDate; day = day.AddDays(1))
                {
                    if (!datesList.ContainsKey(day))
                    {
                        datesList.Add(day, "");
                    }
                }

                string[] employeeIDS = employeeIDString.Split(',');
                for (int j = 0; j < employeeIDS.Length; j++)
                {
                    int emplID = Int32.Parse(employeeIDS[j]);

                    if (!employees.ContainsKey(emplID))
                    {
                        employees.Add(emplID, datesList);
                    }
                }
                //get all time Schemas
                Dictionary<int, WorkTimeSchemaTO> dictTimeSchema = new TimeSchema(dbConnection).getDictionary(trans);

                bool doRecalculation = false;
                StringBuilder deletePairs = new StringBuilder();
                string pairsToDelete = "";
                List<IOPairTO> ioPairList = SearchEmplTimeInterval(employeeIDString, fromDate.AddDays(-1), toDate, trans);
                if (ioPairList.Count > 0)
                {
                    //list of time schedules for all employees, for selected interval
                    List<EmployeeTimeScheduleTO> timeSchedules = new List<EmployeeTimeScheduleTO>();

                    // all employee time schedules for selected Time Interval, key is employee ID
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int, List<EmployeeTimeScheduleTO>>();


                    List<int> employeesID = new List<int>();

                    // Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
                    Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int, List<IOPairTO>>();

                    // io pairs for particular employee are sorted by io_pair_date ascending
                    for (int i = 0; i < ioPairList.Count; i++)
                    {
                        int currentEmployeeID = ioPairList[i].EmployeeID;
                        if (!emplPairs.ContainsKey(currentEmployeeID))
                        {
                            employeesID.Add(currentEmployeeID);

                            emplPairs.Add(currentEmployeeID, new List<IOPairTO>());
                            emplTimeSchedules.Add(currentEmployeeID, new List<EmployeeTimeScheduleTO>());
                        }

                        emplPairs[currentEmployeeID].Add(ioPairList[i]);
                    }

                    //get time schemas for selected Employees, for selected Time Interval
                    timeSchedules = new EmployeesTimeSchedule(dbConnection).SearchEmployeesSchedules(employeeIDString, fromDate.AddDays(-1), toDate, trans);
                    for (int i = 0; i < timeSchedules.Count; i++)
                    {
                        if (emplTimeSchedules.ContainsKey(timeSchedules[i].EmployeeID))
                            emplTimeSchedules[timeSchedules[i].EmployeeID].Add(timeSchedules[i]);
                    }

                    //Start calculation
                    //for each employee, day, interval in that day
                    foreach (int employeeID in employeesID)
                    {
                        if (!emplPairs.ContainsKey(employeeID) && (emplPairs.ContainsKey(employeeID) && emplPairs[employeeID].Count <= 0))
                            continue;

                        if (!employees.ContainsKey(employeeID))
                            continue;

                        Hashtable employeeDates = (Hashtable)employees[employeeID];

                        for (DateTime day = fromDate; day <= toDate; day = day.AddDays(1))
                        {
                            //calculate pause only for employee's dates
                            if (!employeeDates.ContainsKey(day))
                                continue;

                            bool is2DaysShift = false;
                            bool is2DaysShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref is2DaysShift,
                                ref is2DaysShiftPrevious, ref firstIntervalNextDay, dictTimeSchema);

                            if (dayIntervals == null)
                                continue;

                            //if previous day is night shift day, take that night shift interval
                            WorkTimeIntervalTO lastIntervalPreviousDay = new WorkTimeIntervalTO();
                            if (is2DaysShiftPrevious)
                                lastIntervalPreviousDay = getLastIntervalPrevDay(emplTimeSchedules[employeeID], day, dictTimeSchema);

                            //if this day is night shift day, find index of night shift interval
                            int lastIntervalIndex = -1;
                            if (is2DaysShift)
                            {
                                for (int i = 0; i < dayIntervals.Count; i++)
                                {
                                    WorkTimeIntervalTO currentTimeSchemaInterval = dayIntervals[i]; //key is interval_num which is 0, 1...
                                    if ((currentTimeSchemaInterval.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                        && (currentTimeSchemaInterval.EndTime > currentTimeSchemaInterval.StartTime))
                                    {
                                        lastIntervalIndex = i;
                                        break;
                                    }
                                }
                            }

                            //Take only IO Pairs for specific day, considering night shifts
                            List<IOPairTO> dayIOPairList = getEmployeeExactDayPairs(emplPairs[employeeID], day, is2DaysShiftPrevious, lastIntervalPreviousDay);

                            if (dayIOPairList.Count <= 0)
                                continue;

                            //do not calculate pause if day has at least one open interval
                            bool openPairExist = existEmployeeDayOpenPairs(dayIOPairList);
                            if (openPairExist)
                                continue;

                            foreach (IOPairTO ioPairTO in dayIOPairList)
                            {
                                if ((day.Date == toDate.Date) && is2DaysShift)
                                {
                                    WorkTimeIntervalTO tsInterval = dayIntervals[lastIntervalIndex]; //key is interval_num which is 0, 1...
                                    if ((ioPairTO.IOPairDate == day.Date) && (ioPairTO.StartTime.TimeOfDay >= tsInterval.StartTime.TimeOfDay))
                                        continue;
                                }

                                if ((ioPairTO.PassTypeID != Constants.automaticPausePassType)
                                    && (ioPairTO.PassTypeID != Constants.automaticShortBreakPassType))
                                    continue;

                                if (
                                        (day.Date != fromDate.Date)
                                        ||
                                        (
                                            (day.Date == fromDate.Date)
                                            &&
                                            (
                                                (!is2DaysShiftPrevious)
                                                || (
                                                    is2DaysShiftPrevious && (lastIntervalPreviousDay != null) &&
                                                    ((ioPairTO.IOPairDate == day.Date) ||
                                                    (ioPairTO.StartTime.TimeOfDay >= lastIntervalPreviousDay.StartTime.TimeOfDay))
                                                   )
                                            )
                                        )
                                    )
                                    deletePairs.Append(ioPairTO.IOPairID.ToString().Trim() + ",");
                            }
                        } //for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                    } //foreach(int employeeID in employeesID)
                } //if (ioPairList.Count > 0)

                pairsToDelete = deletePairs.ToString();
                if (!pairsToDelete.Equals(""))
                {
                    pairsToDelete = pairsToDelete.Substring(0, pairsToDelete.Length - 1);

                    if (trans != null)
                    {
                        ioPairDAO.setTransaction(trans);
                    }

                    if (trans != null)
                    {
                        doRecalculation = Delete(pairsToDelete, false);
                    }
                    else
                    {
                        doRecalculation = Delete(pairsToDelete);
                    }

                    if (!doRecalculation)
                        log.writeLog(DateTime.Now + " IOPair.recalculatePause(): Can't delete existing pauses. IO Pair ID's are: " + pairsToDelete + "\n");
                }
                else if (ioPairList.Count > 0)
                {
                    doRecalculation = true;
                }

                if (doRecalculation)
                {
                    //get all ioPairs (including open) for all employees in employeeIDString for given time interval
                    List<IOPairTO> ioPairsList = SearchEmplTimeInterval(employeeIDString, fromDate.AddDays(-1), toDate, trans);

                    //calculate pause for this ioPairs
                    calculatePause(ioPairsList, employees, dictTimeSchema, trans, dbConnection);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.recalculatePause(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<IOPairTO> SearchForEmplPerm(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplPerm(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchForEmplPermCount(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsForEmplPermCount(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchForDaysArrayCount(List<DateTime> days, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsForDaysArrayCount(days, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForDaysArrayCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchForDaysArray(List<DateTime> days, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsForDaysArray(days, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchForEmployees(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchEmployeeDateCount(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsEmployeeDateCount(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchEmplDateCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchEmployeeDate(DateTime fromDate, DateTime toDate, List<int> employeesList)
        {
            try
            {
                return ioPairDAO.getIOPairsEmployeeDate(fromDate, toDate, employeesList);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchAll(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public bool ExistEmlpoyeeDatePair(int employeeID, DateTime ioPairDate, DateTime startTime, DateTime endTime, int skipIOPairID, int isWrkHrs)
        {
            try
            {
                return ioPairDAO.existEmlpoyeeDatePair(employeeID, ioPairDate, startTime, endTime, skipIOPairID, isWrkHrs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.ExistEmlpoyeeDatePair(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int SearchCount(DateTime fromDate, DateTime toDate, string wUnits, int wuID)
        {
            try
            {
                return ioPairDAO.getIOPairsCount(this.PairTO, fromDate, toDate, wUnits, wuID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.getIOPairsCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<IOPairTO> SearchWithType(DateTime fromDate, DateTime toDate, string wUnits, int wuID)
        {
            try
            {
                return ioPairDAO.getIOPairsWithType(this.PairTO, fromDate, toDate, wUnits, wuID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.getIOPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public List<IOPairTO> SearchNonEnteredIOPairs(int employeeID, DateTime startTime, DateTime endTime)
        {
            try
            {
                return ioPairDAO.getNonEnteredIOPairs(employeeID, startTime, endTime);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.SearchNonEnteredIOPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public DateTime getFirstArrivedTime(int emplID, DateTime date)
        {
            try
            {
                return ioPairDAO.getFirstArrivedTime(emplID, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.getFirstArrivedTime(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //  22.05.2019. BOJAN
        public List<IOPairTO> GetOpenPairsForReport(string employeeID, DateTime from, DateTime to, string orgUnitID, string workingUnitID) {
            try {
                return ioPairDAO.getOpenPairsForReport(employeeID, from, to, orgUnitID, workingUnitID);
            }
            catch (Exception ex) {
                log.writeLog(DateTime.Now + " IOPair.GetOpenPairs(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }


        public bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore)
        {
            try
            {
                return ioPairDAO.updateToUnprocessed(emplDateList, emplDateListDayAfter, emplDateListDayBefore);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updateToUnprocessed(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDict, bool doCommit)
        {
            try
            {
                return ioPairDAO.updateToUnprocessed(emplDict, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updateToUnprocessed(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public bool updateToUnprocessed(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore, bool p)
        {
            try
            {
                return ioPairDAO.updateToUnprocessed(emplDateList, emplDateListDayAfter, emplDateListDayBefore, false);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updateToUnprocessed(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }



        public bool updateToUnprocessedWorkCount(Dictionary<int, List<DateTime>> emplDateList, Dictionary<int, List<DateTime>> emplDateListDayAfter, Dictionary<int, List<DateTime>> emplDateListDayBefore)
        {
            try
            {
                return ioPairDAO.updateToUnprocessedWorkCount(emplDateList, emplDateListDayAfter, emplDateListDayBefore);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updateToUnprocessedWorkCount(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        //  23.01.2020. BOJAN
        public List<IOPairTO> getIOPairsForBreaks(DateTime from, DateTime to, string emplIDs, string ptIDs)
        {
            try
            {
                return ioPairDAO.getIOPairsForBreaks(from, to, emplIDs, ptIDs);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.getIOPairsForBreaks(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public void deleteForDay(int emplID, DateTime date)
        {
            try
            {
                ioPairDAO.deleteForDay(emplID, date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.deleteForDay(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        public void deleteForAutoTimeSchema(int emplID, DateTime date, bool doCommit)
        {
            try
            {
                ioPairDAO.deleteForAutoTimeSchema(emplID, date, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " IOPair.updateToUnprocessed(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
