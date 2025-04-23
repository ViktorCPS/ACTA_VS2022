using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Serialization;
using DataAccess;
using TransferObjects;
using System.ComponentModel;
using System.Data;
using Util;

namespace Common
{	
	/// <summary>
	/// Summary description for Location.
	/// </summary>
	[Serializable()]
	public class Location : ISerializable
	{
        LocationTO locTO = new LocationTO();

		DAOFactory daoFactory = null;
		LocationDAO locationDAO = null;

		DebugLog log;
		
		public LocationTO LocTO
		{
			get{ return locTO; }
            set { locTO = value; }
		}
        
		public Location()
		{
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			daoFactory = DAOFactory.getDAOFactory();
			locationDAO = daoFactory.getLocationDAO(null);
		}

        public Location(object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            locationDAO = daoFactory.getLocationDAO(dbConnection);
        }

        public Location(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                locationDAO = daoFactory.getLocationDAO(null);
            }
        }

		public Location(SerializationInfo info, StreamingContext ctxt)
		{
			//Get the values from info and assign them to the appropriate properties
			this.LocTO.LocationID = (int)info.GetValue("LocationID", typeof(int));
			this.LocTO.Name = (String)info.GetValue("Name", typeof(string));
			this.LocTO.Description = (String)info.GetValue("Description", typeof(string));
			this.LocTO.ParentLocationID = (int)info.GetValue("ParentLocationID", typeof(int));
			this.LocTO.AddressID = (int)info.GetValue("AddressID", typeof(int));
			this.LocTO.Status = (String)info.GetValue("Status", typeof(string));			
		}
        
