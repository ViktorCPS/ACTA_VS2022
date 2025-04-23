using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using DataAccess;
using Util;
using TransferObjects;

namespace Common
{
    public class Machine
    {
        DAOFactory daoFactory = null;
        MachineDAO machineDAO = null;

        DebugLog log;
        MachineTO mTO = new MachineTO();

        public MachineTO MTO
        {
            get { return mTO; }
            set { mTO = value; }
        }


        public Machine()
        {
            daoFactory = DAOFactory.getDAOFactory();
            machineDAO = daoFactory.getMachineDAO(null);

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
        }

        public Machine(object dbConnection)
        {
            daoFactory = DAOFactory.getDAOFactory();
            machineDAO = daoFactory.getMachineDAO(dbConnection);

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);
        }

        public MachineTO Find(int machineID)
        {
            MachineTO machineTO = new MachineTO();

            try
            {
                machineTO = machineDAO.find(machineID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.Find(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return machineTO;
        }

        public List<EmployeeXMachineTO> FindEmployeesForMachine(int machineID)
        {
            List<EmployeeXMachineTO> employees = new List<EmployeeXMachineTO>();

            try
            {
                employees = machineDAO.findEmployeesForMachine(machineID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.FindEmployeesForMachine(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return employees;
        }

        public List<MachineTO> Search()
        {
            try
            {
                return machineDAO.getMachines(this.mTO);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.Search(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        public int Save(string name, string description)
        {
            int inserted;
            try
            {
                inserted = machineDAO.insert(name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.Save(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public bool Update(int machineID, string name, string description)
        {
            bool isUpdated;

            try
            {
                isUpdated = machineDAO.update(machineID, name, description);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.Update(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isUpdated;
        }

        public bool Delete(int machineID)
        {
            bool isDeleted = false;

            try
            {
                isDeleted = machineDAO.delete(machineID);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.Delete(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }

            return isDeleted;
        }

        public int SaveEmployeesForMachine(List<EmployeeXMachineTO> employeesForMachine)
        {
            int inserted;
            try
            {
                inserted = machineDAO.insertEmployeesForMachine(employeesForMachine);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.SaveEmployeesForMachine(): " + ex.Message + "\n");
                throw ex;
            }

            return inserted;
        }

        public bool UpdateEmployeesForMachine(List<EmployeeXMachineTO> listEmployeesXMachine)
        {
            bool updated;
            try
            {
                updated = machineDAO.deleteFromEmployeeXMachine(listEmployeesXMachine);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Machine.UpdateEmployeesForMachine(): " + ex.Message + "\n");
                throw ex;
            }

            return updated;
        }
    }
}
