using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using InfinetworksOnlineTest.Areas.AdminArea.Models;
using InfinetworksOnlineTest.Areas.AdminArea.ServiceConfig;
using InfinetworksOnlineTest.ServiceConfig;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfinetworksOnlineTest.Areas.AdminArea.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Area("AdminArea")]
    public class LoginController : Controller
    {
        // GET: LoginAccess
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginAccess()
        {
            return View();
        }

        /// <summary>
        /// Login Validation Without Identity
        /// </summary>
        /// <param name="emailOrUsername"></param>
        /// <param name="inputPassword"></param>
        /// <param name="ddlAccess"></param>
        /// <param name="succecChkRemember"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccess(string emailOrUsername, string inputPassword, string ddlAccess, bool succecChkRemember)
        {
            if (String.IsNullOrEmpty(emailOrUsername) && String.IsNullOrEmpty(inputPassword) && String.IsNullOrEmpty(ddlAccess))
            {
                return BadRequest();
            }

            UserLogin result = await DatabaseService<UserLogin>.ExecuteSingleAsync(Constant.SP_GETLOGINUSER,
                new
                {
                    p_Email = emailOrUsername,
                    p_Password = inputPassword,
                    p_AccessUser = ddlAccess
                });

            try
            {
                //admin@infinetworks.com
                if (emailOrUsername != null && inputPassword != null && ddlAccess != null)
                {
                    List<Claim> claims = new List<Claim>
                     {
                         new Claim(ClaimTypes.Name, result.Username),
                         new Claim(ClaimTypes.Email, result.Email),
                         new Claim(ClaimTypes.Role, result.AccessPermission)
                     };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(6),
                        IsPersistent = true,
                        IssuedUtc = DateTime.UtcNow.AddMinutes(60)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    if (result != null)
                    {
                        if (succecChkRemember == true)
                        {
                            TempData["cookieEmail"] = CookiesHelper.SetCookies("CookieEmail", result.Email, 7);
                            TempData["CookieFullname"] = CookiesHelper.SetCookies("CookieFullname", result.Fullname, 7);
                            TempData["CookieUsername"] = CookiesHelper.SetCookies("CookieUsername", result.Username, 7);
                            TempData["CookiePermission"] = CookiesHelper.SetCookies("CookiePermission", result.AccessPermission, 7);
                        }
                        else
                        {
                            TempData["SessionEmail"] = CookiesHelper.SetSession("SessionEmail", result.Email);
                            TempData["SessionFullname"] = CookiesHelper.SetSession("SessionFullname", result.Fullname);
                            TempData["SessionUsername"] = CookiesHelper.SetSession("SessionUsername", result.Username);
                            TempData["SessionPermission"] = CookiesHelper.SetSession("SessionPermission", result.AccessPermission);
                        }
                    }

                    return RedirectToAction("Home", "HomeAdmin");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException myExp)
            {
                ViewBag.ErrorDatabase = myExp.Message;
            }
            catch (Exception ex)
            {
                ViewBag.SystemError = ex.Message;
            }

            return View(result);
        }

        // GET: SignOut
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            List<string> getCookies = new List<string>()
            {
                CookiesHelper.GetCookies("CookieEmail"),
                CookiesHelper.GetCookies("CookieFullname"),
                CookiesHelper.GetCookies("CookieUsername"),
                CookiesHelper.GetCookies("CookiePermission")
            };

            if (getCookies[0] != null && getCookies[3] != null)
            {
                foreach (var item in getCookies)
                {
                    CookiesHelper.RemoveCookies(item);
                }

                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.ReturnUrlParameter, 
                    new AuthenticationProperties() { ExpiresUtc = DateTime.UtcNow.AddDays(-7) });
            }
            else
            {
                string[] key =
                {
                    CookiesHelper.GetSession("SessionEmail"),
                    CookiesHelper.GetSession("SessionFullname"),
                    CookiesHelper.GetSession("SessionUsername"),
                    CookiesHelper.GetSession("SessionPermission")
                };

                foreach (string item in key)
                {
                    CookiesHelper.RemoveSession(item);
                }

                HttpContext.Session.Clear();
            }

            return RedirectToAction("LoginAccess");
        }
    }
}