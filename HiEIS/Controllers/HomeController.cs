using HiEIS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiEIS.Controllers
{
    [AllowAnonymous]
    public class HomeController : HiEISCompanyController
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Company", new { area = "Admin" });
            }

            if (User.Identity.IsAuthenticated && User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Staffs", new { area = "Public" });
            }
            else if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Invoices", new { area = "Public" });
            }

            return RedirectToAction("Lookup", "Home");
        }

        public ActionResult Lookup()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}