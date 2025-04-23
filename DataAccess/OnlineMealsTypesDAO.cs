using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
  public  interface OnlineMealsTypesDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        List<OnlineMealsTypesTO> getTypes(OnlineMealsTypesTO onlineMealsTO);

        bool delete(int mealTypeID);

        int insert(string name, string description);

        bool update(int mealTypeID, string name, string description);
    }
}
