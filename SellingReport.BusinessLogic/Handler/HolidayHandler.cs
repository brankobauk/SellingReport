using System;
using System.Collections.Generic;
using SellingReport.Models.Models;

namespace SellingReport.BusinessLogic.Handler
{
    public class HolidayHandler
    {
        public DateTime EasterSundayOf(int yearToCheck)
        {
            int y = yearToCheck;
            int a = y % 19;
            int b = y / 100;
            int c = y % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 2;
            var dtEasterSunday = new DateTime(yearToCheck, month, day);
            return dtEasterSunday;
        }

        public List<Holiday> GetAllHolidays()
        {
            return null;
        } 
    }
}
