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
    public class ProductSellingYearlyPlanController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        //
        // GET: /ProductSellingPlan/

        public ActionResult Index()
        {
            var page = Convert.ToInt32(Request.QueryString["page"]);
            if (page == 0)
            {
                page = 1;
            }
            var productSellingYearlyPlan = _db.ProductSellingYearlyPlans.Where(p => p.CountryId == p.Country.CountryId && p.ProductId == p.Product.ProductId).Include(p => p.Product).Include(p => p.Country).OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ThenBy(p => p.CountryId).ThenBy(p => p.ProductId);
            var pagedProductSellingYearlyPlan = new PagedList<ProductSellingYearlyPlan>(productSellingYearlyPlan, page, 20);
            return View(pagedProductSellingYearlyPlan);
        }

        public ActionResult Create()
        {
            var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var months = _dropDownHelper.GetMonthsListForDropDown();
            var years = _dropDownHelper.GetYearsListForDropDown();
            var productSellingYearlyPlan = new ProductSellingYearlyPlanViewModel
            {
                ProductSellingYearlyPlan = new ProductSellingYearlyPlan()
                {
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month
                },
                Month = months,
                Year = years,
                Countries = countries,
                Products = products
            };
            return View(productSellingYearlyPlan);
        }

        //
        // POST: /ProductSellingPlan/Create

        [HttpPost]
        public ActionResult Create(ProductSellingYearlyPlanViewModel productSellingReportMonthlyPlanViewModel)
        {
            try
            {
                _db.ProductSellingYearlyPlans.Add(productSellingReportMonthlyPlanViewModel.ProductSellingYearlyPlan);
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
            var productSellingYearlyPlanToEdit = _db.ProductSellingYearlyPlans.FirstOrDefault(p => p.ProductSellingYearlyPlanId == id);
            if (productSellingYearlyPlanToEdit != null)
            {
                var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
                var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
                var months = _dropDownHelper.GetMonthsListForDropDown();
                var years = _dropDownHelper.GetYearsListForDropDown();
                var productSellingYearlyPlan = new ProductSellingYearlyPlanViewModel
                {
                    ProductSellingYearlyPlan = productSellingYearlyPlanToEdit,
                    Month = months,
                    Year = years,
                    Countries = countries,
                    Products = products
                };
                return View(productSellingYearlyPlan);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /ProductSellingPlan/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ProductSellingYearlyPlanViewModel productSellingYearlyPlanViewModel)
        {
            try
            {
                var productSellingYearlyPlanToEdit = _db.ProductSellingYearlyPlans.FirstOrDefault(p => p.ProductSellingYearlyPlanId == id);
                if (productSellingYearlyPlanToEdit != null)
                {
                    productSellingYearlyPlanToEdit.ProductId = productSellingYearlyPlanViewModel.ProductSellingYearlyPlan.ProductId;
                    productSellingYearlyPlanToEdit.CountryId = productSellingYearlyPlanViewModel.ProductSellingYearlyPlan.CountryId;
                    productSellingYearlyPlanToEdit.Month = productSellingYearlyPlanViewModel.ProductSellingYearlyPlan.Month;
                    productSellingYearlyPlanToEdit.Year = productSellingYearlyPlanViewModel.ProductSellingYearlyPlan.Year;
                    productSellingYearlyPlanToEdit.PlannedValue = productSellingYearlyPlanViewModel.ProductSellingYearlyPlan.PlannedValue;
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
            var productSellingYearlyPlanToDelete = _db.ProductSellingYearlyPlans.FirstOrDefault(p => p.ProductSellingYearlyPlanId == id);
            _db.ProductSellingYearlyPlans.Remove(productSellingYearlyPlanToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}