using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Act.Models {
    public class Activity1 {
        [Key]
        public int ActivityId { get; set; }


        [Required]
        [Display(Name = "Title:")]
        public string Title { get; set; }


        [Required]
        [Display(Name = "Time:")]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        
        [Required]
        [Display(Name = "Duration:")]
        public int Duration { get; set; }

        [Required]
        public string TimeLength {get;set;}
    
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Activity Date:")]
        public DateTime ActivityDate { get; set; }


        [Required]
        [Display(Name = "Description:")]
        public string Description { get; set; }

        public string Planner {get;set;}
     
  
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Rsvp> Participants { get; set; }
        public Activity1() {
            Participants = new List<Rsvp>();
        }
    }
}