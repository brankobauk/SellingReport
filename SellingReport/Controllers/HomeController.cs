using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SellingReport.BusinessLogic.Handler;
using SellingReport.Context;
using SellingReport.Models.ViewModels;

namespace SellingReport.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly SellingReportHandler _sellingReportHandler = new SellingReportHandler();
        private readonly DateHandler _dateHandler = new DateHandler();
        public ActionResult Index()
        {
            
            var countryId = Convert.ToInt32(Request.QueryString["countryId"]);
            if (countryId == 0)
            {
                countryId = 1;
            }
            var countries = _db.Countries.Where(p => p.CountryId == countryId).ToList().FirstOrDefault();
            
            var productSellingReport = _db.ProductSellingReports.Where(p=>p.CountryId == countryId && p.CountryId == p.Country.CountryId && p.ProductId == p.Product.ProductId).Include(p => p.Product).Include(p => p.Country).OrderBy(p => p.Product.Name).ToList();
            var date = Request.QueryString["date"] ?? _dateHandler.GetLastActivityDate(productSellingReport);
            var reportDate = Convert.ToDateTime(date);
            var productSellingPlan = _db.ProductSellingPlans.Where(p=>p.CountryId == countryId && p.CountryId == p.Country.CountryId && p.ProductId == p.Product.ProductId).Include(p => p.Product).Include(p => p.Country).OrderBy(p => p.Product.Name).ToList();
            var holiday = _db.Holidays.Where(p => p.CountryId == countryId).ToList();
            var sellingReportTable = _sellingReportHandler.GetSellingReportTable(productSellingReport, productSellingPlan, countries, holiday, reportDate);
            var sellingReportTableViewModel = new SellingReportTableViewModel
            {
                Date = date,
                SellingReportTable = sellingReportTable
            };
            return View(sellingReportTableViewModel);
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
    }
}
