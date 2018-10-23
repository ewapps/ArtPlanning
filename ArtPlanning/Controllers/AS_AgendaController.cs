using ArtPlanning.Common;
using ArtPlanning.Models;
using ArtPlanning.Models.Database;
using ArtPlanning.Models.Repositories;
using ArtPlanning.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtPlanning.Controllers
{
    [Authorize]
    public class AS_AgendaController : Controller
    {
        public ActionResult Index()
        {
            // TODO : get from task_type in database
            List<SelectListItem> values = new List<SelectListItem>() {
                    new SelectListItem { Value = "PCK", Text = "Enlèvement" },
                    new SelectListItem { Value = "EXP", Text = "Expédition" }
                };

            ViewBag.NoYesValues = ARTSHIPPERS.Helpers.NoYesValues();
            ViewBag.TypeValues = values;
            ViewBag.StaffList = GetStaff();
            ViewBag.VehiclesList = GetVehicles();
            ViewBag.MaterialsList = GetMaterials();
            ViewBag.ServicesList = GetServices();

            return View();
        }


        [HttpPost]
        public ActionResult GetFoldersView(string criteria, int page = 1)
        {
            ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase();
            var innerQuery = (from m in database.mission
                              join t in database.task on m.id equals t.mission_id
                              where !t.scheduled_start_date.HasValue
                              select m.folder_id);

            IEnumerable<FolderViewModel> folders = (from f in database.folder
                                                    where innerQuery.Contains(f.id) && (!f.closed.HasValue || f.closed.Value == false)
                                                    select new FolderViewModel
                                                    {
                                                        ID = f.id,
                                                        Name = f.name,
                                                        Description = f.description,
                                                        CustomerName = f.customer_name
                                                    }
                                                   );


            if (!string.IsNullOrEmpty(criteria))
            {
                folders = folders.Where(predicate: x => x.Name.ToLower().Contains(criteria.ToLower()) || x.CustomerName.ToLower().Contains(criteria.ToLower()));
            }            

            folders = folders.OrderBy(f => f.Name);

            return PartialView("FolderPartialView", folders.ToPagedList(page, 10));

        }

        [HttpPost]
        public JsonResult GetStaffMembers()
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var members = (from u in database.AspNetUsers where u.active.HasValue && u.active == true select u)
                               .AsEnumerable()
                               .Select(u => new StaffMemberViewModel
                               {
                                   ID = u.Id,
                                   LastName = u.last_name,
                                   FirstName = u.first_name,
                                   Color = u.color,
                                   Trigram = !string.IsNullOrEmpty(u.initials) ? u.initials : Initials(u.last_name, u.first_name)
                               })
                               .OrderBy(x => x.FirstName).ToList();

                return Json(new { members, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult GetFolderTasks(int folderId)
        {
            var repository = new TaskRepository();
            IEnumerable<TaskViewModel> tasks = repository.GetTasks(folderId);
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDrivers(List<string> members, bool force)
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var drivers = (from u in database.AspNetUsers where u.active.HasValue && u.active == true select u)
                               .AsEnumerable()
                               .Select(x => new DriverResource
                               {
                                   id = x.Id,
                                   title = !string.IsNullOrEmpty(x.initials) ? x.initials : Initials(x.last_name, x.first_name),
                                   eventColor = x.color,
                                   firstname = x.first_name
                               })
                               .OrderBy(x => x.firstname);

                List<DriverResource> resources;
                if (members != null)
                {
                    resources = drivers.Where(d => members.Contains(d.id)).ToList();
                }
                else
                {
                    resources = !force ? drivers.ToList() : new List<DriverResource>();
                }

                return Json(resources, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetAgendaTasks(List<string> members, bool force, double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);

            using (ARTSHIPPERSDatabase db = new ARTSHIPPERSDatabase())
            {
                var tasks = (from t in db.task
                             join s in db.task_staff on t.id equals s.task_id
                             join u in db.AspNetUsers on s.driver_id equals u.Id
                             join m in db.mission on t.mission_id equals m.id
                             join f in db.folder on m.folder_id equals f.id
                             join a in db.address on t.address_id equals a.id into ta from subAdress in ta.DefaultIfEmpty()
                             join c in db.country on subAdress.country_id equals c.id into country from leftCountry in country.DefaultIfEmpty()
                             where ((t.scheduled_start_date >= fromDate && t.scheduled_start_date <= toDate) || (t.scheduled_end_date <= toDate && t.scheduled_end_date >= fromDate) || (t.scheduled_start_date <= fromDate && t.scheduled_end_date >= toDate)) && (u.active.HasValue && u.active == true)
                             select new
                             {
                                 t.id,
                                 t.title,
                                 t.comment,
                                 start = t.scheduled_start_date,
                                 end = t.scheduled_end_date,
                                 day = (t.day ?? false),
                                 @fixed = (t.@fixed ?? false),
                                 u.color,
                                 className = "DELIVERY",
                                 resourceId = s.driver_id,
                                 staff = (from ts in db.task_staff where ts.task_id == t.id select ts.driver_id).ToList(),
                                 vehicles = (from tv in db.task_vehicle where tv.task_id == t.id select tv.vehicle_id).ToList(),
                                 materials = (from tm in db.task_material where tm.task_id == t.id select tm.material_id).ToList(),
                                 services = (from ts in db.task_service where ts.task_id == t.id && ts.service_type_id == 1 select ts.service_id).ToList(),
                                 missionType = m.mission_type.code,
                                 street1 = !string.IsNullOrEmpty(subAdress.street_line1) ? subAdress.street_line1.Trim() : "",
                                 zip = !string.IsNullOrEmpty(subAdress.zip) ? subAdress.zip.Trim() : "",
                                 city = !string.IsNullOrEmpty(subAdress.city) ? subAdress.city.Trim() : "",
                                 countryCode = !string.IsNullOrEmpty(leftCountry.code) ? leftCountry.code.Trim() : "",
                                 countryName = !string.IsNullOrEmpty(leftCountry.name) ? leftCountry.name.Trim() : "",
                                 clientName = f.customer_name,
                                 t.added_date,
                                 t.added_user,
                                 t.modification_date,
                                 t.modification_user,
                                 folderName = f.name,
                                 folderDescription = f.description,
                                 type = t.task_type.code
                             }
                            )
                            .AsEnumerable()
                            .Select(x => new AgendaTaskViewModel{ id = x.id, title = x.title, comment = x.comment, start = x.start, end = x.end, day = x.day, @fixed = x.@fixed, eventColor = x.color, className = x.className, resourceId = x.resourceId, staff = x.staff, vehicles = x.vehicles, materials = x.materials, services = x.services, missionType = x.missionType, addedLabel = ARTSHIPPERS.User.GetUpdateDateDescription(x.added_date, x.added_user), modificationLabel = ARTSHIPPERS.User.GetUpdateDateDescription(x.modification_date, x.modification_user), folderName = x.folderName, folderDescription = x.folderDescription, type = x.type, street1 = x.street1, zip = x.zip, city = x.city, countryCode = x.countryCode, countryName = x.countryName, clientName = x.clientName });

                List<AgendaTaskViewModel> events;
                if (members != null)
                {
                    events = tasks.Where(d => members.Contains(d.resourceId)).ToList();
                }
                else
                {
                    events = !force ? tasks.ToList() : new List<AgendaTaskViewModel>();
                }
                
                return Json(events, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public ActionResult UpdateAgendaTasks(List<AgendaTaskViewModel> tasks)
        {
            if (tasks != null)
            {
                using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
                {
                    foreach (var agendaTask in tasks)
                    {
                        if (agendaTask != null)
                        {
                            if (agendaTask.id > 0)
                            {
                                // UPDATE CURRENT TASK

                                var task = database.task.Find(agendaTask.id);
                                if (task != null)
                                {
                                    //if (task.mission.mission_type.code.Equals("ALL"))
                                    //{
                                    //    task.title = agendaTask.title;
                                    //}

                                    task.title = agendaTask.title;
                                    task.comment = agendaTask.comment;
                                    task.scheduled_start_date = agendaTask.start;
                                    task.scheduled_end_date = agendaTask.end;
                                    task.@fixed = agendaTask.@fixed;                                    

                                    if (!task.added_date.HasValue)
                                    {
                                        task.added_date = DateTime.Now;
                                        task.added_user = User.Identity.GetUserId();
                                        task.sql_modified = false;
                                        task.sql_deleted = false;
                                    }
                                    else
                                    {
                                        task.modification_date = DateTime.Now;
                                        task.modification_user = User.Identity.GetUserId();
                                        task.sql_modified = true;
                                        task.sql_deleted = false;
                                    }

                                    // UPDATE STAFF OF CURRENT TASK

                                    if (agendaTask.staff != null && agendaTask.staff.Count() > 0)
                                    {
                                        var staff = (from s in database.task_staff where s.task_id == task.id select s);
                                        if (staff.Count() > 0)
                                        {
                                            database.task_staff.RemoveRange(staff);
                                        }

                                        foreach (var driver in agendaTask.staff)
                                        {
                                            var task_staff = new task_staff
                                            {
                                                task_id = task.id,
                                                driver_id = driver,
                                                added_date = DateTime.Now,
                                                added_user = User.Identity.GetUserId()
                                            };

                                            database.task_staff.Add(task_staff);
                                        }
                                    }

                                    // UPDATE VEHICLES OF CURRENT TASK

                                    if (agendaTask.vehicles != null)
                                    {
                                        var vehicles = (from v in database.task_vehicle where v.task_id == task.id select v);
                                        if (vehicles.Count() > 0)
                                        {
                                            database.task_vehicle.RemoveRange(vehicles);
                                        }

                                        if (agendaTask.vehicles.Count() > 0)
                                        {
                                            foreach (var vehicle in agendaTask.vehicles)
                                            {
                                                var task_vehicle = new task_vehicle
                                                {
                                                    task_id = task.id,
                                                    vehicle_id = vehicle,
                                                    added_date = DateTime.Now,
                                                    added_user = User.Identity.GetUserId()
                                                };

                                                database.task_vehicle.Add(task_vehicle);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var vehicles = (from v in database.task_vehicle where v.task_id == task.id select v);
                                        if (vehicles.Count() > 0)
                                        {
                                            database.task_vehicle.RemoveRange(vehicles);
                                        }
                                    }

                                    // UPDATE MATERIALS OF CURRENT TASK

                                    if (agendaTask.materials != null)
                                    {
                                        var materials = (from m in database.task_material where m.task_id == task.id select m);
                                        if (materials.Count() > 0)
                                        {
                                            database.task_material.RemoveRange(materials);
                                        }

                                        if (agendaTask.materials.Count() > 0)
                                        {
                                            foreach (var material in agendaTask.materials)
                                            {
                                                var task_material = new task_material
                                                {
                                                    task_id = task.id,
                                                    material_id = material,
                                                    added_date = DateTime.Now,
                                                    added_user = User.Identity.GetUserId()
                                                };

                                                database.task_material.Add(task_material);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var materials = (from m in database.task_material where m.task_id == task.id select m);
                                        if (materials.Count() > 0)
                                        {
                                            database.task_material.RemoveRange(materials);
                                        }
                                    }

                                    // UPDATE SERVICES OF CURRENT TASK

                                    if (agendaTask.services != null)
                                    {
                                        int serviceTypeSCHEDULED = (from st in database.task_service_type where st.code == "SCHEDULED" select st.id).SingleOrDefault();
                                        var services = (from s in database.task_service where s.task_id == task.id && s.service_type_id == serviceTypeSCHEDULED select s);                                        

                                        if (services.Count() > 0)
                                        {
                                            database.task_service.RemoveRange(services);
                                        }

                                        if (agendaTask.services.Count() > 0)
                                        {
                                            foreach (var service in agendaTask.services)
                                            {
                                                var task_service = new task_service
                                                {
                                                    task_id = task.id,
                                                    service_id = service,
                                                    service_type_id = serviceTypeSCHEDULED,
                                                    added_date = DateTime.Now,
                                                    added_user = User.Identity.GetUserId()
                                                };

                                                database.task_service.Add(task_service);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var services = (from s in database.task_service where s.task_id == task.id select s);
                                        if (services.Count() > 0)
                                        {
                                            database.task_service.RemoveRange(services);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // ADD NEW TASK

                                // vérifier si le dossier "[PARENT]" existe

                                int primaryFolderID = (from f in database.folder where f.name == "[PARENT]" select f.id).SingleOrDefault();
                                int primaryMissionID = 0;
                                int missionTypeCMR = (from mt in database.mission_type where mt.code == "ALL" select mt.id).SingleOrDefault();

                                if (primaryFolderID == 0)
                                {
                                    // création du dossier primaire pour les tâches ajoutées manuellement (sans dossier de provenance)

                                    var primaryFolder = new folder
                                    {
                                        client_id = 0,
                                        name = "[PARENT]",
                                        description = "--UNUSED FOLDER FOR TASK WITHOUT FOLDER--",
                                        customer_name = "[PARENT]",
                                        added_date = DateTime.Now
                                    };

                                    database.folder.Add(primaryFolder);
                                    database.SaveChanges();
                                    primaryFolderID = primaryFolder.id;
                                }

                                // création de la mission primaire pour les tâches ajoutées manuellement (sans dossier de provenance)

                                var primaryMission = new mission
                                {
                                    folder_id = primaryFolderID,
                                    mission_type_id = missionTypeCMR,
                                    name = "[PARENT]",
                                    description = "--UNUSED MISSION FOR TASK WITHOUT FOLDER--",
                                    added_date = DateTime.Now
                                };

                                database.mission.Add(primaryMission);
                                database.SaveChanges();
                                primaryMissionID = primaryMission.id;
                                
                                // ajout de la nouvelle tâche manuelle

                                var newTask = new task();
                                newTask.mission_id = primaryMissionID;
                                newTask.task_type_id = 0;
                                newTask.title = agendaTask.title;
                                newTask.comment = agendaTask.comment;
                                newTask.scheduled_start_date = agendaTask.start;
                                newTask.scheduled_end_date = agendaTask.end;
                                newTask.day = agendaTask.day;
                                newTask.@fixed = agendaTask.@fixed;
                                newTask.added_date = DateTime.Now;
                                newTask.added_user = User.Identity.GetUserId();
                                newTask.sql_modified = false;
                                newTask.sql_deleted = false;
                                database.task.Add(newTask);
                                database.SaveChanges();

                                // ADD STAFF TO NEW TASK

                                if (agendaTask.staff != null && agendaTask.staff.Count() > 0)
                                {
                                    foreach (var driver in agendaTask.staff)
                                    {
                                        var task_staff = new task_staff
                                        {
                                            task_id = newTask.id,
                                            driver_id = driver,
                                            added_date = DateTime.Now,
                                            added_user = User.Identity.GetUserId()
                                        };

                                        database.task_staff.Add(task_staff);
                                    }

                                    database.SaveChanges();
                                }

                                // ADD VEHICLES TO NEW TASK

                                if (agendaTask.vehicles != null && agendaTask.vehicles.Count() > 0)
                                {
                                    foreach (var vehicle in agendaTask.vehicles)
                                    {
                                        var task_vehicle = new task_vehicle
                                        {
                                            task_id = newTask.id,
                                            vehicle_id = vehicle,
                                            added_date = DateTime.Now,
                                            added_user = User.Identity.GetUserId()
                                        };

                                        database.task_vehicle.Add(task_vehicle);
                                    }
                                }

                                // ADD MATERIALS TO NEW TASK

                                if (agendaTask.materials != null && agendaTask.materials.Count() > 0)
                                {
                                    foreach (var material in agendaTask.materials)
                                    {
                                        var task_material = new task_material
                                        {
                                            task_id = newTask.id,
                                            material_id = material,
                                            added_date = DateTime.Now,
                                            added_user = User.Identity.GetUserId()
                                        };

                                        database.task_material.Add(task_material);
                                    }
                                }
                                // ADD SERVICES TO NEW TASK

                                if (agendaTask.services != null && agendaTask.services.Count() > 0)
                                {
                                    int serviceTypeSCHEDULED = (from st in database.task_service_type where st.code == "SCHEDULED" select st.id).SingleOrDefault();

                                    foreach (var service in agendaTask.services)
                                    {
                                        var task_service = new task_service
                                        {
                                            task_id = newTask.id,
                                            service_id = service,
                                            service_type_id = serviceTypeSCHEDULED,
                                            added_date = DateTime.Now,
                                            added_user = User.Identity.GetUserId()
                                        };

                                        database.task_service.Add(task_service);
                                    }
                                }
                            }
                        }
                    }

                    database.SaveChanges();

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateAgendaTaskDuration(AgendaTaskViewModel task)
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                if (task != null)
                {
                    if (task.id > 0)
                    {
                        // UPDATE CURRENT TASK

                        var currentTask = database.task.Find(task.id);
                        if (currentTask != null)
                        {
                            currentTask.scheduled_start_date = task.start;
                            currentTask.scheduled_end_date = task.end;
                            currentTask.day = task.day;
                            currentTask.modification_date = DateTime.Now;
                            currentTask.modification_user = User.Identity.GetUserId();
                            currentTask.sql_modified = true;
                            currentTask.sql_deleted = false;

                            database.SaveChanges();

                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteAgendaTaskDuration(int id)
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {                
                if (id > 0)
                {
                    // DELETE CURRENT TASK

                    var currentTask = database.task.Find(id);
                    if (currentTask != null)
                    {
                        string code = currentTask.mission.mission_type.code;
                        bool isCMR = currentTask.mission.mission_type.code.Equals("CMR");

                        // DELETE STAFF OF CURRENT TASK

                        var staff = (from s in database.task_staff where s.task_id == currentTask.id select s);
                        if (staff.Count() > 0)
                        {
                            database.task_staff.RemoveRange(staff);
                        }

                        // DELETE VEHICLES OF CURRENT TASK

                        var vehicles = (from v in database.task_vehicle where v.task_id == currentTask.id select v);
                        if (vehicles.Count() > 0)
                        {
                            database.task_vehicle.RemoveRange(vehicles);
                        }

                        // DELETE MATERIALS OF CURRENT TASK

                        var materials = (from m in database.task_material where m.task_id == currentTask.id select m);
                        if (materials.Count() > 0)
                        {
                            database.task_material.RemoveRange(materials);
                        }

                        // DELETE SERVICES OF CURRENT TASK

                        var services = (from srv in database.task_service where srv.task_id == currentTask.id select srv);
                        if (services.Count() > 0)
                        {
                            database.task_service.RemoveRange(services);
                        }

                        if (isCMR)
                        {
                            currentTask.scheduled_start_date = null;
                            currentTask.scheduled_end_date = null;
                            currentTask.arrival_date = null;
                            currentTask.departure_date = null;
                            currentTask.arrival_mileage = null;
                            currentTask.departure_mileage = null;
                            currentTask.added_date = null;
                            currentTask.added_user = null;
                            currentTask.modification_date = null;
                            currentTask.modification_user = null;
                            currentTask.sql_deleted = true;
                            currentTask.day = false;
                            currentTask.@fixed = false;
                            currentTask.comment = null;
                        }
                        else
                        {
                            database.task.Remove(currentTask);
                        }

                        database.SaveChanges();

                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CloseFolder(int id)
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                if (id > 0)
                {
                    var currentFolder = database.folder.Find(id);
                    if (currentFolder != null)
                    {
                        currentFolder.closed = true;
                        currentFolder.closed_date = DateTime.Now;
                        currentFolder.closed_user = User.Identity.GetUserId();

                        database.SaveChanges();

                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }



        private List<SelectListItem> GetStaff()
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var staff = (from u in database.AspNetUsers where u.active.HasValue && u.active == true select u)
                             .AsEnumerable()
                             .Select(x => new SelectListItem
                             {
                                 Text = NameWithInitials(x.last_name, x.first_name),
                                 Value = x.Id
                             })
                             .OrderBy(x => x.Text).ToList();

                return staff;
            }
        }

        private List<SelectListItem> GetVehicles()
        {
            var languageID = 1;

            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase()) 
            {
                var vehiclesList = (from tv in database.translate_vehicle
                                    join v in database.vehicle on tv.vehicle_id equals v.id
                                    where tv.language_id == languageID
                                    orderby v.code
                                    select new SelectListItem
                                    {
                                        Value = v.id.ToString(),
                                        Text = tv.name
                                    }
                                   ).ToList();

                return vehiclesList;           
            }
        }

        private List<SelectListItem> GetMaterials()
        {
            var languageID = 1;

            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var materialsList = (from tm in database.translate_material
                                     join m in database.material on tm.material_id equals m.id
                                     where tm.language_id == languageID
                                     orderby m.code
                                     select new SelectListItem
                                     {
                                        Value = m.id.ToString(),
                                        Text = tm.name
                                     }
                                    ).ToList();

                return materialsList;
            }
        }

        private List<SelectListItem> GetServices()
        {
            var languageID = 1;

            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var servicesList = (from ts in database.translate_service
                                     join s in database.service on ts.service_id equals s.id
                                     where ts.language_id == languageID
                                     orderby s.code
                                     select new SelectListItem
                                     {
                                         Value = s.id.ToString(),
                                         Text = ts.name
                                     }
                                    ).ToList();

                return servicesList;
            }
        }



        private DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private string Initials(string lastName, string firstName)
        {
            string prefix = "";
            string suffix = "";

            if (!string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(firstName))
            {
                prefix = firstName.Substring(0, 1);
                suffix = lastName.Substring(0, lastName.Length > 1 ? 2 : 1);

                return string.Format("{0}{1}", prefix, suffix).ToUpper();
            }
            else
            {
                if (!string.IsNullOrEmpty(lastName))
                {
                    prefix = lastName.Substring(0, 1);
                }

                if (!string.IsNullOrEmpty(firstName))
                {
                    suffix = firstName.Substring(0, firstName.Length > 1 ? 2 : 1);
                }

                return string.Format("{0}{1}", prefix, suffix).ToUpper();
            }
        }

        private string NameWithInitials(string lastName, string firstName)
        {
            if (!string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(firstName))
            {
                return string.Format("{0} {1}.", lastName, firstName.Substring(0, 1));
            }
            else
            {
                if (!string.IsNullOrEmpty(lastName)) { return lastName; }
                if (!string.IsNullOrEmpty(firstName)) { return firstName; }

                return "DRV";
            }
        }        
    }
}