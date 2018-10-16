using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WeddingPlannerProject.Models;

namespace WeddingPlannerProject.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveUser()
        {
            using (var db = new ApplicationDbContext())
            {
                var UsersList = new UsersViewModel
                {
                    Users = db.Users.ToList()
                };
                return View("ChangeUser", UsersList);
            }
        }

        [HttpPost]
        public ActionResult RemoveUser(string userNameToDelete)
        {
            using (var db = new ApplicationDbContext())
            {

   ///             Membership.DeleteUser(userNameToDelete);
       //         Membership.FindUsersByName(userNameToDelete);

                var userToDelete = db.Users.FirstOrDefault(x => x.Id == userNameToDelete);
                db.Users.Attach(userToDelete);
                db.Users.Remove(userToDelete);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }
    }
}