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
    public class ProductSellingMonthlyPlanController : Controller
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
            var productSellingMonthlyPlan = _db.ProductSellingMonthlyPlans.Where(p => p.CountryId == p.Country.CountryId).Include(p => p.Country).OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ThenBy(p => p.CountryId);
            var pagedProductSellingMonthlyPlan = new PagedList<ProductSellingMonthlyPlan>(productSellingMonthlyPlan, page, 20);
            return View(pagedProductSellingMonthlyPlan);
        }

        public ActionResult Create()
        {
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var months = _dropDownHelper.GetMonthsListForDropDown();
            var years = _dropDownHelper.GetYearsListForDropDown();
            var productSellingMonthlyPlan = new ProductSellingMonthlyPlanViewModel
            {
                ProductSellingMonthlyPlan = new ProductSellingMonthlyPlan
                {
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month
                },
                Month = months,
                Year = years,
                Countries = countries
            };
            return View(productSellingMonthlyPlan);
        }

        //
        // POST: /ProductSellingPlan/Create

        [HttpPost]
        public ActionResult Create(ProductSellingMonthlyPlanViewModel productSellingReportMonthlyPlanViewModel)
        {
            try
            {
                _db.ProductSellingMonthlyPlans.Add(productSellingReportMonthlyPlanViewModel.ProductSellingMonthlyPlan);
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
            var productSellingMonthlyPlanToEdit = _db.ProductSellingMonthlyPlans.FirstOrDefault(p => p.ProductSellingMonthlyPlanId == id);
            if (productSellingMonthlyPlanToEdit != null)
            {
                var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
                var months = _dropDownHelper.GetMonthsListForDropDown();
                var years = _dropDownHelper.GetYearsListForDropDown();
                var productSellingMonthlyPlan = new ProductSellingMonthlyPlanViewModel
                {
                    ProductSellingMonthlyPlan = productSellingMonthlyPlanToEdit,
                    Month = months,
                    Year = years,
                    Countries = countries
                };
                return View(productSellingMonthlyPlan);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /ProductSellingPlan/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ProductSellingMonthlyPlanViewModel productSellingMonthlyPlanViewModel)
        {
            try
            {
                var productSellingMonthlyPlanToEdit = _db.ProductSellingMonthlyPlans.FirstOrDefault(p => p.ProductSellingMonthlyPlanId == id);
                if (productSellingMonthlyPlanToEdit != null)
                {
                    productSellingMonthlyPlanToEdit.CountryId = productSellingMonthlyPlanViewModel.ProductSellingMonthlyPlan.CountryId;
                    productSellingMonthlyPlanToEdit.Month = productSellingMonthlyPlanViewModel.ProductSellingMonthlyPlan.Month;
                    productSellingMonthlyPlanToEdit.Year = productSellingMonthlyPlanViewModel.ProductSellingMonthlyPlan.Year;
                    productSellingMonthlyPlanToEdit.PlannedValue = productSellingMonthlyPlanViewModel.ProductSellingMonthlyPlan.PlannedValue;
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
            var productSellingMonthlyPlanToDelete = _db.ProductSellingMonthlyPlans.FirstOrDefault(p => p.ProductSellingMonthlyPlanId == id);
            _db.ProductSellingMonthlyPlans.Remove(productSellingMonthlyPlanToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}