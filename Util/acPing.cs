using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    public class acPing
    {
        System.Net.NetworkInformation.Ping pingSender;

        public acPing()
        {
            pingSender = new System.Net.NetworkInformation.Ping();
        }

        public bool Completed(string terminalIPaddress, int seconds)
        {
            System.Net.NetworkInformation.PingReply rep;
            byte[] pingBytes = { 0x41, 0x42, 0x43, 0x44 };
            rep = pingSender.Send(terminalIPaddress, seconds * 1000, pingBytes);
            return (rep.Status == System.Net.NetworkInformation.IPStatus.Success);
        }
    }
}
