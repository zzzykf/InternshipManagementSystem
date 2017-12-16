using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class FAQModel
    {
        public int Id { get; set; }
        public String Question { get; set; }
        public String Answer { get; set; }
        public DateTime Datetime { get; set; }
        public String AuthorUsername{ get; set; }
    }
}