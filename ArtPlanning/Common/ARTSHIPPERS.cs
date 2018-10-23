using ArtPlanning.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtPlanning.Common
{
    public class ARTSHIPPERS
    {
        public static class Helpers
        {
            /*
             *  Obtient la liste des langues disponibles.
             */
            public static List<SelectListItem> GetLanguages()
            {
                using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
                {
                    return (from l in database.language select new SelectListItem() { Value = l.id.ToString(), Text = l.name }).ToList<SelectListItem>();
                }
            }

            /*
             *  Obtient la lsite des statuts de l'utilisateur disponibles. 
             */
            public static List<SelectListItem> GetStatus(int languageID = 1)
            {
                List<SelectListItem> status = new List<SelectListItem>()
                {
                    new SelectListItem() { Value="true", Text="Activé" },
                    new SelectListItem() { Value="false", Text="Désactivé" }
                };
                return status;
            }

            public static List<SelectListItem> NoYesValues()
            {
                List<SelectListItem> values = new List<SelectListItem>() {
                    new SelectListItem { Value = "true", Text = "Oui" },
                    new SelectListItem { Value = "false", Text = "Non" }
                };
                return values;
            }
        }

        public static class User
        {
            // MODIFICATION

            /*
             *  Obtient une chaîne de caractères correspondant au nom et prénom de l'utilisateur.
             */
            public static string GetFullName(string id)
            {
                using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
                {
                    string fullName = "";
                    if (!string.IsNullOrEmpty(id))
                    {
                        var name = (from u in database.AspNetUsers where u.Id == id select u).AsEnumerable().Select(x => new { FullName = string.Format("{0} {1}", x.last_name, x.first_name) }).FirstOrDefault();
                        if (name != null && !string.IsNullOrEmpty(name.FullName))
                        {
                            fullName = name.FullName;
                        }
                    }
                    return fullName;
                }
            }

            /*
             *  Obtient une chaîne de caractères correspondant à la date et le nom de l'utilisateur.
             */
            public static string GetUpdateDateDescription(DateTime? date, string id, int language = 1)
            {
                using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
                {
                    string description = "";
                    if (date.HasValue && !string.IsNullOrEmpty(id))
                    {
                        var name = (from u in database.AspNetUsers where u.Id == id select u).AsEnumerable().Select(x => new { FullName = string.Format("{0} {1}", x.last_name, x.first_name) }).FirstOrDefault();
                        if (name != null && !string.IsNullOrEmpty(name.FullName))
                        {
                            description = string.Format("{0} par {1}", date.Value.ToString("dd/MM/yyyy à HH:mm"), name.FullName);
                        }
                    }

                    return description;
                }
            }
        }

        public static class Search
        {
            public static string setSearchSortOrder(string controller, string action, string sortOrder)
            {
                HttpContext.Current.Session[controller + "_#_" + action + "_@_" + System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantSortOrderSessionName"]] = sortOrder.ToLower();
                return sortOrder.ToLower();
            }

            public static string getSearchSortOrder(string controller, string action, string defaultSortOrder = "")
            {
                var getResult = (HttpContext.Current.Session[controller + "_#_" + action + "_@_" + System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantSortOrderSessionName"]] ?? defaultSortOrder);
                return getResult.ToString().ToLower();
            }
        }

        public static class Paging
        {
            public static int setSearchCurrentPage(string controller, string action, int pageNumber)
            {
                HttpContext.Current.Session[controller + "_#_" + action + "_@_" + System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantPageNumberSessionName"]] = pageNumber;
                return pageNumber;
            }
            
            public static int getSearchCurrentPage(string controller, string action)
            {
                var getResult = (HttpContext.Current.Session[controller + "_#_" + action + "_@_" + System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantPageNumberSessionName"]] ?? System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantPageNumberDefaultValue"]);
                return Convert.ToInt32(getResult);
            }

            public static int setSearchPageSize(string controller, string action, int pageSize)
            {
                HttpContext.Current.Session[controller + "_#_" + action + "_@_" + System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantPageSizeSessionName"]] = pageSize;
                return pageSize;
            }
            
            public static int getSearchPageSize(string controller, string action)
            {
                var getResult = (HttpContext.Current.Session[controller + "_#_" + action + "_@_" + System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantPageSizeSessionName"]] ?? System.Web.Configuration.WebConfigurationManager.AppSettings["PersistantPageSizeDefaultValue"]);
                return Convert.ToInt32(getResult);
            }
        }
    }
}