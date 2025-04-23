using System;
using System.Collections;
using System.Data;

using TransferObjects;
namespace DataAccess
{
    public interface MealTypeAdditionalDataDAO
    {
        int insert(int mealTypeID, string descriptionAdd, byte[] picture);
        int insert(int mealTypeID, string descriptionAdd, byte[] picture, bool doCommit);
        bool delete(int mealTypeId);
        bool delete(int mealTypeId, bool doCommit);
        bool update(int mealTypeID, string descriptionAdd, byte[] picture);
        bool update(int mealTypeID, string descriptionAdd, byte[] picture, bool doCommit);

        MealTypeAdditionalDataTO getAdditionalData(int mealTypeID);
        int tableExists();

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);
    }
}
