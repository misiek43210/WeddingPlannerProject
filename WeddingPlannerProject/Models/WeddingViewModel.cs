using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeddingPlannerProject.Models
{
    [Table("Weddings")]
    public class WeddingViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        [Required]
        public string LocationOfWedding { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

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
        public virtual WeddingViewModel WeddingId { get; set; }
    }


    [Table("Offers")]
    public class OfferViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Salary { get; set; }

        public virtual ICollection<Wedding2OfferViewModel> Wedding_2_Offer { get; set; }

    }
}