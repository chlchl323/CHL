using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSystemManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json.Serialization;

namespace OrderSystemManager.Controllers
{
    public class FoodsController : Controller
    {
        //获取当前树节点
        public IActionResult GetTreeIndex()
        {
            return Content(TreeIndex.nowIndex);
        }

        public readonly IWebHostEnvironment _webHostEnvironment;
        public FoodsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult FoodList()
        {
            TreeIndex.nowIndex = "1-1";
            return View();
        }

        //获取指定类型食物集合
        public IActionResult GetFoodList(int pageNum, int foodTypeId)
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                List<Food> foods;
                if (foodTypeId != 0)
                {
                    foods = db.Foods.Include("FoodType").Where(f => f.FoodTypeId == foodTypeId).ToList();
                }
                else
                {
                    foods = db.Foods.Include("FoodType").ToList();
                }
                List<Food> returnFoods = new List<Food>();
                if (pageNum == 0)
                {
                    pageNum = 1;
                }
                for (int i = (pageNum - 1) * 5; i < (pageNum - 1) * 5 + 5 && i < foods.Count; i++)
                {
                    returnFoods.Add(foods[i]);
                }

                return Json(new
                {
                    foodList = returnFoods,
                    pageCount = Math.Ceiling((decimal)foods.Count / 5)
                });
            }
        }

        //删除指定食物
        public IActionResult DelFood(int foodId)
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                db.Foods.Remove(db.Foods.Find(foodId));
                db.SaveChanges();
                return Content("ok");
            }
        }

        public IActionResult FoodEdit(int foodId)
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                Food food = db.Foods.Find(foodId);
                return View(food);
            }
        }

        
        [HttpPost]
        public IActionResult FileUpload()
        {
            int n = Request.Form.Files.Count;
            if (n > 0)
            {
                string dir = Path.Combine(_webHostEnvironment.WebRootPath, "UploadFiles/FoodPics");
                string fullpath = null;
                foreach (IFormFile item in this.Request.Form.Files)
                {
                    fullpath = Path.Combine(dir, item.FileName);
                    using (Stream fs = System.IO.File.Create(fullpath))
                    {
                        item.CopyTo(fs);
                    }
                }
                string aa = Path.GetFileName(fullpath);
                return Content("UploadFiles/FoodPics/"+Path.GetFileName(fullpath));
            }
            return Content("noFile");
        }

        [HttpPost]
        public IActionResult FoodEdit()
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                var json = HttpContext.Request.Form.First().Key;
                var newFood = JsonSerializer.Deserialize<Food>(json);
                Food food = db.Foods.Find(newFood.FoodId);
                if (food != null)
                {
                    food.FoodName = newFood.FoodName;
                    food.FoodTypeId = newFood.FoodTypeId;
                    food.Price = newFood.Price;
                    food.PicUrl = newFood.PicUrl;
                    food.Desc = newFood.Desc;
                }
                db.SaveChanges();
                return Content("ok");
            }
        }

        //添加Food
        public IActionResult FoodAdd()
        {
            TreeIndex.nowIndex = "1-2";
            return View();
        }
        [HttpPost]
        public IActionResult addFood()
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                var json = Request.Form.First().Key;
                var options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
                };
                Food food = JsonSerializer.Deserialize<Food>(json, options);
                if (food != null)
                {
                    food.AddDate = DateTime.Now;
                    food.SalesVolume = 0;
                    db.Foods.Add(food);
                    db.SaveChanges();
                    return Content("ok");
                }
                return Content("erro");
            }
        }

        //查看食物详情
        public IActionResult FoodDetail(int foodId)
        {
            using (OrderManagerDBContext db = new OrderManagerDBContext())
            {
                return View(db.Foods.Include("FoodType").SingleOrDefault(f => f.FoodId == foodId));
            }
        }
    }
}
