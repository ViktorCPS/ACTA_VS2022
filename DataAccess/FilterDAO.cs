using System;
using System.Collections;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface FilterDAO
    {
        string serialize(ArrayList filtersTOList);
        ArrayList deserialize(string xml);

        bool save(FilterTO filterTO, bool doCommit);

        FilterTO find(int filterID);

        ArrayList search(string menuItem, string user,string tabID);

        ArrayList getDefaults(string menuItem, string user, string tabID);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        bool setNoDefaults(string menuItem, string user, string tabID, bool doCommit);

        bool delete(int filterID);

        bool update(FilterTO filterTO, int filterID, bool doCommit);
        
    }
}
