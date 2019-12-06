using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public bool IsMale { get; set; }
        public int Request { get; set; }
    }
}
