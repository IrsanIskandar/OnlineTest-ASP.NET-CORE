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
        // GET: RegisterInterviewer
        public IActionResult RegisterInterviewer()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterInterviewer(Interviewer intviewerModel)
        {
            await DatabaseService<object>.ExecuteNoReturn(Constant.SP_INSERTREGISTERUSER,
                new
                {
                    p_NamaLengkap = intviewerModel.NamaLengkap,
                    p_NomorTelepon = intviewerModel.NomorTelepon,
                    p_Divisi = intviewerModel.Divisi
                });

            return View();
        }

        // GET: AddAnswerInterviewer
        public IActionResult AddAnswerInterviewer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnswerInterviewer(AnswerModel answerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await DatabaseService<object>.ExecuteNoReturn(Constant.SP_INSERTANSWERINTERVIEWER,
                        new
                        {
                            p_AnswersOne = answerModel.AnswersOne,
                            p_AnswersTwo = answerModel.AnswersTwo,
                            p_AnswersThree = answerModel.AnswersThree,
                            p_AnswersFourth = answerModel.AnswersFourth,
                            p_AnswersFive = answerModel.AnswersFive,
                            p_AnswersSix = answerModel.AnswersSix,
                            p_AnswersSeven = answerModel.AnswersSeven,
                            p_AnswersEight = answerModel.AnswersEight,
                            p_AnswerNine = answerModel.AnswersNine,
                            p_UserIntervuewID = 1
                        });

                    ViewBag.Status = "Berhasil Masuk Database. 😊😁";

                    return RedirectToAction("Congratulation");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException myEx)
            {
                ViewBag.StatusDBError = myEx.Message;
            }
            catch (Exception ex)
            {
                ViewBag.StatusSystemError = ex.Message;
            }
            return View(answerModel);
        }

        // GET: Congratulation
        public IActionResult Congratulation()
        {
            return View();
        }
    }
}