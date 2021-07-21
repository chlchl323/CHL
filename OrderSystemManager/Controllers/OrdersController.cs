using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSystemManager.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderSystemManager.Controllers
{
    public class OrdersController : Controller
    {
        OrderManagerDBContext db = new OrderManagerDBContext();
        static int orderState;

        public IActionResult OrderList(int id)
        {
            orderState = id;
            return View();
        }

        public IActionResult OrderDetail(int id)
        {
            return View(db.Orders.Include("OrderDetails").SingleOrDefault(o => o.OrderId == id));
        }

        public IActionResult GetOrderList()
        {
            return Json(db.Orders.Where(o => o.OrderState == orderState).ToList());
        }

        //更改订单状态
        public IActionResult upState(int id,int stateNum)
        {
            Order order = db.Orders.Find(id);
            order.OrderState = stateNum;
            db.SaveChanges();
            return Content("ok");
        }
    }
}
