using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;


namespace Util
{
    public class NedapDevice
    {
        #region constants
        const byte DC2 = 0x12;
        const byte DC4 = 0x14;
        const byte STX = 0x02;
        const byte ETX = 0x03;
        const byte ACK = 0x06;
        const byte NAK = 0x15;
        const int NEDAP_PORT = 10001;
        const int LANTRONIX_PORT = 30704;
        const string CLEAR_EVENT_BUFFER = "0101010250>:";
        #endregion

        private string ipAddress = String.Empty;

        private TcpClient tcpClientNedap = null;
        private NetworkStream netStreamNedap = null;
        private TcpClient tcpClientLantronix = null;
        private NetworkStream netStreamLantronix = null;
        private Byte[] receiveBuffer = null;

        public NedapDevice(string ipddress)
        {
            ipAddress = ipddress;
        }

        public void Connect()
        {
            try
            {
                tcpClientNedap = new TcpClient();
                tcpClientNedap.Connect(ipAddress, NEDAP_PORT);
                netStreamNedap = tcpClientNedap.GetStream();
                receiveBuffer = new Byte[tcpClientNedap.ReceiveBufferSize];
                if (netStreamNedap.DataAvailable) netStreamNedap.Read(receiveBuffer, 0, (int)tcpClientNedap.ReceiveBufferSize);
                SendCommand(CLEAR_EVENT_BUFFER);

                tcpClientLantronix = new TcpClient();
                tcpClientLantronix.Connect(ipAddress, LANTRONIX_PORT);
                netStreamLantronix = tcpClientLantronix.GetStream();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (netStreamNedap != null) netStreamNedap.Close();
                if (tcpClientNedap != null) tcpClientNedap.Close();

                if (netStreamLantronix != null) netStreamLantronix.Close();
                if (tcpClientLantronix != null) tcpClientLantronix.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetTagID()
        {
            string tagID = String.Empty;

            try
            {
                if (netStreamNedap.DataAvailable)
                {
                    int bin;

                    // seek for DC2 <
                    bin = 0;
                    while (((byte)bin != DC2) && (bin != -1)) bin = netStreamNedap.ReadByte();
                    if (bin == -1) return String.Empty;

                    // DC4 >
                    netStreamNedap.WriteByte(DC4); Thread.Sleep(200);

                    // seek for STX <
                    bin = 0;
                    while (((byte)bin != STX) && (bin != -1)) bin = netStreamNedap.ReadByte();
                    if (bin == -1) return String.Empty;

                    // read until ETX <
                    bin = 0; int i = 0;
                    while (((byte)bin != ETX))
                    {
                        bin = netStreamNedap.ReadByte();
                        if (bin == -1) return String.Empty;
                        receiveBuffer[i] = (byte)bin;
                        i++;
                        if (i >= tcpClientNedap.ReceiveBufferSize) return String.Empty;
                    }

                    // ACK >
                    netStreamNedap.WriteByte(ACK);

                    string returndata = Encoding.ASCII.GetString(receiveBuffer);
                    for (i = 9; i < 9 + 6; i++) if (!Char.IsDigit(returndata, i)) return String.Empty;
                    tagID = returndata.Substring(9, 6);

                    return tagID;
                }
                else return String.Empty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetLantronixDO(int DO, int state)
        {
            string msg = "DO=" + DO.ToString() + " state=" + state.ToString();
            try
            {
                msg += " Before netStreamLantronix.DataAvailable, ";
                if (netStreamLantronix.DataAvailable)
                {
                    msg += "Before netStreamLantronix.Flush, ";
                    netStreamLantronix.Flush();
                    msg += "After netStreamLantronix.Flush, ";
                }

                byte[] b = new byte[9];
                b[0] = 0x1B; b[1] = 0x00; b[2] = 0x00; b[3] = 0x00; b[4] = 0x00; b[5] = 0x00; b[6] = 0x00; b[7] = 0x00; b[8] = 0x00;
                b[1] = (byte)(0x01 << DO);
                b[5] = (byte)(state << DO);

                msg += "Before netStreamLantronix.Write, ";
                netStreamLantronix.Write(b, 0, b.Length);
                msg += "After netStreamLantronix.Write, ";
            }
            catch (Exception e)
            {
                Exception ex = new Exception(msg + e.ToString());
                throw ex;
            }
        }

        public int GetLantronixDO(int DO)
        {
            try
            {
                if (netStreamLantronix.DataAvailable) netStreamLantronix.Flush();
                byte[] b = new byte[9];
                b[0] = 0x13; b[1] = 0x00; b[2] = 0x00; b[3] = 0x00; b[4] = 0x00; b[5] = 0x00; b[6] = 0x00; b[7] = 0x00; b[8] = 0x00;
                netStreamLantronix.Write(b, 0, b.Length);
                netStreamLantronix.Read(b, 0, 5);
                return (int)(b[1] >> DO);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void SendCommand(string c)
        {
            try
            {
                Byte[] bc = Encoding.ASCII.GetBytes(c);
                Byte[] command = new Byte[c.Length + 4];
                byte checksum = 0; byte b = 0; command[0] = STX; command[c.Length + 3] = ETX;
                for (int i = 0; i < bc.Length; i++)
                {
                    checksum += bc[i];
                    command[i + 1] = bc[i];
                }
                command[c.Length + 1] = (byte)((checksum >> 4) + 0x30);
                command[c.Length + 2] = (byte)((checksum & 0x0F) + 0x30);
                netStreamNedap.Write(command, 0, command.Length); Thread.Sleep(200);
                if (netStreamNedap.DataAvailable) b = (byte)netStreamNedap.ReadByte(); // ACK <
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
