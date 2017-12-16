using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

namespace InternshipManagementSystem.Controllers
{
    [Authorize]
    public class GeneralActivityModuleController : BaseController
    {
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
        // GET: GeneralActivityModule
        [HttpGet]
        public ActionResult AnnouncementView(String id)
        {
            List<Models.AnnouncementModel> announcements = new List<Models.AnnouncementModel>();
            List<Models.AnnouncementModel> foundAnnouncements = null;
            announcements = getAnnouncement();
            if (id == null)
            {
                ViewBag.Found = true;
                return View(announcements);
            }
            else
            {
                id = id.ToLower();
                foreach (Models.AnnouncementModel model in announcements)
                {
                    if (model.Content.ToLower().Contains(id))
                    {
                        if (foundAnnouncements == null)
                        {
                            foundAnnouncements = new List<Models.AnnouncementModel>();
                        }
                        foundAnnouncements.Add(model);
                    }
                    else if (model.Title.ToLower().Contains(id))
                    {
                        if (foundAnnouncements == null)
                        {
                            foundAnnouncements = new List<Models.AnnouncementModel>();
                        }
                        foundAnnouncements.Add(model);
                    }
                }
                if (foundAnnouncements != null)
                {
                    ViewBag.Found = true;
                    return View(foundAnnouncements);
                }
                else
                {
                    ViewBag.Found = false;
                    return View();
                }
            }
        }
        
