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
    public class ProductSellingReportController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        private readonly HolidayHandler _holidayHandler = new HolidayHandler();
        //
        // GET: /ProductSellingReport/

        public ActionResult Index()
        {
            var page = Convert.ToInt32(Request.QueryString["page"]);
            if (page == 0)
            {
                page = 1;
            }
            var productSellingReport = _db.ProductSellingReports.Where(p => p.CountryId == p.Country.CountryId && p.ProductId == p.Product.ProductId).Include(p => p.Product).Include(p => p.Country).OrderByDescending(p => p.Date).ThenBy(p => p.CountryId).ThenBy(p => p.ProductId);
            var pagedProductSellingReport = new PagedList<ProductSellingReport>(productSellingReport, page, 20);
            return View(pagedProductSellingReport);
        }


        //
        // GET: /ProductSellingReport/Create

        public ActionResult Create()
        {
            var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
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
                for (var i = 2010; i <= DateTime.Now.Year; i++) {
                    holidayDatesString += "," + _holidayHandler.GetEaster(i, country.IsOrdthodox).ToString("dd.MM.yyyy");
                }
                
                var holiday = new HolidayDates()
                {
                    CountryId = countryId,
                    HolidayDatesString = holidayDatesString
                };
                holidayDates.Add(holiday);
            }
            var productSellingReport = new ProductSellingReportViewModel
            {
                Countries = countries,
                Products = products,
                HolidayDates = holidayDates
            };
            return View(productSellingReport);
        }

        //
        // POST: /ProductSellingReport/Create

        [HttpPost]
        public ActionResult Create(ProductSellingReportViewModel productSellingReportViewModel)
        {
            try
            {
                _db.ProductSellingReports.Add(productSellingReportViewModel.ProductSellingReport);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /ProductSellingReport/Edit/5

        public ActionResult Edit(int id)
        {
            var productSellingReportToEdit = _db.ProductSellingReports.FirstOrDefault(p => p.ProductSellingReportId == id);
            var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var productSellingReportViewModel = new ProductSellingReportViewModel
            {
                ProductSellingReport = productSellingReportToEdit,
                Countries = countries,
                Products = products
            };
            return View(productSellingReportViewModel);
        }

        //
        // POST: /ProductSellingReport/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ProductSellingReportViewModel productSellingReportViewModel)
        {
            try
            {
                var productSellingReportToEdit = _db.ProductSellingReports.FirstOrDefault(p => p.ProductSellingReportId == id);
                if (productSellingReportToEdit != null)
                {
                    productSellingReportToEdit.ProductId = productSellingReportViewModel.ProductSellingReport.ProductId;
                    productSellingReportToEdit.CountryId = productSellingReportViewModel.ProductSellingReport.CountryId;
                    productSellingReportToEdit.Date = productSellingReportViewModel.ProductSellingReport.Date;
                    productSellingReportToEdit.SoldPieces = productSellingReportViewModel.ProductSellingReport.SoldPieces;
                    productSellingReportToEdit.SoldValue = productSellingReportViewModel.ProductSellingReport.SoldValue;
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
        // GET: /ProductSellingReport/Delete/5

        public ActionResult Delete(int id)
        {
            var productSellingReportToDelete = _db.ProductSellingReports.FirstOrDefault(p => p.ProductSellingReportId == id);
            _db.ProductSellingReports.Remove(productSellingReportToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
