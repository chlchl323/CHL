using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderSystemManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderSystemManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User use,string CheckCode)
        {
            if (CheckCode.ToLower() != HttpContext.Session.GetString("chkCode"))
            {
                ViewBag.ChkCodeMsg = "验证码输入有误！";
                return View();
            }
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                User user = db.Users.SingleOrDefault(u => u.UserName == use.UserName && u.Password == use.Password && u.UserTypeId == 1);
                if (user == null)
                {
                    ViewBag.LoginMsg = "用户名或密码错误！";
                    return View();
                }
                HttpContext.Session.SetInt32("userId", user.UserId);
                HttpContext.Session.SetString("userName", user.UserName);
                return RedirectToAction("FoodList", "Foods");
            }
        }

        public IActionResult Exit()
        {
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("userName");
            return RedirectToAction("Login");
        }

        //生成验证码
        public IActionResult VerifyCode(int id)
        {
            string chkCode = string.Empty;
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            //将验证码存入Session
            this.HttpContext.Session.SetString("chkCode", chkCode.ToLower());
            return File(new VerifyCode().GetVerifyCode(chkCode), @"image/Gif");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
