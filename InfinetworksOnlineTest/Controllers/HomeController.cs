using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinetworksOnlineTest.Models;
using InfinetworksOnlineTest.ServiceConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfinetworksOnlineTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: RegisterInterviewer
        [HttpGet]
        public IActionResult RegisterInterviewer()
        {
            return View();
        }

        #region Fake Method Test No Save In Database
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult RegisterInterviewer(long id, string inputNama, string inputNoTlp, string ddlDivisi)
        //{
        //    AddInterviewer spModel = new AddInterviewer();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            spModel.UserID = id;
        //            spModel.NamaLengkap = inputNama;
        //            spModel.NomorTelepon = inputNoTlp;
        //            spModel.Divisi = ddlDivisi;

        //            string timeLeft = DateTime.Now.AddMinutes(2).ToString();
        //            HttpContext.Session.SetString("userID", id.ToString());
        //            HttpContext.Session.SetString("Fullname", spModel.NamaLengkap);
        //            HttpContext.Session.SetString("PhoneNumber", spModel.NomorTelepon);
        //            HttpContext.Session.SetString("Divisi", spModel.Divisi);
        //            HttpContext.Session.SetString("TimeLeft", timeLeft);

        //            return RedirectToAction("AddAnswerInterviewer");
        //        }
        //    }
        //    catch (MySql.Data.MySqlClient.MySqlException myEx)
        //    {
        //        ViewBag.StatusDBError = myEx.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.StatusSystemError = ex.Message;
        //    }

        //    return View(spModel);
        //}
        #endregion

        #region Real Method Save In Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterInterviewer(long id, string inputNama, string inputNoTlp, string ddlDivisi)
        {
            AddInterviewer spModel = new AddInterviewer();
            try
            {
                if (ModelState.IsValid)
                {
                    spModel.UserID = id;
                    spModel.NamaLengkap = inputNama;
                    spModel.NomorTelepon = inputNoTlp;
                    spModel.Divisi = ddlDivisi;

                    MySql.Data.MySqlClient.MySqlParameter[] parameters =
                    {
                        new MySql.Data.MySqlClient.MySqlParameter("p_FullName", spModel.NamaLengkap),
                        new MySql.Data.MySqlClient.MySqlParameter("p_PhoneNumber", spModel.NomorTelepon),
                        new MySql.Data.MySqlClient.MySqlParameter("p_Divisi", spModel.Divisi),
                        new MySql.Data.MySqlClient.MySqlParameter("p_UserID", MySql.Data.MySqlClient.MySqlDbType.Int64){ Direction = System.Data.ParameterDirection.Output }
                    };

                    MySql.Data.MySqlClient.MySqlCommand command = await DatabaseService.Execute(Constant.SP_ADDUSERINTERVIEW, parameters: parameters);

                    string timeLeft = DateTime.Now.AddMinutes(2).ToString();
                    int result = await command.ExecuteNonQueryAsync();
                    id = Convert.ToInt64(command.Parameters["p_UserID"].Value);
                    await DatabaseConnection.GetSqlConnection().CloseAsync();

                    if (result >= 0)
                    {
                        HttpContext.Session.SetString("userID", id.ToString());
                        HttpContext.Session.SetString("Fullname", spModel.NamaLengkap);
                        HttpContext.Session.SetString("PhoneNumber", spModel.NomorTelepon);
                        HttpContext.Session.SetString("Divisi", spModel.Divisi);
                        HttpContext.Session.SetString("TimeLeft", timeLeft);

                        return RedirectToAction("Congratulation");
                    }
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

            return View(spModel);
        }
        #endregion

        // GET: AddAnswerInterviewer
        [HttpGet]
        public IActionResult AddAnswerInterviewer()
        {
            string totalMinutes = HttpContext.Session.GetString("TimeLeft");
            ViewBag.SessionUserID = HttpContext.Session.GetString("userID");
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.Division = HttpContext.Session.GetString("Divisi");
            ViewBag.TimeLeft = (int)DateTime.Parse(totalMinutes).Subtract(DateTime.Now).TotalMinutes * 1000 * 60;

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
        [HttpGet]
        public IActionResult Congratulation()
        {
            return View();
        }

        
    }
}