using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Act.Models {
    public class Rsvp {
        public int RsvpId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ActivityId { get; set; }
        public Activity1 Activity { get; set; }
    }
}