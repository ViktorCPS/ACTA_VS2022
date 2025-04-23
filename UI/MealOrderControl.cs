using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Util;
using Common;
using System.Collections;

namespace UI
{
    public partial class MealOrderControl : UserControl
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        public MealOrderControl()
        {
            InitializeComponent();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();
        }

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // button's text
                btnGenerateReport.Text = rm.GetString("btnGenerateReport", culture);              
             
                // label's text              
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            
             
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealOrderControl.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan ts = dtpTo.Value.Date - dtpFrom.Value.Date;
                if (ts.TotalDays > 30)
                {
                    MessageBox.Show(rm.GetString("selMaxOneMonth", culture));
                    return;
                }
                Hashtable list = new Hashtable();
                Hashtable typesTable = new Hashtable();
                MealsEmployeeSchedule ech = new MealsEmployeeSchedule();
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date;
                ArrayList scheduleList = ech.Search(from, to.AddDays(1), -1);

                MealType mealType = new MealType();
                ArrayList meals = mealType.Search(-1, "", "", new DateTime(), new DateTime());

                foreach (MealType m in meals)
                {
                    if (!typesTable.Contains(m.MealTypeID))
                    {
                        typesTable.Add(m.MealTypeID, m);
                    }
                }

                Dictionary<int, Dictionary<string, Dictionary<DateTime, int>>> shiftsOrdersByMealTypeDic = new Dictionary<int, Dictionary<string, Dictionary<DateTime, int>>>();
                foreach (MealsEmployeeSchedule sch in scheduleList)
                {
                    if (!shiftsOrdersByMealTypeDic.ContainsKey(sch.MealTypeID))
                    {
                        shiftsOrdersByMealTypeDic.Add(sch.MealTypeID, new Dictionary<string, Dictionary<DateTime, int>>());
                    }
                    if (!shiftsOrdersByMealTypeDic[sch.MealTypeID].ContainsKey(sch.Shift))
                    {
                        shiftsOrdersByMealTypeDic[sch.MealTypeID].Add(sch.Shift, new Dictionary<DateTime, int>());
                    }
                    if (!shiftsOrdersByMealTypeDic[sch.MealTypeID][sch.Shift].ContainsKey(sch.Date.Date))
                    {
                        shiftsOrdersByMealTypeDic[sch.MealTypeID][sch.Shift].Add(sch.Date.Date, 0);
                    }
                    shiftsOrdersByMealTypeDic[sch.MealTypeID][sch.Shift][sch.Date.Date] += 1;
                }


                //foreach (MealsEmployeeSchedule sch in scheduleList)
                //{
                //    if (!list.Contains(sch.MealTypeID))
                //    {
                //        list.Add(sch.MealTypeID, new Hashtable());
                //    }                   
                //    if (!((Hashtable)list[sch.MealTypeID]).Contains(sch.Date.Date))
                //    {
                //        ((Hashtable)list[sch.MealTypeID]).Add(sch.Date.Date, 0);
                //    }
                //    ((Hashtable)list[sch.MealTypeID])[sch.Date.Date] = ((int)((Hashtable)list[sch.MealTypeID])[sch.Date.Date])+1;
                //}

                DataSet dataSet = new DataSet();
                DataTable table = dataSet.Tables.Add("meal_order");

