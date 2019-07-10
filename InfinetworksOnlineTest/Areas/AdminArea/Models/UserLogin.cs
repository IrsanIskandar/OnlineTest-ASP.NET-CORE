using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinetworksOnlineTest.Areas.AdminArea.Models
{
    public class UserLogin
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string AccessPermission { get; set; }
        public string Password_adm { get; set; }
        public bool RememberMe { get; set; }

        public class OptionsPrivillage
        {
            public string Administrator { get; set; }
            public string User { get; set; }
        }
    }
}
