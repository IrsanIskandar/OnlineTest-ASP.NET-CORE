using System;
using System.Threading.Tasks;
using InfinetworksOnlineTest.Areas.AdminArea.ServiceConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfinetworksOnlineTest.Areas.AdminArea.Controllers
{
    public class HomeAdminController : Controller
    {
        [AllowAnonymous]
        // GET: Home
        [HttpGet]
        public IActionResult Home()
        {
            //string CookieFullname, CookiePermission, SessionFullname, SessionPermission;

            if (TempData.ContainsKey("CookieEmail") || TempData.ContainsKey("CookieEmail").Equals(0))
            {
                ViewBag.CookieFullname = TempData["CookieFullname"].ToString();
                ViewBag.CookiePermission = TempData["CookiePermission"].ToString();
            }
            else if(TempData.ContainsKey("SessionEmail"))
            {
                ViewBag.SessionFullname = TempData["SessionFullname"].ToString();
                ViewBag.SessionPermission = TempData["SessionPermission"].ToString();
            }
            else
            {
                return RedirectToAction("LoginAccess", "Login");
            }

            return View();
        }
    }
}