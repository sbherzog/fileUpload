using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fileUploadeData;
using fileUploade.Models;
using System.IO;

namespace fileUploade.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            managerDB manager = new managerDB(Properties.Settings.Default.conStr);
            homeModelView vm = new homeModelView();
            vm.imageList = manager.GetAllimages();
            return View(vm);
        }


        public ActionResult newListing()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addNewListin(imageClass image, HttpPostedFileBase file)
        {

            HttpCookie cookie = Request.Cookies["userManager"];
            if (cookie == null)
            {
                string g = Guid.NewGuid()+"";
                HttpCookie userManager = new HttpCookie("userManager", g);
                Response.Cookies.Add(userManager);
                image.cookieCode = g;
            }else
            {
                image.cookieCode = cookie.Value;
            }

            if (file != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/image") + "/" + fileName);

                managerDB manager = new managerDB(Properties.Settings.Default.conStr);
                manager.addNewListing(image, fileName);
                TempData["successful"] = "Listing uploaded successfully";
            }else
            {
                TempData["successful"] = "Did not fill in all the info";
            }

            return Redirect("/");
        }

        public ActionResult viewSingleItem(int id)
        {
            HttpCookie cookie = Request.Cookies["userManager"];
            managerDB manager = new managerDB(Properties.Settings.Default.conStr);
            singleListingViewModel vm = new singleListingViewModel();
            vm.singleListing = manager.GetSingleListing(id);
            if (cookie != null)
            {
                vm.cookie = cookie.Value;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteListing(int id)
        {
            managerDB manager = new managerDB(Properties.Settings.Default.conStr);
            string fullPath = Server.MapPath("~/image/" + manager.fileName(id));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            manager.DeleteListing(id);
            TempData["successful"] = "Successfully Delete Listing #"+ id;
            return Redirect("/");
        }
    }
}