using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface PassTypesConfirmationDAO
    {
        int insert(PassTypesConfirmationTO ptcTO);

        int insert(PassTypesConfirmationTO ptcTO, bool doCommit);

        bool delete(int passTypeID, int confirmPTID);

        bool delete(int passTypeID, int confirmPTID, bool doCommit);

        bool delete(int passTypeID, bool doCommit);

        List<PassTypesConfirmationTO> getPTConfirmation(PassTypesConfirmationTO auXwuTO);

        Dictionary<int, List<int>> getPTConfirmationDictionary(PassTypesConfirmationTO ptConfirmTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
