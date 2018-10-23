using ArtPlanning.Common;
using ArtPlanning.Models;
using ArtPlanning.Models.Database;
using ArtPlanning.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtPlanning.Controllers
{
    [Authorize]
    public class AS_UsersController : Controller
    {
        private ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase();
        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;

        private readonly string KEY_ACTION = "action";
        private readonly string KEY_CONTROLLER = "controller";

        #region manager

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        #endregion        

        #region ctor

        public AS_UsersController()
        {
        }

        public AS_UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #endregion        


        //
        // GET: Users
        //
        public ActionResult Index(string sortOrder, int page = 0, int pageSize = 0, string forceFirstPage = "")
        {
            string action = ControllerContext.RouteData.Values[KEY_ACTION].ToString();
            string controller = ControllerContext.RouteData.Values[KEY_CONTROLLER].ToString();
            string searchLastName, searchFirstName, searchMail, searchStatus;

            //page = page > 0 ? page : 1;
            //pageSize = pageSize > 0 ? pageSize : 10;
            
            var netUsers = (from nu in database.AspNetUsers select nu);
            List<UserViewModel> usersCustom = new List<UserViewModel>();

            foreach (AspNetUsers netUser in netUsers)
            {
                UserViewModel user = new UserViewModel
                {
                    ID = netUser.Id,
                    Mail = netUser.Email,
                    LastName = netUser.last_name,
                    FirstName = netUser.first_name,
                    Active = netUser.active
                };

                usersCustom.Add(user);
            }

            var users = usersCustom.AsQueryable();

            // SEARCH

            // SORT

            sortOrder = string.IsNullOrEmpty(sortOrder) ? ARTSHIPPERS.Search.getSearchSortOrder(controller, action, "last_name") : ARTSHIPPERS.Search.setSearchSortOrder(controller, action, sortOrder);

            ViewBag.LastNameSortParam = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            ViewBag.FirstNameSortParam = sortOrder == "first_name" ? "first_name_desc" : "first_name";
            ViewBag.MailSortParam = sortOrder == "mail" ? "mail_desc" : "mail";
            ViewBag.CurrentSort = sortOrder;

            switch (sortOrder.ToLower())
            {
                case "last_name":
                    users = users.OrderBy(u => u.LastName);
                    break;
                case "last_name_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "first_name":
                    users = users.OrderBy(u => u.FirstName);
                    break;
                case "first_name_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "mail":
                    users = users.OrderBy(u => u.Mail);
                    break;
                case "mail_desc":
                    users = users.OrderByDescending(u => u.Mail);
                    break;
                default:
                    users = users.OrderBy(u => u.LastName);
                    break;
            }
            
            var currentPageSize = (pageSize == 0) ? ARTSHIPPERS.Paging.getSearchPageSize(controller, action) : ARTSHIPPERS.Paging.setSearchPageSize(controller, action, pageSize);
            ViewBag.PageSize = currentPageSize;

            var currentPage = (page == 0) ? ARTSHIPPERS.Paging.getSearchCurrentPage(controller, action) : ARTSHIPPERS.Paging.setSearchCurrentPage(controller, action, page);
            ViewBag.Page = currentPage;

            ViewBag.NumberItems = users.Count();

            string currentForceFirstPage = forceFirstPage;
            ViewBag.ForceFirstPage = "" + ViewBag.PageSize;
            if (!String.IsNullOrEmpty(currentForceFirstPage))
            {
                if (currentForceFirstPage != ViewBag.ForceFirstPage)
                {
                    currentPage = ARTSHIPPERS.Paging.setSearchCurrentPage(controller, action, 1);
                    ViewBag.Page = 1;
                }
            }
            return View(users.ToPagedList(currentPage, currentPageSize));
        }

        //
        // GET : Users/Edit/id
        //
        // Ajout ou mise à jout d'un utilisateur
        //
        [HttpGet]
        public ActionResult Edit(string id)
        {
            UserViewModel model = new UserViewModel();

            if (string.IsNullOrEmpty(id))
            {
                // ADD

                // ASSIGN MODEL

                model.Active = true;

                // TITLE - MESSAGE - BREADCRUMB

                ViewBag.BreadCrumb = "<li>Gestion des utilisateurs</li><li class=\"active\"><strong>Ajout</strong></li>";
                ViewBag.SubTitle = "Ajouter un utilisateur";
                ViewBag.Message = "Compléter le formulaire suivant pour ajouter un nouvel utilisateur.";
            }
            else
            {
                // UPDATE

                AspNetUsers netUser = database.AspNetUsers.Find(id);

                if (netUser == null) {
                    // TODO : Retourner un message d'erreur pour signaler que l'utilisateur n'existe pas
                    return RedirectToAction("Index");
                }
                
                // ASSIGN MODEL

                model.ID = netUser.Id;
                model.LastName = netUser.last_name;
                model.FirstName = netUser.first_name;
                model.Mail = netUser.Email;
                model.Active = (netUser.active ?? true);
                model.Language = netUser.language_id;
                model.Color = netUser.color;
                model.Initials = netUser.initials;
                model.LastConnectionDate = (netUser.last_connection_date.HasValue ? netUser.last_connection_date.Value.ToString("dd/MM/yyyy à HH:mm") : "");
                model.BirthDate = (netUser.birth_date.HasValue ? netUser.birth_date.Value.ToString("dd/MM/yyyy") : "");
                model.AddedLabel = ARTSHIPPERS.User.GetUpdateDateDescription(netUser.added_date, netUser.added_user);
                model.ModificationLabel = ARTSHIPPERS.User.GetUpdateDateDescription(netUser.modification_date, netUser.modification_user);

                // TITLE - MESSAGE - BREADCRUMB

                ViewBag.BreadCrumb = "<li>Gestion des utilisateurs</li><li class=\"active\"><strong>Mise à jour</strong></li>";
                ViewBag.SubTitle = "Mettre à jour un utilisateur";
                ViewBag.Message = "Compléter le formulaire suivant pour mettre à jour un utilisateur.";
            }

            ViewBag.StatusList = ARTSHIPPERS.Helpers.GetStatus();
            ViewBag.LanguagesList = ARTSHIPPERS.Helpers.GetLanguages();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel user, bool backToList)
        {
            if (ModelState.IsValid)
            {
                bool backToEdit = false;
                string guid = null;
                DateTime parsedBirthDate;

                if (user == null || string.IsNullOrEmpty(user.ID))
                {
                    // ADD
                    
                    ApplicationUser newUser = new ApplicationUser
                    {
                        Email = user.Mail,
                        UserName = user.Mail,
                        LastName = user.LastName,
                        FirstName = user.FirstName,
                        Language = user.Language,
                        Color = user.Color,
                        Initials = !string.IsNullOrEmpty(user.Initials) ? user.Initials.ToUpper() : "",
                        AddedDate = DateTime.Now,
                        AddedUser = User.Identity.GetUserId(),
                        BirthDate = DateTime.TryParse(user.BirthDate, out parsedBirthDate) ? parsedBirthDate : (DateTime?)null,
                        Active = user.Active
                    };

                    var result = UserManager.Create(newUser, user.Password);

                    if (result.Succeeded)
                    {
                        guid = user.ID;
                    }
                    else
                    {
                        backToEdit = true;
                    }
                }
                else
                {
                    // UPDATE

                    guid = user.ID;
                    AspNetUsers netUser = database.AspNetUsers.Find(user.ID);

                    if (netUser == null)
                    {
                        backToEdit = true;
                    }

                    if (!backToEdit)
                    {
                        netUser.Email = user.Mail;
                        netUser.last_name = user.LastName;
                        netUser.first_name = user.FirstName;
                        netUser.language_id = user.Language;
                        netUser.active = user.Active;
                        netUser.color = user.Color;
                        netUser.initials = !string.IsNullOrEmpty(user.Initials) ? user.Initials.ToUpper() : "";
                        netUser.birth_date = DateTime.TryParse(user.BirthDate, out parsedBirthDate) ? parsedBirthDate : (DateTime?)null;
                        netUser.modification_date = DateTime.Now;
                        netUser.modification_user = User.Identity.GetUserId();

                        database.SaveChanges();

                        if (!string.IsNullOrEmpty(user.Password))
                        {
                            var removePasswordResult = UserManager.RemovePassword(user.ID);
                            if (removePasswordResult.Succeeded)
                            {
                                var addPasswordResult = UserManager.AddPassword(user.ID, user.Password);
                                if (addPasswordResult.Succeeded)
                                {
                                    backToEdit = true;
                                }
                            }
                            else
                            {
                                backToEdit = true;
                            }
                        }
                    }
                }



                if (backToEdit || !backToList)
                {
                    if (backToEdit)
                    {
                        AspNetUsers item = database.AspNetUsers.Find(user.ID);

                        if (item != null)
                        {
                            user.AddedLabel = ARTSHIPPERS.User.GetUpdateDateDescription(item.added_date, item.added_user);
                            user.ModificationLabel= ARTSHIPPERS.User.GetUpdateDateDescription(item.modification_date, item.modification_user);
                            user.LastConnectionDate = "";

                            if (item.last_connection_date.HasValue)
                            {
                                user.LastConnectionDate = (item.last_connection_date.HasValue ? item.last_connection_date.Value.ToString("dd/MM/yyyy à HH:mm") : "");
                            }
                        }
                        else
                        {
                            user.AddedLabel = "";
                            user.ModificationLabel = "";
                            user.LastConnectionDate = "";
                        }

                        ViewBag.StatusList = ARTSHIPPERS.Helpers.GetStatus();
                        ViewBag.LanguagesList = ARTSHIPPERS.Helpers.GetLanguages();
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { ID = guid });
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View(user);
        }

        //
        // POST : Users/ActiveUser/id
        //
        // Active ou non un utilisateur via AJAX 
        //
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SwitchUserActiveStatus(string id)
        {
            int records = 0;
            AspNetUsers selectedUser = database.AspNetUsers.Find(id);
            selectedUser.modification_date = DateTime.Now;
            selectedUser.modification_user = User.Identity.GetUserId();

            if (selectedUser.active.HasValue) {
                selectedUser.active = (selectedUser.active.Value == true ? false : true);
            }
            else {
                selectedUser.active = false;
            }
            records = database.SaveChanges();

            return Json(new { response = (records > 0 ? true : false), JsonRequestBehavior.AllowGet });
        }        
    }
}