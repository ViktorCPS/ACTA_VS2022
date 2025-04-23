using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ReaderInterface;
using Util;
using Common;
using TransferObjects;
using System.Net;

namespace ReaderRemoteManagement
{
    public class DownloadLog
    {
        private DebugLog log = null;
        //validation terminal controlling
        public ValidationTerminalTO validationTerminal = new ValidationTerminalTO();

        Object dbConnection = null;
        public OnlineMealsPointsTO validationTerminalOnline = new OnlineMealsPointsTO();

        Dictionary<uint, Dictionary<int, int>> dataDictionaryFiat = new Dictionary<uint, Dictionary<int, int>>();
        Dictionary<uint, Dictionary<int, Dictionary<int, List<int>>>> dataDictionary = new Dictionary<uint, Dictionary<int, Dictionary<int, List<int>>>>();

        //cards for upload
        ArrayList cardsList = new ArrayList();


        //tags and employees - ACTIVE
        Dictionary<uint, EmployeeTO> tagsEmployees = new Dictionary<uint, EmployeeTO>();

        Dictionary<int, EmployeeAsco4TO> employeesAsco4 = new Dictionary<int, EmployeeAsco4TO>();

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
        Dictionary<int, List<string>> listLocations = new Dictionary<int, List<string>>();


        public DownloadLog(ValidationTerminalTO vt, Dictionary<int, string> locations)
        {
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log" + vt.ValidationTerminalID + "_" + vt.Name + ".txt");
            validationTerminal = vt;

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
            getValidationData();

        }
        string fileloc;
        public void GetLog(bool diff, string loc)
        {
            ReaderFactory.TechnologyType = "MIFARE";
            IReaderInterface readerInterfaceLog = ReaderFactory.GetReader;
            ArrayList logLineList = new ArrayList();
            fileloc = loc;
            if (diff)
                logLineList = readerInterfaceLog.GetDiffLog(GetReaderAddress());
            else logLineList = ll();

            logsDownload(logLineList);

        }
        public int GetReaderAddress()
        {
            int address = -1;

            try
            {

                // Convert string to int
                try
                {
                    IPAddress ip = IPAddress.Parse(validationTerminal.IpAddress.Trim());
                    address = ip.GetAddressBytes()[3] + (ip.GetAddressBytes()[2] << 8) +
                        (ip.GetAddressBytes()[1] << 16) + (ip.GetAddressBytes()[0] << 24);
                }
                catch (Exception exIP)
                {
                    throw exIP;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Exception in: " + this.ToString() +
                    ".GetReaderAddres() : " + ex.Message);
            }

            return address;
        }

        private ArrayList ll()
        {
            ArrayList logLineList = new ArrayList();
            int counter = 0;
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(fileloc);
            while ((line = file.ReadLine()) != null)
            {
                LogLine lline = new LogLine();
                string[] str = line.Split('\t');
                lline.TagID = str[0];
                lline.Antenna = str[1];
                lline.Event = str[2];
                lline.Action = str[3];
                lline.DateTime = str[4];
                logLineList.Add(lline);
                counter++;
            }
            file.Close();
            return logLineList;
        }
        private bool logsDownload(ArrayList logLineList)
        {
            bool succBool = false;
            try
            {
                WriteLog("Number of meals downloaded: " + logLineList.Count, Constants.basicDebugLevel);
                // Kontrola da li su meseci u log zapisima u rastucem redosledu - anomalija primecena u Eunet-u, Hitag
                int prevMonth = -1;
                foreach (LogLine logLine in logLineList)
                {
                    int month = Int32.Parse((logLine.DateTime).Substring(5, 2));
                    if ((prevMonth > month) && (month != 1))
                    {
                        month = prevMonth;
                        logLine.DateTime = logLine.DateTime.Substring(0, 5) + month.ToString("D2") + logLine.DateTime.Substring(7);
                    }
                    prevMonth = month;
                }
                int succ = 0;
                int total = 0;
                foreach (LogLine logLine in logLineList)
                {
                    //standalone
                    if (logLine.Action.Equals("20") && logLine.Event.Equals("4"))
                    {

                        if (logLine.TagID.ToString().Trim() != "0")
                        {
                            uint tagid = uint.Parse(logLine.TagID.ToString());
                            DateTime date = DateTime.Parse(logLine.DateTime.ToString());
                            total++;
                            succ += InsertLog(tagid, date, validationTerminal.ValidationTerminalID, int.Parse(logLine.Antenna.ToString()));
                        }
                    }
                }

                WriteLog("Log Interted OK Num = " + succ + " / Log Download Num Total = " + total, Constants.extendedDebugLevel);
                succBool = succ == total;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Exception in: " + this.ToString() +
                    ".logsDownload() : " + ex.Message);
            }
            return succBool;
        }

