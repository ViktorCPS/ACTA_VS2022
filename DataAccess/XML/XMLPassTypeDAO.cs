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
    /// DAO implementation for managing PassTypes data form XML files, 
    /// using serialization/deserialization.
    /// </summary>
    public class XMLPassTypeDAO : PassTypeDAO
    {
        private static DataSet cachedPassTypesTO = new DataSet();
        private const string tableName = "AllPassTypes";
        private const string resultTableName = "resultRows";

        public XMLPassTypeDAO()
        {
        }

        #region PassTypeDAO Members

        public int insert(PassTypeTO ptTO)
        {
            // TODO:  Add XMLPassTypeDAO.insert implementation
            return 0;
        }

        public int insert(PassTypeTO ptTO, bool doCommit)
        {
            // TODO:  Add XMLPassTypeDAO.insert implementation
            return 0;
        }

        public bool delete(int passTypeID)
        {
            // TODO:  Add XMLPassTypeDAO.delete implementation
            return false;
        }

        public bool delete(int passTypeID, bool doCommit)
        {
            // TODO:  Add XMLPassTypeDAO.delete implementation
            return false;
        }

        public bool update(PassTypeTO ptTO, int oldButton)
        {
            // TODO:  Add XMLPassTypeDAO.update implementation
            return false;
        }
        public bool update(PassTypeTO ptTO, int oldButton, bool doCommit)
        {// TODO:  Add XMLPassTypeDAO.update implementation
            return false;
        }
        public PassTypeTO find(int passTypeID)
        {
            // TODO:  Add XMLPassTypeDAO.find implementation
            return null;
        }
        public Dictionary<int,PassTypeTO> find(string passTypeID,int company)
        {
            // TODO:  Add XMLPassTypeDAO.find implementation
            return null;
        }
        // TODO!!!!!
        public int findMAXPassTypeID()
        {
            return 0;
        }
        public Dictionary<int, PassTypeTO> getPassTypesDictionary(PassTypeTO ptTO)
        {
            return new Dictionary<int, PassTypeTO>();
        }
        public Dictionary<int, PassTypeTO> getPassTypesDictionary(PassTypeTO ptTO, IDbTransaction trans)
        {
            return new Dictionary<int, PassTypeTO>();
        }
        public Dictionary<int, PassTypeTO> getPassTypesDictionaryCodeSorted(PassTypeTO ptTO)
        {
            return new Dictionary<int, PassTypeTO>();
        }
        public Dictionary<int, PassTypeTO> getPassTypesForCompanyDictionary(int company, bool isAlternativeLang)
        {
            return new Dictionary<int, PassTypeTO>();
        }
        public List<PassTypeTO> getPassTypesForCompany(int company, bool isAlternativeLang)
        {
            return new List<PassTypeTO>();
        }
        public List<PassTypeTO> getPassTypesMassiveInputForCompany(int company, string ptIDs, bool isAlternativeLang)
        {
            return new List<PassTypeTO>();
        }
        public List<PassTypeTO> getConformationTypes(int ptID, string ptIDs, bool isAltLang)
        {
            return null;
        }
        public List<string> getPassTypesDistinctField(string field)
        {
            return null;
        }
        public bool beginTransaction()
        {
            return false;
        }

        public void commitTransaction()
        {
        }

        public void rollbackTransaction()
        {
        }

        public void setTransaction(IDbTransaction trans)
        {
        }

        public IDbTransaction getTransaction()
        {
            return null;
        }
        // TODO: payment_code added to table pass_types!!!!!
        public List<PassTypeTO> getPassTypes(PassTypeTO ptTO)
        {
            List<PassTypeTO> passTypesTOList = new List<PassTypeTO>();

            List<int> arePass = new List<int>();
            arePass.Add(ptTO.IsPass);

            passTypesTOList = this.getPassTypes(ptTO, arePass);

            return passTypesTOList;
        }

        // TODO: payment_code added to table pass_types!!!!!
        public List<PassTypeTO> getPassTypes(PassTypeTO ptTO, List<int> isPass)
        {
            DataSet allPassTypes = getCachedPassTypes();
            DataTable table = new DataTable();
            DataTable result = new DataTable();
            List<PassTypeTO> passTypeTOList = new List<PassTypeTO>();
            StringBuilder sb = new StringBuilder();

            string select = "";

            try
            {
                if ((allPassTypes.Tables.Contains(tableName)) &&
                    ((table = allPassTypes.Tables[tableName]).Rows.Count > 0))
                {
                    result = table.Clone();

                    if ((ptTO.PassTypeID != -1) || (!ptTO.Description.Trim().Equals("")) ||
                        (ptTO.Button != -1) || ((isPass.Count > 0) && (isPass[0] != -1)))
                    {
                        if (ptTO.PassTypeID != -1)
                        {
                            sb.Append(" PassTypeID = '" + ptTO.PassTypeID.ToString().Trim() + "' and");
                        }
                        if (!ptTO.Description.Trim().Equals(""))
                        {
                            sb.Append(" Description like '%" + ptTO.Description.ToUpper().Trim() + "%' and");
                        }
                        if (ptTO.Button != -1)
                        {
                            sb.Append(" Button = '" + ptTO.Button.ToString().Trim() + "' and");
                        }

                        if ((isPass.Count > 0) && (isPass[0] != -1))
                        {
                            sb.Append(" ( ");
                            foreach (int isPassMember in isPass)
                            {
                                if (isPassMember != -1)
                                {
                                    sb.Append(" IsPass = '" + isPassMember.ToString().Trim() + "' or");
                                }
                            }

                            string temp = sb.ToString(0, sb.ToString().Length - 2);
                            sb.Remove(0, sb.Length);
                            sb.Append(temp);

                            sb.Append(") and");
                        }

                        select = sb.ToString(0, sb.ToString().Length - 3);

                        DataRow[] resultRows = table.Select(select);

                        foreach (DataRow row in resultRows)
                        {
                            result.ImportRow(row);
                        }
                    }
                    else
                    {
                        result = table;
                    }

                    passTypeTOList = dataTable2ArrayList(result);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return passTypeTOList;
        }


        public Dictionary<int,PassTypeTO> findByPaymentCode(string paymentCode, int company)
        {
            DataSet allPassTypes = getCachedPassTypes();
            DataTable table = new DataTable();
            DataTable result = new DataTable();
            Dictionary<int, PassTypeTO> passTypeTOList = new Dictionary<int, PassTypeTO>();
            StringBuilder sb = new StringBuilder();

            string select = "";

            try
            {
                if ((allPassTypes.Tables.Contains(tableName)) &&
                    ((table = allPassTypes.Tables[tableName]).Rows.Count > 0))
                {
                    result = table.Clone();
                    if (!paymentCode.Equals("") || company != -1)
                    {
                        if (!paymentCode.Equals(""))
                        {

                            string[] payment = paymentCode.Split(',');

                            string payment_code = "";
                            foreach (string p in payment)
                            {
                                payment_code += "'" + p + "'" + ",";
                            }
                            if (payment_code.Length > 0)
                            {
                                payment_code = payment_code.Remove(payment_code.LastIndexOf(","));
                            }

                            sb.Append(" payment_code in (" + payment_code + ") AND");
                        }
                        if (company != -1)
                        {
                            sb.Append(" working_unit_id=" + company);
                        }
                        select = sb.ToString(0, sb.ToString().Length - 3);
                    }
                    else
                        select = sb.ToString();


                    DataRow[] resultRows = table.Select(select);

                    foreach (DataRow row in resultRows)
                    {
                        result.ImportRow(row);
                    }
                }
                else
                {
                    result = table;
                }

                passTypeTOList = dataTable2Dictionary(result);

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return passTypeTOList;
        }

        private DataSet getCachedPassTypes()
        {
            if ((!cachedPassTypesTO.Tables.Contains(tableName)) ||
            ((DataTable)cachedPassTypesTO.Tables[tableName]).Rows.Count == 0)
            {
                deserializeToCache();
            }
            return cachedPassTypesTO;
        }

        public void serialize(List<PassTypeTO> PassTypesTOList)
        {
            try
            {
                //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLPassTypesFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLPassTypesFile;
                Stream stream = File.Open(filename, FileMode.Create);

                PassTypeTO[] passTypeTOArray = (PassTypeTO[])PassTypesTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(PassTO[]));
                bformatter.Serialize(stream, passTypeTOArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void deserializeToCache()
        {
            List<PassTypeTO> passTypeList = new List<PassTypeTO>();

            try
            {
                //if (File.Exists(Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLPassTypesFile"]))
                if (File.Exists(Constants.XMLDataSourceDir + Constants.XMLPassTypesFile))
                {
                    //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLPassTypesFile"];
                    string filename = Constants.XMLDataSourceDir + Constants.XMLPassTypesFile;
                    Stream stream = File.Open(filename, FileMode.Open);

                    XmlSerializer bformatter = new XmlSerializer(typeof(PassTypeTO[]));
                    PassTypeTO[] deserialized = (PassTypeTO[])bformatter.Deserialize(stream);
                    ArrayList passTypeArray = ArrayList.Adapter(deserialized);

                    foreach (PassTypeTO ptTO in passTypeArray)
                    {
                        passTypeList.Add(ptTO);
                    }

                    stream.Close();

                    cachedPassTypesTO = toDataSet(passTypeList);
                }
                else
                {
                    cachedPassTypesTO = new DataSet();
                    DataTable dataTable = new DataTable();
                    cachedPassTypesTO.Tables.Add(tableName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // TODO: payment_code added to table pass_types!!!!!
        private DataSet toDataSet(List<PassTypeTO> list)
        {
            DataSet dataset = new DataSet();

            try
            {
                DataTable dataTable = new DataTable();

                dataset.Tables.Add(tableName);
                dataTable = dataset.Tables[tableName];

                dataTable.Columns.Add("PassTypeID", typeof(string));
                dataTable.Columns.Add("Description", typeof(string));
                dataTable.Columns.Add("Button", typeof(string));
                dataTable.Columns.Add("IsPass", typeof(string));

                foreach (PassTypeTO pt in list)
                {
                    DataRow row = dataTable.NewRow();

                    row["PassTypeID"] = pt.PassTypeID.ToString();
                    row["Description"] = pt.Description;
                    row["Button"] = pt.Button.ToString();
                    row["IsPass"] = pt.IsPass.ToString();

                    dataTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataset;
        }
        private Dictionary<int,PassTypeTO> dataTable2Dictionary(DataTable dataTable)
        {
            Dictionary<int, PassTypeTO> passTypeTOList = new Dictionary<int, PassTypeTO>();

            try
            {
                PassTypeTO pt = new PassTypeTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    pt = new PassTypeTO();

                    pt.PassTypeID = Int32.Parse(row["PassTypeID"].ToString());
                    pt.Description = row["Description"].ToString();
                    pt.Button = Int32.Parse(row["Button"].ToString());
                    pt.IsPass = Int32.Parse(row["IsPass"].ToString());

                    passTypeTOList.Add(pt.PassTypeID,pt);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return passTypeTOList;
        }

        private List<PassTypeTO> dataTable2ArrayList(DataTable dataTable)
        {
            List<PassTypeTO> passTypeTOList = new List<PassTypeTO>();

            try
            {
                PassTypeTO pt = new PassTypeTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    pt = new PassTypeTO();

                    pt.PassTypeID = Int32.Parse(row["PassTypeID"].ToString());
                    pt.Description = row["Description"].ToString();
                    pt.Button = Int32.Parse(row["Button"].ToString());
                    pt.IsPass = Int32.Parse(row["IsPass"].ToString());

                    passTypeTOList.Add(pt);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return passTypeTOList;
        }
    }
}
