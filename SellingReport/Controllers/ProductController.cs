using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellingReport.Context;
using SellingReport.Models;

namespace SellingReport.Controllers
{
    public class ProductController : Controller
    {
        SellingReportContext db = new SellingReportContext();

        public ActionResult Index()
        {
            var product = db.Products.ToList();
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id)
        {
            var productToEdit = db.Products.FirstOrDefault(p => p.ProductId == id);
            return View(productToEdit);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase file)
        {
            byte[] image = null;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    string path;
                    if (fileName != null) 
                    {
                        path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                        file.SaveAs(path);
                        image = System.IO.File.ReadAllBytes(path);
                        System.IO.File.Delete(path);
                    }
                    
                }
                var productToEdit = db.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (productToEdit != null)
                {
                    productToEdit.Name = product.Name;
                    if (image != null)
                    {
                        productToEdit.Image = image;
                    }
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
        // GET: /Product/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Product/Delete/5

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
