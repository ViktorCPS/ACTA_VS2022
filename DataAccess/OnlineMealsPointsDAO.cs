using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TransferObjects;

namespace DataAccess
{
    public interface OnlineMealsPointsDAO
    {

        bool beginTransaction();

        void commitTransaction();

        void rollbackTransaction();

        IDbTransaction getTransaction();

        void setTransaction(IDbTransaction trans);

        List<OnlineMealsPointsTO> getMealsPoints(string ipAddress, int ant_num);

        List<OnlineMealsPointsTO> getMealsPoints(OnlineMealsPointsTO onlineMealsPointTO);

        int insert(OnlineMealsPointsTO onlineMealsPointTO);

        bool update(OnlineMealsPointsTO onlineMealsPointTO);

        bool delete(int pointID);

        List<OnlineMealsPointsTO> searchForIDs(string pointID);

        OnlineMealsPointsTO find(string pointID);

        List<OnlineMealsPointsTO> findByRestaurant(string restaurantID);
    }
}
