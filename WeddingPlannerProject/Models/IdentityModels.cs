using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WeddingPlannerProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Dodanie kolekcji tasków do użytkownika
        public virtual ICollection<TaskModel> UserTasks { get; set; }
        //jedno wesele do jednego użytkownika
        public virtual ICollection<WeddingViewModels> WeddingModel { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class OtherDbContext : IdentityDbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<WeddingViewModels> Weddings { get; set; }
        public DbSet<OfferViewModel> Offers { get; set; }
        public DbSet<Wedding2OfferViewModel> Wedding2Offers { get; set; }

        public OtherDbContext() : base("DefaultConnection")
        { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}