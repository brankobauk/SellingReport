using System;
using System.Collections.Generic;
using SellingReport.BusinessLogic.Manager;
using SellingReport.Models.Models;

namespace SellingReport.BusinessLogic.Handler
{
    public class HolidayHandler
    {
        private readonly HolidayManager _holidayManager = new HolidayManager();
        public DateTime GetEaster(int year, bool isOrhtodox)
        {
            if (isOrhtodox)
            {
                return _holidayManager.GetOrthodoxEaster(year);
            }
            else
            {
                return _holidayManager.GetCatholicEaster(year);
            }
        }

        public int GetWorkingDaysTillNow(DateTime date, List<DateTime> holidays)
        {
            return _holidayManager.GetWorkingDaysTillNow(date, holidays);
        }

        public List<DateTime> GetNonWorkingDays(DateTime date, List<DateTime> holidays)
        {
            return _holidayManager.GetNonWorkingDays(date, holidays);
        }
    }
}
