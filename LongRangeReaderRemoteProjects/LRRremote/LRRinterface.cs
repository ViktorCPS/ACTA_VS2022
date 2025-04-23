using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO.Ports;
using System.Threading;

namespace LRRremote
{
    public interface LRRinterface
    {
        bool Open();
        bool IsOpen();
        void Close();
        uint Send(byte[] b);
        uint Recv(byte[] b, uint n);
        uint Ready();
        void Flush();
        uint Receive(byte[] b, uint n);
        uint Receive(byte[] b);
    }

    public class IPInterface : LRRinterface
    {
        const int TIMEOUT = 500;
        private TcpClient tcpClient = new TcpClient();
        private NetworkStream netStream = null;
        private string ipAddress;
        private bool isOpen = false;

        public IPInterface(string IPAddress)
        {
            ipAddress = IPAddress;
        }

        public bool Open()
        {
            try
            {
                tcpClient.Connect(ipAddress, 10001);
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
                Thread.Sleep(100);
                netStream = null;
                tcpClient = null;
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

        public uint Receive(byte[] b, uint n)
        {
            DateTime t0 = DateTime.Now; int msElapsed = 0;
            while ((!netStream.DataAvailable) && (msElapsed < TIMEOUT))
            {
                Thread.Sleep(10);
                msElapsed = (int)((DateTime.Now - t0).Ticks / TimeSpan.TicksPerMillisecond);
            }
            if (netStream.DataAvailable) return ((uint)netStream.Read(b, 0, 1)); else return 0;
        }

        public uint Receive(byte[] b)
        {
            DateTime t0 = DateTime.Now; int msElapsed = 0;
            while ((!netStream.DataAvailable) && (msElapsed < TIMEOUT))
            {
                Thread.Sleep(10);
                msElapsed = (int)((DateTime.Now - t0).Ticks / TimeSpan.TicksPerMillisecond);
            }
            if (netStream.DataAvailable)
            {
                int i = 0;
                while (netStream.DataAvailable)
                {
                    netStream.Read(b, i, 1);
                    i++;
                }
                return (uint)i;
            }
            else return 0;
        }
    }

    public class SerialInterface : LRRinterface
    {
        const int TIMEOUT = 500;
        private SerialPort sp = null;

        public SerialInterface(string comPort, int baudrate)
        {
            try
            {
                sp = new System.IO.Ports.SerialPort(comPort, baudrate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
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
            try
            {
                if (sp != null) sp.Close();
                Thread.Sleep(100);
                sp = null;
            }
            catch { }
        }

        public uint Send(byte[] b)
        {
            sp.Write(b, 0, b.Length);
            return ((uint)b.Length);
        }

        public uint Recv(byte[] b, uint n)
        {
            return (uint)(sp.Read(b, 0, (int)n));
        }

        public uint Ready()
        {
            return ((uint)sp.BytesToRead);
        }

        public void Flush()
        {
            sp.DiscardInBuffer();
            sp.DiscardOutBuffer();
        }

        public uint Receive(byte[] b, uint n)
        {
            DateTime t0 = DateTime.Now; int msElapsed = 0;
            while ((sp.BytesToRead == 0) && (msElapsed < TIMEOUT))
            {
                Thread.Sleep(10);
                msElapsed = (int)((DateTime.Now - t0).Ticks / TimeSpan.TicksPerMillisecond);
            }
            if (sp.BytesToRead > 0) return ((uint)sp.Read(b, 0, 1)); else return 0;
        }

        public uint Receive(byte[] b)
        {
            DateTime t0 = DateTime.Now; int msElapsed = 0;
            while ((sp.BytesToRead == 0) && (msElapsed < TIMEOUT))
            {
                Thread.Sleep(10);
                msElapsed = (int)((DateTime.Now - t0).Ticks / TimeSpan.TicksPerMillisecond);
            }
            if (sp.BytesToRead > 0) { Thread.Sleep(10); return ((uint)sp.Read(b, 0, sp.BytesToRead)); } else return 0;
        }
    }

}
