using DigitalGreen.Business.ClientAPI.Interface;
using DigitalGreen.Core;
using DigitalGreen.Core.DataBase.DigitalGreenDB;
using GolfCentra.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalGreen.Business.ClientAPI.Implementation
{
  public  class ApiClientVaildationService : IApiClientVaildationService
    {
        private readonly UnitOfWork _unitOfWork;

        public ApiClientVaildationService()
        {
            _unitOfWork = new UnitOfWork();
        }

       

        /// <summary>
        /// Check Api Access Detail Is Vaild Or Not
        /// </summary>
        /// <param name="apiUserName"></param>
        /// <param name="apiPassword"></param>
        /// <returns></returns>
        public bool IsClientVaild(string apiUserName, string apiPassword)
        {
            apiUserName.TryValidate("Api User Name");
            apiPassword.TryValidate("Api Password");
            Client aPIClient = _unitOfWork.ClientRepository.Get(x => x.UserName == apiUserName && x.Password == apiPassword && x.IsActive == true);
            if (aPIClient == null)
                return false;
            return true;
        }
    }
}
