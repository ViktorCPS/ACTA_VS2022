using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface ApplUserCategoryXPassTypeDAO
    {
        int insert(ApplUserCategoryXPassTypeTO userCatXPtTO, bool doCommit);

        bool delete(int categoryID, int ptID, string purpose);        

        List<ApplUserCategoryXPassTypeTO> getUserCategoriesXPassTypes(ApplUserCategoryXPassTypeTO userCatXPtTO);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        Dictionary<int, List<int>> getUserCategoriesXPassTypesDictionary(ApplUserCategoryXPassTypeTO applUserCategoryXPassTypeTO);

        bool delete(int passTypeID, bool doCommit);
    }
}
