using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wajeeh.AdminReuests.Dto;
using Wajeeh.Authorization.Roles;
using Wajeeh.Authorization.Users;
using Wajeeh.ClientAdresses;
using Wajeeh.Clinets;
using Wajeeh.Companies;
using Wajeeh.DriverNotifications;
using Wajeeh.Drivers;
using Wajeeh.EntityFrameworkCore;
using Wajeeh.NotificationCenters;
using Wajeeh.NotificationLogs;
using Wajeeh.NotificationTokens;
using Wajeeh.OfferPrices;
using Wajeeh.OfferPrices.Dto;
using Wajeeh.Requests;
using Wajeeh.RequestStatus;
using Wajeeh.Wasel;
using Wajeeh.Wasel.models;

namespace Wajeeh.AdminReuests
{
    public class AdminRequestAppService : AsyncCrudAppService<Request, AdminRequestDto, long, AdminPagedRequestResultRequestDto, AdminCreateRequestDto, AdminUpdateRquestDto>,
        IAdminRequestAppService
    {

        private readonly IRepository<Client, long> _clientRepository;
        private readonly IRepository<Request, long> _repository;
        private readonly IRepository<ClientAdress, long> _adressRepository;
        private readonly IRepository<OfferPrice, long> _offerPriceRepository;
        private readonly IRepository<Driver, long> _driverRepository;
        private readonly IRepository<Company, long> _companyRepository;
        private readonly IRepository<NotificationToken, long> _notificationTokenRepository;
        private readonly IRepository<NotificationLog, long> _notificationLogRepository;
        private readonly INotificationCenter _notificationCenter;
        private readonly IOfferPriceAppService _offerPriceAppService;
        private readonly IWaselService _waselService;
        private readonly IRepository<User, long> _userRepository;
        private readonly RoleManager _roleManager;
        private readonly IDbContextProvider<WajeehDbContext> _dbContextProvider;

        private readonly IRepository<RequestState, long> _requestStateRepository;

        public AdminRequestAppService(
            IRepository<Client, long> clientRepository,
            IRepository<Request, long> repository,
            RoleManager roleManager,
            IRepository<ClientAdress, long> adressRepository,
            IRepository<User, long> userRepository,
            IRepository<OfferPrice, long> offerPriceRepository,
            IRepository<Driver, long> driverRepository,
            IRepository<Company, long> companyRepository,
            IWaselService waselService,
            IRepository<NotificationToken, long> notificationTokenRepository,
            IRepository<NotificationLog, long> notificationLogRepository,
            INotificationCenter notificationCenter,
            IOfferPriceAppService offerPriceAppService,
            IRepository<RequestState, long> requestStateRepository,
            IDbContextProvider<WajeehDbContext> dbContextProvider) : base(repository)
        {
            _dbContextProvider = dbContextProvider;
            _repository = repository;
            _userRepository = userRepository;
            _roleManager = roleManager;
            _adressRepository = adressRepository;
            _offerPriceRepository = offerPriceRepository;
            _driverRepository = driverRepository;
            _companyRepository = companyRepository;
            _notificationTokenRepository = notificationTokenRepository;
            _notificationLogRepository = notificationLogRepository;
            _notificationCenter = notificationCenter;
            _offerPriceAppService = offerPriceAppService;
            _waselService = waselService;
            _requestStateRepository = requestStateRepository;
            _clientRepository = clientRepository;
        }
        IDictionary<int, string> RquestStatus = new Dictionary<int, string>() {
                    { 1, "Scheduled , مجدول"},
                    { 2, "Scheduled , مجدول"},
                    { 3, "Current , الحالي"},
                    { 4, "Previous , السابق"},
                    { 5, "Cancelled , ملغي"},
                };
        protected override IQueryable<Request> ApplySorting(IQueryable<Request> query, AdminPagedRequestResultRequestDto input)
        {
            if (input.Sorting != null)
            {
                if (input.Sorting.StartsWith("Subcategory"))
                {
                    if (input.Sorting.EndsWith("Asc"))
                    {
                        return query.OrderBy(p => p.Subcategory.Id);

                    }
                    else
                    {
                        return query.OrderByDescending(p => p.Subcategory.Id);

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
        protected override IQueryable<Request> CreateFilteredQuery(AdminPagedRequestResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);

            query = query.Include(x => x.OfferPrices);


            if (input.Status.HasValue)
            {
                if (input.Status == 1)//حالية تم قبول عرض سعر
                {
                    query = query.Where(x => x.Status == 3);
                }
                else if (input.Status == 2)//مجدولة تم تقديم عرض سعر او لم يتم
                {
                    query = query.Where(x => x.Status == 1 || x.Status == 2);
                }
                else if (input.Status == 3)//المنجزة
                {
                    query = query.Where(x => x.Status == 4);
                }
                else if (input.Status == 4)//الملغية
                {
                    query = query.Where(x => x.Status == 5);
                }
            }

            if (input.CompanyId != null)
            {
                try
                {
                    var driversId = _driverRepository.GetAll().Where(x => x.CompanyId == input.CompanyId).Select(x => x.Id).ToList();
                    query = query.Where(x => driversId.Contains(x.OfferPrices.Where(o => o.IsAccepted == true).Select(of => of.DriverId).FirstOrDefault()));
                }
                catch
                {
                    return query.Include(x => x.Subcategory);
                }
            }
            if (!input.KeyWord.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.KeyWord);
                    string EndingPointTitle = filter_query["EndingPointTitle"];
                    query = query.WhereIf(!EndingPointTitle.IsNullOrEmpty(), t => t.EndingPointTitle.Contains(EndingPointTitle));
                    string StratingPointTitle = filter_query["StratingPointTitle"];
                    query = query.WhereIf(!StratingPointTitle.IsNullOrEmpty(), t => t.StratingPointTitle.Contains(StratingPointTitle));
                    string Subcategory = filter_query["Subcategory"];
                    query = query.WhereIf(!Subcategory.IsNullOrEmpty(), t => t.Subcategory.Name.Contains(Subcategory) || t.Subcategory.NameAr.Contains(Subcategory));

                    string CreationTime = filter_query["CreationTime"];
                    if (!CreationTime.IsNullOrEmpty())
                    {
                        DateTime creationTime = DateTime.ParseExact(CreationTime, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                        query = query.Where(t =>
                        t.CreationTime.Year == creationTime.Year &&
                        t.CreationTime.Month == creationTime.Month &&
                        t.CreationTime.Day == creationTime.Day);
                    }

                    string AcceptedDriverName = filter_query["AcceptedDriverName"];
                    query = query.WhereIf(!AcceptedDriverName.IsNullOrEmpty(), t => t.OfferPrices.Any(x => x.DriverName.Contains(AcceptedDriverName)));

                    string RequestStateName = filter_query["RequestStateName"];
                    if (!RequestStateName.IsNullOrEmpty())
                    {
                        var requestStateIds = _requestStateRepository.GetAll()
                            .Where(x =>
                            (CultureInfo.CurrentCulture.Name != "ar-EG" && x.Name.Contains(RequestStateName)) ||
                            (CultureInfo.CurrentCulture.Name == "ar-EG" && x.NameAr.Contains(RequestStateName)))
                            .Select(x => x.Id).ToArray();
                        query = query.WhereIf(requestStateIds != null, t => requestStateIds.Contains(t.Status));
                    }

                }
                catch
                {
                    query = query.Where(x => x.StratingPointTitle.Contains(input.KeyWord) || x.EndingPointTitle.Contains(input.KeyWord));
                }
            }



            var userId = AbpSession.GetUserId();

            var user = _userRepository.GetAll().Include(x => x.Roles).Where(x => x.Id == userId).FirstOrDefault();

            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            if (!roles.Contains("ADMIN"))
            {
                if (roles.Contains("CLIENT"))
                {
                    query = query.Where(x => x.UserRequsetId == userId);
                }
                if (roles.Contains("DRIVER"))
                {
                    query = query.Where(x => x.OfferPrices.Any(y => y.DriverId == userId && y.IsAccepted == true));
                }
                else
                {
                    query = query.Where(x => x.Id == 0);
                }
            }

            return query.Where(x => !x.IsDeleted).Include(x => x.Subcategory);
        }


        public string RegTrip(Trip model)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            var req = _repository.GetAll()
                .Where(r => r.Id == model.RequestId).FirstOrDefault();

            var driverId = _offerPriceRepository.GetAll()
                .Where(x => x.RequestId == model.RequestId && x.IsAccepted == true)
                .Select(x => x.DriverId).FirstOrDefault();

            var driver = _driverRepository.GetAll().Where(x => x.Id == driverId).FirstOrDefault();

            if (req == null)
                return "reservation not found";


            var trip = new TripRegVM()
            {
                tripNumber = req.Id.ToString(),
                departedWhen = DateTime.Now.ToString(),
                departureLatitude = (int)double.Parse(req.StartingPoint.Split(',')[0]),
                departureLongitude = (int)double.Parse(req.StartingPoint.Split(',')[1]),
                expectedDestinationLatitude = (int)double.Parse(req.EndingPoint.Split(',')[0]),
                expectedDestinationLongitude = (int)double.Parse(req.EndingPoint.Split(',')[1]),
                driverIdentityNumber = driver.DriverIdentityNumber,
                vehicleSequenceNumber = driver.VehicleSequenceNumber,
            };

            req.IsReg = true;

            _repository.Update(req);
            CurrentUnitOfWork.SaveChanges();
            return _waselService.RegTrip(trip);
        }

        public bool UpdateTrip(Trip model)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            var req = _repository.GetAll()
                .Where(r => r.Id == model.RequestId).FirstOrDefault();


            if (req == null)
                return false;

            var trip = new TripUpdateVM()
            {
                actualDestinationLatitude = (int)double.Parse(req.EndingPoint.Split(',')[0]),
                actualDestinationLongitude = (int)double.Parse(req.EndingPoint.Split(',')[1]),
                arrivedWhen = DateTime.Now.ToString(),
                tripNumber = (int)req.Id
            };
            return _waselService.UpdateTrip(trip);
        }


        public AdminRequestDto CreateRequest(AdminCreateRequestDto model)
        {
            model.ArrivalDateTime = DateTime.Now;
            var request = ObjectMapper.Map<Request>(model);
            var userId = AbpSession.GetUserId();
            request.UserRequsetId = userId;
            request.Status = 1;
            request.IsRated = false;
            request.Rate = null;
            var requestEntity = _repository.Insert(request);

            if (request.StartingPoint == request.EndingPoint &&
                !_adressRepository.GetAll().Any(a => a.LangLat == request.StartingPoint && a.UserId == userId))
                _adressRepository.Insert(new ClientAdress()
                {
                    UserId = userId,
                    Adress = request.StratingPointAdress,
                    Title = request.StratingPointTitle,
                    LangLat = request.StartingPoint
                });
            else
            {
                if (!_adressRepository.GetAll().Any(a => a.LangLat == request.StartingPoint && a.UserId == userId))
                    _adressRepository.Insert(new ClientAdress()
                    {
                        UserId = userId,
                        Adress = request.StratingPointAdress,
                        Title = request.StratingPointTitle,
                        LangLat = request.StartingPoint
                    });

                if (!_adressRepository.GetAll().Any(a => a.LangLat == request.EndingPoint && a.UserId == userId))
                    _adressRepository.Insert(new ClientAdress()
                    {
                        UserId = userId,
                        Adress = request.EndingPointAdress,
                        Title = request.EndingPointTitle,
                        LangLat = request.EndingPoint
                    });
            }


            CurrentUnitOfWork.SaveChanges();

            var driverUsersIds = _driverRepository.GetAll().Where(d => d.VehicleType == model.SubcategoryId).Select(d => d.UserId).ToList();

            var usersTokens = _notificationTokenRepository.GetAll()
                    .Where(n => driverUsersIds.Contains(n.UserId))
                    .Select(n => n.Token).ToArray();


            string title = "طلبات جديدة";
            string body = "لقد تم اضافة طلب جديد من قبل العميل";

            _notificationCenter.SendPushNotification(usersTokens, title, body, new
            {
                Type = 3,
            });

            //_notificationLogRepository.Insert(new NotificationLog()
            //{
            //    UserId = rquestWOfferPrice.DriverId,
            //    NotificationTitle = title,
            //    NotificationBody = body
            //});

            return ObjectMapper.Map<AdminRequestDto>(requestEntity);
        }

        public async override Task<AdminRequestDto> GetAsync(EntityDto<long> input)
        {
            var userId = AbpSession.GetUserId();
            var user = _userRepository.GetAll().Include(x => x.Roles).Where(x => x.Id == userId).FirstOrDefault();
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();
            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);
            if (!roles.Contains("ADMIN"))
            {
                if (roles.Contains("CLIENT"))
                {
                    if (!_repository.GetAll().Any(x => x.UserRequsetId == userId && x.Id == input.Id))
                        return null;
                }
                if (roles.Contains("DRIVER"))
                {
                    if (!_repository.GetAll().Any(x => x.OfferPrices.Any(y => y.DriverId == userId && y.IsAccepted == true)))
                        return null;
                }
                else
                {
                    return null;
                }
            }

            Request entity = await _repository.GetAllIncluding(x => x.UserRequset, x => x.OfferPrices).FirstOrDefaultAsync(x => x.Id == input.Id);
            return MapToEntityDto(entity);

            //return base.GetAsync(input);
        }

