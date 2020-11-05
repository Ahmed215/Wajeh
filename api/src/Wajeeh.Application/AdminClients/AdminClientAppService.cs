using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Linq.Extensions;
using System.Linq;
using Wajeeh.AdminClients.Dto;
using Wajeeh.Clinets;
using Wajeeh.OfferPrices;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Wajeeh.Authorization.Users;
using Abp.Runtime.Session;
using System.Diagnostics;
using System.Threading.Tasks;
using Wajeeh.Users;
using Abp.Authorization.Users;
using Wajeeh.MultiTenancy;
using Abp.Authorization;
using Wajeeh.Authorization;
using Microsoft.AspNetCore.Identity;
using Abp.Extensions;

namespace Wajeeh.AdminClients
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminClientAppService : AsyncCrudAppService<Client, AdminClientDto, long, AdminPagedClientResultRequestDto, AdminCreateClientDto, AdminUpdateClientDto>, IAdminClientAppService
    {
        private readonly IRepository<Client, long> _repository;
        private readonly IRepository<OfferPrice, long> _offerPriceRepository;
        private readonly IRepository<User, long> _userRepository;
        private IWebHostEnvironment _webHostingEnvironment;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly LogInManager _logInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AdminClientAppService(IRepository<Client, long> repository,
            UserManager userManager,
            IPasswordHasher<User> passwordHasher,
            LogInManager logInManager,
         AbpLoginResultTypeHelper abpLoginResultTypeHelper,
        IUserAppService userAppService,
            IRepository<OfferPrice, long> offerPriceRepository, IWebHostEnvironment webHostingEnvironment,
            IRepository<User, long> userRepository) : base(repository)
        {
            _logInManager = logInManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _repository = repository;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _userAppService = userAppService;
            _offerPriceRepository = offerPriceRepository;
            _webHostingEnvironment = webHostingEnvironment;
            _userManager = userManager;
        }


        protected override IQueryable<Client> CreateFilteredQuery(AdminPagedClientResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);
            if (!input.Keyword.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.Keyword);
                    string FirstName = filter_query["FirstName"];
                    query = query.WhereIf(!FirstName.IsNullOrEmpty(), t => t.FirstName.Contains(FirstName));
                    string Email = filter_query["Email"];
                    query = query.WhereIf(!Email.IsNullOrEmpty(), t => t.Email.Contains(Email));
                    string LastName = filter_query["LastName"];
                    query = query.WhereIf(!LastName.IsNullOrEmpty(), t => t.LastName.Contains(LastName));
                    string UserName = filter_query["UserName"];
                    query = query.WhereIf(!UserName.IsNullOrEmpty(), t => t.User.UserName.Contains(UserName));
                    string Phone = filter_query["Phone"];
                    query = query.WhereIf(!Phone.IsNullOrEmpty(), t => t.User.PhoneNumber.Contains(Phone) || t.User.UserName.Contains(Phone));
                }
                catch
                {
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Keyword), t => t.FirstName.Contains(input.Keyword) || t.Email.Contains(input.Keyword)
                                                            || t.LastName.Contains(input.Keyword)
                                                            || t.User.UserName.Contains(input.Keyword));
                }
            }
            return query;
        }

        protected override IQueryable<Client> ApplySorting(IQueryable<Client> query, AdminPagedClientResultRequestDto input)
        {
            if (input.Sorting != null)
            {
                if (input.Sorting.StartsWith("Phone"))
                {
                    if (input.Sorting.EndsWith("Asc"))
                    {
                        return query.OrderBy(p => p.User.UserName);

                    }
                    else
                    {
                        return query.OrderByDescending(p => p.User.UserName);

                    }
                }

                else
                {
                    return base.ApplySorting(query, input);
                }
            }

            else
            {
                return base.ApplySorting(query, input);
            }
        }
        public async Task<AdminClientDto> CreateProfile(AdminCreateClientDto model)
        {

            var userEntity = await _userManager.FindByEmailAsync(model.Phone);

            if (userEntity == null)
            {
                var ent= await _userAppService.CreateByPhoneAsync(new Users.Dto.CreateUserByPhoneDto()
                {
                    EmailAddress = model.Phone,
                    IsActive = true,
                    Name = model.Phone,
                    Password = model.Phone,
                    Surname = model.Phone,
                    UserName = model.Phone,
                    RoleNames = new string[] { "CLIENT" }
                });


                var client = MapToEntity(model);

                client.UserId = ent.Id;

                var clientEntity = _repository.Insert(client);

                CurrentUnitOfWork.SaveChanges();

                var clientDto = MapToEntityDto(clientEntity);
                clientDto.NotificationsCount = 0;
                return clientDto;

            }
            else
            {
                throw new Exception();
            }






        }

        public AdminClientDto UpdateProfile(AdminUpdateClientDto model)
        {
            //var userEntity = await _userManager.FindByEmailAsync(model.Phone);
            var userid = _repository.GetAll().Where(x => x.Id == model.Id).Select(x => x.UserId).FirstOrDefault();
            var userEntity = _userRepository.GetAll().Where(x => x.Id == userid).FirstOrDefault();



            if (userEntity != null)
            {
                if (userEntity.UserName != model.Phone)
                {
                    if (_userRepository.GetAll().Any(x => x.UserName == model.Phone))
                        throw new Exception();
                }

                userEntity.Password = _passwordHasher.HashPassword(userEntity, model.Phone);
                userEntity.IsActive = true;
                userEntity.Name = model.Phone;
                userEntity.UserName = model.Phone;
                userEntity.Surname = model.Phone;


                _userRepository.Update(userEntity);




                var client = _repository.Get(model.Id);

                MapToEntity(model, client);
                client.UserId = userEntity.Id;

                var clientEntity = _repository.Update(client);
                CurrentUnitOfWork.SaveChanges();

                var clientDto = MapToEntityDto(clientEntity);
                clientDto.NotificationsCount = GetClientNotifCount(client.UserId);
                return clientDto;

            }
            else
            {
                throw new Exception();
            }


        }


        private int GetClientNotifCount(long userId)
        {
            var querycount = _offerPriceRepository.GetAll()
                .Where(r => r.Request.UserRequsetId == userId && r.Request.Status == 2 && r.IsRead != true);

            var count = querycount.Count();

            return count;
        }

        protected override AdminClientDto MapToEntityDto(Client entity)
        {
            var AdminClientDto = base.MapToEntityDto(entity);
            AdminClientDto.Phone = _userRepository.GetAll().Where(x => x.Id == entity.UserId).Select(x => x.UserName).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(AdminClientDto.Picture))
            {
                AdminClientDto.Picture = GetFile(entity.Picture, "ClientPictures");
            }

            return AdminClientDto;
        }

        protected override Client MapToEntity(AdminCreateClientDto createInput)
        {
            var client = base.MapToEntity(createInput);

            if (!string.IsNullOrWhiteSpace(createInput.Picture))
            {
                var temp= SaveFile(createInput.Picture, "ClientPictures");
                if (temp != null)
                    client.Picture = temp;
            }
            client.UserId = AbpSession.UserId.Value;
            return client;
        }

        protected override void MapToEntity(AdminUpdateClientDto updateInput, Client entity)
        {
            if (!string.IsNullOrWhiteSpace(updateInput.Picture))
            {
                var temp= SaveFile(updateInput.Picture, "ClientPictures");
                if (temp != null)
                    updateInput.Picture = temp;
                else
                    updateInput.Picture = entity.Picture;
            }
            else
            {
                updateInput.Picture = entity.Picture;
            }

            base.MapToEntity(updateInput, entity);
        }

        private String SaveFile(string file, string folder)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {

                try
                {
                    var targetDirectory = Path.Combine(_webHostingEnvironment.WebRootPath, folder);
                    Random rand = new Random();
                    var fileName = rand.Next() * 10000 + (DateTime.Now).Ticks + Guid.NewGuid().ToString().Substring(1, 6) + ".Jpg";

                    var savePath = Path.Combine(targetDirectory, fileName);

                    byte[] bytes = Convert.FromBase64String(file);

                    File.WriteAllBytes(savePath, bytes);

                    return fileName;
                }
                catch (Exception e)
                {

                    return null;
                }
            }
            return null;
        }

        private string GetFile(string fileName, string folder)
        {
            return folder + "/" + fileName;
        }


        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }
    }
}
