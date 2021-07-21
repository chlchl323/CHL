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
    
    public class FoodsController : Controller
    {
        OrderingContext orderingContext;
        public FoodsController(OrderingContext context)
        {
            orderingContext = context;
        }
        
        public IActionResult Food()
        {           
            return View();          
        }
        public IActionResult GetFood(int? foodsid)
        {
            using (OrderingContext db = new OrderingContext())
            {
                ICollection<Food> foods = db.Foods.Where(p => p.FoodTypeId == foodsid).ToList();
                return Json(foods);
            }
        }
        public IActionResult FoodType()
        {
            using (OrderingContext db = new OrderingContext())
            {
                ICollection<FoodType> foodTypes = db.FoodTypes.ToList();
                return Json(foodTypes);
            }            
        }
        
        public IActionResult CartRecord()
        {
            ICollection<CartRecord> cartRecords = null;
            if (HttpContext.Session.GetString("UserName")!=null)
            {
                using (OrderingContext db = new OrderingContext())
                {
                    cartRecords = db.CartRecords.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).Include("Food").ToList();                   
                    return Json(cartRecords);
                }
            }
            return Json(cartRecords);
        }
        public IActionResult IsUser()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return Content("true");
            }
            else
            {
                return Content("false");
            }
            
        }
        public IActionResult AddCart(int foodsid)
        {
            using (OrderingContext db = new OrderingContext())
            {
                var cartRecords = db.CartRecords.SingleOrDefault(p => p.FoodId == foodsid&&p.UserId== int.Parse(HttpContext.Session.GetString("UserId")));
                if (cartRecords == null)
                {
                    CartRecord cartRecord = new CartRecord();
                    cartRecord.Count = 1;
                    cartRecord.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
                    cartRecord.FoodId = foodsid;
                    db.CartRecords.Add(cartRecord);
                }
                else
                {
                    cartRecords.Count++;
                }
                db.SaveChanges();
                return Content("OK");
            }
        }

        public IActionResult Add(int foodid)
        {
            using (OrderingContext db = new OrderingContext())
            {
                var cartRecords= db.CartRecords.SingleOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))&&p.FoodId==foodid);
                cartRecords.Count++;
                db.SaveChanges();
                return Content("OK");
            }
        }
        public IActionResult Sub(int foodid)
        {
            using (OrderingContext db = new OrderingContext())
            {
                var cartRecords = db.CartRecords.SingleOrDefault(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && p.FoodId == foodid);
                cartRecords.Count--;
                if (cartRecords.Count==0)
                {
                    db.Remove(cartRecords);
                }
                db.SaveChanges();
                return Content("OK");
            }
        }
        public IActionResult GetSum()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                using (OrderingContext db = new OrderingContext())
                {
                    ICollection<CartRecord> cartRecords = db.CartRecords.Where(p => p.UserId == int.Parse(HttpContext.Session.GetString("UserId"))).Include("Food").ToList();
                    decimal? sum = 0;
                    foreach (var item in cartRecords)
                    {
                        sum += item.Count * item.Food.Price;
                    }
                    return Json(sum);
                }
            }
            return Json(0);
            
        }
    }
}
