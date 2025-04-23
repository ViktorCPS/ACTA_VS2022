using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Util;
using TransferObjects;
using System.Reflection;
using System.Collections;
using System.IO;
using ReaderInterface;

namespace ACTAReaderRemoteControlProcessing
{
    public class ReaderRemoteControlProcessor
    {
        //every thread has it's own connection
        Object dbConnection = null;
        //debuging object
        private DebugLog log = null;


        //validation terminal controlling
        public ReaderTO validationTerminal = new ReaderTO();

        public OnlineMealsPointsTO validationTerminalOnline = new OnlineMealsPointsTO();

        Dictionary<uint, Dictionary<int, int>> dataDictionaryFiat = new Dictionary<uint, Dictionary<int, int>>();
        Dictionary<uint, Dictionary<int, Dictionary<int, List<int>>>> dataDictionary = new Dictionary<uint, Dictionary<int, Dictionary<int, List<int>>>>();

        //cards for upload
        ArrayList cardsList = new ArrayList();

        public ArrayList CardsList
        {
            get { return cardsList; }
            set { cardsList = value; }
        }

        //tags and employees - ACTIVE
        Dictionary<uint, EmployeeTO> tagsEmployees = new Dictionary<uint, EmployeeTO>();

        Dictionary<int, EmployeeAsco4TO> employeesAsco4 = new Dictionary<int, EmployeeAsco4TO>();

        public Dictionary<int, EmployeeAsco4TO> EmployeesAsco4
        {
            get { return employeesAsco4; }
            set { employeesAsco4 = value; }
        }

        public Dictionary<uint, EmployeeTO> TagsEmployees
        {
            get { return tagsEmployees; }
            set { tagsEmployees = value; }
        }

        //tags active
        Dictionary<ulong, TagTO> tagsActive = new Dictionary<ulong, TagTO>();

        Dictionary<ulong, TagTO> tagsAll = new Dictionary<ulong, TagTO>();

        //day iopairs
        Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsForToday = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

        //intervals
        Dictionary<int, List<WorkTimeIntervalTO>> timeSchemaInterval = new Dictionary<int, List<WorkTimeIntervalTO>>();

        //timeSchema
        Dictionary<int, WorkTimeSchemaTO> schemaEmployees = new Dictionary<int, WorkTimeSchemaTO>();

        //passTypes for companies
        Dictionary<int, Dictionary<int, PassTypeTO>> passTypesForCompany = new Dictionary<int, Dictionary<int, PassTypeTO>>();

        //employees
        Dictionary<int, EmployeeTO> employeesDictionary = new Dictionary<int, EmployeeTO>();

        //wunits
        Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int, WorkingUnitTO>();

        //company restaurant rules
        Dictionary<int, Dictionary<int, int>> dictionaryRules = new Dictionary<int, Dictionary<int, int>>();

        // rule restaurant, employees
        Dictionary<int, int> restaurantAvailable = new Dictionary<int, int>();


        //points
        Dictionary<int, OnlineMealsPointsTO> dictionaryPoints = new Dictionary<int, OnlineMealsPointsTO>();

        public Dictionary<int, OnlineMealsPointsTO> DictionaryPoints
        {
            get { return dictionaryPoints; }
            set { dictionaryPoints = value; }
        }



        int lastButtonPressed = 0;

        private static string SUFFIXFORMAT = "yyyyMMddHHmmss";
        private static string FILEEXT = ".xml";

        string logDirectory = "";

        private IOPairProcessedTO pairTO = new IOPairProcessedTO();
        private Dictionary<int, List<WorkTimeIntervalTO>> dayIntervals = new Dictionary<int, List<WorkTimeIntervalTO>>();
        private Dictionary<int, List<WorkTimeIntervalTO>> prevDayIntervals = new Dictionary<int, List<WorkTimeIntervalTO>>();
        private Dictionary<int, List<WorkTimeIntervalTO>> nextDayIntervals = new Dictionary<int, List<WorkTimeIntervalTO>>();
        private Dictionary<int, PassTypeTO> passTypesAllDic = new Dictionary<int, PassTypeTO>();
        private int prevPairPT = -1;
        private int nextPairPT = -1;
        private DateTime dayBegining = new DateTime();
        private DateTime dayEnd = new DateTime();

        public IOPairProcessedTO PairTO
        {
            get { return pairTO; }
            set { pairTO = value; }
        }

        public Dictionary<int, List<WorkTimeIntervalTO>> DayIntervals
        {
            get { return dayIntervals; }
            set { dayIntervals = value; }
        }

        public Dictionary<int, List<WorkTimeIntervalTO>> PrevDayIntervals
        {
            get { return prevDayIntervals; }
            set { prevDayIntervals = value; }
        }

        public Dictionary<int, List<WorkTimeIntervalTO>> NextDayIntervals
        {
            get { return nextDayIntervals; }
            set { nextDayIntervals = value; }
        }
        public Dictionary<int, PassTypeTO> PassTypesAllDic
        {
            get { return passTypesAllDic; }
            set { passTypesAllDic = value; }
        }

        public ReaderRemoteControlProcessor()
        {
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");
        }

        Dictionary<int, List<string>> listLocations = new Dictionary<int, List<string>>();

