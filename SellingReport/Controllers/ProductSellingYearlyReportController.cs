using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SellingReport.Helper;
using SellingReport.Context;
using SellingReport.Models.Models;
using SellingReport.Models.ViewModels;
using PagedList;

namespace SellingReport.Controllers
{
    [Authorize]
    public class ProductSellingYearlyReportController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        //
        // GET: /ProductSellingReport/

        public ActionResult Index()
        {
            var page = Convert.ToInt32(Request.QueryString["page"]);
            if (page == 0)
            {
                page = 1;
            }
            var productSellingYearlyReport = _db.ProductSellingYearlyReports.Where(p => p.CountryId == p.Country.CountryId && p.ProductId == p.Product.ProductId).Include(p => p.Product).Include(p => p.Country).OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ThenBy(p => p.CountryId).ThenBy(p => p.ProductId);
            var pagedProductSellingYearlyReport = new PagedList<ProductSellingYearlyReport>(productSellingYearlyReport, page, 20);
            return View(pagedProductSellingYearlyReport);
        }

        public ActionResult Create()
        {
            var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var months = _dropDownHelper.GetMonthsListForDropDown();
            var years = _dropDownHelper.GetYearsListForDropDown();
            var productSellingYearlyReport = new ProductSellingYearlyReportViewModel
            {
                ProductSellingYearlyReport = new ProductSellingYearlyReport()
                {
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month
                },
                Month = months,
                Year = years,
                Countries = countries,
                Products = products
            };
            return View(productSellingYearlyReport);
        }

        //
        // POST: /ProductSellingReport/Create

        [HttpPost]
        public ActionResult Create(ProductSellingYearlyReportViewModel productSellingReportMonthlyReportViewModel)
        {
            try
            {
                _db.ProductSellingYearlyReports.Add(productSellingReportMonthlyReportViewModel.ProductSellingYearlyReport);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductSellingReport/Edit/5

        public ActionResult Edit(int id)
        {
            var productSellingYearlyReportToEdit = _db.ProductSellingYearlyReports.FirstOrDefault(p => p.ProductSellingYearlyReportId == id);
            if (productSellingYearlyReportToEdit != null)
            {
                var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
                var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
                var months = _dropDownHelper.GetMonthsListForDropDown();
                var years = _dropDownHelper.GetYearsListForDropDown();
                var productSellingYearlyReport = new ProductSellingYearlyReportViewModel
                {
                    ProductSellingYearlyReport = productSellingYearlyReportToEdit,
                    Month = months,
                    Year = years,
                    Countries = countries,
                    Products = products
                };
                return View(productSellingYearlyReport);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /ProductSellingReport/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ProductSellingYearlyReportViewModel productSellingYearlyReportViewModel)
        {
            try
            {
                var productSellingYearlyReportToEdit = _db.ProductSellingYearlyReports.FirstOrDefault(p => p.ProductSellingYearlyReportId == id);
                if (productSellingYearlyReportToEdit != null)
                {
                    productSellingYearlyReportToEdit.ProductId = productSellingYearlyReportViewModel.ProductSellingYearlyReport.ProductId;
                    productSellingYearlyReportToEdit.CountryId = productSellingYearlyReportViewModel.ProductSellingYearlyReport.CountryId;
                    productSellingYearlyReportToEdit.Month = productSellingYearlyReportViewModel.ProductSellingYearlyReport.Month;
                    productSellingYearlyReportToEdit.Year = productSellingYearlyReportViewModel.ProductSellingYearlyReport.Year;
                    productSellingYearlyReportToEdit.PlannedValue = productSellingYearlyReportViewModel.ProductSellingYearlyReport.PlannedValue;
                    productSellingYearlyReportToEdit.PlannedPieces = productSellingYearlyReportViewModel.ProductSellingYearlyReport.PlannedPieces;
                    productSellingYearlyReportToEdit.SoldValue = productSellingYearlyReportViewModel.ProductSellingYearlyReport.SoldValue;
                    productSellingYearlyReportToEdit.SoldPieces = productSellingYearlyReportViewModel.ProductSellingYearlyReport.SoldPieces;
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
            var productSellingYearlyReportToDelete = _db.ProductSellingYearlyReports.FirstOrDefault(p => p.ProductSellingYearlyReportId == id);
            _db.ProductSellingYearlyReports.Remove(productSellingYearlyReportToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}