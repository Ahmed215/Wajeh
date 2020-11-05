using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using Abp.Linq.Extensions;
using System.Linq;
using System.Text;
using Wajeeh.AdminDrivers.Dto;
using Wajeeh.Drivers;
using Wajeeh.Authorization.Users;
using Wajeeh.Wasel.models;
using Wajeeh.Wasel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Abp.Authorization.Users;
using Wajeeh.MultiTenancy;
using Abp.Authorization;
using Wajeeh.Users;
using Wajeeh.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Wajeeh.NotificationTokens;
using Wajeeh.NotificationCenters;
using Wajeeh.DriverNotifications;
using Microsoft.EntityFrameworkCore;
using Abp.Extensions;
using Wajeeh.AdminWaselDrivers.Dto;
using Wajeeh.WaselDrivers;

namespace Wajeeh.AdminDrivers
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminDriverAppService : AsyncCrudAppService<Driver, AdminDriverDto, long, AdminPagedDriverResultRequestDto, AdminCreateDriverDto, AdminUpdateDriverDto>, IAdminDriverAppService
    {
        private IWebHostEnvironment _webHostingEnvironment;
        private readonly IRepository<User, long> _userRepository;
        private readonly IWaselService _waselService;
        IRepository<Driver, long> _repository;
        private readonly IRepository<WaselDriver, long> _WaselDriverRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<NotificationToken, long> _notificationTokenRepository;
        private readonly INotificationCenter _notificationCenter;
        private readonly IRepository<DriverNotification, long> _driverNotification;


        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly LogInManager _logInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;


        public AdminDriverAppService(IRepository<Driver, long> repository,
            IRepository<NotificationToken, long> notificationTokenRepository,
            UserManager userManager,
            IPasswordHasher<User> passwordHasher,
            LogInManager logInManager,
         AbpLoginResultTypeHelper abpLoginResultTypeHelper,
        IUserAppService userAppService,
            IRepository<User, long> userRepository,
            IRepository<DriverNotification, long> driverNotification,
            IWebHostEnvironment webHostingEnvironment,
            INotificationCenter notificationCenter,
            IWaselService waselService,
            IRepository<WaselDriver, long> WaselDriverRepository) : base(repository)
        {
            _driverNotification = driverNotification;
            _userRepository = userRepository;
            _notificationCenter = notificationCenter;
            _waselService = waselService;
            _notificationTokenRepository = notificationTokenRepository;
            _passwordHasher = passwordHasher;
            _repository = repository;
            _webHostingEnvironment = webHostingEnvironment;
            _logInManager = logInManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userAppService = userAppService;
            _userManager = userManager;
            _WaselDriverRepository = WaselDriverRepository;

            LocalizationSourceName = WajeehConsts.LocalizationSourceName;
        }

        protected override IQueryable<Driver> ApplySorting(IQueryable<Driver> query, AdminPagedDriverResultRequestDto input)
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
        protected override IQueryable<Driver> CreateFilteredQuery(AdminPagedDriverResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);
            if (!input.Keyword.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.Keyword);
                    string phone = filter_query["Phone"];
                    query = query.WhereIf(!phone.IsNullOrEmpty(), t => t.User.PhoneNumber.Contains(phone));

                    string email = filter_query["Email"];
                    query = query.WhereIf(!email.IsNullOrEmpty(), t => t.Email.Contains(email));
                    string fullName = filter_query["FullName"];
                    query = query.WhereIf(!fullName.IsNullOrEmpty(), t => t.FullName.Contains(fullName));
                    string userName = filter_query["UserName"];
                    query = query.WhereIf(!userName.IsNullOrEmpty(), t => t.User.UserName.Contains(userName));
                }
                catch
                {
                    query = query.Where(t => t.FullName.Contains(input.Keyword) || t.Email.Contains(input.Keyword) || t.User.UserName.Contains(input.Keyword));
                }
            }
            query = query.WhereIf(input.CompanyId.HasValue && input.CompanyId.Value > 0, x => x.CompanyId == input.CompanyId);
            query = query.WhereIf(input.IsAvilible.HasValue, x => x.IsDriverAvilable == input.IsAvilible);
            query = query.WhereIf(input.SubcategoryId.HasValue && input.SubcategoryId.Value > 0, x => x.VehicleType == input.SubcategoryId);
            return query;
        }


        public async Task<string> RegisterDriverAsync(AdminCreateWaselDriverDto input)
        {
            var waseldriver = _waselService.RegDriver(new Wasel.models.RegDriverVM()
            {
                dateOfBirthGregorian = input.dateOfBirthGregorian,
                dateOfBirthHijri = input.dateOfBirthHijri,
                email = input.email,
                identityNumber = input.identityNumber,
                mobileNumber = input.mobileNumber
            });
            if (waseldriver.success)
            {
                WaselDriver waselDriver = new WaselDriver()
                {
                    dateOfBirthGregorian = input.dateOfBirthGregorian,
                    //not found 
                    //dateOfBirthHijri = input.dateOfBirthHijri,
                    email = input.email,
                    identityNumber = input.identityNumber,
                    mobileNumber = input.mobileNumber
                };
                await _WaselDriverRepository.InsertAsync(waselDriver);
                await CurrentUnitOfWork.SaveChangesAsync();

            }
            return waseldriver.resultCode;
        }

        //public string RegisterDriver(RegDriverVM model)
        //{
        //    var code= _waselService.RegDriver(model);
        //    return L(code);
        //}

        public async Task<AdminDriverDto> CreateProfile(AdminCreateDriverDto model)
        {

            var userEntity = await _userManager.Users.Where(x => x.UserName == model.Phone).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                var ent = await _userAppService.CreateByPhoneAsync(new Users.Dto.CreateUserByPhoneDto()
                {
                    EmailAddress = model.Phone,
                    IsActive = true,
                    Name = model.Phone,
                    Password = model.Phone,
                    Surname = model.Phone,
                    UserName = model.Phone,
                    RoleNames = new string[] { "Driver" }
                });
                var driver = MapToEntity(model);
                driver.UserId = ent.Id;
                var driverEntity = _repository.Insert(driver);
                CurrentUnitOfWork.SaveChanges();
                var driverDto = MapToEntityDto(driverEntity);
                return driverDto;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<AdminDriverDto> UpdateProfile(AdminUpdateDriverDto model)
        {
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


                var driver = _repository.Get(model.Id);
                if (driver == null)
                    throw new Exception();

                MapToEntity(model, driver);
                driver.UserId = userEntity.Id;
                var driverEntity = _repository.Update(driver);


                var driverDto = MapToEntityDto(driverEntity);


                if (model.IsDriverAvilable == true && driver.IsDriverAvilable != true)
                {
                    var usersTokens = _notificationTokenRepository.GetAll()
                    .Where(n => model.Id == n.User.Driver.Id)
                    .Select(n => n.Token).ToArray();


                    string title = "تم تفعيلك";
                    string body = "لقد تم تفعيلك يمكنك الان متابعة الطلبات الجديدة";

                    var not = _driverNotification.Insert(new DriverNotification()
                    {
                        DriverId = userid,
                        IsRead = false,
                        Title = title,
                        Body = body,
                        Type = 2
                    });


                    await _notificationCenter.SendPushNotification(usersTokens, title, body, new
                    {
                        NotId = not.Id,
                        Type = 2
                    });
                }

                CurrentUnitOfWork.SaveChanges();

                return driverDto;

            }
            else
            {
                throw new Exception();
            }


        }

        [RemoteService(IsMetadataEnabled = false)]
        public bool IsUserHaseProfile(long userId)
        {
            return _repository.GetAll().Any(x => x.UserId == userId);
        }

        [RemoteService(IsMetadataEnabled = false)]
        public AdminDriverDto GetByUserId(long userId)
        {
            var driver = _repository.GetAll().Where(c => c.UserId == userId).FirstOrDefault();
            return MapToEntityDto(driver);
        }

        protected override Driver MapToEntity(AdminCreateDriverDto createInput)
        {
            var driver = base.MapToEntity(createInput);

            if (!string.IsNullOrWhiteSpace(createInput.ProfilePicture))
            {
                var temp = SaveFile(createInput.ProfilePicture, "DriverPictures");
                if (temp != null)
                    driver.ProfilePicture = temp;
            }

            if (!string.IsNullOrWhiteSpace(createInput.IdentityPicture))
            {
                var temp = SaveFile(createInput.IdentityPicture, "DriverPictures");
                if (temp != null)
                    driver.IdentityPicture = temp;
            }

            if (!string.IsNullOrWhiteSpace(createInput.LisencePicture))
            {
                var temp = SaveFile(createInput.LisencePicture, "DriverPictures");
                if (temp != null)
                    driver.LisencePicture = temp;
            }

            if (!string.IsNullOrWhiteSpace(createInput.VehicleLisencePicture))
            {
                var temp = SaveFile(createInput.VehicleLisencePicture, "DriverPictures");
                if (temp != null)
                    driver.VehicleLisencePicture = temp;
            }

            if (!string.IsNullOrWhiteSpace(createInput.FrontVehiclePicture))
            {
                var temp = SaveFile(createInput.FrontVehiclePicture, "DriverPictures");
                if (temp != null)
                    driver.FrontVehiclePicture = temp;
            }

            if (!string.IsNullOrWhiteSpace(createInput.BackVehiclePicture))
            {
                var temp = SaveFile(createInput.BackVehiclePicture, "DriverPictures");
                if (temp != null)
                    driver.BackVehiclePicture = temp;
            }
            driver.UserId = AbpSession.UserId.Value;
            return driver;
        }

        protected override void MapToEntity(AdminUpdateDriverDto updateInput, Driver entity)
        {
            if (!string.IsNullOrWhiteSpace(updateInput.ProfilePicture))
            {
                var temp = SaveFile(updateInput.ProfilePicture, "DriverPictures");
                if (temp != null)
                    updateInput.ProfilePicture = temp;
                else
                    updateInput.ProfilePicture = entity.ProfilePicture;
            }
            else
            {
                updateInput.ProfilePicture = entity.ProfilePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.IdentityPicture))
            {
                var temp = SaveFile(updateInput.IdentityPicture, "DriverPictures");
                if (temp != null)
                    updateInput.IdentityPicture = temp;
                else
                    updateInput.IdentityPicture = entity.IdentityPicture;
            }
            else
            {
                updateInput.IdentityPicture = entity.IdentityPicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.LisencePicture))
            {
                var temp = SaveFile(updateInput.LisencePicture, "DriverPictures");
                if (temp != null)
                    updateInput.LisencePicture = temp;
                else
                    updateInput.LisencePicture = entity.LisencePicture;
            }
            else
            {
                updateInput.LisencePicture = entity.LisencePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.VehicleLisencePicture))
            {
                var temp = SaveFile(updateInput.VehicleLisencePicture, "DriverPictures");
                if (temp != null)
                    updateInput.VehicleLisencePicture = temp;
                else
                    updateInput.VehicleLisencePicture = entity.VehicleLisencePicture;
            }
            else
            {
                updateInput.VehicleLisencePicture = entity.VehicleLisencePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.FrontVehiclePicture))
            {
                var temp = SaveFile(updateInput.FrontVehiclePicture, "DriverPictures");
                if (temp != null)
                    updateInput.FrontVehiclePicture = temp;
                else
                    updateInput.FrontVehiclePicture = entity.FrontVehiclePicture;
            }
            else
            {
                updateInput.FrontVehiclePicture = entity.FrontVehiclePicture;
            }


            if (!string.IsNullOrWhiteSpace(updateInput.BackVehiclePicture))
            {
                var temp = SaveFile(updateInput.BackVehiclePicture, "DriverPictures");
                if (temp != null)
                    updateInput.BackVehiclePicture = temp;
                else
                    updateInput.BackVehiclePicture = entity.BackVehiclePicture;
            }
            else
            {
                updateInput.BackVehiclePicture = entity.BackVehiclePicture;
            }


            base.MapToEntity(updateInput, entity);
        }

        protected override AdminDriverDto MapToEntityDto(Driver entity)
        {
            var driverDto = base.MapToEntityDto(entity);
            driverDto.Phone = _userRepository.GetAll().Where(x => x.Id == entity.UserId).Select(x => x.UserName).FirstOrDefault();
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

        private string SaveFile(string file, string folder)
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
                catch (Exception)
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
