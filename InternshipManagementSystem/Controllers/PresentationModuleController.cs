using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternshipManagementSystem.Controllers
{
    [Authorize]
    public class PresentationModuleController : BaseController
    {
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
        // GET: PresentationModule
        public ActionResult PresentationScheduleCoView()
        {
            if(ViewBag.Role == "Student")
            {
                return RedirectToAction("PresentationScheduleStdView");
            }
            else if (ViewBag.Role == "Industrial Supervisor" || ViewBag.Role=="Faculty Supervisor")
            {
                return RedirectToAction("Index", "Home");
            }
            List<Models.UserModel> alldetails = new List<Models.UserModel>();
            alldetails = GetStudentDetails();
            return View(alldetails);
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
                    studentdetail.Username = Convert.ToString(rd.GetSqlValue(2));
                    studentdetail.FullName = Convert.ToString(rd.GetSqlValue(4));
                    studentdetail.MatricID = Convert.ToString(rd.GetSqlValue(5));
                    studentdetail.State = Convert.ToString(rd.GetSqlValue(8));
                    studentdetail.CompanyName = Convert.ToString(rd.GetSqlValue(9));
                    studentdetail.FacultySupervisorName = Convert.ToString(rd.GetSqlValue(12));
                    studentdetail.IndustrySupervisorName = Convert.ToString(rd.GetSqlValue(13));
                    studentdetail.InternshipPeriodStart = Convert.ToString(rd.GetSqlValue(16));
                    studentdetail.InternshipPeriodEnd = Convert.ToString(rd.GetSqlValue(17));
                    studentdetail.PresentationDate = Convert.ToString(rd.GetSqlValue(18));
                    studentdetail.PresentationTime = Convert.ToString(rd.GetSqlValue(19));
                    studentdetail.PresentationVenue = Convert.ToString(rd.GetSqlValue(20));
                    studentdetail.Accepted = Convert.ToString(rd.GetSqlValue(21));
                    alldetails.Add(studentdetail);
                }
            }
            return alldetails;
        }
        public ActionResult PresentationScheduleFormView(String id)
        {
            Models.UserModel model = new Models.UserModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Id = " + id , cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                model.Username = Convert.ToString(rd.GetSqlValue(2));
            }
            return View(model);
        }
        public ActionResult SetPresentationSchedule(Models.UserModel model)
        {
            String _sql = "";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                _sql = "UPDATE [dbo].[User_Table] "
                        + "SET Presentation_Date = '" + model.PresentationDate
                        + "', Presentation_Time = '" + model.PresentationTime + "', Presentation_Venue ='"
                        + model.PresentationVenue + "', Accepted = 'Waiting for response' WHERE Username = '"
                        + model.Username + "'";
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("PresentationScheduleCoView");
        }
        [Authorize]
        public ActionResult PresentationScheduleStdView()
        {
            Models.UserModel model = new Models.UserModel();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Username = '" + ViewBag.Username + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                model.Username = Convert.ToString(rd.GetSqlValue(2));
                model.PresentationDate = Convert.ToString(rd.GetSqlValue(18));
                model.PresentationTime = Convert.ToString(rd.GetSqlValue(19));
                model.PresentationVenue = Convert.ToString(rd.GetSqlValue(20));
                model.Accepted = Convert.ToString(rd.GetSqlValue(21));
            }
            return View(model);
        }
        public ActionResult UpdateScheduleStatus (String username, String status)
        {
            String finalStatus;
            if(status == "reject")
            {
                finalStatus = "Rejected";
            }
            else
            {
                finalStatus = "Accepted";
            }
            String _sql = "";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                _sql = "UPDATE [dbo].[User_Table] SET "
                        + "Accepted = '" + finalStatus  + "' WHERE Username = '"
                        + username + "'";
                SqlCommand cmd = new SqlCommand(_sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("PresentationScheduleStdView");

        }
    }
}