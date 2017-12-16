using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class ReportModel
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String Student_Name { get; set; }
        public String Evaluation_Mark { get; set; }
        public String Status { get; set; }
        public String Faculty_Supervisor_Name { get; set; }
        public String Industrial_Supervisor_Name { get; set; }
        public String Matric_Id { get; set; }
        public String Company_Name { get; set; }
        public String State { get; set; }
        public String Internship_Start { get; set; }
        public String Internship_End { get; set; }
        public String FullName { get; set; }
        public String Role { get; set; }
    }
}