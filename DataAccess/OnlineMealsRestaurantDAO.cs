using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface OnlineMealsRestaurantDAO
    {
        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        List<OnlineMealsRestaurantTO> getRestaurants(OnlineMealsRestaurantTO onlineRestaurant);

        int insert(string name, string description);

        bool update(int restaurantID, string name, string description);

        bool delete(int restaurantID);


    }
}
