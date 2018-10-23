using ArtPlanning.Helpers.Common;
using ArtPlanning.Models.Database;
using ArtPlanning.ViewModels.Warehouses;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtPlanning.Controllers
{
    [Authorize]
    public class AS_Tools_WarehouseManagementController : Controller
    {
        /* GET: AS_Tools_WarehouseManagement */
        public ActionResult Index()
        {
            return View();
        }

        /* GET: GetWarehouses */
        public ActionResult GetWarehouses()
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var warehousesNodes = (from w in database.warehouse orderby w.position ascending select new WarehouseTreeNodeViewModel { id = w.id, parent = !string.IsNullOrEmpty(w.parent_id) ? w.parent_id : "#", text = w.name, icon = (string.IsNullOrEmpty(w.parent_id) || (!string.IsNullOrEmpty(w.parent_id) && w.parent_id == "#")) ? "glyphicon glyphicon-home" : "glyphicon glyphicon-th-large" });

                return Json(warehousesNodes.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        /* POST: RenameNode */
        [HttpPost]
        public ActionResult RenameNode(WarehouseTreeNodeViewModel wtnvm)
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                if (!string.IsNullOrEmpty(wtnvm.id))
                {
                    var node = database.warehouse.Find(wtnvm.id);

                    if (node != null)
                    {
                        // UPDATE

                        node.name = wtnvm.text;
                        node.modification_date = DateTime.Now;
                        node.modification_user = User.Identity.GetUserId();
                    }
                    else
                    {
                        // ADD

                        short position = GetNewPosition(wtnvm.parent);

                        var newNode = new warehouse
                        {
                            id = Guid.NewGuid().ToString(),
                            name = wtnvm.text,
                            parent_id = wtnvm.parent,
                            position = position,
                            added_date = DateTime.Now,
                            added_user = User.Identity.GetUserId()
                        };

                        database.warehouse.Add(newNode);
                    }

                    database.SaveChanges();

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /* POST: DeleteNode */
        [HttpPost]
        public ActionResult DeleteNode(string id)
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                // get current node
                var currentNode = database.warehouse.Find(id);

                // delete children of current node in recursive mode
                DeleteChildren(database, id);
                // delete current node
                database.warehouse.Remove(currentNode);

                // commit changes to database
                database.SaveChanges();
            }
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GenerateQRCode(string code)
        {
            return Json(QRCodeGeneratorHelper.GenerateQRCodeBase64Image(code), JsonRequestBehavior.AllowGet);
        }


        /* Delete children of a parent node */
        private void DeleteChildren(ARTSHIPPERSDatabase entites, string id)
        {
            // get current children of parent
            var children = entites.warehouse.Where(x => x.parent_id == id).ToList();

            if (children != null) {
                foreach (warehouse child in children)
                {
                    // delete children of current node in recursive mode
                    DeleteChildren(entites, child.id);
                    // delete current child
                    entites.warehouse.Remove(child);
                }
            }
        }

        /* Get position of newly node */
        private short GetNewPosition(string parent)
        {
            short position = 1;

            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                var childrenOfParent = database.warehouse.Where(w => w.parent_id == parent).Select(x => x.position).ToList();

                if (childrenOfParent != null && childrenOfParent.Count() > 0)
                {
                    position = Convert.ToInt16(childrenOfParent.Max() + 1);
                }
            }

            return position;
        }
    }
}