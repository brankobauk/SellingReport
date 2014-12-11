using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellingReport.Helper;
using SellingReport.Context;
using SellingReport.Models.Models;
using SellingReport.Models.ViewModels;

namespace SellingReport.Controllers
{
    public class ProductSellingPlanController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        //
        // GET: /ProductSellingPlan/

        public ActionResult Index()
        {
            var productSellingPlan = _db.ProductSellingPlans.ToList();
            return View(productSellingPlan);
        }

        

        //
        // GET: /ProductSellingPlan/Create

        public ActionResult Create()
        {
            var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var months = _dropDownHelper.GetMonthsListForDropDown();
            var years = _dropDownHelper.GetYearsListForDropDown();
            var productSellingPlan = new ProductSellingPlanViewModel()
            {
                ProductSellingPlan = new ProductSellingPlan(),
                Month = months,
                Year = years,
                Countries = countries,
                Products = products
            };
            return View(productSellingPlan);
        }

        //
        // POST: /ProductSellingPlan/Create

        [HttpPost]
        public ActionResult Create(ProductSellingPlanViewModel productSellingReportPlanViewModel)
        {
            try
            {
                _db.ProductSellingPlans.Add(productSellingReportPlanViewModel.ProductSellingPlan);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductSellingPlan/Edit/5

        public ActionResult Edit(int id)
        {
            var productSellingReportPlanToEdit = _db.ProductSellingPlans.FirstOrDefault(p => p.ProductSellingPlanId == id);
            if (productSellingReportPlanToEdit != null) { 
                var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
                var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
                var months = _dropDownHelper.GetMonthsListForDropDown();
                var years = _dropDownHelper.GetYearsListForDropDown();
                var productSellingPlan = new ProductSellingPlanViewModel()
                {
                    ProductSellingPlan = productSellingReportPlanToEdit,
                    Month = months,
                    Year = years,
                    Countries = countries,
                    Products = products
                };
                return View(productSellingPlan);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        //
        // POST: /ProductSellingPlan/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ProductSellingPlanViewModel productSellingReportPlanViewModel)
        {
            try
            {
                var productSellingReportPlanToEdit = _db.ProductSellingPlans.FirstOrDefault(p => p.ProductSellingPlanId == id);
                if (productSellingReportPlanToEdit != null)
                {
                    productSellingReportPlanToEdit.ProductId = productSellingReportPlanViewModel.ProductSellingPlan.ProductId;
                    productSellingReportPlanToEdit.CountryId = productSellingReportPlanViewModel.ProductSellingPlan.CountryId;
                    productSellingReportPlanToEdit.Pieces = productSellingReportPlanViewModel.ProductSellingPlan.Pieces;
                    productSellingReportPlanToEdit.Month = productSellingReportPlanViewModel.ProductSellingPlan.Month;
                    productSellingReportPlanToEdit.Year = productSellingReportPlanViewModel.ProductSellingPlan.Year;
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
        // GET: /ProductSellingPlan/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ProductSellingPlan/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