        public override Task<AdminRequestDto> UpdateAsync(AdminUpdateRquestDto input)
        {
            var userId = AbpSession.GetUserId();

            var user = _userRepository.GetAll().Include(x => x.Roles).Where(x => x.Id == userId).FirstOrDefault();

            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            if (!roles.Contains("ADMIN"))
            {
                if (roles.Contains("CLIENT"))
                {
                    if (!_repository.GetAll().Any(x => x.UserRequsetId == userId && x.Id == input.Id))
                        return null;
                }
                if (roles.Contains("DRIVER"))
                {
                    if (!_repository.GetAll().Any(x => x.OfferPrices.Any(y => y.DriverId == userId && y.IsAccepted == true)))
                        return null;
                }
                else
                {
                    return null;
                }
            }

            return base.UpdateAsync(input);
        }
        protected override void MapToEntity(AdminUpdateRquestDto updateInput, Request entity)
        {
            updateInput.ArrivalDateTime = DateTime.Now;
            base.MapToEntity(updateInput, entity);
        }

        protected override AdminRequestDto MapToEntityDto(Request entity)
        {
            var requestDto = base.MapToEntityDto(entity);
            requestDto.CustomerName = entity.UserRequset?.FullName;
            requestDto.CTString = entity.CreationTime.ToString("dddd, dd MMMM yyyy");
            if (entity.OfferPrices != null && entity.OfferPrices.Count > 0)
            {
                var acceptedOfferPrice = entity.OfferPrices
                    .FirstOrDefault(x => x.IsAccepted == true);
                if (acceptedOfferPrice != null)
                {
                    requestDto.VAT = (double)acceptedOfferPrice.VAT;
                    requestDto.VATAmount = (double)(acceptedOfferPrice.DeliveryCost * acceptedOfferPrice.VAT / 100);
                    requestDto.AcceptedDriverName = acceptedOfferPrice.DriverName;
                    requestDto.Net = (double)(acceptedOfferPrice.DeliveryCost + (acceptedOfferPrice.DeliveryCost * acceptedOfferPrice.VAT / 100));
                }
            }
            if (entity.Subcategory != null)
            {
                if (CultureInfo.CurrentCulture.Name == "ar-EG")
                {
                    requestDto.SubcategoryDisplayName = entity.Subcategory.NameAr;
                }
                else
                {
                    requestDto.SubcategoryDisplayName = entity.Subcategory.Name;
                }
            }

            try
            {
                //var requestState = _requestStateRepository.Get(entity.Status);

         
             
                if (CultureInfo.CurrentCulture.Name == "ar-EG")
                {

                    requestDto.RequestStateName = RquestStatus[entity.Status].Split(',')[1];
                }
                else
                {
                    requestDto.RequestStateName = requestDto.RequestStateName = RquestStatus[entity.Status].Split(',')[1];
                }
            }
            catch
            { }
            return requestDto;
        }

