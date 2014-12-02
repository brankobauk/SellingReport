using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellingReport.Context;
using SellingReport.Models;

namespace SellingReport.Controllers
{
    public class CountryController : Controller
    {
        SellingReportContext db = new SellingReportContext();
        public ActionResult Index()
        {
            var countries = db.Countries.ToList();
            return View(countries);
        }

        //
        // GET: /Country/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Country/Create

        [HttpPost]
        public ActionResult Create(Country country)
        {
            try
            {
                db.Countries.Add(country);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(country);
            }
        }

        //
        // GET: /Country/Edit/5

        public ActionResult Edit(int id)
        {
            var countryToEdit = db.Countries.FirstOrDefault(p => p.CountryId == id);
            return View(countryToEdit);
        }

        //
        // POST: /Country/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Country country)
        {
            try
            {

                var countryToEdit = db.Countries.FirstOrDefault(p => p.CountryId == id);
                if (countryToEdit != null)
                {
                    countryToEdit.Name = country.Name;
                    countryToEdit.Code = country.Code;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Country/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                var countryToDelete = db.Countries.FirstOrDefault(p => p.CountryId == id);
                db.Countries.Remove(countryToDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

       
    }
}