        public int InsertLog(uint tagID, DateTime dateTime, int readerID, int antenna)
        {
            int succ = 0;
            try
            {
                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();
                int result = 0;
                int employeeID = -1;
                String shift = "";

                if (tagsEmployees.ContainsKey(tagID))
                {
                    employeeID = tagsEmployees[tagID].EmployeeID;
                }

                if (CheckRequest(tagID, antenna, ref result))
                {
                    int list = 0;
                    OnlineMealsUsed mealUsed = new OnlineMealsUsed(dbConnection);
                    List<OnlineMealsUsedTO> meals = mealUsed.Search(employeeID, dateTime.Date, dateTime.Date);

                    if (meals.Count == 0)
                    {
                        OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                        onlineMealUsedTo.EmployeeID = employeeID;
                        MealsEmployeeSchaduleTO mealsEmployeeTo = new MealsEmployeeSchaduleTO();
                        mealsEmployeeTo.Shift = shift;
                        if (dictionaryPoints.ContainsKey(antenna))
                        {
                            onlineMealUsedTo.PointID = dictionaryPoints[antenna].PointID;
                            onlineMealUsedTo.MealTypeID = dictionaryPoints[antenna].MealTypeID;
                            onlineMealUsedTo.Qty = 1;
                            onlineMealUsedTo.OnlineValidation = 1;
                            onlineMealUsedTo.ManualCreated = 0;
                            onlineMealUsedTo.EventTime = dateTime;
                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                            onlineMealUsedTo.CreatedTime = DateTime.Now;

                            OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                            onlineMeal.Save(true, 0);
                            WriteLog("Valid meal inserted, employee: " + employeeID + "shift" + shift, Constants.basicDebugLevel);

                        }
                    }
                    else
                    {
                        OnlineMealsUsed mealU = new OnlineMealsUsed(dbConnection);
                        List<OnlineMealsUsedTO> listMeals = mealU.Search(employeeID, dateTime.Date, dateTime.Date);
                        if (listMeals.Count > 0)
                        {
                            bool onlineValid = false;
                            foreach (OnlineMealsUsedTO meal in listMeals)
                            {
                                if (meal.OnlineValidation == 1)
                                {
                                    if (listMeals[0].EventTime.TimeOfDay > dateTime.TimeOfDay)
                                    {
                                        OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                                        onlineMealUsedTo.EmployeeID = employeeID;
                                        MealsEmployeeSchaduleTO mealsEmployeeTo = new MealsEmployeeSchaduleTO();
                                        mealsEmployeeTo.Shift = shift;
                                        if (dictionaryPoints.ContainsKey(antenna))
                                        {
                                            onlineMealUsedTo.PointID = dictionaryPoints[antenna].PointID;
                                            onlineMealUsedTo.MealTypeID = dictionaryPoints[antenna].MealTypeID;
                                            onlineMealUsedTo.Qty = 1;
                                            onlineMealUsedTo.OnlineValidation = 1;
                                            onlineMealUsedTo.ManualCreated = 0;
                                            onlineMealUsedTo.EventTime = dateTime;
                                            onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                            onlineMealUsedTo.CreatedTime = DateTime.Now;

                                            OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                            onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                            onlineMeal.Save(true, 0);
                                            WriteLog("Valid meal inserted, employee: " + employeeID + "smena" + shift, Constants.basicDebugLevel);

                                        }
                                        //change status, previous meal that is set to valid, and has to be used
                                        listMeals[0].OnlineValidation = 0;
                                        listMeals[0].ReasonRefused = Constants.RestaurantUsed;

                                        OnlineMealsUsed online = new OnlineMealsUsed(dbConnection);
                                        online.OnlineMealsUsedTO = listMeals[0];
                                        online.Update(true);
                                        onlineValid = true;
                                    }
                                }
                            }
                            if (!onlineValid)
                            {
                                OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                                onlineMealUsedTo.EmployeeID = employeeID;
                                if (dictionaryPoints.ContainsKey(antenna))
                                {
                                    onlineMealUsedTo.PointID = dictionaryPoints[antenna].PointID;
                                    onlineMealUsedTo.MealTypeID = dictionaryPoints[antenna].MealTypeID;
                                    onlineMealUsedTo.Qty = 1;
                                    onlineMealUsedTo.ReasonRefused = Constants.RestaurantUsed;
                                    onlineMealUsedTo.OnlineValidation = 0;
                                    onlineMealUsedTo.EventTime = dateTime;
                                    onlineMealUsedTo.ManualCreated = 0;
                                    onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                    onlineMealUsedTo.CreatedTime = DateTime.Now;

                                    OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                    onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                    onlineMeal.Save(true, 0);
                                    WriteLog("Used meal inserted, employee: " + employeeID + "shift" + shift, Constants.basicDebugLevel);
                                }
                            }
                        }
                    }
                }
                else
                {
                    switch (result)
                    {
                        case (int)Constants.MealDenied.CompanyRestaurant:
                            {
                                OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                                onlineMealUsedTo.EmployeeID = employeeID;
                                if (dictionaryPoints.ContainsKey(antenna))
                                {
                                    onlineMealUsedTo.PointID = dictionaryPoints[antenna].PointID;
                                    onlineMealUsedTo.MealTypeID = dictionaryPoints[antenna].MealTypeID;
                                    onlineMealUsedTo.Qty = 1;
                                    onlineMealUsedTo.ReasonRefused = Constants.RestaurantCompany;
                                    onlineMealUsedTo.OnlineValidation = 0;
                                    onlineMealUsedTo.EventTime = dateTime;
                                    onlineMealUsedTo.ManualCreated = 0;
                                    onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                    onlineMealUsedTo.CreatedTime = DateTime.Now;

                                    OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                    onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                    onlineMeal.Save(true, 0);
                                    WriteLog("Invalid meal inserted, employee: " + employeeID + "shift" + shift + ", reason: " + Constants.RestaurantCompany, Constants.basicDebugLevel);

                                }
                                break;
                            }
                        case (int)Constants.MealDenied.EmployeeNotActive:
                            {

                                OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                                onlineMealUsedTo.EmployeeID = employeeID;

                                if (dictionaryPoints.ContainsKey(antenna))
                                {
                                    onlineMealUsedTo.PointID = dictionaryPoints[antenna].PointID;
                                    onlineMealUsedTo.MealTypeID = dictionaryPoints[antenna].MealTypeID;
                                    onlineMealUsedTo.Qty = 1;
                                    onlineMealUsedTo.ReasonRefused = Constants.RestaurantEmployeeNotActive;
                                    onlineMealUsedTo.OnlineValidation = 0;
                                    onlineMealUsedTo.EventTime = dateTime;
                                    onlineMealUsedTo.ManualCreated = 0;
                                    onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                    onlineMealUsedTo.CreatedTime = DateTime.Now;

                                    OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                    onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                    onlineMeal.Save(true, 0);
                                    WriteLog("Invalid meal inserted, employee: " + employeeID + "shift" + shift + ", reason: " + Constants.RestaurantEmployeeNotActive, Constants.basicDebugLevel);

                                }
                                break;
                            }
                        case (int)Constants.MealDenied.TagUnknown:
                            {
                                WriteLog("Tag unknown: " + tagID, Constants.basicDebugLevel);

                                break;
                            }
                        case (int)Constants.MealDenied.TagNotActive:
                            {
                                EmployeeTO empl = new Employee(dbConnection).SearchByTag(tagID.ToString());
                                if (empl != new EmployeeTO())
                                {
                                    OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                                    onlineMealUsedTo.EmployeeID = empl.EmployeeID;

                                    if (dictionaryPoints.ContainsKey(antenna))
                                    {
                                        onlineMealUsedTo.PointID = dictionaryPoints[antenna].PointID;
                                        onlineMealUsedTo.MealTypeID = dictionaryPoints[antenna].MealTypeID;
                                        onlineMealUsedTo.Qty = 1;
                                        onlineMealUsedTo.ReasonRefused = Constants.RestaurantTagNotActive;
                                        onlineMealUsedTo.OnlineValidation = 0;
                                        onlineMealUsedTo.EventTime = dateTime;
                                        onlineMealUsedTo.ManualCreated = 0;
                                        onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                        onlineMealUsedTo.CreatedTime = DateTime.Now;

                                        OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                        onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                        onlineMeal.Save(true, 0);
                                        WriteLog("Invalid meal inserted, employee: " + employeeID + ", reason: " + Constants.RestaurantTagNotActive, Constants.basicDebugLevel);
                                    }
                                }
                                break;
                            }
                        case (int)Constants.MealDenied.Location:
                            {
                                OnlineMealsUsedTO onlineMealUsedTo = new OnlineMealsUsedTO();
                                onlineMealUsedTo.EmployeeID = employeeID;
                                if (dictionaryPoints.ContainsKey(antenna))
                                {
                                    onlineMealUsedTo.PointID = dictionaryPoints[antenna].PointID;
                                    onlineMealUsedTo.MealTypeID = dictionaryPoints[antenna].MealTypeID;
                                    onlineMealUsedTo.Qty = 1;
                                    onlineMealUsedTo.ReasonRefused = Constants.RestaurantLocation;
                                    onlineMealUsedTo.OnlineValidation = 0;
                                    onlineMealUsedTo.EventTime = dateTime;
                                    onlineMealUsedTo.ManualCreated = 0;
                                    onlineMealUsedTo.CreatedBy = "RC SERVICE";
                                    onlineMealUsedTo.CreatedTime = DateTime.Now;

                                    OnlineMealsUsedDaily onlineMeal = new OnlineMealsUsedDaily(dbConnection);
                                    onlineMeal.OnlineMealsUsedTO = onlineMealUsedTo;
                                    onlineMeal.Save(true, 0);
                                    WriteLog("Invalid meal inserted, employee: " + employeeID + ", reason: " + Constants.RestaurantLocation, Constants.basicDebugLevel);

                                }
                                break;
                            }
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
        public bool getValidationData()
        {
            bool succ = false;
            try
            {

                dbConnection = DBConnectionManager.Instance.MakeNewDBConnection();

                OnlineMealsPoints onlinePoints = new OnlineMealsPoints(dbConnection);
                dictionaryPoints = new Dictionary<int, OnlineMealsPointsTO>();
                List<OnlineMealsPointsTO> listPoints = onlinePoints.Search(validationTerminal.IpAddress, 0);
                if (listPoints.Count == 1)
                    dictionaryPoints.Add(0, listPoints[0]);
                listPoints = onlinePoints.Search(validationTerminal.IpAddress, 1);

                if (listPoints.Count == 1)
                    dictionaryPoints.Add(1, listPoints[0]);

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

        public void WriteLog(string message, int level)
        {

            log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Validation terminal IP: " + validationTerminal.IpAddress + "; " + message.Trim(), level);

        }

    }
}
