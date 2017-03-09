using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using WhosOnFirst.Data.Sql.Controllers;
using WhosOnFirst.Data.Sql.Models;
using WhosOnFirstWeb.Models;

namespace WhosOnFirstWeb.Controllers
{
    public class HomeController : Controller
    {
        #region Index

        public ActionResult Index()
        {
            ViewBag.Warning = "";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginRequestModel loginRequestModel)
        {
            try
            {
                var security = new SqlSecurity(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
                var userValidation = await security.AuthenticateAsync(loginRequestModel.userName, loginRequestModel.password);
                var person = new Person();
                var personManager =
                    new PersonManager(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
                if (userValidation.PersonId != 0)
                {
                    person = await personManager.RetrieveAsync(userValidation.PersonId);
                }
                else
                {
                    person.IsValid = false;
                }

                if (person.IsValid)
                {
                    var userModel = new UserModel();
                    userModel.UserName = userValidation.UserName;
                    userModel.PersonId = person.PersonId;
                    userModel.FirstName = person.FirstName;
                    userModel.LastName = person.LastName;
                    userModel.PhoneNumber = person.PhoneNumber;
                    userModel.EMail = person.EMail;
                    userModel.IsPlayer = person.IsPlayer;
                    userModel.IsCoach = person.IsCoach;
                    userModel.IsValid = person.IsValid;
                    userModel.TeamId = person.TeamId;
                    Session["userModel"] = userModel;

                    if (person.IsPlayer)
                    {
                        return RedirectToAction("PlayerIndex", "Player");
                    }
                    else if (person.IsCoach)
                    {
                        return RedirectToAction("CoachIndex", "Coach");
                    }
                }
                else
                {
                    ViewBag.Warning = "That was not a valid login.  Try again.";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Warning = "That was not a valid login.  Try again.";
                return View();
            }

            return View();
        }

        #endregion

        #region RegisterUser
        public ActionResult RegisterUser()
        {
            ViewBag.Warning = "";

            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisterUserModel user)
        {
            try
            {
                var security = new SqlSecurity(ConfigurationManager.ConnectionStrings["WhosOnFirstDb"].ConnectionString);
                var userValidation = new UserValidation();
                var person = new Person();
                userValidation.UserName = user.UserName;
                userValidation.Password = user.Password;
                if (user.UserRole == "Coach")
                {
                    user.IsPlayer = false;
                    user.IsCoach = true;
                }
                else if (user.UserRole == "Player")
                {
                    user.IsPlayer = true;
                    user.IsCoach = false;
                }
                else
                {
                    user.IsPlayer = true;
                    user.IsCoach = false;
                }
                person.IsPlayer = user.IsPlayer;
                person.IsCoach = user.IsCoach;
                person.IsValid = true;
                person.IsAdmin = false;
                person.FirstName = user.FirstName;
                person.LastName = user.LastName;
                person.EMail = user.EMail;
                person.PhoneNumber = user.PhoneNumber;
                //TempData["RegistrationInfo"] = user;
                security.RegisterUserAsync(userValidation, person);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Warning = "Invalid or Blank registration!  Try again.";
                return View();
            }
        }
        #endregion

        #region EditUserProfile
        public ActionResult EditUserProfile()
        {
            return View();
        }
        #endregion




    }
}