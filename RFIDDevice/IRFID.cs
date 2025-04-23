using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.IO.Ports;

namespace DeviceRFID
{
    /// <summary>
    /// Summary description for IRFID.
    /// </summary>
    public interface IRFIDInterface
    {
        bool Open();
        bool Open(int secTimeout);
        bool IsOpen();
        void Close();
        uint Send(byte[] b);
        uint Recv(byte[] br, uint n);
        uint Ready();
        void Flush();
        void HardReset();
        void SetLantronixDO(int DO, int state);
        int GetLantronixDO(int DO);
    }

    public class IRFIDFactory
    {
        private static string interfaceType = "SERIAL";
        private static string address = "1";
        private static int speed = 38400;

        public static string InterfaceType
        {
            set
            {
                interfaceType = value;
            }
        }

        public static string Address
        {
            set
            {
                address = value;
            }
        }

        public static int Speed
        {
            set
            {
                speed = value;
            }
        }

        public static IRFIDInterface GetInterface
        {
            get
            {
                IRFIDInterface irfid = null;

                if (interfaceType.ToUpper() == "SERIAL")
                {
                    irfid = new SerialInterface(address, speed);
                }
                else if (interfaceType.ToUpper() == "IP")
                {
                    irfid = new IPInterface(address);
                }
                else if (interfaceType.ToUpper() == "GSM")
                {
                    irfid = new GSMInterface(address);
                }

                return irfid;
            }
        }
    }

    public class SerialInterface : IRFIDInterface
    {
        //private SerialPorts.SerialPort sp = null;
        private SerialPort sp = null;

