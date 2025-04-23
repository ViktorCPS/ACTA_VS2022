using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TransferObjects;
using Util;
using System.Windows.Forms;
using DataAccess;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Common
{
   public class Filter
    {
        private string _applMenuItemID = "";
        private string _tabID = "";
        private ArrayList _controlValues = new ArrayList();
        private int _filterID = -1;
        private string _filterName = "";
        private string _description = "";
        private int _default = -1;
        private string _xmlDocument = "";
        private string _userID = "";
        private DateTime _createdTime = new DateTime();

        

       Hashtable controlTable = new Hashtable();
       Form currentForm = new Form();
       ComboBox currentCombo = new ComboBox();

        DAOFactory daoFactory = null;
        FilterDAO fdao = null;

        DebugLog log;

       public Button SerachButton = new Button();

       ArrayList alredySet = new ArrayList();

       public DateTime CreatedTime
       {
           get { return _createdTime; }
           set { _createdTime = value; }
       }
       public string UserID
       {
           get { return _userID; }
           set { _userID = value; }
       }

        public string XmlDocument
        {
            get { return _xmlDocument; }
            set { _xmlDocument = value; }
        }
        public int Default
        {
            get { return _default; }
            set { _default = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string FilterName
        {
            get { return _filterName; }
            set { _filterName = value; }
        }

        public int FilterID
        {
            get { return _filterID; }
            set { _filterID = value; }
        }

        public ArrayList ControlValues
        {
            get { return _controlValues; }
            set { _controlValues = value; }
        }

        public string TabID
        {
            get { return _tabID; }
            set { _tabID = value; }
        }

        public string ApplMenuItemID
        {
            get { return _applMenuItemID; }
            set { _applMenuItemID = value; }
        }

       public Filter()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			fdao = daoFactory.getFilterDAO(null);
			
		}
       public Filter(object dbConnection)
       {
           string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
           log = new DebugLog(logFilePath);

           daoFactory = DAOFactory.getDAOFactory();
           fdao = daoFactory.getFilterDAO(dbConnection);

       }

       public bool saveFilter(System.Windows.Forms.Form form,string tabID, string name, string description, int def,bool doCommit)
       {
           bool saved = false;
           try
           {
              //make array list of control values 
               
                   saveFilterControls(form.Controls, form);
               
                   foreach (Control contr in form.Controls)
                   {
                       if (contr.GetType().ToString().Equals("System.Windows.Forms.TabControl"))
                       {
                           foreach (Control c in contr.Controls)
                           {
                               if (c.Text.Equals(tabID))
                                   saveFilterControls(c.Controls, contr);
                           }
                       }
                   }
              

               if (ControlValues.Count > 0)
               {
                   //write control values to xml document
                   this.XmlDocument = fdao.serialize(ControlValues);

                   this.FilterName = name;
                   this.Description = description;
                   this.ApplMenuItemID = NotificationController.GetCurrentMenuItemID();
                   this.UserID = NotificationController.GetLogInUser().UserID;
                   this.TabID = tabID;                
                   this.Default = def;                   

                   //save fiter to data base
                   saved = fdao.save(this.sendTransferObject(),doCommit);
               }
               else
               {
                   throw new Exception("No Control on form marked as filterable.");
               }
                   
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.saveFilter(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return saved;
       }

       private void saveFilterControls(System.Windows.Forms.Control.ControlCollection controlCollection, Control parentControl)
       {
           try
           {
               if (!parentControl.GetType().ToString().Equals("System.Windows.Forms.Form") && !parentControl.GetType().ToString().Equals("System.Windows.Forms.TabPage"))
               {
                   foreach (Control ctrl in controlCollection)
                   {

                       if (ctrl.Tag == null || !ctrl.Tag.ToString().ToUpper().Trim().Equals(Constants.CONTROL_NOT_Filterable))
                       {
                           ControlFilterTO control = new ControlFilterTO();

                           controlToArray(ctrl, control);
                         
                       }
                   }
               }
               else
               {
                   foreach (Control ctrl in controlCollection)
                   {

                       if (ctrl.Tag != null && ctrl.Tag.ToString().ToUpper().Trim().Equals(Constants.CONTROL_Filterable))
                       {
                           ControlFilterTO control = new ControlFilterTO();

                           controlToArray(ctrl, control);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.saveFilter(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }
       private void controlToArray(Control formControl,ControlFilterTO arrayControl)
       {
           try
           {
               if (formControl.GetType().ToString().Substring(0, 3).Equals("UI."))
               {
                   saveFilterControls(formControl.Controls, formControl);
                   return;
               }

               if (formControl.Parent != null && formControl.Parent.AccessibleDescription != null)
               {
                   formControl.AccessibleDescription = formControl.Parent.AccessibleDescription;
               }

               string controlName = formControl.Name;
               if (formControl.AccessibleDescription != null)
                   controlName += formControl.AccessibleDescription.ToString() + controlName;

               switch (formControl.GetType().ToString())
               {
                   case Constants.CONTROL_GROUP_BOX:
                   case Constants.CONTROL_PANEL:
                   case "Reports.TypesCell":
                       saveFilterControls(formControl.Controls, formControl);
                       break;
                   case Constants.CONTROL_LIST_VIEW:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       string s = "";
                       foreach (ListViewItem item in ((ListView)formControl).SelectedItems)
                       {
                           s += item.Tag.ToString() + ",";
                       }
                       arrayControl.Value = s;
                       break;
                   case Constants.CONTROL_NUMERIC_UP_DOWN:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       arrayControl.Value = ((NumericUpDown)formControl).Value.ToString();
                       break;
                   case Constants.CONTROL_RADIO_BUTTON:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       arrayControl.Value = ((RadioButton)formControl).Checked.ToString();
                       break;
                   case Constants.CONTROL_RICH_TEXT_BOX:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       arrayControl.Value = ((RichTextBox)formControl).Text.ToString();
                       break;
                   case Constants.CONTROL_TEXT_BOX:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       arrayControl.Value = ((TextBox)formControl).Text.ToString();
                       break;
                   case Constants.CONTROL_COMBO_BOX:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       arrayControl.Value = ((ComboBox)formControl).SelectedValue.ToString();
                       break;
                   case Constants.CONTROL_CHECK_BOX:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       arrayControl.Value = ((CheckBox)formControl).Checked.ToString();
                       break;
                   case Constants.CONTROL_DATE_TIME_PICKER:
                       arrayControl.ControlID = controlName;
                       arrayControl.Type = formControl.GetType().ToString();
                       arrayControl.Value = ((DateTimePicker)formControl).Value.ToString();
                       break;
               }
               if (!arrayControl.ControlID.Equals(""))
               {
                   this.ControlValues.Add(arrayControl);
               }
           }
           catch
           { }
       }

       public void setFilter(Form form, int filterID)
       {
           try
           {
               alredySet = new ArrayList();
               controlTable = new Hashtable();

               FilterTO filter = fdao.find(filterID);

               this.FilterName = filter.FilterName;
               this.Description = filter.Description;
               this.Default = filter.Default;

               filter.ControlValues = fdao.deserialize(filter.XmlDocument);

               foreach (ControlFilterTO control in filter.ControlValues)
               {
                   if(!controlTable.ContainsKey(control.ControlID))
                   controlTable.Add(control.ControlID, control);

               }

               setFilterControls(form.Controls);
              
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.setFilter(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       private void setFilterControls(System.Windows.Forms.Control.ControlCollection controlCollection)
       {
           try
           {
               foreach (Control ctrl in controlCollection)
               {
                   if (alredySet.Contains(ctrl))
                       continue;

                   if (ctrl.Parent != null && ctrl.Parent.AccessibleDescription != null)
                   {
                       ctrl.AccessibleDescription = ctrl.Parent.AccessibleDescription;
                   }

                   if (ctrl.Name.Equals(currentCombo.Name))
                       continue;
                   ////for working unit's and employees we must reload combo box's before we turn of events
                   //if (ctrl.GetType().ToString().Equals(Constants.CONTROL_COMBO_BOX))
                   //{
                   //    try
                   //    {
                   //        ((ComboBox)ctrl).SelectedIndex = 0;
                   //    }
                   //    catch{}
                   //}
                   string controlName =  ctrl.Name;
                   if (ctrl.AccessibleDescription != null)
                       controlName += ctrl.AccessibleDescription.ToString() + controlName;

                   //if control is Group box or Panel recall method
                   if (ctrl.GetType().ToString().Equals(Constants.CONTROL_GROUP_BOX) || ctrl.GetType().ToString().Equals(Constants.CONTROL_PANEL)||
                       ctrl.GetType().ToString().Equals(Constants.CONTROL_TAB_PAGE)||ctrl.GetType().ToString().Equals(Constants.CONTROL_TAB_CONTROL)
                       || ctrl.GetType().ToString().Substring(0,3).Equals("UI.")||ctrl.GetType().ToString().Equals("Reports.TypesCell"))
                   {
                       setFilterControls(ctrl.Controls);
                   }                  
                     
                    //seting values of controls
                   else if (controlTable.ContainsKey(controlName))
                   {
                       try
                       {
                       ////turning of control event's
                       //FieldInfo pi = typeof(Component).GetField("events", BindingFlags.NonPublic | BindingFlags.Instance);
                       //object o = pi.GetValue(ctrl);
                       //if (pi != null)
                       //{
                       //    pi.SetValue(ctrl, null);
                       //}

                       ControlFilterTO control = (ControlFilterTO)controlTable[controlName];
                      
                           switch (ctrl.GetType().ToString())
                           {
                               case Constants.CONTROL_LIST_VIEW:                                  
                                   string[] s = control.Value.Split(',');
                                   ArrayList stringList = ArrayList.Adapter(s);

                                   ListView lv = (ListView)ctrl;
                                   lv.SelectedItems.Clear();
                                   foreach (ListViewItem item in ((ListView)ctrl).Items)
                                   {
                                       if (stringList.Contains(item.Tag.ToString()))
                                       {
                                           item.Selected = true;
                                       }                                  
                                       
                                   }
                                   if (lv.SelectedItems.Count > 0)
                                   {
                                       lv.EnsureVisible(lv.Items.IndexOf(lv.SelectedItems[0]));
                                   }
                                   break;
                               case Constants.CONTROL_NUMERIC_UP_DOWN:
                                   ((NumericUpDown)ctrl).Value = int.Parse(control.Value);
                                   break;
                               case Constants.CONTROL_RADIO_BUTTON:
                                   //pi.SetValue(ctrl, o);
                                   ((RadioButton)ctrl).Checked = bool.Parse(control.Value);
                                   break;
                               case Constants.CONTROL_RICH_TEXT_BOX:
                                   ((RichTextBox)ctrl).Text = control.Value;
                                   break;
                               case Constants.CONTROL_TEXT_BOX:
                                   ((TextBox)ctrl).Text = control.Value;
                                   break;
                               case Constants.CONTROL_COMBO_BOX:
                                   int i = 0;
                                   if (ctrl.Name.ToString().ToLower().Contains("empl"))
                                   {
                                       foreach (Control ctr in controlCollection)
                                       {
                                           if ((ctr.Name.ToString().ToLower().Contains("workingunit") || ctr.Name.ToString().ToLower().Contains("wu")) 
                                               && controlTable.ContainsKey(ctr.Name)&&ctr.GetType().ToString().Equals(Constants.CONTROL_COMBO_BOX))
                                           {
                                               ControlFilterTO contr = (ControlFilterTO)controlTable[ctr.Name];
                                               try
                                               {
                                                   ((ComboBox)ctr).SelectedValue = int.Parse(contr.Value);
                                               }
                                               catch
                                               {
                                                   try
                                                   {
                                                       ((ComboBox)ctr).SelectedValue = contr.Value;
                                                   }
                                                   catch
                                                   {
                                                       ((ComboBox)ctr).SelectedItem = contr.Value;
                                                   }
                                               }
                                               alredySet.Add(ctr);
                                           }
                                       }
                                   }
                                   bool isInt = int.TryParse(control.Value, out i);
                                   try
                                   {
                                       ((ComboBox)ctrl).SelectedValue = int.Parse(control.Value);
                                   }
                                   catch
                                   {
                                       try
                                       {
                                           ((ComboBox)ctrl).SelectedValue = control.Value;
                                       }
                                       catch
                                       {
                                           ((ComboBox)ctrl).SelectedItem = control.Value;
                                       }
                                   }
                                   break;
                               case Constants.CONTROL_CHECK_BOX:
                                   //pi.SetValue(ctrl, o);
                                   ((CheckBox)ctrl).Checked = bool.Parse(control.Value);
                                   break;
                               case Constants.CONTROL_DATE_TIME_PICKER:
                                   DateTime valueTime = DateTime.Parse(control.Value);
                                   if (control.ControlID.ToLower().Contains("from"))
                                   {
                                       if (valueTime.Day > DateTime.Now.Day)
                                       {
                                           valueTime = new DateTime(valueTime.Year, DateTime.Now.AddMonths(-1).Month, valueTime.Day, valueTime.Hour, valueTime.Minute, valueTime.Second);
                                       }
                                       else
                                       {
                                           valueTime = new DateTime(valueTime.Year, DateTime.Now.Month, valueTime.Day, valueTime.Hour, valueTime.Minute, valueTime.Second);
                                       }
                                   }
                                   else
                                   {
                                       valueTime = new DateTime(valueTime.Year, DateTime.Now.Month, valueTime.Day, valueTime.Hour, valueTime.Minute, valueTime.Second);
                                   }
                                   ((DateTimePicker)ctrl).Value = valueTime;
                                   break;
                           }
                       
                       
                       ////turn on events after values are set
                       //pi.SetValue(ctrl, o);
                   }
                       catch(Exception ex)
                       {
                           log.writeLog(DateTime.Now + " Filter.setFilterControls(): " + ex.Message + "\n");
                       }
                   }
                  
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.setFilterControls(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

        public void receiveTransferObject(FilterTO filterTO)
        {
            this.ApplMenuItemID = filterTO.ApplMenuItemID;
            this.ControlValues = filterTO.ControlValues;
            this.Default = filterTO.Default;
            this.Description = filterTO.Description;
            this.FilterID = filterTO.FilterID;
            this.FilterName = filterTO.FilterName;
            this.TabID = filterTO.TabID;
            this.XmlDocument= filterTO.XmlDocument;
        }

        /// <summary>
        /// Prepare TO for DAO processing
        /// </summary>
        /// <returns></returns>
        public FilterTO sendTransferObject()
        {
            FilterTO filterTO = new FilterTO();

            filterTO.ApplMenuItemID = this.ApplMenuItemID;
            filterTO.UserID = this.UserID;
            filterTO.ControlValues = this.ControlValues;
            filterTO.Default = this.Default;
            filterTO.Description = this.Description;
            filterTO.FilterID = this.FilterID;
            filterTO.FilterName = this.FilterName;
            filterTO.TabID = this.TabID;
            filterTO.XmlDocument = this.XmlDocument;
            filterTO.CreatedTime = this.CreatedTime;

            return filterTO;
        }
       private void currentCombo_DraItemEvent(object sender, DrawItemEventArgs e) 
       { 
           e.DrawBackground(); 
           if (e.Index == 0) 
           {
               Font fntFont = new Font(currentCombo.Font.FontFamily, currentCombo.Font.Size, FontStyle.Italic); 
               e.Graphics.DrawString(((FilterTO)currentCombo.Items[e.Index]).FilterName, fntFont, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height)); 
           } 
           else
               e.Graphics.DrawString(((FilterTO)currentCombo.Items[e.Index]).FilterName, currentCombo.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height)); 
           e.DrawFocusRectangle(); 
       }

       public void LoadFilters(ComboBox cbFilters,Form form,string newFilter)
       {
           try
           {
               //add event for combo box
               currentForm = form;
               currentCombo = cbFilters;
               currentCombo.SelectedIndexChanged += new EventHandler(cbFilters_SelectedIndexChanged);

               currentCombo.DrawMode = DrawMode.OwnerDrawVariable;
               currentCombo.DrawItem += new DrawItemEventHandler(currentCombo_DraItemEvent);
              
               //find all filters for form and user
               ApplUserTO user = NotificationController.GetLogInUser();
               string menuItem = NotificationController.GetCurrentMenuItemID();
               ArrayList filters = fdao.search(menuItem, user.UserID,this.TabID);
               FilterTO filterTo = new FilterTO();
               filterTo.FilterName = newFilter;
               filters.Insert(0, filterTo);
               if (filters.Count > 0)
               {
                   currentCombo.Visible = true;

                   
                   currentCombo.DataSource = filters;
                   currentCombo.DisplayMember = "FilterName";
                   currentCombo.ValueMember = "FilterID";

                   //select default filter
                   foreach (FilterTO filter in filters)
                   {
                       if (filter.Default == Constants.filterDefault)
                       {
                           currentCombo.SelectedValue = filter.FilterID;
                           break;
                       }
                   }
                   
                   cbFilters_SelectedIndexChanged(form, EventArgs.Empty);
               }
               else
               {
                   currentCombo.Visible = false;
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.LoadFilters(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       private void cbFilters_SelectedIndexChanged(object sender, System.EventArgs e)
       {
           try
           {
               currentForm.Cursor = Cursors.WaitCursor;
               int filterID = 0;
               if (currentCombo.SelectedValue != null)
               {
                   bool isInt = int.TryParse(currentCombo.SelectedValue.ToString(), out filterID);
                   if (this.FilterID != filterID)
                   {
                       this.FilterID = filterID;
                       if (isInt && filterID != -1)
                       {
                           setFilter(currentForm, filterID);
                           //SerachButton.PerformClick();
                       }
                   }
               }

           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.cbFilters_SelectedIndexChanged(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           finally
           {
               currentForm.Cursor = Cursors.Arrow;
           }
       }

       public ArrayList getDefaults(string tabID)
       {
           ArrayList defFilters = new ArrayList();
           try
           {
               defFilters = fdao.getDefaults(NotificationController.GetCurrentMenuItemID(), NotificationController.GetLogInUser().UserID, tabID);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.getDefaults(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return defFilters;
       }
       public bool BeginTransaction()
       {
           bool isStarted = false;

           try
           {
               isStarted = fdao.beginTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.BeginTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }

           return isStarted;
       }

       public void CommitTransaction()
       {
           try
           {
               fdao.commitTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.CommitTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public void RollbackTransaction()
       {
           try
           {
               fdao.rollbackTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.CommitTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public IDbTransaction GetTransaction()
       {
           try
           {
               return fdao.getTransaction();
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.GetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public void SetTransaction(IDbTransaction trans)
       {
           try
           {
               fdao.setTransaction(trans);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.SetTransaction(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
       }

       public bool setNoDefaults(string tabID,bool doCommit)
       {
           bool updated = false;
           try
           {
               updated = fdao.setNoDefaults(NotificationController.GetCurrentMenuItemID(), NotificationController.GetLogInUser().UserID, tabID, doCommit);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.setNoDefaults(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return updated;
       }

       public ArrayList getFilters(string tabID)
       {
           ArrayList defFilters = new ArrayList();
           try
           {
               defFilters = fdao.search(NotificationController.GetCurrentMenuItemID(), NotificationController.GetLogInUser().UserID, tabID);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.getFilters(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return defFilters;
       }

       public bool Delete(int filterID)
       {
           bool deleted = false;

           try
           {
               deleted = fdao.delete(filterID);
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.Delete(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return deleted;
       }

       public bool updateFilter(System.Windows.Forms.Form form, string tabID, string name, string description, int def,int filterID, bool doCommit)
       {
           bool isUpdated = false;
           try
           {
               //make array list of control values 
              
                   saveFilterControls(form.Controls, form);
              
                   foreach (Control contr in form.Controls)
                   {
                       if (contr.GetType().ToString().Equals("System.Windows.Forms.TabControl"))
                       {
                           foreach (Control c in contr.Controls)
                           {
                               if (c.Text.Equals(tabID))
                                   saveFilterControls(c.Controls, contr);
                           }
                       }
                   }
               

               if (ControlValues.Count > 0)
               {
                   //write control values to xml document
                   this.XmlDocument = fdao.serialize(ControlValues);

                   this.FilterName = name;
                   this.Description = description;
                   this.ApplMenuItemID = NotificationController.GetCurrentMenuItemID();
                   this.UserID = NotificationController.GetLogInUser().UserID;
                   this.TabID = tabID;
                   this.Default = def;

                   //save fiter to data base
                   isUpdated = fdao.update(this.sendTransferObject(), filterID, doCommit);
               }
               else
               {
                   throw new Exception("No Control on form marked as filterable.");
               }
           }
           catch (Exception ex)
           {
               log.writeLog(DateTime.Now + " Filter.updateFilter(): " + ex.Message + "\n");
               throw new Exception(ex.Message);
           }
           return isUpdated;
       }
   }
}
