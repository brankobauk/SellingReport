using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SellingReport.Models.Models;

namespace SellingReport.BusinessLogic.Manager
{
    public class HolidayManager
    {
        public DateTime GetOrthodoxEaster(int year)
        {
            var a = year % 19;
            var b = year % 7;
            var c = year % 4;

            var d = (19 * a + 16) % 30;
            var e = (2 * c + 4 * b + 6 * d) % 7;
            var f = (19 * a + 16) % 30;
            var key = f + e + 3;

            var month = (key > 30) ? 5 : 4;
            var day = (key > 30) ? key - 30 : key;

            return new DateTime(year, month, day);
        }

        public DateTime GetCatholicEaster(int year)
        {
            var month = 3;
            var g = year % 19 + 1;
            var c = year / 100 + 1;
            var x = (3 * c) / 4 - 12;
            var y = (8 * c + 5) / 25 - 5;
            var z = (5 * year) / 4 - x - 10;
            var e = (11 * g + 20 + y - x) % 30;
            if (e == 24) { e++; }
            if ((e == 25) && (g > 11)) { e++; }
            var n = 44 - e;
            if (n < 21) { n = n + 30; }
            var p = (n + 7) - ((z + n) % 7);
            if (p <= 31) return new DateTime(year, month, p);
            p = p - 31;
            month = 4;
            return new DateTime(year, month, p);
        }

        public int GetWorkingDaysTillNow(DateTime date, List<DateTime> holidays)
        {
            var workingDaysTillNow = 0;
            var nonWorkingDays = 0;
            var firstDayInMonth = new DateTime(date.Year, date.Month, 1);
            for (var i = 0; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                var isHoliday = false;
                var nextDay = firstDayInMonth.AddDays(i);

                if (holidays.Contains(nextDay.Date))
                {
                    isHoliday = true;
                }
                if (nextDay.DayOfWeek != DayOfWeek.Saturday && nextDay.DayOfWeek != DayOfWeek.Sunday && !isHoliday)
                {
                    if (nextDay == date)
                    {
                        workingDaysTillNow = i+1 - nonWorkingDays;
                    }
                }
                else
                {
                    nonWorkingDays = nonWorkingDays + 1;
                }
            }
            return workingDaysTillNow;
        }

        public List<DateTime> GetNonWorkingDays(DateTime date, List<DateTime> holidays)
        {
            var nonWorkingDays = new List<DateTime>();
            
            var firstDayInMonth = new DateTime(date.Year, date.Month, 1);
            for (var i = 0; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                var isHoliday = false;
                var nextDay = firstDayInMonth.AddDays(i);

                if (holidays.Contains(nextDay.Date))
                {
                    isHoliday = true;
                }
                if (nextDay.DayOfWeek == DayOfWeek.Saturday || nextDay.DayOfWeek == DayOfWeek.Sunday || isHoliday)
                {
                    nonWorkingDays.Add(nextDay);
                }
            }
            return nonWorkingDays;
        }
    }
}