		//Serialization function.
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			//You can use any custom name for your name-value pair. But make sure you
			// read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
			// then you should read the same with "EmployeeId"
			info.AddValue("LocationID", this.LocTO.LocationID);
			info.AddValue("Name", this.LocTO.Name);
			info.AddValue("Description", this.LocTO.Description);
			info.AddValue("ParentLocationID", this.LocTO.ParentLocationID);			
			info.AddValue("AddressID", this.LocTO.AddressID);
			info.AddValue("Status", this.LocTO.Status);	
		}
	
		public int Save()
		{
			int inserted;
			try
			{
				inserted = locationDAO.insert(this.LocTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.Save(): " + ex.Message + "\n");
				throw ex;
			}

			return inserted;
		}


		public List<LocationTO> Search()
		{
			try
			{
                return locationDAO.getLocations(this.LocTO);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.Search(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<LocationTO> Search(bool createDAO)
        {
            try
            {
                return locationDAO.getLocations(this.LocTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Location.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, LocationTO> SearchDict()
        {
            try
            {
                return locationDAO.getLocationsDict(this.LocTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Location.SearchDict(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public bool Update()
		{
			bool isUpdated;

			try
			{
				isUpdated = locationDAO.update(this.LocTO);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.Update(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isUpdated;
		}

		/// <summary>
		/// Change status to RETIRED to the given Location 
		/// </summary>
		/// <param name="locationID"></param>
		/// <returns></returns>
		public bool Delete(int locationID)
		{
			try
			{
				LocationTO locTO = Find(locationID);
                locTO.Status = Constants.statusRetired;
                this.LocTO = locTO;
                return Update();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.Delete(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public LocationTO Find(int locationId)
		{
			try
			{
				return locationDAO.find(locationId);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.Find(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        List<LocationTO> tmpList = new List<LocationTO>();

        public List<LocationTO> FindAllChildren(List<LocationTO> locationIdList)
		{
            List<LocationTO> locationTOList = new List<LocationTO>();
			
			try
			{
				for (int i=0; i < locationIdList.Count; i++)
				{
					LocationTO locationMember = new LocationTO();
					tmpList.Add(locationIdList[i]);
                    locationMember.ParentLocationID = locationIdList[i].LocationID;
					locationTOList = locationDAO.getLocations(locationMember);

                    for (int j = 0; j < locationTOList.Count; j++)
                    {
                        if (locationTOList[j].LocationID == locationTOList[j].ParentLocationID)
                        {
                            locationTOList.RemoveAt(j);
                            j--;
                        }
                    }

                    if ( locationTOList.Count > 0 )
					{							  
						FindAllChildren(locationTOList);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.FindAllChildren(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			return tmpList;
		}

        public List<LocationTO> FindAllChildren(List<LocationTO> locationIdList, bool createDAO)
        {
            List<LocationTO> locationTOList = new List<LocationTO>();

            try
            {
                for (int i = 0; i < locationIdList.Count; i++)
                {
                    tmpList.Add(locationIdList[i]);
                    LocationTO loc = new LocationTO();
                    loc.ParentLocationID = locationIdList[i].LocationID;
                    locationTOList = locationDAO.getLocations(loc);

                    for (int j = 0; j < locationTOList.Count; j++)
                    {
                        if (locationTOList[j].LocationID == locationTOList[j].ParentLocationID)
                        {
                            locationTOList.RemoveAt(j);
                            j--;
                        }
                    }

                    if (locationTOList.Count > 0)
                    {
                        FindAllChildren(locationTOList, createDAO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Location.FindAllChildren(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            return tmpList;
        }

		public int FindMAXLocID()
		{
			try
			{
				return locationDAO.findMAXLocID();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.FindMAXLocID(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
        
        public DataSet getLocations(string locationID)
        {
            try
            {
                return locationDAO.getLocations(locationID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Location.getLocations(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public List<LocationTO> getLocationsForMap(int mapID)
        {
            try
            {
                return locationDAO.getLocationsForMap(mapID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Location.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

		public LocationTO GetParentLocation()
		{			
			try
			{
				return locationDAO.find(this.LocTO.ParentLocationID);				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.GetParentLocation(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        public List<LocationTO> SearchRootLocations()
        {            
            try
            {
                return locationDAO.getRootLocations();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Location.SearchRootLocations(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public List<LocationTO> SearchChildLocations(string parentID)
        {
            try
            {
                return locationDAO.getChildLocations(parentID);                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Location.SearchChildLocations(): " + ex.Message + "\n");
                throw ex;
            }
        }

		public void Clear()
		{
            this.LocTO = new LocationTO();
		}

		
		/// <summary>
		/// Send list of LocationTO objects to serialization.
		/// </summary>
		/// <param name="locatioTOList">List of LocationTO</param>
		private void CacheData(List<LocationTO> locatioTOList)
		{
			try
			{
				locationDAO.serialize(locatioTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		public void CacheData(string locationID, string name, string description, string parentLocationID, string addressID, string status)
		{
            List<LocationTO> locationTOList = new List<LocationTO>();

			try
			{
                LocationTO locTO = new LocationTO(int.Parse(locationID.Trim()), name, description, int.Parse(parentLocationID.Trim()), int.Parse(addressID.Trim()), status);
				locationTOList = locationDAO.getLocations(locTO);
				this.CacheData(locationTOList);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.CacheData(): " + ex.Message + "\n");
				throw ex;
			}
		}
		
		
		/// <summary>
		/// Cache all of the Locations from database
		/// </summary>
		public void CacheAllData()
		{
			try
			{
				this.CacheData(locationDAO.getLocations(new LocationTO()));
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Location.CacheAllData(): " + ex.Message + "\n");
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
				locationDAO = daoFactory.getLocationDAO(null);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " Loaction.SwitchToXMLDAO(): " + ex.Message + "\n");
				throw ex;
			}
		}

        Dictionary<LocationTO, int> allLocations = new Dictionary<LocationTO, int>(); //<location, location level>

        public Dictionary<LocationTO, int> SearchAllLocationsHierarchicly()
        {
            try
            {
                allLocations = new Dictionary<LocationTO, int>();
                List<LocationTO> rootLocations = SearchRootLocations();
                getChildLocations(rootLocations, 0);

                foreach (LocationTO loc in allLocations.Keys)
                {
                    loc.Name = loc.Name.PadLeft(loc.Name.Length + allLocations[loc] * 5, ' ');
                }

                return allLocations;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Loaction.SearchAllLocationsHierarchicly(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void getChildLocations(List<LocationTO> locationList, int level)
        {
            try
            {
                foreach (LocationTO loc in locationList)
                {
                    allLocations.Add(loc, level);
                    List<LocationTO> locChildren = SearchChildLocations(loc.LocationID.ToString().Trim());

                    if (locChildren.Count > 0)
                    {
                        getChildLocations(locChildren, level + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Loaction.getChildLocations(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
