using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InfinetworksOnlineTest.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class HomeAdminController : Controller
    {
        // GET: Home
        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }
    }
}