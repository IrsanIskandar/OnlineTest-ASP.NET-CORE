using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinetworksOnlineTest.ServiceConfig
{
    public class Constant
    {
        //Property Config Services
        public static string connectionString { get; set; }
        public static string AuthServer { get; set; }


        // Create Constant Stored Procedure Name
        public static readonly string SP_ADDUSERINTERVIEW = "spINFINET_AddUserInterview";
        public static readonly string SP_GETALLANSWERINTERVIEW = "spINFINET_GetAllAnswerUser";
        public static readonly string SP_GETANSWERUSER = "spINFINET_GetAnswersUser";
        public static readonly string SP_GETLOGINUSER = "spINFINET_GetLoginAdminInfinet";
        public static readonly string SP_SEARCHANSWER = "spINFINET_GetSearchAnswer";
        public static readonly string SP_GETUSERINTERVIEW = "spINFINET_GetUserInterview";
        public static readonly string SP_INSERTANSWERINTERVIEWER = "spINFINET_InsertUserAnsware";


        //Time on GMT+0 Timezone
        public static readonly DateTime GMT_TIME =
            TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
    }
}
