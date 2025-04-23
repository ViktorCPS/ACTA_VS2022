using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Common;
using System.Globalization;
using System.Resources;

using Util;
using TransferObjects;

namespace UI
{
    public partial class Lockings : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        private DebugLog log;
        DateTime allReadersDate = new DateTime();

        ArrayList lockList;
        Lock currentLock;
        private int sortOrder;
        private int sortField;

        // List View indexes
        const int createdTimeIndex = 0;
        const int typeIndex = 1;
        const int DateIndex = 2;
        const int OperatorIndex = 3;
        

        public Lockings()
        {
            InitializeComponent();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

            rm = new ResourceManager("UI.Resource", typeof(ExitPermissionsAddAdvanced).Assembly);
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           
            log = new DebugLog(logFilePath);
        }

        #region Inner Class for sorting Array List of Lockings

        /*
		 *  Class used for sorting Array List of Lockings
		*/

        private class ArrayListSort : IComparer
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(object x, object y)
            {
                Lock lock1 = null;
                Lock lock2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    lock1 = (Lock)x;
                    lock2 = (Lock)y;
                }
                else
                {
                    lock1 = (Lock)y;
                    lock2 = (Lock)x;
                }

                switch (compField)
                {
                    case Lockings.createdTimeIndex:
                        return lock1.CreatedTime.CompareTo(lock2.CreatedTime);
                    case Lockings.typeIndex:
                        return lock1.Type.CompareTo(lock2.Type);
                    case Lockings.DateIndex:
                        return lock1.LockDate.CompareTo(lock2.LockDate);
                    case Lockings.OperatorIndex:
                        return lock1.CreatedBy.CompareTo(lock2.CreatedBy);
                    default:
                        return lock1.CreatedTime.CompareTo(lock2.CreatedTime);
                }
            }
        }

        #endregion


        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Lockings_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                currentLock = new Lock();
                lockList = currentLock.Search();
                lockList.Sort(new ArrayListSort(Constants.sortDesc, createdTimeIndex));
                //CultureInfo ci = CultureInfo.InvariantCulture;

                if (lockList.Count > 0)
                {
                    currentLock = (Lock)lockList[0];
                    if (currentLock.Type.Equals(Constants.lockTypeLock))
                        dtDate.Value = currentLock.LockDate.Date.AddDays(1);
                    else
                        dtDate.Value = currentLock.LockDate.Date;

                }
                this.CenterToScreen();
                setLanguage();
                populateListView();
                populateReadersListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.Lockings_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }

        }
        

        /// <summary>
        /// Set proper language.
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // button's text
                btnLockUnlock.Text = rm.GetString("btnLock", culture);                
                btnClose.Text = rm.GetString("btnClose", culture);

                // Form name
                this.Text = rm.GetString("menuLocking", culture);

                // group box text
                this.gbActionDef.Text = rm.GetString("gbActionDef", culture);
                gbActionType.Text = rm.GetString("gbActionType", culture);
                gbComment.Text = rm.GetString("gbComment", culture);
                gbCurentStatus.Text = rm.GetString("gbCurentStatus", culture);
                gbPreviewAction.Text = rm.GetString("gbPreviewAction", culture);
                gbTerminals.Text = rm.GetString("gbTerminals", culture);
                gbComment1.Text = rm.GetString("gbComment", culture);

                //tab page's text
                tpPreview.Text = rm.GetString("tpPreview", culture);
                tpLockingUnlocking.Text = rm.GetString("tpLockingUnlocking", culture);

                // radio buttons text
                rbLocking.Text = rm.GetString("rbLocking", culture);
                rbUnlocking.Text = rm.GetString("rbUnlocking", culture);

             
                // label's text
                lblActionDef.Text = rm.GetString("lblActionDefLock", culture);
                lblEndDate.Text = rm.GetString("lblPosibleChangeTill", culture);
                lblLastReaderTime.Text = rm.GetString("lblLastReaderTime", culture);

                // list view initialization
                lvResults.BeginUpdate();
                lvResults.Columns.Add(rm.GetString("hdrTime", culture), (lvResults.Width - 10) / 4 - 20, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrType", culture), (lvResults.Width - 10) / 4 + 20, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrDate", culture), (lvResults.Width - 10) / 4 - 20, HorizontalAlignment.Left);
                lvResults.Columns.Add(rm.GetString("hdrOperator", culture), (lvResults.Width - 10) / 4 , HorizontalAlignment.Left);
                lvResults.EndUpdate();

                // list view
                lvReaders.BeginUpdate();
                lvReaders.Columns.Add(rm.GetString("hdrReaderID", culture), (lvReaders.Width) / 6-10, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrDescripton", culture), 3 * (lvReaders.Width) / 6-10, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrLastReaderTime", culture), 2 * (lvReaders.Width) / 6 - 5, HorizontalAlignment.Left);
                lvReaders.EndUpdate();	
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate List View with earned hours found
        /// </summary>
        /// <param name="accessGroupsList"></param>
        private void populateReadersListView()
        {
            try
            {                
                List<ReaderTO> readerList = new Reader().SearchLastReadTime();
                //CultureInfo ci = CultureInfo.InvariantCulture;

                lvReaders.BeginUpdate();
                lvReaders.Items.Clear();

                //DateTime allReadersDate = new DateTime();

                if (readerList.Count > 0)
                {
                    foreach (ReaderTO reader in readerList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = reader.ReaderID.ToString();
                        item.SubItems.Add(reader.Description);
                        if (!reader.LastReadTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                        {
                            //item.SubItems.Add(reader.DateRead.ToString("dd/MM/yyyy  HH:mm", ci));
                            item.SubItems.Add(reader.LastReadTime.ToString("dd.MM.yyyy  HH:mm"));
                            if ((allReadersDate == new DateTime()) ||
                                (reader.LastReadTime < allReadersDate))
                                allReadersDate = reader.LastReadTime;
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }

                        lvReaders.Items.Add(item);
                    }
                }

                lvReaders.EndUpdate();
                lvReaders.Invalidate();

                if (allReadersDate != new DateTime())
                {
                    lblLastReaderTime.Text = lblLastReaderTime.Text + " " + allReadersDate.ToString("dd.MM.yyyy  HH:mm");
                    
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Populate List View with earned hours found
        /// </summary>
        /// <param name="accessGroupsList"></param>
        private void populateListView()
        {
            try
            {
               
                lvResults.BeginUpdate();
                lvResults.Items.Clear();

                //DateTime allReadersDate = new DateTime();

                if (lockList.Count > 0)
                {
                    foreach (Lock l in lockList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = l.CreatedTime.ToString("dd.MM.yyyy HH:mm");
                        item.SubItems.Add(l.Type);
                        item.SubItems.Add(l.LockDate.ToString("dd.MM.yyyy"));
                        item.SubItems.Add(l.CreatedBy);
                        item.Tag = l;

                        lvResults.Items.Add(item);
                    }
                }

                lvResults.EndUpdate();
                lvResults.Invalidate();
               
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbLocking_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbLocking.Checked)
                {
                    btnLockUnlock.Text = rm.GetString("btnLock", culture);
                    lblActionDef.Text = rm.GetString("lblActionDefLock", culture);
                }
                else
                {
                    btnLockUnlock.Text = rm.GetString("btnUnlock", culture);
                    lblActionDef.Text = rm.GetString("lblActionDefUnlock", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.rbLocking_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvResults.SelectedItems.Count == 1)
                {
                    Lock l = (Lock)lvResults.SelectedItems[0].Tag;
                    richTextBox1.Text = l.Comment;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.lvResults_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void lvResults_ColumnClick(object sender, ColumnClickEventArgs e)
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

                lockList.Sort(new ArrayListSort(sortOrder, sortField));
                
                populateListView();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.lvResults_ColumnClick): " + ex.Message + "\n");
                MessageBox.Show("Exception in lvResults_ColumnClick():" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnLockUnlock_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (rbLocking.Checked)
                {
                    if (currentLock.LockDate < dtpDate.Value.Date)
                    {
                        Lock lock1 = new Lock();

                        int inserted = new Lock().Save(dtpDate.Value.Date, Constants.lockTypeLock, tbComment.Text.ToString(), true);
                        if (inserted > 0)
                        {
                            MessageBox.Show(rm.GetString("lockSucceed", culture));
                        }
                        else
                            MessageBox.Show(rm.GetString("lockFaild", culture));
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("alreadyLocked", culture));
                    }
                }
                else
                {
                    if (currentLock.LockDate > dtpDate.Value.Date)
                    {
                        Lock l = new Lock();
                        bool trans = l.BeginTransaction();
                        if (trans)
                        {
                            try
                            {
                                int inserted = l.Save(dtpDate.Value.Date, Constants.lockTypeUnlock, tbComment.Text.ToString(), false);
                                if (inserted > 0)
                                {
                                    Pass p = new Pass();
                                    p.SetTransaction(l.GetTransaction());
                                    p.UnlockPasses(dtpDate.Value.Date, currentLock.LockDate, false);
                                    p.CommitTransaction();
                                    MessageBox.Show(rm.GetString("unlockSucceed", culture));
                                }
                                else
                                {
                                    MessageBox.Show(rm.GetString("unlockFaild", culture));
                                    l.RollbackTransaction();
                                }
                            }
                            catch (Exception ex)
                            {
                                l.RollbackTransaction();
                                log.writeLog(DateTime.Now + " Lockings.btnLockUnlock_Click(): " + ex.Message + "\n");
                                MessageBox.Show(rm.GetString("unlockFaild", culture));
                            }
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("alreadyUnlocked", culture));
                    }
                }
                tbComment.Text = "";
                rbLocking.Checked = true;
                dtpDate.Value = DateTime.Now.Date;
                lockList = currentLock.Search();
                if (lockList.Count > 0)
                {
                    currentLock = (Lock)lockList[0];
                    if (currentLock.Type.Equals(Constants.lockTypeLock))
                        dtDate.Value = currentLock.LockDate.Date.AddDays(1);
                    else
                        dtDate.Value = currentLock.LockDate.Date;
                }
                lockList.Sort(new ArrayListSort(Constants.sortDesc, createdTimeIndex));
                populateListView();
                NotificationController.SetLastLocking();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.btnLockUnlock_Click(): " + ex.Message + "\n");
                MessageBox.Show("Exception in btnLockUnlock_Click():" + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (dtpDate.Value.Date > DateTime.Now.Date)
                    dtpDate.Value = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Lockings.dtpDate_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show("Exception in dtpDate_ValueChanged():" + ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Lockings_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Lockings.IOPairs_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

    }
}