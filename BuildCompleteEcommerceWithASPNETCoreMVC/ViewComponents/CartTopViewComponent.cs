using BuildCompleteEcommerceWithASPNETCoreMVC.Helpers;
using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using DoAn1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.ViewComponents
{
    [ViewComponent(Name = "CartTop")]
    public class CartTopViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                ViewBag.countItems = 0;
                ViewBag.Total = 0;
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.Total = cart.Sum(it => it.Price * it.Quantity);
            }              
            return View("Index");
        }
    }
}
