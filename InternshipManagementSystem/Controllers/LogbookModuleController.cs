using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternshipManagementSystem.Controllers
{
    [Authorize]
    public class LogbookModuleController : BaseController
    {
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";

        // GET: LogbookModule
        public ActionResult LogbookStd()
        {
            if (!User.IsInRole("Student"))
            {
                return RedirectToAction("LogbookSv");
            }
            String username = ViewBag.Username;
            Models.UserModel user = Models.UserModel.GetUserDetails(username);
            List<Models.LogbookModel> logbooks = null;
            logbooks = Models.LogbookModel.GetLogbookList(username);
            dynamic mymodel = new ExpandoObject();
            mymodel.User = user;
            mymodel.Logbooks = logbooks;
            return View(mymodel);
        }
        public ActionResult ViewLogbookStd(int id)
        {
            Models.LogbookModel model = new Models.LogbookModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Logbook_Table] " +
                    "WHERE Id = " + id, cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.AuthorUsername = Convert.ToString(rd.GetSqlValue(1));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(2)));
                    model.Title = Convert.ToString(rd.GetSqlValue(3));
                    model.Description = Convert.ToString(rd.GetSqlValue(4));
                    model.Week = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateNewLogbook(int? id)
        {
            if(ViewBag.Role != "Student")
            {
                return RedirectToAction("LogbookSvView");
            }
            if (id == null)
            {
                return View();
            }

            Models.LogbookModel model = new Models.LogbookModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Logbook_Table] " +
                    "WHERE Id = " + id, cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.AuthorUsername = Convert.ToString(rd.GetSqlValue(1));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(2)));
                    model.Title = Convert.ToString(rd.GetSqlValue(3));
                    model.Description = Convert.ToString(rd.GetSqlValue(4));
                    model.Week = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateNewLogbook(Models.LogbookModel model)
        {
            if (SaveLogbook(model))
            {
                return RedirectToAction("LogbookStd");
            }
            return View(model);
        }

        public bool SaveLogbook(Models.LogbookModel model)
        {
            String _sql = "";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                model.AuthorUsername = ViewBag.Username;
                int? id = model.Id;
                if (id == 0)
                {
                    _sql = "INSERT INTO [dbo].[Logbook_Table] VALUES ('"
                        + model.AuthorUsername + "', '" + model.Datetime + "', '"
                        + model.Title + "', '" + model.Description + "', " + model.Week
                        + ")";
                }
                else
                {
                    _sql = "UPDATE [dbo].[Logbook_Table] "
                        + "SET Datetime = '" + model.Datetime
                        + "', Title = '" + model.Title + "', Description = '"
                        + model.Description + "', Week = " + model.Week + " WHERE Id = "
                        + Convert.ToString(model.Id);
                }
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return true;

        }

        [HttpGet]
        public ActionResult LogBookSv(String id)
        {
            List<Models.UserModel> users = null;
            List<Models.UserModel> foundUsers = new List<Models.UserModel>();
            if (ViewBag.Role == "Faculty Coordinator")
            {
                users = Models.UserModel.GetAllUser();
            }
            else if (ViewBag.Role == "Faculty Supervisor" || ViewBag.Role == "Industrial Supervisor")
            {
                users = Models.UserModel.SvGetUser(ViewBag.Username);
            }
            else return RedirectToAction("LogbookStd");
            dynamic mymodel = new ExpandoObject();

            if (id == null)
            {
                ViewBag.found = false;
                return View(users);
            }
            id = id.ToLower();
            foreach (Models.UserModel model in users)
            {
                if (model.MatricID.ToLower().Contains(id))
                {
                    foundUsers.Add(model);
                }
                else if (model.FullName.ToLower().Contains(id))
                {
                    foundUsers.Add(model);
                }
                else if (model.CompanyName.ToLower().Contains(id))
                {
                    foundUsers.Add(model);
                }
                else if (model.State.ToLower().Contains(id))
                {
                    foundUsers.Add(model);
                }
            }
            if (foundUsers != null)
            {
                ViewBag.Found = true;
                mymodel.Users = foundUsers;
                return View(mymodel);
            }
            else
            {
                ViewBag.Found = false;
                return View();
            }
        }

        public PartialViewResult ViewPartial(String username)
        {
            Models.UserModel user = Models.UserModel.GetUserDetails(username);
            List<Models.LogbookModel> logbooks = null;
            logbooks = Models.LogbookModel.GetLogbookList(username);
            dynamic mymodel = new ExpandoObject();
            mymodel.User = user;
            mymodel.Logbooks = logbooks;
            return PartialView("ViewPartial",mymodel);
        }
        [HttpGet]
        public ActionResult ViewStudentLogbook(String username)
        {
            Models.UserModel user = Models.UserModel.GetUserDetails(username);
            List<Models.LogbookModel> logbooks = null;
            logbooks = Models.LogbookModel.GetLogbookList(username);
            dynamic mymodel = new ExpandoObject();
            mymodel.User = user;
            mymodel.Logbooks = logbooks;
            return View(mymodel);

        }
    }
}