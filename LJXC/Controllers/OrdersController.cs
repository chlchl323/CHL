using LJXC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJXC.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("UserName")==null)
            {
                return RedirectToAction("Index","Home");
            }
            using (OrderingContext db = new OrderingContext())
            {
                if (db.CartRecords.FirstOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))) == null)
                {
                    return RedirectToAction("Food", "Foods");
                }
                var user = db.Users.FirstOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId")));
                DateTime dateTime = DateTime.Now;
                Order order = new Order();
                order.Address = user.Address;
                order.Phone = user.Phone;
                order.RealName = user.RealName;
                order.UserId = user.UserId;
                order.Totle = 0;
                order.OrderState = 1;
                order.DateCreate = dateTime;
                db.Orders.Add(order);
                db.SaveChanges();
                order = db.Orders.FirstOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && p.DateCreate == dateTime);
                ICollection<CartRecord> cartRecord = db.CartRecords.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).Include("Food").ToList();
                
                foreach (var item in cartRecord)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.FoodId = item.FoodId;
                    orderDetail.FoodName = item.Food.FoodName;
                    orderDetail.Quatity = item.Count;
                    orderDetail.PicUrl = item.Food.PicUrl;
                    orderDetail.UnitPrice = item.Food.Price;
                    orderDetail.OrderId = order.OrderId;
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();
                }
                
                HttpContext.Session.SetString("orderId", order.OrderId.ToString());
                return View();
            }                                             
        }
        [HttpPost]
        public IActionResult Order(Order order)
        {
            using (OrderingContext db = new OrderingContext())
            {
                db.Orders.Update(order);
                db.SaveChanges();
                return RedirectToAction("Pay","Pay");
            }
            
        }
        public IActionResult GetOrderDetail()
        {
            using (OrderingContext db = new OrderingContext())
            {
                
                ICollection<OrderDetail> orderDetails = db.OrderDetails.Where(p => p.OrderId== int.Parse(HttpContext.Session.GetString("orderId"))).ToList();
                return Json(orderDetails);
            }           
        }

        public IActionResult GetOrder()
        {
            using (OrderingContext db = new OrderingContext())
            {
                ICollection<Order> orders = db.Orders.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).ToList();
                return Json(orders);
            }
        }
        public IActionResult GetSum()
        {
            using (OrderingContext db = new OrderingContext())
            {
                ICollection<OrderDetail> orderDetails = db.OrderDetails.Where(p => p.OrderId == int.Parse(HttpContext.Session.GetString("orderId"))).ToList();
                decimal? sum = 0;
                foreach (var item in orderDetails)
                {
                    sum += item.UnitPrice * item.Quatity;
                }
                db.Orders.SingleOrDefault(p => p.OrderId == int.Parse(HttpContext.Session.GetString("orderId"))).Totle = sum;
                db.SaveChanges();
                return Json(sum);
            }
        }
        public IActionResult Add(int foodid)
        {
            using (OrderingContext db = new OrderingContext())
            {
                var orderDetails = db.OrderDetails.SingleOrDefault(p => p.OrderId == int.Parse(HttpContext.Session.GetString("orderId"))&& p.FoodId == foodid);
                orderDetails.Quatity++;
                db.SaveChanges();
                return Content("OK");
            }
        }
        public IActionResult Sub(int foodid)
        {
            using (OrderingContext db = new OrderingContext())
            {
                var orderDetails = db.OrderDetails.SingleOrDefault(p => p.OrderId == int.Parse(HttpContext.Session.GetString("orderId")) && p.FoodId == foodid);
                orderDetails.Quatity--;
                if (orderDetails.Quatity == 0)
                {
                    db.Remove(orderDetails);
                }
                db.SaveChanges();
                return Content("OK");
            }
        }

        public IActionResult OrderListDetail(int id)
        {
            HttpContext.Session.SetString("OrderListId", id.ToString());
            return View();
        }
        public IActionResult GetOrderList()
        {
            using (OrderingContext db = new OrderingContext())
            {

                ICollection<Order> orders = db.Orders.Where(p => p.OrderId == int.Parse(HttpContext.Session.GetString("OrderListId"))).ToList();
                return Json(orders);
            }
        }
        public IActionResult GetListSum()
        {
            using (OrderingContext db = new OrderingContext())
            {
                ICollection<OrderDetail> orderDetails = db.OrderDetails.Where(p => p.OrderId == int.Parse(HttpContext.Session.GetString("OrderListId"))).ToList();
                decimal? sum = 0;
                foreach (var item in orderDetails)
                {
                    sum += item.UnitPrice * item.Quatity;
                }
                return Json(sum);
            }
        }
        public IActionResult GetPaySum()
        {
            using (OrderingContext db = new OrderingContext())
            {
                var order = db.Orders.SingleOrDefault(p => p.OrderId == int.Parse(HttpContext.Session.GetString("orderId")));
                decimal? sum = order.Totle;
                return Json(sum);
            }
        }
        public IActionResult GetOrderListDetail()
        {
            using (OrderingContext db = new OrderingContext())
            {

                ICollection<OrderDetail> orderDetails = db.OrderDetails.Where(p => p.OrderId == int.Parse(HttpContext.Session.GetString("OrderListId"))).ToList();
                return Json(orderDetails);
            }
        }
        public IActionResult Back()
        {
            return RedirectToAction("PersonOrderList", "Home");
        }

        public IActionResult DelOrder(int id)
        {
            using (OrderingContext db = new OrderingContext())
            {
                var orders = db.Orders.SingleOrDefault(p => p.OrderId == id);
                orders.OrderState = 6;
                db.SaveChanges();
                return Content("ok");
            }
        }
    }
}
