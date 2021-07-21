using LJXC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJXC.Controllers
{
    public class PayController : Controller
    {
        public IActionResult Pay()
        {            
            return View();
        }
        public IActionResult Pays(int id)
        {
            HttpContext.Session.SetString("OrderListId", id.ToString());
            return RedirectToAction("Pay");
        }
        public IActionResult ChangeOrderState()
        {
            
            using (OrderingContext db = new OrderingContext())
            {
                if (HttpContext.Session.GetString("OrderListId") !=null)
                {
                    var orders = db.Orders.SingleOrDefault(p => p.OrderId == int.Parse(HttpContext.Session.GetString("OrderListId")));
                    orders.OrderState = 2;
                    db.Orders.Update(orders);
                    ICollection<CartRecord> cartRecords = db.CartRecords.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).ToList();
                    foreach (var item in cartRecords)
                    {
                        db.CartRecords.Remove(item); ;
                    }
                    db.SaveChanges();

                    return Content("ok");
                }
                else
                {
                    var orders = db.Orders.SingleOrDefault(p => p.OrderId == int.Parse(HttpContext.Session.GetString("orderId")));
                    orders.OrderState = 2;
                    db.Orders.Update(orders);
                    ICollection<CartRecord> cartRecords = db.CartRecords.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).ToList();
                    foreach (var item in cartRecords)
                    {
                        db.CartRecords.Remove(item); ;
                    }
                    db.SaveChanges();

                    return Content("ok");
                }               
            }
           
        }
    }
}
