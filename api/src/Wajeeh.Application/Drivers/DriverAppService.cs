using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Wajeeh.Drivers.Dto;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wajeeh.Authorization;
using Abp.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Abp.Runtime.Session;
using Wajeeh.DriverNotifications;
using Wajeeh.Clinets;
using Wajeeh.CompanyClients;

namespace Wajeeh.Drivers
{
    //[AbpAuthorize(PermissionNames.Pages_Drivers)]
    public class DriverAppService : AsyncCrudAppService<Driver, DriverDto, long, PagedDriverResultRequestDto, CreateDriverDto, UpdateDriverDto>, IDriverAppService
    {
        private readonly IRepository<Driver, long> _repository;
        private readonly IRepository<Client, long> _clientRepository;
        private readonly IRepository<CompanyClient, long> _companyClientRepository;
        private IWebHostEnvironment _webHostingEnvironment;
        private IRepository<DriverNotification, long> _driverNotification;
        public DriverAppService(IRepository<Driver, long> repository,
            IRepository<Client, long> clientRepository,
            IRepository<CompanyClient, long> companyClientRepository,
            IRepository<DriverNotification, long> driverNotification,
            IWebHostEnvironment webHostingEnvironment) : base(repository)
        {
            _repository = repository;
            _clientRepository = clientRepository;
            _companyClientRepository = companyClientRepository;
            _webHostingEnvironment = webHostingEnvironment;
            _driverNotification = driverNotification;
        }

        public DriverDto GetProfile()
        {

            var userId = AbpSession.GetUserId();
            var driverEntity = _repository.GetAll()
                .Where(x => x.UserId == userId).FirstOrDefault();
            var driverDto = MapToEntityDto(driverEntity);
            driverDto.NotificationsCount = GetDriverNotifCount(userId);
            return driverDto;
        }

        public DriverDto CreateProfile(CreateDriverDto model)
        {
            var userId = AbpSession.GetUserId();
            if (IsUserHaseProfile(userId) || IsHasClientProfile(userId) || IsHasCompanyClientProfile(userId))
                throw new Exception();
            var driver = MapToEntity(model);
            driver.IsDriverAvilable = true;
            var driverEntity = _repository.Insert(driver);

            CurrentUnitOfWork.SaveChanges();

            var driverDto = MapToEntityDto(driverEntity);
            //driverDto.NotificationsCount = GetClientNotifCount(userId);
            return driverDto;
        }

        public DriverDto UpdateProfile(UpdateDriverDto model)
        {
            var userId = AbpSession.GetUserId();
            var driverId = _repository.GetAll().Where(c => c.UserId == userId).FirstOrDefault().Id;
            if (model.Id != driverId)
                throw new UnauthorizedAccessException("Unothorized");
            var driver = _repository.Get(model.Id);

            MapToEntity(model, driver);

            var clientEntity = _repository.Update(driver);
            CurrentUnitOfWork.SaveChanges();

            var driverDto = MapToEntityDto(clientEntity);
            driverDto.NotificationsCount = GetDriverNotifCount(userId);
            return driverDto;
        }

        [RemoteService(IsMetadataEnabled = false)]
        public bool IsUserHaseProfile(long userId)
        {
            return _repository.GetAll().Any(x => x.UserId == userId);
        }
        public bool IsHasClientProfile(long userId)
        {
            return _clientRepository.GetAll().Any(x => x.UserId == userId);
        }

        public bool IsHasCompanyClientProfile(long userId)
        {
            return _companyClientRepository.GetAll().Any(x => x.UserId == userId);
        }

        [RemoteService(IsMetadataEnabled = false)]
        public DriverDto GetByUserId(long userId)
        {
            var driver = _repository.GetAll().Where(c => c.UserId == userId).FirstOrDefault();
            return MapToEntityDto(driver);
        }


        protected override Driver MapToEntity(CreateDriverDto createInput)
        {
            var driver = base.MapToEntity(createInput);

            if (!string.IsNullOrWhiteSpace(createInput.ProfilePicture))
            {
                driver.ProfilePicture = SaveFile(createInput.ProfilePicture, "DriverPictures");
            }

            if (!string.IsNullOrWhiteSpace(createInput.IdentityPicture))
            {
                driver.IdentityPicture = SaveFile(createInput.IdentityPicture, "DriverPictures");
            }

            if (!string.IsNullOrWhiteSpace(createInput.LisencePicture))
            {
                driver.LisencePicture = SaveFile(createInput.LisencePicture, "DriverPictures");
            }

            if (!string.IsNullOrWhiteSpace(createInput.VehicleLisencePicture))
            {
                driver.VehicleLisencePicture = SaveFile(createInput.VehicleLisencePicture, "DriverPictures");
            }

            if (!string.IsNullOrWhiteSpace(createInput.FrontVehiclePicture))
            {
                driver.FrontVehiclePicture = SaveFile(createInput.FrontVehiclePicture, "DriverPictures");
            }

            if (!string.IsNullOrWhiteSpace(createInput.BackVehiclePicture))
            {
                driver.BackVehiclePicture = SaveFile(createInput.BackVehiclePicture, "DriverPictures");
            }
            driver.UserId = AbpSession.UserId.Value;
            return driver;
        }

