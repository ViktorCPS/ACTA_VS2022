using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Util
{
	/// <summary>
	/// Ping - check connection utility
	/// </summary>
	public class Ping
	{
		// socket and event
		protected Socket socket;
		protected bool isOpen;
		protected ManualResetEvent readComplete;
		protected byte lastSequenceNr = 0;

		// ping command and result fields
		protected byte[] pingCommand;
		protected byte[] pingResult;

		public Ping()
		{
			// create command field
			pingCommand = new byte[8];
			pingCommand[0] = 8;			// Type
			pingCommand[1] = 0;			// Subtype
			pingCommand[2] = 0;			// Checksum
			pingCommand[3] = 0;	
			pingCommand[4] = 1;			// Identifier
			pingCommand[5] = 0;
			pingCommand[6] = 0;			// Sequence number
			pingCommand[7] = 0;

			// create result field
			pingResult = new byte[pingCommand.Length + 120];	// 20 byte = returned IP Header
		}

		private void Open()
		{
			// create socket and connect
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
			isOpen = true;
			
			// create read complete event
			readComplete = new ManualResetEvent(false);
		}

		private void Close()
		{
			// clear open flag
			isOpen = false;

			// close socket
			socket.Close();

			// close event
			readComplete.Close();
		}

		private TimeSpan Send(string address, TimeSpan timeout)
		{
			try 
			{
				// empty result buffer
				while (socket.Available > 0) 
					socket.Receive(pingResult, Math.Min(socket.Available, pingResult.Length), SocketFlags.None);
			
				// reset event
				readComplete.Reset();

				// save start time
				DateTime timeSend = DateTime.Now;

				// complete ping command
				pingCommand[6] = lastSequenceNr++;
				SetChecksum(pingCommand);

				// send ping command, start receive command
				int iSend = socket.SendTo(pingCommand, new IPEndPoint(IPAddress.Parse(address), 0));
				socket.BeginReceive(pingResult, 0, pingResult.Length, SocketFlags.None,	new AsyncCallback(CallBack), null);

				// wait until data received or timeout
				if (readComplete.WaitOne(timeout, false))
				{
					// check result
					if ((pingResult[20] == 0) &&
						(pingCommand[4] == pingResult[24]) && (pingCommand[5] == pingResult[25]) &&
						(pingCommand[6] == pingResult[26]) && (pingCommand[7] == pingResult[27]))
						// return time delay
						return DateTime.Now.Subtract(timeSend);
				}

				// return timeout
				return TimeSpan.MaxValue; 
			}
			catch (Exception)
			{
				return TimeSpan.MaxValue; 
			}
		}

		public bool Completed(string ipAddress, int secTimeout)
		{
			TimeSpan span = TimeSpan.MaxValue;
			try
			{
				this.Open();
				span = this.Send(ipAddress,new TimeSpan(0,0,secTimeout));
			}
			catch
			{
				if(this.isOpen) this.Close();
			}
		
			return (span < TimeSpan.MaxValue);
		}

		protected void CallBack(IAsyncResult result)
		{
			try
			{
				if (isOpen)
				{
					socket.EndReceive(result);
					readComplete.Set();
				}
			}
			catch(Exception) {}
		}

		protected void SetChecksum(byte[] tel)
		{
			// reset old checksum
			tel[2] = 0;
			tel[3] = 0;

			// calculate new checksum
			uint cs = 0;
			for (int i = 0; i < pingCommand.Length; i = i+2)
				cs += BitConverter.ToUInt16(pingCommand, i);
			cs = ~((cs & 0xffffu) + (cs >> 16));

			// set new checksum
			tel[2] = (byte) cs;
			tel[3] = (byte) (cs >> 8);
		}
	}
}
