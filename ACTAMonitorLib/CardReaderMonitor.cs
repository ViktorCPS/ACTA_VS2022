using System;
using System.Threading;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

using Common;
using Util;
using TransferObjects;
using ReaderInterface;

namespace ACTAMonitorLib
{
	/// <summary>
	/// CardReaderMonitor class is responsible for running a thread that reads card readers at specified addresses.
	/// Then, all found tags are placed in a queue for further processing by CardOwnerResolver to resolve a card owner 
	/// based on the card serial number. It is also responsible for synchronization with ReaderControl application.
	/// </summary>
	public class CardReaderMonitor : IDisposable 
	{
		private Thread worker;
		private bool stopMonitoring;
		private CardOwnerResolver cardOwnerResolver;

		private List<ReaderTO> readers = new List<ReaderTO>();
		private ArrayList readerInterfaces = new ArrayList();

		private NotificationController notificationController;

		private DebugLog log = null;

		private bool syncWithReaderControl = true;
		private bool isMonitoring = false;
		private ReaderControlMonitorSync readerControlMonitorSync = null;
		private bool rewriteGateSyncFile = false;

        private string gate;
        private Dictionary<int, string> readerCameras;
        private ArrayList tagsForSnapshots = new ArrayList();

        public CardReaderMonitor(List<ReaderTO> readers, string gate, Dictionary<int, string> camerasIP, ArrayList tags)
		{
            this.gate = gate.Trim();
            this.readerCameras = camerasIP;
            tagsForSnapshots = tags;

			//log = new DebugLog(Constants.logFilePath + System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id).ProcessName + "\\Log.txt");
            log = new DebugLog(Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt");

			notificationController = NotificationController.GetInstance();

			stopMonitoring = false;
			this.readers = readers;

            string auxPorts = ConfigurationManager.AppSettings["AuxPorts" + this.gate];
			if (auxPorts != null && auxPorts.Trim().Length > 0) 
			{
				syncWithReaderControl = false;
			}

            // testing GateSyncDAO
            //ReaderControlMonitorSyncFactory.TestGateSyncDAO();

			readerControlMonitorSync = ReaderControlMonitorSyncFactory.GetReaderControlMonitorSync(this.gate);

			// create gate synchronization file
			if (syncWithReaderControl)
			{
				readerControlMonitorSync.CreateGateSync();
			}

			if (CreateReadersInterfaces())
			{
				if (OpenReadersConnections()) 
				{
					isMonitoring = true;

					worker = new Thread(new ThreadStart(Work));
					worker.Name = "CardReaderMonitor";
					cardOwnerResolver = new CardOwnerResolver();
					worker.Start();
				}
			}
		}


        public void SetEmployeesAndTags(List<EmployeeTO> employees, List<TagTO> tags)
        {
            cardOwnerResolver.SetEmployeesAndTags(employees, tags);
        }

        public void Dispose() 
		{
			stopMonitoring = true;
			if (worker != null) 
			{
				worker.Join(); // Wait for the worker's thread to finish.
			}

			if (cardOwnerResolver != null) 
			{
				cardOwnerResolver.Dispose();
			}

			if (isMonitoring) 
			{
				CloseReadersConnections();
			}

			// delete gate synchronization file
			if (syncWithReaderControl)
			{
                readerControlMonitorSync.DeleteGateSync();
			}
		}

		private bool CreateReadersInterfaces() 
		{
			bool allCreated = true;
			foreach(ReaderTO reader in this.readers) 
			{
				try 
				{
					ReaderFactory.TechnologyType = reader.TechType;
					IReaderInterface readerInterface = ReaderFactory.GetReader;
					readerInterfaces.Add(readerInterface);
				}
				catch (Exception ex)
				{
					log.writeLog(ex);
					notificationController.DataProcessingStateChanged("Error creating reader " + 
						reader.ReaderID.ToString() + " interface!");
					allCreated = false;
				}
			}
			return allCreated;
		}


		private bool OpenReadersConnections() 
		{
            string auxPorts = ConfigurationManager.AppSettings["AuxPorts" + this.gate.Trim()];
			string [] comPortNums = null;
			if (auxPorts != null && auxPorts.Trim().Length > 0) 
			{
				comPortNums = auxPorts.Split(',');
			}

			bool anyOpened = false;
			foreach(IReaderInterface readerInterface in this.readerInterfaces) 
			{
				try 
				{
					bool opened = false;
					if (comPortNums != null) 
					{
						int comPortNum = Convert.ToInt32(comPortNums[readerInterfaces.IndexOf(readerInterface)]);
						if (readerInterface.Open(comPortNum)) opened = true;
					}
					else 
					{
                        Reader r = new Reader();
                        r.RdrTO = readers[readerInterfaces.IndexOf(readerInterface)];

						//if (readerInterface.Open(r.GetReaderAddress())) opened = true;
                        string monitorPort = "10001";
                        try
                        {
                            monitorPort = ConfigurationManager.AppSettings["MonitorPort"];
                        }
                        catch {}
                        string connectionAddress = r.RdrTO.ConnectionAddress.Trim() + ":" + monitorPort;
                        if (readerInterface.Open(connectionAddress)) opened = true;
					}
                    if (opened)
                    {
                        anyOpened = true;
                        readerInterface.Flush();
                    }
                    else
                    {
                        log.writeLog(DateTime.Now + " Error opening reader " + readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
                        notificationController.DataProcessingStateChanged("Error opening reader " +
                            readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
                    }
				}
				catch (Exception ex)
				{
					log.writeLog(ex);
					notificationController.DataProcessingStateChanged("Error opening reader " + 
						readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
				}
			}
			return anyOpened;
		}

		private void CloseReadersConnections() 
		{
			foreach(IReaderInterface readerInterface in this.readerInterfaces) 
			{
				try 
				{
					if (!readerInterface.Close()) log.writeLog(DateTime.Now + " Error closing reader " + readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
				}
				catch (Exception ex)
				{
					log.writeLog(ex);
					notificationController.DataProcessingStateChanged("Error closing reader " + 
						readers[readerInterfaces.IndexOf(readerInterface)].ReaderID.ToString() + " connection!");
				}
			}
		}

		private byte[] ReadThrownLogRecord(IReaderInterface readerInterface)
		{
            int messageLength = 0;
            if (readerInterface.GetTechnologyType().ToUpper() == "MIFARE")
            {
                messageLength = 16;
            }
            else if (readerInterface.GetTechnologyType().ToUpper() == "HITAG1")
            {
                messageLength = 19;     // ? - check, depends on firmware
            }
            else if (readerInterface.GetTechnologyType().ToUpper() == "HITAG2")
            {
                messageLength = 19;
            }
            else if (readerInterface.GetTechnologyType().ToUpper() == "ICODE")
            {
                messageLength = 20;     // ? - check, depends on firmware
            }

			const byte STX = 0x7E;
			const byte ETX = 0x7F;
			const byte ESC = 0x7D;

            byte[] resultBytes = new byte[messageLength];
			resultBytes[0] = resultBytes[1] = resultBytes[2] = resultBytes[3] = 0;

			byte[] portContent = new byte[1024];

			byte[] buffer = new byte[1];
			byte dataByte = 0;

			uint n = 0;

			int counter = 0;

			try 
			{
				n = readerInterface.Ready();
				if(n == 0) return resultBytes;
			
				for(int i = 0; i < n; i++)
				{
					readerInterface.Recv(buffer,1);	dataByte = buffer[0];
					if(dataByte == STX) break;
				}

				bool wasEsc = false; 

				if (dataByte == STX) 
				{
					counter = 0;
					DateTime t0 = DateTime.Now;
					while ((dataByte != ETX) && (counter < (messageLength-1))) 
					{
						
						if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 1000) 
						{
							Console.WriteLine("\nTime out!\n");
							break;
						}

						if(readerInterface.Recv(buffer,1) > 0)	
						{
							dataByte = buffer[0];
						}
						else
						{
							continue;
						}

						if (dataByte == ESC) 
						{
							wasEsc = true;
						} 
						else 
						{
							if (wasEsc) 
							{
								portContent[counter] = (byte) (dataByte ^ 0x20);
							}
							else
							{
								portContent[counter] = dataByte;
							}
							wasEsc = false;
							counter++;							
						}
					}
                    if ((counter == (messageLength - 1)) && (dataByte == ETX))
					{
						Console.WriteLine("\nSuccess\n");
						for (int i = 0; i < counter; i++) 
						{
							resultBytes[i] = portContent[i];
							Console.Write(resultBytes[i].ToString("X2") + " ");
						}
						Console.WriteLine("\n");
					}
					else
					{
						// for testing purpose
						Console.WriteLine("\nFailure\n");
						for (int i = 0; i < counter; i++) 
						{
							Console.Write(portContent[i].ToString("X2") + " ");
						}
						Console.WriteLine("\n");

						readerInterface.Flush();
					}
				} 
			} 
			catch (Exception ex) 
			{
				log.writeLog(ex);
				throw ex;
			}
		
			return resultBytes;
		}

		private void SynchronizeWithReaderControlApplication() 
		{
			ReaderControlMonitorSync rcmSync = readerControlMonitorSync.ReadGateSync();
			if (rcmSync != null) 
			{
				if (rcmSync.readerControlRequest.ToUpper() == "YES" &&
					isMonitoring) 
				{
					CloseReadersConnections();

					isMonitoring = false;
					rewriteGateSyncFile = !readerControlMonitorSync.WriteGateSync(new ReaderControlMonitorSync("YES", "NO"));
					notificationController.DataProcessingStateChanged("Monitoring on gate " + this.gate + " stopped by another application!");
                    log.writeLog(DateTime.Now + " Monitoring on gate " + this.gate + " stopped by another application!\n");
				}
				if (rcmSync.readerControlRequest.ToUpper() == "NO" &&
					!isMonitoring) 
				{
					OpenReadersConnections();

					isMonitoring = true;
					rewriteGateSyncFile = !readerControlMonitorSync.WriteGateSync(new ReaderControlMonitorSync("NO", "YES"));
                    notificationController.DataProcessingStateChanged("Monitoring on gate " + this.gate + " restarted by another application!");
                    log.writeLog(DateTime.Now + " Monitoring on gate " + this.gate + " restarted by another application!\n");
				}
			}

			if (rewriteGateSyncFile) 
			{
				if (!isMonitoring) 
				{
					rewriteGateSyncFile = !readerControlMonitorSync.WriteGateSync(new ReaderControlMonitorSync("YES", "NO"));
				}
				else 
				{
					rewriteGateSyncFile = !readerControlMonitorSync.WriteGateSync(new ReaderControlMonitorSync("NO", "YES"));
				}
			}
		}

		private void Work() 
		{
			try 
			{
				while (true) 
				{
					if (!stopMonitoring) 
					{
						// iterate through the readers
						foreach(IReaderInterface readerInterface in readerInterfaces) 
						{
							if(!readerInterface.isOpen()) continue;
							ReaderTO reader = readers[readerInterfaces.IndexOf(readerInterface)];
							//Console.WriteLine ("Monitor > reader: " + reader.GetReaderAddress().ToString("X8"));
							try
							{
								byte[] logRecord = ReadThrownLogRecord(readerInterface);

                                bool isValid = true;   // DC 1.9.2008.

								uint serNo = logRecord[3]*(uint)Math.Pow(2,24) + logRecord[2]*(uint)Math.Pow(2,16) + logRecord[1]*(uint) Math.Pow(2,8) + logRecord[0];
                                if (serNo == 0) isValid = false;

								int antenna = (int)logRecord[4];
                                if ((antenna != 0) && (antenna != 1)) isValid = false;

								bool entrancePermitted = (logRecord[5] == 4);

                                int seconds = logRecord[6];
                                int minutes = logRecord[7];
                                int hours = logRecord[8];
                                int day = logRecord[9];
                                int month = logRecord[10] & 0x0F;
                                int year = logRecord[11];
                                if ((seconds > 59) || (minutes > 59) || (hours > 23) ||
                                    (day < 1) || (day > 31) || (month < 1) || (month > 12) || (year > 99)) isValid = false;
                                year += 2000;
                                DateTime eventTime = new DateTime(year, month, day, hours, minutes, seconds);

								//if(serNo != 0)        
                                if (isValid)            // DC 1.9.2008.
								{
									Console.WriteLine ("Monitor >  card serial number: " + serNo.ToString());
									cardOwnerResolver.EnqueueTask(new CardOwner(reader,serNo,antenna,entrancePermitted,eventTime));

                                    //09.07.2009 Natasa
                                    if (readerCameras != null && readerCameras.Count > 0&&readerCameras[reader.ReaderID].Trim().Length>0&&tagsForSnapshots.Contains(serNo))
                                    {
                                        takeCameraSnapshot(readerCameras[reader.ReaderID],reader.Description);
                                    }
								}
							}
							catch {}
							Thread.Sleep (100);
						}

						// synchronization with ReaderControl application
						if (syncWithReaderControl) 
						{
							SynchronizeWithReaderControlApplication();
						}
					}
					else 
					{
						return;
					}
				}
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " The thread " + Thread.CurrentThread.Name + " has exited with exception: " + 
					ex.Message + "\n\n" + ex.StackTrace + "\n\n");
			}
		}

        private void takeCameraSnapshot(string IP, string reader)
        {
            try
            {
                
                int total = 0;
                byte[] buffer = GetSnapshot(IP, out total);
                
                // get bitmap
                Bitmap bmp = (Bitmap)Bitmap.FromStream(
                              new MemoryStream(buffer, 0, total));
                bmp.Save(Constants.SnapshotsFilePath +reader+"_"+ DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".jpg", ImageFormat.Jpeg);

            }
            catch(Exception ex) 
            {
                log.writeLog(DateTime.Now + " Camera: "+IP+" snapshots faild."+ex.Message);
            }
        }

       

        //returns snapshot from camera as byte array
        public byte[] GetSnapshot(string IP, out int total)
        {

            byte[] buffer = null;
            total = 0;
            try
            {

                string sourceURL = "http://" + IP + "/jpg/1/image.jpg";
                int read = 0;
                
                // create HTTP request
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
                req.Credentials = new NetworkCredential(Constants.CammeraUser,Constants.CammeraPass);
                // get response
                try
                {
                    
                    WebResponse resp = req.GetResponse();
                    buffer = new byte[100000];

                    // get response stream
                    Stream stream = resp.GetResponseStream();
                    // read data from stream
                    while ((read = stream.Read(buffer, total, 1000)) != 0)
                    {
                        total += read;
                    }
                    resp.Close();
                  
                }
                catch (WebException ex)
                {
                    log.writeLog(DateTime.Now + " Camera: " + IP + " snapshots faild." + ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Camera: " + IP + " snapshots faild." + ex.Message);              
            }

            return buffer;

        }
	}
}
