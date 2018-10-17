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

        public ActionResult MyTasks()
        {
            using (var db = new OtherDbContext())
            {
                var currentUserId = User.Identity.GetUserId();
                var UserTasks = db.Tasks.Where(x => x.UserId == currentUserId).ToList();
                return View(UserTasks);
            }
        }

        // Metoda wywołująca widok AddTask, w którym można podać szczegółowe dane zadania
        public ActionResult AddTask()
        {
            return View();
        }

        //Metoda która wysyła dane uzupełnione przez użytkownika do bazy danych
        [HttpPost]
        public ActionResult AddTask(TaskModel task)
        {
            try
            {
                using (var db = new OtherDbContext())
                {
                    var NewTask = new TaskModel { Description = task.Description, Status = task.Status, UserId = User.Identity.GetUserId() };
                    db.Tasks.Add(NewTask);
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