using System;
using System.Collections;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface MealsPointDAO
    {
        int insert(string terminalSerial, string name, string description);

        ArrayList getMealsPoints(int pointId, string terminalSerial, string name, string description);

        int getMealsPointsCount(int pointId, string terminalSerial, string name, string description);

        bool update(int pointId, string terminalSerial, string name, string description);

        bool delete(string employeeID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
