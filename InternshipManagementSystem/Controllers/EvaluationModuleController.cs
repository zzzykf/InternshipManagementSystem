using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternshipManagementSystem.Controllers
{
    public class EvaluationModuleController : BaseController
    {
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";

        // GET: EvaluationModule
        [HttpGet]
        public ActionResult EvaluationSvView (String id)
        {
            List<Models.UserModel> users = null;
            List<Models.UserModel> foundUsers = new List<Models.UserModel>();
            users = Models.UserModel.SvGetUser(ViewBag.Username);
            if(ViewBag.Role == "Student")
            {
                return RedirectToAction("EvaluationStdView");
            }
            else if (ViewBag.Role == "Faculty Coordinator")
            {
                return RedirectToAction("Index", "Home");
            }
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
                return View(foundUsers);
            }
            else
            {
                ViewBag.Found = false;
                return View();
            }
        }
        public ActionResult LogbookEvaluation (String username)
        {
            if(username == null)
            {
                return RedirectToAction("EvaluationSvView");
            }
            Models.EvaluationModel evaluation = new Models.EvaluationModel();
            evaluation = GetLogbookEvaluation(username);
            evaluation.User = new Models.UserModel();
            evaluation.User = Models.UserModel.GetUserDetails(username);
            return View(evaluation);
        }
        public Models.EvaluationModel GetLogbookEvaluation (String username)
        {
            Models.EvaluationModel model = new Models.EvaluationModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Evaluation_Table] WHERE Type ='Logbook' AND Fk_username = '" + username + "' AND Evaluated_By ='" + ViewBag.Role + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Type = Convert.ToString(rd.GetSqlValue(1));
                    model.Fk_username = Convert.ToString(rd.GetSqlValue(2));
                    model.OptionA = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(3)));
                    model.OptionB = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(4)));
                    model.OptionC = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                    if (model.Type == "Presentation") model.OptionD = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(6)));
                    model.Comment = Convert.ToString(rd.GetSqlValue(7));
                    model.EvaluatedBy = Convert.ToString(rd.GetSqlValue(8));

                }
            }
            return model;
        }
        public ActionResult EvaluateLogbook(Models.EvaluationModel model)
        {
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                String _sql = @"INSERT INTO [dbo].[Evaluation_Table] VALUES ('Logbook', '"
                + model.User.Username + "', " + model.OptionA + " , " + model.OptionB + " , "
                + model.OptionC + ", 0, '" + model.Comment + "', '" + model.EvaluatedBy + "')";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("EvaluationSvView");
        }
        public ActionResult InternshipEvaluation (String username)
        {
            if(username == null)
            {
                return RedirectToAction("EvaluationSvView");
            }
            Models.EvaluationModel evaluation = new Models.EvaluationModel();
            evaluation = GetInternshipEvaluation(username);
            evaluation.User = new Models.UserModel();
            evaluation.User = Models.UserModel.GetUserDetails(username);
            return View(evaluation);
        }
        public Models.EvaluationModel GetInternshipEvaluation(String username)
        {
            Models.EvaluationModel model = new Models.EvaluationModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Evaluation_Table] WHERE Type ='Internship' AND Fk_username = '" + username + "' AND Evaluated_By ='" + ViewBag.Role + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Type = Convert.ToString(rd.GetSqlValue(1));
                    model.Fk_username = Convert.ToString(rd.GetSqlValue(2));
                    model.OptionA = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(3)));
                    model.OptionB = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(4)));
                    model.OptionC = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                    model.Comment = Convert.ToString(rd.GetSqlValue(7));
                    model.EvaluatedBy = Convert.ToString(rd.GetSqlValue(8));

                }
            }
            return model;
        }
        public ActionResult EvaluateInternship(Models.EvaluationModel model)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                String _sql = @"INSERT INTO [dbo].[Evaluation_Table] VALUES ('Internship', '"
                + model.User.Username + "', " + model.OptionA + " , " + model.OptionB + " , "
                + model.OptionC + ", 0, '" + model.Comment + "', '" + model.EvaluatedBy + "')";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("EvaluationSvView");

        }
        public ActionResult PresentationEvaluation(String username)
        {
            if (username == null)
            {
                return RedirectToAction("EvaluationSvView");
            }
            Models.EvaluationModel evaluation = new Models.EvaluationModel();
            evaluation = GetPresentationEvaluation(username);
            evaluation.User = new Models.UserModel();
            evaluation.User = Models.UserModel.GetUserDetails(username);
            return View(evaluation);
        }
        public Models.EvaluationModel GetPresentationEvaluation(String username)
        {
            Models.EvaluationModel model = new Models.EvaluationModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Evaluation_Table] WHERE Type ='Presentation' AND Fk_username = '" + username + "' AND Evaluated_By ='" + ViewBag.Role + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Type = Convert.ToString(rd.GetSqlValue(1));
                    model.Fk_username = Convert.ToString(rd.GetSqlValue(2));
                    model.OptionA = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(3)));
                    model.OptionB = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(4)));
                    model.OptionC = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                    model.OptionD = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(6)));
                    model.Comment = Convert.ToString(rd.GetSqlValue(7));
                    model.EvaluatedBy = Convert.ToString(rd.GetSqlValue(8));
                }
            }
            return model;
        }
        public ActionResult EvaluatePresentation(Models.EvaluationModel model)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                String _sql = @"INSERT INTO [dbo].[Evaluation_Table] VALUES ('Presentation', '"
                + model.User.Username + "', " + model.OptionA + " , " + model.OptionB + " , "
                + model.OptionC + ", 0, '" + model.Comment + "', '" + model.EvaluatedBy + "')";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("EvaluationSvView");

        }
        public ActionResult EvaluationStdView()
        {
            List<Models.EvaluationModel> list = new List<Models.EvaluationModel>();
            Models.EvaluationModel model = null;
            Models.EvaluationModel result = new Models.EvaluationModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Evaluation_Table] WHERE Fk_username = '" + ViewBag.Username + "'", cn);
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
                foreach(Models.EvaluationModel test in list)
                {
                    if(test.EvaluatedBy == "Industrial Supervisor" && test.Type == "Internship")
                    {
                        result.ISupervisorInternshipMark = (Double)(test.OptionA + test.OptionB + test.OptionC) / 15 * 50;
                    }
                    else if (test.EvaluatedBy == "Industrial Supervisor" && test.Type == "Logbook")
                    {
                        result.ISupervisorLogbookMark = (Double)(test.OptionA + test.OptionB + test.OptionC) / 15 * 50;
                    }
                    else if (test.EvaluatedBy == "Industrial Supervisor" && test.Type == "Presentation")
                    {
                        result.ISupervisorPresentationMark = (Double)(test.OptionA + test.OptionB + test.OptionC + test.OptionD) / 20 * 50;
                    }
                    else if (test.EvaluatedBy == "Faculty Supervisor" && test.Type == "Internship")
                    {
                        result.FSupervisorInternshipMark = (Double)(test.OptionA + test.OptionB + test.OptionC) / 15 * 50;
                    }
                    else if (test.EvaluatedBy == "Faculty Supervisor" && test.Type == "Logbook")
                    {
                        result.FSupervisorLogbookMark = (Double)(test.OptionA + test.OptionB + test.OptionC ) / 15 * 50;
                    }
                    else if (test.EvaluatedBy == "Faculty Supervisor" && test.Type == "Presentation")
                    {
                        result.FSupervisorPresentationMark = (Double)(test.OptionA + test.OptionB + test.OptionC + test.OptionD) / 20 * 50;
                    }
                }
            }
            return View(result);
        }
    }
}