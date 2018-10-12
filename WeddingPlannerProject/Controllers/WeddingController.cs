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
    public class WeddingController : Controller
    {
        //Wyświetlanie strony rezerwacji terminu
        public ActionResult BookDate()
        {
            return View();
        }

        //Wysyłanie danych odnośnie rezerwacji terminu
        [HttpPost]
        public ActionResult BookDate(WeddingViewModel weddingModel, OfferViewModel offerModel, Wedding2OfferViewModel Wedding2Offer)
        {
            try
            { 
                using (var db = new OtherDbContext())
                {
                    var NewWedding = new WeddingViewModel { UserId = User.Identity.GetUserId(), Date=weddingModel.Date, NumberOfGuests = weddingModel.NumberOfGuests, LocationOfWedding = weddingModel.LocationOfWedding };
                    //TODO: Dodać oferty do bazy danych, następnie przekazać je do Wedding2Offer, nastepnie zapisać do bazy danych!
                        db.Weddings.Add(NewWedding);
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