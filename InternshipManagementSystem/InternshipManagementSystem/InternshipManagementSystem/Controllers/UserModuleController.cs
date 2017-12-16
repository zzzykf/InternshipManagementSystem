using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;

namespace InternshipManagementSystem.Controllers
{
    public class UserModuleController : Controller
    {
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zzzyk\Desktop\InternshipManagementSystem\InternshipManagementSystem\InternshipManagementSystem\App_Data\IMSdb.mdf;Integrated Security=True";
        // GET: UserModule
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "UserModule");
        }

        [HttpPost]
        public ActionResult Login(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (VerifyLogin(user.Username, user.Password))
                {
                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        String _sql = @"SELECT * FROM [dbo].[User_Table] " +
                            @"WHERE [Username] = '" + user.Username +"'";
                        var cmd = new SqlCommand(_sql, cn);
                        cn.Open();
                        var rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            user.Roles = Convert.ToString(rd.GetSqlValue(1));
                        }
                    }
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.Roles);
                    string encryptedTicket = FormsAuthentication .Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if(RegisterUser(user))
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.Roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The username already exists!");
                }

            }
            return View(user);
        }



        public bool VerifyLogin(string _username, string _password)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                String _sql = @"SELECT [Username] FROM [dbo].[User_Table] " +
                    @"WHERE [Username] = @u AND [Password] = @p";
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = _username;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = _password;
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return true;
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return false;
                }
            }

        }

        public bool RegisterUser (Models.UserModel user)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                String _sql = @"INSERT INTO [dbo].[User_Table] VALUES (" + "'" + user.Roles
                    + "', '" + user.Username + "', '" + user.Password + "', '" + user.FullName
                    + "', '" + user.MatricID + "', '" + user.Email + "', '" + user.PhoneNumber
                    + "', '" + user.State + "', '" + user.CompanyName + "', '" + user.CompanyAddress
                    + "', '" + user.CompanyContact + "')";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;

            }
        }
    }
}