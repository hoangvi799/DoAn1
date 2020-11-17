using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DoAn1.Models;
using BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn1.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ProductController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.Products = db.Products.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.Product = new Product();
            productViewModel.Categorises = new List<SelectListItem>();
            var categories = db.Categories.ToList();
            foreach (var category in categories)
            {
                var group = new SelectListGroup { Name = category.Name };
                if (category.InverseParents != null && category.InverseParents.Count > 0)
                {
                    foreach (var subCategory in category.InverseParents)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Text = subCategory.Name,
                            Value = subCategory.Id.ToString(),
                            Group = group
                        };
                        productViewModel.Categorises.Add(selectListItem);
                    }
                }
            }
            return View("Add", productViewModel);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(ProductViewModel productViewModel)
        {
            db.Products.Add(productViewModel.Product);
            db.SaveChanges();

            // Create default photo for new product
            var defaultPhoto = new Photo
            {
                Name = "add.jpg",
                Status = true,
                ProductId = productViewModel.Product.Id,
                Featured = true
            };
            db.Photos.Add(defaultPhoto);
            db.SaveChanges();

            return RedirectToAction("index", "product", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction("index", "product", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var productViewModel = new ProductViewModel();
            productViewModel.Product = db.Products.Find(id);
            productViewModel.Categorises = new List<SelectListItem>();
            var categories = db.Categories.ToList();
            foreach (var category in categories)
            {
                var group = new SelectListGroup { Name = category.Name };
                if (category.InverseParents != null && category.InverseParents.Count > 0)
                {
                    foreach (var subCategory in category.InverseParents)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Text = subCategory.Name,
                            Value = subCategory.Id.ToString(),
                            Group = group
                        };
                        productViewModel.Categorises.Add(selectListItem);
                    }
                }
            }
            return View("Edit", productViewModel);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, ProductViewModel productViewModel)
        {
            db.Entry(productViewModel.Product).State =
                Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index", "product", new { area = "admin" });
        }
    }
}