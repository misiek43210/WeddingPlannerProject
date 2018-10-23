using System;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WeddingPlannerProject.Models;

namespace WeddingPlannerProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ApplicationDbContext userscontext = new ApplicationDbContext();
                var userStore = new UserStore<ApplicationUser>(userscontext);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var roleStore = new RoleStore<IdentityRole>(userscontext);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                if (userManager.IsInRole(User.Identity.GetUserId(), "Admin"))
                {
                    return RedirectToAction("AdminPanel", "Admin");
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}