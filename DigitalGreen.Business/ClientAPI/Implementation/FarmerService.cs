using DigitalGreen.Business.ClientAPI.Interface;
using DigitalGreen.Core;
using DigitalGreen.Core.DataBase.DigitalGreenDB;
using DigitalGreen.Model.APIRequestModels;
using DigitalGreen.Model.APIResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalGreen.Business.ClientAPI.Implementation
{
    public class FarmerService : IFarmerService
    {
        private readonly UnitOfWork _unitOfWork;

        public FarmerService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Get farmer images by farmer id.
        /// </summary>
        /// <param name="farmerReqViewModel">Request DTO.</param>
        /// <returns>Returns an object of GetFarmerByFarmerIdResVM.</returns>
        public GetFarmerByFarmerIdResVM GetFarmerByFarmerId(FarmerReqViewModel farmerReqViewModel)
        {
            Client client = _unitOfWork.ClientRepository.Get(x => x.UserName == farmerReqViewModel.ClientUserName && x.Password == farmerReqViewModel.ClientPassword && x.IsActive == true);
            if (client == null) throw new Exception("Invalid Client");

            Farmer farmer = _unitOfWork.FarmerRepository.Get(x => x.FarmerId == farmerReqViewModel.FarmerId && x.ClientId == client.ClientId && x.IsActive == true);
            if (farmer == null) throw new Exception("Invalid Farmer");

            GetFarmerByFarmerIdResVM getFarmerByFarmerIdResponseViewModel = new GetFarmerByFarmerIdResVM()
            {
                FarmerId = farmer.FarmerId,
                FarmerImages = FarmerImagesDBTOUser(farmer.FarmerImages.ToList())
            };
            return getFarmerByFarmerIdResponseViewModel;
        }


        /// <summary>
        /// Get farmer images by village id.
        /// </summary>
        /// <param name="farmerReqViewModel">Request DTO.</param>
        /// <returns>Returns list of objects of GetFarmerByFarmerIdResVM.</returns>
        public List<GetFarmerByFarmerIdResVM> GetFarmerByVillageId(FarmerReqViewModel farmerReqViewModel)
        {
            List<GetFarmerByFarmerIdResVM> getFarmerByFarmerIdResVMs = new List<GetFarmerByFarmerIdResVM>();
            Client client = _unitOfWork.ClientRepository.Get(x => x.UserName == farmerReqViewModel.ClientUserName && x.Password == farmerReqViewModel.ClientPassword && x.IsActive == true);
            if (client == null) throw new Exception("Invalid Client");

            List<Farmer> farmers = _unitOfWork.FarmerRepository.GetMany(x => x.VillageId == farmerReqViewModel.VillageId && x.ClientId == client.ClientId && x.IsActive == true).ToList();
            if (farmers == null) throw new Exception("Invalid Farmer");
            foreach (Farmer farmerDB in farmers)
            {
                GetFarmerByFarmerIdResVM getFarmerByFarmerIdResponseViewModel = new GetFarmerByFarmerIdResVM()
                {
                    FarmerId = farmerDB.FarmerId,
                    FarmerImages = FarmerImagesDBTOUser(farmerDB.FarmerImages.ToList())
                };
                getFarmerByFarmerIdResVMs.Add(getFarmerByFarmerIdResponseViewModel);
            }
            return getFarmerByFarmerIdResVMs;
        }

        /// <summary>
        /// To save farmer.
        /// </summary>
        /// <param name="farmerReqViewModel">Request DTO.</param>
        /// <returns>Return true on success, otherwise false.</returns>
        public bool SaveFarmer(FarmerReqViewModel farmerReqViewModel)
        {
            Client client = _unitOfWork.ClientRepository.Get(x => x.UserName == farmerReqViewModel.ClientUserName && x.Password == farmerReqViewModel.ClientPassword && x.IsActive == true);
            if (client == null) throw new Exception("Invalid Client");

            Farmer farmer = _unitOfWork.FarmerRepository.Get(x => x.Email == farmerReqViewModel.Email && x.ClientId == client.ClientId && x.IsActive == true);
            if (farmer != null) throw new Exception("Farmer Already Exist");
            List<FarmerImage> farmerImages = new List<FarmerImage>();
           

            Farmer farmerdb = new Farmer()
            {
                FirstName = farmerReqViewModel.FirstName,
                Landline = farmerReqViewModel.Landline,
                Email = farmerReqViewModel.Email,
                ClientId = client.ClientId,
                GenderId = farmerReqViewModel.GenderId,
                LastName = farmerReqViewModel.LastName,
                VillageId = farmerReqViewModel.VillageId,
                Mobile = farmerReqViewModel.Mobile,
                Latitude = farmerReqViewModel.Latitude,
                Longitude = farmerReqViewModel.Longitude,
                IsActive = true,
                CreatedOn = System.DateTime.UtcNow,
                FarmerImages = FarmerImagesToDBModel(farmerReqViewModel.Images)
            };
            _unitOfWork.FarmerRepository.Insert(farmerdb);
            _unitOfWork.Save();
            _unitOfWork.Dispose();
            return true;
        }

        private List<FarmerImage> FarmerImagesToDBModel(string images)
        {
            List<FarmerImage> farmerImages = new List<FarmerImage>();
            List<string> imagesList = new List<string>();
            if (images != null && images !="")
                imagesList = images.Split(',').ToList();

            foreach(string image in imagesList)
            {
                FarmerImage farmerImage = new FarmerImage()
                {
                    ImagePath=image,
                    IsActive=true,
                    CreatedOn=System.DateTime.UtcNow
                };
                farmerImages.Add(farmerImage);
            }
            return farmerImages;
        }


        private List<string> FarmerImagesDBTOUser(List<FarmerImage> farmerImages)
        {
            List<string> images = new List<string>();
            foreach(FarmerImage im in farmerImages)
            {
                images.Add(DigitalGreen.Core.Helper.Constants.Url.WebApiUrlWithoutSlash + "/Uploads/UploadFarmerImages/" + im.ImagePath);
            };
            return images;
        }
    }
}
