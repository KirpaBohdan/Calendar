using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
    }
}
