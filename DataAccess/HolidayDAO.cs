using System;
using System.Collections;
using System.Collections.Generic;

using TransferObjects;

namespace DataAccess
{
	/// <summary>
	/// Summary description for HolidayDAO.
	/// </summary>
	public interface HolidayDAO
	{
		int insert(string description, DateTime holidayDate);

		bool delete(DateTime holidayDate);

		bool update(string description, DateTime oldHolidayDate, DateTime newHolidayDate);

		HolidayTO find(DateTime holidayDate);

		List<HolidayTO> getHolidays(HolidayTO hTOs, DateTime fromDate, DateTime toDate);

		void serialize(List<HolidayTO> HolidayTOList);
	}
}
