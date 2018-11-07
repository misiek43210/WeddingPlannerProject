using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeddingPlannerProject.Models
{
    [Table("Weddings")]
    public class WeddingViewModels
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Prosze uzupelnic pole o poprawna date!")]
        [Display(Name = "Data wesela")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage ="Prosze uzupelnic pole o szacowana liczbe gosci!")]
        [Display(Name = "Liczba Gości")]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Prosze uzupelnic pole o planowana lokalizacje wesela!")]
        [Display(Name = "Lokalizacja wesela")]
        public string LocationOfWedding { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Potwierdzone")]
        public bool IsConfirmed { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Wedding2OfferViewModel> Wedding_To_Offer { get; set; }
    }

    [Table("Wedding2Offers")]
    public class Wedding2OfferViewModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("WeddingId")]
        public int Wedding_Id { get; set; }

        [ForeignKey("OfferId")]
        public int Offer_Id { get; set; }

        public virtual OfferViewModel OfferId { get; set; }
        public virtual WeddingViewModels WeddingId { get; set; }
    }


    [Table("Offers")]
    public class OfferViewModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name="Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Opis oferty")]
        public string Description { get; set; }

        [Required]
        [Display(Name="Kwota")]
        public string Salary { get; set; }

        public virtual ICollection<Wedding2OfferViewModel> Wedding_2_Offer { get; set; }

        public bool IsChecked { get; set; }
    }

    public class WeddingOfferViewModel
    {
        public WeddingViewModels Wedding { get; set; }

        [Required(ErrorMessage = "Prosze wybrac oferte!")]
        public List<OfferViewModel> Offer { get; set; }
    }

    public class WeddingToUserViewModel
    {
        public List<WeddingViewModels> Wedding { get; set; }
        public List<ApplicationUser> User { get; set; }
    } 
}