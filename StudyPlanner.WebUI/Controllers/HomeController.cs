using System;
using System.Linq;
using System.Web.Mvc;
using StudyPlanner.Domain.Abstract;
using StudyPlanner.Domain.Entities;
using StudyPlanner.Infrastructure.Abstract;
using static StudyPlanner.WebUI.Infrastructure.Utilities;

namespace StudyPlanner.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        private IAuthProvider authProvider;

        public HomeController(IRepository repository, IAuthProvider authProvider)
        {
            this.repository = repository;
            this.authProvider = authProvider;
        }

        public ActionResult Index(string returnUrl, User user, bool register = false, bool recover = false)
        {
            ViewBag.Register = register;
            ViewBag.Recover = recover;
            ViewBag.ReturnUrl = returnUrl;
            return View(user);
        }

        // Should be Ajax
        [HttpPost]
        public RedirectResult LogIn( string returnUrl, User user)
        {
            if (!String.IsNullOrEmpty(user.Email) && !String.IsNullOrEmpty(user.Password) && authProvider.Authenticate(user.Email, user.Password))
                return Redirect(string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Home") : returnUrl);
            else
            {
                TempData["ErrorMessage"] = "Incorrect e-mail or password";
                return Redirect(Url.Action("Index", "Home", new { returnUrl = returnUrl, email = user.Email }));
            }
        }

        // Should be Ajax
        [HttpPost]
        public RedirectResult Register(string returnUrl, User user)
        {
            if (user.Name != null && user.Email != null && user.Password != null)
            {
                if (repository.Users.FirstOrDefault(u => u.Email.ToLower() == user.Email.ToLower()) == null)
                {
                    user.Salt = Guid.NewGuid().ToString();
                    user.Password = HashPassword(user.Password, user.Salt);
                    repository.AddUser(user);
                    TempData["SuccessMessage"] = "New account registered successfully";
                    return Redirect(Url.Action("Index", "Home", new { returnUrl = returnUrl }));
                }
                else
                {
                    TempData["ErrorMessage"] = "Choose different e-mail address";
                    return Redirect(Url.Action("Index", "Home", new { returnUrl = returnUrl, register = true, name = user.Name, email = user.Email }));
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Incorrect data";
                return Redirect(Url.Action("Index", "Home", new { returnUrl = returnUrl, register = true, name = user.Name, email = user.Email }));
            }
        }

        public RedirectToRouteResult LogOut()
        {
            authProvider.SignOut();
            return RedirectToAction("Index");
        }
    }
}