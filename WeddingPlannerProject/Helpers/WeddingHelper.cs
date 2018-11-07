using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeddingPlannerProject.Models;

namespace WeddingPlannerProject.Helpers
{
    public static class WeddingHelper
    {
        public static bool HasUserWedding(ApplicationUser user)
        {
            using (var db = new OtherDbContext())
            {
                var Wedding = db.Weddings.Where(x => x.UserId == user.Id).FirstOrDefault();

                if(Wedding != null)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
        }

        public static bool IsConfirmedWeddingExist(DateTime date)
        {
            using (var db = new OtherDbContext())
            {
                var Wedding = db.Weddings.Where(x => x.IsConfirmed == true).ToList();

                var BookedWedding = false;
                foreach(var weddings in Wedding)
                {
                    if(weddings.Date == date)
                    {
                        return BookedWedding == true;
                    }
                }
                return BookedWedding;
            }
        }
    }
}