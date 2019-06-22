using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Act.Models;
namespace Act.Controllers {
    public class ActivityController : Controller {
        private Context dbContext;
        public ActivityController(Context context) {
            dbContext = context;
        }
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            List<Activity1> AllActivities = dbContext.Activities.Where((a => a.ActivityDate.Date >= DateTime.Now.Date))
                                        .Include(Activity => Activity.Participants)
                                            .ThenInclude(guest => guest.User).OrderBy(a => a.ActivityDate)
                                        .ToList();
            List<Rsvp> UserRsvps = dbContext.Rsvps.Where(rsvp => rsvp.User.Equals(CurrentUser)).ToList();
            
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.AllActivities = AllActivities;
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.UserRsvps = UserRsvps;
            ViewBag.Date = DateTime.Now.Date;
            ViewBag.Time = DateTime.Now.TimeOfDay;

           

            
            return View();
        }

        [HttpGet]
        [Route("NewActivity")]
        public IActionResult NewActivity() {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            string Planner = CurrentUser.FirstName;
            ViewBag.Planner = Planner;
            return View();
        }

        [HttpPost]
        [Route("AddActivity")]
        public IActionResult AddActivity(Activity1 act) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            if((act.ActivityDate < DateTime.Now && act.Time < DateTime.Now) || (act.ActivityDate <= DateTime.Now.Date )) {
                ModelState.AddModelError("ActivityDate", "Activity must be in the future");
            }
            if(ModelState.IsValid) {
                dbContext.Add(act);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else {
                ViewBag.errors = ModelState.Values;
                return View("NewActivity");
            }
        }

        [HttpGet]
        [Route("Activity/{ActivityId}")]
        public IActionResult Activity(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            Activity1 CurrentActivity = dbContext.Activities
                                        .Include(Activity => Activity.Participants)
                                            .ThenInclude(guest => guest.User)
                                        .SingleOrDefault(Activity => Activity.ActivityId == ActivityId);
            List<Rsvp> Participants = dbContext.Rsvps.Where(rsvp => rsvp.Activity.Equals(CurrentActivity)).ToList();    
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            List<Activity1> AllActivities = dbContext.Activities
                                        .Include(Activity => Activity.Participants)
                                            .ThenInclude(guest => guest.User)
                                        .ToList();
            List<Rsvp> UserRsvps = dbContext.Rsvps.Where(rsvp => rsvp.User.Equals(CurrentUser)).ToList();
            
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.AllActivities = AllActivities;
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.UserRsvps = UserRsvps;
            
            ViewBag.Participants = Participants;
            ViewBag.CurrentActivity = CurrentActivity;
            string Planner = CurrentUser.FirstName;
            ViewBag.Planner = Planner;
            return View("Activity");
        }

        [HttpGet]
        [Route("RSVP/{ActivityId}")]
        public IActionResult RSVP(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            Activity1 CurrentActivity = dbContext.Activities
                                            .Include(Activity => Activity.Participants)
                                                .ThenInclude(guest => guest.User)
                                            .SingleOrDefault(Activity => Activity.ActivityId == ActivityId);
            Rsvp NewRsvp = new Rsvp {
                UserId = CurrentUser.UserId,
                User = CurrentUser,
                ActivityId = CurrentActivity.ActivityId,
                Activity = CurrentActivity
            };
            
            CurrentActivity.Participants.Add(NewRsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Decline/{ActivityId}")]
        public IActionResult Decline(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            Rsvp CurrentRsvp = dbContext.Rsvps.SingleOrDefault(rsvp => rsvp.UserId == HttpContext.Session.GetInt32("UserId") && rsvp.ActivityId == ActivityId);
            Activity1 CurrentActivity = dbContext.Activities
                                            .Include(Activity => Activity.Participants)
                                                .ThenInclude(guest => guest.User)
                                            .SingleOrDefault(Activity => Activity.ActivityId == ActivityId);
            CurrentActivity.Participants.Remove(CurrentRsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            Activity1 CurrentActivity = dbContext.Activities
                                            .SingleOrDefault(Activity => Activity.ActivityId == ActivityId);
            dbContext.Activities.Remove(CurrentActivity);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}