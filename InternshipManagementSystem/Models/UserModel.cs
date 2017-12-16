using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "Username")]
        public String Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public String Password { get; set; }
        public int Id { get; set; }
        public String Roles { get; set; }
        public String FullName { get; set; }
        public String MatricID { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String State { get; set; }
        public String CompanyName { get; set; }
        public String CompanyAddress { get; set; }
        public String CompanyContact { get; set; }
        public String FacultySupervisorName { get; set; }
        public String IndustrySupervisorName { get; set; }
        public String InternshipPeriodStart { get; set; }
        public String InternshipPeriodEnd { get; set; }
        public List<UserModel> Supervisorlist { get; set; }
        public String PresentationDate { get; set; }
        public String PresentationTime { get; set; }
        public String PresentationVenue { get; set; }
        public String Accepted { get; set; }

        public static UserModel GetUserDetails (String username)
        {
            UserModel model = new UserModel();
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Username = '" + username + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Roles = Convert.ToString(rd.GetSqlValue(1));
                    model.Username = username;
                    model.FullName = Convert.ToString(rd.GetSqlValue(4));
                    model.MatricID = Convert.ToString(rd.GetSqlValue(5));
                    model.Email = Convert.ToString(rd.GetSqlValue(6));
                    model.PhoneNumber = Convert.ToString(rd.GetSqlValue(7));
                    model.State = Convert.ToString(rd.GetSqlValue(8));
                    model.CompanyName = Convert.ToString(rd.GetSqlValue(9));
                    model.CompanyAddress = Convert.ToString(rd.GetSqlValue(10));
                    model.CompanyContact = Convert.ToString(rd.GetSqlValue(6));
                }
            }
            return model;
        }
        public static List<UserModel> GetAllUser ()
        {
            List<UserModel> users = new List<UserModel>();
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
            UserModel model = null;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Roles = 'Student'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model = new UserModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Roles = Convert.ToString(rd.GetSqlValue(1));
                    model.Username = Convert.ToString(rd.GetSqlValue(2));
                    model.FullName = Convert.ToString(rd.GetSqlValue(4));
                    model.MatricID = Convert.ToString(rd.GetSqlValue(5));
                    model.Email = Convert.ToString(rd.GetSqlValue(6));
                    model.PhoneNumber = Convert.ToString(rd.GetSqlValue(7));
                    model.State = Convert.ToString(rd.GetSqlValue(8));
                    model.CompanyName = Convert.ToString(rd.GetSqlValue(9));
                    model.CompanyAddress = Convert.ToString(rd.GetSqlValue(10));
                    model.CompanyContact = Convert.ToString(rd.GetSqlValue(6));
                    users.Add(model);
                }
                return users;
            }
        }
        public static List<UserModel> SvGetUser(String svUsername)
        {
            String fullname = GetUserFullName(svUsername);
            List<UserModel> users = new List<UserModel>();
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
            UserModel model = null;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[User_Table] WHERE Roles = 'Student' AND (F_Supervisor_Name ='"
                    + svUsername + "' OR I_Supervisor_Name = '" + svUsername +"')", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model = new UserModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.Roles = Convert.ToString(rd.GetSqlValue(1));
                    model.Username = Convert.ToString(rd.GetSqlValue(2));
                    model.FullName = Convert.ToString(rd.GetSqlValue(4));
                    model.MatricID = Convert.ToString(rd.GetSqlValue(5));
                    model.Email = Convert.ToString(rd.GetSqlValue(6));
                    model.PhoneNumber = Convert.ToString(rd.GetSqlValue(7));
                    model.State = Convert.ToString(rd.GetSqlValue(8));
                    model.CompanyName = Convert.ToString(rd.GetSqlValue(9));
                    model.CompanyAddress = Convert.ToString(rd.GetSqlValue(10));
                    model.CompanyContact = Convert.ToString(rd.GetSqlValue(6));
                    users.Add(model);
                }
                return users;
            }
        }
        public static String GetUserFullName(String username)
        {
            String fullname;
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Full_Name FROM [dbo].[User_Table] WHERE Username ='"
                    + username + "'", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                fullname = Convert.ToString(rd.GetSqlValue(0));
                return fullname;
            }
        }
    }
 }

