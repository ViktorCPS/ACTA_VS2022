using System;
using System.Threading;
using System.Text;

namespace DeviceRFID
{
    public class RFIDDevice
    {
        #region constants
		private const int MAX_BUFF_LENGTH = 1024;
		private const byte NO_ERROR = 0;
		private const int TIMEQUANT = 10;
		private const int MIN_REMOTE_TIMEOUT = 30000;
		public const byte PACKAGE_LENGTH = 16;
        public const int MAX_LOG_RECS = 32768; // for readers before Cortex M3: 28800;
		public const byte REMOTE = 0;
		public const byte AUTOMATIC = 1;
		public const byte CARDS = 1;
		public const byte EVENTS = 2;
		public const byte GROUPS = 3;
        public const byte CARDS_2 = 5;
		public const byte LOG = 4;
        public const byte CARDS7 = 6;
        public const byte IP_OR_SERIAL = 0;
		public const byte GSM = 1;
        public const byte GATEDEVICE = 1;
        public const byte BARRIERDEVICE = 0;
		private const byte CMD_RESETDEVICE = 0x52;
		private const byte CMD_SETRTC = 0x58;
		private const byte CMD_GETRTC = 0x59;
		private const byte CMD_SETMODE = 0x88;
		private const byte CMD_GETMODE = 0x89;
		private const byte CMD_SETAUTOMODE = 0x9E;
		private const byte CMD_UPLOADCARDS = 0x79;
		private const byte CMD_UPLOADEVENTS = 0x7B;
		private const byte CMD_UPLOADGROUPS = 0x7D;
		private const byte CMD_DOWNLOADCARDS = 0x7A;
		private const byte CMD_DOWNLOADEVENTS = 0x7C;
		private const byte CMD_DOWNLOADGROUPS = 0x7E;
		private const byte CMD_DOWNLOADLOG = 0x7F;
		private const byte CMD_DIFFDOWNLOADLOG = 0x80;
		private const byte CMD_DIFFHITAGDOWNLOADLOG = 0x85;
		private const byte CMD_ERASELOG = 0x86;
		private const byte CMD_ERASEFLASH = 0x87;
		private const byte CMD_GETLOGOCCUPIED = 0x9C;
		private const byte CMD_SETSERNUM = 0x93;
		private const byte CMD_GETSERNUM = 0xB0;
		private const byte CMD_SETDOTIME = 0x94;
		private const byte CMD_GETDOTIME = 0xB1;
		private const byte CMD_GETPRODUCTINFO = 0xB3;
		private const byte CMD_SETTAMPER = 0x95;
		private const byte CMD_GETTAMPER = 0xB2;
        private const byte CMD_SETCAMERATRIGGER = 0xAB;
        private const byte CMD_GETCAMERATRIGGER = 0xAA;

		private const byte CMD_READBLOCK_ANT0 = 0x41;
		private const byte CMD_READBLOCK_ANT1 = 0x42;
		private const byte CMD_WRITEBLOCK_ANT0 = 0x61;
		private const byte CMD_WRITEBLOCK_ANT1 = 0x62;

		private const byte CMD_GETCARDSERIALNUMBER_ANT0 = 0x46;
		private const byte CMD_GETCARDSERIALNUMBER_ANT1 = 0x47;

		private const byte CMD_READPROTECTEDBLOCKA_ANT0 = 0x43;
		private const byte CMD_WRITEPROTECTEDBLOCKA_ANT0 = 0x63;
		private const byte CMD_WRITESECTORPASSWORDA_ANT0 = 0x64;
        private const byte CMD_READPROTECTEDBLOCKA_ANT1 = 0x31;
        private const byte CMD_WRITEPROTECTEDBLOCKA_ANT1 = 0x32;
        private const byte CMD_WRITESECTORPASSWORDA_ANT1 = 0x33;

        private const byte CMD_READPROTECTEDBLOCKB_ANT0 = 0x44;
        private const byte CMD_WRITEPROTECTEDBLOCKB_ANT0 = 0x65;
        private const byte CMD_WRITESECTORPASSWORDB_ANT0 = 0x66;
        private const byte CMD_READPROTECTEDBLOCKB_ANT1 = 0x34;
        private const byte CMD_WRITEPROTECTEDBLOCKB_ANT1 = 0x35;
        private const byte CMD_WRITESECTORPASSWORDB_ANT1 = 0x36;

        private const byte CMD_GETHITAG1CARDSERIALNUMBER_ANT0 = 0x47;
        private const byte CMD_GETHITAG2CARDSERIALNUMBER_ANT0 = 0x80;

		private const byte CMD_SETRELAY = 0x4F;
		private const byte CMD_SETSIGNALS = 0x53;
		private const byte CMD_GETDIGITALINPUTS = 0x49;
		private const byte CMD_SETREMOTETIMEOUT = 0x96;
		private const byte CMD_WRITELOGRECORD = 0x81;
		private const byte CMD_WRITEHITAGLOGRECORD = 0x90;

		private const byte CMD_SETRELAYHOLDTIME = 0xA0;
		private const byte CMD_GETRELAYHOLDTIME = 0xA1;
		private const byte CMD_SETAFTERPASSTIME = 0xA2;
		private const byte CMD_GETAFTERPASSTIME = 0xA3;
		private const byte CMD_SETERRORDECLARETIME = 0xA4;
		private const byte CMD_GETERRORDECLARETIME = 0xA5;
		private const byte CMD_GETFIRMWAREVER = 0xA6;
		private const byte CMD_SETANTTYPE = 0xA7;
		private const byte CMD_GETANTTYPE = 0xA8;

        private const byte CMD_READWHOLEMEMORY = 0x71;
        private const byte CMD_WRITEWHOLEMEMORY = 0x72;

        private const byte CMD_GETGSMMODE = 0x29;
        private const byte CMD_SETGSMMODE = 0x2A;

        private const byte CMD_WRITEEEPROMREG = 0xC0;
        private const byte CMD_READEEPROMREG = 0xC1;
        private const byte CMD_WRITEREGISTER = 0xC2;
        private const byte CMD_READREGISTER = 0xC3;

        private const byte CMD_GETPRODUCTINFOFIELD = 0xC4;

        private const byte CMD_GETDIFFLOGPOINTER = 0xC5;
        private const byte CMD_RESETDIFFLOGPOINTER = 0xC6;
        private const byte CMD_SETDIFFLOGPOINTER = 0xC8;

        private const byte CMD_SETDEVICETYPE = 0xC7;

		public const byte AUTONORMAL = 0;
		public const byte AUTOCAMERA = 1;
		public const byte AUTOLOG = 2;

        // Cortex reader version commands
        private const byte CMD_SETREGIME = 0x98;
        private const byte CMD_GETREGIME = 0x99;
        private const byte CMD_SETREGIMETIMEOUT = 0x9A;
        private const byte CMD_GETREGIMETIMEOUT = 0x9B;
        private const byte CMD_SETEMERGENCY = 0xBB;
        private const byte CMD_GETEMERGENCY = 0xBC;
        private const byte CMD_SETDIGITALOUTPUTS = 0xBD;

        // alarm reader version commands
        private const byte CMD_UPLOADCARDS_2 = 0x20;
        private const byte CMD_DOWNLOADCARDS_2 = 0x21;
        private const byte CMD_SETACTIVETABLE = 0x22;
        private const byte CMD_GETACTIVETABLE = 0x23;
        private const byte CMD_SETIPPARAMETERS = 0x24;
        private const byte CMD_GETIPPARAMETERS = 0x25;
        private const byte CMD_SETMACADDRESS = 0x26;
        private const byte CMD_GETMACADDRESS = 0x27;
        private const byte CMD_SETLOGINTIME = 0x34;
        private const byte CMD_GETLOGINTIME = 0x2B;
        private const byte CMD_SETAUTHORIZATIONTIMEOUT = 0x2C;
        private const byte CMD_GETAUTHORIZATIONTIMEOUT = 0x2D;
        private const byte CMD_SETALARMTIME = 0x2E;
        private const byte CMD_GETALARMTIME = 0x2F;
        private const byte CMD_SETTCPSOCKET = 0x30;
        private const byte CMD_GETTCPSOCKET = 0x31;
        private const byte CMD_SETTCPLOGSOCKET = 0x32;
        private const byte CMD_GETTCPLOGSOCKET = 0x33;

        // ups monitor setup commands
        private const byte CMD_GETTRAPIP = 0x3A;
        private const byte CMD_SETTRAPIP = 0x3B;
        private const byte CMD_GETDHCP = 0x3C;
        private const byte CMD_SETDHCP = 0x3D;
        private const byte CMD_GETMAXVOLTAGE = 0x3E;
        private const byte CMD_SETMAXVOLTAGE = 0x3F;
        private const byte CMD_GETMINVOLTAGE = 0x40;
        private const byte CMD_SETMINVOLTAGE = 0x41;
        private const byte CMD_GETMAXCURRENT = 0x42;
        private const byte CMD_SETMAXCURRENT = 0x43;
        private const byte CMD_GETTRAPMASK = 0x4D;
        private const byte CMD_SETTRAPMASK = 0x4E;

        // Bluetooth commands
        private const byte CMD_GETBTNO = 0x50;
        private const byte CMD_GETBTADDRESS = 0x51;

        // 7 bytes ID cards
        private const byte CMD_UPLOAD7BYTEIDCARDS = 0xC9;
        private const byte CMD_DOWNLOAD7BYTEIDCARDS = 0xCA;

        // locker controller commands
        private const byte CMD_GETLOCKERSTATUSES = 0x011;
        private const byte CMD_SETLOCKERSTATUSES = 0x012;
		#endregion

		#region variables
		private IRFIDInterface sp;
		public string technologyType = null;
        private byte[] rcvBuff = null;
        private int TIMEOUT = 1000;

        // logging functionality properties
        public bool logging = false;
        private const int MAX_BYTE_SEQUENCES = 10240;
        int currByteSequence = -1;
        private string[] byteSequences = new string[MAX_BYTE_SEQUENCES];
		#endregion

		#region private functions
		private bool SendMsg(byte command)
		{
			try
			{
				sp.Flush();
				byte[] message = new byte[3];
				message[0] = (byte)(message.Length-1); message[1] = command;
				bcc(message);
                if (logging) AddByteSequenceToLog("C>R", message);
				return(sp.Send(message) == message.Length);
			}
			catch(Exception) { return false; }
		}

		private bool SendMsg(byte command, byte param)
		{
			try
			{
				sp.Flush();
				byte[] message = new byte[4];
				message[0] = (byte)(message.Length-1); message[1] = command;
				message[2] = param;
                bcc(message);
                if (logging) AddByteSequenceToLog("C>R",message);
				return(sp.Send(message) == message.Length);
			}
			catch(Exception) { return false; }
		}

		private bool SendMsg(byte[] b)
		{
			try
			{
				sp.Flush();
                byte[] message = new byte[b.Length + 1]; b.CopyTo(message, 0); bcc(message);
                if (logging) AddByteSequenceToLog("C>R", message);
				return(sp.Send(message) == message.Length);
			}
			catch(Exception) { return false; }
		}

