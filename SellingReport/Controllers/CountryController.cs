using System.Linq;
using System.Web.Mvc;
using SellingReport.Context;
using SellingReport.Models.Models;

namespace SellingReport.Controllers
{
    [Authorize]
    public class CountryController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        public ActionResult Index()
        {
            var countries = _db.Countries.ToList();
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
                _db.Countries.Add(country);
                _db.SaveChanges();

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
            var countryToEdit = _db.Countries.FirstOrDefault(p => p.CountryId == id);
            return View(countryToEdit);
        }

        //
        // POST: /Country/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Country country)
        {
            try
            {

                var countryToEdit = _db.Countries.FirstOrDefault(p => p.CountryId == id);
                if (countryToEdit != null)
                {
                    countryToEdit.Name = country.Name;
                    countryToEdit.Code = country.Code;
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Country/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                var countryToDelete = _db.Countries.FirstOrDefault(p => p.CountryId == id);
                _db.Countries.Remove(countryToDelete);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

       
    }
}
