using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DataAccess
{
  public  interface EmployeeAsco4HistDAO
  {
        bool insert(int emplID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5,
          DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10,string createdBy, DateTime createdTime, bool doCommit);
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();
    }
}
