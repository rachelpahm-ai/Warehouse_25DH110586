using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warehouse_25DH110586.Models;

namespace Warehouse_25DH110586.Controllers
{
    public class ProductsController : Controller
    {
        private Warehouse_25DH110586Entities db = new Warehouse_25DH110586Entities();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Category,Price,Quantity")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string ProductName,
            string Category, decimal? Price)
        {
            var product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            if (string.IsNullOrWhiteSpace(ProductName))
                ModelState.AddModelError(
                    "ProductName", "Tên sản phẩm không được để trống.");

            if (string.IsNullOrWhiteSpace(Category))
                ModelState.AddModelError(
                    "Category", "Danh mục không được để trống.");

            if (!Price.HasValue || Price.Value <= 0)
                ModelState.AddModelError(
                    "Price", "Đơn giá phải lớn hơn 0.");

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.ProductName = ProductName.Trim();
            product.Category = Category.Trim();
            product.Price = Price.Value;

            db.SaveChanges();

            TempData["Success"] = "Cập nhật sản phẩm thành công!";

            return RedirectToAction("Index");
        }


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Filter(
            decimal? minPrice,
            decimal? maxPrice,
            string category,
            string sortOrder)
        {
            var products = db.Products.AsQueryable();

            // Lấy danh mục từ database
            ViewBag.Categories = db.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Giữ lại giá trị lọc
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Category = category;
            ViewBag.SortOrder = sortOrder;

            // Kiểm tra khoảng giá
            if (minPrice.HasValue && maxPrice.HasValue
                && minPrice.Value > maxPrice.Value)
            {
                ViewBag.Error = "Từ giá không được lớn hơn Đến giá!";
                return View(db.Products.Where(p => false).ToList());
            }

            // Lọc từ giá (bao gồm biên)
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            // Lọc đến giá (bao gồm biên)
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            // Lọc danh mục
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            // Sắp xếp giá
            if (sortOrder == "desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else
            {
                products = products.OrderBy(p => p.Price);
            }

            return View(products.ToList());
        }

    }
}
