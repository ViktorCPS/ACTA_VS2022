using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;

namespace DataAccess
{
    public interface HolidaysExtendedDAO
    {
        int insert(HolidaysExtendedTO hTOs);

        bool delete(int recID);

        bool update(HolidaysExtendedTO hTOs);

        List<HolidaysExtendedTO> getHolidays(HolidaysExtendedTO hTOs, DateTime fromDate, DateTime toDate);


        List<string> SearchDescriptions();
    }
}
