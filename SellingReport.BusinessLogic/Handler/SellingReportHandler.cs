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

        public List<SellingReportTable> GetSellingReportTable(IEnumerable<ProductSellingReport> productSellingReports, List<ProductSellingPlan> productSellingPlans, List<Country> countries, IEnumerable<Holiday> holidays, DateTime date)
        {
            const string montlyPlan = "Monthly Plan (No of Bottles)";
            const string monthlyPlanToDate = "Plan Till Date (No of Bottles)";
            const string soldPieces = "SoldPieces (No of Bottles)";
            const string soldPrecentage = "SoldPieces / Month (%)";

            var startDate = date;

            var holidayDates = holidays.Select(item => Convert.ToDateTime(item.Day + "." + item.Month + "." + startDate.Year)).ToList();
            foreach (var country in countries)
            {
                holidayDates.Add(_holidayHandler.GetEaster(date.Year, country.IsOrdthodox));
            }

            var workingDaysTillNow = _holidayHandler.GetWorkingDaysTillNow(startDate, holidayDates);
            var nonWorkingDays = _holidayHandler.GetNonWorkingDays(startDate, holidayDates);


            var allDaysInCurrentMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var workingDays = allDaysInCurrentMonth - nonWorkingDays.Count;

            //var workingDaysPercentage = workingDaysTillNow*100/workingDays;


            var sellingReports = productSellingReports as ProductSellingReport[] ?? productSellingReports.ToArray();
            //decimal productSellingOverAllReportsToDate = 0;
            //foreach (var sellingReport in sellingReports.Where(sellingReport => sellingReport.Date <= startDate))
            //{
            //    productSellingOverAllReportsToDate = + sellingReport.SoldValue;
            //}

            var productSellingReportsToDate =
                sellingReports.Where(
                    p => p.Date >= new DateTime(startDate.Year, startDate.Month, 1) && p.Date <= startDate).ToList();

            productSellingReports = sellingReports.Where(p => p.Date == date).OrderBy(p => p.ProductId).ToList();
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

            var sellingReportTempTable = new List<SellingReportTable>();
            foreach (var item in productSellingReports)
            {
                var productSellingPlan = productSellingPlans.FirstOrDefault(p => p.ProductId == item.ProductId && p.CountryId == item.CountryId);
                var productSellingReportToDate =
                    productSellingReportsToDate.Where(p => p.ProductId == item.ProductId && p.CountryId == item.CountryId).ToList();
                var productSoldPiecesToDate = 0;

                foreach (var itemReport in productSellingReportToDate)
                {
                    productSoldPiecesToDate += itemReport.SoldPieces;
                }
                if (productSellingPlan != null)
                {
                    var itemMonthlyPlan = productSellingPlan.Pieces;
                    var itemPlanToDate = productSellingPlan.Pieces * workingDaysTillNow / workingDays;
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
                    sellingReportTempTable.Add(sellingReport);

                }
            }
            sellingReportTempTable = sellingReportTempTable.GroupBy(p => p.Name)
                .Select(pl => new SellingReportTable
                {
                    Name = pl.First().Name,
                    MonthlyPlan = pl.Sum(c => Convert.ToInt32(c.MonthlyPlan)).ToString(CultureInfo.InvariantCulture),
                    MonthlyPlanToDate = pl.Sum(c => Convert.ToInt32(c.MonthlyPlanToDate)).ToString(CultureInfo.InvariantCulture),
                    SoldPieces = pl.Sum(c => Convert.ToInt32(c.SoldPieces)).ToString(CultureInfo.InvariantCulture),
                    SoldPercentage = (pl.Sum(c => Convert.ToInt32(c.MonthlyPlanToDate)) * 100 / pl.Sum(c => Convert.ToInt32(c.MonthlyPlan))).ToString(CultureInfo.InvariantCulture),
                    Image = pl.First().Image,
                    OnPlan = pl.Sum(c => Convert.ToInt32(c.MonthlyPlanToDate)) < pl.Sum(c => Convert.ToInt32(c.SoldPieces))
                }).ToList();
            sellingReportTable.AddRange(sellingReportTempTable);

            //var productSellingPlanTotalValue = productSellingPlans.Aggregate<ProductSellingPlan, decimal>(0, (current, item) => current + item.PlannedValue);
            //var productSellingTotalPercentage = Math.Round(productSellingOverAllReportsToDate * 100 / productSellingPlanTotalValue, 2);
            //var sellingReportFooter = new SellingReportTable
            //{
            //    Name = "Sum",
            //    MonthlyPlan = productSellingPlanTotalValue.ToString(CultureInfo.InvariantCulture),
            //    MonthlyPlanToDate = workingDaysPercentage.ToString(CultureInfo.InvariantCulture),
            //    SoldPieces = productSellingOverAllReportsToDate.ToString(CultureInfo.InvariantCulture),
            //    SoldPercentage = productSellingTotalPercentage.ToString(CultureInfo.InvariantCulture)
            //};
            //sellingReportTable.Add(sellingReportFooter);

            return sellingReportTable;
        }


        public SellingReportMonthlyTable GetSellingReportMonthlyTable(ProductSellingMonthlyReport productSellingMonthlyReport, ProductSellingMonthlyPlan productSellingMonthlyPlan, List<Country> countries, IEnumerable<Holiday> holidays, DateTime date)
        {
            var startDate = date;
            var holidayDates = holidays.Select(item => Convert.ToDateTime(item.Day + "." + item.Month + "." + startDate.Year)).ToList();
            foreach (var country in countries)
            {
                holidayDates.Add(_holidayHandler.GetEaster(date.Year, country.IsOrdthodox));
            }

            var workingDaysTillNow = _holidayHandler.GetWorkingDaysTillNow(startDate, holidayDates);
            var nonWorkingDays = _holidayHandler.GetNonWorkingDays(startDate, holidayDates);


            var allDaysInCurrentMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var workingDays = allDaysInCurrentMonth - nonWorkingDays.Count;


            return new SellingReportMonthlyTable
            {
                MonthlyPlan = productSellingMonthlyPlan.PlannedValue.ToString(CultureInfo.InvariantCulture),
                MonthlyPlanToDate =
                    (productSellingMonthlyPlan.PlannedValue*workingDaysTillNow/workingDays).ToString(
                        CultureInfo.InvariantCulture),
                SoldValue = productSellingMonthlyReport.SoldValue.ToString(CultureInfo.InvariantCulture)
            };
        }

        public List<SellingReportYearlyTable> GetSellingReportYearlyTable(IEnumerable<ProductSellingYearlyReport> productSellingYearlyReport)
        {
            var sellingReportYearlyTable = new List<SellingReportYearlyTable>();
            var productSellingYearlyReports = productSellingYearlyReport as ProductSellingYearlyReport[] ?? productSellingYearlyReport.ToArray();
            var products = productSellingYearlyReports.Select(item => new Product
            {
                ProductId = item.ProductId, Name = item.Product.Name
            }).ToList();

            products = products.GroupBy(p => p.ProductId)
                .Select(pl => new Product
                {
                    ProductId = pl.First().ProductId,
                    Name = pl.First().Name
                }).ToList();

            foreach (var item in products)
            {
                var product = item;
                var sellingReportYearlyReportTable = new List<ProductSellingYearlyReport>();
                for (var i = 1; i <= 12; i++)
                {
                    var count = i;
                    var productSellingMonthlyReport =
                        productSellingYearlyReports.FirstOrDefault(p => p.ProductId == product.ProductId && p.Month == count);

                    if (productSellingMonthlyReport != null) sellingReportYearlyReportTable.Add(productSellingMonthlyReport);
                }
                var yearlyAmmount = Convert.ToInt32(sellingReportYearlyReportTable.Sum(p => p.PlannedValue));
                var monthInserted = sellingReportYearlyReportTable.Count(p => p.SoldValue > 0);
                var plannedAmmount = Convert.ToInt32(sellingReportYearlyReportTable.Where(p => p.Month <= monthInserted).Sum(p => p.PlannedValue));
                var achievedAmmount = Convert.ToInt32(sellingReportYearlyReportTable.Where(p => p.Month <= monthInserted).Sum(p => p.SoldValue));
                
                var sellingReportYearlyTempTable = new SellingReportYearlyTable
                {
                    Name = product.Name,
                    ProductSellingYearlyReport = sellingReportYearlyReportTable,
                    YearlyAmmount = yearlyAmmount.ToString(CultureInfo.InvariantCulture),
                    PlannedAmmount = plannedAmmount.ToString(CultureInfo.InvariantCulture),
                    AchievedAmmount = achievedAmmount.ToString(CultureInfo.InvariantCulture)
                };

                sellingReportYearlyTable.Add(sellingReportYearlyTempTable);
            }

            return sellingReportYearlyTable;
        }
    }
}
