using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class EvaluationModel
    {
        public int Id { get; set; }
        public String Type { get; set; }
        public String Fk_username { get; set; }
        public int OptionA { get; set; }
        public int OptionB { get; set; }
        public int OptionC { get; set; }
        public int OptionD { get; set; }
        public String Comment { get; set;}
        public String EvaluatedBy { get; set; }
        public UserModel User { get; set; }
        public Double ISupervisorLogbookMark { set; get; }
        public Double ISupervisorInternshipMark { set; get; }
        public Double ISupervisorPresentationMark { set; get; }
        public Double FSupervisorLogbookMark { set; get; }
        public Double FSupervisorInternshipMark { set; get; }
        public Double FSupervisorPresentationMark { set; get; }
    }
}