using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DoAn1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAn1.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/slideshow")]
    public class SlideShowController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        private IHostingEnvironment ihostingEnvironment;

        public SlideShowController(DatabaseContext _db, IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.SlideShows = db.SlideShows.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var slideShow = new SlideShow();
            return View("Add", slideShow);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(SlideShow slideShow, IFormFile photo)
        {
            var fileName = DateTime.Now.ToString("MMddyyyymmss") + photo.FileName;
            var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "slideshows",  fileName);
            var stream = new FileStream(path, FileMode.Create);
            photo.CopyToAsync(stream);
            slideShow.Name = fileName;
            db.SlideShows.Add(slideShow);
            db.SaveChanges();
            return RedirectToAction("index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var slideShow = db.SlideShows.Find(id);
            db.SlideShows.Remove(slideShow);
            db.SaveChanges();
            return RedirectToAction("index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var slideShow = db.SlideShows.Find(id);
            return View("Edit", slideShow);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, SlideShow slideShow, IFormFile photo)
        {
            var currentSlideShow = db.SlideShows.Find(slideShow.Id);
            if(photo != null && !string.IsNullOrEmpty(photo.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyymmss") + photo.FileName;
                var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "slideshows", fileName);
                var stream = new FileStream(path, FileMode.Create);
                photo.CopyToAsync(stream);
                currentSlideShow.Name = fileName;
            }
            currentSlideShow.Status = slideShow.Status;
            currentSlideShow.Title = slideShow.Title;
            currentSlideShow.Description = slideShow.Description;
            db.SaveChanges();
            return RedirectToAction("index", "slideshow", new { area = "admin" });
        }
    }
}