using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Requests;
using Wajeeh.Reuests.Dto;
using Abp.Linq.Extensions;
using System.Linq;
using System.Globalization;
using Abp.Runtime.Session;
using Wajeeh.ClientAdresses;
using AutoMapper;
using Abp.Application.Services.Dto;
using Wajeeh.OfferPrices;
using Microsoft.EntityFrameworkCore;
using Wajeeh.Drivers;
using Wajeeh.NotificationTokens;
using Wajeeh.NotificationLogs;
using Wajeeh.NotificationCenters;
using System.Threading.Tasks;
using Wajeeh.DriverNotifications;

namespace Wajeeh.Reuests
{
    public class RequestAppService : AsyncCrudAppService<Request, RequestDto, long, PagedRequestResultRequestDto, CreateRequestDto, UpdateRquestDto>, IRequestAppService
    {
        private readonly IRepository<Request, long> _repository;
        private readonly IRepository<ClientAdress, long> _adressRepository;
        private readonly IRepository<OfferPrice, long> _offerPriceRepository;
        private readonly IRepository<Driver, long> _driverRepository;
        private readonly IRepository<NotificationToken, long> _notificationTokenRepository;
        private readonly IRepository<NotificationLog, long> _notificationLogRepository;
        private readonly INotificationCenter _notificationCenter;
        private readonly IRepository<DriverNotification, long> _driverNotification;

        public RequestAppService(IRepository<Request, long> repository,
            IRepository<ClientAdress, long> adressRepository,
            IRepository<DriverNotification, long> driverNotification,
            IRepository<OfferPrice, long> offerPriceRepository,
            IRepository<Driver, long> driverRepository,
            IRepository<NotificationToken, long> notificationTokenRepository,
            IRepository<NotificationLog, long> notificationLogRepository,
            INotificationCenter notificationCenter) : base(repository)
        {
            _repository = repository;
            _adressRepository = adressRepository;
            _driverNotification = driverNotification;
            _offerPriceRepository = offerPriceRepository;
            _driverRepository = driverRepository;
            _notificationTokenRepository = notificationTokenRepository;
            _notificationLogRepository = notificationLogRepository;
            _notificationCenter = notificationCenter;
        }

        public RequestDto CreateRequest(CreateRequestDto model)
        {
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




            var driverUsersIds = _driverRepository.GetAll().Where(d => d.VehicleType == model.SubcategoryId && d.IsDriverAvilable && !d.OffDuty).Select(d => d.UserId).ToList();

            var usersTokens = _notificationTokenRepository.GetAll()
                    .Where(n => driverUsersIds.Contains(n.UserId))
                    .Select(n => n.Token).ToArray();
            string title = "طلبات جديدة";
            string body = "لقد تم اضافة طلب جديد من قبل العميل";
            foreach (var item in driverUsersIds)
            {
                var not = _driverNotification.Insert(new DriverNotification()
                {
                    DriverId = item,
                    IsRead = false,
                    Title = title,
                    Body = body,
                    Type = 3
                });

                _notificationCenter.SendPushNotification(usersTokens, title, body, new
                {
                    NotId = not.Id,
                    Type = 3,
                });
            }
            CurrentUnitOfWork.SaveChanges();
            //_notificationLogRepository.Insert(new NotificationLog()
            //{
            //    UserId = rquestWOfferPrice.DriverId,
            //    NotificationTitle = title,
            //    NotificationBody = body
            //});

            return ObjectMapper.Map<RequestDto>(requestEntity);
        }

