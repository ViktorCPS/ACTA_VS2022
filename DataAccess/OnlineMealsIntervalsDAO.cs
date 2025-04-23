using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;

namespace DataAccess
{
    public interface OnlineMealsIntervalsDAO
    {

        List<OnlineMealsIntervalsTO> getAll(string type);
    }
}
