using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BuildCompleteEcommerceWithASPNETCoreMVC.Helpers;
using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using DoAn1.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAn1.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public CartController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            List<Item>cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                ViewBag.countItems = 0;
                ViewBag.Total = 0;
            }
            else
            {
                ViewBag.countItems = cart.Count;
                ViewBag.Total = cart.Sum(it => it.Price * it.Quantity);
            }   
            return View();
        }

        [HttpGet]
        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            var product = db.Products.Find(id);
            var photo = product.Photos.SingleOrDefault(ph => ph.Status && ph.Featured);
            var photoName = photo == null ? "add.jpg" : photo.Name;
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<Item>();
                cart.Add(new Item
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Photo = photoName,
                    Quantity = 1
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                int index = exists(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Photo = photoName,
                        Quantity = 1
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
                return RedirectToAction("index", "Cart");
        }

        [HttpPost]
        [Route("buy/{id}")]
        public IActionResult Buy(int id, int quantity)
        {
            var product = db.Products.Find(id);
            var photo = product.Photos.SingleOrDefault(ph => ph.Status && ph.Featured);
            var photoName = photo == null ? "add.jpg" : photo.Name;
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<Item>();
                cart.Add(new Item
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Photo = photoName,
                    Quantity = quantity
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                int index = exists(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Photo = photoName,
                        Quantity = quantity
                    });
                }
                else
                {
                    cart[index].Quantity += quantity;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("index", "Cart");
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(int[] quantity)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for(var i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = quantity[i];
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("index", "Cart");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = exists(id, cart);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("index", "Cart");
        }

        [Route("checkout")]
        public IActionResult Checkout()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            else
            {
                var customer = db.Accounts.SingleOrDefault(a => a.Username.Equals(user.Value));

                // Tạo mới đơn hàng
                var invoice = new Invoice() { 
                    Name = "InvoiceOnline",
                    Created = DateTime.Now,
                    Status = 1,
                    AccountId = customer.Id
                };
                db.Invoices.Add(invoice);
                db.SaveChanges();

                // Tạo xem đơn hàng
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session,
                    "cart");
                foreach(var item in cart)
                {
                    var invoiceDetails = new InvoiceDetails
                    { 
                        InvoiceId = invoice.Id,
                        ProductId = item.Id,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    db.InvoiceDetailses.Add(invoiceDetails);
                    db.SaveChanges();
                }

                // Remove items in cart
                HttpContext.Session.Remove("cart");

                return RedirectToAction("Thanks", "Cart");
            }
        }

        [Route("thanks")]
        public IActionResult Thanks()
        {
            return View("Thanks");
        }

        private int exists(int id, List<Item> cart)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}