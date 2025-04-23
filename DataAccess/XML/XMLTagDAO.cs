using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using TransferObjects;
using Util;

namespace DataAccess
{
    /// <summary>
    /// DAO implementation for managing Tags data form XML files, 
    /// using serialization/deserialization.
    /// </summary>
    public class XMLTagDAO : TagDAO
    {
        private static DataSet cachedTagsTO = new DataSet();
        private static string tableName = "AllTags";
        private static string resultTableName = "resultRows";
        protected string dateTimeformat = "";

        private DataSet getCachedTags()
        {
            try
            {
                if ((!cachedTagsTO.Tables.Contains(tableName)) ||
                    (cachedTagsTO.Tables[tableName].Rows.Count == 0))
                {
                    deserializeToCache();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cachedTagsTO;
        }

        public XMLTagDAO()
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        #region TagDAO Members

        public void SetDBConnection(Object dbConnection)
        {
        }

        public int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO)
        {
            // TODO:  Add XMLTagDAO.insert implementation
            return 0;
        }


        public int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy)
        {
            // TODO:  Add XMLTagDAO.insert implementation
            return 0;
        }

        public int insert(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy, bool doCommit)
        {
            // TODO:  Add XMLTagDAO.insert implementation
            return 0;
        }

        public bool delete(int recordID)
        {
            // TODO:  Add XMLTagDAO.delete implementation
            return false;
        }

        public bool update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTo, bool doCommit)
        {
            // TODO:  Add XMLTagDAO.update implementation
            return false;
        }

