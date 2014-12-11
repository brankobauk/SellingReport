using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellingReport.Context;
using SellingReport.Helper;

namespace SellingReport.Controllers
{
    public class ProductSellingReportController : Controller
    {
        readonly SellingReportContext _db = new SellingReportContext();
        private readonly DropDownHelper _dropDownHelper = new DropDownHelper();
        //
        // GET: /ProductSellingReport/

        public ActionResult Index()
        {
            var productSellingReport = _db.ProductSellingReports.ToList();
            return View();
        }

        //
        // GET: /ProductSellingReport/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ProductSellingReport/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProductSellingReport/Create

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
        // GET: /ProductSellingReport/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ProductSellingReport/Edit/5

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
        // GET: /ProductSellingReport/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ProductSellingReport/Delete/5

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
