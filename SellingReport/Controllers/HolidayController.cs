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
    public class HolidayController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();

        public ActionResult Index()
        {
            var holidays = _db.Holidays.Where(p => p.CountryId == p.Country.CountryId).Include(p => p.Country).OrderBy(p=>p.CountryId);
            return View(holidays);
        }

        //
        // GET: /Holiday/Create

        public ActionResult Create()
        {
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var holidaysToAdd = new HolidayViewModel
            {
                Countries = countries
            };
            return View(holidaysToAdd);
        }

        //
        // POST: /Holiday/Create

        [HttpPost]
        public ActionResult Create(Holiday holiday, DateTime holidayDate)
        {
            try
            {
                var holidayToAdd = new Holiday
                {
                    CountryId = holiday.CountryId,
                    Day = holidayDate.Day,
                    Month = holidayDate.Month
                };
                _db.Holidays.Add(holidayToAdd);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Holiday/Edit/5

        public ActionResult Edit(int id)
        {
            var holidayToEdit = _db.Holidays.FirstOrDefault(p => p.HolidayId == id);
            if (holidayToEdit != null) { 
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var holidayViewModelToEdit = new HolidayViewModel
            {
                Countries = countries,
                Holiday = holidayToEdit
            };
            return View(holidayViewModelToEdit);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Holiday/Edit/5

        [HttpPost]
        public ActionResult Edit(Holiday holiday, DateTime holidayDate)
        {
            try
            {
                var holidayToEdit = _db.Holidays.FirstOrDefault(p => p.HolidayId == holiday.HolidayId);
                if (holidayToEdit != null)
                {
                    holidayToEdit.CountryId = holiday.CountryId;
                    holidayToEdit.Day = holidayDate.Day;
                    holidayToEdit.Month = holidayDate.Month;
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
        // GET: /Holiday/Delete/5

        public ActionResult Delete(int id)
        {
            var holidayToDelete = _db.Holidays.FirstOrDefault(p => p.HolidayId == id);
            _db.Holidays.Remove(holidayToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}
