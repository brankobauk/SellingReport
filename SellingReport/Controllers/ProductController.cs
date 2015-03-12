using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellingReport.Context;
using SellingReport.Models.Models;

namespace SellingReport.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        SellingReportContext _db = new SellingReportContext();

        public ActionResult Index()
        {
            var product = _db.Products.ToList();
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
                _db.Products.Add(product);
                _db.SaveChanges();

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
            var productToEdit = _db.Products.FirstOrDefault(p => p.ProductId == id);
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
                var productToEdit = _db.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (productToEdit != null)
                {
                    productToEdit.Name = product.Name;
                    productToEdit.BottleHeight = product.BottleHeight;
                    if (image != null)
                    {
                        productToEdit.Image = image;
                    }
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
        // GET: /Product/Delete/5

        public ActionResult Delete(int id)
        {
            var productToDelete = _db.Products.FirstOrDefault(p => p.ProductId == id);
            _db.Products.Remove(productToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
