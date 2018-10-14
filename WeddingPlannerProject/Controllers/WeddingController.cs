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
            using (var db = new OtherDbContext()) {
                WeddingOfferViewModel Offers = new WeddingOfferViewModel { Offer = db.Offers.ToList() };
                return View(Offers);
            }             
        }

        //Wysyłanie danych odnośnie rezerwacji terminu
        [HttpPost]
        public ActionResult BookDate(WeddingOfferViewModel WeddingOfferViewModel, Wedding2OfferViewModel Wedding2Offer)
        {
            try
            { 
                using (var db = new OtherDbContext())
                {
                    var NewWedding = new WeddingOfferViewModel
                    {
                        Wedding = new WeddingViewModels { UserId = User.Identity.GetUserId(),
                            Date = WeddingOfferViewModel.Wedding.Date,
                            NumberOfGuests = WeddingOfferViewModel.Wedding.NumberOfGuests,
                            LocationOfWedding = WeddingOfferViewModel.Wedding.LocationOfWedding }
                    };
                              
                    var NewWedding2Offer = new Wedding2OfferViewModel { Offer_Id = 1, Wedding_Id = NewWedding.Wedding.Id };
                        db.Weddings.Add(NewWedding.Wedding);
                        db.Wedding2Offers.Add(NewWedding2Offer);
                        db.SaveChanges();                                      
                }
                return RedirectToAction("Index","Home");
            }

            catch (DbEntityValidationException dbEx)
            {
                throw dbEx;
            }
        }
    }
}