        public ReaderRemoteControlProcessor(int readerID, Dictionary<int, string> locations)
        {
            dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
            //Reader reader = new Reader(dbConnection);
            //validationTerminal = reader.find(readerID);

            OnlineMealsPoints mealPoint = new OnlineMealsPoints(dbConnection);
            OnlineMealsPointsTO onlinePointTO = new OnlineMealsPointsTO();
            onlinePointTO.PointID = readerID;
            mealPoint.OnlineMealsPointTO = onlinePointTO;

            validationTerminalOnline = mealPoint.find(readerID);
            //validationTerminalOnline.Locations = locations[validationTerminalOnline.Reader_ant];

            foreach (int ant in locations.Keys)
            {

                List<string> listStringLoc = new List<string>();
                foreach (string location in locations[ant].Split(','))
                {
                    if (!listStringLoc.Contains(location))
                        listStringLoc.Add(location);

                }
                if (!listLocations.ContainsKey(ant))
                    listLocations.Add(ant, listStringLoc);

            }
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log" + validationTerminal.ConnectionAddress + ".txt");
            logDirectory = getLogFilePath();


            dictionaryPoints = new Dictionary<int, OnlineMealsPointsTO>();

            OnlineMealsPoints onlinePoints = new OnlineMealsPoints(dbConnection);
            List<OnlineMealsPointsTO> listPoints = onlinePoints.Search(validationTerminalOnline.ReaderIPAddress, -1);
            if (listPoints.Count > 0)
            {
                dictionaryPoints.Add(listPoints[0].Reader_ant, listPoints[0]);
                if (listPoints.Count > 1)
                    dictionaryPoints.Add(listPoints[1].Reader_ant, listPoints[1]);
            }



            DBConnectionManager.Instance.CloseDBConnection(dbConnection);


        }

