using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;

using DataAccess;
using TransferObjects;
using Util;

namespace Common
{
	/// <summary>
	/// Manage coordinataion between data sources, XML and Database.
	/// </summary>
	public class DataManager
	{
		DAOFactory daoFactory = null;
		DebugLog debug;

		// Track tag's changes
		// Old Active Tags
		private Dictionary<ulong, TagTO> oldActiveTags = new Dictionary<ulong,TagTO>();
		// New Active Tags
		private Dictionary<ulong, TagTO> newActiveTags = new Dictionary<ulong,TagTO>();
		// File Sufix Format
		private static string SUFFIXFORMAT = "yyyyMMddHHmmss";
		private static string FILEEXT = ".xml";
		private static string FILEPREFIX = "Cards";
		private static string FILEPREFIXTIMEACCESS = "TimeAcessProfile";
		private static string UPLOADTIMEFLAG = "_L";
		private static string TEMPTIMEFLAG = "_T";

		public DataManager()
		{
			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);	

			daoFactory = DAOFactory.getDAOFactory();
		}

		/// <summary>
		/// If connection exists, push all of data from XML files found 
		/// in Update dir to database tables;
		/// </summary>
		public bool PushToDatabase()
		{
			bool areUpdated = false;

			try
			{
				string updateDirPath = Constants.XMLUpdateFilesDir;

				if (daoFactory.TestDataSourceConnection())
				{
					// Get all of files from Update Directory
					// Passes
					//string passesXMLUpdateFile = updateDirPath + ConfigurationManager.AppSettings["XMLPassesUpdateFile"];
                    string passesXMLUpdateFile = updateDirPath + Constants.XMLPassesUpdateFile;
					if (File.Exists(passesXMLUpdateFile))
					{
                        Pass pass = new Pass();
						List<PassTO> passesToUpdate = pass.GetFromXMLSource(passesXMLUpdateFile);
						int affectedRec = 0;

						foreach(PassTO passMember in passesToUpdate)
						{
                            pass.PssTO = passMember;
                            if (pass.Save() == 1)
							{
								affectedRec ++;
							}
						}

						if (passesToUpdate.Count == affectedRec)
						{
							try
							{
								File.Delete(passesXMLUpdateFile);
							}
							catch(Exception ex)
							{
								throw ex;
							}
						}
					}
					
					// Visits
					//string visitsXMLUpdateFile = updateDirPath + ConfigurationManager.AppSettings["XMLVisitsUpdateFile"];
                    string visitsXMLUpdateFile = updateDirPath + Constants.XMLVisitsUpdateFile;
					if (File.Exists(visitsXMLUpdateFile))
					{
						Visit visits = new Visit();
						
						ArrayList visitsToUpdate = visits.GetFromXMLSource(visitsXMLUpdateFile);
						int affectedRec = 0;	
					
						foreach(Visit visitMember in visitsToUpdate)
						{
							if (visitMember.Update(visitMember.sendTransferObject()))
							{
								affectedRec ++;
							}
							else if (visitMember.Save() == 1)
							{
								affectedRec ++;
							}
						}

						// Data Successfully uploaded so delete update file
						if (affectedRec == visitsToUpdate.Count)
						{
							try
							{
								File.Delete(visitsXMLUpdateFile);
							}
							catch(Exception ex)
							{
								throw ex;
							}
						}
					}

				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " DataManager.PushToDatabase(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return areUpdated;
		}

		public void GetAllFromDatabase()
		{
			try
			{
				if (daoFactory.TestDataSourceConnection())
				{
					// Export Data from Database table to XML 
					// files using serialization
					Employee employee = new Employee();
					employee.CacheAllData();

					Tag tag = new Tag();
					tag.CacheAllData();

					WorkingUnit wu = new WorkingUnit();
					wu.CacheAllData();

					Location location = new Location();
					location.CacheAllData();

					PassType passType = new PassType();
					passType.CacheAllData();

					//Visit visit = new Visit();
					//visit.CacheAllData();

					//Pass pass = new Pass();
					//pass.CacheAllData();

					Reader reader = new Reader();
					reader.CacheAllData();
				}
				
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " DataManager.GetAllFromDatabase(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void GetVisitsFromDatabase()
		{
			try
			{
				if (daoFactory.TestDataSourceConnection())
				{
					Visit visit = new Visit();
					visit.CacheAllData();
				}
				
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " DataManager.GetVisitsFromDatabase(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void StartTagsTracking()
		{
			try
			{
				if (daoFactory.TestDataSourceConnection())
				{
					oldActiveTags = new Tag().SearchActive();
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " DataManager.StartTagsTracking(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}
		
		public bool CheckNewTags(object conn)
		{
			bool findedNewTags = false;
			try
			{
                Tag tag;
                if (conn != null)
                    tag = new Tag(conn);
                else
                    tag = new Tag();

				newActiveTags = tag.SearchActive();
				if (!oldActiveTags.Count.Equals(newActiveTags.Count))
				{
					findedNewTags = true;
				}
				else
				{
					foreach(uint tagID in newActiveTags.Keys)
					{
						if (!oldActiveTags.ContainsKey(tagID))
						{
							findedNewTags = true;
							break;
						}
						else
						{ 
							TagTO oldValue = oldActiveTags[tagID];
							TagTO newValue = newActiveTags[tagID];
							if (oldValue.OwnerID != newValue.OwnerID)
							{
								findedNewTags = true;
								break;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " DataManager.CheckNewTags(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			
			return findedNewTags;
		}

        //doRename is true when comming from Employee form - rename temporary files into real ones
        //doRename is false when comming from TimeAccessProfile - don't rename temporary files, it 
        //         will be done in FinalizeTimeaccessprofilesTracking for both Cards and TAProfiles
		public void FinalizeTagsTracking(bool updateNow, bool doRename, IDbTransaction transaction, object conn)
		{
			//ArrayList newTags = new ArrayList();
            Tag tag;
            if (conn != null)
                tag = new Tag(conn);
            else
                tag = new Tag();

			List<ReaderTO> readerList = new List<ReaderTO>();
			string oldCardFile = "";

            AccessControlFile acf;
            if (conn != null)
                acf = new AccessControlFile(conn);
            else
                acf = new AccessControlFile();
                        
            bool trans = false;
            bool useDatabaseFiles = false;

			try
			{
                bool isInserted = true;

                if (transaction != null)
                {
                    acf.SetTransaction(transaction);
                }
                int databaseCount = acf.SearchCount("", "");
                if (databaseCount >= 0)
                    useDatabaseFiles = true;

				/*newActiveTags = tag.SearchActive();
				newTags = new ArrayList(newActiveTags.Values);*/
                if (transaction != null)
                {
                    tag.SetTransaction(transaction);
                }
				newActiveTags = tag.SearchActiveWithAccessGroup();
								
				// If there is a difference, serialize it
				// and crate file for every readers founded in database
				//if (newTags.Count > 0)
				if (newActiveTags.Values.Count > 0)
				{					
					Reader reader;
                    if (conn != null)
                        reader = new Reader(conn);
                    else
                        reader = new Reader();

                    if (transaction != null)
                    {
                        reader.SetTransaction(transaction);
                    }
					readerList = reader.Search();
					string filePath = "";

					int fileCount = 0;

                    if (!useDatabaseFiles)
                    {
                        if (!Directory.Exists(Constants.cards))
                        {
                            Directory.CreateDirectory(Constants.cards);
                        }
                    }
                    else
                    {
                        if (transaction == null)
                        {
                            trans = acf.BeginTransaction();
                        }
                        else
                        {
                            acf.SetTransaction(transaction);
                            trans = true;
                        }
                    }

					foreach(ReaderTO current in readerList)
					{
						List<TagTO> tagsForReader = new List<TagTO>();
						foreach (TagTO tagTO in newActiveTags.Values)
						{
							TagTO newTagTO = new TagTO(tagTO.RecordID, tagTO.TagID, tagTO.OwnerID,
								tagTO.Status, tagTO.Description, tagTO.AccessGroupID);
							tagsForReader.Add(newTagTO);
						}

                        AccessGroupXGate accessGroupXGate1;
                        if (conn != null)
                            accessGroupXGate1 = new AccessGroupXGate(conn);
                        else
                            accessGroupXGate1 = new AccessGroupXGate();

                        if (trans)
                        {
                            accessGroupXGate1.SetTransaction(acf.GetTransaction());
                        }
                        ArrayList accessGroupXGateList = accessGroupXGate1.Search("", current.A0GateID.ToString());

						foreach (TagTO tagTO in tagsForReader)
						{
							bool setDefault = true;
							foreach(AccessGroupXGate accessGroupXGate in accessGroupXGateList)
							{
								if (tagTO.AccessGroupID == accessGroupXGate.AccessGroupID)
								{
									tagTO.AccessGroupID = accessGroupXGate.ReaderAccessGroupOrdNum;
									setDefault = false;
									break;
								}
							}
							if (setDefault)
								tagTO.AccessGroupID = 0;
						}

						if (tagsForReader.Count > 0)
						{
                            if (!useDatabaseFiles)
                            {
                                // Check if temporery file already exists, if exists - remove it
                                //oldCardFile = NewCardsExists(current);
                                oldCardFile = NewFileExists(Constants.cards, current, true);
                                if (!oldCardFile.Equals(""))
                                {
                                    try
                                    {
                                        File.Delete(oldCardFile);
                                    }
                                    catch (Exception ioEx)
                                    {
                                        throw ioEx;
                                    }
                                }

                                // Generate file name
                                filePath = Constants.cards + FILEPREFIX + "_" + current.ReaderID + "_" + DateTime.Now.ToString(SUFFIXFORMAT);
                                /*if (updateNow)
                                {
                                    filePath = filePath + FILEEXT; 
                                }
                                else
                                {
                                    filePath = filePath + UPLOADTIMEFLAG + FILEEXT; 
                                }*/
                                filePath = filePath + TEMPTIMEFLAG + FILEEXT;

                                // Create XML file 
                                //tag.Serialize(newTags, filePath);
                                tag.Serialize(tagsForReader, filePath);

                                fileCount++;
                            }
                            else
                            {
                                if (trans)
                                {
                                    MemoryStream memStream = new MemoryStream();
                                    tag.Serialize(tagsForReader, memStream);

                                    // Set the position to the beginning of the stream.
                                    memStream.Seek(0, SeekOrigin.Begin);

                                    byte[] byteArray = new byte[memStream.Length];
                                    int count = memStream.Read(byteArray, 0, 20);
                                    // Read the remaining bytes, byte by byte.
                                    while (count < memStream.Length)
                                    {
                                        byteArray[count++] =
                                            Convert.ToByte(memStream.ReadByte());
                                    }

                                    memStream.Close();

                                    acf.Update(current.ReaderID, Constants.ACFilesTypeCards, Constants.ACFilesStatusUnused, Constants.ACFilesStatusOverwritten,
                                        new DateTime(0), new DateTime(0), -1, -1, "", false);

                                    int delay = (updateNow ? (int)Constants.ACFilesDelay.DontDelay : (int)Constants.ACFilesDelay.Delay);
                                    bool insertedForReader = acf.Save(Constants.ACFilesTypeCards, current.ReaderID, delay, Constants.ACFilesStatusUnused, new DateTime(0), new DateTime(0), byteArray, false);
                                    isInserted = insertedForReader && isInserted;

                                    fileCount++;
                                }
                            }
						}
					} //foreach(Reader current in readerList)

					if (fileCount == readerList.Count)
					{
                        if (!useDatabaseFiles)
                        {
                            if (doRename)
                                RenameTempFiles(Constants.cards, updateNow, "");
                        }
                        else if (trans)
                        {
                            if (isInserted)
                            {
                                if (transaction == null)
                                {
                                    acf.CommitTransaction();
                                }
                            }
                            else
                            {
                                acf.RollbackTransaction();
                            }
                        }
					}
					else
					{
                        if (!useDatabaseFiles)
                        {
                            deleteAllTempFiles(Constants.cards);
                        }
                        else if (trans)
                        {
                            acf.RollbackTransaction();
                        }
					}                    
				} //if (newActiveTags.Values.Count > 0)
			}
			catch(Exception ex)
			{
                if (!useDatabaseFiles)
                {
                    deleteAllTempFiles(Constants.cards);
                }
                else if (trans)
                {
                    try
                    {
                        acf.RollbackTransaction();
                    }
                    catch
                    { }
                }
                debug.writeLog(DateTime.Now + " DataManager.FinalizeTagsTracking(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
			}
		}

		/*public string NewCardsExists(Reader reader)
		{
			string newFile = "";
			string[] paths;

			try
			{
				// If file exists in Cards directory
				if (Directory.Exists(Constants.cards))
				{
					paths = Directory.GetFiles(Constants.cards);

					if (paths.Length > 0)
					{
						foreach(string currentFile in paths)
						{
							// Checking if files for current reader exists
							if (currentFile.IndexOf ("_" + reader.ReaderID + "_").Equals(
								currentFile.LastIndexOf("_" + reader.ReaderID + "_"))
								&& (currentFile.IndexOf ("_" + reader.ReaderID + "_") > 0)
								&& (currentFile.LastIndexOf ("_" + reader.ReaderID + "_") > 0))
							{
								newFile = currentFile;
								break;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + ".NewCardsExists() : " + ex.StackTrace + "\n");
				throw new Exception(ex.Message);
			}

			return newFile;
		}*/

		// Test database connection.
		public bool TestDataBaseConnection()
		{
			DAOFactory daoFactory = DAOFactory.getDAOFactory();
			return daoFactory.TestDataSourceConnection();
		}

		// Close database connection.
		public static void CloseDataBaseConnection()
		{
			DAOFactory daoFactory = DAOFactory.getDAOFactory();
			daoFactory.CloseConnection();
		}

		public void FinalizeTimeaccessprofilesTracking(bool updateNow)
		{
			ArrayList newTimeAccessProfiles = new ArrayList();
			TimeAccessProfileDtl timeAccessProfileDtl = new TimeAccessProfileDtl();
			List<ReaderTO> readerList = new List<ReaderTO>();
			string oldFile = "";

            AccessControlFile acf = new AccessControlFile();
            bool trans = false;
            bool useDatabaseFiles = false;

			try
			{				
				// If there is a difference, serialize it
				// and crate file for every reader founded in database
				Reader reader = new Reader();
				readerList = reader.Search();
				string filePath = "";

                int fileCount = 0;

                bool isInserted = true;

                int databaseCount = acf.SearchCount("", "");
                if (databaseCount >= 0)
                    useDatabaseFiles = true;

                if (!useDatabaseFiles)
                {
                    if (!Directory.Exists(Constants.timeaccessprofiles))
                    {
                        Directory.CreateDirectory(Constants.timeaccessprofiles);
                    }
                }
                else
                {
                    trans = acf.BeginTransaction();
                }

				foreach(ReaderTO current in readerList)
				{					
					newTimeAccessProfiles.Clear();
                    Gate gate = new Gate();
                    if (trans)
                    {
                        gate.SetTransaction(acf.GetTransaction());
                    }
                    GateTO gateTO = gate.FindGetAccessProfile(current.A0GateID.ToString());
					if (gateTO.GateID != -1)
					{
						TransferObjects.GateTimeAccessProfileTO gtapTO = new TransferObjects.GateTimeAccessProfileTO();
                        GateTimeAccessProfile gateTimeAccessProfile = new GateTimeAccessProfile();
                        if (trans)
                        {
                            gateTimeAccessProfile.SetTransaction(acf.GetTransaction());
                        }
                        gtapTO = gateTimeAccessProfile.Find(gateTO.GateTimeaccessProfileID.ToString());
						if (gtapTO.GateTAProfileId != -1)
						{
							for (int i = 0; i < 16; i++)
							{								
								int profileID = -1;
								switch (i.ToString())
								{
									case "0":							
										profileID = gtapTO.GateTAProfile0;
										break;
									case "1":							
										profileID = gtapTO.GateTAProfile1;
										break;
									case "2":
										profileID = gtapTO.GateTAProfile2;
										break;
									case "3":							
										profileID = gtapTO.GateTAProfile3;
										break;
									case "4":
										profileID = gtapTO.GateTAProfile4;
										break;
									case "5":					
										profileID = gtapTO.GateTAProfile5;
										break;
									case "6":							
										profileID = gtapTO.GateTAProfile6;
										break;
									case "7":							
										profileID = gtapTO.GateTAProfile7;
										break;
									case "8":							
										profileID = gtapTO.GateTAProfile8;
										break;
									case "9":							
										profileID = gtapTO.GateTAProfile9;
										break;
									case "10":							
										profileID = gtapTO.GateTAProfile10;
										break;
									case "11":							
										profileID = gtapTO.GateTAProfile11;
										break;
									case "12":							
										profileID = gtapTO.GateTAProfile12;
										break;
									case "13":							
										profileID = gtapTO.GateTAProfile13;
										break;
									case "14":							
										profileID = gtapTO.GateTAProfile14;
										break;
									case "15":							
										profileID = gtapTO.GateTAProfile15;
										break;
								}
								if (profileID != -1)
								{
                                    if (trans)
                                    {
                                        timeAccessProfileDtl.SetTransaction(acf.GetTransaction());
                                    }
									ArrayList tapDtlTOList = timeAccessProfileDtl.Search(profileID.ToString());

									foreach(TimeAccessProfileDtlTO tapDtlTO in tapDtlTOList)
									{
										tapDtlTO.TimeAccessProfileId = i;
										newTimeAccessProfiles.Add(tapDtlTO);
									}																									
								}
							} //for (int i = 0; i < 16; i++)
						} //if (gtapTO.GateTAProfileId != -1)
					} //if (gateTO.GateID != -1)

					if (newTimeAccessProfiles.Count > 0)
					{
                        if (!useDatabaseFiles)
                        {
                            // Check if temporary file already exists, if exists - remove it
                            //oldFile = NewFileExists(Constants.timeaccessprofiles, current, false);
                            oldFile = NewFileExists(Constants.timeaccessprofiles, current, true);
                            if (!oldFile.Equals(""))
                            {
                                try
                                {
                                    File.Delete(oldFile);
                                }
                                catch (Exception ioEx)
                                {
                                    throw ioEx;
                                }
                            }

                            // Generate file name
                            filePath = Constants.timeaccessprofiles + FILEPREFIXTIMEACCESS + "_" + current.ReaderID + "_" + DateTime.Now.ToString(SUFFIXFORMAT);
                            /*if (updateNow)
                            {
                                filePath = filePath + FILEEXT; 
                            }
                            else
                            {
                                filePath = filePath + UPLOADTIMEFLAG + FILEEXT; 
                            }*/
                            filePath = filePath + TEMPTIMEFLAG + FILEEXT;

                            // Create XML file 
                            timeAccessProfileDtl.Serialize(newTimeAccessProfiles, filePath);

                            fileCount++;
                        }
                        else
                        {
                            if (trans)
                            {
                                MemoryStream memStream = new MemoryStream();
                                timeAccessProfileDtl.Serialize(newTimeAccessProfiles, memStream);

                                // Set the position to the beginning of the stream.
                                memStream.Seek(0, SeekOrigin.Begin);

                                byte[] byteArray = new byte[memStream.Length];
                                int count = memStream.Read(byteArray, 0, 20);
                                // Read the remaining bytes, byte by byte.
                                while (count < memStream.Length)
                                {
                                    byteArray[count++] =
                                        Convert.ToByte(memStream.ReadByte());
                                }

                                memStream.Close();

                                acf.Update(current.ReaderID, Constants.ACFilesTypeTAProfile, Constants.ACFilesStatusUnused, Constants.ACFilesStatusOverwritten,
                                    new DateTime(0), new DateTime(0), -1, -1, "", false);

                                int delay = (updateNow ? (int)Constants.ACFilesDelay.DontDelay : (int)Constants.ACFilesDelay.Delay);
                                bool insertedForReader = acf.Save(Constants.ACFilesTypeTAProfile, current.ReaderID, delay, Constants.ACFilesStatusUnused, new DateTime(0), new DateTime(0), byteArray, false);
                                isInserted = insertedForReader && isInserted;

                                fileCount++;
                            }
                        }
					}
				} //foreach(Reader current in readerList)

				if (fileCount == readerList.Count)
				{
                    if (!useDatabaseFiles)
                    {
                        //doRename in FinalizeTagsTracking is false, don't rename temporary files for Cards, it 
                        //         will be done here, in RenameTempFiles for both Cards and TAProfiles
                        FinalizeTagsTracking(updateNow, false, null, null);
                        RenameTempFiles(Constants.timeaccessprofiles, updateNow, Constants.cards);
                    }
                    else if (trans)
                    {
                        if (isInserted)
                        {
                            FinalizeTagsTracking(updateNow, false, acf.GetTransaction(), null);
                            try
                            {
                                //maybe rollback happened in FinalizeTagsTracking
                                acf.CommitTransaction();
                            }
                            catch
                            {}
                        }
                        else
                        {
                            acf.RollbackTransaction();
                        }
                    }
				}
				else
				{
                    if (!useDatabaseFiles)
                    {
                        deleteAllTempFiles(Constants.timeaccessprofiles);
                    }
                    else if (trans)
                    {
                        acf.RollbackTransaction();
                    }
				}
			}
			catch(Exception ex)
			{
                if (!useDatabaseFiles)
                {
                    deleteAllTempFiles(Constants.timeaccessprofiles);
                }
                else if (trans)
                {
                    try
                    {
                        acf.RollbackTransaction();
                    }
                    catch
                    {}
                }
				debug.writeLog(DateTime.Now + " DataManager.FinalizeTimeaccessprofilesTracking(): " + ex.Message + "\n");				
				throw new Exception(ex.Message);
			}
		}

		public string NewFileExists(string directory, ReaderTO reader, bool temporary)
		{
			string newFile = "";
			string[] paths;

			try
			{
				// If file exists in directory
				if (Directory.Exists(directory))
				{
					paths = Directory.GetFiles(directory);

					if (paths.Length > 0)
					{
						foreach(string currentFile in paths)
						{
							if (temporary)
							{
								// Checking if temporary files for current reader exists
								if ((currentFile.IndexOf ("_" + reader.ReaderID + "_") > 0)
									&& (currentFile.LastIndexOf ("_" + reader.ReaderID + "_") > 0)
									&& (currentFile.LastIndexOf(TEMPTIMEFLAG) >= 0)
									&& currentFile.IndexOf ("_" + reader.ReaderID + "_").Equals(
									currentFile.LastIndexOf("_" + reader.ReaderID + "_")))
								{
									newFile = currentFile;
									break;
								}
							}
							else
							{
								// Checking if nontemporary files for current reader exists
								if ((currentFile.IndexOf ("_" + reader.ReaderID + "_") > 0)
									&& (currentFile.LastIndexOf ("_" + reader.ReaderID + "_") > 0)
									&& (currentFile.LastIndexOf(TEMPTIMEFLAG) < 0)
									&& currentFile.IndexOf ("_" + reader.ReaderID + "_").Equals(
									currentFile.LastIndexOf("_" + reader.ReaderID + "_")))
								{
									newFile = currentFile;
									break;
								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + "DataManager.NewFileExists() : " + ex.StackTrace + "\n");
				throw new Exception(ex.Message);
			}

			return newFile;
		}

		public void RenameTempFiles(string directory, bool updateNow, string additionalDir)
		{
			List<ReaderTO> readerList = new List<ReaderTO>();
			string oldFile = "";

			try
			{				
				Reader reader = new Reader();
				readerList = reader.Search();
				int dirNum = 1;
				if (additionalDir != "")
					dirNum = 2;

				foreach(ReaderTO current in readerList)
				{							
					// Check if nontemporary file already exists, if exists - remove it					
					for (int i = 0; i < dirNum; i++)
					{
						if (i == 0)
							oldFile = NewFileExists(directory, current, false);
						else
							oldFile = NewFileExists(additionalDir, current, false);
						if (!oldFile.Equals(""))
						{
							try
							{
								File.Delete(oldFile);
							}
							catch(Exception ioEx)
							{
								throw ioEx;
							}
						}
					}
								
					//find temporary file, if exists - rename it to nontemporary file
					for (int i = 0; i < dirNum; i++)
					{
						if (i == 0)
							oldFile = NewFileExists(directory, current, true);
						else
							oldFile = NewFileExists(additionalDir, current, true);
						if (!oldFile.Equals(""))
						{
							string newFile = oldFile;
							// Generate file name
							if (updateNow)
							{
								int lastIndex = newFile.LastIndexOf(TEMPTIMEFLAG);
								if (lastIndex >= 0)
									newFile = newFile.Remove(lastIndex, TEMPTIMEFLAG.Length);
							}
							else
							{
								int lastIndex = newFile.LastIndexOf(TEMPTIMEFLAG);
								if (lastIndex >= 0)
								{
									newFile = newFile.Remove(lastIndex, TEMPTIMEFLAG.Length);
									newFile = newFile.Insert(lastIndex, UPLOADTIMEFLAG);
								}
							}
							File.Move(oldFile, newFile);
						}
					}
				} //foreach(Reader current in readerList)
			}
			catch(Exception ex)
			{
				if (additionalDir != "")
					deleteAllTempFiles(additionalDir);
				debug.writeLog(DateTime.Now + " DataManager.RenameTempFiles(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void deleteAllTempFiles(string directory)
		{
			string[] paths;

			try
			{
				// If file exists in directory
				if (Directory.Exists(directory))
				{
					paths = Directory.GetFiles(directory);

					if (paths.Length > 0)
					{
						foreach(string currentFile in paths)
						{	
							//delete only temporary files
							if (currentFile.LastIndexOf(TEMPTIMEFLAG) >= 0)
							{
								File.Delete(currentFile);
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " Exception in: " + 
					this.ToString() + "DataManager.deleteAllTempFiles() : " + ex.StackTrace + "\n");
				throw new Exception(ex.Message);
			}
		}

        public void pushCameraSnapshotsIntoDatabase()
        {
            try
            {
                string directoryPath = Constants.snapshots;
                if (Directory.Exists(directoryPath))
                {
                    CameraSnapshotFile currentCameraSnapshotFile = new CameraSnapshotFile();

                    //07.08.2009 Natasa 
                    //From now data processing will search pictures in all folders under snapshots folder 
                    string[] photosDirFileList = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                    int databaseCount = currentCameraSnapshotFile.SearchCount(-1);
                    if (databaseCount >= 0)
                    {
                        //if database exists
                        foreach (string filePathString in photosDirFileList)
                        {
                            FileInfo file = new FileInfo(filePathString);
                            try
                            {
                                int cameraID = -1;
                                DateTime cameraTime = new DateTime(0);
                                bool correctFormat = IsCorrectFormat(file.Name, ref cameraID, ref cameraTime);

                                string filePath = file.DirectoryName + "\\" + file.Name;
                                if (!correctFormat)
                                {
                                    debug.writeLog(DateTime.Now + " Exception in: " +
                                        this.ToString() + "DataManager.pushCameraSnapshotsIntoDatabase(): Incorrect format for file " + file.Name + "\n");

                                    if (Directory.Exists(Constants.trash))
                                    {
                                        //move to trash
                                        string targetPath = Constants.trash
                                            + Path.GetFileNameWithoutExtension(file.Name) + "_Error" + DateTime.Now.ToString("mmss")
                                            + Path.GetExtension(file.Name);
                                        File.Move(filePath, targetPath);
                                    }

                                    continue;
                                }

                                DateTime fileTime = File.GetCreationTime(filePath);

                                FileStream FilStr = new FileStream(filePath, FileMode.Open);
                                if (FilStr != null)
                                {
                                    BinaryReader BinRed = new BinaryReader(FilStr);

                                    byte[] imgbyte = new byte[FilStr.Length + 1];

                                    // Here you use ReadBytes method to add a byte array of the image stream.
                                    //so the image column will hold a byte array.
                                    imgbyte = BinRed.ReadBytes(Convert.ToInt32(BinRed.BaseStream.Length));

                                    BinRed.Close();
                                    FilStr.Close();

                                    int insertedCount = currentCameraSnapshotFile.Save(file.Name, cameraID, cameraTime,
                                        fileTime, imgbyte, true);
                                    if (insertedCount > 0)
                                    {
                                        file.Delete();
                                    }
                                    else
                                    {
                                        debug.writeLog(DateTime.Now + " Exception in: " +
                                            this.ToString() + "DataManager.pushCameraSnapshotsIntoDatabase(): Can't insert image file: " + file.Name + " into database. \n");
                                    }
                                } //if (FilStr != null)
                                else
                                {
                                    debug.writeLog(DateTime.Now + " Exception in: " +
                                        this.ToString() + "DataManager.pushCameraSnapshotsIntoDatabase(): Can't read file " + file.Name + "\n");
                                }
                            }
                            catch (Exception ex)
                            {
                                debug.writeLog(DateTime.Now + " Exception in: " +
                                    this.ToString() + "DataManager.pushCameraSnapshotsIntoDatabase(): " + ex.Message + " Exception for file: " + file.Name + "\n");
                            }
                        } //foreach (FileInfo file in photosDirFileList)
                    } //if (databaseCount >= 0)
                } //if (Directory.Exists(path))
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + "DataManager.pushCameraSnapshotsIntoDatabase(): " + ex.Message + "\n");
            }
        }

        private bool IsCorrectFormat(string fileName, ref int cameraID, ref DateTime cameraTime)
        {
            bool correctFormat = false;
            string currFileName = fileName;

            try
            {
                if ((fileName.ToUpper().EndsWith(".JPG") || fileName.ToUpper().EndsWith(".JPEG"))
                    && fileName.StartsWith("C_"))
                {
                    // If New Snapshots exists

                    //fileName is C_three digits camera ID_yy-MM-dd_HH-mm-ss-msms.jpg
                    //for example C_123_08-02-12_16-38-14-88.jpg

                    int index = -1;

                    //calculate cameraID
                    string cameraIDString = "";
                    cameraID = -1;
                    if (fileName.Length > 6)
                        cameraIDString = fileName.Substring(2, 3);

                    if (cameraIDString.Equals("000"))
                        cameraID = 0;
                    else
                        cameraID = Int32.Parse(cameraIDString.TrimStart('0'));

                    if ((cameraID != -1) && fileName.Substring(5, 1).Equals("_"))
                    {
                        fileName = fileName.Substring(6);
                        index = fileName.IndexOf("_");
                        if(index == -1)
                            index = fileName.IndexOf("-");
                        if (index > 0)
                        {
                            //calculate camera date
                            string cameraDateString = fileName.Substring(0, index);
                            int year = -1;
                            int month = -1;
                            int day = -1;

                            string[] dateElements = cameraDateString.Split('-');
                            if ((dateElements.Length == 3) && (cameraDateString.Length == 8)
                                && (dateElements[0].Length == 2) && (dateElements[1].Length == 2)
                                && (dateElements[2].Length == 2))
                            {
                                if (dateElements[0].Equals("00"))
                                    year = 0;
                                else
                                {
                                    year = Int32.Parse(dateElements[0].TrimStart('0'));
                                    if (year >= 70)
                                        year += 1900;
                                    else
                                        year += 2000;
                                }

                                if (dateElements[1].Equals("00"))
                                    month = 0;
                                else
                                    month = Int32.Parse(dateElements[1].TrimStart('0'));

                                if (dateElements[2].Equals("00"))
                                    day = 0;
                                else
                                    day = Int32.Parse(dateElements[2].TrimStart('0'));
                            }
                            //07.08.2009 Natasa
                            //For SANYO camera date format withaut '-' as spliter
                            else if (!cameraDateString.Contains("-"))
                            {
                                year = int.Parse(cameraDateString.Substring(0, 2));

                                if (year >= 70)
                                    year += 1900;
                                else
                                    year += 2000;

                                month = int.Parse(cameraDateString.Substring(2, 2));
                                day = int.Parse(cameraDateString.Substring(4, 2));
                            }

                            if (((year != -1) && (month != -1) && (day != -1))
                                && (fileName.Length > (index + 1)))
                            {
                                fileName = fileName.Substring(index + 1);
                                index = fileName.IndexOf(".");
                                if ((index > 0) && (index == fileName.LastIndexOf(".")))
                                {
                                    //calculate camera time
                                    string cameraTimeString = fileName.Substring(0, index);
                                    int hour = -1;
                                    int min = -1;
                                    int sec = -1;
                                    int milisec = -1;

                                    string[] timeElements = cameraTimeString.Split('-');
                                    if ((timeElements.Length == 4) && (cameraTimeString.Length == 11)
                                        && (timeElements[0].Length == 2) && (timeElements[1].Length == 2)
                                        && (timeElements[2].Length == 2) && (timeElements[3].Length == 2))
                                    {
                                        if (timeElements[0].Equals("00"))
                                            hour = 0;
                                        else
                                            hour = Int32.Parse(timeElements[0].TrimStart('0'));

                                        if (timeElements[1].Equals("00"))
                                            min = 0;
                                        else
                                            min = Int32.Parse(timeElements[1].TrimStart('0'));

                                        if (timeElements[2].Equals("00"))
                                            sec = 0;
                                        else
                                            sec = Int32.Parse(timeElements[2].TrimStart('0'));

                                        if (timeElements[3].Equals("00"))
                                            milisec = 0;
                                        else
                                            milisec = Int32.Parse(timeElements[3].TrimStart('0'));
                                    }
                                    //07.08.2009 Natasa
                                    //For SANYO camera date format withaut '-' as spliter
                                    else if (!cameraDateString.Contains("-"))
                                    {
                                        hour = int.Parse(cameraDateString.Substring(6, 2));
                                        min = int.Parse(cameraDateString.Substring(8, 2));
                                        sec = int.Parse(cameraDateString.Substring(10, 2));
                                        int i = fileName.IndexOf("-");
                                        milisec = int.Parse(fileName.Substring(i + 1, 4));
                                    }


                                    if ((hour != -1) && (min != -1) && (sec != -1) && (milisec != -1))
                                    {
                                        cameraTime = new DateTime(year, month, day, hour, min, sec, milisec);

                                        correctFormat = true;
                                    } //if (((hour != -1) && (min != -1) && (day != -1) && (milisec != -1))
                                } //if (index > 0), Camera Time
                            } //if ((year != 0) || (month != 0) || (day != 0)) ...
                        } //if (index > 0), direction
                    } //if ((cameraID != -1) && fileName.Substring(5, 1).Equals("_"))
                } //if (fileName.ToUpper().EndsWith(".JPG") || fileName.ToUpper().EndsWith(".JPEG")...
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " Exception in: " +
                    this.ToString() + "DataManager.pushCameraSnapshotsIntoDatabase(): " + ex.Message + " Exception for file: " + currFileName + "\n");
            }

            return correctFormat;
        }
	}
}