		private bool GetMsg()
		{
			byte[] l = new byte[1];
			byte[] rb = new byte[MAX_BUFF_LENGTH];
			try
			{
				long ticks0 = DateTime.Now.Ticks;
				while(sp.Ready() <= 0)
				{
					if((DateTime.Now.Ticks-ticks0) / TimeSpan.TicksPerMillisecond > TIMEOUT) break;
				}
				if(sp.Ready() <= 0) return false;
				sp.Recv(l,1); uint n = l[0];

				ticks0 = DateTime.Now.Ticks;
				uint spReady = 0;
				while(spReady < n)
				{
					while(sp.Ready() > 0)
					{
						sp.Recv(l,1);
						rb[spReady] = l[0];
						spReady++;
 					}
					if((DateTime.Now.Ticks-ticks0) / TimeSpan.TicksPerMillisecond > TIMEOUT) break;
				}
				if(spReady < n) return false;

				rcvBuff[0] = (byte)n;
				for(int i = 0; i < n; i++) rcvBuff[i+1] = rb[i];
                if (logging) AddByteSequenceToLog("R>C", rcvBuff);
				return CheckCRC(rcvBuff,n+1);
			}
			catch(Exception) { return false; }		
		}

		private bool DeviceStatus()
		{
			try
			{
				return(rcvBuff[1] == NO_ERROR);
			}
			catch(Exception) { return false; }		
		}

		private void bcc(byte[] b)
		{
			for(int i = 0; i < b[0]; i++) b[b[0]] ^= b[i];
		}

		private bool CheckCRC(byte[] b,uint n)
		{
			byte crc = 0;
			for(int i = 0; i < n-1; i++) crc ^= b[i];
			return (crc == b[n-1]);
		}

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public RFIDDevice()
		{
			try
			{
				rcvBuff = new byte[MAX_BUFF_LENGTH];
			}
			catch(Exception ex) { throw ex; }
		}

