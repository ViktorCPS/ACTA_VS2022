using System;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

using Util;

namespace Common
{
	/// <summary>
	/// ReaderControlMonitorFileSync is responsible for synchronization of a gate access among ReaderControl 
	/// and Monitor applications. The synchronization is based on GateSync file.
	/// </summary>
    public class ReaderControlMonitorFileSync : ReaderControlMonitorSync
	{
		public ReaderControlMonitorFileSync(string gate) : base(gate)
		{
		}

        public override bool CreateGateSync()
        {
            bool success = true;
            try
            {
                if (!Directory.Exists(Constants.MonitoringPath))
                {
                    Directory.CreateDirectory(Constants.MonitoringPath);
                }
            }
            catch (IOException)
            {
                success = false;
                log.writeLog(DateTime.Now + " ReaderControlMonitorFileSync.CreateGateSync(): " + 
                    "can't create monitoring GateSync folder.\n");
            }
            catch (Exception ex)
            {
                success = false;
                log.writeLog(ex);
            }

            if (success)
            {
                return this.WriteGateSync(new ReaderControlMonitorSync("NO", "YES"));
            }
            else
            {
                return false;
            }
        }

        public override bool WriteGateSync(ReaderControlMonitorSync rcmSync) 
		{
			bool success = true;
			FileStream stream = null;
			try 
			{
				stream = new FileStream(syncFile, FileMode.Create,
					FileAccess.Write, FileShare.None);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ReaderControlMonitorSync));
				xmlSerializer.Serialize(stream, rcmSync);
			}
            catch (IOException)
            {
                success = false;
                log.writeLog(DateTime.Now + " ReaderControlMonitorFileSync.WriteGateSync(): " + 
                    syncFile + " is locked.\n");
            }
            catch (Exception ex)
			{
				success = false;
				log.writeLog(ex);
			}
			finally 
			{
				if (stream != null) 
				{
					stream.Close();
				}
			}
			return success;
		}

		public override ReaderControlMonitorSync ReadGateSync() 
		{
			ReaderControlMonitorSync rcmSync = null;
			if (File.Exists(syncFile)) 
			{
				FileInfo syncFileInfo = new FileInfo(syncFile);
				if (lastWriteTime != syncFileInfo.LastWriteTime) 
				{
					FileStream stream = null;
					try 
					{
						stream = new FileStream(syncFile, FileMode.Open,
							FileAccess.Read, FileShare.None);
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(ReaderControlMonitorSync));
						rcmSync = (ReaderControlMonitorSync) xmlSerializer.Deserialize(stream);
					}
                    catch (IOException)
                    {
                        log.writeLog(DateTime.Now + " ReaderControlMonitorFileSync.ReadGateSync(): " +
                            syncFile + " is locked.\n");
                    }
                    catch (Exception ex)
					{
						log.writeLog(ex);
					}
					finally 
					{
						if (stream != null) 
						{
							stream.Close();
							lastWriteTime = syncFileInfo.LastWriteTime;
						}
					}
				}
			}
			return rcmSync;
		}

        public override bool DeleteGateSync()
        {
            bool deleted = false;
            bool succeeded = false;
            DateTime t0 = DateTime.Now;
            while (!succeeded)
            {
                try
                {
                    File.Delete(syncFile);
                    succeeded = true;
                    deleted = true;
                }
                catch
                {
                    Thread.Sleep(100);
                    if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 60000)
                    {
                        succeeded = true;
                        if (!deleted)
                        {
                            log.writeLog(DateTime.Now + " Monitor unable to delete gate synchronization file for the gate " + gate + "!");
                        }
                    }
                }
            }
            return deleted;
        }

        public override bool CanSynchronize()
        {
            return File.Exists(syncFile);
        }
	}
}
