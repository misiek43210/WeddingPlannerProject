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
    }
}