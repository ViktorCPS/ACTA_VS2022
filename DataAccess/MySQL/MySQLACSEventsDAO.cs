using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text;

using TransferObjects;

namespace DataAccess.MySQL
{
    public class MySQLACSEventsDAO: ACSEventsDAO
	{
		MySqlConnection conn = null;
		MySqlTransaction _sqlTrans = null;
		protected string dateTimeformat = "";

		public MySQLACSEventsDAO()
		{
			conn = MySQLDAOFactory.getConnection();
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
		}
        public MySQLACSEventsDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
			DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
			dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        #region ACSEventsDAO Members

        public List<ACSEventTO> findAll()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
