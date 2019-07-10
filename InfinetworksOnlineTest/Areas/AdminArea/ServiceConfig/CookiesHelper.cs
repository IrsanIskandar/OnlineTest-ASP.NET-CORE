using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InfinetworksOnlineTest.Areas.AdminArea.ServiceConfig
{
    public class CookiesHelper
    {
        private static HttpContextAccessor ContextAccessor = new HttpContextAccessor();
        private static CookieOptions cookieOptions;

        public CookiesHelper() { }

        public CookiesHelper(HttpContextAccessor httpContextAccessor)
        {
            ContextAccessor = httpContextAccessor;
        }


        /// ++++++++++++++++++++++++++++++++++
        /// +        CREATE COOKIES          +
        /// ++++++++++++++++++++++++++++++++++
        /// <summary>
        /// Set Cookies Key, Value, expireTime
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        public static string SetCookies(string key, string value, int? expireTime)
        {
            cookieOptions = new CookieOptions();
            if (!expireTime.HasValue)
            {
                cookieOptions.Expires = DateTime.UtcNow.AddHours(6);
            }
            else
            {
                cookieOptions.Expires = DateTime.UtcNow.AddDays(expireTime.Value);
                ContextAccessor.HttpContext.Response.Cookies.Append(key: key, value: value, options: cookieOptions);
            }

            return value;
        }

        /// <summary>
        /// Get Cookies Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns> Cookei Key </returns>
        public static string GetCookies(string key)
        {
            return ContextAccessor.HttpContext.Request.Cookies[key: key];
        }

        /// <summary>
        /// Delete Key Cookies
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveCookies(string key)
        {
            ContextAccessor.HttpContext.Response.Cookies.Delete(key: key);
        }



        /// ++++++++++++++++++++++++++++++++++
        /// +        CREATE SESSION          +
        /// ++++++++++++++++++++++++++++++++++
        /// <summary>
        /// Set Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static string SetSession(string key, string value)
        {
            ContextAccessor.HttpContext.Session.SetString(key: key, value: value);

            return value;
        }

        /// <summary>
        /// Get Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns> Session Key </returns>
        public static string GetSession(string key)
        {
            return ContextAccessor.HttpContext.Session.GetString(key: key);
        }

        /// <summary>
        /// Remove Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveSession(string key)
        {
            // Remove Session Key
            ContextAccessor.HttpContext.Session.Remove(key: key);

            // Clear Session
            ContextAccessor.HttpContext.Session.Clear();

            return key;
        }
    }
}
