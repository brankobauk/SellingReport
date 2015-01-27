using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellingReport.BusinessLogic.Handler;
using SellingReport.Context;
using SellingReport.Models.Models;
using SellingReport.Models.ViewModels;

namespace SellingReport.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SellingReportContext _db = new SellingReportContext();
        private readonly SellingReportHandler _sellingReportHandler = new SellingReportHandler();
        private readonly DateHandler _dateHandler = new DateHandler();

        public ActionResult Index()
        {

            var countries = _db.Countries.ToList();
            return View(countries);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CountryReport()
        {
            try
            {
                var countryId = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["countryId"]));
                if (countryId == 0)
                {
                    countryId = 1;
                }
                var countries = _db.Countries.Where(p => p.CountryId == countryId).ToList();

                var productSellingReport =
                    _db.ProductSellingReports.Where(
                        p =>
                            p.CountryId == countryId && p.CountryId == p.Country.CountryId &&
                            p.ProductId == p.Product.ProductId)
                        .Include(p => p.Product)
                        .Include(p => p.Country)
                        .OrderBy(p => p.Product.Name)
                        .ToList();

                var date = Request.QueryString["date"];
                if (date == "")
                {
                    date = _dateHandler.GetLastActivityDate(productSellingReport);
                }
                var reportDate = Convert.ToDateTime(date);
                var productSellingPlan =
                    _db.ProductSellingPlans.Where(
                        p =>
                            p.CountryId == countryId && p.CountryId == p.Country.CountryId &&
                            p.ProductId == p.Product.ProductId)
                        .Include(p => p.Product)
                        .Include(p => p.Country)
                        .OrderBy(p => p.Product.Name)
                        .ToList();
                var holiday = _db.Holidays.Where(p => p.CountryId == countryId).ToList();
                var sellingReportTable = _sellingReportHandler.GetSellingReportTable(productSellingReport,
                    productSellingPlan, countries, holiday, reportDate);

                var productSellingMonthlyReport = _db.ProductSellingMonthlyReports.FirstOrDefault(p => p.Date == reportDate);
               SellingReportMonthlyTable sellingReportMonthlyTable = null;
                if (productSellingMonthlyReport != null) {
                    var productSellingMonthlyPlan = _db.ProductSellingMonthlyPlans.FirstOrDefault(p => p.Month == reportDate.Month && p.Year == reportDate.Year);
                    sellingReportMonthlyTable = _sellingReportHandler.GetSellingReportMonthlyTable(productSellingMonthlyReport, productSellingMonthlyPlan, countries, holiday, reportDate);
                }
                var productSellingYearlyReport =
                    _db.ProductSellingYearlyReports.Where(p => p.Year == reportDate.Year && p.CountryId==countryId)
                        .ToList();
                var sellingReportYearlyTable = _sellingReportHandler.GetSellingReportYearlyTable(productSellingYearlyReport);
                var sellingReportTableViewModel = new SellingReportTableViewModel
                {
                    Date = date,
                    SellingReportTable = sellingReportTable,
                    SellingReportMonthlyTable = sellingReportMonthlyTable,
                    SellingReportYearlyTable = sellingReportYearlyTable
                };
                return View("_Report", sellingReportTableViewModel);
            }
            catch
            {
                return View("_Report", null);
            }
        }

        public ActionResult Report()
        {
            var countries = _db.Countries.ToList();
            var productSellingReport =
                    _db.ProductSellingReports.Where(
                        p =>
                            p.ProductId == p.Product.ProductId)
                        .Include(p => p.Product)
                        .OrderBy(p => p.Product.Name)
                        .ToList();
            var date = Request.QueryString["date"];
            if (date == null)
            {
                date = _dateHandler.GetLastActivityDate(productSellingReport);
            }
            var reportDate = Convert.ToDateTime(date);
            var productSellingPlan =
                _db.ProductSellingPlans.Where(
                    p =>
                        p.ProductId == p.Product.ProductId)
                    .Include(p => p.Product)
                    .Include(p => p.Country)
                    .OrderBy(p => p.Product.Name)
                    .ToList();
            var holiday = _db.Holidays.ToList();
            var sellingReportTable = _sellingReportHandler.GetSellingReportTable(productSellingReport,
                productSellingPlan, countries, holiday, reportDate);
            var sellingReportTableViewModel = new SellingReportTableViewModel
            {
                Date = date,
                SellingReportTable = sellingReportTable
            };
            return View("_Report", sellingReportTableViewModel);
        }
    }
}
