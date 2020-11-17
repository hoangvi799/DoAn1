using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn1.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/dashboard")]
    public class DashBoardController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public DashBoardController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.countInvoices = db.Invoices.Count(i => i.Status == 1);
            ViewBag.countProducts = db.Products.Count();
            ViewBag.countCustomer = db.RoleAccounts.Count(ro => ro.RoleId == 2);
            ViewBag.countCategories = db.Categories.Count(ro => ro.ParentId == null);
            return View();
        }
    }
}