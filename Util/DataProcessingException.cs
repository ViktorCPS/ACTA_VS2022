using System;

namespace Util
{
	/// <summary>
	/// Define custom exception handling for 
	/// Processing data from the moment when they are 
	/// downloaded from reader till populate io_pairs table.
	/// </summary>
	public class DataProcessingException : ApplicationException
	{
		public string message;
		private int number = -1;
		
		public int Number
		{
			get { return number; }
		}

        public override string Message 
        {
            get { return message; } 
        }

		public DataProcessingException()
		{
			
		}

		public DataProcessingException(string exMessage): base()
		{
			this.message = exMessage;
		}

		public DataProcessingException(string exMessage, System.Exception inner) : base()
		{
			this.message = exMessage;
		}

		public DataProcessingException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base()
		{
			
		}

		public DataProcessingException(string exMessage, int exNumber)
		{
			this.number = exNumber;
			this.message = this.setMessage(this.number) + " " + exMessage;
		}

		private string setMessage(int exceptionNum)
		{
			string returnMessage;

			switch (exceptionNum)
			{
				case 1:
				{
					returnMessage = "\nCan't reach directory! \n";
					break;
				}
				case 2:
				{
					returnMessage = "\nCan't open file! \n";
					break;
				}
				case 3:
				{
					returnMessage = "\nFile can't be deserialized. \n";
					break;
				}
				case 4:
				{
					returnMessage = "\nCan't delete records from log_tmp table. \n";
					break;
				}
				case 5:
				{
					returnMessage = "\nCan't insert to log_tmp table. \n";
					break;
				}
				case 6:
				{
					returnMessage = "\nCan't move records from log_tmp to log. \n";
					break;
				}
				case 7:
				{
					returnMessage = "\nException while create Passes. \n";
					break;
				}
				case 8:
				{
					returnMessage = "\nException while inserting Pass to table. Duplicate record found.\n";
					break;
				}
				case 9:
				{
					returnMessage = "\nUnhandled Exception while inserting to Passes table.\n";
					break;
				}
				case 10:
				{
					returnMessage = "\nException while updating Log table.\n";
					break;
				}
				case 11:
				{
					returnMessage = "\nUnhandled Exception.\n";
					break;
				}
				case 12:
				{
					returnMessage = "\nException while inserting IOPair to the table. Duplicate record found.\n";
					break;
				}
				case 13:
				{
					returnMessage = "\nException while create IOPairs. \n";
					break;
				}
				case 14:
				{
					returnMessage = "\nException while update Passes table.\n";
					break;
				}
				default:
				{
					returnMessage = "";
					break;
				}
			}

			return returnMessage;
		}

		/// <summary>
		/// Messages for easyer traciking of Data Processing.
		/// Those messages will be written to the debug log file.
		/// </summary>
		/// <param name="messNumber"></param>
		/// <returns></returns>
		public string TrackMessage(int messNumber)
		{
			string message = "";

			switch(messNumber)
			{
				case 1:
					message = "\nProcessing will be stopped!\n";
					break;
				case 2: 
					message = "\nProcessing will be continued !\n";
					break;
				default:
					break;
			}

			return message;
		}

	}
}
