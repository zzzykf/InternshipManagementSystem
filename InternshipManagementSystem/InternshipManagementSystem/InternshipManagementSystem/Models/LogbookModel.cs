using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InternshipManagementSystem.Models
{
    public class LogbookModel
    {
        public int Id { get; set; }
        public String AuthorUsername { get; set; }
        public DateTime Datetime { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public int Week { get; set; }

        public static List<LogbookModel> GetLogbookList (String authorname)
        {
            List<LogbookModel> logbooks = new List<LogbookModel>();
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
            LogbookModel model = null;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Logbook_Table] " + 
                    "WHERE FK_Username = '" + authorname + "' ORDER BY Datetime ASC", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model = new LogbookModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.AuthorUsername = Convert.ToString(rd.GetSqlValue(1));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(2)));
                    model.Title = Convert.ToString(rd.GetSqlValue(3));
                    model.Description = Convert.ToString(rd.GetSqlValue(4));
                    model.Week = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                    logbooks.Add(model);
                }
                return logbooks;
            }

        }
        public static List<LogbookModel> GetAllLogbook()
        {
            List<LogbookModel> logbooks = new List<LogbookModel>();
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
            LogbookModel model = null;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Logbook_Table]", cn);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    model = new LogbookModel();
                    model.Id = Int32.Parse(Convert.ToString(rd.GetSqlValue(0)));
                    model.AuthorUsername = Convert.ToString(rd.GetSqlValue(1));
                    model.Datetime = Convert.ToDateTime(Convert.ToString(rd.GetSqlValue(2)));
                    model.Title = Convert.ToString(rd.GetSqlValue(3));
                    model.Description = Convert.ToString(rd.GetSqlValue(4));
                    model.Week = Convert.ToInt32(Convert.ToString(rd.GetSqlValue(5)));
                    logbooks.Add(model);
                }
                return logbooks;
            }
        }
    }
}