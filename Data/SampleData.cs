using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Data
{
    public static class SampleData
    {
        public static void Initialize(ApplicationContext db)
        {
            Models.Task task = new Models.Task()
            {
                Value = "Перевірити пошту",
                Date = new DateTime(2021, 2, 15, 10, 30, 0)
            };

            db.Tasks.Add(task);
            db.SaveChanges();
        }
    }
}