        public long CreateRequestFromAdminPanel(AdminCreateRequestWithOfferDto input)
        {
            long Inserted = 0;
            var CreatedRequest = CreateRequest(input.adminCreateRequestDto);
            if (CreatedRequest != null)
            {
                input.adminOfferPriceDto.RequestId = CreatedRequest.Id;
                var newOffer = ObjectMapper.Map<OfferPrice>(input.adminOfferPriceDto);
                newOffer.IsAccepted = true;
                var CreatedOffer = _offerPriceRepository.InsertAndGetId(newOffer);
                CurrentUnitOfWork.SaveChanges();
                if (CreatedOffer != 0)
                {
                    Inserted = CreatedOffer;
                }
            }
            return Inserted;
        }

        public bool UpdaterequestFromAdminPanel(AdminCreateRequestWithOfferDto input)
        {
            bool Updated = false;
            var oldRequest = _repository.Get(input.adminCreateRequestDto.Id);
            var req = ObjectMapper.Map(input.adminCreateRequestDto, oldRequest);
            req.LastModificationTime = DateTime.Now;
            req.ArrivalDateTime = DateTime.Now;
            var updatedRequest = _repository.Update(req);
            CurrentUnitOfWork.SaveChanges();
            if (updatedRequest != null)
            {
                var off = _offerPriceRepository.FirstOrDefault(xxx => xxx.RequestId == req.Id);
                off.DriverId = input.adminOfferPriceDto.DriverId;
                off.DriverName = input.adminOfferPriceDto.DriverName;
                var updatedOffer = _offerPriceRepository.Update(off);
                if (updatedOffer != null)
                {
                    CurrentUnitOfWork.SaveChanges();
                    Updated = true;
                }
            }
            return Updated;
        }