                table.Columns.Add("meal_type_id", typeof(System.String));
                table.Columns.Add("meal_type", typeof(System.String));
                table.Columns.Add("shift", typeof(System.String));
                table.Columns.Add("day_1", typeof(System.Int32));
                table.Columns.Add("day_2", typeof(System.Int32));
                table.Columns.Add("day_3", typeof(System.Int32));
                table.Columns.Add("day_4", typeof(System.Int32));
                table.Columns.Add("day_5", typeof(System.Int32));
                table.Columns.Add("day_6", typeof(System.Int32));
                table.Columns.Add("day_7", typeof(System.Int32));
                table.Columns.Add("day_8", typeof(System.Int32));
                table.Columns.Add("day_9", typeof(System.Int32));
                table.Columns.Add("day_10", typeof(System.Int32));
                table.Columns.Add("day_11", typeof(System.Int32));
                table.Columns.Add("day_12", typeof(System.Int32));
                table.Columns.Add("day_13", typeof(System.Int32));
                table.Columns.Add("day_14", typeof(System.Int32));
                table.Columns.Add("day_15", typeof(System.Int32));
                table.Columns.Add("day_16", typeof(System.Int32));
                table.Columns.Add("day_17", typeof(System.Int32));
                table.Columns.Add("day_18", typeof(System.Int32));
                table.Columns.Add("day_19", typeof(System.Int32));
                table.Columns.Add("day_20", typeof(System.Int32));
                table.Columns.Add("day_21", typeof(System.Int32));
                table.Columns.Add("day_22", typeof(System.Int32));
                table.Columns.Add("day_23", typeof(System.Int32));
                table.Columns.Add("day_24", typeof(System.Int32));
                table.Columns.Add("day_25", typeof(System.Int32));
                table.Columns.Add("day_26", typeof(System.Int32));
                table.Columns.Add("day_27", typeof(System.Int32));
                table.Columns.Add("day_28", typeof(System.Int32));
                table.Columns.Add("day_29", typeof(System.Int32));
                table.Columns.Add("day_30", typeof(System.Int32));
                table.Columns.Add("day_31", typeof(System.Int32));
                table.Columns.Add("total", typeof(System.Int32));
                table.Columns.Add("imageID", typeof(byte));

                DataTable tableI = dataSet.Tables.Add("images");
                tableI.Columns.Add("image", typeof(System.Byte[]));
                tableI.Columns.Add("imageID", typeof(byte));

                if (typesTable.Count > 0)
                {
                    foreach (int type in typesTable.Keys)
                    {

                        MealType mType = new MealType();
                        if (typesTable.Contains(type))
                        {
                            mType = (MealType)typesTable[type];
                        }
                        Dictionary<string, Dictionary<DateTime, int>> shiftsOrders = new Dictionary<string, Dictionary<DateTime, int>>();
                        if (shiftsOrdersByMealTypeDic.ContainsKey(type))
                        {
                            shiftsOrders = shiftsOrdersByMealTypeDic[type];
                            string shift = "I";
                            int noOfShifts = 0;

                            //foreach (string shift in shiftsOrders.Keys)
                            //{
                            while (noOfShifts < 3)
                            {
                                if (shiftsOrders.ContainsKey(shift))
                                {
                                    DataRow row = table.NewRow();

                                    row["meal_type_id"] = mType.MealTypeID;
                                    row["meal_type"] = mType.Name;
                                    row["shift"] = shift;
                                    int total = 0;
                                    int counter = 1;

                                    Dictionary<DateTime, int> orderByDay = new Dictionary<DateTime, int>();
                                    if (shiftsOrders.ContainsKey(shift))
                                    {
                                        orderByDay = shiftsOrders[shift];
                                    }
                                    for (DateTime date = from; date <= to; date = date.AddDays(1))
                                    {
                                        if (orderByDay.ContainsKey(date.Date))
                                        {
                                            row["day_" + counter.ToString()] = (int)orderByDay[date.Date];
                                            total += (int)orderByDay[date.Date];
                                        }
                                        else
                                            row["day_" + counter.ToString()] = 0;
                                        counter++;
                                    }
                                    for (int i = counter; counter <= 31; counter++)
                                    {
                                        row["day_" + counter.ToString()] = -1;
                                    }
                                    row["total"] = total;
                                    row["imageID"] = 1;

                                    table.Rows.Add(row);
                                    table.AcceptChanges();
                                }
                                shift += "I";
                                noOfShifts++;
                            }

                        }
                    }

                    //add logo image 
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();
                }
                if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                {
                    Reports.Reports_sr.MealOrderCRView_sr view = new Reports.Reports_sr.MealOrderCRView_sr(dataSet,
                        dtpFrom.Value, dtpTo.Value, NotificationController.GetLogInUser().Name);
                    view.ShowDialog(this);
                }
                if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                {
                    Reports.Reports_en.MealOrderCRView_en view = new Reports.Reports_en.MealOrderCRView_en(dataSet,
                        dtpFrom.Value, dtpTo.Value, NotificationController.GetLogInUser().Name);
                    view.ShowDialog(this);
                }
                if (table.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " MealOrderControl.btnGenerateReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
