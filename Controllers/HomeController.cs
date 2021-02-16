using Calendar.Data;
using Calendar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Threading.Tasks;

namespace Calendar.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        
        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        [NonAction]
        private void CreateViewBag(int y, int m, int d)
        {
            ViewBag.Year = y;
            ViewBag.Month = m;
            ViewBag.Day = d;


            ViewBag.TaskData = (from task in db.Tasks
                                where task.Date > new DateTime(y, m, d)
                                where task.Date < new DateTime(y, m, d, 23, 59, 59)
                                select task);
        }
        public IActionResult Index(int? y, int? m)
        {
            DateTime date;
            if (y == null || m == null)
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            else
                date = new DateTime((int)y, (int)m, 1);
            

            ViewBag.Year = date.Year;
            ViewBag.Month = date.Month;
            ViewBag.MonthString = date.ToString("Y");
            ViewBag.Day = date.Day;
            ViewBag.StartDay = (int)new DateTime(date.Year, date.Month, 1).DayOfWeek;
            ViewBag.DaysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            //string time = DateTime.Now.ToShortDateString();

            var tasks = (from task in db.Tasks
                    where task.Date > new DateTime(date.Year, date.Month, 1)
                    where task.Date < new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59)
                    select task);

            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                var num = from a in tasks
                          where a.Date > new DateTime(date.Year, date.Month, i)
                          where a.Date < new DateTime(date.Year, date.Month, i, 23, 59, 59)
                          select a;
                dict.Add(i, num.Count());
            }


            ViewBag.TaskNum = dict;
            
            return View();
        }
        public IActionResult TimeTable(int y, int m, int d)
        {
            CreateViewBag(y, m, d);

            return View();
        }
        [HttpPost]
        public IActionResult TimeTable(int y, int m, int d, int h, int min, string text)
        {
            db.Tasks.Add(new Task() { Date = new DateTime(y, m, d, h, min, 0), Value = text});
            db.SaveChanges();

            CreateViewBag(y, m, d);

            return View("TimeTable");
        }

        public IActionResult Delete(int id, int y, int m, int d)
        {
            try
            {
                Task t = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
                db.Tasks.Remove(t);
                db.SaveChanges();
            }
            catch { }

            CreateViewBag(y, m, d);

            return View("TimeTable");
        }
    }
}
