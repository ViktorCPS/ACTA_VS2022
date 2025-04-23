using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using SerialPorts;
using Common;
using Util;
using TransferObjects;

namespace ReaderManagement
{
	public class MonitorThread
	{
		protected SerialPort serialPort;
		private bool doReadComPort = false;
		protected Thread comPortReaderThread;
		private string cardSerial = "";
		private string antenna = "";		
		private string readerId = "";
		private int readerNum = 0;
		private string port = "";
		private byte SOM = 0x7E;
		private byte EOM = 0x7F;
		private byte ESC = 0x7D;
		private ReaderTO currentReader;

		ReaderEventController controller;
		
		public string ReaderID
		{
			get { return readerId; }
		}

		public bool IsActive
		{
			get { return doReadComPort; }
		}
		

		public MonitorThread(ReaderTO reader, int readerNum) 
		{
			this.readerId = reader.ReaderID.ToString();
			this.readerNum = readerNum;
			this.currentReader = reader;

			if (reader.ConnectionType.Equals(Constants.ConnTypeIP))
			{
				// TODO:
			}
			else if (reader.ConnectionType.Equals(Constants.ConnTypeSerial))
			{
				this.port = reader.ConnectionAddress;
			}

			controller = ReaderEventController.GetInstance();
		}

		public void StartComPortReading() 
		{
			try 
			{
				OpenComPort();
				doReadComPort = true;
				comPortReaderThread = new Thread(new ThreadStart(ComPortReader));
				comPortReaderThread.Start();
			} 
			catch
			{
				doReadComPort = false;
				if (comPortReaderThread != null) 
				{
					comPortReaderThread.Abort();
				}				
			}
		}

		public void StopComPortReading() 
		{
			try 
			{
				if (doReadComPort)
				{
					doReadComPort = false;

					try 
					{
						comPortReaderThread.Abort();
						comPortReaderThread = null;
					} 
					catch {}
					CloseComPort();
				}
			} 
			catch (Exception e) 
			{
				throw e;
			}
		}

		private void OpenComPort() 
		{			
			try	
			{ 
				serialPort = new SerialPort(int.Parse(this.port),38400,8,0,1);
				if (!serialPort.Open()) 
				{				
					throw new Exception("Cannot open COM Port.");
				}
			} 
			catch (Exception e) 
			{ 
				serialPort.Close();
				serialPort = null;
				throw e;
			}
		}

		private void CloseComPort() 
		{
			try 
			{
				if (serialPort != null) 
				{
					serialPort.Close();
					serialPort = null;
				}
			} 
			catch (Exception e) 
			{
				throw e;
			}
		}

		private void ComPortReader() 
		{
			int comPortContentLength = 1024;
			byte[] comPortContent = new byte[comPortContentLength];

			byte[] buffer = new byte[1];

			uint n = 0;

			serialPort.Flush();
			while (doReadComPort) 
			{
				int counter = 0;
			
				try 
				{
					n = serialPort.Ready();
					if(n > 0)
					{
						serialPort.Recv(buffer, 1);
					}
					byte dataByte = buffer[0];

					bool wasEsc = false; 

					if (dataByte == SOM) 
					{
						counter = 0;
						while ((dataByte != EOM) && (counter < 16)) 
						{
							serialPort.Recv(buffer, 1);
							dataByte = buffer[0];

							if (dataByte == ESC) 
							{
								wasEsc = true;
							} 
							else 
							{
								if (wasEsc) 
								{
									dataByte = (byte) (dataByte ^ 0x20);
								}
								wasEsc = false;
								comPortContent[counter] = dataByte;
								counter++;							
							}
						}
						if (counter < 16)
						{
							byte[] resultBytes = new byte[16];
							for (int i=0; i<16; i++) 
							{
								resultBytes[i] = comPortContent[i];
							}

							GetInfo(resultBytes);					
						}
						else
						{
							serialPort.Flush();
						}
					} 
				} 
				catch (Exception e) 
				{
					throw e;
				}
		
				Thread.Sleep(500);
			}
		}

		public void GetInfo(byte[] data) 
		{
		//	string[] info = new string[2];
			// Serial number ( bytes 0-3 )
            //uint serial = data[3] * (uint)Math.Pow(2,24) + data[2] * (uint)Math.Pow(2,16) + data[1] * (uint) Math.Pow(2,8) + data[0];

            //cardSerial = serial.ToString();			


            ulong serial = 0;
            //if (data[15] == 0xC3)            // standard 4-bytes UID
            //{
            //    serial = ((uint)data[3] << 24) + ((uint)data[2] << 16) + ((uint)data[1] << 8) + (uint)data[0];
            //    this.cardSerial = serial.ToString();
            //}
             if (data[15] == 0x04)      // 7-byte UID, NXP
            {
                serial = ((ulong)data[3] << 48) + ((ulong)data[2] << 40) + ((ulong)data[1] << 32) + ((ulong)data[0] << 24) + ((ulong)data[13] << 16) + ((ulong)data[14] << 8) + (ulong)data[15];
                this.cardSerial = serial.ToString();
            }
            
            
            antenna = data[4].ToString();
			try
			{
				//controller.populateControl(currentReader, Convert.ToInt32(antenna), Convert.ToUInt32(cardSerial)); 
				controller.SendMonitorNotification(currentReader, Convert.ToInt32(antenna), Convert.ToUInt64(cardSerial)); 
			}
			catch(Exception e)
			{
				throw e;
			}
		}

	}		
}

