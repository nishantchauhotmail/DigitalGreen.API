using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalGreen.Model.APIRequestModels
{
    public class FarmerReqViewModel : ClientAccessViewModel
    {
        public long FarmerId { get; set; }
        public long ClientId { get; set; }
        public long GenderId { get; set; }
        public long VillageId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string Images { get; set; }
    }

    public class ClientAccessViewModel
    {
        public string ClientUserName { get; set; }
        public string ClientPassword { get; set; }
    }
}
