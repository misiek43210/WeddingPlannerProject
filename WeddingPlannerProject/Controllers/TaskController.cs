using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingPlannerProject.Models;

namespace WeddingPlannerProject.Controllers
{
    [Authorize(Roles = "suser")]
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
                using (var db = new OtherDbContext())
                {
                    var NewTask = new TaskModel { Description = task.Description, Status = task.Status, UserId = User.Identity.GetUserId() };
                    db.Tasks.Add(NewTask);
                    db.SaveChanges();
                }
                return RedirectToAction("MyTasks");         
        }

        public ActionResult RemoveTask(int TaskId)
        {
            using (var db = new OtherDbContext())
            {
                var taskToDelete = db.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();
                db.Tasks.Remove(taskToDelete);
                db.SaveChanges();
            return RedirectToAction("MyTasks");
            }
        }

        public ActionResult EditTask(int TaskId)
        {
            using (var db = new OtherDbContext())
            {
                var currentTask = db.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();

                return View(currentTask);
            }               
        }

        [HttpPost]
        public ActionResult EditTask(int TaskId, TaskModel task)
        {
            using (var db = new OtherDbContext())
            {
                var EditTask = db.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();
                EditTask.Status = task.Status;
                EditTask.Description = task.Description;
                db.Tasks.Add(EditTask);
                db.Entry(EditTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyTasks");
            }


        }
    }
}