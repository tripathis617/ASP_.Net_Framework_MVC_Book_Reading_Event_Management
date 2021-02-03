using BookReadingEventManagement.Data;
using BookReadingEventManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookReadingEventManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var listofEvents = context.BookReadingEvents.ToList();
            return View(listofEvents);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Events eventdata)
        {
            eventdata.Id = Guid.NewGuid().ToString();
            List<string> invitedlist= new List<string>();
            if (eventdata.TotalInviteByEmails != null)
            {
                if (!eventdata.TotalInviteByEmails.Contains(','))
                {
                    eventdata.TotalInviteByEmails = eventdata.TotalInviteByEmails + ",";
                }
                invitedlist = eventdata.TotalInviteByEmails.Split(',').ToList();
            }         
            List<ApplicationUser> authenticUsers = new List<ApplicationUser>();
            for (int i = 0; i < invitedlist.Count; i++)
            {
                var user = context.Users.Where(a => a.Email == invitedlist[i]).FirstOrDefault();
                if (user != null) 
                {
                    authenticUsers.Add((ApplicationUser)user);
                }
            }
            eventdata.TotalInviteByEmails = authenticUsers.Count().ToString();
            List<UsersEvents> items = new List<UsersEvents>();
            foreach (var item in authenticUsers)
            {
                items.Add(new UsersEvents
                {
                    Users = item,
                    Events = eventdata,
                });
            }
            eventdata.UsersEventses = items;
            context.BookReadingEvents.Add(eventdata);
            context.SaveChanges();
            return RedirectToAction("MyEvents");
        }

        public IActionResult MyEvents()
        {
            var useremail = User.Identity.Name;
            var listofEvents = context.BookReadingEvents.Where(a => a.UserEmail == useremail).OrderByDescending(a => a.DateTime).ToList();
            return View(listofEvents);
        }

        public IActionResult EventsInvitedto()
        {
            var useremail = User.Identity.Name;
            var userid = context.Users.Where(a => a.Email == useremail).FirstOrDefault().Id;
            var eventsinvitedto = context.ApplicationUsers.Where(a => a.Id == userid).Select(a => a.UsersEventses).ToList();
            List<Events> eventsinvitedtolist = new List<Events>();
            for (int i = 0; i < eventsinvitedto[0].Count(); i++)
            {
                eventsinvitedtolist.Add(context.BookReadingEvents.Where(a => a.Id == eventsinvitedto[0][i].EventId).FirstOrDefault());
            }
            return View(eventsinvitedtolist);
        }


        [HttpGet]
        public IActionResult Details(string Id)
        {
            Events eventfordetails = context.BookReadingEvents.Where(a => a.Id == Id).FirstOrDefault();
            var commentlist = context.Comments.Where(a => a.EventId == Id).OrderByDescending(a => a.CommentId).ToList();
            var tuple = new Tuple<Events, List<Comment>, Comment>(eventfordetails, commentlist, new Comment());
            return View(tuple);
        }


        [HttpGet]
        public IActionResult Delete(string Id)
        {
            Events eventfordetails = context.BookReadingEvents.Where(a => a.Id == Id).FirstOrDefault();
            return View(eventfordetails);
        }

        [HttpPost]
        public IActionResult Delete(Events eventfordelete)
        {
            context.BookReadingEvents.Remove(eventfordelete);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            Events eventforedit = context.BookReadingEvents.Where(a => a.Id == Id).FirstOrDefault();
            var eventsinvitedto = context.BookReadingEvents.Where(a => a.Id == Id).Select(a => a.UsersEventses).ToList();
            string emailslist = "";
            for (int i = 0; i < eventsinvitedto[0].Count(); i++)
            {
                emailslist += context.ApplicationUsers.Where(a => a.Id == eventsinvitedto[0][i].UserId).FirstOrDefault().Email + ",";
            }
            eventforedit.TotalInviteByEmails = emailslist;
            return View(eventforedit);
        }

        [HttpPost]
        public IActionResult Edit(Events eventforEdit)
        {
            //For Deleting 
            var eventsinvitedto = context.BookReadingEvents.Where(a => a.Id == eventforEdit.Id).Select(a => a.UsersEventses).ToList();
            foreach (var item in eventsinvitedto[0])
            {
                context.ApplicationUsers.Find(item.UserId).UsersEventses.Remove(item);
                context.SaveChanges();
            }
            List<string> invitedlist = new List<string>();
            if (eventforEdit.TotalInviteByEmails != null)
            {
                if (!eventforEdit.TotalInviteByEmails.Contains(','))
                {
                    eventforEdit.TotalInviteByEmails = eventforEdit.TotalInviteByEmails + ",";
                }
                invitedlist = eventforEdit.TotalInviteByEmails.Split(',').ToList();
            }
            List<ApplicationUser> authenticUsers = new List<ApplicationUser>();
            for (int i = 0; i < invitedlist.Count; i++)
            {
                var user = context.Users.Where(a => a.Email == invitedlist[i]).FirstOrDefault();
                if (user != null)
                {
                    authenticUsers.Add((ApplicationUser)user);
                }
            }
            eventforEdit.TotalInviteByEmails = authenticUsers.Count().ToString();
            List<UsersEvents> items = new List<UsersEvents>();
            foreach (var item in authenticUsers)
            {
                items.Add(new UsersEvents
                {
                    Users = item,
                    Events = eventforEdit,
                });
            }
            eventforEdit.UsersEventses = items;
            context.BookReadingEvents.Update(eventforEdit);
            context.SaveChanges();
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("MyEvents");
            }
        }

        //[HttpGet]
        //public IActionResult CreateComment()
        //{
        //    return View();
        //}
        int commentidGenerator()
        {
            var listofcommentids = context.Comments.ToList();
            if (listofcommentids.Count == 0)
            {
                return 1;
            }
            else
            {
                var lascommentid = listofcommentids.LastOrDefault().CommentId;
                return Convert.ToInt32(lascommentid) + 1;
            }
        }

        [HttpPost]
        public string CreateComment(Comment comment)
        {
            if (comment.CommentText == null || comment.CommentText == "")
            {
                return "Fail";
            }
            else
            {
                comment.CommentId = commentidGenerator();
                context.Comments.Add(comment);
                context.SaveChanges();
                return "Success";
            }
        }
    }
}
