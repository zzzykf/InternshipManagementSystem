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
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
        // GET: GeneralActivityModule
        
        public ActionResult AnnouncementView()
        {
            List<Models.AnnouncementModel> announcements = new List<Models.AnnouncementModel>();
            announcements = getAnnouncement();
            return View(announcements);
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
                        model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(2)));
                        model.AuthorName = Convert.ToString(rd.GetSqlValue(3));
                        model.Category = Convert.ToString(rd.GetSqlValue(4));
                        model.Title = Convert.ToString(rd.GetSqlValue(5));
                    }
                }
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
                int? id = model.Id;
                if (id == 0)
                {
                    _sql = "INSERT INTO [dbo].[Announcement_Table] VALUES ('"
                        + model.Content + "', '" + model.Datetime + "', '"
                        + model.AuthorName + "', '" + model.Category + "', '"
                        + model.Title + "')";
                }
                else
                {
                    _sql = "UPDATE [dbo].[Announcement_Table] " 
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

        public ActionResult FAQView()
        {
            List<Models.FAQModel> FAQs = new List<Models.FAQModel>();
            FAQs = GetFAQ();
            return View(FAQs);
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
                        model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(3)));
                        model.AuthorUsername = Convert.ToString(rd.GetSqlValue(4));
                    }
                }
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
                int? id = model.Id;
                if (id == 0)
                {
                    _sql = "INSERT INTO [dbo].[FAQ_Table] VALUES ('"
                        + model.Question + "', '" + model.Answer + "', '"
                        + model.Datetime + "', '" + model.AuthorUsername + "')";
                }
                else
                {
                    _sql = "UPDATE [dbo].[FAQ_Table] "
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
                    model.Student_Name = Convert.ToString(rd.GetSqlValue(2));             
                    model.Status = Convert.ToString(rd.GetSqlValue(14));
                    model.Faculty_Supervisor_Name = Convert.ToString(rd.GetSqlValue(12));
                    model.Industrial_Supervisor_Name = Convert.ToString(rd.GetSqlValue(13));
                    model.Evaluation_Mark = Convert.ToString(rd.GetSqlValue(15));
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE F_Supervisor_Name = '"+ ViewBag.Username+ "' OR I_Supervisor_Name = '" + ViewBag.Username + "' ORDER BY Username", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.ReportModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Student_Name = Convert.ToString(rd.GetSqlValue(2));
                    model.Status = Convert.ToString(rd.GetSqlValue(14));
                    model.Faculty_Supervisor_Name = Convert.ToString(rd.GetSqlValue(12));
                    model.Industrial_Supervisor_Name = Convert.ToString(rd.GetSqlValue(13));
                    model.Evaluation_Mark = Convert.ToString(rd.GetSqlValue(15));
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] ORDER BY Username", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    model = new Models.ReportModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Student_Name = Convert.ToString(rd.GetSqlValue(2));
                    model.Status = Convert.ToString(rd.GetSqlValue(14));
                    model.Faculty_Supervisor_Name = Convert.ToString(rd.GetSqlValue(12));
                    model.Industrial_Supervisor_Name = Convert.ToString(rd.GetSqlValue(13));
                    model.Evaluation_Mark = Convert.ToString(rd.GetSqlValue(15));
                    coreport.Add(model);
                }
            }
            return coreport;
        }

        public ActionResult Report()
        {
            if (User.IsInRole("Student"))
            {
                
                return RedirectToAction("StdReportView");
            }
            else if (User.IsInRole("Faculty Supervisor") )
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
                for(i=0;i<list.Count; i++)
                {
                    for(n=i+1;n<list.Count; n++)
                    {
                        if (list[i].Receiver == list[n].Sender && list[i].Sender == list[n].Receiver)
                        {
                            if (list[i].Id > list[i + 1].Id)
                            {
                                list.RemoveAt(i + 1);
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
                    if(list[i].Sender == ViewBag.Username)
                    {
                        temp = list[i].Sender;
                        list[i].Sender = list[i].Receiver;
                        list[i].Receiver = temp;
                    }
                }
            }
          
            return list;
        }
        
        [HttpGet]
        public ActionResult PrivateMessageView(String user)
        {
            ViewBag.TargetUser = user;
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
            ViewBag.TargetUser = pm.PrivateMessage.Receiver; //
            //continue here for saving Private Message using pm.PrivateMessage
            return RedirectToAction("PrivateMessageView","GeneralActivityModule", new { user = pm.PrivateMessage.Receiver });
        }
        public Boolean SavePM(Models.PrivateMessageModel model)
        {
            return true;
        }


    }
}