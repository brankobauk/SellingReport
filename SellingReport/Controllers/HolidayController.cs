using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellingReport.BusinessLogic.Handler;

namespace SellingReport.Controllers
{
    public class HolidayController : Controller
    {
        private readonly HolidayHandler _holidayHandler = new HolidayHandler();

        public ActionResult Index()
        {
            var easterDate = _holidayHandler.EasterSundayOf(DateTime.Now.Year);
            var holidays = _holidayHandler.GetAllHolidays();
            return View();
        }

        //
        // GET: /Holiday/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Holiday/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Holiday/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Holiday/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Holiday/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Holiday/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Holiday/Delete/5

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
