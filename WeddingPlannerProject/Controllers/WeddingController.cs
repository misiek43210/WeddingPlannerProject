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
        public ActionResult BookDate(WeddingOfferViewModel WeddingOfferViewModel)
        {
            try
            { 
                using (var db = new OtherDbContext())
                {
                    var NewWedding = new WeddingOfferViewModel
                    {
                        Wedding = new WeddingViewModels
                        {
                            UserId = User.Identity.GetUserId(),
                            Date = WeddingOfferViewModel.Wedding.Date,
                            NumberOfGuests = WeddingOfferViewModel.Wedding.NumberOfGuests,
                            LocationOfWedding = WeddingOfferViewModel.Wedding.LocationOfWedding
                        }
                    };


                    //Przypisanie ID oferty do ID wesela i dodanie do kontekstu DB
                    var selectedOffer = WeddingOfferViewModel.Offer.Where(x => x.IsChecked == true).ToList();
                    foreach (var item in selectedOffer)
                    {
                        var Wedding2Offer = new Wedding2OfferViewModel
                        {
                            Wedding_Id = NewWedding.Wedding.Id,
                            Offer_Id = item.Id
                        };
                        db.Wedding2Offers.Add(Wedding2Offer);
                    }

                    //Dodanie danych o weselu i zapisanie zmian w bazie danych. 
                    db.Weddings.Add(NewWedding.Wedding);
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