        public bool update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTo, string user, bool doCommit)
        {
            // TODO:  Add XMLTagDAO.update implementation
            return false;
        }

        public TagTO find(int recordID)
        {
            // TODO:  Add XMLTagDAO.find implementation
            return null;
        }

        public List<TagTO> searchTags(int emplID, string status, string wuString, DateTime from, DateTime to, string tagID)
        {
            return null;
        }

        public List<TagTO> getTags(TagTO tag)
        {
            DataSet dataSet = getCachedTags();
            DataTable dataTable = dataSet.Tables[tableName];
            List<TagTO> resultList = new List<TagTO>();
            string select = "";
            StringBuilder sb = new StringBuilder();
            DataTable resultTable = new DataTable(resultTableName);
            resultTable = dataTable.Clone();

            try
            {
                if (dataSet.Tables.Contains(tableName))
                {
                    if ((tag.RecordID != -1) || (tag.TagID != 0) ||
                        (tag.OwnerID != -1) || (!tag.Status.Trim().Equals("")) ||
                        (!tag.Description.Trim().Equals("")))
                    {
                        if (tag.RecordID != -1)
                        {
                            sb.Append(" RecordID like '" + tag.RecordID.ToString().Trim() + "' and");
                        }
                        if (tag.TagID != 0)
                        {
                            sb.Append(" TagID like '" + tag.TagID.ToString().Trim() + "' and");
                        }
                        if (tag.OwnerID != -1)
                        {
                            sb.Append(" OwnerID like '" + tag.OwnerID.ToString().Trim() + "' and");
                        }
                        if (!tag.Status.Trim().Equals(""))
                        {
                            sb.Append(" Status like '" + tag.Status.Trim() + "' and");
                        }
                        if (!tag.Description.Trim().Equals(""))
                        {
                            sb.Append(" Description like '" + tag.Description.Trim() + "' and");
                        }

                        select = sb.ToString(0, sb.ToString().Length - 3);
                        dataTable.CaseSensitive = false;
                        DataRow[] resultRows = dataTable.Select(select);

                        foreach (DataRow row in resultRows)
                        {
                            resultTable.ImportRow(row);
                        }
                    }
                    else
                    {
                        resultTable = dataTable;
                    }

                    resultList = dataTable2ArrayList(resultTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultList;
        }


        public Dictionary<ulong, TagTO> getActiveTags()
        {
            // TODO:  Add XMLTagDAO.getActiveTags implementation
            return null;
        }
        public int searchTagsCount(int emplID, string status, string wUnits, DateTime from, DateTime to, string tagID)
        {
            // TODO:  Add XMLTagDAO.getActiveTags implementation
            return -1;
        }

        public Dictionary<ulong, TagTO> getActiveTagsWithAccessGroup()
        {
            // TODO:  Add XMLTagDAO.getActiveTags implementation
            return null;
        }

        // TODO
        public List<TagTO> getInactiveTags(string wUnits, DateTime from, DateTime to)
        {
            return new List<TagTO>();
        }

        public TagTO findActive(int ownerID)
        {
            // TODO:  Add XMLTagDAO.findActive implementation
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

        public IDbTransaction getTransaction()
        {
            return null;
        }

        public void setTransaction(IDbTransaction trans)
        {
        }

        public void serialize(List<TagTO> TagsTO)
        {
            try
            {
                //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLTagsFile"];
                string filename = Constants.XMLDataSourceDir + Constants.XMLTagsFile;
                this.serialize(TagsTO, filename);
                /*
                Stream stream = File.Open(filename, FileMode.Create);

                TagTO[] TagArray = (TagTO[]) TagsTO.ToArray(typeof(TagTO));

                XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                bformatter.Serialize(stream, TagArray);
                stream.Close();
                */
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void serialize(List<TagTO> TagTOList, String filename)
        {
            try
            {
                Stream stream = File.Open(filename, FileMode.Create);
                TagTO[] TagArray = (TagTO[])TagTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                bformatter.Serialize(stream, TagArray);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void serialize(List<TagTO> TagTOList, Stream stream)
        {
            try
            {
                TagTO[] TagArray = (TagTO[])TagTOList.ToArray();

                XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                bformatter.Serialize(stream, TagArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void deserializeToCache()
        {
            try
            {
                //if (File.Exists(Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLTagsFile"]))
                if (File.Exists(Constants.XMLDataSourceDir + Constants.XMLTagsFile))
                {
                    //string filename = Constants.XMLDataSourceDir + ConfigurationManager.AppSettings["XMLTagsFile"];
                    string filename = Constants.XMLDataSourceDir + Constants.XMLTagsFile;
                    Stream stream = File.Open(filename, FileMode.Open);

                    XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                    TagTO[] deserialized = (TagTO[])bformatter.Deserialize(stream);
                    ArrayList tagsTOList = ArrayList.Adapter(deserialized);
                    List<TagTO> tags = new List<TagTO>();
                    foreach (TagTO tag in tagsTOList)
                    {
                        tags.Add(tag);
                    }
                    stream.Close();

                    cachedTagsTO = toDataSet(tags);
                }
                else
                {
                    cachedTagsTO = new DataSet();
                    DataTable dataTable = new DataTable();
                    cachedTagsTO.Tables.Add(tableName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet toDataSet(List<TagTO> list)
        {
            DataSet dataset = new DataSet();

            try
            {
                DataTable dataTable = new DataTable();

                dataset.Tables.Add(tableName);
                dataTable = dataset.Tables[tableName];

                dataTable.Columns.Add("RecordID", typeof(string));
                dataTable.Columns.Add("TagID", typeof(string));
                dataTable.Columns.Add("OwnerID", typeof(string));
                dataTable.Columns.Add("Issued", typeof(string));
                dataTable.Columns.Add("ValidTO", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("Description", typeof(string));

                foreach (TagTO tTO in list)
                {
                    DataRow row = dataTable.NewRow();

                    row["RecordID"] = tTO.RecordID.ToString();
                    row["TagID"] = tTO.TagID.ToString();
                    row["OwnerID"] = tTO.OwnerID.ToString();
                    row["Issued"] = tTO.Issued.ToString(dateTimeformat).Trim();
                    row["ValidTO"] = tTO.ValidTO.ToString(dateTimeformat).Trim();
                    row["Status"] = tTO.Status;
                    row["Description"] = tTO.Description;

                    dataTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataset;
        }

        private List<TagTO> dataTable2ArrayList(DataTable dataTable)
        {

            List<TagTO> tagList = new List<TagTO>();

            try
            {
                TagTO tTO = new TagTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    tTO = new TagTO();

                    tTO.RecordID = Int32.Parse(row["RecordID"].ToString());
                    tTO.TagID = UInt32.Parse(row["TagID"].ToString());
                    tTO.OwnerID = Int32.Parse(row["OwnerID"].ToString());
                    tTO.Issued = Convert.ToDateTime(row["Issued"].ToString());

                    tTO.ValidTO = Convert.ToDateTime(row["ValidTO"]);
                    tTO.Status = row["Status"].ToString();
                    tTO.Description = row["Description"].ToString();

                    tagList.Add(tTO);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tagList;
        }

        public List<TagTO> deserialize(string filePath)
        {
            List<TagTO> tagList = new List<TagTO>();

            try
            {
                if (File.Exists(filePath))
                {
                    Stream stream = File.Open(filePath, FileMode.Open);
                    XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                    TagTO[] deserialized;

                    try
                    {
                        deserialized = (TagTO[])bformatter.Deserialize(stream);
                        ArrayList tags = ArrayList.Adapter(deserialized);

                        foreach (TagTO tag in tags)
                        {
                            tagList.Add(tag);
                        }
                    }
                    catch (Exception ex)
                    {
                        stream.Close();
                        throw new DataProcessingException("File: " + filePath + " " + ex.Message + "\n", 3);
                    }

                    stream.Close();
                }
            }
            catch (IOException ioEx)
            {
                throw new DataProcessingException(ioEx + " File: " + filePath, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tagList;
        }

        public List<TagTO> deserialize(Stream stream)
        {
            List<TagTO> tagList = new List<TagTO>();

            try
            {
                XmlSerializer bformatter = new XmlSerializer(typeof(TagTO[]));
                TagTO[] deserialized;

                try
                {
                    deserialized = (TagTO[])bformatter.Deserialize(stream);
                    ArrayList tags = ArrayList.Adapter(deserialized);

                    foreach (TagTO tag in tags)
                    {
                        tagList.Add(tag);
                    }
                }
                catch (Exception ex)
                {
                    stream.Close();
                    throw new DataProcessingException(ex.Message + "\n", 3);
                }
            }
            catch (IOException ioEx)
            {
                throw new DataProcessingException(ioEx.Message, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tagList;
        }
    }
}
