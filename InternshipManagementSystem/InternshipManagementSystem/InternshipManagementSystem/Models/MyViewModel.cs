using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class MyViewModel
    {
        public List<PrivateMessageModel> PrivateMessageList { get; set; }
        public PrivateMessageModel PrivateMessage { get; set; }

    }
}