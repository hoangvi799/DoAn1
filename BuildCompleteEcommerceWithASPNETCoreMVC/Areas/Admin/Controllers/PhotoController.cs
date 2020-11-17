using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/photo")]
    public class PhotoController : Controller
    {
        public DatabaseContext db = new DatabaseContext();

        private IHostingEnvironment ihostingEnvironment;

        public PhotoController(DatabaseContext _db, IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }

        [Route("index/{id}")]
        public IActionResult Index(int id)
        {
            ViewBag.Product = db.Products.Find(id);
            ViewBag.Photos = db.Photos.Where(p => p.ProductId == id).ToList();
            return View();
        }

        [HttpGet]
        [Route("add/{id}")]
        public IActionResult Add(int id)
        {
            ViewBag.Product = db.Products.Find(id);
            var photo = new Photo
            {
                ProductId = id
            };
            return View("Add", photo);
        }

        [HttpPost]
        [Route("add/{productId}")]
        public IActionResult Add(int productId, Photo photo, IFormFile fileUpload)
        {
            var fileName = DateTime.Now.ToString("MMddyyyymmss") + fileUpload.FileName;
            var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "products", fileName);
            var stream = new FileStream(path, FileMode.Create);
            fileUpload.CopyToAsync(stream);
            photo.Name = fileName;
            db.Photos.Add(photo);
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }

        [HttpGet]
        [Route("delete/{id}/productId")]
        public IActionResult Delete(int id, int productId)
        {
            var photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }

        [HttpGet]
        [Route("edit/{id}/{productId}")]
        public IActionResult Edit(int id, int productId)
        {
            ViewBag.Product = db.Products.Find(productId);
            var photo = db.Photos.Find(id);
            return View("Edit", photo);
        }

        [HttpPost]
        [Route("edit/{photoId}/{productId}")]
        public IActionResult Edit(int photoId, int productId, Photo photo, IFormFile fileUpload)
        {
            var currentPhoto = db.Photos.Find(photo.Id);
            if (fileUpload != null && !string.IsNullOrEmpty(fileUpload.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyymmss") + fileUpload.FileName;
                var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "products", fileName);
                var stream = new FileStream(path, FileMode.Create);
                fileUpload.CopyToAsync(stream);
                currentPhoto.Name = fileName;
            }
            currentPhoto.Status = photo.Status;
            currentPhoto.Featured = photo.Featured;
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }

        [HttpGet]
        [Route("SetFeatured/{id}/{productId}")]
        public IActionResult SetFeatured(int id, int productId)
        {
            var product = db.Products.Find(productId);
            product.Photos.ToList().ForEach(p =>
            {
                p.Featured = false;
                db.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            });
            var photo = db.Photos.Find(id);
            photo.Featured = true;
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }
    }
}