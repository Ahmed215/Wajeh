using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Wajeeh.Authorization;
using Wajeeh.Authorization.Accounts;
using Wajeeh.Authorization.Roles;
using Wajeeh.Authorization.Users;
using Wajeeh.Roles.Dto;
using Wajeeh.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wajeeh.Messages;
using System;
using Wajeeh.DriverNotifications;
using Wajeeh.NotificationTokens;
using Wajeeh.NotificationCenters;
using Abp.Notifications;
using Abp;
using Abp.Authorization.Users;
using Wajeeh.ChatRooms;

namespace Wajeeh.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IRepository<Message, long> _messageRepository;
        private readonly IRepository<User, long> _userRepository;
        private IRepository<DriverNotification, long> _driverNotification;
        private readonly IRepository<NotificationToken, long> _notificationTokenRepository;
        private readonly INotificationCenter _notificationCenter;

        public UserAppService(
            INotificationPublisher notificationPublisher,
            IRepository<DriverNotification, long> driverNotification,
            IRepository<NotificationToken, long> notificationTokenRepository,
            INotificationCenter notificationCenter,
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            IRepository<Message, long> messageRepository,
            IRepository<User, long> userRepository,
            LogInManager logInManager)
            : base(repository)
        {
            _notificationPublisher = notificationPublisher;
            _notificationCenter = notificationCenter;
            _notificationTokenRepository = notificationTokenRepository;
            _driverNotification = driverNotification;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _logInManager = logInManager;
        }


        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }
        public List<UserDto> AdminGetMessageUsers()
        {
            var userCreatorIds = _messageRepository.GetAll().OrderByDescending(x => x.CreationTime).ThenBy(x => x.CreatorUserId)
                .Select(x => x.CreatorUserId).Distinct().ToList();


            var adminRole = _roleManager.GetRoleByName("admin");

            if (adminRole != null)
            {
                var adminUsersId = _userRepository.GetAll().Where(u => u.Roles.Any(r => r.RoleId == adminRole.Id)).Select(x => x.Id).ToList();

                var userListIds = new List<long>();
                foreach (var item in userCreatorIds)
                {
                    if (item.HasValue && !adminUsersId.Contains(item.Value))
                        userListIds.Add(item.Value);
                }
                var clientUser = _userRepository.GetAll().Where(x => userListIds.Contains(x.Id))
                    .Select(x => new UserDto()
                    {
                        Id = x.Id,
                        EmailAddress = x.EmailAddress,
                        FullName = x.FullName,
                        Name = x.Name,
                        Surname = x.Surname,
                        UserName = x.UserName,
                        CreationTime = DateTime.Now
                    }).ToList();

                var ddddd = _userRepository.GetAll().Where(x => userListIds.Contains(3));
                return clientUser;
            }
            else
            {
                throw new Exception("no admin role found");
            }
        }
        public List<AdminMessageDto> AdminGetSpUserMessages(long userId)
        {
            var MessFromUser = _messageRepository.GetAll().Where(x => x.From == userId)
                .Select(x => new AdminMessageDto()
                {
                    Id = x.Id,
                    Content = x.Content,
                    IsFromAdmin = false,
                    MessageTime = x.CreationTime
                }).ToList();

            var MessToUser = _messageRepository.GetAll().Where(x => x.To == userId)
                .Select(x => new AdminMessageDto()
                {
                    Id = x.Id,
                    Content = x.Content,
                    IsFromAdmin = true,
                    MessageTime = x.CreationTime
                }).ToList();

            MessFromUser.AddRange(MessToUser);

            return MessFromUser.OrderBy(x => x.Id).ToList();
        }
        public AdminMessageDto AdminSendMessage(AdminCreateMessageDto model)
        {
            var userId = AbpSession.GetUserId();

            _messageRepository.Insert(new Message()
            {
                From = userId,
                To = model.To,
                Content = model.Content,
                CreationTime = DateTime.Now,
                CreatorUserId = userId,
                IsDeleted = false,
            });


            var userToken = _notificationTokenRepository.GetAll()
                .Where(n => n.UserId == model.To)
                .Select(n => n.Token).FirstOrDefault();


            string title = "رسائل جديدة";
            string body = "لقد تم ارسال رسالة من الدعم الفنى";


            var not = _driverNotification.Insert(new DriverNotification()
            {
                DriverId = model.To,
                IsRead = false,
                Title = title,
                Body = body,
                Type = 6
            });


            _notificationCenter.SendPushNotification(new string[] { userToken }, title, body, new
            {
                NotId = not.Id,
                Type = 6,
                Content = model.Content
            });

            CurrentUnitOfWork.SaveChanges();

            return new AdminMessageDto()
            {
                Content = model.Content,
                MessageTime = DateTime.Now,
                IsFromAdmin = true
            };
        }
        [AbpAllowAnonymous]
        public List<MessageDto> GetMessages()
        {
            var userId = AbpSession.GetUserId();

            var from = _messageRepository.GetAll().Where(x => x.From == userId)
                .Select(x => new MessageDto()
                {
                    Id = x.Id,
                    Content = x.Content,
                    MessageTime = x.CreationTime,
                    Type = 0,
                }).ToList();
            var to = _messageRepository.GetAll().Where(x => x.To == userId)
                .Select(x => new MessageDto()
                {
                    Id = x.Id,
                    Content = x.Content,
                    MessageTime = x.CreationTime,
                    Type = 1
                }).ToList();

            from.AddRange(to);

            return from.OrderBy(x => x.Id).ToList();

        }
        [AbpAllowAnonymous]
        public async Task<MessageDto> SendMessage(CreateMessageDto model)
        {
            try
            {
                var userId = AbpSession.GetUserId();
                var defaultTenantAdmin = new UserIdentifier(null, 10030);
                var hostAdmin = new UserIdentifier(null, 10030);

                _messageRepository.Insert(new Message()
                {
                    From = userId,
                    To = hostAdmin.UserId,
                    Content = model.Content,
                    CreationTime = DateTime.Now,
                    CreatorUserId = userId,
                    IsDeleted = false,
                });

                CurrentUnitOfWork.SaveChanges();


                await _notificationPublisher.PublishAsync(
                    "App.SimpleMessage",
                    new MessageNotificationData(model.Content),
                    severity: NotificationSeverity.Info,
                    userIds: new[] { hostAdmin }
                );

                return new MessageDto()
                {
                    Content = model.Content,
                    MessageTime = DateTime.Now,
                    Type = 0
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [RemoteService(IsMetadataEnabled = false)]
        [AbpAllowAnonymous]
        public async Task<UserDto> CreateByPhoneAsync(CreateUserByPhoneDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }
        [AbpAllowAnonymous]
        public async Task UpdatePhone(string phone)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());

            user.EmailAddress = phone;
            user.Name = phone;
            user.Surname = phone;
            user.UserName = phone;
            user.Password = _passwordHasher.HashPassword(user, phone);

            CheckErrors(await _userManager.UpdateAsync(user));
        }
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }
        [AbpAllowAnonymous]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }
        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }
        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }
        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }
        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }
        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }
        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }
        [AbpAllowAnonymous]
        public void Logout()
        {
            var userId = AbpSession.GetUserId();
            var tokenEnt = _notificationTokenRepository.GetAll().Where(x => x.UserId == userId).FirstOrDefault();
            if (tokenEnt != null)
            {
                tokenEnt.Token = "";
                _notificationTokenRepository.Update(tokenEnt);
                CurrentUnitOfWork.SaveChanges();
            }
        }
        public async Task<bool> GetUserRoles(UserRoleDto input)
        {
            var user = _userManager.Users.Where(u => u.Id == input.UserID).FirstOrDefault();
            return await _userManager.IsInRoleAsync(user, input.RoleName);
        }
    }
}

