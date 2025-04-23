using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

using Util;
using TransferObjects;
using Common;

namespace CommonWeb
{
    public class Misc
    {
        public Misc()
        {
        }

        // create date value from text box's string (expected date format is "dd.MM.yyyy.")
        public static DateTime createDate(string dateStr)
        {
            try
            {                
                string day = "";
                string month = "";
                string year = "";

                int dotIndex = dateStr.IndexOf('.');

                if (dotIndex > 0)
                {
                    day = dateStr.Substring(0, dotIndex);
                    dateStr = dateStr.Substring(dotIndex + 1);

                    dotIndex = dateStr.IndexOf('.');

                    if (dotIndex > 0)
                    {
                        month = dateStr.Substring(0, dotIndex);
                        dateStr = dateStr.Substring(dotIndex + 1);

                        dotIndex = dateStr.IndexOf('.');

                        if (dotIndex > 0)
                        {
                            year = dateStr.Substring(0, dotIndex);
                            dateStr = dateStr.Substring(dotIndex + 1);

                            if (!dateStr.Trim().Equals(""))
                                return new DateTime();
                        }
                        else
                            return new DateTime();
                    }
                    else
                        return new DateTime();
                }
                else
                    return new DateTime();

                int dateDay = -1;
                int dateMonth = -1;
                int dateYear = -1;

                if (!int.TryParse(day, out dateDay) || !int.TryParse(month, out dateMonth) || !int.TryParse(year, out dateYear))
                    return new DateTime();

                DateTime date = new DateTime();

                try
                {
                    date = new DateTime(dateYear, dateMonth, dateDay, 0, 0, 0);
                }
                catch
                {
                    date = new DateTime();
                }

                return date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // create time value from text box's string (expected time format is "HH:mm")
        public static DateTime createTime(string timeStr)
        {
            try
            {
                string hour = "";
                string minute = "";
                
                int index = timeStr.IndexOf(':');

                if (index > 0)
                {
                    hour = timeStr.Substring(0, index);
                    minute = timeStr.Substring(index + 1);
                }
                else
                    return new DateTime();

                int h = -1;
                int min = -1;
                
                if (!int.TryParse(hour, out h) || !int.TryParse(minute, out min))
                    return new DateTime();

                DateTime date = DateTime.Now;

                try
                {
                    date = new DateTime(date.Year, date.Month, date.Day, h, min, 0);
                }
                catch
                {
                    date = new DateTime();
                }

                return date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // cretate hours + minutes from minutes value
        public static string createHoursFromMinutes(int minutes)
        {
            try
            {
                if (minutes % 60 > 0)
                    return (minutes / 60) + "h" + (minutes % 60) + "min";
                else
                    return (minutes / 60) + "h";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get universal date time format
        public static string getDateTimeFormatUniversal()
        {
            try
            {
                return new CultureInfo("en-US", true).DateTimeFormat.SortableDateTimePattern;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Page filter methods
        // save controls state
        public static Dictionary<string, string> SaveState(Control ctrl, string key, Dictionary<string, string> filterState)
        {
            try
            {
                foreach (Control control in ctrl.Controls)
                {
                    if (control is TextBox)
                    {                        
                        string text = ((TextBox)control).Text;
                        if (((TextBox)control).Attributes["id"] != null)
                        {
                            text += "|id=" + ((TextBox)control).Attributes["id"];
                        }
                        if (!filterState.ContainsKey(key + ((TextBox)control).ID))
                            filterState.Add(key + ((TextBox)control).ID, text);
                        else
                            filterState[key + ((TextBox)control).ID] = text;
                    }
                    else if (control is Label)
                    {
                        string text = ((Label)control).Text;
                        if (!filterState.ContainsKey(key + ((Label)control).ID))
                            filterState.Add(key + ((Label)control).ID, text);
                        else
                            filterState[key + ((Label)control).ID] = text;
                    }
                    else if (control is DropDownList)
                    {
                        if (!filterState.ContainsKey(key + ((DropDownList)control).ID))
                            filterState.Add(key + ((DropDownList)control).ID, ((DropDownList)control).SelectedValue);
                        else
                            filterState[key + ((DropDownList)control).ID] = ((DropDownList)control).SelectedValue;
                    }
                    else if (control is CheckBox)
                    {
                        if (!filterState.ContainsKey(key + ((CheckBox)control).ID))
                            filterState.Add(key + ((CheckBox)control).ID, ((CheckBox)control).Checked.ToString());
                        else
                            filterState[key + ((CheckBox)control).ID] = ((CheckBox)control).Checked.ToString();
                    }
                    else if (control is RadioButton)
                    {
                        if (!filterState.ContainsKey(key + ((RadioButton)control).ID))
                            filterState.Add(key + ((RadioButton)control).ID, ((RadioButton)control).Checked.ToString());
                        else
                            filterState[key + ((RadioButton)control).ID] = ((RadioButton)control).Checked.ToString();
                    }
                    else if (control is MultiView)
                    {
                        if (!filterState.ContainsKey(key + ((MultiView)control).ID))
                            filterState.Add(key + ((MultiView)control).ID, ((MultiView)control).Views.IndexOf(((MultiView)control).GetActiveView()).ToString());
                        else
                            filterState[key + ((MultiView)control).ID] = ((MultiView)control).Views.IndexOf(((MultiView)control).GetActiveView()).ToString();
                    }
                    else if (control is Menu)
                    {
                        if (!filterState.ContainsKey(key + ((Menu)control).ID))
                            filterState.Add(key + ((Menu)control).ID, ((Menu)control).Items.IndexOf(((Menu)control).SelectedItem).ToString());
                        else
                            filterState[key + ((Menu)control).ID] = ((Menu)control).Items.IndexOf(((Menu)control).SelectedItem).ToString();
                    }
                    else if (control is ListBox)
                    {
                        int[] selectedIndexes = ((ListBox)control).GetSelectedIndices();
                        string selected = "";
                        foreach (int index in selectedIndexes)
                        {
                            selected += index + ",";
                        }

                        if (selected.Trim().Length > 0)
                            selected = selected.Substring(0, selected.Length - 1);

                        if (!filterState.ContainsKey(key + ((ListBox)control).ID))
                            filterState.Add(key + ((ListBox)control).ID, selected);
                        else
                            filterState[key + ((ListBox)control).ID] = selected;
                    }

                    if (control.Controls.Count > 0)
                        SaveState(control, key, filterState);
                }

                return filterState;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // load controls state
        public static void LoadState(Control ctrl, string key, Dictionary<string, string> filterState)
        {
            try
            {
                foreach (Control control in ctrl.Controls)
                {
                    if (control is TextBox)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((TextBox)control).ID))
                        {
                            string text = filterState[key + ((TextBox)control).ID];

                            int idAtributeIndex = text.IndexOf("|id=");
                            if (idAtributeIndex >= 0)
                            {                                
                                ((TextBox)control).Attributes["id"] = text.Substring(idAtributeIndex + 4);
                                text = text.Substring(0, idAtributeIndex);
                            }

                            ((TextBox)control).Text = text;
                        }
                    }
                    if (control is Label)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((Label)control).ID))
                            ((Label)control).Text = filterState[key + ((Label)control).ID];                        
                    }
                    else if (control is DropDownList)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((DropDownList)control).ID))
                            ((DropDownList)control).SelectedValue = filterState[key + ((DropDownList)control).ID];
                    }
                    else if (control is CheckBox)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((CheckBox)control).ID))
                            ((CheckBox)control).Checked = filterState[key + ((CheckBox)control).ID].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                    }
                    else if (control is RadioButton)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((RadioButton)control).ID))
                            ((RadioButton)control).Checked = filterState[key + ((RadioButton)control).ID].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                    }
                    else if (control is MultiView)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((MultiView)control).ID))
                            ((MultiView)control).SetActiveView(((MultiView)control).Views[int.Parse(filterState[key + ((MultiView)control).ID])]);
                    }
                    else if (control is Menu)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((Menu)control).ID))
                        {
                            for (int i = 0; i < ((Menu)control).Items.Count; i++)
                            {
                                if (i == int.Parse(filterState[key + ((Menu)control).ID]))
                                {
                                    ((Menu)control).Items[i].Selected = true;                                    
                                }

                                setMenuImage(((Menu)control), i, ((Menu)control).Items[i].Selected);
                                setMenuSeparator(((Menu)control), i, ((Menu)control).Items[i].Selected);
                            }                            
                        }
                    }
                    else if (control is ListBox)
                    {
                        if (filterState != null && filterState.ContainsKey(key + ((ListBox)control).ID))
                        {
                            string[] selectedIndexes = filterState[key + ((ListBox)control).ID].Split(',');
                            int itemIndex = -1;
                            foreach (string index in selectedIndexes)
                            {
                                if (int.TryParse(index, out itemIndex) && itemIndex >= 0 && itemIndex < ((ListBox)control).Items.Count)
                                    ((ListBox)control).Items[itemIndex].Selected = true;
                            }
                                
                        }
                    }

                    if (control.Controls.Count > 0)
                        LoadState(control, key, filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get language of login user
        public static string getLanguage(object user)
        {
            try
            {
                string lang = Constants.Lang_sr;

                if (user != null && user is ApplUserTO)
                    lang = ((ApplUserTO)user).LangCode;                

                return lang;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CultureInfo getCulture(object user)
        {
            try
            {
                return CultureInfo.CreateSpecificCulture(getLanguage(user));                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ResourceManager getResourceManager(string resources, System.Reflection.Assembly assembly)
        {
            try
            {
                return new ResourceManager(resources, assembly);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // set tab image
        public static void setMenuImage(Menu m, int index, bool isActive)
        {
            try
            {
                if (index >= 0 && index < m.Items.Count)
                {
                    if (isActive)
                    {
                        if (index == 0)
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/activeFirst.jpg";
                        else
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactive-activeSeparator.jpg";
                    }
                    else
                    {
                        if (index == 0)
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveFirst.jpg";
                        else if (!m.Items[index - 1].Selected)
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveSeparator.jpg";
                        else
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/active-nonactiveSeparator.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // set tab separator image
        public static void setMenuSeparator(Menu m, int index, bool isActive)
        {
            try
            {
                if (index == m.Items.Count - 1)
                {
                    if (isActive)
                    {
                        m.Items[index].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/activeLast.jpg";
                    }
                    else
                    {
                        m.Items[index].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveLast.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // set main tab image
        public static void setMainMenuImage(Menu m, int index, bool isActive, int tabCount)
        {
            try
            {
                if (index >= 0 && index < m.Items.Count && index < tabCount)
                {
                    if (isActive)
                    {
                        if (index == 0)
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/activeFirst.jpg";
                        else
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactive-activeSeparator.jpg";
                    }
                    else
                    {
                        if (index == 0)
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveFirst.jpg";
                        else if (!m.Items[index - 1].Selected)
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveSeparator.jpg";
                        else
                            m.Items[index].ImageUrl = "/ACTAWeb/CommonWeb/images/active-nonactiveSeparator.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // set main tab separator image
        public static void setMainMenuSeparator(Menu m, int index, bool isActive, int tabCount)
        {
            try
            {
                if (index >=0 && index < m.Items.Count && index == tabCount - 1)
                {
                    if (isActive)
                    {
                        m.Items[index].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/activeLast.jpg";
                    }
                    else
                    {
                        m.Items[index].SeparatorImageUrl = "/ACTAWeb/CommonWeb/images/nonactiveLast.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get userID of login user
        public static string getLoginUser(object user)
        {
            try
            {
                string userID = "";

                if (user != null && user is ApplUserTO)
                    userID = ((ApplUserTO)user).UserID;

                return userID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        // check if it is day with verified/confirmed pairs
        public static bool isVerifiedConfirmedDay(List<IOPairProcessedTO> dayPairs, string userID)
        {
            try
            {
                bool isVerifiedConfirmedDay = false;

                foreach (IOPairProcessedTO pairTO in dayPairs)
                {
                    if ((!pairTO.VerifiedBy.Trim().Equals("") && !pairTO.VerifiedBy.Trim().Equals(userID))
                        || (!pairTO.ConfirmedBy.Trim().Equals("") && !pairTO.ConfirmedBy.Trim().Equals(userID)))
                    {
                        isVerifiedConfirmedDay = true;
                        break;
                    }
                }

                return isVerifiedConfirmedDay;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // check if it is day set pair changed by user
        public static bool isUserChangedDay(List<IOPairsProcessedHistTO> dayPairs, string userID)
        {
            try
            {
                bool isUserChangedDay = true;

                foreach (IOPairsProcessedHistTO pairTO in dayPairs)
                {
                    if (!pairTO.ModifiedBy.Trim().Equals(userID))                        
                    {
                        isUserChangedDay = false;
                        break;
                    }
                }

                return isUserChangedDay;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // check if logged in user should automatically verify changed pair
        public static bool automaticallyVerified(object loginCategory, object loginEmpl, int emplID)
        {
            try
            {
                // TL and HRSSC automatically verify pairs for all except themselves
                
                    if (loginCategory != null && loginCategory is ApplUserCategoryTO && (((ApplUserCategoryTO)loginCategory).CategoryID == (int)Constants.Categories.TL
                    || ((ApplUserCategoryTO)loginCategory).CategoryID == (int)Constants.Categories.HRSSC
                    || ((ApplUserCategoryTO)loginCategory).CategoryID == (int)Constants.Categories.WCManager)) //MM
                    /* bilo
                     * if (loginCategory != null && loginCategory is ApplUserCategoryTO && (((ApplUserCategoryTO)loginCategory).CategoryID == (int)Constants.Categories.TL
                    || ((ApplUserCategoryTO)loginCategory).CategoryID == (int)Constants.Categories.HRSSC)
                     */
                {
                    if (loginEmpl != null)
                    {
                        if (loginEmpl is EmployeeTO && emplID != ((EmployeeTO)loginEmpl).EmployeeID)
                            return true;
                        else
                            return false;
                    }
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // check if logged in user should automatically verify changed pair
        public static bool checkLimit(object loginCategory)
        {
            try
            {
                // HRSSC do not check limits
                if (loginCategory != null && loginCategory is ApplUserCategoryTO && ((ApplUserCategoryTO)loginCategory).CategoryID == (int)Constants.Categories.HRSSC)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // return flag for verification after manually pair changes
        public static int verificationFlag(PassTypeTO pt, bool forVerification, bool forConfirmation)
        {
            try
            {
                if (forVerification || forConfirmation || pt.PassTypeID == Constants.absence || pt.PassTypeID == Constants.ptEmptyInterval || pt.ConfirmFlag == (int)Constants.Confirmation.NotConfirmed)
                    return (int)Constants.Verification.Verified;
                else
                    return (int)Constants.Verification.NotVerified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // return if pair belongs to previous day
        public static bool isPreviousDayPair(IOPairProcessedTO pairTO, Dictionary<int, PassTypeTO> passTypesAllDic, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema, 
            List<WorkTimeIntervalTO> dayIntervals)
        {
            try
            {
                bool previousDayPair = false;

                if (passTypesAllDic.ContainsKey(pairTO.PassTypeID))
                {
                    if (passTypesAllDic[pairTO.PassTypeID].IsPass == Constants.overtimePassType)
                    {
                        int index = indexOf(dayPairs, pairTO);

                        int i = index - 1;

                        bool overtimeAfterShift = true;
                        while (i >= 0 && i < dayPairs.Count - 1)
                        {
                            if (!dayPairs[i + 1].StartTime.Equals(dayPairs[i].EndTime))
                            {
                                overtimeAfterShift = false;
                                break;
                            }
                            else if (passTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && passTypesAllDic[dayPairs[i].PassTypeID].IsPass != Constants.overtimePassType)
                                break;

                            i--;
                        }

                        if (overtimeAfterShift)
                        {
                            if (i >= 0)
                            {
                                WorkTimeIntervalTO pairInterval = getPairInterval(dayPairs[i], dayPairs, schema, dayIntervals, passTypesAllDic);

                                if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                                {
                                    if (Common.Misc.isThirdShiftEndInterval(pairInterval))// && checkCutOffDate(pairTO.IOPairDate.AddDays(-1).Date))
                                        previousDayPair = true;
                                }
                            }
                            else if (dayPairs.Count > 0 && !dayPairs[0].StartTime.Equals(new DateTime()) && !dayPairs[0].EndTime.Equals(new DateTime())
                                    && dayPairs[0].StartTime.Hour == 0 && dayPairs[0].StartTime.Minute == 0)// && checkCutOffDate(dayPairs[0].IOPairDate.AddDays(-1).Date))
                                previousDayPair = true;
                        }
                    }
                    else
                    {
                        WorkTimeIntervalTO pairInterval = getPairInterval(pairTO, dayPairs, schema, dayIntervals, passTypesAllDic);

                        if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                        {
                            if (Common.Misc.isThirdShiftEndInterval(pairInterval))// && checkCutOffDate(PairTO.IOPairDate.AddDays(-1).Date))
                                previousDayPair = true;
                        }
                    }
                }

                return previousDayPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int indexOf(List<IOPairProcessedTO> dayPairs, IOPairProcessedTO pair)
        {
            try
            {
                int index = -1;

                for (int i = 0; i < dayPairs.Count; i++)
                {
                    if (pair.compare(dayPairs[i]))
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static WorkTimeIntervalTO getPairInterval(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema, List<WorkTimeIntervalTO> dayIntervals,
            Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                WorkTimeIntervalTO pairInterval = new WorkTimeIntervalTO();
                foreach (WorkTimeIntervalTO interval in dayIntervals)
                {
                    if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                    {
                        if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                            && passTypesAllDic.ContainsKey(pair.PassTypeID) && passTypesAllDic[pair.PassTypeID].IsPass != Constants.overtimePassType
                            && pair.EndTime.Subtract(pair.StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        {
                            pairInterval = interval.Clone();
                            pairInterval.StartTime = getIntervalStart(interval, dayPairs, schema, pair.IOPairDate.Date, passTypesAllDic);
                            pairInterval.EndTime = getIntervalEnd(interval, dayPairs, schema, pair.IOPairDate.Date, passTypesAllDic);
                            break;
                        }
                    }
                    else if (pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    {
                        pairInterval = interval;
                        break;
                    }
                }

                return pairInterval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int indexOfFirstInInterval(List<IOPairProcessedTO> dayPairs, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema, DateTime date, Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                int index = -1;

                for (int i = 0; i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(date.Date))
                        continue;

                    if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                    {
                        if (dayPairs[i].StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && dayPairs[i].EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                            && passTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && passTypesAllDic[dayPairs[i].PassTypeID].IsPass != Constants.overtimePassType
                            && dayPairs[i].EndTime.Subtract(dayPairs[i].StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        {
                            index = i;
                            break;
                        }
                    }
                    else if (dayPairs[i].StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && dayPairs[i].EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isWholeIntervalPair(IOPairProcessedTO pair, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema)
        {
            try
            {
                bool wholeIntervalPair = false;

                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                        && pair.EndTime.Subtract(pair.StartTime).TotalMinutes == interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                    {
                        wholeIntervalPair = true;
                    }
                }
                else if (interval.EndTime.TimeOfDay == pair.EndTime.TimeOfDay && interval.StartTime.TimeOfDay == pair.StartTime.TimeOfDay)
                    wholeIntervalPair = true;

                return wholeIntervalPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime getIntervalStart(WorkTimeIntervalTO interval, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema, DateTime date, Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                DateTime intervalStart = interval.StartTime;

                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    int index = indexOfFirstInInterval(dayPairs, interval, schema, date, passTypesAllDic);

                    if (index >= 0 && index < dayPairs.Count)
                        intervalStart = dayPairs[index].StartTime;
                    else
                        intervalStart = interval.EarliestArrived;
                }

                return intervalStart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime getIntervalEnd(WorkTimeIntervalTO interval, List<IOPairProcessedTO> dayPairs, WorkTimeSchemaTO schema, DateTime date, Dictionary<int, PassTypeTO> passTypesAllDic)
        {
            try
            {
                DateTime intervalEnd = getIntervalStart(interval, dayPairs, schema, date, passTypesAllDic).AddMinutes((int)(interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes));

                return intervalEnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LogOut(object loginUserLog, object loginUser, object conn, HttpSessionState session, HttpResponse response)
        {
            try
            {
                // close log record
                if (loginUserLog != null && loginUserLog is ApplUserLogTO)
                {
                    ApplUserLog log = new ApplUserLog(conn);
                    log.UserLogTO = (ApplUserLogTO)loginUserLog;

                    if (loginUser != null && loginUser is ApplUserTO)
                        log.Update("", ((ApplUserTO)loginUser).UserID);
                    else
                        log.Update();
                }

                session.Abandon();
                FormsAuthentication.SignOut();
                response.Redirect("/ACTAWeb/Login.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