        public SerialInterface(string comPortNum, int baudrate)
        {
            try
            {
                //sp = new SerialPort(Int32.Parse(comPortNum),baudrate,8,0,1);
                sp = new System.IO.Ports.SerialPort("COM" + comPortNum, baudrate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Open()
        {
            //return(sp.Open());
            try
            {
                sp.Open();
                return true;
            }
            catch { return false; }
        }

        public bool Open(int secTimeout)
        {
            //return(sp.Open());
            try
            {
                sp.Open();
                return true;
            }
            catch { return false; }
        }

        public bool IsOpen()
        {
            return sp.IsOpen;
        }

        public void Close()
        {
            sp.Close();
        }

        public uint Send(byte[] b)
        {
            //return(sp.Send(b));
            sp.Write(b, 0, b.Length);
            return ((uint)b.Length);
        }

        public uint Recv(byte[] b, uint n)
        {
            //return(sp.Recv(b,n));
            return (uint)(sp.Read(b, 0, (int)n));
        }

        public uint Ready()
        {
            //return(sp.Ready());
            return ((uint)sp.BytesToRead);
        }

        public void Flush()
        {
            //sp.Flush();
            sp.DiscardInBuffer();
            sp.DiscardOutBuffer();
        }

        public void HardReset()
        {
            return;
        }

        public void SetLantronixDO(int DO, int state)
        {
            return;
        }

        public int GetLantronixDO(int DO)
        {
            return 0;
        }
    }


    public class IPInterface : IRFIDInterface
    {
        private TcpClient tcpClient = new TcpClient();
        private NetworkStream netStream = null;
        private string ipAddress;
        private int ipPort = 10001;
        private bool isOpen = false;

        public IPInterface(string IPAddress)
        {
            try
            {
                string[] IPAddressParts = IPAddress.Split(':');
                ipAddress = IPAddressParts[0];
                if (IPAddressParts.Length > 1)
                {
                    if (!Int32.TryParse(IPAddressParts[1], out ipPort)) ipPort = 10001;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Open()
        {
            try
            {
                tcpClient.Connect(ipAddress, ipPort);
                netStream = tcpClient.GetStream();
                if (netStream.DataAvailable) netStream.Flush();
                isOpen = true;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Open(int secTimeout)
        {
            try
            {
                isOpen = false;

                IAsyncResult ar = tcpClient.BeginConnect(ipAddress, ipPort, null, null);
                System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
                try
                {
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(secTimeout), false))
                    {
                        return false;
                    }

                    tcpClient.EndConnect(ar);
                }
                finally
                {
                    wh.Close();
                }

                netStream = tcpClient.GetStream();
                if (netStream.DataAvailable) netStream.Flush();
                isOpen = true;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool IsOpen()
        {
            return isOpen;
        }

        public void Close()
        {
            try
            {
                if (netStream != null) netStream.Close();
                if (tcpClient != null) tcpClient.Close();
                isOpen = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public uint Send(byte[] b)
        {
            try
            {
                netStream.Write(b, 0, b.Length);
                return (uint)b.Length;
            }
            catch
            {
                return 0;
            }
        }

        public uint Recv(byte[] b, uint n)
        {
            try
            {
                if (netStream.DataAvailable)
                {
                    return ((uint)netStream.Read(b, 0, (int)n));
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public uint Ready()
        {
            try
            {
                if (!netStream.DataAvailable)
                {
                    return 0;
                }
                else
                {
                    return 1024;
                }
            }
            catch
            {
                return 0;
            }
        }

        public void Flush()
        {
            try
            {
                while (netStream.DataAvailable)
                {
                    netStream.ReadByte();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void HardReset()
        {
            try
            {
                TcpClient resetTcpClient = new TcpClient();
                NetworkStream resetNetStream = null;
                resetTcpClient.Connect(ipAddress, 30704);
                resetNetStream = resetTcpClient.GetStream();
                if (resetNetStream.DataAvailable) resetNetStream.Flush();
                byte[] b = new byte[9];

                b[0] = 0x19; b[1] = 0x01; b[2] = 0x00; b[3] = 0x00; b[4] = 0x00; b[5] = 0x01; b[6] = 0x00; b[7] = 0x00; b[8] = 0x00;
                resetNetStream.Write(b, 0, b.Length);
                Thread.Sleep(500);
                for (int i = 0; i < b.Length; i++) b[i] = 0x00;
                resetNetStream.Read(b, 0, b.Length);

                b[0] = 0x1B; b[1] = 0x01; b[2] = 0x00; b[3] = 0x00; b[4] = 0x00; b[5] = 0x01; b[6] = 0x00; b[7] = 0x00; b[8] = 0x00;
                resetNetStream.Write(b, 0, b.Length);
                Thread.Sleep(3000);
                b[5] = 0x00;
                resetNetStream.Write(b, 0, b.Length);
                Thread.Sleep(2000);
                if (resetNetStream != null) resetNetStream.Close();
                if (resetTcpClient != null) resetTcpClient.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Sets Lantronix digital output DO (0-2) to state (0/1)
        /// </summary>
        /// <param name="DO"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public void SetLantronixDO(int DO, int state)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                NetworkStream netStream = null;
                tcpClient.Connect(ipAddress, 30704);
                netStream = tcpClient.GetStream();
                if (netStream.DataAvailable) netStream.Flush();
                byte[] b = new byte[9];
                b[0] = 0x1B; b[1] = 0x00; b[2] = 0x00; b[3] = 0x00; b[4] = 0x00; b[5] = 0x00; b[6] = 0x00; b[7] = 0x00; b[8] = 0x00;
                b[1] = (byte)(0x01 << DO);
                b[5] = (byte)(state << DO);
                netStream.Write(b, 0, b.Length);
                if (netStream != null) netStream.Close();
                if (tcpClient != null) tcpClient.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets state (0/1) of Lantronix digital output DO (0-2)
        /// </summary>
        /// <param name="DO"></param>
        /// <returns></returns>
        public int GetLantronixDO(int DO)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                NetworkStream netStream = null;
                tcpClient.Connect(ipAddress, 30704);
                netStream = tcpClient.GetStream();
                if (netStream.DataAvailable) netStream.Flush();
                byte[] b = new byte[9];
                b[0] = 0x13; b[1] = 0x00; b[2] = 0x00; b[3] = 0x00; b[4] = 0x00; b[5] = 0x00; b[6] = 0x00; b[7] = 0x00; b[8] = 0x00;
                netStream.Write(b, 0, b.Length);
                netStream.Read(b, 0, 5);
                if (netStream != null) netStream.Close();
                if (tcpClient != null) tcpClient.Close();
                return (int)(b[1] >> DO);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class GSMInterface : IRFIDInterface
    {
        //private SerialPorts.SerialPort sp = null;
        private SerialPort sp = null;

        private const int gsmPhoneBaudRate = 19200;
        private string comPortNum = "";
        private string gsmPhoneNum = "";
        char[] separator = { ' ' };

        public GSMInterface(string gsmaddress)
        {
            try
            {
                string[] segments = gsmaddress.Trim().Split(separator);
                comPortNum = segments[0];
                gsmPhoneNum = segments[1];
                //sp = new SerialPort(Int32.Parse(comPortNum), gsmPhoneBaudRate, 8, 0, 1);
                sp = new System.IO.Ports.SerialPort("COM" + comPortNum, gsmPhoneBaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Open()
        {
            try
            {
                sp.Open();
                sp.RtsEnable = true;
            }
            catch { return false; }

            //if (!sp.Open()) return false;

            //if (GSMAlreadyConnected()) return true;

            if (!GSMPresent()) return false;

            return (GSMConnect());
        }

        public bool Open(int secTimeout)
        {
            try
            {
                sp.Open();
                sp.RtsEnable = true;
            }
            catch { return false; }

            //if (!sp.Open()) return false;

            //if (GSMAlreadyConnected()) return true;

            if (!GSMPresent()) return false;

            return (GSMConnect());
        }

        public bool IsOpen()
        {
            return sp.IsOpen;
        }

        public void Close()
        {
            // no hangup - GSM modem do it itself after a period of inactivity
            // hangup
            Hangup();
            sp.Close();
        }

        public uint Send(byte[] b)
        {
            //return(sp.Send(b));
            sp.Write(b, 0, b.Length);
            return ((uint)b.Length);
        }

        public uint Recv(byte[] b, uint n)
        {
            //return(sp.Recv(b,n));
            return (uint)(sp.Read(b, 0, (int)n));
        }

        public uint Ready()
        {
            //return(sp.Ready());
            return ((uint)sp.BytesToRead);
        }

        public void Flush()
        {
            //sp.Flush();
            sp.DiscardInBuffer();
            sp.DiscardOutBuffer();
        }

        public void HardReset()
        {
            return;
        }

        public void SetLantronixDO(int DO, int state)
        {
            return;
        }

        public int GetLantronixDO(int DO)
        {
            return 0;
        }

        private bool GSMAlreadyConnected()
        {
            try
            {
                byte[] recBuf = new byte[1024];

                // check if already connected
                Thread.Sleep(1000);     // silence time
                bool commandMode = false;
                Flush();
                Send(Encoding.ASCII.GetBytes("+++"));         // go to command mode
                Thread.Sleep(100);
                int miliseconds = 100;
                while ((Ready() == 0) && (miliseconds < 2000))
                {
                    Thread.Sleep(100);
                    miliseconds += 100;
                }
                if (Ready() > 0)
                {
                    Thread.Sleep(100);
                    Recv(recBuf, Math.Min(Ready(), (uint)recBuf.Length));
                    if (Encoding.ASCII.GetString(recBuf).ToUpper().Contains("OK")) commandMode = true;
                }
                if (!commandMode) return false;

                bool alreadyConnected = false;
                Flush();
                Send(Encoding.ASCII.GetBytes("AT+CPAS\r"));   // check connection status (4 = already connected)
                Thread.Sleep(200);
                if (Ready() > 0)
                {
                    Recv(recBuf, Math.Min(Ready(), (uint)recBuf.Length));
                    if (Encoding.ASCII.GetString(recBuf).ToUpper().Contains("+CPAS: 4")) alreadyConnected = true;
                }

                Flush();
                Send(Encoding.ASCII.GetBytes("ATO\r"));         // go to on-line (bypass) mode
                Thread.Sleep(200);
                if (Ready() > 0)
                {
                    Recv(recBuf, Math.Min(Ready(), (uint)recBuf.Length));
                    if (Encoding.ASCII.GetString(recBuf).ToUpper().Contains("CONNECT")) commandMode = false;
                }
                if (commandMode) return false;

                return alreadyConnected;
            }
            catch { return false; }
        }

        private bool GSMPresent()
        {
            try
            {
                byte[] recBuf = new byte[1024];

                // check for gsm modem presence
                bool present = false;
                int noTries = 0;
                while ((!present) && (noTries < 3))
                {
                    Flush();
                    byte[] at = { 0x41, 0x54, 0x0D };   // "AT\r"
                    Send(at);
                    Thread.Sleep(100);
                    int miliseconds = 100;
                    uint n = Ready();
                    while ((Ready() == 0) && (miliseconds < 1000))
                    {
                        Thread.Sleep(100);
                        miliseconds += 100;
                    }
                    if (Ready() > 0)
                    {
                        Thread.Sleep(100);
                        Recv(recBuf, Math.Min(Ready(), (uint)recBuf.Length));
                        if (Encoding.ASCII.GetString(recBuf).ToUpper().Contains("OK")) present = true;
                    }
                    noTries++;
                }
                return present;
            }
            catch { return false; }
        }

        private bool GSMConnect()
        {
            try
            {
                byte[] recBuf = new byte[1024];

                // connect
                bool connected = false;
                int noTries = 0;
                while ((!connected) && (noTries < 3))
                {
                    Flush();
                    byte[] dial = Encoding.ASCII.GetBytes("ATD" + gsmPhoneNum + "\r");
                    Send(dial);
                    Thread.Sleep(1000);
                    int seconds = 1;
                    while ((Ready() <= dial.Length) && (seconds < 60))   // if there's anything besides the local echo in the receiving buffer
                    {
                        Thread.Sleep(1000);
                        seconds++;
                    }
                    if (Ready() > 0)
                    {
                        Thread.Sleep(100);
                        Recv(recBuf, Math.Min(Ready(), (uint)recBuf.Length));
                        if (Encoding.ASCII.GetString(recBuf).ToUpper().Contains("CONNECT"))
                        {
                            Thread.Sleep(100);
                            connected = true;
                        }
                    }
                    noTries++;
                }
                return connected;
            }
            catch { return false; }
        }

        private void Hangup()
        {
            Flush(); byte[] hangup = { 0x02, 0x2B, 0x29 }; Send(hangup);
        }
    }
}