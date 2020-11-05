using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wajeeh.Authorization;
using Wajeeh.Authorization.Users;
using Wajeeh.Clients.Dto;
using Wajeeh.Clinets;
using Wajeeh.CompanyClients;
using Wajeeh.Drivers;
using Wajeeh.OfferPrices;

namespace Wajeeh.Clients
{
    //[AbpAuthorize(PermissionNames.Pages_Clients)]
    public class ClientAppService : AsyncCrudAppService<Client, ClientDto, long, PagedClientResultRequestDto, CreateClientDto, UpdateClientDto>, IClientAppService
    {
        private readonly IRepository<Client, long> _repository;
        private readonly IRepository<OfferPrice, long> _offerPriceRepository;
        private readonly IRepository<Driver, long> _driverRepository;
        private readonly IRepository<CompanyClient, long> _companyClientRepository;
        private IWebHostEnvironment _webHostingEnvironment;
        public ClientAppService(IRepository<Client, long> repository,
            IWebHostEnvironment webHostingEnvironment,
            IRepository<Driver, long> driverRepository,
            IRepository<CompanyClient, long> companyClientRepository,
            IRepository<OfferPrice, long> offerPriceRepository) : base(repository)
        {
            _companyClientRepository = companyClientRepository;
            _driverRepository = driverRepository;
            _repository = repository;
            _offerPriceRepository = offerPriceRepository;
            _webHostingEnvironment = webHostingEnvironment;
        }




        public ClientDto GetProfile()
        {
            var userId = AbpSession.GetUserId();
            var clientEntity = _repository.GetAll()
                .Where(x => x.UserId == userId).FirstOrDefault();
            var clientDto = MapToEntityDto(clientEntity);
            clientDto.NotificationsCount = GetClientNotifCount(userId);
            return clientDto;
        }


        public ClientDto CreateProfile(CreateClientDto model)
        {
            try
            {
                var userId = AbpSession.GetUserId();
                if (IsUserHaseProfile(userId) || IsHasDriverProfile(userId) || IsHasClientCompanyProfile(userId))
                {
                    var _TempClient = _repository.GetAll().Where(x => x.UserId == userId).FirstOrDefault();
                    if (_TempClient != null)
                        return MapToEntityDto(_TempClient);
                }
                var client = MapToEntity(model);

                var clientEntity = _repository.Insert(client);

                CurrentUnitOfWork.SaveChanges();

                var clientDto = MapToEntityDto(clientEntity);
                clientDto.NotificationsCount = GetClientNotifCount(userId);
                return clientDto;
            }
            catch (Exception ex)
            {
                return new ClientDto { FirstName = ex.Message };
            }
        }

        public ClientDto UpdateProfile(UpdateClientDto model)
        {
            var userId = AbpSession.GetUserId();
            var clientId = _repository.GetAll().Where(c => c.UserId == userId).FirstOrDefault().Id;
            if (model.Id != clientId)
                throw new UnauthorizedAccessException("Unothorized");
            var client = _repository.Get(model.Id);

            MapToEntity(model, client);

            var clientEntity = _repository.Update(client);
            CurrentUnitOfWork.SaveChanges();

            var clientDto = MapToEntityDto(clientEntity);
            clientDto.NotificationsCount = GetClientNotifCount(userId);
            return clientDto;
        }

        private int GetClientNotifCount(long userId)
        {
            var querycount = _offerPriceRepository.GetAll()
                .Where(r => r.Request.UserRequsetId == userId && r.Request.Status == 2 && r.IsRead != true);

            var count = querycount.Count();

            return count;
        }

        [RemoteService(IsMetadataEnabled = false)]
        public bool IsUserHaseProfile(long userId)
        {
            return _repository.GetAll().Any(x => x.UserId == userId);
        }

        public bool IsHasDriverProfile(long userId)
        {
            return _driverRepository.GetAll().Any(x => x.UserId == userId);
        }

        public bool IsHasClientCompanyProfile(long userId)
        {
            return _companyClientRepository.GetAll().Any(x => x.UserId == userId);
        }

        [RemoteService(IsMetadataEnabled = false)]
        public ClientDto GetByUserId(long userId)
        {
            var client = _repository.GetAll().Where(c => c.UserId == userId).FirstOrDefault();
            return MapToEntityDto(client);
        }

        protected override Client MapToEntity(CreateClientDto createInput)
        {
            var client = base.MapToEntity(createInput);

            if (!string.IsNullOrWhiteSpace(createInput.Picture))
            {
                client.Picture = SaveFile(createInput.Picture, "ClientPictures");
            }
            client.UserId = AbpSession.UserId.Value;
            return client;
        }

        protected override void MapToEntity(UpdateClientDto updateInput, Client entity)
        {
            if (!string.IsNullOrWhiteSpace(updateInput.Picture))
            {
                updateInput.Picture = SaveFile(updateInput.Picture, "ClientPictures");
            }
            else
            {
                updateInput.Picture = entity.Picture;
            }

            base.MapToEntity(updateInput, entity);
        }

        protected override ClientDto MapToEntityDto(Client entity)
        {
            var clientDto = base.MapToEntityDto(entity);
            if (!string.IsNullOrWhiteSpace(clientDto.Picture))
            {
                clientDto.Picture = GetFile(entity.Picture, "ClientPictures");
            }
            return clientDto;
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
    }
}
