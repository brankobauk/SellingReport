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
    public class ProductSellingPlanController : Controller
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
            var productSellingPlan = _db.ProductSellingPlans.Where(p => p.CountryId == p.Country.CountryId && p.ProductId == p.Product.ProductId).Include(p => p.Product).Include(p => p.Country).OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ThenBy(p=>p.CountryId).ThenBy(p=>p.ProductId);
            var pagedProductSellingPlan = new PagedList<ProductSellingPlan>(productSellingPlan, page, 20);
            return View(pagedProductSellingPlan);
        }

        

        //
        // GET: /ProductSellingPlan/Create

        public ActionResult Create()
        {
            var products = _dropDownHelper.GetProductsListForDropDown(_db.Products);
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var months = _dropDownHelper.GetMonthsListForDropDown();
            var years = _dropDownHelper.GetYearsListForDropDown();
            var productSellingPlan = new ProductSellingPlanViewModel
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
                var productSellingPlan = new ProductSellingPlanViewModel
                {
                    ProductSellingPlan = productSellingReportPlanToEdit,
                    Month = months,
                    Year = years,
                    Countries = countries,
                    Products = products
                };
                return View(productSellingPlan);
            }
            return RedirectToAction("Index");
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
                    productSellingReportPlanToEdit.PlannedValue = productSellingReportPlanViewModel.ProductSellingPlan.PlannedValue;
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
            var productSellingReportToDelete = _db.ProductSellingPlans.FirstOrDefault(p => p.ProductSellingPlanId == id);
            _db.ProductSellingPlans.Remove(productSellingReportToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}
