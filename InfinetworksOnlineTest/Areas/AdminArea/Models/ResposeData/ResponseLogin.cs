using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinetworksOnlineTest.Areas.AdminArea.Models.ResposeData
{
    public class ResponseLogin
    {
        public int _ID { get; set; }
        public string _Username { get; set; }
        public string _Fullname { get; set; }
        public string _Email { get; set; }
        public string _AccessPermission { get; set; }
        public string _Password_adm { get; set; }
        public bool _RememberMe { get; set; }

        public ResponseLogin(List<UserLogin> spModels)
        {
            _ID = spModels[0].ID;
            _Username = spModels[0].Username;
            _Fullname = spModels[0].Fullname;
            _Email = spModels[0].Email;
            _AccessPermission = spModels[0].AccessPermission;
            _Password_adm = spModels[0].Password_adm;
            _RememberMe = spModels[0].RememberMe;
        }
    }
}
