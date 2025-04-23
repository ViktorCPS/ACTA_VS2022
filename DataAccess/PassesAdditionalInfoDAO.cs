using System;
using System.Collections.Generic;
using System.Text;

using TransferObjects;
using System.Data;

namespace DataAccess
{
    public interface PassesAdditionalInfoDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        void setTransaction(IDbTransaction trans);

        IDbTransaction getTransaction();

        PassesAdditionalInfoTO find(uint passID);

        int insert(PassesAdditionalInfoTO passesAdditionalInfoTO, bool doCommit);

        bool update(PassesAdditionalInfoTO passesAdditionalInfoTO, bool doCommit);

        List<PassesAdditionalInfoTO> getPasses(PassesAdditionalInfoTO passesAdditionalInfoTO);

        bool delete(uint passId, bool doCommit);
    }
}
