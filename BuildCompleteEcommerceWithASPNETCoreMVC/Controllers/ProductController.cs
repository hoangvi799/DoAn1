using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn1.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ProductController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("details/{id}")]
        public IActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            var featuredPhoto = product.Photos.SingleOrDefault(p => p.Status && p.Featured);
            ViewBag.Product = product;
            ViewBag.FeaturedPhoto = featuredPhoto == null ? "add.jpg" : featuredPhoto.Name;
            ViewBag.ProductImages = product.Photos.Where(p => p.Status).ToList();
            ViewBag.RelatedProducts = db.Products.Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.Status).ToList();
            return View("Details");
        }

        [Route("category/{id}")]
        public IActionResult Category(int id, int? page)
        {
            var pageNumber = page ?? 1;
            var category = db.Categories.Find(id);
            ViewBag.Category = category;
            ViewBag.CountProducts = category.Products.Count(p => p.Status);
            ViewBag.Products = category.Products.Where(p => p.Status).ToList().ToPagedList(pageNumber, 3);
            return View("Category");
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string keyword, int categoryId, int? page)
        {
            var pageNumber = page ?? 1;
            var products = db.Products.Where(p => p.Name.Contains(keyword) && p.CategoryId == categoryId && p.Status).ToList();
            ViewBag.Keyword = keyword;
            ViewBag.categoryId = categoryId;
            ViewBag.CountProducts = products.Count;
            ViewBag.Products = products.ToPagedList(pageNumber, 3);
            return View("Search");
        }
    }
}