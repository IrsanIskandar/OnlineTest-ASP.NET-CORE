using InfinetworksOnlineTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinetworksOnlineTest.ParsingData
{
    public class RegisterUser
    {
        public long ID { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
        public string division { get; set; }

        public RegisterUser() { }

        public RegisterUser(long id, string fullName, string phoneNumber, string division)
        {
            ID = id;
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
            this.division = division;
        }

        public List<AddInterviewer> GetInterviewer(List<AddInterviewer> spModel)
        {
            List<AddInterviewer> ListInterviewer = new List<AddInterviewer>();

            ID = spModel[0].UserID;
            fullName = spModel[0].NamaLengkap;
            phoneNumber = spModel[0].NomorTelepon;
            division = spModel[0].Divisi;

            foreach (var item in spModel)
            {
                fullName = item.NamaLengkap;
                phoneNumber = item.NomorTelepon;
                division = item.Divisi;

                ListInterviewer.Add(item);
            }

            return ListInterviewer;
        }
    }
}