		/// <summary>
		/// Opens COM port with given baudrate
		/// </summary>
		/// <param name="comportnumber"></param>
		/// <param name="baudrate"></param>
		/// <returns></returns>
		public int Open(int comportnumber,int baudrate)
		{
            if (logging) AddByteSequenceToLog("CMD", "Open" + " " + comportnumber.ToString() + " " + baudrate.ToString());

			try
			{
				IRFIDFactory.InterfaceType = "SERIAL";
				IRFIDFactory.Address = comportnumber.ToString();
				IRFIDFactory.Speed = baudrate;
				sp = IRFIDFactory.GetInterface;
				//sp = new SerialPort(comportnumber,baudrate,8,0,1);
				return(sp.Open() ? 1 : 0);
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Opens IP port
		/// </summary>
		/// <param name="IPAddress"></param>
		/// <returns></returns>
		public int Open(string IPAddress)
		{
            if (logging) AddByteSequenceToLog("CMD", "Open" + " " + IPAddress);

			try
			{
				IRFIDFactory.InterfaceType = "IP";
				IRFIDFactory.Address = IPAddress;
				sp = IRFIDFactory.GetInterface;
				return(sp.Open() ? 1 : 0);
			}
			catch(Exception) { return 0; }
		}

        /// <summary>
        /// Opens IP port with timeout
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        public int Open(string IPAddress, int secTimeout)
        {
            if (logging) AddByteSequenceToLog("CMD", "Open" + " " + IPAddress);

            try
            {
                IRFIDFactory.InterfaceType = "IP";
                IRFIDFactory.Address = IPAddress;
                sp = IRFIDFactory.GetInterface;
                return (sp.Open(secTimeout) ? 1 : 0);
            }
            catch (Exception) { return 0; }
        }



        /// <summary>
        /// Opens GSM port
        /// </summary>
        /// <param name="comportnumber"></param>
        /// <param name="phonenumber"></param>
        /// <returns></returns>
        public int Open(int comportnumber, string phonenumber)
        {
            if (logging) AddByteSequenceToLog("CMD", "Open" + " " + comportnumber.ToString() + " " + phonenumber);

            try
            {
                TIMEOUT = 3000;
                IRFIDFactory.InterfaceType = "GSM";
                IRFIDFactory.Address = comportnumber.ToString() + " " + phonenumber;
                sp = IRFIDFactory.GetInterface;
                return (sp.Open() ? 1 : 0);
            }
            catch (Exception) { return 0; }
        }

		/// <summary>
		/// Closes COM port
		/// </summary>
		/// <returns></returns>
		public int Close()
		{
            if (logging) AddByteSequenceToLog("CMD", "Close");

			try
			{
				sp.Close();
				return 1;
			}
			catch(Exception) { return 0; }
		}

		public byte Error { get { return rcvBuff[1]; } }

		public bool IsOpen { get { if(sp != null) return sp.IsOpen(); else return false; } }

		/// <summary>
		/// Sets RFID device's working mode (0 - REMOTE, 1 - AUTOMATIC)
		/// </summary>
		/// <param name="mode"></param>
		/// <returns></returns>
		public int SetMode(byte mode)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetMode");

			try
			{
				if(!SendMsg(CMD_SETMODE,mode)) return 0;
				if(!GetMsg())
				{
					sp.Flush();
					if(!SendMsg(CMD_SETMODE,mode)) return 0;
					if(!GetMsg()) return 0;
					if(!DeviceStatus()) return 0;
				}
				else 				
				{
					if(!DeviceStatus()) return 0;
				}
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Returns RFID device's working mode (0 - REMOTE, 1 - AUTOMATIC)
		/// </summary>
		/// <param name="mode"></param>
		/// <returns></returns>
		unsafe public int GetMode(byte* mode)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetMode");

            *mode = 0;
			try
			{
				if(!SendMsg(CMD_GETMODE)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				(*mode) = rcvBuff[2];
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets submode in AUTO mode (0 - AUTONORMAL, 1 - AUTOCAMERA, 2 - AUTOLOG)
		/// </summary>
		/// <param name="automode"></param>
		/// <returns></returns>
		public int SetAutoMode(byte automode)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetAutoMode");

			try
			{
				byte am;
				switch(automode)
				{
					case AUTONORMAL:	am = (byte)'N'; break;
					case AUTOCAMERA:	am = (byte)'C'; break;
					case AUTOLOG:		am = (byte)'L'; break;
					default:			am = (byte)'N'; break;
				}
				if(!SendMsg(CMD_SETAUTOMODE,am)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus())	return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}


		/// <summary>
		/// Sets real time clock
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <param name="dayOfWeek"></param>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		/// <param name="dayLight"></param>
		/// <returns></returns>
		public int SetRTC(byte year,byte month,byte day,byte dayOfWeek,byte hour,byte minute,byte second,byte dayLight)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetRTC");

			try
			{
				byte[] message = new byte[9]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETRTC; message[2] = second; message[3] = minute; message[4] = hour;
				message[5] = day; message[6] = (byte)((dayOfWeek << 5) + month); message[7] = year; message[8] = dayLight;

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets real time clock
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <param name="dayOfWeek"></param>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		/// <param name="dayLight"></param>
		/// <returns></returns>
		unsafe public int GetRTC(byte* year,byte* month,byte* day,byte* dayOfWeek,byte* hour,byte* minute,byte* second,byte* dayLight)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetRTC");

			try
			{
				if(!SendMsg(CMD_GETRTC)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;

				(*second) = rcvBuff[2]; (*minute) = rcvBuff[3]; (*hour) = rcvBuff[4]; (*day) = rcvBuff[5];
				(*dayOfWeek) = (byte)(rcvBuff[6] >> 5); (*month) = (byte)(rcvBuff[6] & 0x1F); (*year) = rcvBuff[7]; (*dayLight) = rcvBuff[8];				
				
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Starts uploading table (0 - CARDS, 1 - EVENTS, 2 - GROUPS, 5 - CARDS_2, 6 - CARDS7)
		/// </summary>
		/// <param name="size"></param>
		/// <param name="table"></param>
		/// <returns></returns>
		public int StartUploadTable(int size,byte table)
		{
            if (logging) AddByteSequenceToLog("CMD", "StartUploadTable" + " " + table.ToString());

			try
			{
				byte cmd = 0;
				switch(table) 
				{
					case CARDS:		cmd = CMD_UPLOADCARDS;  break;
					case EVENTS:	cmd = CMD_UPLOADEVENTS; break;
					case GROUPS:	cmd = CMD_UPLOADGROUPS; break;
                    case CARDS_2:   cmd = CMD_UPLOADCARDS_2; break;
                    case CARDS7:    cmd = CMD_UPLOAD7BYTEIDCARDS; break;
				}

				byte[] message = new byte[8]; message[0] = (byte)(message.Length);
				message[1] = cmd; message[2] = 0x00; message[3] = 0x00;
				message[4] = (byte)((size/0x1000000) & 0xFF); message[5] = (byte)((size/0x10000) & 0xFF);
				message[6] = (byte)((size/0x100) & 0xFF);     message[7] = (byte)(size & 0xFF);

				if(!SendMsg(message)) return 0;
				Thread.Sleep(2000);
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Uploads one package of a table (0 - CARDS, 1 - EVENTS, 2 - GROUPS, 5 - CARDS_2, 6 - CARDS7)
		/// </summary>
		/// <param name="package"></param>
		/// <param name="data"></param>
		/// <param name="table"></param>
		/// <returns></returns>
		public int NextUploadTable(int package,byte[] data,byte table)
		{
            if (logging) AddByteSequenceToLog("CMD", "NextUploadTable" + " " + table.ToString());

			try
			{
				byte cmd = 0;
				switch(table) 
				{
					case CARDS:		cmd = CMD_UPLOADCARDS;  break;
					case EVENTS:	cmd = CMD_UPLOADEVENTS; break;
					case GROUPS:	cmd = CMD_UPLOADGROUPS; break;
                    case CARDS_2:   cmd = CMD_UPLOADCARDS_2; break;
                    case CARDS7:    cmd = CMD_UPLOAD7BYTEIDCARDS; break;
				}

				byte[] message = new byte[4+PACKAGE_LENGTH]; message[0] = (byte)(message.Length);
				message[1] = cmd; message[2] = (byte)((package/0x100) & 0xFF); message[3] = (byte)(package & 0xFF);
				for(int i = 0; i < PACKAGE_LENGTH; i++) message[i+4] = data[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Starts downloading table (0 - CARDS, 1 - EVENTS, 2 - GROUPS, 3 - LOG, 5 - CARDS_2)
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public int StartDownloadTable(byte table)
		{
            if (logging) AddByteSequenceToLog("CMD", "StartDownloadTable" + " " + table.ToString());

			int size = 0;
			try
			{
				byte cmd = 0;
				switch(table) 
				{
					case CARDS:		cmd = CMD_DOWNLOADCARDS;  break;
					case EVENTS:	cmd = CMD_DOWNLOADEVENTS; break;
					case GROUPS:	cmd = CMD_DOWNLOADGROUPS; break;
					case LOG:		cmd = CMD_DOWNLOADLOG; break;
                    case CARDS_2:   cmd = CMD_DOWNLOADCARDS_2; break;
                    case CARDS7:    cmd = CMD_DOWNLOAD7BYTEIDCARDS; break;
				}

				byte[] message = new byte[4]; message[0] = (byte)(message.Length);
				message[1] = cmd; message[2] = 0x0; message[3] = 0x0;

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				size = rcvBuff[7] + 0x100*rcvBuff[6] + 0x10000*rcvBuff[5] + 0x1000000*rcvBuff[4];
				return size;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Starts downloading table (0 - CARDS, 1 - EVENTS, 2 - GROUPS, 3 - LOG, 5 - CARDS_2)
		/// </summary>
		/// <param name="size"></param>
		/// <param name="table"></param>
		/// <returns></returns>
		unsafe public int StartDownloadTable(int* size,byte table)
		{
            if (logging) AddByteSequenceToLog("CMD", "StartDownloadTable" + " " + table.ToString());

			*size = 0;
			try
			{
				byte cmd = 0;
				switch(table) 
				{
					case CARDS:		cmd = CMD_DOWNLOADCARDS;  break;
					case EVENTS:	cmd = CMD_DOWNLOADEVENTS; break;
					case GROUPS:	cmd = CMD_DOWNLOADGROUPS; break;
					case LOG:		cmd = CMD_DOWNLOADLOG; break;
                    case CARDS_2:   cmd = CMD_DOWNLOADCARDS_2; break;
                    case CARDS7:    cmd = CMD_DOWNLOAD7BYTEIDCARDS; break;
				}

				byte[] message = new byte[4]; message[0] = (byte)(message.Length);
				message[1] = cmd; message[2] = 0x0; message[3] = 0x0;

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				if (!((rcvBuff[2] == message[2]) && (rcvBuff[3] == message[3]))) return 0;
				*size = rcvBuff[7] + 0x100*rcvBuff[6] + 0x10000*rcvBuff[5] + 0x1000000*rcvBuff[4];
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Downloads one package of a table (0 - CARDS, 1 - EVENTS, 2 - GROUPS)
		/// </summary>
		/// <param name="package"></param>
		/// <param name="data"></param>
		/// <param name="table"></param>
		/// <returns></returns>
		public int NextDownloadTable(int package,byte[] data,byte table)
		{
            if (logging) AddByteSequenceToLog("CMD", "NextDownloadTable" + " " + table.ToString());

			try
			{
				byte cmd = 0;
				switch(table) 
				{
					case CARDS:		cmd = CMD_DOWNLOADCARDS; break;
					case EVENTS:	cmd = CMD_DOWNLOADEVENTS; break;
					case GROUPS:	cmd = CMD_DOWNLOADGROUPS; break;
					case LOG:		cmd = CMD_DOWNLOADLOG; break;
                    case CARDS_2:   cmd = CMD_DOWNLOADCARDS_2; break;
                    case CARDS7:    cmd = CMD_DOWNLOAD7BYTEIDCARDS; break;
				}

				byte[] message = new byte[7]; message[0] = (byte)(message.Length);
				message[1] = cmd; message[2] = (byte)((package/0x100) & 0xFF); message[3] = (byte)(package & 0xFF);

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				if (!((rcvBuff[2] == message[2]) && (rcvBuff[3] == message[3]))) return 0;
				for(int i = 4; i < PACKAGE_LENGTH+4; i++) data[i-4] = rcvBuff[i];
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Erases LOG table from the flash memory
		/// </summary>
		/// <returns></returns>
		public int EraseLog()
		{
            if (logging) AddByteSequenceToLog("CMD", "EraseLog");

			try
			{
				if(!SendMsg(CMD_ERASELOG)) return 0;
				//Thread.Sleep(15000);
                //if (!GetMsg()) return 0;
                //if (!DeviceStatus()) return 0;
                //return 1;

                for (int noPasses = 0; noPasses < 150; noPasses++)
                {
                    Thread.Sleep(100);
                    if (GetMsg()) if (DeviceStatus()) return 1;
                }
                return 0;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Erases the whole flash memory
		/// </summary>
		/// <returns></returns>
		public int EraseFlash()
		{
            if (logging) AddByteSequenceToLog("CMD", "EraseFlash");

			try
			{
				if(!SendMsg(CMD_ERASEFLASH)) return 0;
                //Thread.Sleep(15000);
                //if(!GetMsg()) return 0;
                //if(!DeviceStatus()) return 0;
                //return 1;
                
                for (int noPasses = 0; noPasses < 150; noPasses++)
                {
                    Thread.Sleep(100);
                    if (GetMsg()) if (DeviceStatus()) return 1;
                }
                return 0;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Returns percentage that LOG currently occupies
		/// </summary>
		/// <param name="percentage"></param>
		/// <returns></returns>
		unsafe public int GetLogOccupied(byte* percentage)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetLogOccupied");

			*percentage = 0;
			try
			{
				if(!SendMsg(CMD_GETLOGOCCUPIED)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				(*percentage) = rcvBuff[2];
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets device's serial number
		/// </summary>
		/// <param name="sernum"></param>
		/// <returns></returns>
		public int SetSerNum(string sernum)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetSerNum");

			try
			{
				byte[] message = new byte[13]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETSERNUM;
				for(int i = 0; i < 18; i++) message[i+2] = 0x00;
				for(int i = 0; i < Math.Min(sernum.Length,11); i++) message[i+2] = (Encoding.ASCII.GetBytes(sernum))[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets devices serial number
		/// </summary>
		/// <param name="sernum"></param>
		/// <returns></returns>
		public int GetSerNum(ref string sernum)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetSerNum");

			try
			{
				if(!SendMsg(CMD_GETSERNUM)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				sernum = Encoding.ASCII.GetString(rcvBuff,2,18);
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets duration of the digital output signal
		/// </summary>
		/// <param name="DO"></param>
		/// <param name="ms"></param>
		/// <returns></returns>
		public int SetDOTime(int DO,int ms)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetDOTime");

			try
			{
				byte[] message = new byte[11]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETDOTIME; message[2] = (byte)DO;
				string msstring = ms.ToString("D8");
				for(int i = 0; i < 8; i++) message[i+3] = (Encoding.ASCII.GetBytes(msstring))[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets duration of the digital output signal
		/// </summary>
		/// <param name="DO"></param>
		/// <returns></returns>
		public int GetDOTime(int DO)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetDOTime");

			try
			{
				int ms = 0;
				if(!SendMsg(CMD_GETDOTIME,(byte)DO)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				string msstring = Encoding.ASCII.GetString(rcvBuff,2,8);
				ms = Int32.Parse(msstring);
				return ms;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets duration of the digital output signal
		/// </summary>
		/// <param name="DO"></param>
		/// <param name="ms"></param>
		/// <returns></returns>
		unsafe public int GetDOTime(int DO,int* ms)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetDOTime");

			try
			{
				if(!SendMsg(CMD_GETDOTIME,(byte)DO)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				string msstring = Encoding.ASCII.GetString(rcvBuff,2,8);
				(*ms) = Int32.Parse(msstring);
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets serial number of a tag on a given antenna
		/// </summary>
		/// <param name="antenna"></param>
		/// <returns></returns>
		public ulong GetTagSerialNumber(int antenna)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetTagSerialNumber");

			try
			{
				byte cmd = 0;
				if (technologyType == null) technologyType = "MIFARE";
				if (technologyType.ToUpper() == "HITAG1")
				{
					cmd = CMD_GETHITAG1CARDSERIALNUMBER_ANT0;
				}
				else if (technologyType.ToUpper() == "HITAG2")
				{
					cmd = CMD_GETHITAG2CARDSERIALNUMBER_ANT0;
				}
				else
				{
					switch(antenna) 
					{
						case 0:	cmd = CMD_GETCARDSERIALNUMBER_ANT0; break;
						case 1:	cmd = CMD_GETCARDSERIALNUMBER_ANT1; break;
                        default: cmd = CMD_GETCARDSERIALNUMBER_ANT0; break;
					}
				}
				if(!SendMsg(cmd)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;

                if (rcvBuff[0] > 6) return Int64.MaxValue;   // tag serial number is longer then 4 bytes (advanced Mifare versions)

                return 
                    ((uint)rcvBuff[2] + (((uint)rcvBuff[3]) << 8) + (((uint)rcvBuff[4]) << 16) + (((uint)rcvBuff[5]) << 24));
			}
			catch(Exception)
			{
				return 0;
			}
		}

        /// <summary>
        /// Gets serial number of a tag on a given antenna
        /// </summary>
        /// <param name="antenna"></param>
        /// <returns></returns>
        public byte[] GetTagSerialNumberBytes(int antenna)
        {
            if (logging) AddByteSequenceToLog("CMD", "byte[] GetTagSerialNumber");

            try
            {
                byte cmd = 0;
                if (technologyType == null) technologyType = "MIFARE";
                if (technologyType.ToUpper() == "HITAG1")
                {
                    cmd = CMD_GETHITAG1CARDSERIALNUMBER_ANT0;
                }
                else if (technologyType.ToUpper() == "HITAG2")
                {
                    cmd = CMD_GETHITAG2CARDSERIALNUMBER_ANT0;
                }
                else
                {
                    switch (antenna)
                    {
                        case 0: cmd = CMD_GETCARDSERIALNUMBER_ANT0; break;
                        case 1: cmd = CMD_GETCARDSERIALNUMBER_ANT1; break;
                        default: cmd = CMD_GETCARDSERIALNUMBER_ANT0; break;
                    }
                }
                if (!SendMsg(cmd)) return null;
                if (!GetMsg()) return null;
                if (!DeviceStatus()) return null;

                byte[] b = new byte[rcvBuff[0] - 2];
                for (int i = 0; i < b.Length; i++) b[i] = rcvBuff[i + 2];

                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }

		/// <summary>
		/// Reads data from a given block
		/// </summary>
		/// <param name="antenna"></param>
		/// <param name="block"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public int ReadBlock(int antenna,int block,byte[] data)
		{
            if (logging) AddByteSequenceToLog("CMD", "ReadBlock");

			try
			{
				byte cmd = 0;
				switch(antenna) 
				{
					case 0:	cmd = CMD_READBLOCK_ANT0; break;
					case 1:	cmd = CMD_READBLOCK_ANT1; break;
				}

				byte[] message = new byte[4]; message[0] = (byte)(message.Length); message[1] = cmd;
				message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;

				for(int i = 2; i < PACKAGE_LENGTH+2; i++) data[i-2] = rcvBuff[i];
				return 1;
			}
			catch(Exception)
			{
				return 0;
			}
		}

        /// <summary>
        /// Reads data from a given block Acta Android
        /// </summary>
        /// <param name="antenna"></param>
        /// <param name="block"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ReadBlockAndroidCard(int antenna, int block, byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "ReadBlockAndroidCard");

            try
            {
                byte cmd = 0;
                switch (antenna)
                {
                    case 0: cmd = CMD_READBLOCK_ANT0; break;
                    case 1: cmd = CMD_READBLOCK_ANT1; break;
                }

                byte[] message = new byte[4]; message[0] = (byte)(message.Length); message[1] = cmd;
                message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 2; i < data.Length + 2; i++) data[i - 2] = rcvBuff[i];
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }




		/// <summary>
        /// Reads data from a given block that is protected with 6 byte password key A. Desktop device only.
		/// </summary>
		/// <param name="block"></param>
		/// <param name="password"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public int ReadProtectedBlock(int block,byte[] password,byte[] data)
		{
            if (logging) AddByteSequenceToLog("CMD", "ReadProtectedBlock");

			try
			{
				if(password.Length != 6) return 0;
				byte[] message = new byte[10]; message[0] = (byte)(message.Length); message[1] = CMD_READPROTECTEDBLOCKA_ANT0;
				message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
				for(int i = 0; i < 6; i++) message[4+i] = password[i];
				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;

				for(int i = 2; i < PACKAGE_LENGTH+2; i++) data[i-2] = rcvBuff[i];
				return 1;
			}
			catch(Exception)
			{
				return 0;
			}
		}

        /// <summary>
        /// Reads data from a given block that is protected with 6 byte password key B. Desktop device only.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ReadProtectedBlockB(int block, byte[] password, byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "ReadProtectedBlockB");

            try
            {
                if (password.Length != 6) return 0;
                byte[] message = new byte[10]; message[0] = (byte)(message.Length); message[1] = CMD_READPROTECTEDBLOCKB_ANT0;
                message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
                for (int i = 0; i < 6; i++) message[4 + i] = password[i];
                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 2; i < PACKAGE_LENGTH + 2; i++) data[i - 2] = rcvBuff[i];
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Reads data from a given block that is protected with 6 byte password key from given antenna (0 or 1) with given password key type (0=A or 1=B)
        /// </summary>
        /// <param name="block"></param>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ReadProtectedBlock(int antenna, int passwordAorB, int block, byte[] password, byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "ReadProtectedBlockGeneral");

            try
            {
                byte cmd;
                switch (antenna)
                {
                    case 0: if (passwordAorB == 0) cmd = CMD_READPROTECTEDBLOCKA_ANT0; else cmd = CMD_READPROTECTEDBLOCKB_ANT0; break;
                    case 1: if (passwordAorB == 0) cmd = CMD_READPROTECTEDBLOCKA_ANT1; else cmd = CMD_READPROTECTEDBLOCKB_ANT1; break;
                    default: cmd = CMD_GETCARDSERIALNUMBER_ANT0; break;
                }

                if (password.Length != 6) return 0;
                byte[] message = new byte[10]; message[0] = (byte)(message.Length); message[1] = cmd;
                message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
                for (int i = 0; i < 6; i++) message[4 + i] = password[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 2; i < PACKAGE_LENGTH + 2; i++) data[i - 2] = rcvBuff[i];
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }


		/// <summary>
		/// Writes data into a given block
		/// </summary>
		/// <param name="antenna"></param>
		/// <param name="block"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public int WriteBlock(int antenna,int block,byte[] data)
		{
            if (logging) AddByteSequenceToLog("CMD", "WriteBlock");

			try
			{
				byte cmd = 0;
				switch(antenna) 
				{
					case 0:	cmd = CMD_WRITEBLOCK_ANT0; break;
					case 1:	cmd = CMD_WRITEBLOCK_ANT1; break;
				}

				byte[] message = new byte[PACKAGE_LENGTH+4]; message[0] = (byte)(message.Length); message[1] = cmd;
				message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
				for(int i = 0; i < PACKAGE_LENGTH; i++) message[i+4] = data[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception)
			{
				return 0;
			}
		}

		/// <summary>
		/// Writes data into a given block that is protected with 6 byte password key A. Desktop device only.
		/// </summary>
		/// <param name="block"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public int WriteProtectedBlock(int block,byte[] password,byte[] data)
		{
            if (logging) AddByteSequenceToLog("CMD", "WriteProtectedBlock");

			try
			{
				if(password.Length != 6) return 0;			
				byte[] message = new byte[PACKAGE_LENGTH+10]; message[0] = (byte)(message.Length); message[1] = CMD_WRITEPROTECTEDBLOCKA_ANT0;
				message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
				for(int i = 0; i < PACKAGE_LENGTH; i++) message[i+4] = data[i];
				for(int i = 0; i < 6; i++) message[4+PACKAGE_LENGTH+i] = password[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception)
			{
				return 0;
			}
		}

        /// <summary>
        /// Writes data into a given block that is protected with 6 byte password key B. Desktop device only.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int WriteProtectedBlockB(int block, byte[] password, byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "WriteProtectedBlockB");

            try
            {
                if (password.Length != 6) return 0;
                byte[] message = new byte[PACKAGE_LENGTH + 10]; message[0] = (byte)(message.Length); message[1] = CMD_WRITEPROTECTEDBLOCKB_ANT0;
                message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
                for (int i = 0; i < PACKAGE_LENGTH; i++) message[i + 4] = data[i];
                for (int i = 0; i < 6; i++) message[4 + PACKAGE_LENGTH + i] = password[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Writes data into a given block that is protected with 6 byte password key using given antenna (0 or 1) with given password key type (0=A or 1=B)
        /// </summary>
        /// <param name="block"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int WriteProtectedBlock(int antenna, int passwordAorB, int block, byte[] password, byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "WriteProtectedBlockGeneral");

            try
            {
                byte cmd;
                switch (antenna)
                {
                    case 0: if (passwordAorB == 0) cmd = CMD_WRITEPROTECTEDBLOCKA_ANT0; else cmd = CMD_WRITEPROTECTEDBLOCKB_ANT0; break;
                    case 1: if (passwordAorB == 0) cmd = CMD_WRITEPROTECTEDBLOCKA_ANT1; else cmd = CMD_WRITEPROTECTEDBLOCKB_ANT1; break;
                    default: cmd = CMD_WRITEPROTECTEDBLOCKA_ANT0; break;
                }

                if (password.Length != 6) return 0;
                byte[] message = new byte[PACKAGE_LENGTH + 10]; message[0] = (byte)(message.Length); message[1] = cmd;
                message[2] = (byte)((block & 0x0000FF00) >> 8); message[3] = (byte)(block & 0x000000FF);
                for (int i = 0; i < PACKAGE_LENGTH; i++) message[i + 4] = data[i];
                for (int i = 0; i < 6; i++) message[4 + PACKAGE_LENGTH + i] = password[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

		/// <summary>
		/// Set new password into a given block that is protected with 6 byte password key A. Desktop device only.
		/// </summary>
		/// <param name="block"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public int SetSectorPassword(int sector,byte[] oldPassword,byte[] newPassword)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetSectorPassword");

			try
			{
				if((oldPassword.Length != 6) || (newPassword.Length != 6)) return 0;			
				int trailerBlock = sector*4+3;

				byte[] trailerBlockContent = new byte[PACKAGE_LENGTH];
				int status = ReadProtectedBlock(trailerBlock,oldPassword,trailerBlockContent);
				if(status == 0) return 0;
				for(int i = 0; i < 6; i++) trailerBlockContent[i] = newPassword[i];

				byte[] message = new byte[PACKAGE_LENGTH+10]; message[0] = (byte)(message.Length); message[1] = CMD_WRITESECTORPASSWORDA_ANT0;
				message[2] = (byte)((trailerBlock & 0x0000FF00) >> 8); message[3] = (byte)(trailerBlock & 0x000000FF);
				for(int i = 0; i < PACKAGE_LENGTH; i++) message[i+4] = trailerBlockContent[i];
				for(int i = 0; i < 6; i++) message[4+PACKAGE_LENGTH+i] = oldPassword[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception)
			{
				return 0;
			}
		}

        /// <summary>
        /// Set new password into a given block that is protected with 6 byte password key B. Desktop device only.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SetSectorPasswordB(int sector, byte[] oldPassword, byte[] newPassword)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetSectorPasswordB");

            try
            {
                if ((oldPassword.Length != 6) || (newPassword.Length != 6)) return 0;
                int trailerBlock = sector * 4 + 3;

                byte[] trailerBlockContent = new byte[PACKAGE_LENGTH];
                int status = ReadProtectedBlock(trailerBlock, oldPassword, trailerBlockContent);
                if (status == 0) return 0;
                for (int i = 0; i < 6; i++) trailerBlockContent[i] = newPassword[i];

                byte[] message = new byte[PACKAGE_LENGTH + 10]; message[0] = (byte)(message.Length); message[1] = CMD_WRITESECTORPASSWORDB_ANT0;
                message[2] = (byte)((trailerBlock & 0x0000FF00) >> 8); message[3] = (byte)(trailerBlock & 0x000000FF);
                for (int i = 0; i < PACKAGE_LENGTH; i++) message[i + 4] = trailerBlockContent[i];
                for (int i = 0; i < 6; i++) message[4 + PACKAGE_LENGTH + i] = oldPassword[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// Set new password into a given block that is protected with 6 byte password key A. Desktop device only.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SetTrailerBlockA(int sector, byte[] oldPassword, byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetTrailerBlockA");

            try
            {
                if ((oldPassword.Length != 6) || (data.Length != 16)) return 0;
                int trailerBlock = sector * 4 + 3;

                byte[] message = new byte[PACKAGE_LENGTH + 10]; message[0] = (byte)(message.Length); message[1] = CMD_WRITESECTORPASSWORDA_ANT0;
                message[2] = (byte)((trailerBlock & 0x0000FF00) >> 8); message[3] = (byte)(trailerBlock & 0x000000FF);
                for (int i = 0; i < PACKAGE_LENGTH; i++) message[i + 4] = data[i];
                for (int i = 0; i < 6; i++) message[4 + PACKAGE_LENGTH + i] = oldPassword[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Set new password into a given block that is protected with 6 byte password key B. Desktop device only.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SetTrailerBlockB(int sector, byte[] oldPassword, byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetTrailerBlockB");

            try
            {
                if ((oldPassword.Length != 6) || (data.Length != 16)) return 0;
                int trailerBlock = sector * 4 + 3;

                byte[] message = new byte[PACKAGE_LENGTH + 10]; message[0] = (byte)(message.Length); message[1] = CMD_WRITESECTORPASSWORDB_ANT0;
                message[2] = (byte)((trailerBlock & 0x0000FF00) >> 8); message[3] = (byte)(trailerBlock & 0x000000FF);
                for (int i = 0; i < PACKAGE_LENGTH; i++) message[i + 4] = data[i];
                for (int i = 0; i < 6; i++) message[4 + PACKAGE_LENGTH + i] = oldPassword[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #region Self Generated Password (SGP) init/read/write methods

        /// <summary>
        /// Calculates new password based on card UID (self generated password)
        /// </summary>
        /// <returns>byte[] password</returns>
        public byte[] SGP()
        {
            byte[] uidBytes = GetTagSerialNumberBytes(0); if (uidBytes == null) return null;

            byte[] sgp = new byte[6];
            sgp[0] = (byte)(uidBytes[0] * uidBytes[1]); sgp[1] = (byte)(sgp[0] * uidBytes[2]);
            sgp[2] = (byte)(uidBytes[2] * uidBytes[3]); sgp[3] = (byte)(sgp[2] * uidBytes[0]);
            sgp[4] = (byte)(uidBytes[1] * uidBytes[2]); sgp[5] = (byte)(sgp[3] * uidBytes[3]);

            return sgp;
        }

        /// <summary>
        /// Sets self generated password to a given card sector
        /// </summary>
        /// <returns>int status</returns>
        public int SGPInitCard(int sector)
        {
            try
            {
                byte[] defaultPassword = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                byte[] sgpPassword = SGP();
                int status = SetSectorPassword(sector, defaultPassword, sgpPassword);
                if (status == 1) return 1;      // SGP password successfully set
                else
                {
                    status = SetSectorPassword(sector, sgpPassword, sgpPassword);
                    if (status == 1) return 2;  // SGP password had been already set
                    else return 0;              // unsuccessful
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// Reads a given block using self generated password
        /// </summary>
        /// <returns>int status</returns>
        public int SGPReadProtectedBlock(int block, byte[] data)
        {
            try
            {
                byte[] sgp = SGP(); Thread.Sleep(10);
                int status = ReadProtectedBlock(block, sgp, data);
                return status;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Writes a given block using self generated password
        /// </summary>
        /// <returns>int status</returns>
        public int SGPWriteProtectedBlock(int block, byte[] data)
        {
            try {
                byte[] sgp = SGP(); Thread.Sleep(10);
                int status = WriteProtectedBlock(block, sgp, data);
                return status;
            }
            catch { return 0; }
        }

        #endregion

        /// <summary>
		/// Gets product technology info
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public int GetProductInfo(byte[] data)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetProductInfo");

			try
			{
				if(!SendMsg(CMD_GETPRODUCTINFO)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;

				for(int i = 2; i < PACKAGE_LENGTH+2; i++) data[i-2] = rcvBuff[i];
				return 1;
			}
			catch(Exception)
			{
				return 0;
			}
		}

		/// <summary>
		/// Sets state of a given relay
		/// </summary>
		/// <param name="relayNum"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		public int SetRelay(byte relayNum,byte state)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetRelay");

			try
			{
				byte[] message = new byte[4]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETRELAY; message[2] = relayNum; message[3] = state;

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets signals on a given antenna
		/// </summary>
		/// <param name="antenna"></param>
		/// <param name="green"></param>
		/// <param name="red"></param>
		/// <param name="buzzer"></param>
		/// <returns></returns>
		public int SetSignals(byte antenna, byte green, byte red, byte buzzer)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetSignals");

			try
			{
				byte[] message = new byte[6]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETSIGNALS; message[2] = antenna; message[3] = green;
				message[4] = red; message[5] = buzzer;

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets states of device's digital inputs
		/// </summary>
		/// <param name="di0"></param>
		/// <param name="di1"></param>
		/// <param name="di2"></param>
		/// <param name="di3"></param>
		/// <returns></returns>
		unsafe public int GetDigitalInputs(byte* di0,byte* di1,byte* di2,byte* di3)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetDigitalInputs");

			try
			{
				if(!SendMsg(CMD_GETDIGITALINPUTS)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;

				(*di0) = rcvBuff[2]; (*di1) = rcvBuff[3]; (*di2) = rcvBuff[4]; (*di3) = rcvBuff[5];
				
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets the timeout after last command in the remote mode when device goes automaticaly to automatic mode
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public int SetRemoteTimeout(int ms)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetRemoteTimeout");

			try
			{
				ms = Math.Max(ms,MIN_REMOTE_TIMEOUT);
				byte[] message = new byte[10]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETREMOTETIMEOUT;
				string msstring = ms.ToString("D8");
				for(int i = 0; i < 8; i++) message[i+2] = (Encoding.ASCII.GetBytes(msstring))[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

        /// <summary>
        /// Starts differential download of the LOG table
        /// </summary>
        /// <returns></returns>
        public int StartDiffDownloadLogTable()
        {
            if (logging) AddByteSequenceToLog("CMD", "StartDiffDownloadLogTable");

            int size = 0;
            byte cmd = CMD_DIFFDOWNLOADLOG;
            if (technologyType == null) technologyType = "MIFARE";
            if ((technologyType.ToUpper() == "HITAG1") || (technologyType.ToUpper() == "HITAG2")) cmd = CMD_DIFFHITAGDOWNLOADLOG;
            try
            {
                byte[] message = new byte[4]; message[0] = (byte)(message.Length);
                message[1] = cmd; message[2] = 0x0; message[3] = 0x0;

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                size = rcvBuff[7] + 0x100 * rcvBuff[6] + 0x10000 * rcvBuff[5] + 0x1000000 * rcvBuff[4];
                return size;
            }
            catch (Exception) { return 0; }
        }


		/// <summary>
		/// Starts differential download of the LOG table
		/// </summary>
		/// <returns></returns>
		unsafe public int StartDiffDownloadLogTable(int* size)
		{
            if (logging) AddByteSequenceToLog("CMD", "StartDiffDownloadLogTable");

			*size = 0;
			byte cmd = CMD_DIFFDOWNLOADLOG;
			if (technologyType == null) technologyType = "MIFARE";
			if ((technologyType.ToUpper() == "HITAG1") || (technologyType.ToUpper() == "HITAG2")) cmd = CMD_DIFFHITAGDOWNLOADLOG;
			try
			{
				byte[] message = new byte[4]; message[0] = (byte)(message.Length);
				message[1] = cmd; message[2] = 0x0; message[3] = 0x0;

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				*size = rcvBuff[7] + 0x100*rcvBuff[6] + 0x10000*rcvBuff[5] + 0x1000000*rcvBuff[4];
				return 1;
			}
			catch(Exception) { return 0; }		
		}

		/// <summary>
		/// Downloads one package of the LOG table in a process of differential download
		/// </summary>
		/// <param name="package"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public int NextDiffDownloadLogTable(int package,byte[] data)
		{
            if (logging) AddByteSequenceToLog("CMD", "NextDiffDownloadLogTable");

			byte cmd = CMD_DIFFDOWNLOADLOG;
			if (technologyType == null) technologyType = "MIFARE";
			if ((technologyType.ToUpper() == "HITAG1") || (technologyType.ToUpper() == "HITAG2")) cmd = CMD_DIFFHITAGDOWNLOADLOG;
			try
			{
				byte[] message = new byte[4]; message[0] = (byte)(message.Length);
				message[1] = cmd; message[2] = (byte)((package/0x100) & 0xFF); message[3] = (byte)(package & 0xFF);

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				for(int i = 4; i < PACKAGE_LENGTH+4; i++) data[i-4] = rcvBuff[i];
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets RFID device's tamper bit to the given state (0, 1)
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public int SetTamper(int t)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetTamper");

			try
			{
				byte tb = (byte)t;
				if(!SendMsg(CMD_SETTAMPER,tb)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus())	return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Returns RFID device's tamper bit state (0, 1)
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		unsafe public int GetTamper(int* t)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetTamper");

			try
			{
				if(!SendMsg(CMD_GETTAMPER)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				(*t) = (int)rcvBuff[2];
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets RFID device's antennas type (0 - signal/pause, 1 - RS232 data)
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public int SetAntennaTypes(byte ant0type, byte ant1type)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetAntennaTypes");

			try
			{
				byte[] message = new byte[4]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETANTTYPE; message[2] = ant0type; message[3] = ant1type;

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets RFID device's antenna types (0 - signal/pause, 1 - RS232 data)
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		unsafe public int GetAntennaTypes(byte* ant0type, byte* ant1type)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetAntennaTypes");

			*ant0type = *ant1type = 0xFF;
			try
			{
				if(!SendMsg(CMD_GETANTTYPE)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				(*ant0type) = rcvBuff[2];
				(*ant1type) = rcvBuff[3];
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Resets device
		/// </summary>
		/// <returns></returns>
		public int Reset()
		{
            if (logging) AddByteSequenceToLog("CMD", "Reset");

			try
			{
				if(!SendMsg(CMD_RESETDEVICE)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Hardware reset of the device over XPort
		/// </summary>
		/// <returns></returns>
		public int HardReset()
		{
            if (logging) AddByteSequenceToLog("CMD", "HardReset");

			try
			{
				IRFIDFactory.GetInterface.HardReset();
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Sets Lantronix digital output DO (0-2) to state (0/1)
		/// </summary>
		/// <returns></returns>
		public int SetLantronixDO(int DO, int state)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetLantronixDO");

			try
			{
				IRFIDFactory.GetInterface.SetLantronixDO(DO,state);
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets state (0/1) of Lantronix digital output DO (0-2)
		/// </summary>
		/// <returns></returns>
		public int GetLantronixDO(int DO)
		{
            if (logging) AddByteSequenceToLog("CMD", "GetLantronixDO");

			try
			{
				return (IRFIDFactory.GetInterface.GetLantronixDO(DO));
			}
			catch(Exception) { return 0; }
		}

        /// <summary>
        /// Gets product info field
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetProductInfoField(byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetProductInfoField");

            try
            {
                if (!SendMsg(CMD_GETPRODUCTINFOFIELD)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 2; i < 9 + 2; i++) data[i - 2] = rcvBuff[i];
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
		/// Write one record to LOG table
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public int WriteLogRecord(byte[] data)
		{
            if (logging) AddByteSequenceToLog("CMD", "WriteLogRecord");

			try
			{
				byte cmd = CMD_WRITELOGRECORD;
				if (technologyType == null) technologyType = "MIFARE";
				if ((technologyType.ToUpper() == "HITAG1") || (technologyType.ToUpper() == "HITAG2")) cmd = CMD_WRITEHITAGLOGRECORD;

				byte[] message = new byte[2+14]; message[0] = (byte)(message.Length);
				message[1] = cmd; 
				for(int i = 0; i < 14; i++) message[i+2] = data[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

        /// <summary>
        /// Sets RFID device's camera trigger to the given state (0, 1, 2, 3 - none, ant0, ant1, ant0 & ant1)
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public int SetCameraTrigger(int trigger)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetCameraTrigger");

            try
            {
                byte triggerb = (byte)trigger;
                if (!SendMsg(CMD_SETCAMERATRIGGER, triggerb)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Returns RFID device's camera trigger state (0, 1, 2, 3 - none, ant0, ant1, ant0 & ant1)
        /// </summary>
        /// <returns></returns>
        public int GetCameraTrigger()
        {
            if (logging) AddByteSequenceToLog("CMD", "GetCameraTrigger");

            try
            {
                if (!SendMsg(CMD_GETCAMERATRIGGER)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return ((int)rcvBuff[2]);
            }
            catch (Exception) { return -1; }
        }

        #region MFRC specific methods

        /// <summary>
        /// Reads entire EEprom
        /// </summary>
        /// <returns></returns>
        public int ReadEEprom(byte[] data)
        {
            if (logging) AddByteSequenceToLog("CMD", "ReadEEprom");

            try
            {
                byte[] message = new byte[4]; message[0] = (byte)(message.Length);
                message[1] = CMD_READEEPROMREG; message[2] = 0x10; message[3] = 0x20;

                if (!SendMsg(message)) return 0;
                Thread.Sleep(100);
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                if (rcvBuff[0] != (0x20 + 2)) return 0;
                for (int i = 2; i < 0x20 + 2; i++) data[i - 2] = rcvBuff[i];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Reads register from EEprom
        /// </summary>
        /// <returns></returns>
        unsafe public int ReadEEpromReg(byte register, byte* value)
        {
            if (logging) AddByteSequenceToLog("CMD", "ReadEEpromReg");

            try
            {
                byte[] message = new byte[4]; message[0] = (byte)(message.Length);
                message[1] = CMD_READEEPROMREG; message[2] = register; message[3] = 0x01;

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*value) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Writes EEprom register
        /// </summary>
        /// <returns></returns>
        public int WriteEEpromReg(byte register, byte value)
        {
            if (logging) AddByteSequenceToLog("CMD", "WriteEEpromReg");

            try
            {
                if (register < 0x11) return 0;

                byte maskedValue = (byte)(value & MaskRegister(register));

                byte[] message = new byte[4]; message[0] = (byte)(message.Length);
                message[1] = CMD_WRITEEEPROMREG; message[2] = register; message[3] = maskedValue;

                if (!SendMsg(message)) return 0;
                Thread.Sleep(100);
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Reads register
        /// </summary>
        /// <returns></returns>
        unsafe public int ReadRegister(byte register, byte* value)
        {
            if (logging) AddByteSequenceToLog("CMD", "ReadRegister");

            try
            {
                if (!SendMsg(CMD_READREGISTER, register)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*value) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Writes register
        /// </summary>
        /// <returns></returns>
        public int WriteRegister(byte register, byte value)
        {
            if (logging) AddByteSequenceToLog("CMD", "WriteRegister");

            try
            {
                if (register < 0x11) return 0;

                byte maskedValue = (byte)(value & MaskRegister(register));

                byte[] message = new byte[4]; message[0] = (byte)(message.Length);
                message[1] = CMD_WRITEREGISTER; message[2] = register; message[3] = maskedValue;

                if (!SendMsg(message)) return 0;
                Thread.Sleep(100);
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        byte MaskRegister(byte register)
        {
            byte mask;
            switch (register)
            {
                case 0x11: mask = 0x7F; break;
                case 0x12: mask = 0x3F; break;
                case 0x13: mask = 0x3F; break;
                case 0x14: mask = 0xBF; break;
                case 0x1A: mask = 0x7F; break;
                case 0x1E: mask = 0xC3; break;
                case 0x1F: mask = 0xDF; break;
                case 0x22: mask = 0x3F; break;
                case 0x26: mask = 0x17; break;
                case 0x29: mask = 0x3F; break;
                case 0x2A: mask = 0x3F; break;
                case 0x2B: mask = 0x0F; break;
                case 0x2D: mask = 0x03; break;

                default: mask = 0xFF; break;
            }
            return mask;
        }

        #endregion

        #region specific Hitag desktop device methods

        public string GetHitag1DesktopTagSerialNumber()
		{
            if (logging) AddByteSequenceToLog("CMD", "GetHitag1DesktopTagSerialNumber");

			if (!HitagHFReset()) return "";
			byte[] message = new byte[2]; message[0] = 0x02; message[1] = 0x47;
			if(!SendMsg(message)) return "";
			if(!GetMsg()) return "";
			if(!DeviceStatus()) return "";
			uint snr = (uint)((1-2*rcvBuff[6])*(rcvBuff[2]+0x100*rcvBuff[3]+0x10000*rcvBuff[4]+0x1000000*rcvBuff[5]));
			return snr.ToString();
		}

		public string GetHitag2DesktopTagSerialNumber()
		{
            if (logging) AddByteSequenceToLog("CMD", "GetHitag2DesktopTagSerialNumber");

			if (!HitagHFReset()) return "";
			byte[] message = new byte[3]; message[0] = 0x03; message[1] = 0x80; message[2] = 0x0;
			if(!SendMsg(message)) return "";
			if(!GetMsg()) return "";
			if(!DeviceStatus()) return "";
			uint snr = (uint)(rcvBuff[2]+0x100*rcvBuff[3]+0x10000*rcvBuff[4]+0x1000000*rcvBuff[5]);
			return snr.ToString();
		}

		private bool HitagHFReset()
		{
            if (logging) AddByteSequenceToLog("CMD", "HitagHFReset");

			byte[] message = new byte[2]; message[0] = 0x02; message[1] = 0x68;
			if(!SendMsg(message)) return false;
			if(!GetMsg()) return false;
			if(!DeviceStatus()) return false;
			return true;
		}

        /// <summary>
        /// Returns RFID device's GSM mode (0 - IP_OR_SERIAL, 1 - GSM)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        unsafe public int GetGSMMode(byte* gsmmode)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetGSMMode");

            *gsmmode = 0;
            try
            {
                if (!SendMsg(CMD_GETGSMMODE)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*gsmmode) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets RFID device's GSM mode (0 - IP_OR_SERIAL, 1 - GSM)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int SetGSMMode(byte gsmmode)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetGSMMode");

            try
            {
                if (!SendMsg(CMD_SETGSMMODE, gsmmode)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

		#endregion

		#region maintenance methods specific for ramp device

		/// <summary>
		/// Sets duration of the relay hold time
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public int SetRelayHoldTime(int ms)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetRelayHoldTime");

			try
			{
				byte[] message = new byte[10]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETRELAYHOLDTIME;
				string msstring = ms.ToString("D8");
				for(int i = 0; i < 8; i++) message[i+2] = (Encoding.ASCII.GetBytes(msstring))[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets duration of the the relay hold time
		/// </summary>
		/// <returns></returns>
		public int GetRelayHoldTime()
		{
            if (logging) AddByteSequenceToLog("CMD", "GetRelayHoldTime");

			try
			{
				int ms = 0;
				if(!SendMsg(CMD_GETRELAYHOLDTIME)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				string msstring = Encoding.ASCII.GetString(rcvBuff,2,8);
				ms = Int32.Parse(msstring);
				return ms;
			}
			catch(Exception) { return 0; }
		}		

		/// <summary>
		/// Sets duration of the after pass time
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public int SetAfterPassTime(int ms)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetAfterPassTime");

			try
			{
				byte[] message = new byte[10]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETAFTERPASSTIME;
				string msstring = ms.ToString("D8");
				for(int i = 0; i < 8; i++) message[i+2] = (Encoding.ASCII.GetBytes(msstring))[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets duration of the after pass time
		/// </summary>
		/// <returns></returns>
		public int GetAfterPassTime()
		{
            if (logging) AddByteSequenceToLog("CMD", "GetAfterPassTime");

			try
			{
				int ms = 0;
				if(!SendMsg(CMD_GETAFTERPASSTIME)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				string msstring = Encoding.ASCII.GetString(rcvBuff,2,8);
				ms = Int32.Parse(msstring);
				return ms;
			}
			catch(Exception) { return 0; }
		}		

		/// <summary>
		/// Sets duration of the error declare time
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public int SetErrorDeclareTime(int ms)
		{
            if (logging) AddByteSequenceToLog("CMD", "SetErrorDeclareTime");

			try
			{
				byte[] message = new byte[10]; message[0] = (byte)(message.Length);
				message[1] = CMD_SETERRORDECLARETIME;
				string msstring = ms.ToString("D8");
				for(int i = 0; i < 8; i++) message[i+2] = (Encoding.ASCII.GetBytes(msstring))[i];

				if(!SendMsg(message)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				return 1;
			}
			catch(Exception) { return 0; }
		}

		/// <summary>
		/// Gets duration of the error declare time
		/// </summary>
		/// <returns></returns>
		public int GetErrorDeclareTime()
		{
            if (logging) AddByteSequenceToLog("CMD", "GetErrorDeclareTime");

			try
			{
				int ms = 0;
				if(!SendMsg(CMD_GETERRORDECLARETIME)) return 0;
				if(!GetMsg()) return 0;
				if(!DeviceStatus()) return 0;
				string msstring = Encoding.ASCII.GetString(rcvBuff,2,8);
				ms = Int32.Parse(msstring);
				return ms;
			}
			catch(Exception) { return 0; }
		}		

		/// <summary>
		/// Gets firmware version
		/// </summary>
		/// <returns></returns>
		public string GetFirmwareVer()
		{
            if (logging) AddByteSequenceToLog("CMD", "GetFirmwareVer");

			try
			{
				if(!SendMsg(CMD_GETFIRMWAREVER)) return "";
				if(!GetMsg()) return "";
				if(!DeviceStatus()) return "";
				string firmwarever = Encoding.ASCII.GetString(rcvBuff,2,rcvBuff[0]-3);
				return firmwarever;
			}
			catch(Exception) { return ""; }
		}

        /// <summary>
        /// Sets diff log pointer
        /// </summary>
        /// <returns></returns>
        public int SetDiffLogPointer(int newPointer)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetDiffLogPointer");

            if (newPointer == 0) return (ResetDiffLogPointer()); else if (newPointer > (MAX_LOG_RECS * PACKAGE_LENGTH)) return 0;

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length);
                message[1] = CMD_SETDIFFLOGPOINTER;
                message[2] = (byte)((newPointer / 0x1000000) & 0xFF); message[3] = (byte)((newPointer / 0x10000) & 0xFF);
                message[4] = (byte)((newPointer / 0x100) & 0xFF); message[5] = (byte)(newPointer & 0xFF);
                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets diff log pointer
        /// </summary>
        /// <returns></returns>
        public int GetDiffLogPointer()
        {
            if (logging) AddByteSequenceToLog("CMD", "GetDiffLogPointer");

            try
            {
                int address = 0;
                if (!SendMsg(CMD_GETDIFFLOGPOINTER)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                address = (rcvBuff[2] << 24) + (rcvBuff[3] << 16) + (rcvBuff[4] << 8) + (rcvBuff[5] << 0);
                return address;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets diff log pointer to 0
        /// </summary>
        /// <returns></returns>
        public int ResetDiffLogPointer()
        {
            if (logging) AddByteSequenceToLog("CMD", "ResetDiffLogPointer");

            try
            {
                if (!SendMsg(CMD_RESETDIFFLOGPOINTER)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return -99; }
        }

        /// <summary>
        /// Sets RFID device's type (0 - GATE (standard terminal), 1 - BARRIER (tripod))
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public int SetDeviceType(byte deviceType)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetDeviceType");

            try
            {
                if (!SendMsg(CMD_SETDEVICETYPE, deviceType)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

		#endregion

        #region specific Cortex reader methods

        /// <summary>
        /// Returns RFID device's regime (0 - STANDALONE, 1 - REMOTE)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        unsafe public int GetRegime(byte* regime)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetRegime");

            *regime = 0;
            try
            {
                if (!SendMsg(CMD_GETREGIME)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*regime) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets RFID device's regime (0 - STANDALONE, 1 - REMOTE)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int SetRegime(byte regime)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetRegime");

            try
            {
                if (!SendMsg(CMD_SETREGIME, regime)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Returns RFID device's emergency state (0 - RESET 1 - SET)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        unsafe public int GetEmergency(byte* emergency)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetEmergency");

            *emergency = 0;
            try
            {
                if (!SendMsg(CMD_GETEMERGENCY)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*emergency) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets RFID device's emergency state (0 - RESET 1 - SET)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int SetEmergency(byte emergency)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetEmergency");

            try
            {
                if (!SendMsg(CMD_SETEMERGENCY, emergency)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets IP parameters (4 bytes IP address, 4 bytes default gateway, 4 bytes subnet mask,
        /// 4 bytes preferred DNS server, 4 bytes alternate DNS server, 1 byte DHCP enabled)
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="gateway"></param>
        /// <param name="subnetmask"></param>
        /// <param name="dns1"></param>
        /// <param name="dns2"></param>
        /// <returns></returns>
        unsafe public int GetIPparameters(byte[] ipaddress, byte[] gateway, byte[] subnetmask, byte[] dns1, byte[] dns2, byte* dhcpenabled)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetIPparameters");

            try
            {
                if (!SendMsg(CMD_GETIPPARAMETERS)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) ipaddress[i] = rcvBuff[i + 2];
                for (int i = 0; i < 4; i++) gateway[i] = rcvBuff[i + 6];
                for (int i = 0; i < 4; i++) subnetmask[i] = rcvBuff[i + 10];
                for (int i = 0; i < 4; i++) dns1[i] = rcvBuff[i + 14];
                for (int i = 0; i < 4; i++) dns2[i] = rcvBuff[i + 18];
                *dhcpenabled = rcvBuff[22];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets IP parameters (4 bytes IP address, 4 bytes default gateway, 4 bytes subnet mask,
        /// 4 bytes preferred DNS server, 4 bytes alternate DNS server, 1 byte DHCP enabled)
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="gateway"></param>
        /// <param name="subnetmask"></param>
        /// <param name="dns1"></param>
        /// <param name="dns2"></param>
        /// <returns></returns>
        public int SetIPparameters(byte[] ipaddress, byte[] gateway, byte[] subnetmask, byte[] dns1, byte[] dns2, byte dhcpenabled)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetIPparameters");

            try
            {
                byte[] message = new byte[2 + 5 * 4 + 1]; message[0] = (byte)(message.Length); message[1] = CMD_SETIPPARAMETERS;
                for (int i = 0; i < 4; i++) message[i + 2] = ipaddress[i];
                for (int i = 0; i < 4; i++) message[i + 6] = gateway[i];
                for (int i = 0; i < 4; i++) message[i + 10] = subnetmask[i];
                for (int i = 0; i < 4; i++) message[i + 14] = dns1[i];
                for (int i = 0; i < 4; i++) message[i + 18] = dns2[i];
                message[22] = dhcpenabled;

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets regime timeout [ms]
        /// </summary>
        /// <param name="regimetimeout"></param>
        /// <returns></returns>
        unsafe public int GetRegimeTimeout(int* regimetimeout)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetRegimeTimeout");

            try
            {
                if (!SendMsg(CMD_GETREGIMETIMEOUT)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                *regimetimeout = (int)rcvBuff[2] + ((int)rcvBuff[3] << 8);

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets regime timeout [ms]
        /// </summary>
        /// <returns></returns>
        public int SetRegimeTimeout(int regimetimeout)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetRegimeTimeout");

            try
            {
                byte[] message = new byte[4]; message[0] = (byte)(message.Length); message[1] = CMD_SETREGIMETIMEOUT;
                message[2] = (byte)(regimetimeout & 0x000000FF); message[3] = (byte)((regimetimeout & 0x0000FF00) >> 8);

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        #endregion

		#region constant getters (made for use in VB6)
		public byte Remote { get { return REMOTE; } }
		public byte Automatic { get { return AUTOMATIC; } }
		public byte Cards{ get { return CARDS; } }
		public byte Groups { get { return GROUPS; } }
		public byte Events { get { return EVENTS; } }
		public byte Log { get { return LOG; } }
		#endregion

        #region logging functionality methods
        private void AddByteSequenceToLog(string label, byte[] b)
        {
            try
            {
                if (currByteSequence == -1) for (int i = 0; i < MAX_BYTE_SEQUENCES; i++) byteSequences[i] = String.Empty;
                currByteSequence++;
                if (currByteSequence >= MAX_BYTE_SEQUENCES)
                {
                    for (int i = 0; i < MAX_BYTE_SEQUENCES; i++) byteSequences[i] = String.Empty;
                    currByteSequence = 0;
                }

                byteSequences[currByteSequence] = label + "   ";
                for (int i = 0; i < ((int)b[0] + 1); i++)
                {
                    byteSequences[currByteSequence] += b[i].ToString("X2");
                    if (i < (b.Length - 1)) byteSequences[currByteSequence] += " ";
                }
            }
            catch { }
        }

        private void AddByteSequenceToLog(string label, string text)
        {
            try
            {
                if (currByteSequence == -1) for (int i = 0; i < MAX_BYTE_SEQUENCES; i++) byteSequences[i] = String.Empty;
                currByteSequence++;
                if (currByteSequence >= MAX_BYTE_SEQUENCES)
                {
                    for (int i = 0; i < MAX_BYTE_SEQUENCES; i++) byteSequences[i] = String.Empty;
                    currByteSequence = 0;
                }

                byteSequences[currByteSequence] = label + "   " + text;
            }
            catch { }
        }

        public string[] SeqLog
        {
            get { currByteSequence = -1; return byteSequences; }
        }
        #endregion

        #region alarm reader version methods

        /// <summary>
        /// Sets active cards table (<> 0 - cards table 1, 0 - cards table 2)
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public int SetActiveTable(byte table)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetActiveTable");

            try
            {
                if (!SendMsg(CMD_SETACTIVETABLE, table)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Returns active cards table (<> cards table 1, 0 - cards table 2)
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        unsafe public int GetActiveTable(byte* table)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetActiveTable");

            *table = 0;
            try
            {
                if (!SendMsg(CMD_GETACTIVETABLE)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*table) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets IP parameters (4 bytes IP address, 4 bytes default gateway,
        /// 4 bytes subnet mask, 4 bytes preferred DNS server, 4 bytes alternate DNS server)
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="gateway"></param>
        /// <param name="subnetmask"></param>
        /// <param name="dns1"></param>
        /// <param name="dns2"></param>
        /// <returns></returns>
        public int SetIPparameters(byte[] ipaddress, byte[] gateway, byte[] subnetmask, byte[] dns1, byte[] dns2)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetIPparameters");

            try
            {
                byte[] message = new byte[2 + 5 * 4]; message[0] = (byte)(message.Length); message[1] = CMD_SETIPPARAMETERS;
                for (int i = 0; i < 4; i++) message[i + 2] = ipaddress[i];
                for (int i = 0; i < 4; i++) message[i + 6] = gateway[i];
                for (int i = 0; i < 4; i++) message[i + 10] = subnetmask[i];
                for (int i = 0; i < 4; i++) message[i + 14] = dns1[i];
                for (int i = 0; i < 4; i++) message[i + 18] = dns2[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets IP parameters (4 bytes IP address, 4 bytes default gateway,
        /// 4 bytes subnet mask, 4 bytes preferred DNS server, 4 bytes alternate DNS server)
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="gateway"></param>
        /// <param name="subnetmask"></param>
        /// <param name="dns1"></param>
        /// <param name="dns2"></param>
        /// <returns></returns>
        unsafe public int GetIPparameters(byte[] ipaddress, byte[] gateway, byte[] subnetmask, byte[] dns1, byte[] dns2)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetIPparameters");

            try
            {
                if (!SendMsg(CMD_GETIPPARAMETERS)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) ipaddress[i] = rcvBuff[i + 2];
                for (int i = 0; i < 4; i++) gateway[i] = rcvBuff[i + 6];
                for (int i = 0; i < 4; i++) subnetmask[i] = rcvBuff[i + 10];
                for (int i = 0; i < 4; i++) dns1[i] = rcvBuff[i + 14];
                for (int i = 0; i < 4; i++) dns2[i] = rcvBuff[i + 18];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets MAC address, 6 bytes, MSB first
        /// </summary>
        /// <param name="macaddress"></param>
        /// <returns></returns>
        unsafe public int GetMACaddress(byte[] macaddress)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetMACaddress");

            try
            {
                if (!SendMsg(CMD_GETMACADDRESS)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 6; i++) macaddress[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets MAC address, 6 bytes, MSB first
        /// </summary>
        /// <param name="macaddress"></param>
        /// <returns></returns>
        public int SetMACaddress(byte[] macaddress)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetMACaddress");

            try
            {
                byte[] message = new byte[10]; message[0] = (byte)(message.Length); message[1] = CMD_SETMACADDRESS;
                for (int i = 0; i < 6; i++) message[i + 2] = macaddress[i]; MACcrc(message,2,6);

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        private void MACcrc(byte[] data, byte offset, byte len)
        {
            byte d;
            UInt16 CRC;

            CRC = 0xFFFF;
            do
            {
                d = (byte)(data[offset++] ^ (CRC & 0xFF)); // Compute combined value.
                d ^= (byte)(d << 4);
                CRC = (UInt16)((d << 3) ^ (d << 8) ^ (CRC >> 8) ^ (d >> 4));
            } while ((--len) > 0);

            data[offset++] = (byte)((CRC & 0xFF) ^ 0xFF);
            data[offset] = (byte)((CRC >> 8) ^ 0xFF);
        }

        /// <summary>
        /// Sets login time, 4 bytes, little endian
        /// </summary>
        /// <returns></returns>
        public int SetLoginTime(byte[] logintime)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetLoginTime");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETLOGINTIME;
                for (int i = 0; i < 4; i++) message[i + 2] = logintime[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets login time, 4 bytes, little endian
        /// </summary>
        /// <param name="logintime"></param>
        /// <returns></returns>
        unsafe public int GetLoginTime(byte[] logintime)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetLoginTime");

            try
            {
                if (!SendMsg(CMD_GETLOGINTIME)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) logintime[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets authorization timeout, 4 bytes, little endian
        /// </summary>
        /// <returns></returns>
        public int SetAuthorizationTimeout(byte[] authorizationtimeout)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetAuthorizationTimeout");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETAUTHORIZATIONTIMEOUT;
                for (int i = 0; i < 4; i++) message[i + 2] = authorizationtimeout[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets authorization timeout, 4 bytes, little endian
        /// </summary>
        /// <param name="authorizationtimeout"></param>
        /// <returns></returns>
        unsafe public int GetAuthorizationTimeout(byte[] authorizationtimeout)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetAuthorizationTimeout");

            try
            {
                if (!SendMsg(CMD_GETAUTHORIZATIONTIMEOUT)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) authorizationtimeout[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets alarm time, 4 bytes, little endian
        /// </summary>
        /// <returns></returns>
        public int SetAlarmTime(byte[] alarmtime)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetAlarmTime");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETALARMTIME;
                for (int i = 0; i < 4; i++) message[i + 2] = alarmtime[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets alarm time, 4 bytes, little endian
        /// </summary>
        /// <param name="alarmtime"></param>
        /// <returns></returns>
        unsafe public int GetAlarmTime(byte[] alarmtime)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetAlarmTime");

            try
            {
                if (!SendMsg(CMD_GETALARMTIME)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) alarmtime[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets TCP socket, 2 bytes, little endian
        /// </summary>
        /// <returns></returns>
        public int SetTCPsocket(byte[] tcpsocket)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetTCPsocket");

            try
            {
                byte[] message = new byte[4]; message[0] = (byte)(message.Length); message[1] = CMD_SETTCPSOCKET;
                for (int i = 0; i < 2; i++) message[i + 2] = tcpsocket[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets TCP socket, 2 bytes, little endian
        /// </summary>
        /// <param name="tcpsocket"></param>
        /// <returns></returns>
        unsafe public int GetTCPsocket(byte[] tcpsocket)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetTCPsocket");

            try
            {
                if (!SendMsg(CMD_GETTCPSOCKET)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 2; i++) tcpsocket[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets TCP log socket, 2 bytes, little endian
        /// </summary>
        /// <returns></returns>
        public int SetTCPlogsocket(byte[] tcplogsocket)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetTCPlogsocket");

            try
            {
                byte[] message = new byte[4]; message[0] = (byte)(message.Length); message[1] = CMD_SETTCPLOGSOCKET;
                for (int i = 0; i < 2; i++) message[i + 2] = tcplogsocket[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets TCP log socket, 2 bytes, little endian
        /// </summary>
        /// <param name="tcplogsocket"></param>
        /// <returns></returns>
        unsafe public int GetTCPlogsocket(byte[] tcplogsocket)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetTCPlogsocket");

            try
            {
                if (!SendMsg(CMD_GETTCPLOGSOCKET)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 2; i++) tcplogsocket[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets digital outputs 0-15
        /// </summary>
        /// <returns></returns>
        public int SetDOs(byte[] dos)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetDOs");

            try
            {
                byte[] b = new byte[2];
                for (int i = 0; i < 8; i++)  b[0] |= (byte)(dos[i] << i);
                for (int i = 8; i < 15; i++) b[1] |= (byte)(dos[i] << (i - 8));

                byte[] message = new byte[4]; message[0] = (byte)(message.Length); message[1] = CMD_SETDIGITALOUTPUTS;
                message[2] = b[0]; message[3] = b[1];
                
                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        #endregion

        #region ups monitor setup methods

        /// <summary>
        /// Gets trap IP, 4 bytes
        /// </summary>
        /// <param name="trapIP"></param>
        /// <returns></returns>
        unsafe public int GetTrapIP(byte[] trapIP)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetTrapIP");

            try
            {
                if (!SendMsg(CMD_GETTRAPIP)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) trapIP[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets trap IP, 4 bytes
        /// </summary>
        /// <param name="trapIP"></param>
        /// <returns></returns>
        public int SetTrapIP(byte[] trapIP)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetTrapIP");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETTRAPIP;
                for (int i = 0; i < 4; i++) message[i + 2] = trapIP[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets DHCP (0 - disabled, 1 - enabled)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        unsafe public int GetDHCP(byte* dhcp)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetDHCP");

            *dhcp = 0;
            try
            {
                if (!SendMsg(CMD_GETDHCP)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*dhcp) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets DHCP (0 - disabled, 1 - enabled)
        /// </summary>
        /// <param name="dhcp"></param>
        /// <returns></returns>
        public int SetDHCP(byte dhcp)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetDHCP");

            try
            {
                if (!SendMsg(CMD_SETDHCP, dhcp)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets max voltage [mV], 4 bytes, little endian
        /// </summary>
        /// <param name="maxvoltage"></param>
        /// <returns></returns>
        unsafe public int GetMaxVoltage(byte[] maxvoltage)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetMaxVoltage");

            try
            {
                if (!SendMsg(CMD_GETMAXVOLTAGE)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) maxvoltage[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets max voltage [mV], 4 bytes, little endian
        /// </summary>
        /// <param name="maxvoltage"></param>
        /// <returns></returns>
        public int SetMaxVoltage(byte[] maxvoltage)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetMaxVoltage");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETMAXVOLTAGE;
                for (int i = 0; i < 4; i++) message[i + 2] = maxvoltage[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets min voltage [mV], 4 bytes, little endian
        /// </summary>
        /// <param name="minvoltage"></param>
        /// <returns></returns>
        unsafe public int GetMinVoltage(byte[] minvoltage)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetMinVoltage");

            try
            {
                if (!SendMsg(CMD_GETMINVOLTAGE)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) minvoltage[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets min voltage [mV], 4 bytes, little endian
        /// </summary>
        /// <param name="minvoltage"></param>
        /// <returns></returns>
        public int SetMinVoltage(byte[] minvoltage)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetMinVoltage");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETMINVOLTAGE;
                for (int i = 0; i < 4; i++) message[i + 2] = minvoltage[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets max current [mA], 4 bytes, little endian
        /// </summary>
        /// <param name="maxcurrent"></param>
        /// <returns></returns>
        unsafe public int GetMaxCurrent(byte[] maxcurrent)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetMaxCurrent");

            try
            {
                if (!SendMsg(CMD_GETMAXCURRENT)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) maxcurrent[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets max current [mA], 4 bytes, little endian
        /// </summary>
        /// <param name="maxcurrent"></param>
        /// <returns></returns>
        public int SetMaxCurrent(byte[] maxcurrent)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetMaxCurrent");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETMAXCURRENT;
                for (int i = 0; i < 4; i++) message[i + 2] = maxcurrent[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets trap mask, 4 bytes, little endian
        /// </summary>
        /// <param name="trapmask"></param>
        /// <returns></returns>
        unsafe public int GetTrapMask(byte[] trapmask)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetTrapMask");

            try
            {
                if (!SendMsg(CMD_GETTRAPMASK)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 4; i++) trapmask[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Sets trap mask, 4 bytes, little endian
        /// </summary>
        /// <param name="trapmask"></param>
        /// <returns></returns>
        public int SetTrapMask(byte[] trapmask)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetTrapMask");

            try
            {
                byte[] message = new byte[6]; message[0] = (byte)(message.Length); message[1] = CMD_SETTRAPMASK;
                for (int i = 0; i < 4; i++) message[i + 2] = trapmask[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Returns number of found bluetooth devices
        /// </summary>
        /// <param name="numberOfDevices"></param>
        /// <returns></returns>
        unsafe public int GetBtNo(byte* numberOfDevices)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetBtNo");

            *numberOfDevices = 0;
            try
            {
                if (!SendMsg(CMD_GETBTNO)) return 0;
                Thread.Sleep(22000);
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                (*numberOfDevices) = rcvBuff[2];
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets bluetooth device address
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public int GetBtAddress(byte deviceNumber, byte[] address, ref string friendlyName)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetBtAddress");

            try
            {
                if (!SendMsg(CMD_GETBTADDRESS, deviceNumber)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                int dataLength = (int)rcvBuff[0] - 2; if ((dataLength < 6) || (dataLength > (6 + 32))) return 0;
                for (int i = 2; i < 6 + 2; i++) address[i - 2] = rcvBuff[i];

                friendlyName = String.Empty;
                if (dataLength > 6) friendlyName = Encoding.ASCII.GetString(rcvBuff, 8, (dataLength - 6));

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region locker controler methods

        /// <summary>
        /// Sets locker statuses (byte 0: 0 - with lock confirmation 1 - without lock confirmation)
        /// bytes 1-32: 0x00 - don't change anything, 0x01 - lock the locker
        /// </summary>
        /// <param name="lockerstatuses"></param>
        /// <returns></returns>
        public int SetLockerStatuses(byte[] lockerstatuses)
        {
            if (logging) AddByteSequenceToLog("CMD", "SetLockerStatuses");

            try
            {
                byte[] message = new byte[lockerstatuses.Length + 2]; message[0] = (byte)(message.Length); message[1] = CMD_SETLOCKERSTATUSES;
                for (int i = 0; i < lockerstatuses.Length; i++) message[i + 2] = lockerstatuses[i];

                if (!SendMsg(message)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;
                return 1;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Gets locker statuses, 33 bytes, 1 - controller mode, 32 - one status byte per locker
        /// </summary>
        /// <param name="lockerstatuses"></param>
        /// <returns></returns>
        unsafe public int GetLockerStatuses(byte[] lockerstatuses)
        {
            if (logging) AddByteSequenceToLog("CMD", "GetLockerStatuses");

            try
            {
                if (!SendMsg(CMD_GETLOCKERSTATUSES)) return 0;
                if (!GetMsg()) return 0;
                if (!DeviceStatus()) return 0;

                for (int i = 0; i < 33; i++) lockerstatuses[i] = rcvBuff[i + 2];

                return 1;
            }
            catch (Exception) { return 0; }
        }

        #endregion

        #region crc16
        private ushort[] crc16tab =
		{
			0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
			0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef,
			0x1231,0x0210,0x3273,0x2252,0x52b5,0x4294,0x72f7,0x62d6,
			0x9339,0x8318,0xb37b,0xa35a,0xd3bd,0xc39c,0xf3ff,0xe3de,
			0x2462,0x3443,0x0420,0x1401,0x64e6,0x74c7,0x44a4,0x5485,
			0xa56a,0xb54b,0x8528,0x9509,0xe5ee,0xf5cf,0xc5ac,0xd58d,
			0x3653,0x2672,0x1611,0x0630,0x76d7,0x66f6,0x5695,0x46b4,
			0xb75b,0xa77a,0x9719,0x8738,0xf7df,0xe7fe,0xd79d,0xc7bc,
			0x48c4,0x58e5,0x6886,0x78a7,0x0840,0x1861,0x2802,0x3823,
			0xc9cc,0xd9ed,0xe98e,0xf9af,0x8948,0x9969,0xa90a,0xb92b,
			0x5af5,0x4ad4,0x7ab7,0x6a96,0x1a71,0x0a50,0x3a33,0x2a12,
			0xdbfd,0xcbdc,0xfbbf,0xeb9e,0x9b79,0x8b58,0xbb3b,0xab1a,
			0x6ca6,0x7c87,0x4ce4,0x5cc5,0x2c22,0x3c03,0x0c60,0x1c41,
			0xedae,0xfd8f,0xcdec,0xddcd,0xad2a,0xbd0b,0x8d68,0x9d49,
			0x7e97,0x6eb6,0x5ed5,0x4ef4,0x3e13,0x2e32,0x1e51,0x0e70,
			0xff9f,0xefbe,0xdfdd,0xcffc,0xbf1b,0xaf3a,0x9f59,0x8f78,
			0x9188,0x81a9,0xb1ca,0xa1eb,0xd10c,0xc12d,0xf14e,0xe16f,
			0x1080,0x00a1,0x30c2,0x20e3,0x5004,0x4025,0x7046,0x6067,
			0x83b9,0x9398,0xa3fb,0xb3da,0xc33d,0xd31c,0xe37f,0xf35e,
			0x02b1,0x1290,0x22f3,0x32d2,0x4235,0x5214,0x6277,0x7256,
			0xb5ea,0xa5cb,0x95a8,0x8589,0xf56e,0xe54f,0xd52c,0xc50d,
			0x34e2,0x24c3,0x14a0,0x0481,0x7466,0x6447,0x5424,0x4405,
			0xa7db,0xb7fa,0x8799,0x97b8,0xe75f,0xf77e,0xc71d,0xd73c,
			0x26d3,0x36f2,0x0691,0x16b0,0x6657,0x7676,0x4615,0x5634,
			0xd94c,0xc96d,0xf90e,0xe92f,0x99c8,0x89e9,0xb98a,0xa9ab,
			0x5844,0x4865,0x7806,0x6827,0x18c0,0x08e1,0x3882,0x28a3,
			0xcb7d,0xdb5c,0xeb3f,0xfb1e,0x8bf9,0x9bd8,0xabbb,0xbb9a,
			0x4a75,0x5a54,0x6a37,0x7a16,0x0af1,0x1ad0,0x2ab3,0x3a92,
			0xfd2e,0xed0f,0xdd6c,0xcd4d,0xbdaa,0xad8b,0x9de8,0x8dc9,
			0x7c26,0x6c07,0x5c64,0x4c45,0x3ca2,0x2c83,0x1ce0,0x0cc1,
			0xef1f,0xff3e,0xcf5d,0xdf7c,0xaf9b,0xbfba,0x8fd9,0x9ff8,
			0x6e17,0x7e36,0x4e55,0x5e74,0x2e93,0x3eb2,0x0ed1,0x1ef0
		};

        public ushort crc16(byte[] buffer, int len)
        {
            ushort crc = 0xFFFF; //0x8408; //0xFFFF;
            for (int i = 0; i < len; i++)
            {
                crc = (ushort)((crc << 8) ^ crc16tab[((crc >> 8) ^ buffer[i]) & 0x00FF]);
            }
            return crc;
        }
        #endregion

        public void Flush() { sp.Flush(); }

        public uint Ready() { return (sp.Ready()); }

        public uint Recv(byte[] b, uint n) { return (sp.Recv(b, n)); }

    }
}