        public List<Models.AnnouncementModel> getAnnouncement()
        {
            Models.AnnouncementModel model = null;
            List<Models.AnnouncementModel> announcements = new List<Models.AnnouncementModel>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Announcement_Table] ORDER BY Datetime DESC", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model = new Models.AnnouncementModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Content = Convert.ToString(rd.GetSqlValue(1));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(2)));
                    model.AuthorName = Convert.ToString(rd.GetSqlValue(3));
                    model.Category = Convert.ToString(rd.GetSqlValue(4));
                    model.Title = Convert.ToString(rd.GetSqlValue(5));
                    announcements.Add(model);
                }
            }
            return announcements;
        }
        [HttpGet]
        [Authorize(Roles="Faculty Coordinator")]
        public ActionResult AnnouncementFormView(int? id)
        {
            if(id == null)
            {
                ViewBag.Action = "";
                return View();
            }
            else
            {
                Models.AnnouncementModel model = null;
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Announcement_Table] WHERE Id = " + Convert.ToString(id), cn);
                    cn.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while(rd.Read())
                    {
                        model = new Models.AnnouncementModel();
                        model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                        model.Content = Convert.ToString(rd.GetSqlValue(1));
                        model.Content = model.Content.Replace("<br />", "\r\n");
                        model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(2)));
                        model.AuthorName = Convert.ToString(rd.GetSqlValue(3));
                        model.Category = Convert.ToString(rd.GetSqlValue(4));
                        model.Title = Convert.ToString(rd.GetSqlValue(5));
                    }
                }
                ViewBag.Action = "Edit";
                return View(model);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Faculty Coordinator")]
        public ActionResult AnnouncementFormView (Models.AnnouncementModel model)
        {
            if (SaveAnnouncement(model))
            {
                return RedirectToAction("AnnouncementView");
            }
            return View(model);
        }
        [Authorize(Roles = "Faculty Coordinator")]
        public ActionResult DeleteAnnouncement(int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[Announcement_Table] "
                    + "WHERE Id = " + Convert.ToString(id), cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("AnnouncementView");
            }

        }
        [Authorize(Roles = "Faculty Coordinator")]
        public bool SaveAnnouncement(Models.AnnouncementModel model)
        {
            String _sql = "";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                model.AuthorName = ViewBag.Username;
                model.Datetime = DateTime.Now;
                model.Content = model.Content.Replace("\r\n", "<br />");
                int? id = model.Id;
                if (id == 0)
                {
                    _sql = @"INSERT INTO [dbo].[Announcement_Table] VALUES ('"
                        + model.Content + "', '" + Convert.ToString(model.Datetime) + "', '"
                        + model.AuthorName + "', '" + model.Category + "', '"
                        + model.Title + "')";
                }
                else
                {
                    _sql = @"UPDATE [dbo].[Announcement_Table] " 
                        +"SET Contents = '" + model.Content
                        +"', Title = '" + model.Title + "', Category ='"
                        + model.Category + "' WHERE Id = " 
                        + Convert.ToString(model.Id);
                }
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                    cmd.ExecuteNonQuery();
            }
            return true;
        }
        [HttpGet]
        public ActionResult FAQView(String id)
        {
            List<Models.FAQModel> FAQs = new List<Models.FAQModel>();
            List<Models.FAQModel> foundFAQs = null;
            FAQs = GetFAQ();
            if (id == null)
            {
                ViewBag.Found = true;
                return View(FAQs);
            }
            else
            {
                id = id.ToLower();
                foreach (Models.FAQModel model in FAQs)
                {
                    if (model.Question.ToLower().Contains(id))
                    {
                        if (foundFAQs == null)
                        {
                            foundFAQs = new List<Models.FAQModel>();
                        }
                        foundFAQs.Add(model);
                    }
                    else if (model.Answer.ToLower().Contains(id))
                    {
                        if (foundFAQs == null)
                        {
                            foundFAQs = new List<Models.FAQModel>();
                        }
                        foundFAQs.Add(model);
                    }
                }
                if (foundFAQs != null)
                {
                    ViewBag.Found = true;
                    return View(foundFAQs);
                }
                else
                {
                    ViewBag.Found = false;
                    return View();
                }
            }
        }
        public List<Models.FAQModel> GetFAQ()
        {
            Models.FAQModel model = null;
            List<Models.FAQModel> FAQs = new List<Models.FAQModel>();
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[FAQ_Table]", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.FAQModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Question = Convert.ToString(rd.GetSqlValue(1));
                    model.Answer = Convert.ToString(rd.GetSqlValue(2));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(3)));
                    model.AuthorUsername = Convert.ToString(rd.GetSqlValue(4));
                    FAQs.Add(model);
                }
            }
            return FAQs;
        }
        [HttpGet]
        [Authorize(Roles = "Faculty Coordinator")]
        public ActionResult FAQFormView (int? id)
        {
            if(id == null)
            {
                ViewBag.Action = "";
                return View();
            }
            else
            {
                Models.FAQModel model = null;
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[FAQ_Table] WHERE Id = " + Convert.ToString(id), cn);
                    cn.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        model = new Models.FAQModel();
                        model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                        model.Question = Convert.ToString(rd.GetSqlValue(1));
                        model.Answer = Convert.ToString(rd.GetSqlValue(2));
                        model.Answer = model.Answer.Replace("<br />", "\r\n");
                        model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(3)));
                        model.AuthorUsername = Convert.ToString(rd.GetSqlValue(4));
                    }
                }
                ViewBag.Action = "Edit";
                return View(model);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Faculty Coordinator")]
        public ActionResult FAQFormView (Models.FAQModel model)
        {
            if (SaveFAQ(model))
            {
                return RedirectToAction("FAQView");
            }
            return View(model);
        }
        [Authorize(Roles = "Faculty Coordinator")]
        public ActionResult DeleteFAQ(int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[FAQ_Table] "
                    + "WHERE Id = " + Convert.ToString(id), cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("FAQView");
            }

        }
        [Authorize(Roles = "Faculty Coordinator")]
        public bool SaveFAQ(Models.FAQModel model)
        {
            String _sql = "";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                model.AuthorUsername = ViewBag.Username;
                model.Datetime = DateTime.Now;
                model.Question = model.Question.Replace("\r\n", "<br />");
                model.Answer = model.Answer.Replace("\r\n", "<br />");
                int? id = model.Id;
                if (id == 0)
                {
                    _sql = @"INSERT INTO [dbo].[FAQ_Table] VALUES ('"
                        + model.Question + "', '" + model.Answer + "', '"
                        + model.Datetime + "', '" + model.AuthorUsername + "')";
                }
                else
                {
                    _sql = @"UPDATE [dbo].[FAQ_Table] "
                        + "SET Question = '" + model.Question
                        + "', Answer = '" + model.Answer + "' WHERE Id = "
                        + Convert.ToString(model.Id);
                }
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return true;

        }
        public ActionResult StdReportView()
        {
            Models.ReportModel report = new Models.ReportModel();
            report = GetStudentReport();
            return View(report);
        }
        public Models.ReportModel GetStudentReport()
        {
            Models.ReportModel model = new Models.ReportModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Username = '"+ ViewBag.Username +"'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Username = Convert.ToString(rd.GetSqlValue(2));
                    model.Student_Name = Convert.ToString(rd.GetSqlValue(4));             
                    model.Status = Convert.ToString(rd.GetSqlValue(14));
                    model.Faculty_Supervisor_Name = Convert.ToString(rd.GetSqlValue(22));
                    model.Industrial_Supervisor_Name = Convert.ToString(rd.GetSqlValue(23));
                    model.Evaluation_Mark = GetEvaluationMark(model.Username);
                }
            }
            return model;
        }
        public ActionResult SvReportView()
        {
            List<Models.ReportModel> svreport = new List<Models.ReportModel>();
            svreport = GetSvReport();
            return View(svreport);
        }

        public List<Models.ReportModel> GetSvReport()
        {
            Models.ReportModel model = null;
            List<Models.ReportModel> svreports = new List<Models.ReportModel>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE F_Supervisor_Name = '"+ ViewBag.Username+ "' OR I_Supervisor_Name ='" + ViewBag.Username +"' ORDER BY Username", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.ReportModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Username = Convert.ToString(rd.GetSqlValue(2));
                    model.Student_Name = Convert.ToString(rd.GetSqlValue(4));
                    model.Status = Convert.ToString(rd.GetSqlValue(14));
                    model.Faculty_Supervisor_Name = Convert.ToString(rd.GetSqlValue(22));
                    model.Industrial_Supervisor_Name = Convert.ToString(rd.GetSqlValue(23));
                    model.Evaluation_Mark = GetEvaluationMark(model.Username);
                    svreports.Add(model);

                }
                
            }
            return svreports;
        }
        public ActionResult CoReportView()
        {
            List<Models.ReportModel> coreports = new List<Models.ReportModel>();
            coreports = GetCoReport();
            return View(coreports);
        }
        public List<Models.ReportModel> GetCoReport()
        {
            Models.ReportModel model = null;
            List<Models.ReportModel> coreport = new List<Models.ReportModel>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Roles ='Student' ORDER BY Username", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.ReportModel();
                    model.Username = Convert.ToString(rd.GetSqlValue(2));
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Student_Name = Convert.ToString(rd.GetSqlValue(4));
                    model.Status = Convert.ToString(rd.GetSqlValue(14));
                    model.Faculty_Supervisor_Name = Convert.ToString(rd.GetSqlValue(22));
                    model.Industrial_Supervisor_Name = Convert.ToString(rd.GetSqlValue(23));
                    model.Evaluation_Mark = GetEvaluationMark(model.Username);
                    coreport.Add(model);
                }
            }
            return coreport;
        }
        public String GetEvaluationMark (String username)
        {
            List<Models.EvaluationModel> list = new List<Models.EvaluationModel>();
            Models.EvaluationModel model = null;
            Models.EvaluationModel result = new Models.EvaluationModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Evaluation_Table] WHERE Fk_username = '" + username + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.EvaluationModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Type = Convert.ToString(rd.GetSqlValue(1));
                    model.Fk_username = Convert.ToString(rd.GetSqlValue(2));
                    model.OptionA = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(3)));
                    model.OptionB = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(4)));
                    model.OptionC = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                    model.OptionD = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(6)));
                    model.Comment = Convert.ToString(rd.GetSqlValue(7));
                    model.EvaluatedBy = Convert.ToString(rd.GetSqlValue(8));
                    list.Add(model);
                }
                foreach (Models.EvaluationModel test in list)
                {
                    if (test.EvaluatedBy == "Industrial Supervisor" && test.Type == "Internship")
                    {
                        result.ISupervisorInternshipMark = (Double)(test.OptionA + test.OptionB + test.OptionC) / 15 * 100/6;
                    }
                    else if (test.EvaluatedBy == "Industrial Supervisor" && test.Type == "Logbook")
                    {
                        result.ISupervisorLogbookMark = (Double)(test.OptionA + test.OptionB + test.OptionC) / 15 * 100 / 6;
                    }
                    else if (test.EvaluatedBy == "Industrial Supervisor" && test.Type == "Presentation")
                    {
                        result.ISupervisorPresentationMark = (Double)(test.OptionA + test.OptionB + test.OptionC + test.OptionD) / 20 * 100 / 6;
                    }
                    else if (test.EvaluatedBy == "Faculty Supervisor" && test.Type == "Internship")
                    {
                        result.FSupervisorInternshipMark = (Double)(test.OptionA + test.OptionB + test.OptionC) / 15 * 100 / 6;
                    }
                    else if (test.EvaluatedBy == "Faculty Supervisor" && test.Type == "Logbook")
                    {
                        result.FSupervisorLogbookMark = (Double)(test.OptionA + test.OptionB + test.OptionC) / 15 * 100 / 6;
                    }
                    else if (test.EvaluatedBy == "Faculty Supervisor" && test.Type == "Presentation")
                    {
                        result.FSupervisorPresentationMark = (Double)(test.OptionA + test.OptionB + test.OptionC + test.OptionD) / 20 * 100 / 6;
                    }
                }
                cn.Close();
            }
            Double resultDouble = result.FSupervisorInternshipMark + result.FSupervisorLogbookMark + result.FSupervisorPresentationMark + result.ISupervisorInternshipMark + result.ISupervisorLogbookMark + result.ISupervisorPresentationMark;
            
            return resultDouble.ToString("0.00");
        }
        public ActionResult Report()
        {
            if (User.IsInRole("Student"))
            {
                
                return RedirectToAction("StdReportView");
            }
            else if (User.IsInRole("Faculty Supervisor") || User.IsInRole ("Industrial Supervisor"))
            {
                return RedirectToAction("SvReportView");
            }
            else
            {
                return RedirectToAction("CoReportView");
            }
        }

        public ActionResult ReleaseReportView()
        {
            Models.ReportModel ReleaseReport = new Models.ReportModel();
            ReleaseReport = GetReleaseReport();
            return View(ReleaseReport);
        }
        public Models.ReportModel GetReleaseReport()
        {
            Models.ReportModel report = new Models.ReportModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Username = '" + ViewBag.Username + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    report.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    report.Student_Name = Convert.ToString(rd.GetSqlValue(2));
                    report.Matric_Id = Convert.ToString(rd.GetSqlValue(5));
                    report.Company_Name = Convert.ToString(rd.GetSqlValue(9));
                    report.State = Convert.ToString(rd.GetSqlValue(8));
                    report.Status = Convert.ToString(rd.GetSqlValue(14));
                    report.Internship_Start = Convert.ToString(rd.GetSqlValue(16));
                    report.Internship_End = Convert.ToString(rd.GetSqlValue(17));
                }
            }

            return report;
        }
        public ActionResult SubmitReportStatus()
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[User_Table] SET Status = 'Completed' WHERE Username = '" + ViewBag.Username + "'", cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                return RedirectToAction("StdReportView");
            }

        }

        public ActionResult NewPrivateMessageView()
        {
            List<Models.PrivateMessageModel> pm = new List<Models.PrivateMessageModel>();
            pm = GetNewPrivateMessage();
            return View(pm);
        }
        public List<Models.PrivateMessageModel> GetNewPrivateMessage()
        {
            List<Models.PrivateMessageModel> list = new List<Models.PrivateMessageModel>();
            Models.PrivateMessageModel model = null;
            String _sql;
            int i = 0, n = 0 ;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                _sql = "SELECT * FROM [dbo].[PrivateMessage_Table] WHERE Id IN (SELECT max(Id) FROM [dbo].[PrivateMessage_Table] WHERE Sender_Username = '" + ViewBag.Username + "' OR Receiver_Username ='"
                    + ViewBag.Username + "' GROUP BY Sender_Username, Receiver_Username) ORDER BY Id DESC";
                /*_sql = "SELECT 'User' = CASE WHEN Sender_Username = '" + ViewBag.Username
                    + "' THEN Receiver_Username ELSE Sender_Username END FROM [dbo].[PrivateMessage_Table] WHERE Sender_Username = '"
                    + ViewBag.Username + "' OR Receiver_Username = '" + ViewBag.Username + "'";*/
                SqlCommand cmd = new SqlCommand(_sql, cn);


                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.PrivateMessageModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Sender = Convert.ToString(rd.GetSqlValue(1));
                    model.Receiver = Convert.ToString(rd.GetSqlValue(2));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(3)));
                    model.Message = Convert.ToString(rd.GetSqlValue(4));
                    list.Add(model);
                }
                cn.Close();
                for(i=0;i<list.Count; i++)
                {
                    for(n=i+1;n<list.Count; n++)
                    {
                        if (list[i].Receiver == list[n].Sender && list[i].Sender == list[n].Receiver)
                        {
                            if (list[i].Id > list[n].Id)
                            {
                                list.RemoveAt(n);
                                break;
                            }
                            else
                            {
                                list.RemoveAt(i);
                                break;
                            }

                        }
                    }
                }
                for (i = 0; i < list.Count; i++)
                {
                    String temp;
                    if (list[i].Sender == ViewBag.Username)
                    {
                        temp = list[i].Sender;
                        list[i].Sender = list[i].Receiver;
                        list[i].Receiver = temp;
                    }
                }
                for (i=0; i< list.Count; i++)
                {
                    _sql = "SELECT Full_Name FROM [dbo].[User_Table] WHERE Username ='" + list[i].Sender + "'";
                    SqlCommand cmd2 = new SqlCommand(_sql, cn);
                    cn.Open();
                    SqlDataReader rd2 = cmd2.ExecuteReader();
                    rd2.Read();
                    list[i].FullName = Convert.ToString(rd2.GetSqlValue(0));
                    cn.Close();
                }

            }
          
            return list;
        }
        
        [HttpGet]
        public ActionResult PrivateMessageView(String user, String fullname)
        {
            ViewBag.TargetUser = user;
            ViewBag.FullName = fullname;
            Models.MyViewModel model = new Models.MyViewModel();
            model.PrivateMessageList = GetPrivateMessage(user);
            model.PrivateMessage = new Models.PrivateMessageModel();
            model.PrivateMessage.Receiver = user;
            model.PrivateMessage.Sender = ViewBag.Username;
            return View(model);
        }

        
        public List<Models.PrivateMessageModel> GetPrivateMessage (String user)
        {
            Models.PrivateMessageModel model = null;
            List<Models.PrivateMessageModel> privatemessage = new List<Models.PrivateMessageModel>();
            String _sql;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                _sql = "SELECT * FROM [dbo].[PrivateMessage_Table] WHERE Sender_Username = '" + user + "' AND Receiver_Username = '"
                    + ViewBag.Username + "' OR Sender_Username = '" + ViewBag.Username + "' AND Receiver_Username = '" + user
                    + "' ORDER BY Datetime ASC";
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.PrivateMessageModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Sender = Convert.ToString(rd.GetSqlValue(1));
                    model.Receiver = Convert.ToString(rd.GetSqlValue(2));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(3)));
                    model.Message = Convert.ToString(rd.GetSqlValue(4));
                    privatemessage.Add(model);
                }

            }
            return privatemessage;

        }
        [HttpPost]
        public ActionResult PrivateMessageView(Models.MyViewModel pm)
        {
            
            ViewBag.TargetUser = pm.PrivateMessage.Receiver;
            //continue here for saving Private Message using pm.PrivateMessage
            SavePM(pm.PrivateMessage);
            return RedirectToAction("PrivateMessageView", "GeneralActivityModule", new { user = pm.PrivateMessage.Receiver });
            
            
            
            
        }

        public bool SavePM(Models.PrivateMessageModel model)
        {
            
            String _sql = "";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                model.Sender = ViewBag.Username;
                model.Datetime = DateTime.Now;
                int? id = model.Id;
                model.Message = model.Message.Replace("\r\n", "<br />");
                if (id == 0)
                {
                    _sql = "INSERT INTO [dbo].[PrivateMessage_Table] VALUES ('"
                        + model.Sender + "', '" + model.Receiver + "', '"
                        + model.Datetime + "', '" + model.Message + "')";
                }
                
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return true;

        }
         
        [HttpGet]
        public ActionResult PrivateMessageFormView()
        {
            Models.PrivateMessageModel userlist = new Models.PrivateMessageModel();
            userlist.ReceiverList = GetUser();
            return View(userlist);
        }
        public List<Models.UserModel> GetUser()
        {
            Models.UserModel model = null;
            List<Models.UserModel> receiver = new List<Models.UserModel>();
            String _sql;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                _sql = "SELECT * FROM [dbo].[User_Table] WHERE Username NOT IN('" + ViewBag.Username + "')" ;
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.UserModel();
                    model.Username = Convert.ToString(rd.GetSqlValue(2));
                    model.FullName = Convert.ToString(rd.GetSqlValue(4));
                    receiver.Add(model);
                }

            }
            return receiver;
        }
        
        [HttpPost]
        public ActionResult PrivateMessageFormView(Models.PrivateMessageModel send)
        {
            send.Message = send.Message.Replace("\r\n", "<br />");
            SaveOm(send);
            return RedirectToAction("NewPrivateMessageView");
        }
        public bool SaveOm(Models.PrivateMessageModel model)
        {
            String _sql = "";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                model.Datetime = DateTime.Now;
                
                    _sql = "INSERT INTO [dbo].[PrivateMessage_Table] VALUES ('"
                        + ViewBag.Username + "', '" + model.Receiver + "', '"
                        + model.Datetime + "', '" + model.Message + "')";
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return true;

        }



    }
}