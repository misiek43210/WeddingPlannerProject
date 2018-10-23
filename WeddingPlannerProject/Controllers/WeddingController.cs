using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingPlannerProject.Models;
using WeddingPlannerProject.Helpers;
namespace WeddingPlannerProject.Controllers
{
    public class WeddingController : Controller
    {
        //Wyświetlanie strony rezerwacji terminu
        public ActionResult BookDate()
        {
            var LoggedUserId = User.Identity.GetUserId();
            using (var db = new OtherDbContext()) {
                var isUserHasWedding = db.Weddings.FirstOrDefault(x => x.UserId == LoggedUserId); ;
           
                if(isUserHasWedding != null){
                    return View("HasUserAlreadyWedding");
                }

                else {
                    WeddingOfferViewModel Offers = new WeddingOfferViewModel { Offer = db.Offers.ToList() };
                    return View(Offers);
                }               
            }             
        }

        //Wysyłanie danych odnośnie rezerwacji terminu
        [HttpPost]
        public ActionResult BookDate(WeddingOfferViewModel WeddingOfferViewModel)
        {
            var appdb = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = appdb.Users.Where(x => x.Id == currentUserId).FirstOrDefault();
            

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
                string message = "Uzytkownik " + currentUser.FirstName + " " + currentUser.LastName + " zarezerwowal termin " + NewWedding.Wedding.Date + " . Skontaktuj sie z nim!";
                string subject = "Zarezerwowano nowe wesele!";
                EmailHelper.SendEmail(message, subject);
                }
                return RedirectToAction("Index","Home");
        }

        //Zmiana danych o weselu
        public ActionResult ChangeWedding()
        {
            using (var db = new OtherDbContext())
            {
                WeddingOfferViewModel Offers = new WeddingOfferViewModel { Offer = db.Offers.ToList() };
                return View(Offers);
            }
        }

        [HttpPost]
        public ActionResult ChangeWedding(WeddingOfferViewModel WeddingOfferViewModel)
        {
            using (var db = new OtherDbContext())
            {
                var currentUserId = User.Identity.GetUserId();
                var WeddingToChange = new WeddingOfferViewModel
                {
                    Offer = new List<OfferViewModel>(),
                    Wedding = new WeddingViewModels()                    
                };


                //Pobranie wesela i przypisanie nowych wartości   
                WeddingToChange.Wedding = db.Weddings.Where(x => x.UserId == currentUserId).FirstOrDefault();
                WeddingToChange.Wedding.LocationOfWedding = WeddingOfferViewModel.Wedding.LocationOfWedding;
                WeddingToChange.Wedding.NumberOfGuests = WeddingOfferViewModel.Wedding.NumberOfGuests;

                var WeddingOffers = db.Wedding2Offers.Where(x => x.Wedding_Id == WeddingToChange.Wedding.Id).ToList();
                foreach (var item in WeddingOffers)
                {
                    db.Wedding2Offers.Remove(item);
                }

                //Przypisanie ID oferty do ID wesela i dodanie do kontekstu DB
                var selectedOffer = WeddingOfferViewModel.Offer.Where(x => x.IsChecked == true).ToList();
                foreach (var item in selectedOffer)
                {
                    var Wedding2Offer = new Wedding2OfferViewModel
                    {
                        Wedding_Id = WeddingToChange.Wedding.Id,
                        Offer_Id = item.Id
                    };
                    db.Wedding2Offers.Add(Wedding2Offer);
                }

                //Dodanie danych o weselu i zapisanie zmian w bazie danych. 
                db.SaveChanges();

                ViewBag.Message = "Pomyslnie zmieniono dane!";

                return RedirectToAction("Home","Index");
            }
        }
    }
}