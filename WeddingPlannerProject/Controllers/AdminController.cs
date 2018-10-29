using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Vereyon.Web;
using WeddingPlannerProject.Models;
namespace WeddingPlannerProject.Controllers
{
    [Authorize(Roles="admin")]
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
        public ActionResult RemoveUser(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var userToDelete = db.Users.FirstOrDefault(x => x.Id == userId);
                db.Users.Attach(userToDelete);
                db.Users.Remove(userToDelete);
                db.SaveChanges();

                return RedirectToAction("UsersList", "Admin");
            }
        }

        public ActionResult EditUser(string userId)
        {
            using (var db = new ApplicationDbContext())
            {
                var User = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                EditUserViewModel userToEdit = new EditUserViewModel
                {
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email
                };
                return View(userToEdit);
            }
        }

        [HttpPost]
        public ActionResult EditUser(string UserId, EditUserViewModel EditUserViewModel)
        {
            using (var db = new ApplicationDbContext())
            {
                List<IdentityRole> ir = db.Roles.Include(r => r.Users).ToList();

                var userToEdit = db.Users.FirstOrDefault(x => x.Id == UserId);

                var userStore = new UserStore<IdentityUser>();
                var manager = new UserManager<IdentityUser>(userStore);
                var currentRole = manager.GetRoles(userToEdit.Id);
                manager.RemoveFromRoles(userToEdit.Id, currentRole[0]);
                manager.AddToRole(userToEdit.Id, EditUserViewModel.ApplicationRoleId);

                userToEdit.Email = EditUserViewModel.Email;
                userToEdit.FirstName = EditUserViewModel.FirstName;
                userToEdit.LastName = EditUserViewModel.LastName;
                db.Users.Add(userToEdit);
                db.Entry(userToEdit).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("UsersList", "Admin");
            }
        }    

        public ActionResult EditWedding(string userId)
        {
            using (var db = new OtherDbContext())
            {
                var UserContextWedding = db.Weddings.Where(x => x.UserId == userId).FirstOrDefault();
                return View(UserContextWedding);
            }
        }
        
        [HttpPost]
        public ActionResult ConfirmWedding(int WeddingId, WeddingViewModels WeddingViewModels, bool Confirmed)
        {
            using (var db = new OtherDbContext())
            {
                var UserContextWedding = db.Weddings.Where(x => x.Id == WeddingId).FirstOrDefault();
                UserContextWedding.IsConfirmed = Confirmed;

                db.Weddings.Add(UserContextWedding);
                db.Entry(UserContextWedding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UsersList");
            }
        }

        public ActionResult ConfirmedWeddings()
        {
            using(var db = new OtherDbContext())
            {
                var WeddingToUser = new WeddingToUserViewModel()
                {
                    Wedding = new List<WeddingViewModels>(),
                    User = new List<ApplicationUser>()
                };
               var AppDb = new ApplicationDbContext();

                WeddingToUser.Wedding = db.Weddings.Where(x => x.IsConfirmed == true).OrderBy
                    (p => p.Date).ToList();

                foreach (var weddings in WeddingToUser.Wedding)
                {
                    var UserWithConfirmedWedding = AppDb.Users.Where(x => x.Id == weddings.UserId).FirstOrDefault();
                    WeddingToUser.User.Add(UserWithConfirmedWedding);
                }
               return View(WeddingToUser);
            }
        }

        public ActionResult UserDetails(string UserId)
        {
            using (var db = new ApplicationDbContext())
            {
                var User = db.Users.Where(x => x.Id == UserId).FirstOrDefault();

                return View(User);
            }
        }

        public ActionResult OffersList()
        {
            using (var db = new OtherDbContext())
            {
                var OfferList = db.Offers.ToList();
                return View(OfferList);
            }
        }   
    
        public ActionResult RemoveOffer(int OfferId)
        {
            using (var db = new OtherDbContext())
            {
                var OfferToDelete = db.Offers.Where(x => x.Id == OfferId).FirstOrDefault();
                db.Offers.Remove(OfferToDelete);
                db.SaveChanges();
                Response.Write("<script>alert('Usunięto');</script>");
                return RedirectToAction("OffersList");
            }
        }

        public ActionResult AddOffer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddOffer(OfferViewModel Offer)
        {
            using (var db = new OtherDbContext())
            {
                var OfferToAdd = Offer;
                db.Offers.Add(OfferToAdd);
                db.SaveChanges();
                return RedirectToAction("OffersList");
            }
        }
    }
}