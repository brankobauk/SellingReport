using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using SellingReport.BusinessLogic.Manager;
using SellingReport.Models.Models;

namespace SellingReport.BusinessLogic.Handler
{
    
    public class SellingReportHandler
    {
        private readonly SellingReportManager _sellingReportManager = new SellingReportManager();
        private readonly HolidayHandler _holidayHandler = new HolidayHandler();

        public List<SellingReportTable> GetSellingReportTable(IEnumerable<ProductSellingReport> productSellingReports, List<ProductSellingPlan> productSellingPlans,Country country, IEnumerable<Holiday> holidays, DateTime date)
        {
            const string montlyPlan = "Monthly Plan (No of Bottles)";
            const string monthlyPlanToDate = "Plan Till Date (No of Bottles)";
            const string soldPieces = "SoldPieces (No of Bottles)";
            const string soldPrecentage = "SoldPieces / Month (%)";
            
            var startDate = date;
            
            var holidayDates = holidays.Select(item => Convert.ToDateTime(item.Day + "." + item.Month + "." + startDate.Year)).ToList();
            holidayDates.Add(_holidayHandler.GetEaster(date.Year, country.IsOrdthodox));

            var workingDaysTillNow = _holidayHandler.GetWorkingDaysTillNow(startDate, holidayDates);
            var nonWorkingDays = _holidayHandler.GetNonWorkingDays(startDate, holidayDates);
            

            var allDaysInCurrentMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var workingDays = allDaysInCurrentMonth - nonWorkingDays.Count;

            var workingDaysPercentage = workingDaysTillNow*100/workingDays;


            var sellingReports = productSellingReports as ProductSellingReport[] ?? productSellingReports.ToArray();
            decimal productSellingOverAllReportsToDate = 0;
            foreach (var sellingReport in sellingReports.Where(sellingReport => sellingReport.Date <= startDate))
            {
                productSellingOverAllReportsToDate = + sellingReport.SoldValue;
            }

            var productSellingReportsToDate =
                sellingReports.Where(
                    p => p.Date >= new DateTime(startDate.Year, startDate.Month, 1) && p.Date <= startDate).ToList();

            productSellingReports = sellingReports.Where(p => p.Date == date).ToList();
            productSellingPlans = productSellingPlans.Where(p => p.Month == date.Month && p.Year == date.Year).ToList();


            var sellingReportTable = new List<SellingReportTable>();

            var sellingReportHeader = new SellingReportTable
            {
                Name = "",
                MonthlyPlan = montlyPlan,
                MonthlyPlanToDate = monthlyPlanToDate,
                SoldPieces = soldPieces,
                SoldPercentage = soldPrecentage
            };
            sellingReportTable.Add(sellingReportHeader);

            foreach (var item in productSellingReports)
            {
                var productSellingPlan = productSellingPlans.FirstOrDefault(p => p.ProductId == item.ProductId);
                var productSellingReportToDate =
                    productSellingReportsToDate.Where(p => p.ProductId == item.ProductId).ToList();
                var productSoldPiecesToDate = 0;
                foreach (var itemReport in productSellingReportToDate)
                {
                    productSoldPiecesToDate += itemReport.SoldPieces;
                }
                if (productSellingPlan != null)
                {
                    var itemMonthlyPlan = productSellingPlan.Pieces;
                    var itemPlanToDate = productSellingPlan.Pieces * workingDaysTillNow/workingDays;
                    var itemSellingPercentage = productSoldPiecesToDate * 100 / itemMonthlyPlan;
                    var onplan = itemPlanToDate < productSoldPiecesToDate;
                    var sellingReport = new SellingReportTable
                    {
                        Name = item.Product.Name,
                        MonthlyPlan = itemMonthlyPlan.ToString(CultureInfo.InvariantCulture),
                        MonthlyPlanToDate = itemPlanToDate.ToString(CultureInfo.InvariantCulture),
                        SoldPieces = productSoldPiecesToDate.ToString(CultureInfo.InvariantCulture),
                        SoldPercentage = itemSellingPercentage.ToString(CultureInfo.InvariantCulture),
                        Image = item.Product.Image,
                        OnPlan = onplan
                    };
                    sellingReportTable.Add(sellingReport);
                }
            }

            var productSellingPlanTotalValue = productSellingPlans.Aggregate<ProductSellingPlan, decimal>(0, (current, item) => current + item.PlannedValue);
            var productSellingTotalPercentage = Math.Round(productSellingOverAllReportsToDate*100/productSellingPlanTotalValue, 2);
            var sellingReportFooter = new SellingReportTable
            {
                Name = "Sum",
                MonthlyPlan = productSellingPlanTotalValue.ToString(CultureInfo.InvariantCulture),
                MonthlyPlanToDate = workingDaysPercentage.ToString(CultureInfo.InvariantCulture),
                SoldPieces = productSellingOverAllReportsToDate.ToString(CultureInfo.InvariantCulture),
                SoldPercentage = productSellingTotalPercentage.ToString(CultureInfo.InvariantCulture)
            };
            sellingReportTable.Add(sellingReportFooter);

            return sellingReportTable;
        }

        
    }
}
