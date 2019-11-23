using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalGreen.Model.APIResponseModels
{
    public class GetFarmerByFarmerIdResVM
    {
        public long FarmerId { get; set; }
        public long ClientId { get; set; }
        public List<string> FarmerImages { get; set; }
    }

   
}
