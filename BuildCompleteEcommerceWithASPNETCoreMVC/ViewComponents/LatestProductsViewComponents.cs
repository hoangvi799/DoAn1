using DoAn1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.ViewComponents
{
    [ViewComponent(Name = "LatestProducts")]
    public class LatestProductsViewComponents : ViewComponent
    {
        private DatabaseContext db;

        public LatestProductsViewComponents(DatabaseContext _db)
        {
            this.db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = db.Products.OrderByDescending(p => p.Id).Where(p => p.Status).Take(2).ToList();
            return View("Index", products);
        }
    }
}
