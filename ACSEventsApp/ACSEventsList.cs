using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Util;
using Common;
using System.Collections;
using TransferObjects;

namespace ACSEventsApp
{
    public partial class ACSEventsList : Form
    {

        private CultureInfo culture;
        ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        const int IDIndex = 0;
        const int EventTypeIndex = 1;
        const int NameIndex = 2;
        const int LocationIndex = 3;
        const int ACSNameIndex = 4;
        const int EventDateTimeIndex = 5;
        const int ACSIDIndex = 6;
        const int CardIDIndex = 7;
        const int DirectionIndex = 8;
        const int EmployeeIDIndex = 9;
        List<ACSEventTO> list = new List<ACSEventTO>();

        public ACSEventsList()
        {
            try
            {
                InitializeComponent();
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(ACSEventsList).Assembly);

                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();

                lvEvents.BeginUpdate();
                lvEvents.Columns.Add("ID", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("Tip događaja", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("Ime", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("Lokacija", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("ASC ime", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("Datum", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("ASC ID", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("ID kartice", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("Smer", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.Columns.Add("ID zaposlenog", (lvEvents.Width - 5) / 10, HorizontalAlignment.Left);
                lvEvents.EndUpdate();

                list = new List<ACSEventTO>();
                list = new ACSEvents().Search();
                sortOrder = Constants.sortAsc;
                sortField = ACSEventsList.IDIndex;
                startIndex = 0;

                populateList(list);
            }
            catch
            {
            }
        }

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List 
		*/
        private class ArrayListSort : IComparer<ACSEventTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(ACSEventTO x, ACSEventTO y)
            {
                ACSEventTO ev1 = null;
                ACSEventTO ev2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    ev1 = x;
                    ev2 = y;
                }
                else
                {
                    ev1 = y;
                    ev2 = x;
                }

                switch (compField)
                {
                    case ACSEventsList.IDIndex:
                        return ev1.Id.CompareTo(ev2.Id);
                    case ACSEventsList.EventTypeIndex:
                        return ev1.EventType.CompareTo(ev2.EventType);
                    case ACSEventsList.NameIndex:
                        return ev1.Name.CompareTo(ev2.Name);
                    case ACSEventsList.LocationIndex:
                        return ev1.Location.CompareTo(ev2.Location);
                    case ACSEventsList.ACSNameIndex:
                        return ev1.AcsName.CompareTo(ev2.AcsName);
                    case ACSEventsList.EventDateTimeIndex:
                        return ev1.EventDateTime.CompareTo(ev2.EventDateTime);
                    case ACSEventsList.ACSIDIndex:
                        return ev1.AcsID.CompareTo(ev2.AcsID);
                    case ACSEventsList.CardIDIndex:
                        return ev1.CardID.CompareTo(ev2.CardID);
                    case ACSEventsList.DirectionIndex:
                        return ev1.Direction.CompareTo(ev2.Direction);
                    case ACSEventsList.EmployeeIDIndex:
                        return ev1.EmployeeID.CompareTo(ev2.EmployeeID);
                    default:
                        return ev1.Id.CompareTo(ev2.Id);

                }
            }
        }

        #endregion


        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                list = new List<ACSEventTO>();
                list = new ACSEvents().Search();
                populateList(list);
            }
            catch
            {
                MessageBox.Show("Došlo je do greške u kreiranju liste.", "Greška!");
            }
        }

        private void populateList(List<ACSEventTO> list)
        {
            try
            {
                list.Sort(new ArrayListSort(sortOrder, sortField));
                if (list.Count > 0)
                {
                    lvEvents.BeginUpdate();
                    lvEvents.Items.Clear();

                    foreach (ACSEventTO acsEvent in list)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = acsEvent.Id.ToString().Trim();
                        item.SubItems.Add(acsEvent.EventType.ToString().Trim());
                        item.SubItems.Add(acsEvent.Name.ToString().Trim());
                        item.SubItems.Add(acsEvent.Location.ToString().Trim());
                        item.SubItems.Add(acsEvent.AcsName.ToString().Trim());
                        item.SubItems.Add(acsEvent.EventDateTime.ToString("dd.MM.yyyy HH:mm:ss"));
                        item.SubItems.Add(acsEvent.AcsID.ToString().Trim());
                        item.SubItems.Add(acsEvent.CardID.ToString().Trim());
                        item.SubItems.Add(acsEvent.Direction.ToString().Trim());
                        item.SubItems.Add(acsEvent.EmployeeID.ToString().Trim());
                        item.Tag = acsEvent;

                        lvEvents.Items.Add(item);
                    }


                    lvEvents.EndUpdate();
                    lvEvents.Invalidate();
                }
                else
                {
                    MessageBox.Show("Ne postoje podaci.", "Obaveštenje");
                }
            }
            catch
            {
                MessageBox.Show("Došlo je do greške u popunjavanju liste.", "Greška!");
            }
        }

        private void ACSEventsList_Load(object sender, EventArgs e)
        {

        }
        private int sortOrder = 0;
        private int sortField = 0;
        private int startIndex = 0;

        private void lvEvents_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int prevOrder = sortOrder;

                if (e.Column == sortField)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortField = e.Column;

                list.Sort(new ArrayListSort(sortOrder, sortField));
                startIndex = 0;
                populateList(list);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ASCEventList.lvEvents_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}
