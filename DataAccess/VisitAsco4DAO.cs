using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface VisitAsco4DAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        bool insert(int visitID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5, DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10);
        bool insert(int visitID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5, DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10, bool doCommit);

        bool update(int visitID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5, DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10);
        bool update(int visitID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5, DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10, bool doCommit);

        bool delete(int visitID);
        bool delete(int visitID, bool doCommit);

        List<VisitAsco4TO> getVisitsAsco(int visitID, int int1, int int2, int int3, int int4, int int5, int int6, int int7, int int8, int int9, int int10, DateTime dateTime1, DateTime dateTime2, DateTime dateTime3, DateTime dateTime4, DateTime dateTime5, DateTime dateTime6, DateTime dateTime7, DateTime dateTime8, DateTime dateTime9, DateTime dateTime10, string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10);

        List<string> getDistinctVisitors();

        List<string> getDistinctDocumentNames();

        
        List<string> getDistinct(string column);

        DateTime getPrivacyStatement(string JMBG, string documentNumber);

        int enterBan(string visitID, int banActive, DateTime banDate);

        List<VisitAsco4TO> getVisitAscoForID(string JMBG, string documentNumber);

        List<VisitAsco4TO> getVisitsDetailsAsco(string visitIDs);

        List<VisitAsco4TO> getVisitAsco(string firstName, string lastName, string jmbg, string documentNum, string company);

        List<VisitTO> getVisitAsco(string firstName, string lastName, string jmbg, Dictionary<int, List<VisitAsco4TO>> ascoTO);
    }
}
