using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class AnnouncementModel
    {

        public int Id { get; set; }
        public String Content { get; set; }
        public DateTime Datetime { get; set; }
        public String AuthorName { get; set; }
        public String Category { get; set; }
        public String Title { get; set; }



    }
    public enum Category
    {
        Official,
        Unofficial
    }
}