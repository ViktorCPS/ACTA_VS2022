using System;
using System.Text;

namespace Util
{
	/// <summary>
	/// Crypt class contains crypt and decrypt methods. 
	/// </summary>
	public class Crypt
	{

		public Crypt()
		{

		}

		/// <summary>
		/// Crypt given string by shifting to right ASCII value of each char 
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public string CryptMoveRight(string inputStr)
		{
			StringBuilder output = new StringBuilder();

			try
			{
				char[] charArray = inputStr.ToCharArray(0, inputStr.Length);
				
				for(int i=0; i < charArray.Length; i++)
				{
					if (charArray[i] == 'z')
					{
						output.Append('a');
					}
					else
					{
						output.Append(Convert.ToChar(Convert.ToInt16(charArray[i]) + 1));
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return output.ToString();
		}

		public string DecryptMoveRight(string inputStr)
		{
			StringBuilder output = new StringBuilder();

			try
			{
				char[] charArray = inputStr.ToCharArray(0, inputStr.Length);
				
				for(int i=0; i < charArray.Length; i++)
				{
					if (charArray[i] == 'z')
					{
						output.Append('a');
					}
					else
					{
						output.Append(Convert.ToChar(Convert.ToInt16(charArray[i]) - 1));
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return output.ToString();
		}

		public string DecryptConnectionString(string connSttring)
		{
			StringBuilder sb = new StringBuilder();

			try
			{
				int userStart = connSttring.IndexOf("uid=") + 4;
				int to = connSttring.IndexOf(";pwd=");

				sb.Append(connSttring.Substring(0, userStart));

				string user = connSttring.Substring(userStart, to - userStart);
				sb.Append(this.DecryptMoveRight(user.Trim()));
				sb.Append(";pwd=");
				string password = connSttring.Substring(to + 5, connSttring.Length - (to + 5));
				sb.Append(this.DecryptMoveRight(password));
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return sb.ToString();
		}
	}
}
