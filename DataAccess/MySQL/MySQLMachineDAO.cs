using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Globalization;
using TransferObjects;
using System.Data;

namespace DataAccess.MySQL
{
    public class MySQLMachineDAO : MachineDAO
    {
        MySqlConnection conn = null;
        MySqlTransaction _sqlTrans = null;
        protected string dateTimeformat = "";

        public MySqlTransaction SqlTrans
        {
            get { return _sqlTrans; }
            set { _sqlTrans = value; }
        }

        public MySQLMachineDAO()
        {
            conn = MySQLDAOFactory.getConnection();
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }
        public MySQLMachineDAO(MySqlConnection mySqlConnection)
        {
            conn = mySqlConnection;
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", true).DateTimeFormat;
            dateTimeformat = dateTimeFormat.SortableDateTimePattern;
        }

        #region MachineDAO Members

        public int insert(string name, string description)
        {
            throw new NotImplementedException();
        }

        public bool delete(int machineID)
        {
            throw new NotImplementedException();
        }

        public bool update(int machineID, string name, string description)
        {
            throw new NotImplementedException();
        }

        public bool beginTransaction()
        {
            throw new NotImplementedException();
        }

        public void commitTransaction()
        {
            throw new NotImplementedException();
        }

        public void rollbackTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction getTransaction()
        {
            throw new NotImplementedException();
        }

        public void setTransaction(IDbTransaction trans)
        {
            throw new NotImplementedException();
        }

        public MachineTO find(int machineID)
        {
            throw new NotImplementedException();
        }

        public List<MachineTO> getMachines(MachineTO machineTO)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeXMachineTO> findEmployeesForMachine(int machineID)
        {
            throw new NotImplementedException();
        }

        public int insertEmployeesForMachine(List<EmployeeXMachineTO> employeesForMachine)
        {
            throw new NotImplementedException();
        }

        public bool deleteFromEmployeeXMachine(List<EmployeeXMachineTO> eXmTO)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