        public RequestDto GetClientRequest(long requestId)
        {

            var userId = AbpSession.GetUserId();
            var request = _repository.GetAll().Include(r => r.Subcategory)
                .Where(r => r.Id == requestId && r.UserRequsetId == userId)
                .FirstOrDefault();

            var requestDto = MapToEntityDto(request);
            return requestDto;
        }
        public PagedResultDto<RequestDto> GetClientRequests(PagedClientRequestResultRequestDto input)
        {
            var userId = AbpSession.GetUserId();

            var query = _repository.GetAll().Include(r => r.Subcategory).Include(r => r.OfferPrices)
                .Where(r => r.UserRequsetId == userId);

            if (input.status.HasValue)
            {
                if (input.status == 1)//حالية تم قبول عرض سعر
                {
                    query = query.Where(r => r.Status == 3);
                }
                else if (input.status == 2)//مجدولة تم تقديم عرض سعر او لم يتم
                {
                    query = query.Where(r => r.Status == 1 || r.Status == 2);
                }
                else if (input.status == 3)//المنجزة
                {
                    query = query.Where(r => r.Status == 4);
                }
                else if (input.status == 4)//الملغية
                {
                    query = query.Where(r => r.Status == 5);
                }
            }

            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var requests = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll()
                .Where(r => r.UserRequsetId == userId);

            if (input.status.HasValue)
            {
                if (input.status == 1)//حالية تم قبول عرض سعر
                {
                    query = query.Where(r => r.Status == 3);
                }
                else if (input.status == 2)//مجدولة تم تقديم عرض سعر او لم يتم
                {
                    query = query.Where(r => r.Status == 1 || r.Status == 2);
                }
                else if (input.status == 3)//المنجزة
                {
                    query = query.Where(r => r.Status == 4);
                }
                else if (input.status == 4)//الملغية
                {
                    query = query.Where(r => r.Status == 5);
                }
            }

            var count = querycount.Count();

            return new PagedResultDto<RequestDto>()
            {
                Items = requests.Select(r => MapToEntityDto(r)).ToList(),
                TotalCount = count
            };
        }
        public PagedResultDto<RequestDto> GetRequests(PagedClientRequestResultRequestDto input, UserType userType)
        {
            var userId = AbpSession.GetUserId();
            IQueryable<Request> query = Enumerable.Empty<Request>().AsQueryable();
            if (userType == UserType.Client)
            {
                query = _repository.GetAll().Include(r => r.Subcategory).Include(r => r.OfferPrices)
                .Where(r => r.UserRequsetId == userId);
            }
            else if (userType == UserType.Driver)
            {

                query = _repository.GetAll().Include(r => r.Subcategory).Include(r => r.OfferPrices)
                     .Where(r => r.OfferPrices.Any(x => x.CreatorUserId == userId));
            }


            if (input.status.HasValue)
            {
                if (input.status == 1)//حالية تم قبول عرض سعر
                {
                    query = query.Where(r => r.Status == 3);
                }
                else if (input.status == 2)//مجدولة تم تقديم عرض سعر او لم يتم
                {
                    query = query.Where(r => r.Status == 1 || r.Status == 2);
                }
                else if (input.status == 3)//المنجزة
                {
                    query = query.Where(r => r.Status == 4);
                }
                else if (input.status == 4)//الملغية
                {
                    query = query.Where(r => r.Status == 5);
                }
            }

            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var requests = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll()
                .Where(r => r.UserRequsetId == userId);

            if (input.status.HasValue)
            {
                if (input.status == 1)//حالية تم قبول عرض سعر
                {
                    query = query.Where(r => r.Status == 3);
                }
                else if (input.status == 2)//مجدولة تم تقديم عرض سعر او لم يتم
                {
                    query = query.Where(r => r.Status == 1 || r.Status == 2);
                }
                else if (input.status == 3)//المنجزة
                {
                    query = query.Where(r => r.Status == 4);
                }
                else if (input.status == 4)//الملغية
                {
                    query = query.Where(r => r.Status == 5);
                }
            }

            var count = querycount.Count();

            return new PagedResultDto<RequestDto>()
            {
                Items = requests.Select(r => MapToEntityDto(r)).ToList(),
                TotalCount = count
            };
        }

        //لعرض  طلبات جديدة للسائق ليقدم عرض سعر لها
        public PagedResultDto<RequestDto> GetDriverNewRequests(PagedDriverNewRequestResultRequestDto input)
        {
            var userId = AbpSession.GetUserId();

            var driver = GetDriverByUserId(userId);

            if (driver == null)
                throw new Exception("no driver found");

            var query = _repository.GetAll().Include(r => r.Subcategory)
                .Where(r => r.SubcategoryId == driver.VehicleType && (r.Status == 1 || r.Status == 2));



            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var requests = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll().Include(r => r.Subcategory)
                .Where(r => r.SubcategoryId == driver.VehicleType && (r.Status == 1 || r.Status == 2));



            var count = querycount.Count();

            return new PagedResultDto<RequestDto>()
            {
                Items = requests.Select(r => MapToEntityDto(r)).ToList(),
                TotalCount = count
            };
        }

        //لعرض تفاصيل طلب جديد للسائق ليقدم عرض سعر لة
        public RequestDto GetDriverNewRequest(long requestId)
        {

            var userId = AbpSession.GetUserId();

            var driver = GetDriverByUserId(userId);

            var request = _repository.GetAll().Include(r => r.Subcategory)
                .Where(r => r.Id == requestId && r.SubcategoryId == driver.VehicleType && (r.Status == 1 || r.Status == 2))
                .FirstOrDefault();

            var requestDto = MapToEntityDto(request);
            return requestDto;
        }
        protected override RequestDto MapToEntityDto(Request entity)
        {
            var requestDto = base.MapToEntityDto(entity);
            requestDto.VAT = 0;
            requestDto.Net = 0;
            requestDto.AcceptedDriverName = string.Empty;

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

            if (entity.OfferPrices != null && entity.OfferPrices.Count > 0)
            {
                var acceptedOfferPrice = entity.OfferPrices
                    .FirstOrDefault(x => x.IsAccepted == true);
                if (acceptedOfferPrice != null)
                {
                    requestDto.AcceptedDriverName = acceptedOfferPrice.DriverName;
                    requestDto.VAT = acceptedOfferPrice.VAT;
                    requestDto.Net = acceptedOfferPrice.DeliveryCost + (acceptedOfferPrice.DeliveryCost * acceptedOfferPrice.VAT / 100);
                }
            }

            return requestDto;
        }

        private Driver GetDriverByUserId(long userId)
        {
            return _driverRepository.GetAll().Where(d => d.UserId == userId).FirstOrDefault();
        }

        //protected override IQueryable<Request> CreateFilteredQuery(PagedRequestResultRequestDto input)
        //{

        //}
        //protected override RequestDto MapToEntityDto(Request entity)
        //{
        //    var requestDto = base.MapToEntityDto(entity);
        //    if (CultureInfo.CurrentCulture.Name == "ar-EG")
        //    {
        //        requestDto.DisplayName = entity.NameAr;
        //        requestDto.DisplayDescription = entity.DescriptionAr;
        //    }
        //    else
        //    {
        //        requestDto.DisplayName = entity.Name;
        //        requestDto.DisplayDescription = entity.Description;
        //    }


        //    return requestDto;
        //}
    }
}
