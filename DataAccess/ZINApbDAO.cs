using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface ZINApbDAO
    {

        bool insert(int recID, int employeeID, string cardNum, string string1, string string2, string string3, string string4, string string5);

        bool update(int recID, int employeeID, string cardNum, string string1, string string2, string string3, string string4, string string5);

        bool delete(int recID);

        List<TransferObjects.ZINApbTO> getApbsAsco(int recID, int employeeID, string cardNum, string string1, string string2, string string3, string string4, string string5, string createdBy, DateTime fromTime, DateTime toTime,string wUnits);

        List<string> getDistinct(string columnName);
    }
}
