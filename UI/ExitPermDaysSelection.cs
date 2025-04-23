using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using System.Data.SqlClient;
using Common;
using Util;
using TransferObjects;
using ReaderInterface;

using System.Resources;
using System.Globalization;

namespace UI
{
    public partial class ExitPermDaysSelection : Form
    {
        ExitPermissionsAddAdvanced parentForm;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        public ExitPermDaysSelection(ExitPermissionsAddAdvanced parent)
        {
            InitializeComponent();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(ExitPermissionsAddAdvanced).Assembly);
            setLanguage();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           
            log = new DebugLog(logFilePath);
            this.CenterToScreen();
            this.parentForm = parent;
        }

        public ExitPermDaysSelection(ExitPermissionsAddAdvanced parent, ArrayList selctedDays)
        {
            InitializeComponent();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(ExitPermissionsAddAdvanced).Assembly);
            setLanguage();
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";

            log = new DebugLog(logFilePath);
            this.CenterToScreen();
            this.parentForm = parent;

            lvDays.BeginUpdate();
            foreach (DateTime date in selctedDays)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = date;
                item.Text = date.ToString("dd.MM.yyyy");
                lvDays.Items.Add(item);
            }
            lvDays.EndUpdate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermDaysSelection.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("ExitPermDaysSel", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);
                
                //label's text
                lblRemoveDay.Text = rm.GetString("lblRemoveDay", culture);
               
                // list view
                lvDays.BeginUpdate();
                lvDays.Columns.Add(rm.GetString("choosenDays", culture),lvDays.Width-25);
                lvDays.EndUpdate();


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermDaysSelection.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

       
        private void lvDays_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                this.parentForm.lvDays.BeginUpdate();
                this.parentForm.lvDays.Items.RemoveAt(this.lvDays.SelectedItems[0].Index);
                this.parentForm.lvDays.EndUpdate();

                this.lvDays.BeginUpdate();
                this.lvDays.Items.Remove(this.lvDays.SelectedItems[0]);
                this.lvDays.EndUpdate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermDaysSelection.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (monthCalendar1.SelectionStart.Date > DateTime.Now.Date)
                {
                    MessageBox.Show(rm.GetString("dateInFuture", culture));
                    return;
                }
                bool exist = false;
                foreach(ListViewItem lvItem in lvDays.Items)
                {
                    if(lvItem.Tag.Equals(monthCalendar1.SelectionStart.Date))
                    {
                        exist = true;
                    }
                }
                if (!exist)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = monthCalendar1.SelectionStart.Date;
                    item.Text = monthCalendar1.SelectionStart.ToString("dd.MM.yyyy");

                    this.lvDays.BeginUpdate();
                    this.lvDays.Items.Add(item);
                    this.lvDays.EndUpdate();

                    ListViewItem item1 = new ListViewItem();
                    item1.Tag = monthCalendar1.SelectionStart.Date;
                    item1.Text = monthCalendar1.SelectionStart.ToString("dd.MM.yyyy");

                    this.parentForm.lvDays.BeginUpdate();
                    this.parentForm.lvDays.Items.Add(item1);
                    this.parentForm.lvDays.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermDaysSelection.monthCalendar1_DateSelected(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                monthCalendar1.SelectionEnd = monthCalendar1.SelectionStart;
            }
            catch(Exception ex) {
                log.writeLog(DateTime.Now + " ExitPermDaysSelection.monthCalendar1_DateChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void ExitPermDaysSelection_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermDaysSelection.ExitPermDaysSelection_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

    }
}