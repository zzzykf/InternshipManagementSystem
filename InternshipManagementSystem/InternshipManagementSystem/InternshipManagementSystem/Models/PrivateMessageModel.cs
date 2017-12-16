using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class PrivateMessageModel
    {
        public int Id { get; set; }
        public String Sender { get; set; }
        public String Receiver { get; set; }
        public String Message { get; set; }
        public DateTime Datetime { get; set; }

     
    }
}