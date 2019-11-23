using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalGreen.Business.ClientAPI.Interface
{
    public interface IApiClientVaildationService
    {
        bool IsClientVaild(string apiUserName, string apiPassword);
    }
}