        public string GetTerminals(ref string str)
        {
            string terminals = "";
            try
            {
                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                Reader reader = new Reader(dbConnection);
                reader.RdrTO.Status = Constants.readerStatusEnabled;
                reader.RdrTO.ConnectionType = Constants.ConnTypeIP;
                List<ReaderTO> readerList = reader.Search();
                foreach (ReaderTO readerTO in readerList)
                {
                    terminals += readerTO.ReaderID.ToString() + "," + readerTO.ConnectionAddress + "|";
                }
                if (terminals.Length > 0)
                    terminals = terminals.Substring(0, terminals.Length - 1);

                DBConnectionManager.Instance.CloseDBConnection(dbConnection);
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
            return terminals;
        }
        private Dictionary<int, PassTypeTO> getPassTypeList(int company)
        {
            try
            {
                Dictionary<int, PassTypeTO> ptList = new Dictionary<int, PassTypeTO>();

                if (company != -1)
                {

                    ptList = new PassType(dbConnection).SearchForCompanyDictionary(company, true);
                }

                return ptList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int check_passFiat(byte messageType, int validationTerminalID, int passDetected, int antenna, uint tagID, int buttonNumber, ref int response, ref byte buttonsToShow, ref string screenMessage, DateTime passTime)
        {
            int i = 0;
            try
            {
                buttonsToShow = 0;
                response = 0;
                screenMessage = "";

                if (validationTerminal.ReaderID < 0)
                    return i;

                switch (messageType)
                {
                    case Constants.MESSAGE_TAG_DETECTED:
                        i = CheckRequestFiat(tagID);
                        if (i == 0)
                        {
                            if ((validationTerminal.A0Direction == Constants.DirectionIn && antenna == 0)
                                || (validationTerminal.A1Direction == Constants.DirectionIn && antenna == 1))
                            {
                                response = Constants.app_response_pass_permitted;
                            }
                            else
                            {
                                buttonsToShow = Constants.buttonsScreen;
                                response = Constants.app_response_show_buttons;
                            }
                        }
                        else
                        {
                            response = Constants.app_response_pass_unpermitted;
                        }
                        lastButtonPressed = 0;
                        break;
                    case Constants.MESSAGE_BUTTON_PRESSED:

                        if (buttonNumber != Constants.buttonCancel)
                        {
                            if (buttonNumber != Constants.buttonRegular)
                            {
                                lastButtonPressed = buttonNumber;
                            }
                            else
                            {
                                lastButtonPressed = 0;
                            }
                            response = Constants.app_response_pass_permitted;
                        }
                        else
                        {
                            response = Constants.app_response_pass_unpermitted;
                        }
                        break;
                    case Constants.MESSAGE_PASS_STATUS:
                        //if (passDetected == Constants.passIsDetected)
                        //{
                        savePass(tagID, antenna, lastButtonPressed, passTime);
                        //}
                        break;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
            return i;
        }


        private void savePass(uint tagID, int antennaNum, int lastButtonPressed, DateTime passTime)
        {
            try
            {
                LogTO logTOData = new LogTO();
                List<LogTO> logTOList = new List<LogTO>();

                // Create LogTO List
                logTOData = new LogTO();

                logTOData.ReaderID = this.validationTerminal.ReaderID;
                logTOData.TagID = tagID;
                logTOData.Antenna = antennaNum;
                logTOData.EventHappened = (int)Constants.EventTag.eventTagAllowed;
                logTOData.ActionCommited = (int)Constants.actionCommitedAllowed;
                logTOData.EventTime = passTime;
                logTOData.PassGenUsed = 0;

                logTOList.Add(logTOData);

                if (lastButtonPressed > 0)
                {
                    // Create LogTO List
                    logTOData = new LogTO();

                    logTOData.ReaderID = this.validationTerminal.ReaderID;
                    logTOData.TagID = 0;
                    logTOData.Antenna = antennaNum;
                    logTOData.EventHappened = (int)Constants.EventTag.eventButtonPressed;
                    logTOData.ActionCommited = lastButtonPressed;
                    logTOData.EventTime = passTime;
                    logTOData.PassGenUsed = 0;

                    logTOList.Add(logTOData);
                }

                // Serialize LogTOList to XML file
                Log currentLog = new Log();

                // Send diff log XML file to unprocessed
                string path = this.logDirectory
                    + Constants.ReaderXMLLogFile
                    //+ ConfigurationManager.AppSettings["ReaderXMLLogFile"] 
                    + "_" + this.validationTerminal.ReaderID.ToString().Trim()
                    + "_" + DateTime.Now.ToString(SUFFIXFORMAT)
                    + FILEEXT;

                bool succeed = false;

                if (logTOList.Count > 0)
                {
                    succeed = currentLog.SaveToFile(logTOList, path);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(ex);
            }
        }

        public string getLogFilePath()
        {
            string filePath = "";
            string remotePath = Constants.unprocessed;
            string localPath = Constants.LocalReaderLogDir;

            try
            {
                Stream writer = File.Open(remotePath + "Test", FileMode.Create);
                filePath = remotePath;
                writer.Close();
                File.Delete(remotePath + "Test");
            }
            catch (IOException)
            {
                filePath = localPath;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Exception in: " + this.ToString() +
                    ".getLogFilePath() : " + ex.Message);
                throw ex;
            }

            return filePath;
        }

        //private bool CheckRequest(uint tagID, int antennaNum)
        //{
        //    bool valid = false;

        //    try
        //    {
        //        int day = -1;

        //        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        //            day = Constants.SundayDay;
        //        else
        //            day = (int)DateTime.Now.DayOfWeek;


        //        if (dataDictionary.ContainsKey(tagID) && dataDictionary[tagID].ContainsKey(antennaNum)
        //            && dataDictionary[tagID][antennaNum].ContainsKey(day)
        //            && dataDictionary[tagID][antennaNum][day].Contains(DateTime.Now.Hour))
        //        {
        //            valid = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog("Tag.CheckMethod() throw exception: " + ex.Message, Constants.basicDebugLevel);
        //    }

        //    return valid;
        //}

        /** PRIMARY USED FOR FIAT VALIDATION, NOW USED CheckRequest **/
        private int CheckRequestFiat(uint tagID)
        {

            int result = -1;
            int resPom = -1;
            try
            {
                if (tagsEmployees.ContainsKey(tagID))
                {
                    EmployeeTO Empl = new EmployeeTO();
                    if (employeesDictionary.ContainsKey(tagsEmployees[tagID].EmployeeID))
                        Empl = employeesDictionary[tagsEmployees[tagID].EmployeeID];
                    if (restaurantAvailable.ContainsKey(tagsEmployees[tagID].EmployeeID) && restaurantAvailable[tagsEmployees[tagID].EmployeeID] == 1)
                    {
                        if (timeSchemaInterval.ContainsKey(tagsEmployees[tagID].EmployeeID))
                        {
                            List<WorkTimeIntervalTO> listIntervals = timeSchemaInterval[tagsEmployees[tagID].EmployeeID];
                            foreach (WorkTimeIntervalTO interval in listIntervals)
                            {
                                if (interval.StartTime.TimeOfDay <= DateTime.Now.TimeOfDay && interval.EndTime.TimeOfDay >= DateTime.Now.TimeOfDay)
                                {
                                    resPom = 1;
                                    if (pairsForToday.ContainsKey(tagsEmployees[tagID].EmployeeID))
                                    {
                                        List<IOPairProcessedTO> listIOPairs = pairsForToday[tagsEmployees[tagID].EmployeeID][DateTime.Now.Date];

                                        foreach (IOPairProcessedTO ioPair in listIOPairs)
                                        {
                                            if (ioPair.StartTime <= DateTime.Now && ioPair.EndTime >= DateTime.Now)
                                            {
                                                int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, wUnits);
                                                if (passTypesForCompany.ContainsKey(company) && passTypesForCompany[company].ContainsKey(ioPair.PassTypeID))
                                                {
                                                    if (passTypesForCompany[company][ioPair.PassTypeID].IsPass != Constants.wholeDayAbsence)
                                                    {

                                                        result = (int)Constants.MealDenied.Approved;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (resPom == -1)
                                result = (int)Constants.MealDenied.NotWorkingSchedule;
                            else
                            {
                                if (result == -1)
                                    result = (int)Constants.MealDenied.WholeDayAbsence;
                            }
                        }
                        else
                        {
                            result = (int)Constants.MealDenied.NotWorkingSchedule;
                        }
                    }
                    else
                    {
                        result = (int)Constants.MealDenied.CompanyRestaurant;
                    }
                }
                else
                {
                    if (tagsActive.ContainsKey(tagID))
                        result = (int)Constants.MealDenied.EmployeeNotActive;
                    else
                        result = (int)Constants.MealDenied.TagNotActive;

                }
            }
            catch (Exception ex)
            {
                WriteLog("Tag.CheckMethod() throw exception: " + ex.Message, Constants.basicDebugLevel);
            }

            return result;
        }


        /** CURRENTLY USED FOR FIAT RESTAURANT VALIDATION **/
        public bool CheckRequest(uint tagID, int antNum, ref int result)
        {
            try
            {
                if (tagsEmployees.ContainsKey(tagID))
                {
                    EmployeeTO Empl = new EmployeeTO();
                    if (employeesDictionary.ContainsKey(tagsEmployees[tagID].EmployeeID))
                        Empl = employeesDictionary[tagsEmployees[tagID].EmployeeID];
                    if (restaurantAvailable.ContainsKey(tagsEmployees[tagID].EmployeeID) && restaurantAvailable[tagsEmployees[tagID].EmployeeID] == 1)
                    {
                        if (Empl.Status == Constants.statusActive)
                        {
                            if (employeesAsco4.ContainsKey(Empl.EmployeeID) && employeesAsco4[Empl.EmployeeID].IntegerValue5 != null && listLocations.ContainsKey(antNum) && (listLocations[antNum].Contains(employeesAsco4[Empl.EmployeeID].IntegerValue5.ToString().Trim())))
                            {
                                result = (int)Constants.MealDenied.Approved;
                            }
                            else
                            {
                                result = (int)Constants.MealDenied.Location;
                            }
                        }
                        else
                            result = (int)Constants.MealDenied.EmployeeNotActive;

                    }
                    else
                    {
                        result = (int)Constants.MealDenied.CompanyRestaurant;
                    }
                }
                else
                {
                    if (tagsAll.ContainsKey(tagID))
                        result = (int)Constants.MealDenied.TagNotActive;
                    else
                        result = (int)Constants.MealDenied.TagUnknown;
                }
            }
            catch (Exception ex)
            {
                WriteLog("Tag.CheckRequestFiatOnline() throw exception: " + ex.Message, Constants.basicDebugLevel);
            }

            if (result == (int)Constants.MealDenied.Approved)
                return true;
            else
                return false;
        }

        //public bool getValidationData()
        //{
        //    bool succ = false;
        //    try
        //    {
        //        WriteLog("Data reading started", Constants.basicDebugLevel);
        //        dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
        //        //initialize data dictionary
        //        dataDictionary = new Dictionary<uint, Dictionary<int, Dictionary<int, List<int>>>>();

        //        //get all tags from DB
        //        Tag tag = new Tag(dbConnection);
        //        tag.TgTO.Status = Constants.statusActive;
        //        List<TagTO> tagsListActive = tag.Search();

        //        //create string to search employees from tagID's
        //        string tags = "";
        //        Dictionary<int, uint> tagsEmpl = new Dictionary<int, uint>();
        //        foreach (TagTO tagTO in tagsListActive)
        //        {
        //            Dictionary<int, Dictionary<int, List<int>>> emptyDictionary = new Dictionary<int, Dictionary<int, List<int>>>();
        //            emptyDictionary.Add(0, new Dictionary<int, List<int>>());
        //            emptyDictionary.Add(1, new Dictionary<int, List<int>>());
        //            tags += tagTO.TagID + ", ";

        //            if (!tagsEmpl.ContainsKey(tagTO.OwnerID))
        //            {
        //                tagsEmpl.Add(tagTO.OwnerID, tagTO.TagID);
        //            }
        //        }
        //        if (tags.Length > 0)
        //        {
        //            tags = tags.Substring(0, tags.Length - 2);
        //        }

        //        //search employees by tags
        //        List<EmployeeTO> employees = new Employee(dbConnection).SearchByTags(tags);
        //        List<int> accessGroups = new List<int>();

        //        Dictionary<uint, EmployeeTO> tagsEmployees = new Dictionary<uint, EmployeeTO>();
        //        //create string from AccessGroupID's
        //        string accessStr = "";
        //        List<int> distinctAccessGroups = new List<int>();
        //        foreach (EmployeeTO eTO in employees)
        //        {
        //            if (tagsEmpl.ContainsKey(eTO.EmployeeID))
        //            {
        //                if (!tagsEmployees.ContainsKey(tagsEmpl[eTO.EmployeeID]))
        //                {
        //                    tagsEmployees.Add(tagsEmpl[eTO.EmployeeID], eTO);
        //                }
        //            }
        //            if (!distinctAccessGroups.Contains(eTO.AccessGroupID))
        //            {
        //                distinctAccessGroups.Add(eTO.AccessGroupID);
        //                accessStr += eTO.AccessGroupID.ToString() + ", ";
        //            }
        //        }
        //        if (accessStr.Length > 0)
        //            accessStr = accessStr.Substring(0, accessStr.Length - 2);


        //        //create string from gates on both anntenas
        //        string gateID = "";
        //        gateID = validationTerminal.A0GateID.ToString();
        //        if (validationTerminal.A0GateID != validationTerminal.A1GateID)
        //            gateID += ", " + validationTerminal.A1GateID.ToString();

        //        //search accessGroupXGates
        //        ArrayList accessGroup = new AccessGroupXGate(dbConnection).Search(accessStr, gateID);

        //        Dictionary<int, List<AccessGroupXGateTO>> accessGroupGates = new Dictionary<int, List<AccessGroupXGateTO>>();
        //        //create string from timeAccessIDs
        //        string timeAccessIDs = "0, ";
        //        foreach (AccessGroupXGate acg in accessGroup)
        //        {
        //            if (!accessGroupGates.ContainsKey(acg.AccessGroupID))
        //            {
        //                accessGroupGates.Add(acg.AccessGroupID, new List<AccessGroupXGateTO>());
        //            }
        //            accessGroupGates[acg.AccessGroupID].Add(acg.SendTransferObject());
        //            timeAccessIDs += acg.GateTimeAccessProfile.ToString() + ", ";
        //        }
        //        if (timeAccessIDs.Length > 0)
        //            timeAccessIDs = timeAccessIDs.Substring(0, timeAccessIDs.Length - 2);

        //        //search details of time access profile
        //        TimeAccessProfileDtl profile = new TimeAccessProfileDtl();
        //        ArrayList timeAccessProfileDtls = profile.Search(timeAccessIDs);

        //        Dictionary<int, TimeAccessProfileDtlTO> timeAccessDict = new Dictionary<int, TimeAccessProfileDtlTO>();
        //        Dictionary<int, Dictionary<int, Dictionary<string, List<int>>>> timeAccessHoursList = new Dictionary<int, Dictionary<int, Dictionary<string, List<int>>>>();
        //        foreach (TimeAccessProfileDtlTO tapTO in timeAccessProfileDtls)
        //        {
        //            if (!timeAccessDict.ContainsKey(tapTO.TimeAccessProfileId))
        //                timeAccessDict.Add(tapTO.TimeAccessProfileId, tapTO);
        //            foreach (PropertyInfo pi in tapTO.GetType().GetProperties())
        //            {
        //                if (pi.Name.StartsWith("Hrs"))
        //                {
        //                    if (!timeAccessHoursList.ContainsKey(tapTO.TimeAccessProfileId))
        //                        timeAccessHoursList.Add(tapTO.TimeAccessProfileId, new Dictionary<int, Dictionary<string, List<int>>>());
        //                    if (!timeAccessHoursList[tapTO.TimeAccessProfileId].ContainsKey(tapTO.DayOfWeek))
        //                        timeAccessHoursList[tapTO.TimeAccessProfileId].Add(tapTO.DayOfWeek, new Dictionary<string, List<int>>());
        //                    if (!timeAccessHoursList[tapTO.TimeAccessProfileId][tapTO.DayOfWeek].ContainsKey(tapTO.Direction))
        //                        timeAccessHoursList[tapTO.TimeAccessProfileId][tapTO.DayOfWeek].Add(tapTO.Direction, new List<int>());
        //                    if ((int)pi.GetValue(tapTO, null) == 1)
        //                    {
        //                        timeAccessHoursList[tapTO.TimeAccessProfileId][tapTO.DayOfWeek][tapTO.Direction].Add(int.Parse(pi.Name.Substring(3, pi.Name.Length - 3)));
        //                    }
        //                }
        //            }
        //        }

        //        foreach (TagTO t in tagsListActive)
        //        {
        //            bool isDefault0 = true;
        //            bool isDefoult1 = true;
        //            if (!tagsEmployees.ContainsKey(t.TagID))
        //                continue;
        //            EmployeeTO owner = tagsEmployees[t.TagID];

        //            List<AccessGroupXGateTO> accessList = new List<AccessGroupXGateTO>();
        //            if (accessGroupGates.ContainsKey(owner.AccessGroupID))
        //                accessList = accessGroupGates[owner.AccessGroupID];
        //            if (accessList.Count > 0)
        //            {
        //                foreach (AccessGroupXGateTO agg in accessList)
        //                {
        //                    if (agg.GateID == validationTerminal.A0GateID)
        //                    {
        //                        if (timeAccessDict.ContainsKey(agg.GateTimeAccessProfile))
        //                        {
        //                            TimeAccessProfileDtlTO dtl = timeAccessDict[agg.GateTimeAccessProfile];
        //                            foreach (int i in timeAccessHoursList[dtl.TimeAccessProfileId].Keys)
        //                            {
        //                                if (!dataDictionary[t.TagID][0].ContainsKey(i) && timeAccessHoursList[dtl.TimeAccessProfileId][i].ContainsKey(validationTerminal.A0Direction))
        //                                    dataDictionary[t.TagID][0].Add(i, timeAccessHoursList[dtl.TimeAccessProfileId][i][validationTerminal.A0Direction]);
        //                                isDefault0 = false;
        //                            }

        //                        }
        //                    }
        //                    if (agg.GateID == validationTerminal.A1GateID)
        //                    {
        //                        if (timeAccessDict.ContainsKey(agg.GateTimeAccessProfile))
        //                        {
        //                            TimeAccessProfileDtlTO dtl = timeAccessDict[agg.GateTimeAccessProfile];
        //                            foreach (int i in timeAccessHoursList[dtl.TimeAccessProfileId].Keys)
        //                            {
        //                                if (!dataDictionary[t.TagID][1].ContainsKey(i) && timeAccessHoursList[dtl.TimeAccessProfileId][i].ContainsKey(validationTerminal.A1Direction))
        //                                    dataDictionary[t.TagID][1].Add(i, timeAccessHoursList[dtl.TimeAccessProfileId][i][validationTerminal.A1Direction]);
        //                                isDefoult1 = false;
        //                            }
        //                        }
        //                    }

        //                }
        //            }

        //            if ((isDefault0 || isDefoult1) && timeAccessDict.ContainsKey(0))
        //            {
        //                TimeAccessProfileDtlTO dtl = timeAccessDict[0];

        //                if (isDefoult1)
        //                {
        //                    foreach (int i in timeAccessHoursList[dtl.TimeAccessProfileId].Keys)
        //                    {
        //                        if (!dataDictionary[t.TagID][1].ContainsKey(i) && timeAccessHoursList[dtl.TimeAccessProfileId][i].ContainsKey(validationTerminal.A1Direction))
        //                            dataDictionary[t.TagID][1].Add(i, timeAccessHoursList[dtl.TimeAccessProfileId][i][validationTerminal.A1Direction]);
        //                    }
        //                }
        //                if (isDefault0)
        //                {
        //                    foreach (int i in timeAccessHoursList[dtl.TimeAccessProfileId].Keys)
        //                    {
        //                        if (!dataDictionary[t.TagID][0].ContainsKey(i) && timeAccessHoursList[dtl.TimeAccessProfileId][i].ContainsKey(validationTerminal.A0Direction))
        //                            dataDictionary[t.TagID][0].Add(i, timeAccessHoursList[dtl.TimeAccessProfileId][i][validationTerminal.A0Direction]);
        //                    }
        //                }
        //            }
        //        }
        //        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
        //        //lastDBLoadTime = DateTime.Now.Ticks;
        //        WriteLog("Data reading end", Constants.basicDebugLevel);
        //        succ = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (dbConnection != null)
        //        {
        //            try
        //            {
        //                DBConnectionManager.Instance.CloseDBConnection(dbConnection);
        //            }
        //            catch { }
        //        }
        //        log.writeLog(ex);
        //    }
        //    return succ;
        //}

        /** PRIMARY USED FOR FIAT VALIDATION, NOW USED getValidationData() **/
        public bool getValidationDataFiat()
        {
            bool succ = false;
            try
            {
                WriteLog("Data reading started fiat", Constants.basicDebugLevel);
                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();

                //initialize data dictionary

                employeesDictionary = new Dictionary<int, EmployeeTO>();
                tagsActive = new Dictionary<ulong, TagTO>();
                restaurantAvailable = new Dictionary<int, int>();
                pairsForToday = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                schemaEmployees = new Dictionary<int, WorkTimeSchemaTO>();
                timeSchemaInterval = new Dictionary<int, List<WorkTimeIntervalTO>>();
                passTypesForCompany = new Dictionary<int, Dictionary<int, PassTypeTO>>();
                wUnits = new Dictionary<int, WorkingUnitTO>();


                foreach (int comp in Enum.GetValues(typeof(Constants.FiatCompanies)))
                {
                    if (!passTypesForCompany.ContainsKey(comp))
                    {
                        passTypesForCompany.Add(comp, getPassTypeList(Common.Misc.getRootWorkingUnit(comp, getWUnits(dbConnection))));
                    }

                }
                wUnits = getWUnits(dbConnection);
                //get all tags from DB
                Tag tag = new Tag(dbConnection);
                tag.TgTO.Status = Constants.statusActive;

                tagsActive = tag.SearchActive();


                //create string to search employees from tagID's
                string tags = "";

                foreach (KeyValuePair<ulong, TagTO> pair in tagsActive)
                {
                    TagTO tagTO = pair.Value;
                    tags += tagTO.TagID + ", ";

                }
                if (tags.Length > 0)
                {
                    tags = tags.Substring(0, tags.Length - 2);
                }

                //search employees by tags
                tagsEmployees = new Employee(dbConnection).SearchByTagsDictionary(tags);

                //search worktimeschemas
                Dictionary<int, WorkTimeSchemaTO> schemas = Common.Misc.getSchemas(null, dbConnection);

                string emplID = "";


                //aktivni tagovi i aktivni zaposleni
                foreach (KeyValuePair<uint, EmployeeTO> pair in tagsEmployees)
                {
                    EmployeeTO eTO = pair.Value;
                    if (!employeesDictionary.ContainsKey(eTO.EmployeeID))
                        employeesDictionary.Add(eTO.EmployeeID, eTO);

                    emplID += eTO.EmployeeID + ",";

                    int company = Common.Misc.getRootWorkingUnit(eTO.WorkingUnitID, wUnits);
                    int restaurant = -1;
                    // company restaurant rule
                    Common.Rule rulePaymentDay = new Common.Rule(dbConnection);
                    rulePaymentDay.RuleTO.WorkingUnitID = company;
                    rulePaymentDay.RuleTO.EmployeeTypeID = eTO.EmployeeTypeID;
                    rulePaymentDay.RuleTO.RuleType = Constants.RuleRestaurant;

                    List<RuleTO> rulesPaymentDay = rulePaymentDay.Search();

                    if (rulesPaymentDay.Count == 1)
                    {
                        restaurant = rulesPaymentDay[0].RuleValue;
                    }
                    if (!restaurantAvailable.ContainsKey(eTO.EmployeeID))
                        restaurantAvailable.Add(eTO.EmployeeID, restaurant);

                }
                if (emplID.Length > 0)
                    emplID = emplID.Remove(emplID.LastIndexOf(','));

                Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(dbConnection).SearchEmployeesSchedulesExactDate(emplID.Trim(), DateTime.Now.Date.AddDays(-1), DateTime.Now.Date.AddDays(1), null);
                foreach (KeyValuePair<uint, EmployeeTO> pair in tagsEmployees)
                {
                    EmployeeTO eTO = pair.Value;
                    // get time schedule and day time schema intervals for current, previous and next day
                    List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

                    if (emplSchedules.ContainsKey(eTO.EmployeeID))
                        timeScheduleList = emplSchedules[eTO.EmployeeID];

                    //wt intervals for that day
                    List<WorkTimeIntervalTO> timeSchemaIntervalList = Common.Misc.getTimeSchemaInterval(DateTime.Now.Date, timeScheduleList, schemas);
                    if (!timeSchemaInterval.ContainsKey(eTO.EmployeeID))
                    {
                        timeSchemaInterval.Add(eTO.EmployeeID, timeSchemaIntervalList);

                    }
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                        schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];

                    if (!schemaEmployees.ContainsKey(eTO.EmployeeID))
                    {
                        schemaEmployees.Add(eTO.EmployeeID, schema);
                    }
                }

                pairsForToday = new IOPairProcessed(dbConnection).getPairsForInterval(DateTime.Now.Date, new DateTime(), emplID);



                DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                //lastDBLoadTime = DateTime.Now.Ticks;
                WriteLog("Data reading end", Constants.basicDebugLevel);
                succ = true;
            }
            catch (Exception ex)
            {
                if (dbConnection != null)
                {
                    try
                    {
                        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                    }
                    catch { }
                }
                log.writeLog(ex);
            }
            return succ;
        }


        /** CURRENTLY USED FOR FIAT RESTAURANT VALIDATION **/
        public bool getValidationData()
        {
            bool succ = false;
            try
            {

                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();

                //initialize data dictionary
                employeesDictionary = new Dictionary<int, EmployeeTO>();
                tagsActive = new Dictionary<ulong, TagTO>();
                restaurantAvailable = new Dictionary<int, int>();
                cardsList = new ArrayList();
                wUnits = new Dictionary<int, WorkingUnitTO>();


                wUnits = getWUnits(dbConnection);
                //get all tags from DB
                Tag tag = new Tag(dbConnection);
                tag.TgTO.Status = Constants.statusActive;

                tagsActive = tag.SearchActive();

                List<TagTO> list = new Tag(dbConnection).Search();
                foreach (TagTO tagTO in list)
                {

                    if (!tagsAll.ContainsKey(tagTO.TagID))
                        tagsAll.Add(tagTO.TagID, tagTO);


                }

                //create string to search employees from tagID's
                string tags = "";

                foreach (KeyValuePair<ulong, TagTO> pair in tagsActive)
                {
                    TagTO tagTO = pair.Value;
                    tags += tagTO.TagID + ", ";

                }
                if (tags.Length > 0)
                {
                    tags = tags.Substring(0, tags.Length - 2);
                }

                //search employees by tags
                tagsEmployees = new Employee(dbConnection).SearchByTagsDictionary(tags);

                string emplIDs = "";
                //aktivni tagovi i aktivni zaposleni
                foreach (KeyValuePair<uint, EmployeeTO> pair in tagsEmployees)
                {
                    EmployeeTO eTO = pair.Value;
                    if (!employeesDictionary.ContainsKey(eTO.EmployeeID))
                        employeesDictionary.Add(eTO.EmployeeID, eTO);
                    emplIDs += eTO.EmployeeID + ",";

                    int company = Common.Misc.getRootWorkingUnit(eTO.WorkingUnitID, wUnits);
                    int restaurant = -1;
                    // company restaurant rule
                    Common.Rule rulePaymentDay = new Common.Rule(dbConnection);
                    rulePaymentDay.RuleTO.WorkingUnitID = company;
                    rulePaymentDay.RuleTO.EmployeeTypeID = eTO.EmployeeTypeID;
                    rulePaymentDay.RuleTO.RuleType = Constants.RuleRestaurant;

                    List<RuleTO> rulesPaymentDay = rulePaymentDay.Search();

                    if (rulesPaymentDay.Count == 1)
                    {
                        restaurant = rulesPaymentDay[0].RuleValue;
                    }

                    if (!restaurantAvailable.ContainsKey(eTO.EmployeeID))
                        restaurantAvailable.Add(eTO.EmployeeID, restaurant);
                    if (restaurant != 0 && restaurant != -1)
                    {
                        if (tagsActive.ContainsKey(pair.Key))
                        {
                            Card card = new Card(pair.Key, (byte)tagsActive[pair.Key].AccessGroupID);
                            cardsList.Add(card);
                        }
                    }

                }
                if (emplIDs.Length > 0)
                    emplIDs = emplIDs.Remove(emplIDs.LastIndexOf(','));
                employeesAsco4 = new EmployeeAsco4(dbConnection).SearchDictionary(emplIDs);

                DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                //lastDBLoadTime = DateTime.Now.Ticks;

                succ = true;
            }
            catch (Exception ex)
            {
                if (dbConnection != null)
                {
                    try
                    {
                        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                    }
                    catch { }
                }
                log.writeLog(ex);
            }
            return succ;
        }


        public Dictionary<int, WorkingUnitTO> getWUnits(object conn)
        {
            try
            {
                Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int, WorkingUnitTO>();
                List<WorkingUnitTO> wuList = new WorkingUnit(conn).Search();

                foreach (WorkingUnitTO wu in wuList)
                {
                    if (!wUnits.ContainsKey(wu.WorkingUnitID))
                        wUnits.Add(wu.WorkingUnitID, wu);
                    else
                        wUnits[wu.WorkingUnitID] = wu;
                }

                return wUnits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleListForEmployee, List<WorkTimeSchemaTO> timeSchema)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
            try
            {
                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleListForEmployee.Count; scheduleIndex++)
                {

                    if (date >= timeScheduleListForEmployee[scheduleIndex].Date)
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleListForEmployee[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleListForEmployee[timeScheduleIndex].TimeSchemaID;
                    List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                    foreach (WorkTimeSchemaTO timeSch in timeSchema)
                    {
                        if (timeSch.TimeSchemaID == schemaID)
                        {
                            timeSchemaEmployee.Add(timeSch);
                        }
                    }
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaEmployee.Count > 0)
                    {
                        schema = timeSchemaEmployee[0];
                        cycleDuration = schema.CycleDuration;
                    }

                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleListForEmployee[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return intervalList;
        }

        public void WriteLog(string message, int level)
        {
            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + validationTerminalOnline.ReaderIPAddress + "; " + message.Trim());
        }

        public ArrayList GetGroups(bool justNoDelayRecords)
        {
            ArrayList cardList = new ArrayList();
            try
            {
                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                ArrayList ReaderLogList = new ArrayList();


                AccessControlFile acf = new AccessControlFile(dbConnection);
                ArrayList al = acf.Search(Constants.ACFilesTypeCards, validationTerminal.ReaderID, -1, Constants.ACFilesStatusUnused, new DateTime(0), new DateTime(0));
                log.writeBenchmarkLog(DateTime.Now + " GetGroups(): AccessControlFile.Search count: " + al.Count.ToString() + "\n");
                // Check if record for that reader exists
                if (al.Count > 0)
                {
                    byte[] uploadCard = ((AccessControlFile)al[0]).Content;
                    int cardRecordID = ((AccessControlFile)al[0]).RecordID;

                    //If there is more than one record with status Unused, update all except last one to status Overwritten
                    if (al.Count > 1)
                    {
                        bool updated = acf.UpdateOthers(validationTerminal.ReaderID, Constants.ACFilesTypeCards, Constants.ACFilesStatusUnused,
                            Constants.ACFilesStatusOverwritten, new DateTime(0), new DateTime(0), -1, cardRecordID, Constants.ReaderControlUser, true);
                        log.writeBenchmarkLog(DateTime.Now + " GetGroups(): AccessControlFile.UpdateOthers: " + updated.ToString() + "\n");
                    }

                    // Upload now
                    if (((AccessControlFile)al[0]).Delayed == (int)Constants.ACFilesDelay.DontDelay || !justNoDelayRecords)
                    {
                        List<TagTO> tagList = new List<TagTO>();

                        MemoryStream memStream = new MemoryStream(uploadCard);

                        // Set the position to the beginning of the stream.
                        memStream.Seek(0, SeekOrigin.Begin);

                        tagList = new Tag(dbConnection).GetFromXMLSource(memStream);

                        memStream.Close();

                        foreach (TagTO tag in tagList)
                        {
                            Card card = new Card(tag.TagID, (byte)tag.AccessGroupID);
                            cardList.Add(card);
                        }

                        bool dbupdated = acf.Update(-1, "", "", Constants.ACFilesStatusUsed, new DateTime(0),
                                            DateTime.Now, -1, cardRecordID, Constants.ReaderControlUser, true);
                    }
                }

                DBConnectionManager.Instance.CloseDBConnection(dbConnection);
            }
            catch (Exception ex)
            {
                if (dbConnection != null)
                {
                    try
                    {
                        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                    }
                    catch { }
                }
                log.writeLog(ex);
            }
            return cardList;
        }

        public ArrayList GetTimeAccessProfiles(bool justNoDelayRecords, ref string a0Direction, ref string a1Direction)
        {

            ArrayList timeAccessProfilesList = new ArrayList();
            a0Direction = validationTerminal.A0Direction;
            a1Direction = validationTerminal.A1Direction;

            try
            {
                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                ArrayList ReaderLogList = new ArrayList();


                AccessControlFile acf = new AccessControlFile();
                ArrayList al = acf.Search(Constants.ACFilesTypeTAProfile, validationTerminal.ReaderID, -1, Constants.ACFilesStatusUnused, new DateTime(0), new DateTime(0));
                log.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): AccessControlFile.Search count: " + al.Count.ToString() + "\n");
                // Check if record for that reader exists
                if (al.Count > 0)
                {
                    byte[] uploadTAP = ((AccessControlFile)al[0]).Content;
                    int tapRecordID = ((AccessControlFile)al[0]).RecordID;

                    //If there is more than one record with status Unused, update all except last one to status Overwritten
                    if (al.Count > 1)
                    {
                        bool updated = acf.UpdateOthers(validationTerminal.ReaderID, Constants.ACFilesTypeTAProfile, Constants.ACFilesStatusUnused,
                            Constants.ACFilesStatusOverwritten, new DateTime(0), new DateTime(0), -1, tapRecordID, Constants.ReaderControlUser, true);
                        log.writeBenchmarkLog(DateTime.Now + " UpdateTimeAccessProfiles(): AccessControlFile.UpdateOthers: " + updated.ToString() + "\n");
                    }

                    // Upload now
                    if (((AccessControlFile)al[0]).Delayed == (int)Constants.ACFilesDelay.DontDelay || !justNoDelayRecords)
                    {
                        TimeAccessProfileDtl timeAccessProfileDtl = new TimeAccessProfileDtl();

                        MemoryStream memStream = new MemoryStream(uploadTAP);

                        // Set the position to the beginning of the stream.
                        memStream.Seek(0, SeekOrigin.Begin);

                        timeAccessProfilesList = timeAccessProfileDtl.GetFromXMLSource(memStream);

                        memStream.Close();


                        bool update1 = acf.Update(-1, "", "", Constants.ACFilesStatusUsed, new DateTime(0),
                                            DateTime.Now, -1, tapRecordID, Constants.ReaderControlUser, true);
                    }
                }

                DBConnectionManager.Instance.CloseDBConnection(dbConnection);
            }
            catch (Exception ex)
            {
                if (dbConnection != null)
                {
                    try
                    {
                        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                    }
                    catch { }
                }
                log.writeLog(ex);
            }
            return timeAccessProfilesList;
        }

        public int InsertLog(uint tagID, DateTime dateTime, int readerID, int antenna)
        {
            int succ = 0;
            try
            {
                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                Log log = new Log(dbConnection);
                log.LgTO.TagID = tagID;
                log.LgTO.ReaderID = readerID;
                log.LgTO.Antenna = antenna;
                List<LogTO> list = log.SearchForLogPeriod(dateTime.AddMinutes(-1), dateTime.AddMinutes(1));
                if (list.Count <= 0)
                {
                    savePass(tagID, antenna, 0, DateTime.Now);
                }
                succ = 1;
                DBConnectionManager.Instance.CloseDBConnection(dbConnection);
            }
            catch (Exception ex)
            {
                if (dbConnection != null)
                {
                    try
                    {
                        DBConnectionManager.Instance.CloseDBConnection(dbConnection);
                    }
                    catch { }
                }
                log.writeLog(ex);
            }
            return succ;
        }

    }
}
