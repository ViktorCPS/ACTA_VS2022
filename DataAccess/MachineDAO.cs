using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface MachineDAO
    {
        int insert(string name, string description);

        bool delete(int machineID);

        bool update(int machineID, string name, string description);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        MachineTO find(int machineID);

        List<MachineTO> getMachines(MachineTO machineTO);

        List<EmployeeXMachineTO> findEmployeesForMachine(int machineID);

        int insertEmployeesForMachine(List<EmployeeXMachineTO> employeesForMachine);

        bool deleteFromEmployeeXMachine(List<EmployeeXMachineTO> eXmTO);
    }
}
