using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SellingReport.Context;
using SellingReport.Helper;
using SellingReport.Models.ViewModels;

namespace SellingReport.Controllers
{
    [Authorize]
    public class VariableHolidayController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        //
        // GET: /VariableHoliday/

        public ActionResult Index()
        {
            var variableHoliday =
                _db.VariableHolidays.Where(p => p.CountryId == p.Country.CountryId)
                    .Include(p => p.Country)
                    .OrderByDescending(p => p.Date).ThenBy(p=>p.CountryId);
            return View(variableHoliday);
        }

        //
        // GET: /VariableHoliday/Create

        public ActionResult Create()
        {
            var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
            var variableHolidaysToAdd = new VariableHolidayViewModel
            {
                Countries = countries
            };
            return View(variableHolidaysToAdd);
        }

        //
        // POST: /VariableHoliday/Create

        [HttpPost]
        public ActionResult Create(VariableHolidayViewModel variableHolidayViewModel)
        {
            try
            {
                _db.VariableHolidays.Add(variableHolidayViewModel.VariableHoliday);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /VariableHoliday/Edit/5

        public ActionResult Edit(int id)
        {
            var variableHolidayToEdit = _db.VariableHolidays.FirstOrDefault(p => p.VariableHolidayId == id);
            if (variableHolidayToEdit != null)
            {
                var countries = _dropDownHelper.GetCountryListForDropDown(_db.Countries);
                var variableHolidayViewModelToEdit = new VariableHolidayViewModel
                {
                    Countries = countries,
                    VariableHoliday = variableHolidayToEdit
                };
                return View(variableHolidayViewModelToEdit);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /VariableHoliday/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, VariableHolidayViewModel variableHolidayViewModel)
        {
            try
            {
                var variableHolidayToEdit = _db.VariableHolidays.FirstOrDefault(p => p.VariableHolidayId == id);
                if (variableHolidayToEdit != null)
                {
                    variableHolidayToEdit.Date = variableHolidayViewModel.VariableHoliday.Date;
                    variableHolidayToEdit.Name = variableHolidayViewModel.VariableHoliday.Name;
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
        // GET: /VariableHoliday/Delete/5

        public ActionResult Delete(int id)
        {
            var variableHolidayToDelete = _db.VariableHolidays.FirstOrDefault(p => p.VariableHolidayId == id);
            _db.VariableHolidays.Remove(variableHolidayToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
