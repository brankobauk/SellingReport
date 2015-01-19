using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SellingReport.BusinessLogic.Handler;
using SellingReport.Context;
using SellingReport.Helper;
using SellingReport.Models.Models;
using SellingReport.Models.ViewModels;
using PagedList;

namespace SellingReport.Controllers
{
    [Authorize]
    public class ProductSellingMonthlyReportController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        private readonly HolidayHandler _holidayHandler = new HolidayHandler();
        //
        // GET: /ProductSellingMonthlyReport/

        public ActionResult Index()
        {
            var page = Convert.ToInt32(Request.QueryString["page"]);
            if (page == 0)
            {
                page = 1;
            }
            var productSellingMonthlyReport = _db.ProductSellingMonthlyReports.Where(p => p.CountryId == p.Country.CountryId).Include(p => p.Country).OrderByDescending(p => p.Date).ThenBy(p => p.CountryId);
            var pagedProductSellingMonthlyReport = new PagedList<ProductSellingMonthlyReport>(productSellingMonthlyReport, page, 20);
            return View(pagedProductSellingMonthlyReport);
        }


        //
        // GET: /ProductSellingMonthlyReport/Create

        public ActionResult Create()
        {
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var holidays = _db.Holidays.ToList();
            var holidayDates = new List<HolidayDates>();
            foreach (var country in _db.Countries.ToList())
            {
                var countryId = Convert.ToInt32(country.CountryId);
                holidays = holidays.Where(p => p.CountryId == countryId).ToList();
                var holidayDatesString = "";
                foreach (var item in holidays)
                {
                    if (holidayDatesString == "")
                    {
                        holidayDatesString += item.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + "." + item.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ".";
                    }
                    else
                    {
                        holidayDatesString += "," + item.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + "." + item.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ".";
                    }
                }
                for (var i = 2010; i <= DateTime.Now.Year; i++)
                {
                    holidayDatesString += "," + _holidayHandler.GetEaster(i, country.IsOrdthodox).ToString("dd.MM.yyyy");
                }

                var holiday = new HolidayDates
                {
                    CountryId = countryId,
                    HolidayDatesString = holidayDatesString
                };
                holidayDates.Add(holiday);
            }
            var productSellingMonthlyReport = new ProductSellingMonthlyReportViewModel
            {
                Countries = countries,
                HolidayDates = holidayDates
            };
            return View(productSellingMonthlyReport);
        }

        //
        // POST: /ProductSellingMonthlyReport/Create

        [HttpPost]
        public ActionResult Create(ProductSellingMonthlyReportViewModel productSellingMonthlyReportViewModel)
        {
            try
            {
                _db.ProductSellingMonthlyReports.Add(productSellingMonthlyReportViewModel.ProductSellingMonthlyReport);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /ProductSellingMonthlyReport/Edit/5

        public ActionResult Edit(int id)
        {
            var productSellingMonthlyReportToEdit = _db.ProductSellingMonthlyReports.FirstOrDefault(p => p.ProductSellingMonthlyReportId == id);
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var productSellingMonthlyReportViewModel = new ProductSellingMonthlyReportViewModel
            {
                ProductSellingMonthlyReport = productSellingMonthlyReportToEdit,
                Countries = countries
            };
            return View(productSellingMonthlyReportViewModel);
        }

        //
        // POST: /ProductSellingMonthlyReport/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ProductSellingMonthlyReportViewModel productSellingMonthlyReportViewModel)
        {
            try
            {
                var productSellingMonthlyReportToEdit = _db.ProductSellingMonthlyReports.FirstOrDefault(p => p.ProductSellingMonthlyReportId == id);
                if (productSellingMonthlyReportToEdit != null)
                {
                    productSellingMonthlyReportToEdit.CountryId = productSellingMonthlyReportViewModel.ProductSellingMonthlyReport.CountryId;
                    productSellingMonthlyReportToEdit.Date = productSellingMonthlyReportViewModel.ProductSellingMonthlyReport.Date;
                    productSellingMonthlyReportToEdit.SoldValue = productSellingMonthlyReportViewModel.ProductSellingMonthlyReport.SoldValue;
                    _db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductSellingMonthlyReport/Delete/5

        public ActionResult Delete(int id)
        {
            var productSellingMonthlyReportToDelete = _db.ProductSellingMonthlyReports.FirstOrDefault(p => p.ProductSellingMonthlyReportId == id);
            _db.ProductSellingMonthlyReports.Remove(productSellingMonthlyReportToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
