using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SellingReport.Context;
using SellingReport.Helper;
using SellingReport.Models.Models;
using SellingReport.Models.ViewModels;

namespace SellingReport.Controllers
{
    [Authorize]
    public class ProductSellingReportController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        //
        // GET: /ProductSellingReport/

        public ActionResult Index()
        {
            var productSellingReport = _db.ProductSellingReports.Where(p => p.CountryId == p.Country.CountryId && p.ProductId == p.Product.ProductId).Include(p => p.Product).Include(p => p.Country).OrderByDescending(p => p.Date).ThenBy(p => p.CountryId).ThenBy(p => p.ProductId);
            return View(productSellingReport);
        }


        //
        // GET: /ProductSellingReport/Create

        public ActionResult Create()
        {
            var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var productSellingReport = new ProductSellingReportViewModel
            {
                ProductSellingReport = new ProductSellingReport()
                {
                    Date = DateTime.Now.Date
                },
                Countries = countries,
                Products = products
            };
            return View(productSellingReport);
        }

        //
        // POST: /ProductSellingReport/Create

        [HttpPost]
        public ActionResult Create(ProductSellingReportViewModel productSellingReportViewModel)
        {
            
                _db.ProductSellingReports.Add(productSellingReportViewModel.ProductSellingReport);
                _db.SaveChanges();

                return RedirectToAction("Index");
            
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
