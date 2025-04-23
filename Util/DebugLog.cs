using System;
using System.IO;
using System.Threading;
using System.Configuration;

namespace Util
{
	/// <summary>
	/// Summary description for Log.
	/// </summary>
	public class DebugLog
	{
		private string logFilePath = "";
		private bool succeeded = false;

		private static bool logFolderExistanceSolved = false;

		private static bool benchmarking = false;
		private static bool benchmarkingRead = false;

        private static int currentLevel = 0;

		public DebugLog(string logFilePath)
		{
			try
			{
				this.logFilePath = logFilePath;
				if (!logFolderExistanceSolved)
				{
					string logPath = Directory.GetParent(logFilePath).FullName;
					if (!Directory.Exists(logPath)) 
					{
						Directory.CreateDirectory(logPath);
					}
					logFolderExistanceSolved = true;
				}

				if (!benchmarkingRead)
				{
					try
					{
						benchmarking = (ConfigurationManager.AppSettings["BENCHMARKING"].ToUpper() == "YES");
					}
					catch
					{
					}
					benchmarkingRead = true;
				}
                try
                {
                    currentLevel = int.Parse(ConfigurationManager.AppSettings["DebugLevel"].ToString());
                }
                catch
                {
                }
			}
			catch
			{
				this.logFilePath = Constants.logFilePath;
			}
		}

		public void createLog()
		{
			FileStream stream = new FileStream(this.logFilePath, FileMode.Append);
			stream.Close();
		}

		public void writeLog(string message)
		{	
			succeeded = false;
			DateTime t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					StreamWriter writer = File.AppendText(logFilePath);
					writer.WriteLine(message);
					writer.Close();
					succeeded = true;

				}
				catch
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 60000) succeeded = true;
				}
			}
		}

		public void writeLog(System.Exception ex)
		{	
			string message = DateTime.Now.ToString()+" Exception: "+ex.Message+"\n\n"+ex.StackTrace+"\n\n";

			succeeded = false;
			DateTime t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					StreamWriter writer = File.AppendText(logFilePath);
					writer.WriteLine(message);
					writer.Close();
					succeeded = true;

				}
				catch
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 60000) succeeded = true;
				}
			}
		}

		public void writeBenchmarkLog(string msg)
		{	
			if (!benchmarking) return;

			string message = "BENCHMARK>>> " + DateTime.Now.ToString() + " " + msg;

			succeeded = false;
			DateTime t0 = DateTime.Now;
			while(!succeeded)
			{
				try
				{
					StreamWriter writer = File.AppendText(logFilePath);
					writer.WriteLine(message);
					writer.Close();
					succeeded = true;

				}
				catch
				{
					Thread.Sleep(100);
					if((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 60000) succeeded = true;
				}
			}
		}

        public void writeLog(string message, int level)
        {
            if (level <= currentLevel)
            {
                succeeded = false;
                DateTime t0 = DateTime.Now;
                while (!succeeded)
                {
                    try
                    {
                        // if Log file > MAX_LOG_SIZE rename it adding date&time and start a new one
                        //try
                        //{
                        //    if ((new FileInfo(logFilePath)).Length > MAX_LOG_SIZE)
                        //    {
                        //        string renamedLogFilePath = logFilePath.Replace(".txt", (DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt"));
                        //        File.Move(logFilePath, renamedLogFilePath);

                        //        string logFolder = (new FileInfo(renamedLogFilePath)).DirectoryName;
                        //        string zipName = logFolder + @"\Logs.zip";
                        //        Misc.Add2Zip(renamedLogFilePath, zipName);

                        //        File.Delete(renamedLogFilePath);
                        //    }
                        //}
                        //catch (Exception ex) { string e = ex.Message; }

                        StreamWriter writer = File.AppendText(logFilePath);
                        writer.WriteLine(message);
                        writer.Close();
                        succeeded = true;
                    }
                    catch
                    {
                        Thread.Sleep(100);
                        if ((DateTime.Now.Ticks - t0.Ticks) / TimeSpan.TicksPerMillisecond > 60000) succeeded = true;
                    }
                }
            }
        }
	}
}
