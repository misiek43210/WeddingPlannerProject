using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WeddingPlannerProject.Models
{
    [Table("Tasks")]
    public class TaskModel
    {       
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; }

       
        [ForeignKey("User")]
        public string UserId { get; set; }
    
        public  ApplicationUser User { get; set; }
    }
}