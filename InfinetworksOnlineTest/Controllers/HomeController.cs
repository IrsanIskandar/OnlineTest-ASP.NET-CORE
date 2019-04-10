using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinetworksOnlineTest.Models;
using InfinetworksOnlineTest.ServiceConfig;
using Microsoft.AspNetCore.Mvc;

namespace InfinetworksOnlineTest.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Interviewer test)
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterInterviewer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterInterviewer(Interviewer intviewerModel)
        {
            await DatabaseService<object>.ExecuteNoReturn(Constant.SP_INSERT,
                new {
                    p_NamaLengkap = intviewerModel.NamaLengkap,
                    p_NomorTelepon = intviewerModel.NomorTelepon,
                    p_Divisi = intviewerModel.Divisi
                });

            return View();
        }
    }
}