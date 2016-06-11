using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyPlanner.Models;

namespace StudyPlanner.Controllers
{
    public class HomeController : Controller
    {
        private StudyPlannerDB db = new StudyPlannerDB();

        // GET: Home
        public ActionResult Index()
        {
            IEnumerable<Author> authors = from x in db.Authors select x;
            return View(authors);
        }
    }
}