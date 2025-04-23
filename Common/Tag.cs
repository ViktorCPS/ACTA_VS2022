using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.IO;

using DataAccess;
using TransferObjects;
using Util;
using System.Collections.Generic;

namespace Common
{
	/// <summary>
	/// Summary description for Tag.
	/// </summary>
	public class Tag 
	{
		DAOFactory daoFactory = null;
		TagDAO tdao = null;

		DebugLog log;

        TagTO tgTO = new TagTO();

		public TagTO TgTO
		{
			get{ return tgTO; }
			set{ tgTO = value; }
		}
        
		public Tag()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			tdao = daoFactory.getTagDAO(null);
		}

        public Tag(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            tdao = daoFactory.getTagDAO(dbConnection);
        }

        public Tag(int recordID, uint tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			tdao = daoFactory.getTagDAO(null);

			this.TgTO.RecordID = recordID;
			this.TgTO.TagID = tagID;
			this.TgTO.OwnerID = ownerID;
			this.TgTO.Status = status;
			this.TgTO.Description = description;
            this.TgTO.Issued = issued;
            this.TgTO.ValidTO = validTO;
		}

        public Tag(int recordID, uint tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, int accessGroupID)
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			tdao = daoFactory.getTagDAO(null);

			this.TgTO.RecordID = recordID;
			this.TgTO.TagID = tagID;
			this.TgTO.OwnerID = ownerID;
			this.TgTO.Status = status;
			this.TgTO.Description = description;
			this.TgTO.AccessGroupID = accessGroupID;
            this.TgTO.Issued = issued;
            this.TgTO.ValidTO = validTO;
		}

		public int Save(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO)
		{
			int saved = 0;
			try
			{
				// TODO: cerate TO and send to DAO
				saved = tdao.insert(tagID, ownerID, status, description, issued, validTO);

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return saved;
		}

        public int SaveFromS(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy, bool doCommit)
        {
            int saved = 0;
            try
            {
                // TODO: cerate TO and send to DAO
                saved = tdao.insert(tagID, ownerID, status, description, issued, validTO, createdBy, doCommit);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public int SaveFromS(ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string createdBy)
        {
            int saved = 0;
            try
            {
                // TODO: cerate TO and send to DAO
                saved = tdao.insert(tagID, ownerID, status, description, issued, validTO, createdBy);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return saved;
        }

        public bool Update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, bool doCommit)
		{
			bool isUpdated = false;

			try
			{
				// TODO: cerate TO and send to DAO
				isUpdated = tdao.update(recordID, tagID, ownerID, status, description, issued, validTO, doCommit);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		public List<TagTO> Search()
		{
			try
			{
				return tdao.getTags(this.TgTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
     
		public Dictionary<ulong, TagTO> SearchActive()
		{
			try
			{
				return tdao.getActiveTags();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public Dictionary<int, List<TagTO>> SearchTagForEmployees()
        {
            try
            {
                Dictionary<int, List<TagTO>> tagsForEmpl = new Dictionary<int, List<TagTO>>();
                List<TagTO> tagTOList = tdao.getTags(new TagTO());

                foreach (TagTO tagMember in tagTOList)
                {
                    if (tagsForEmpl.ContainsKey(tagMember.OwnerID))
                    {
                        tagsForEmpl[tagMember.OwnerID].Add(tagMember);
                    }
                    else
                    {
                        List<TagTO> tags = new List<TagTO>();
                        tags.Add(tagMember);
                        tagsForEmpl.Add(tagMember.OwnerID, tags);
                    }
                }

                return tagsForEmpl;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public Dictionary<ulong, TagTO> SearchActiveWithAccessGroup()
		{
			try
			{
				return tdao.getActiveTagsWithAccessGroup();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.SearchActiveWithAccessGroup(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public List<TagTO> SearchInactiveTags(string wUnits, DateTime from, DateTime to)
		{
			try
			{
				return tdao.getInactiveTags(wUnits, from, to);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.SearchInactiveTags(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public bool Delete(int recordID)
		{
			bool isDeleted = false;

			try
			{
				isDeleted = tdao.delete(recordID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isDeleted;
		}

		public TagTO Find(int recordID)
		{
			TagTO tagTO = new TagTO();

			try
			{
				tagTO = tdao.find(recordID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return tagTO;
		}

		public TagTO FindActive(int ownerID)
		{
			TagTO tag = new TagTO();

			try
			{
				tag = tdao.findActive(ownerID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.FindActive(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return tag;
		}

        public bool BeginTransaction()
        {
            bool isStarted = false;

            try
            {
                isStarted = tdao.beginTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.BeginTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isStarted;
        }

        public void CommitTransaction()
        {
            try
            {
                tdao.commitTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.CommitTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                tdao.rollbackTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.RollbackTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public IDbTransaction GetTransaction()
        {
            try
            {
                return tdao.getTransaction();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.GetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public void SetTransaction(IDbTransaction trans)
        {
            try
            {
                tdao.setTransaction(trans);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.SetTransaction(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        
        public void Clear()
		{
			this.TgTO = new TagTO();
		}

		
		/// <summary>
		/// Send list of TagTO objects to serialization.
		/// </summary>
		/// <param name="tagTOList">List of TagTO</param>
		private void CacheData(List<TagTO> tagTOList)
		{
			try
			{
				tdao.serialize(tagTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/// <summary>
		/// Serialize TagTO objects to file.
		/// </summary>
		/// <param name="tagTOList">List of TagTO</param>
		public void Serialize(List<TagTO> tagTOList, string fileName)
		{
			try
			{
				tdao.serialize(tagTOList, fileName);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.Serialize(): " + ex.Message + "\n");
				throw ex;
			}
		}

        /// <summary>
        /// Serialize TagTO objects to file.
        /// </summary>
        /// <param name="tagTOList">List of TagTO</param>
        public void Serialize(List<TagTO> tagTOList, Stream stream)
        {
            try
            {
                tdao.serialize(tagTOList, stream);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.Serialize(): " + ex.Message + "\n");
                throw ex;
            }
        }
		
		public void CacheData()
		{
			List<TagTO> tagTOList = new List<TagTO>();

			try
			{
				tagTOList = tdao.getTags(this.TgTO);	
				this.CacheData(tagTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}		
		
		/// <summary>
		/// Cache all of Tags from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(tdao.getTags(new TagTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " tag.CacheAllData(): " + ex.Message + "\n");
				throw ex;
			}
		}

		/// <summary>
		/// Change DAO, start to use XML data source
		/// </summary>
		public void SwitchToXMLDAO()
		{
			try
			{
                daoFactory = DAOFactory.getDAOFactory(Constants.XMLDataProvider);
				tdao = daoFactory.getTagDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Tag.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

		public List<TagTO> GetFromXMLSource(string filePath)
		{
            try
            {
                return tdao.deserialize(filePath);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Log.GetFromFile(): " + ex.Message + "\n");
                throw ex;
            }
		}

        public List<TagTO> GetFromXMLSource(Stream stream)
        {
            try
            {
                return tdao.deserialize(stream);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.GetFromXMLSource(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public bool Update(int recordID, ulong tagID, int ownerID, string status, string description, DateTime issued, DateTime validTO, string user, bool doCommit)
        {
            bool isUpdated = false;

            try
            {
                // TODO: cerate TO and send to DAO
                isUpdated = tdao.update(recordID, tagID, ownerID, status, description, issued, validTO, user, doCommit);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }


        public int SearchTagsCount(int emplID, string status, string wUnits, DateTime from, DateTime to, string tagID)
        {
            int numOfTags = 0;
            try
            {
                numOfTags = tdao.searchTagsCount(emplID, status,wUnits, from, to, tagID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return numOfTags;
        }

        public List<TagTO> SearchTags(int emplID, string status, string wuString, DateTime from, DateTime to, string tagID)
        {
            try
            {
                return tdao.searchTags(emplID, status, wuString, from, to, tagID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Tag.SearchInactiveTags(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
    }
}
