using DigitalGreen.Business.ClientAPI.Implementation;
using DigitalGreen.Business.ClientAPI.Interface;
using DigitalGreen.Core.ResponseModel;
using DigitalGreen.Model.APIRequestModels;
using DigitalGreen.Model.APIResponseModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;

namespace DigitalGreen.API.Controllers
{
    public class FarmerController : ApiController
    {

        #region Private member variables...
        private readonly IFarmerService _farmerService;
        private readonly IApiClientVaildationService _apiClientVaildationService;

        #endregion

        #region Public Constructor...
        public FarmerController()
        {
            _farmerService = new FarmerService();
            _apiClientVaildationService = new ApiClientVaildationService();
        }
        #endregion

        #region Farmer

        /// <summary>
        /// Get farmer images by farmer ID.
        /// </summary>
        /// <param name="farmerReqViewModel">Request DTO.</param>
        /// <returns>It returns farmers meta data and images.</returns>
        [HttpPost]
        public IHttpActionResult GetFarmerImagesByFarmerID([FromBody]FarmerReqViewModel farmerReqViewModel)
        {
            ResponseViewModel<GetFarmerByFarmerIdResVM> responseViewModel = new ResponseViewModel<GetFarmerByFarmerIdResVM>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(farmerReqViewModel.ClientUserName, farmerReqViewModel.ClientPassword))
                {
                    var data = _farmerService.GetFarmerByFarmerId(farmerReqViewModel);
                    responseViewModel = data != null ? ResponseViewModel<GetFarmerByFarmerIdResVM>.Succeeded(data, "") : ResponseViewModel<GetFarmerByFarmerIdResVM>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<GetFarmerByFarmerIdResVM>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new GetFarmerByFarmerIdResVM());
                    return Ok(responseViewModel);
                }

            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<GetFarmerByFarmerIdResVM>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new GetFarmerByFarmerIdResVM());
                return Ok(responseViewModel);
            }

        }

        /// <summary>
        /// Get farmer images by village id.
        /// </summary>
        /// <param name="farmerReqViewModel">Request DTO.</param>
        /// <returns>It returns list of all the farmers with their meta data and images.</returns>
        [HttpPost]
        public IHttpActionResult GetFarmerImagesByVillageId([FromBody]FarmerReqViewModel farmerReqViewModel)
        {
            ResponseViewModel<List<GetFarmerByFarmerIdResVM>> responseViewModel = new ResponseViewModel<List<GetFarmerByFarmerIdResVM>>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(farmerReqViewModel.ClientUserName, farmerReqViewModel.ClientPassword))
                {
                    var data = _farmerService.GetFarmerByVillageId(farmerReqViewModel);
                    responseViewModel = data != null ? ResponseViewModel<List<GetFarmerByFarmerIdResVM>>.Succeeded(data, "") : ResponseViewModel<List<GetFarmerByFarmerIdResVM>>.Succeeded(data, "No Record Found");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<List<GetFarmerByFarmerIdResVM>>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new List<GetFarmerByFarmerIdResVM>());
                    return Ok(responseViewModel);
                }

            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<List<GetFarmerByFarmerIdResVM>>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new List<GetFarmerByFarmerIdResVM>());
                return Ok(responseViewModel);
            }

        }


        /// <summary>
        /// To save farmer to system.
        /// </summary>
        /// <param name="farmerReqViewModel">Request DTO.</param>
        /// <returns>It returns an object of ResponseViewModel with its respective fields.</returns>
        [HttpPost]
        public IHttpActionResult SaveFarmer([FromBody]FarmerReqViewModel farmerReqViewModel)
        {
            ResponseViewModel<Boolean> responseViewModel = new ResponseViewModel<Boolean>();
            try
            {
                if (_apiClientVaildationService.IsClientVaild(farmerReqViewModel.ClientUserName, farmerReqViewModel.ClientPassword))
                {
                    var data = _farmerService.SaveFarmer(farmerReqViewModel);
                    responseViewModel = ResponseViewModel<Boolean>.Succeeded(data, "");
                    return Ok(responseViewModel);
                }
                else
                {
                    responseViewModel = ResponseViewModel<Boolean>.Failed(Core.ResponseModel.StatusCode.Unauthorized, Core.Helper.Constants.StrMessage.InValidAccess, Core.Helper.Constants.StrMessage.InValidAccess, null, "", new Boolean());
                    return Ok(responseViewModel);
                }

            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<Boolean>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", new Boolean());
                return Ok(responseViewModel);
            }

        }

        /// <summary>
        /// To upload image one by one.
        /// </summary>
        /// <returns>It returns image name on success. Otherwise, respective error message.</returns>
        [HttpPost]
        [ResponseType(typeof(FileUpload))]
        public IHttpActionResult UploadFarmerImage()
        {
            /// <summary>  
            ///Sets file size property. Default is 1MB (the value is in Bytes).  
            /// </summary>  
            int allowedFileSize = 1 * 1024 * 1024 ;

            ResponseViewModel<string> responseViewModel = new ResponseViewModel<string>();
            try
            {
                int num1 = 0;
                string path1 = HostingEnvironment.MapPath("~/Uploads/UploadFarmerImages/");
                if (!Directory.Exists(HostingEnvironment.MapPath("~/Uploads/UploadFarmerImages/")))
                    Directory.CreateDirectory(HostingEnvironment.MapPath("~/Uploads/UploadFarmerImages/"));
                HttpFileCollection files = HttpContext.Current.Request.Files;
                int index = 0;
                if (index > files.Count - 1)
                {
                    responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Bad_Request, "", "No File Found", null, "", string.Empty);
                    return Ok(responseViewModel);
                }
                HttpPostedFile httpPostedFile = files[index];
                if (httpPostedFile.ContentLength > 0 && !File.Exists(path1 + Path.GetFileName(httpPostedFile.FileName)))
                {
                    string extension = Path.GetExtension(httpPostedFile.FileName);
                    if (!new List<string>() { ".bmp", ".png", ".jpg", "jpeg", ".gif" }.Contains(extension.ToLower()))
                    {
                        string str = string.Format("Please Upload file of type images");
                        responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Bad_Request, "", str, null, "", string.Empty);
                        return Ok(responseViewModel);
                    }
                    var fileSize = httpPostedFile.ContentLength;

                    // Settings.  
                  if( fileSize > allowedFileSize)
                    {
                        string str = string.Format("Please Upload file of 1 MB Data");
                        responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Bad_Request, "", str, null, "", string.Empty);
                        return Ok(responseViewModel);
                    }
                   
                    string str1 = Guid.NewGuid().ToString();
                    string filename = Path.Combine(path1, Path.GetFileName(str1 + extension));
                    httpPostedFile.SaveAs(filename);
                    int num2 = num1 + 1;
                    Path.GetFileName(str1 + extension);

                    var data = Path.GetFileName(str1 + extension);
                    responseViewModel = data != null ? ResponseViewModel<string>.Succeeded(data, "") : ResponseViewModel<string>.Succeeded(data, "");
                    return Ok(responseViewModel);
                }

                responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Bad_Request, "", "Failed", null, "", string.Empty);
                return Ok(responseViewModel);
            }
            catch (Exception err)
            {
                responseViewModel = ResponseViewModel<string>.Failed(Core.ResponseModel.StatusCode.Bad_Request, err.Message, err.Message, err, "", string.Empty);
                return Ok(responseViewModel);
            }
        }
        #endregion
    }
}
