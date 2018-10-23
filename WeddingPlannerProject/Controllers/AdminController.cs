using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WeddingPlannerProject.Models;
namespace WeddingPlannerProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminPanel()
        {
            return View();
        }

        public ActionResult UsersList(string searching)
        {
            using (var db = new ApplicationDbContext())
            {
                var UsersList = new UsersViewModel
                {
                    Users = db.Users.Include(u => u.Roles).Where(x => x.UserName.Contains(searching) || searching == null).ToList(),
                    Roles = db.Roles.Include(r => r.Users).ToList()
                };
                return View("ChangeUser", UsersList);
            }
        }

        public ActionResult SearchUser(string searching)
        {
            using (var db = new ApplicationDbContext())
            {
                var UserListSearch = new UsersViewModel
                {
                    Roles = db.Roles.Include(r => r.Users).ToList()
                };
                return View(UserListSearch);
            }
        }

        [HttpPost]
        public ActionResult RemoveUser(string userNameToDelete)
        {
            using (var db = new ApplicationDbContext())
            {
                var userToDelete = db.Users.FirstOrDefault(x => x.Id == userNameToDelete);
                db.Users.Attach(userToDelete);
                db.Users.Remove(userToDelete);
                db.SaveChanges();

                return RedirectToAction("UsersList", "Admin");
            }
        }

        public ActionResult EditUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditUser(string userNameToEdit, EditUserViewModel EditUserViewModel)
        {
            using (var db = new ApplicationDbContext())
            {
                List<IdentityRole> ir = db.Roles.Include(r => r.Users).ToList();

                var userToEdit = db.Users.FirstOrDefault(x => x.Id == userNameToEdit);

                var userStore = new UserStore<IdentityUser>();
                var manager = new UserManager<IdentityUser>(userStore);
                var currentRole = manager.GetRoles(userToEdit.Id);
                manager.RemoveFromRoles(userToEdit.Id, currentRole[0]);
                manager.AddToRole(userToEdit.Id, EditUserViewModel.ApplicationRoleId);

                userToEdit.Email = EditUserViewModel.Email;
                userToEdit.FirstName = EditUserViewModel.Name;
                db.Users.Add(userToEdit);
                db.Entry(userToEdit).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("UsersList", "Admin");
            }

        }
    
    }
}