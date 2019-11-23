using DigitalGreen.Model.APIRequestModels;
using DigitalGreen.Model.APIResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalGreen.Business.ClientAPI.Interface
{
   public interface IFarmerService
    {
        GetFarmerByFarmerIdResVM GetFarmerByFarmerId(FarmerReqViewModel farmerReqViewModel);
        bool SaveFarmer(FarmerReqViewModel farmerReqViewModel);
        List<GetFarmerByFarmerIdResVM> GetFarmerByVillageId(FarmerReqViewModel farmerReqViewModel);

    }
}
