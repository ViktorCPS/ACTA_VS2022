using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    /// <summary>
    /// LicenceDAO interface is implemented by 
    /// database specific Licence DAO classes
    /// </summary>
    public interface LicenceDAO
    {
        int insert(string licenceKey, bool doCommit);

        bool delete(string licenceKey);

        bool update(int recID, string licenceKey, bool doCommit);

        LicenceTO findMAX();

        List<LicenceTO> getLicences(LicenceTO licTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();
        
        void setTransaction(IDbTransaction trans);
    }
}
