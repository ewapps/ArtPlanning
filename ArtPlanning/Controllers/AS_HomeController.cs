using ArtPlanning.Models;
using ArtPlanning.Models.Database;
using ArtPlanning.Models.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ArtPlanning.Controllers
{
    [Authorize]
    public class AS_HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UnclosedFoldersCount = new FolderRepository().GetUnclosedFoldersCount();
            ViewBag.UnsignedTasksCount = new TaskRepository().GetUnsignedTasksCount();
            ViewBag.UserFirstName = GetFirstName();
            ViewBag.Birthdays = GetBithdays(); ;

            return View();
        }

        private string GetFirstName()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return currentUser.FirstName;
        }

        private string GetBithdays()
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var birthdays = from u in database.AspNetUsers where u.active.HasValue && u.active == true && (DbFunctions.TruncateTime(u.birth_date) == DbFunctions.TruncateTime(DateTime.Now)) select new { u.last_name, u.first_name };
                string bdays = "--";
                if (birthdays.Any())
                {
                    bdays = "";
                    foreach (var item in birthdays)
                    {
                        bdays += string.Format("{0} {1}<br />", item.first_name, item.last_name);
                    }
                }

                return bdays;
            }
        }
    }
}