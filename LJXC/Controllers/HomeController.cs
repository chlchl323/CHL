using LJXC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJXC.Controllers
{
    public class HomeController : Controller
    {
        OrderingContext orderingContext;
        public HomeController(OrderingContext context)
        {
            orderingContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string userName,string password)
        {
            User user = orderingContext.Users.FirstOrDefault(p => p.UserName == userName && p.Password == password);
            if (user!=null)
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                string sex;
                if (user.Sex == "男")
                {
                    sex = "先生";
                }
                else
                {
                    sex = "女士";
                }
                HttpContext.Session.SetString("Sex", sex);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "用户名账号或密码错误！");
            return View("Index");
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index");

        }
        public IActionResult Reg()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Reg(User user)
        {           
            orderingContext.Users.Add(user);
            int count = orderingContext.SaveChanges();
            if (count > 0)
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                string sex;
                if (user.Sex == "男")
                {
                    sex = "先生";
                }
                else
                {
                    sex = "女士";
                }
                HttpContext.Session.SetString("Sex", sex);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "注册失败！");
            return View();
        }
        public IActionResult Detail()
        {
            return View(orderingContext.Users.SingleOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))));
        }
        [HttpPost]
        public IActionResult Detail(User user)
        {
            User users = orderingContext.Users.SingleOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId")));
            users.RealName = user.RealName;
            users.Address = user.Address;
            users.Sex = user.Sex;
            users.Phone = user.Phone;
            orderingContext.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        public IActionResult GetUser()
        {
            ICollection<User> user = orderingContext.Users.Where(p=>p.UserId==int.Parse(HttpContext.Session.GetString("UserId"))).ToList();
            return Json(user);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public IActionResult SelPassword(string oldPwd, string newPwd)
        {
            User users = orderingContext.Users.SingleOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId")));
            if (users.Password== oldPwd)
            {               
                users.Password = newPwd;
                orderingContext.Update(users);
                orderingContext.SaveChanges();
                return Content("yes");
            }
            else
            {
                return Content("no");
            }
        }
        public IActionResult PersonOrderList()
        {
            return View();
        }

        public IActionResult GetOrderList()
        {           
            ICollection<Order> orders= orderingContext.Orders.Where(p=>p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).ToList();
            return Json(orders);
        }

        public IActionResult SelOrderList()
        {
            ICollection<Order> orders = orderingContext.Orders.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).ToList();
            return Json(orders);
        }

        public IActionResult Select(string value)
        {
            ICollection<Order> orders = null;
            if (value == "0")
            {
                orders = orderingContext.Orders.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).ToList();
            }
            else
            {

                orders = orderingContext.Orders.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && p.OrderState == int.Parse(value)).ToList();
            }
            return Json(orders);
        }       
        public IActionResult FinOrder(int id)
        {
            var order = orderingContext.Orders.SingleOrDefault(p => p.OrderId == id);
            order.OrderState = 5;
            orderingContext.SaveChanges();
            return Content("ok");
        }


    }
}
