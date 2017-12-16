using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternshipManagementSystem.Controllers
{
    [Authorize]
    public class FacultyCoordinatorModuleController : BaseController
    {
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";

        [HttpGet]
        public ActionResult AssignSupervisorView(String id)
        {
            if (ViewBag.Role == "Student")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (ViewBag.Role =="Faculty Supervisor" || ViewBag.Role =="Industrial Supervisor")
            {
                return RedirectToAction("Index", "Home");
            }
            List<Models.UserModel> alldetails = new List<Models.UserModel>();
            List<Models.UserModel> found = null;
            alldetails = GetStudentDetails();
            if (id == null)
            {
                ViewBag.Found = true;
                return View(alldetails);
            }
            else
            {
                id = id.ToLower();
                foreach (Models.UserModel user in alldetails)
                {
                    if (user.MatricID.ToLower().Contains(id))
                    {
                        if (found == null)
                        {
                            found = new List<Models.UserModel>();
                        }
                        found.Add(user);
                    }
                    else if (user.FullName.ToLower().Contains(id))
                    {
                        if (found == null)
                        {
                            found = new List<Models.UserModel>();
                        }
                        found.Add(user);
                    }
                    else if (user.State.ToLower().Contains(id))
                    {
                        if (found == null)
                        {
                            found = new List<Models.UserModel>();
                        }
                        found.Add(user);
                    }
                    else if (user.CompanyName.ToLower().Contains(id))
                    {
                        if (found == null)
                        {
                            found = new List<Models.UserModel>();
                        }
                        found.Add(user);
                    }
                    else if (user.FacultySupervisorName.ToLower().Contains(id))
                    {
                        if (found == null)
                        {
                            found = new List<Models.UserModel>();
                        }
                        found.Add(user);
                    }
                    else if (user.IndustrySupervisorName.ToLower().Contains(id))
                    {
                        if (found == null)
                        {
                            found = new List<Models.UserModel>();
                        }
                        found.Add(user);
                    }

                }
                if (found == null)
                {
                    ViewBag.Found = false;
                    return View();
                }
                else
                {
                    ViewBag.Found = true;
                    return View(found);
                }
            }
        }
        public List<Models.UserModel> GetStudentDetails()
        {
            Models.UserModel studentdetail = null;
            List<Models.UserModel> alldetails = new List<Models.UserModel>();
            int i;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Roles = 'Student' ORDER BY Full_Name ASC", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    studentdetail = new Models.UserModel();
                    studentdetail.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    studentdetail.FullName = Convert.ToString(rd.GetSqlValue(4));
                    studentdetail.MatricID = Convert.ToString(rd.GetSqlValue(5));
                    studentdetail.State = Convert.ToString(rd.GetSqlValue(8));
                    studentdetail.CompanyName = Convert.ToString(rd.GetSqlValue(9));
                    studentdetail.FacultySupervisorName = Convert.ToString(rd.GetSqlValue(22));
                    studentdetail.IndustrySupervisorName = Convert.ToString(rd.GetSqlValue(23));
                    studentdetail.InternshipPeriodStart = Convert.ToString(rd.GetSqlValue(16));
                    studentdetail.InternshipPeriodEnd = Convert.ToString(rd.GetSqlValue(17));
                    alldetails.Add(studentdetail);
                }
            }
            for (i = 0; i < alldetails.Count; i++)
            {
                alldetails[i].Id = i + 1;
            }
            return alldetails;
        }
        [HttpGet]
        public ActionResult AssignSupervisorFormView(String fullname)
        {
            if (ViewBag.Role == "Student")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Student = fullname;
            Models.UserModel supervisors = new Models.UserModel();
            supervisors.Supervisorlist = GetFacultySupervisor();
            return View(supervisors);
        }

        public List<Models.UserModel> GetFacultySupervisor()
        {
            Models.UserModel model = null;
            List<Models.UserModel> supervisors = new List<Models.UserModel>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Roles = 'Faculty Supervisor' ORDER BY Full_Name ASC", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model = new Models.UserModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.FullName = Convert.ToString(rd.GetSqlValue(4));
                    supervisors.Add(model);
                }
            }
            return supervisors;
        }
        [HttpPost]
        public ActionResult AssignSupervisor(Models.UserModel supervisor)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[User_Table] SET F_Supervisor_Full_Name = '" + supervisor.FacultySupervisorName + "', F_Supervisor_Name = '" + GetUsernameByName(supervisor.FacultySupervisorName )+ "' WHERE Full_Name = '" + supervisor.FullName + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                cn.Close();
            }
            return RedirectToAction("AssignSupervisorView");
        }
        public String GetUsernameByName(String fullname)
        {
            String username;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Username FROM [dbo].[User_Table] WHERE Full_Name = '" + fullname +"'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                rd.Read();
                username = Convert.ToString(rd.GetSqlValue(0));
            }
            return username;
        }
    }
}