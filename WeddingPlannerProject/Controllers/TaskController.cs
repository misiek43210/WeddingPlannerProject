using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingPlannerProject.Models;

namespace WeddingPlannerProject.Controllers
{
    public class TaskController : Controller
    {
        public ActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTask(TaskModel task)
        {
            try
            {
                using (var db = new OtherDbContext())
                {
                    var NewTask = new TaskModel { Description = task.Description, Status = task.Status, UserId = User.Identity.GetUserId() };
                    db.UserTasks.Add(NewTask);
                    db.SaveChanges();
                }
                return RedirectToAction("index", "Home");
            }

            catch (DbEntityValidationException dbEx)
            {
                throw dbEx;
            }

        }
    }
}