        public bool CancelRequestFromAdminPanel(long input)
        {
            var requestDeleted = false;
            var request = _repository.Get(input);
            OfferPrice acceptedOffer = null;
            if (request != null)
            {
                var offerPrices = _offerPriceRepository.GetAll().Where(x => x.RequestId == input).ToList();
                if (offerPrices.Count > 0)
                {
                    foreach (var offer in offerPrices)
                    {
                        if (offer.IsAccepted.HasValue && offer.IsAccepted.Value)
                        {
                            acceptedOffer = offer;
                        }
                        offer.IsDeleted = true;
                        _offerPriceRepository.Update(offer);
                        CurrentUnitOfWork.SaveChanges();
                    }
                }
                if (acceptedOffer != null)
                {
                    if (!OffersRequestsAvailabilityCancelation(request, acceptedOffer))


                        acceptedOffer.IsDeleted = true;
                    _offerPriceRepository.Update(acceptedOffer);
                }

                if (!OffersRequestsAvailabilityCancelation(request))
                {

                    request.Status = 5;
                }
                request.Status = 5;
                _repository.Update(request);
                CurrentUnitOfWork.SaveChanges();

                var userToken = _notificationTokenRepository.GetAll()
                .Where(n => n.UserId == request.UserRequsetId)
                .Select(n => n.Token).FirstOrDefault();


                string title = "رسائل جديدة";
                string body = "لقد تم الغاء الرحلة ";


                var UserNot = _notificationLogRepository.Insert(new NotificationLog()
                {
                    UserId = request.UserRequsetId,
                    NotificationTitle = title,
                    NotificationBody = body,
                });


                _notificationCenter.SendPushNotification(new string[] { userToken }, title, body, new
                {
                    NotId = UserNot.Id,
                    Type = 6,
                    Content = UserNot.NotificationBody
                });
                requestDeleted = true;
            }

            return requestDeleted;
        }

