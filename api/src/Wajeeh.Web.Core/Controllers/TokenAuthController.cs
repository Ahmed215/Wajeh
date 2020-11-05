using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using Wajeeh.Authentication.External;
using Wajeeh.Authentication.JwtBearer;
using Wajeeh.Authorization;
using Wajeeh.Authorization.Users;
using Wajeeh.Models.TokenAuth;
using Wajeeh.MultiTenancy;
using Wajeeh.Users;
using Wajeeh.Clients;
using Wajeeh.Drivers;
using Wajeeh.Clients.Dto;
using Wajeeh.Drivers.Dto;
using Abp.Runtime.Session;
using Abp.Domain.Repositories;
using Wajeeh.OfferPrices;
using Wajeeh.PhoneCodeConfirms;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Wajeeh.AdminCompanyClients.Dto;
using Wajeeh.AdminCompanyClients;
using Wajeeh.CompanyClients;
using Wajeeh.DriverNotifications;
using Microsoft.EntityFrameworkCore;

namespace Wajeeh.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : WajeehControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IClientAppService _clientAppService;
        private readonly IRepository<CompanyClient, long> _companyClientRepository;
        private readonly IDriverAppService _driverAppService;
        private readonly IRepository<OfferPrice, long> _offerPriceRepository;
        private readonly IRepository<PhoneCodeConfirm, long> _phoneCodeConfirmRepository;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<DriverNotification, long> _driverNotification;



        public TokenAuthController(
            IRepository<User, long> userRepository,
            LogInManager logInManager,
            IPasswordHasher<User> passwordHasher,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            UserManager userManager,
            IUserAppService userAppService,
            IClientAppService clientAppService,
            IRepository<CompanyClient, long> companyClientRepository,
            IDriverAppService driverAppService,
            IRepository<OfferPrice, long> offerPriceRepository,
            IRepository<PhoneCodeConfirm, long> phoneCodeConfirmRepository,
           IRepository<DriverNotification, long> driverNotification,
        IConfiguration config)
        {
            _driverNotification = driverNotification;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
            _userAppService = userAppService;
            _clientAppService = clientAppService;
            _companyClientRepository = companyClientRepository;
            _driverAppService = driverAppService;
            _offerPriceRepository = offerPriceRepository;
            _phoneCodeConfirmRepository = phoneCodeConfirmRepository;
            _config = config;
        }


        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id
            };
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> AuthenticateAdmin([FromBody] AuthenticateAdminModel model)
        {
            if (_config["Sms:SendSms"] == "true")
            {
                if (string.IsNullOrWhiteSpace(model.Code))
                    throw new Exception();

                if (!IsPhoneCodeConfirmed(model.Phone, model.Code))
                    throw new Exception();
            }
            try
            {
                var loginResult = await GetLoginResultAsync(
                    model.Phone,
                    model.Phone,
                    GetTenancyNameOrNull()

                );
                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                    UserId = loginResult.User.Id
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ResultCodeSms AuthenticateSendSms([FromBody] AuthenticateByPhoneModel model)
        {
            string code = "123456";

            var result = 3;

            if (!(model.Phone == "123456789" || model.Phone == "987654321" || model.Phone == "531290434" || model.Phone == "512345678"))
            {
                code = (new Random()).Next(0, 999999).ToString("D6");
                result = SendSms(model.Phone, code);
            }

            var phoneWithCode = _phoneCodeConfirmRepository.GetAll().Where(p => p.Phone == model.Phone).FirstOrDefault();

            if (phoneWithCode == null)
            {
                _phoneCodeConfirmRepository.Insert(new PhoneCodeConfirm()
                {
                    Phone = model.Phone,
                    Code = code
                });
            }
            else
            {
                phoneWithCode.Code = code;
                _phoneCodeConfirmRepository.Update(phoneWithCode);
            }

            CurrentUnitOfWork.SaveChanges();

            return new ResultCodeSms()
            {
                resultCode = result
            };
        }


        private int SendSms(string phone, string code)
        {
            using (var client = new HttpClient())
            {
                string url = "http://4sms.com/";
                client.BaseAddress = new Uri(url);
                //HTTP GET
                var responseTask = client.GetAsync("api.php?send_sms&username=" + "0566655829" + "&password=" + "Truegun@2006" + "&numbers=" + phone + "&sender=" + "Wajeehapp" + "&message=" + code);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var res = readTask.Result.Split('-');
                    try
                    {
                        if (int.TryParse(res[0], out int resultcode))
                            return resultcode;
                        else
                            return 0;
                    }
                    catch
                    {
                        return 0;
                    }

                }
            }

            return 0;
        }


        [HttpPost]
        public async Task<AuthenticateClientResultModel> AuthenticateClient([FromBody] AuthenticateByPhoneConfirmModel model)
        {

            if (_config["Sms:SendSms"] == "true")
            {
                if (string.IsNullOrWhiteSpace(model.Code))
                    throw new Exception();

                if (!IsPhoneCodeConfirmed(model.Phone, model.Code))
                    throw new Exception();
            }

            bool isNew = false;
            bool isHaseProfile = false;
            var userEntity = await _userManager.Users.Where(u => u.UserName == model.Phone).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                isNew = true;

                await _userAppService.CreateByPhoneAsync(new Users.Dto.CreateUserByPhoneDto()
                {
                    EmailAddress = model.Phone,
                    IsActive = true,
                    Name = model.Phone,
                    Password = model.Phone,
                    Surname = model.Phone,
                    UserName = model.Phone,
                    RoleNames = new string[] { "CLIENT" }
                });
            }


            var loginResult = await GetLoginResultAsync(
                model.Phone,
                model.Phone,
                GetTenancyNameOrNull()
            );

            ClientDto profile = null;
            if (!isNew)
            {
                isHaseProfile = _clientAppService.IsUserHaseProfile(loginResult.User.Id);
                if (isHaseProfile)
                {
                    profile = _clientAppService.GetByUserId(loginResult.User.Id);
                    profile.NotificationsCount = GetClientNotifCount(loginResult.User.Id);
                }
            }

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));


            return new AuthenticateClientResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
                isNew = isNew,
                IsDriver = false,
                IsHasePrifle = isHaseProfile,
                Profile = profile
            };
        }


        private int GetClientNotifCount(long userId)
        {
            var querycount = _offerPriceRepository.GetAll()
                .Where(r => r.Request.UserRequsetId == userId && r.Request.Status == 2 && r.IsRead != true);

            var count = querycount.Count();

            return count;
        }
        private int GetDriverNotifCount(long userId)
        {
            var querycount = _driverNotification.GetAll()
                .Where(r => r.DriverId == userId && r.IsRead != true);

            var count = querycount.Count();

            return count;
        }
        private bool IsPhoneCodeConfirmed(string phone, string code)
        {
            return _phoneCodeConfirmRepository.GetAll().Where(p => p.Phone.ToLower() == phone.ToLower()
            && p.Code.ToLower() == code.ToLower()).Any();
        }

        //[HttpPost]
        //public async Task<AuthenticateDriverResultModel> AuthenticateDriver([FromBody] AuthenticateByPhoneModel model)
        //{
        //    bool isNew = false;
        //    bool isHaseProfile = false;

        //    var userEntity = await _userManager.FindByEmailAsync(model.Phone);

        //    if (userEntity == null)
        //    {
        //        isNew = true;

        //        await _userAppService.CreateByPhoneAsync(new Users.Dto.CreateUserByPhoneDto()
        //        {
        //            EmailAddress = model.Phone,
        //            IsActive = true,
        //            Name = model.Phone,
        //            Password = model.Phone,
        //            Surname = model.Phone,
        //            UserName = model.Phone,
        //            RoleNames = new string[] { "DRIVER" }
        //        });
        //    }


        //    var loginResult = await GetLoginResultAsync(
        //        model.Phone,
        //        model.Phone,
        //        GetTenancyNameOrNull()
        //    );

        //    DriverDto profile = null;

        //    if (!isNew)
        //    {
        //        isHaseProfile = _driverAppService.IsUserHaseProfile(loginResult.User.Id);
        //        if (isHaseProfile)
        //            profile = _driverAppService.GetByUserId(loginResult.User.Id);
        //    }

        //    var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

        //    return new AuthenticateDriverResultModel
        //    {
        //        AccessToken = accessToken,
        //        EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
        //        ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
        //        UserId = loginResult.User.Id,
        //        isNew = isNew,
        //        IsDriver = true,
        //        IsHasePrifle = isHaseProfile,
        //        Profile = profile
        //    };
        //}


        [HttpPost]
        public async Task<AuthenticateDriverResultModel> AuthenticateDriver([FromBody] AuthenticateByPhoneConfirmModel model)
        {

            if (_config["Sms:SendSms"] == "true")
            {
                if (string.IsNullOrWhiteSpace(model.Code))
                    throw new Exception();

                if (!IsPhoneCodeConfirmed(model.Phone, model.Code))
                    throw new Exception();
            }

            bool isNew = false;
            bool isHaseProfile = false;
            var userEntity = await _userManager.Users.Where(u => u.UserName == model.Phone).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                isNew = true;

                await _userAppService.CreateByPhoneAsync(new Users.Dto.CreateUserByPhoneDto()
                {
                    EmailAddress = model.Phone,
                    IsActive = true,
                    Name = model.Phone,
                    Password = model.Phone,
                    Surname = model.Phone,
                    UserName = model.Phone,
                    RoleNames = new string[] { "DRIVER" }
                });
            }


            var loginResult = await GetLoginResultAsync(
                model.Phone,
                model.Phone,
                GetTenancyNameOrNull()
            );

            DriverDto profile = null;
            if (!isNew)
            {
                isHaseProfile = _driverAppService.IsUserHaseProfile(loginResult.User.Id);
                if (isHaseProfile)
                {
                    profile = _driverAppService.GetByUserId(loginResult.User.Id);
                    profile.NotificationsCount = GetDriverNotifCount(loginResult.User.Id);
                }
            }

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));


            return new AuthenticateDriverResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
                isNew = isNew,
                IsDriver = false,
                IsHasePrifle = isHaseProfile,
                Profile = profile
            };
        }



        [HttpPost]
        public async Task<AdminCompanyClientDto> CreateCompanyClient([FromBody] AdminCreateCompanyClientDto model)
        {
            var userEntity = await _userManager.FindByEmailAsync(model.Phone);

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
                    //RoleNames = new string[] { "COMPANYCLIENY" }
                });



                await _companyClientRepository.InsertAsync(new CompanyClient()
                {
                    CompanyId = model.CompanyId,
                    Email = model.Email,
                    FullName = model.FullName,
                    UserId = ent.Id
                });

            }
            else
            {
                throw new Exception();
            }

            return new AdminCompanyClientDto()
            {
                CompanyId = model.CompanyId,
                Email = model.Email,
                Phone = model.Phone,
                FullName = model.FullName
            };
        }
        [HttpPost]
        public async Task<AdminCompanyClientDto> UpdateCompanyClient([FromBody] AdminUpdateCompanyClientDto model)
        {
            var userid = _companyClientRepository.GetAll().Where(x => x.Id == model.Id).Select(x => x.UserId).FirstOrDefault();
            var userEntity = _userRepository.GetAll().Where(x => x.Id == userid).FirstOrDefault();


            if (userEntity != null)
            {
                userEntity.Password = _passwordHasher.HashPassword(userEntity, model.Phone);

                userEntity.IsActive = true;
                userEntity.Name = model.Phone;
                userEntity.UserName = model.Phone;
                userEntity.Surname = model.Phone;


                _userRepository.Update(userEntity);



                await _companyClientRepository.UpdateAsync(new CompanyClient()
                {
                    Id = model.Id,
                    CompanyId = model.CompanyId,
                    Email = model.Email,
                    FullName = model.FullName,
                    UserId = userEntity.Id
                });

            }
            else
            {
                throw new Exception();
            }

            return new AdminCompanyClientDto()
            {
                CompanyId = model.CompanyId,
                Email = model.Email,
                Phone = model.Phone,
                FullName = model.FullName
            };
        }

        //[HttpGet]
        //public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        //{
        //    return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        //}

        //[HttpPost]
        //public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        //{
        //    var externalUser = await GetExternalUserInfo(model);

        //    var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

        //    switch (loginResult.Result)
        //    {
        //        case AbpLoginResultType.Success:
        //            {
        //                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
        //                return new ExternalAuthenticateResultModel
        //                {
        //                    AccessToken = accessToken,
        //                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
        //                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
        //                };
        //            }
        //        case AbpLoginResultType.UnknownExternalLogin:
        //            {
        //                var newUser = await RegisterExternalUserAsync(externalUser);
        //                if (!newUser.IsActive)
        //                {
        //                    return new ExternalAuthenticateResultModel
        //                    {
        //                        WaitingForActivation = true
        //                    };
        //                }

        //                // Try to login again with newly registered user!
        //                loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
        //                if (loginResult.Result != AbpLoginResultType.Success)
        //                {
        //                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
        //                        loginResult.Result,
        //                        model.ProviderKey,
        //                        GetTenancyNameOrNull()
        //                    );
        //                }

        //                return new ExternalAuthenticateResultModel
        //                {
        //                    AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
        //                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
        //                };
        //            }
        //        default:
        //            {
        //                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
        //                    loginResult.Result,
        //                    model.ProviderKey,
        //                    GetTenancyNameOrNull()
        //                );
        //            }
        //    }
        //}

        //private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        //{
        //    var user = await _userRegistrationManager.RegisterAsync(
        //        externalUser.Name,
        //        externalUser.Surname,
        //        externalUser.EmailAddress,
        //        externalUser.EmailAddress,
        //        Authorization.Users.User.CreateRandomPassword(),
        //        true
        //    );

        //    user.Logins = new List<UserLogin>
        //    {
        //        new UserLogin
        //        {
        //            LoginProvider = externalUser.Provider,
        //            ProviderKey = externalUser.ProviderKey,
        //            TenantId = user.TenantId
        //        }
        //    };

        //    await CurrentUnitOfWork.SaveChangesAsync();

        //    return user;
        //}

        //private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        //{
        //    var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
        //    if (userInfo.ProviderKey != model.ProviderKey)
        //    {
        //        throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
        //    }

        //    return userInfo;
        //}

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
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

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }
    }
}
