using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSystemManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace OrderSystemManager.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult UserList()
        {
            return View();
        }

        //获取用户列表
        public IActionResult GetUserList()
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                return Json(db.Users.Include("UserType").Where(u=>u.UserTypeId != 1).ToList());
            }
        }

        //删除用户
        public IActionResult Delete(int id)
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                User user = db.Users.Find(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return Content("ok");
                }
                return Content("erro");
            }
        }

        //用户详情
        public IActionResult UserDetail(int id)
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                User user = db.Users.Include("UserType").SingleOrDefault(u => u.UserId == id);
                if (user != null)
                {
                    return View(user);
                }
                return RedirectToAction("UserList");
            }
        }

        //修改管理员密码
        public IActionResult ChangePwd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdatePwd()
        {
            string oldPwd = Request.Form["txtOldPwd"];
            string newPwd = Request.Form["txtNewPwd"];
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                User user = db.Users.Find(HttpContext.Session.GetInt32("userId"));
                if (user.Password != oldPwd)
                {
                    TempData["PwdMsg"] = "旧密码输入有误！";
                    return RedirectToAction("ChangePwd");
                }
                user.Password = newPwd;
                db.SaveChanges();
            }
            return View();
        }
    }
}
