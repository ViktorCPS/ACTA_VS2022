using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using TransferObjects;

namespace DataAccess
{
    public interface ApplUserXApplUserCategoryDAO
    {
        int insert(ApplUserXApplUserCategoryTO applUserXCategoryTO, bool doCommit);

        bool delete(int categoryID, string userID);

        bool update(ApplUserXApplUserCategoryTO applUserXCategoryTO, bool doCommit);

        bool setDefaultCategory(ApplUserXApplUserCategoryTO applUserXCategoryTO);

        List<ApplUserXApplUserCategoryTO> getUserCategories(ApplUserXApplUserCategoryTO applUserXCategoryTO);

        List<ApplUserXApplUserCategoryTO> getUserCategories(ApplUserXApplUserCategoryTO applUserXCategoryTO, IDbTransaction trans);

        List<int> getSupervisors(string emplIDs);

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        bool delete(string userID, bool doCommit);

        Dictionary<string, List<int>> getUserCategoriesDict();

        bool delete(int categoryID, string userID, bool doCommit);
    }
}