        protected override void MapToEntity(UpdateDriverDto updateInput, Driver entity)
        {
            if (!string.IsNullOrWhiteSpace(updateInput.ProfilePicture))
            {
                updateInput.ProfilePicture = SaveFile(updateInput.ProfilePicture, "DriverPictures");
            }
            else
            {
                updateInput.ProfilePicture = entity.ProfilePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.IdentityPicture))
            {
                updateInput.IdentityPicture = SaveFile(updateInput.IdentityPicture, "DriverPictures");
            }
            else
            {
                updateInput.IdentityPicture = entity.IdentityPicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.LisencePicture))
            {
                updateInput.LisencePicture = SaveFile(updateInput.LisencePicture, "DriverPictures");
            }
            else
            {
                updateInput.LisencePicture = entity.LisencePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.VehicleLisencePicture))
            {
                updateInput.VehicleLisencePicture = SaveFile(updateInput.VehicleLisencePicture, "DriverPictures");
            }
            else
            {
                updateInput.VehicleLisencePicture = entity.VehicleLisencePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.FrontVehiclePicture))
            {
                updateInput.FrontVehiclePicture = SaveFile(updateInput.FrontVehiclePicture, "DriverPictures");
            }
            else
            {
                updateInput.FrontVehiclePicture = entity.FrontVehiclePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.BackVehiclePicture))
            {
                updateInput.BackVehiclePicture = SaveFile(updateInput.BackVehiclePicture, "DriverPictures");
            }
            else
            {
                updateInput.BackVehiclePicture = entity.BackVehiclePicture;
            }


            base.MapToEntity(updateInput, entity);
        }

        protected override DriverDto MapToEntityDto(Driver entity)
        {
            var driverDto = base.MapToEntityDto(entity);
            if (!string.IsNullOrWhiteSpace(driverDto.ProfilePicture))
            {
                driverDto.ProfilePicture = GetFile(entity.ProfilePicture, "DriverPictures");
            }
            if (!string.IsNullOrWhiteSpace(driverDto.IdentityPicture))
            {
                driverDto.IdentityPicture = GetFile(entity.IdentityPicture, "DriverPictures");
            }
            if (!string.IsNullOrWhiteSpace(driverDto.LisencePicture))
            {
                driverDto.LisencePicture = GetFile(entity.LisencePicture, "DriverPictures");
            }
            if (!string.IsNullOrWhiteSpace(driverDto.VehicleLisencePicture))
            {
                driverDto.VehicleLisencePicture = GetFile(entity.VehicleLisencePicture, "DriverPictures");
            }
            if (!string.IsNullOrWhiteSpace(driverDto.FrontVehiclePicture))
            {
                driverDto.FrontVehiclePicture = GetFile(entity.FrontVehiclePicture, "DriverPictures");
            }
            if (!string.IsNullOrWhiteSpace(driverDto.BackVehiclePicture))
            {
                driverDto.BackVehiclePicture = GetFile(entity.BackVehiclePicture, "DriverPictures");
            }
            return driverDto;
        }
        private String SaveFile(string file, string folder)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {

                var targetDirectory = Path.Combine(_webHostingEnvironment.WebRootPath, folder);
                Random rand = new Random();
                var fileName = rand.Next() * 10000 + (DateTime.Now).Ticks + Guid.NewGuid().ToString().Substring(1, 6) + ".Jpg";

                var savePath = Path.Combine(targetDirectory, fileName);

                byte[] bytes = Convert.FromBase64String(file);

                File.WriteAllBytes(savePath, bytes);

                return fileName;
            }
            return null;
        }
        private string GetFile(string fileName, string folder)
        {
            return folder + "/" + fileName;
        }
        private int GetDriverNotifCount(long userId)
        {
            var querycount = _driverNotification.GetAll()
                .Where(r => r.DriverId == userId && r.IsRead != true);
            var count = querycount.Count();
            return count;
        }

        public bool SetDriverOffDuty(long userId, bool offDuty)
        {
            var Updated = false;
            var driverProfile = _repository.GetAll().Where(d => d.UserId == userId).FirstOrDefault();
            if (driverProfile != null)
            {
                driverProfile.OffDuty = offDuty;
                _repository.Update(driverProfile);
                CurrentUnitOfWork.SaveChanges();
                Updated = true;
            }
            return Updated;
        }
    }
}