        private bool OffersRequestsAvailabilityCancelation(Request request, OfferPrice offer = null)
        {
            var CanCancel = false;

            if ((request.Status == 2 && offer?.OfferStatus == 1)
                || ((request.Status == 1 || request.Status == 3) && offer?.OfferStatus == 1)
                || (request.Status == 1 && offer == null))
                CanCancel = true;
            return CanCancel;
        }

        public async Task<List<TopRequestSalesClientDto>> GetTopRequestSalesClients(GetTopRequestSalesClientInput input)
        {
            var result = await _repository
                .GetAll()
               .Where(x => x.Status == 4)
               .WhereIf(!input.Email.IsNullOrEmpty(), x => x.UserRequset.EmailAddress.Contains(input.Email))
               .WhereIf(!input.Phone.IsNullOrEmpty(), x => x.UserRequset.PhoneNumber.Contains(input.Phone))
                .GroupBy(p => p.UserRequsetId)
               .Select(g => new TopRequestSalesClientDto()
               {
                   Id = g.Key,
                   RequestsCount = g.Count()
               })
               .OrderByDescending(x => x.RequestsCount)
               .ToListAsync();
            if (result != null)
            {
                foreach (var item in result)
                {
                    try
                    {
                        var client = await _clientRepository.GetAllIncluding(x => x.User).FirstOrDefaultAsync(x => x.UserId == item.Id);
                        if (client == null)
                            continue;

                        item.Email = client.User.EmailAddress;
                        item.FirstName = client.FirstName;
                        item.LastName = client.LastName;
                        item.Phone = client.User.PhoneNumber;
                    }
                    catch
                    {
                    }
                }
                result = result
                    .WhereIf(!input.FirstName.IsNullOrEmpty(), x => x.FirstName != null && x.FirstName.Contains(input.FirstName))
                    .WhereIf(!input.LastName.IsNullOrEmpty(), x => x.LastName != null && x.LastName.Contains(input.LastName))
                    .ToList();
            }
            //var result=     await _repository.GetAll()
            //        .GroupBy(p => p.UserRequset)
            //        .Select(g => new TopRequestSalesClientDto()
            //        {
            //            Email = g.Key.EmailAddress,
            //            FirstName = g.Key.Name,
            //            LastName = g.Key.Surname,
            //            Id = g.Key.Id,
            //            Phone = g.Key.PhoneNumber,
            //            RequestsCount = g.Count()
            //        })
            //        //.OrderByDescending(x => x.RequestsCount)
            //        .ToListAsync();
            return result;
        }
    }
}
