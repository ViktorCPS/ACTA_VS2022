using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using TransferObjects;
using Util;


namespace DataAccess
{
	/// <summary>
	/// DAO implementation for managing WorkingUnits data form XML files, 
	/// using serialization/deserialization.
	/// </summary>
	public class XMLWorkingUnitsDAO : WorkingUnitsDAO
	{
		private static DataSet cachedWorkingUnitsTO = new DataSet();
		private const string tableName = "AllWorkingUnits";
		private const string resultTableName = "ResultTable";

		private DataSet getCachedWorkingUnits()
		{
			try
			{
				if (!cachedWorkingUnitsTO.Tables.Contains(tableName) || 
				((DataTable) cachedWorkingUnitsTO.Tables[tableName]).Rows.Count == 0)
				{
					deserializeToCache();
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return cachedWorkingUnitsTO;
		}

		public XMLWorkingUnitsDAO()
		{
			
		}

		#region WorkingUnitsDAO Members

		public int insert(WorkingUnitTO wuTO)
		{
			// TODO:  Add XMLWorkingUnitsDAO.insert implementation
			return 0;
		}
        public int insert(WorkingUnitTO wuTO, bool doCommit)
        {
            // TODO:  Add XMLWorkingUnitsDAO.insert implementation
            return 0;
        }

		public bool delete(int workingUnitsID)
		{
			// TODO:  Add XMLWorkingUnitsDAO.delete implementation
			return false;
		}

		public bool update(WorkingUnitTO wuTO)
		{
			// TODO:  Add XMLWorkingUnitsDAO.update implementation
			return false;
		}

		public WorkingUnitTO find(int workingUnitID)
		{
			// TODO:  Add XMLWorkingUnitsDAO.find implementation
			return null;
		}

        public DataSet getRootWorkingUnits(string workigUnitID)
        {
            return null;
        }
        public List<WorkingUnitTO> getRootWorkingUnitsList(string workigUnitID)
        {
            return null;
        }
		// TODO!!!!!
		public int findMAXWUID()
		{
			return 0;
		}
        public int findMINWUID()
        {
            return 0;
        }
        public Dictionary<int, WorkingUnitTO> getWUDictionary()
        {
            return null;
        }
        public Dictionary<int, WorkingUnitTO> getWUDictionary(IDbTransaction trans)
        {
            return null;
        }
        public List<WorkingUnitTO> getWUnits(string wUnits)
        {
            return null;
        }
        public List<WorkingUnitTO> getWorkingUnitsExact(WorkingUnitTO wuTO)
        {
            return null;
        }
        public List<WorkingUnitTO> getWorkingUnits(WorkingUnitTO wuTO)
		{
			DataSet dataSet = getCachedWorkingUnits();
			DataTable table = new DataTable();
			DataTable reusltTable = new DataTable(resultTableName);
			string select = "";
			StringBuilder sb = new StringBuilder();
            List<WorkingUnitTO> workingUnitsTOList = new List<WorkingUnitTO>();

			try
			{
				if (dataSet.Tables.Contains(tableName) && 
					((table = dataSet.Tables[tableName]).Rows.Count > 0))

				{
					reusltTable = table.Clone();

					if((wuTO.WorkingUnitID != -1) || (wuTO.ParentWorkingUID != -1) || 
						(!wuTO.Description.Trim().Equals("")) || (!wuTO.Name.Trim().Equals("")) || (!wuTO.Status.Trim().Equals("")))
					{
						if (wuTO.WorkingUnitID != -1)
						{
							sb.Append(" WorkingUnitID like '" + wuTO.WorkingUnitID.ToString().Trim() + "' and");
						}
						if (wuTO.ParentWorkingUID != -1)
						{
							sb.Append(" ParentWorkingUID like '" + wuTO.ParentWorkingUID.ToString().Trim() + "' and");
						}
						if (!wuTO.Description.Trim().Equals(""))
						{
							sb.Append(" Description like '" + wuTO.Description.ToUpper().Trim() + "' and");
						}
						if (!wuTO.Name.Trim().Equals(""))
						{
							sb.Append(" Name like '" + wuTO.Name.ToUpper().Trim() + "' and");
						}
						if (!wuTO.Status.Trim().Equals(""))
						{
							sb.Append(" Status like '" + wuTO.Status.ToUpper().Trim() + "' and");
						}

						select = sb.ToString(0, sb.ToString().Length - 3);
						DataRow[] resultRows = table.Select(select);

						foreach(DataRow row in resultRows)
						{
							reusltTable.ImportRow(row);
						}
					}
					else
					{
						reusltTable = table;
					}
					
					workingUnitsTOList = dataTable2ArrayList(reusltTable);

				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return workingUnitsTOList;
		}

        public List<WorkingUnitTO> getRootWU(string wuIDs)
        {
            return null;
        }

        public List<WorkingUnitTO> getChildWU(string parentID)
        {
            return null;
        }

		#endregion

		private void deserializeToCache()
		{
			ArrayList workingUnitsList = new ArrayList();
			try
			{
				//if (File.Exists(Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLWUsFile"]))
                if (File.Exists(Constants.XMLDataSourceDir + Constants.XMLWUsFile))
				{
					//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLWUsFile"];
                    string filename = Constants.XMLDataSourceDir + Constants.XMLWUsFile;
					Stream stream = File.Open(filename, FileMode.Open);

					XmlSerializer bformatter = new XmlSerializer(typeof(WorkingUnitTO[]));
					WorkingUnitTO[] deserialized = (WorkingUnitTO[]) bformatter.Deserialize(stream);
					workingUnitsList = ArrayList.Adapter(deserialized);

					stream.Close();

					cachedWorkingUnitsTO = toDataSet(workingUnitsList);
				}
				else
				{
					cachedWorkingUnitsTO = new DataSet();
					DataTable dataTable = new DataTable();
					cachedWorkingUnitsTO.Tables.Add(tableName);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void serialize(List<WorkingUnitTO> WorkingUnitTO)
		{
			try
			{
				//string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLWUsFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLWUsFile;
				Stream stream = File.Open(filename, FileMode.Create);

				WorkingUnitTO[] workingUnitArray = (WorkingUnitTO[]) WorkingUnitTO.ToArray();

				XmlSerializer bformatter = new XmlSerializer(typeof(WorkingUnitTO[]));
				bformatter.Serialize(stream, workingUnitArray);
				stream.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		private DataSet toDataSet(ArrayList list)
		{
			DataSet dataSet = new DataSet();

			try
			{
				dataSet.Tables.Add(tableName);
				DataTable dataTable = dataSet.Tables[tableName];
				
				dataTable.Columns.Add("WorkingUnitID", typeof(string));
				dataTable.Columns.Add("ParentWorkingUID", typeof(string));
				dataTable.Columns.Add("Description", typeof(string));
				dataTable.Columns.Add("Name", typeof(string));
				dataTable.Columns.Add("Status", typeof(string));
				dataTable.Columns.Add("AddressID", typeof(string));
				
				foreach(WorkingUnitTO wuTO in list)
				{
					DataRow row = dataTable.NewRow();

					row["WorkingUnitID"] = wuTO.WorkingUnitID.ToString() ;
					row["ParentWorkingUID"] = wuTO.ParentWorkingUID.ToString() ;
					row["Description"] = wuTO.Description;
					row["Name"] = wuTO.Name;
					row["Status"] = wuTO.Status;
					row["AddressID"] = wuTO.AddressID.ToString();
					
					dataTable.Rows.Add(row);
				}
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			
			return dataSet;
		}
        public List<WorkingUnitTO> getWUByName(string name)
        {
            
            //DataSet dataSet = new DataSet();
            //string select = "select * from actamgr.working_units where name='" + name + "'";
            //SqlCommand cmd = new SqlCommand(select, conn);
            //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);

            //sqlDataAdapter.Fill(dataSet, "WorkingUnits");
            //DataTable table = dataSet.Tables["WorkingUnits"];
            List<WorkingUnitTO> wUnitList = new List<WorkingUnitTO>();
            //WorkingUnitTO wu = new WorkingUnitTO();
            //foreach (var item in table.Rows)
            //{
            //    wu.WorkingUnitID = int.Parse(item["working_unit_id"].ToString());
            //    wu.Name = item["name"].ToString();
            //    wu.ParentWorkingUID = int.Parse(item["parent_working_unit_id"].ToString());
            //    wu.Description = item["description"].ToString();
            //    wu.Status = item["status"].ToString();
            //    wu.Code = item["code"].ToString();
            //    wu.EmplNumber = getNumberOfEmployees(wu.WorkingUnitID);
            //    wu.ChildWUNumber = getNumberOfChild(wu.WorkingUnitID);
            //    wUnitList.Add(wu);
            //}
            return wUnitList;
        }

		private List<WorkingUnitTO> dataTable2ArrayList(DataTable table)
		{
            List<WorkingUnitTO> workingUnitList = new List<WorkingUnitTO>();
			WorkingUnitTO wuTO = new WorkingUnitTO();

			try
			{
				foreach(DataRow row in table.Rows)
				{
					wuTO = new WorkingUnitTO();

					wuTO.WorkingUnitID = Int32.Parse(row["WorkingUnitID"].ToString());
					wuTO.ParentWorkingUID = Int32.Parse(row["ParentWorkingUID"].ToString());
					wuTO.Description = row["Description"].ToString();
					wuTO.Name = row["Name"].ToString();
					wuTO.Status = row["Status"].ToString();
					wuTO.AddressID = Int32.Parse(row["AddressID"].ToString());

					workingUnitList.Add(wuTO);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return workingUnitList;
		}


        public List<WorkingUnitTO> getWorkingUnitsForOU(string orgUnitID)
        {
            // TODO
            return null;
        }

		public bool update(WorkingUnitTO wuTO, bool doCommit)
		{
			// TODO:  Add XMLWorkingUnitsDAO.update implementation
			return false;
		}

		public bool beginTransaction()
		{
			// TODO
			return false;
		}

		public void commitTransaction()
		{
			// TODO
		}

		public void rollbackTransaction()
		{
			// TODO
		}

		public IDbTransaction getTransaction()
		{
			// TODO
			return null;
		}

		public void setTransaction(IDbTransaction trans)
		{
			// TODO
		}

		public WorkingUnitTO find(int workingUnitID, bool useTrans)
		{
			// TODO
			return null;
		}

        public DataSet getWorkingUnits(string workingUnitID)
        {
            // TODO
            return null;
        }
        public List<WorkingUnitTO> getAllWU()
        {
            return new List<WorkingUnitTO>();
        }
	}
}
