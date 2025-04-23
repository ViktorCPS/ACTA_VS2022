using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface ApplUserCategoryDAO
    {
        int insert(ApplUserCategoryTO applUserCategoryTO, bool doCommit);

        bool delete(int categoryID);

        bool update(ApplUserCategoryTO applUserCategoryTO, bool doCommit);

        ApplUserCategoryTO find(int categoryID);

        List<ApplUserCategoryTO> getUserCategories(ApplUserCategoryTO applUserCategoryTO, bool includeMCCategories);

        List<ApplUserCategoryTO> getUserCategoriesForLoginUser(string userID, bool getDefault);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        List<ApplUserCategoryTO> getUserCategories(ApplUserCategoryTO applUserCategoryTO);
    }
}
