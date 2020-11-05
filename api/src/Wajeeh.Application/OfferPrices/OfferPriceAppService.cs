using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using Wajeeh.OfferPrices.Dto;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Runtime.Session;
using Wajeeh.Requests;
using Wajeeh.NotificationTokens;
using Wajeeh.NotificationLogs;
using Wajeeh.NotificationCenters;
using Wajeeh.Drivers;
using Wajeeh.Clinets;
using Wajeeh.DriverNotifications;
using System.Threading.Tasks;
using AutoMapper;

namespace Wajeeh.OfferPrices
{
    public class OfferPriceAppService : AsyncCrudAppService<OfferPrice, OfferPriceDto, long, PagedOfferPriceResultRequestDto, CreateOfferPriceDto, UpdateOfferPriceDto>, IOfferPriceAppService
    {
        private readonly IRepository<OfferPrice, long> _repository;
        private readonly IRepository<Request, long> _rquestRepository;
        private readonly IRepository<NotificationToken, long> _notificationTokenRepository;
        private readonly IRepository<NotificationLog, long> _notificationLogRepository;
        private readonly INotificationCenter _notificationCenter;
        private readonly IRepository<Driver, long> _driverRepository;
        private readonly IRepository<Client, long> _clientRepository;
        private readonly IRepository<DriverNotification, long> _driverNotification;

        public OfferPriceAppService(IRepository<OfferPrice, long> repository,
            IRepository<DriverNotification, long> driverNotification,
            IRepository<Request, long> rquestRepository,
            IRepository<NotificationToken, long> notificationTokenRepository,
            IRepository<NotificationLog, long> notificationLogRepository,
            INotificationCenter notificationCenter,
            IRepository<Driver, long> driverRepository,
            IRepository<Client, long> clientRepository) : base(repository)
        {
            _repository = repository;
            _rquestRepository = rquestRepository;
            _driverNotification = driverNotification;
            _notificationTokenRepository = notificationTokenRepository;
            _notificationLogRepository = notificationLogRepository;
            _notificationCenter = notificationCenter;
            _driverRepository = driverRepository;
            _clientRepository = clientRepository;
        }



        //تفاصيل عرض السعر
        public RequestWOfferPriceDto GetClientRequestWOfferPrice(long offerPriceId)
        {
            var userId = AbpSession.GetUserId();

            var rquestWOfferPrice = _repository.GetAll()
                .Include(o => o.Request)
                .Where(o => o.Id == offerPriceId && o.Request.UserRequsetId == userId && o.Request.Status == 2).FirstOrDefault();

            if (rquestWOfferPrice == null)
                throw new Exception("not found");

            else
            {
                return new RequestWOfferPriceDto()
                {
                    IsDriverRead = rquestWOfferPrice.IsDriverRead,
                    ClientName = rquestWOfferPrice.ClientName,
                    ClientRate = rquestWOfferPrice.ClientRate,
                    IsClientRated = rquestWOfferPrice.IsClientRated,
                    IsRead = rquestWOfferPrice.IsRead,
                    IsRated = rquestWOfferPrice.IsRated,
                    Rate = rquestWOfferPrice.Rate,
                    OfferStatus = rquestWOfferPrice.OfferStatus,
                    DriverName = rquestWOfferPrice.DriverName,
                    AwayFrom = rquestWOfferPrice.AwayFrom,
                    DeliveryCost = (double)rquestWOfferPrice.DeliveryCost,
                    DeliveryThroughDays = rquestWOfferPrice.DeliveryThroughDays,
                    DeliveryThroughHours = rquestWOfferPrice.DeliveryThroughHours,
                    DeliveryThroughMinutes = rquestWOfferPrice.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = rquestWOfferPrice.DeliveryThroughSeconds,
                    DriverId = rquestWOfferPrice.DriverId,
                    OfferPriceId = rquestWOfferPrice.Id,
                    RequestId = rquestWOfferPrice.RequestId,
                    ArrivalDateTime = rquestWOfferPrice.Request.ArrivalDateTime,
                    DiscountPercentage = rquestWOfferPrice.Request.DiscountPercentage,
                    EndingPoint = rquestWOfferPrice.Request.EndingPoint,
                    EndingPointAdress = rquestWOfferPrice.Request.EndingPointAdress,
                    EndingPointTitle = rquestWOfferPrice.Request.EndingPointTitle,
                    Notes = rquestWOfferPrice.Request.Notes,
                    PaymentWay = rquestWOfferPrice.Request.PaymentWay,
                    StartingPoint = rquestWOfferPrice.Request.StartingPoint,
                    Status = rquestWOfferPrice.Request.Status,
                    StratingPointAdress = rquestWOfferPrice.Request.StratingPointAdress,
                    StratingPointTitle = rquestWOfferPrice.Request.StratingPointTitle,
                    SubcategoryId = rquestWOfferPrice.Request.SubcategoryId
                };
            }
        }

        //عروض الاسعار لطلب
        public PagedResultDto<RequestWOfferPriceDto> GetClientRequestOfferPrices(PagedRequestOfferPriceResultRequestDto input)
        {
            var userId = AbpSession.GetUserId();

            var query = _repository.GetAll().Include(o => o.Request)
                .Where(r => r.Request.UserRequsetId == userId && r.RequestId == input.RequestId && r.Request.Status == 2);

            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var offers = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll()
                .Where(r => r.Request.UserRequsetId == userId && r.RequestId == input.RequestId && r.Request.Status == 2);


            var count = querycount.Count();

            return new PagedResultDto<RequestWOfferPriceDto>()
            {
                Items = offers.Select(r => new RequestWOfferPriceDto()
                {
                    IsDriverRead = r.IsDriverRead,
                    ClientName = r.ClientName,
                    ClientRate = r.ClientRate,
                    IsClientRated = r.IsClientRated,
                    IsRead = r.IsRead,
                    IsRated = r.IsRated,
                    Rate = r.Rate,
                    OfferStatus = r.OfferStatus,
                    DriverName = r.DriverName,
                    AwayFrom = r.AwayFrom,
                    DeliveryCost = (double)r.DeliveryCost,
                    DeliveryThroughDays = r.DeliveryThroughDays,
                    DeliveryThroughHours = r.DeliveryThroughHours,
                    DeliveryThroughMinutes = r.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = r.DeliveryThroughSeconds,
                    DriverId = r.DriverId,
                    OfferPriceId = r.Id,
                    RequestId = r.RequestId,
                    ArrivalDateTime = r.Request.ArrivalDateTime,
                    DiscountPercentage = r.Request.DiscountPercentage,
                    EndingPoint = r.Request.EndingPoint,
                    EndingPointAdress = r.Request.EndingPointAdress,
                    EndingPointTitle = r.Request.EndingPointTitle,
                    Notes = r.Request.Notes,
                    PaymentWay = r.Request.PaymentWay,
                    StartingPoint = r.Request.StartingPoint,
                    Status = r.Request.Status,
                    StratingPointAdress = r.Request.StratingPointAdress,
                    StratingPointTitle = r.Request.StratingPointTitle,
                    SubcategoryId = r.Request.SubcategoryId
                }).ToList(),
                TotalCount = count
            };
        }

        //اشعارات عروض الاسعار
        public PagedResultDto<RequestWOfferPriceDto> GetClientNotifOfferPrices(PagedNotifOfferPriceResultRequestDto input)
        {
            var userId = AbpSession.GetUserId();

            var query = _repository.GetAll().Include(o => o.Request)
                .Where(r => r.Request.UserRequsetId == userId && r.Request.Status == 2);

            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var offers = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll()
                .Where(r => r.Request.UserRequsetId == userId && r.Request.Status == 2);


            var count = querycount.Count();

            return new PagedResultDto<RequestWOfferPriceDto>()
            {
                Items = offers.Select(r => new RequestWOfferPriceDto()
                {
                    IsDriverRead = r.IsDriverRead,
                    ClientName = r.ClientName,
                    ClientRate = r.ClientRate,
                    IsClientRated = r.IsClientRated,
                    IsRead = r.IsRead,
                    IsRated = r.IsRated,
                    Rate = r.Rate,
                    OfferStatus = r.OfferStatus,
                    DriverName = r.DriverName,
                    AwayFrom = r.AwayFrom,
                    DeliveryCost = (double)r.DeliveryCost,
                    DeliveryThroughDays = r.DeliveryThroughDays,
                    DeliveryThroughHours = r.DeliveryThroughHours,
                    DeliveryThroughMinutes = r.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = r.DeliveryThroughSeconds,
                    DriverId = r.DriverId,
                    OfferPriceId = r.Id,
                    RequestId = r.RequestId,
                    ArrivalDateTime = r.Request.ArrivalDateTime,
                    DiscountPercentage = r.Request.DiscountPercentage,
                    EndingPoint = r.Request.EndingPoint,
                    EndingPointAdress = r.Request.EndingPointAdress,
                    EndingPointTitle = r.Request.EndingPointTitle,
                    Notes = r.Request.Notes,
                    PaymentWay = r.Request.PaymentWay,
                    StartingPoint = r.Request.StartingPoint,
                    Status = r.Request.Status,
                    StratingPointAdress = r.Request.StratingPointAdress,
                    StratingPointTitle = r.Request.StratingPointTitle,
                    SubcategoryId = r.Request.SubcategoryId
                }).ToList(),
                TotalCount = count
            };
        }

        public RequestWOfferPriceDto MakeDriverOfferPrice(CreateOfferPriceDto input)
        {
            //var userId = AbpSession.GetUserId();

            var newOffer = ObjectMapper.Map<OfferPrice>(input);
            newOffer.DriverId = 1;//admin now // userId;
            newOffer.DriverName = "احمد محمد";
            newOffer.IsRead = false;

            var newOfferId = _repository.InsertAndGetId(newOffer);
            CurrentUnitOfWork.SaveChanges();

            var offer = _repository.GetAll().Include(o => o.Request)
                .Where(o => o.Id == newOfferId)
                .FirstOrDefault();

            if (offer == null)
                throw new Exception("not found");

            offer.Request.Status = 2;
            offer.OfferStatus = 2;

            _rquestRepository.Update(offer.Request);
            CurrentUnitOfWork.SaveChanges();


            var userToken = _notificationTokenRepository.GetAll()
                .Where(n => n.UserId == offer.Request.UserRequsetId)
                .Select(n => n.Token).FirstOrDefault();


            string title = "عروض الاسعار";
            string body = "لقد تم تقديم عرض سعر جديد لطلبك";

            _notificationCenter.SendPushNotification(new string[] { userToken }, title, body, new
            {
                Type = 1,
                Id = offer.Id
            });

            _notificationLogRepository.Insert(new NotificationLog()
            {
                UserId = offer.Request.UserRequsetId,
                NotificationTitle = title,
                NotificationBody = body
            });
            //log noti




            return new RequestWOfferPriceDto()
            {
                IsDriverRead = offer.IsDriverRead,
                IsRead = offer.IsRead,
                ClientName = offer.ClientName,
                ClientRate = offer.ClientRate,
                IsClientRated = offer.IsClientRated,
                IsRated = offer.IsRated,
                Rate = offer.Rate,
                OfferStatus = offer.OfferStatus,
                DriverName = offer.DriverName,
                AwayFrom = offer.AwayFrom,
                DeliveryCost = (double)offer.DeliveryCost,
                DeliveryThroughDays = offer.DeliveryThroughDays,
                DeliveryThroughHours = offer.DeliveryThroughHours,
                DeliveryThroughMinutes = offer.DeliveryThroughMinutes,
                DeliveryThroughSeconds = offer.DeliveryThroughSeconds,
                DriverId = offer.DriverId,
                OfferPriceId = offer.Id,
                RequestId = offer.RequestId,
                ArrivalDateTime = offer.Request.ArrivalDateTime,
                DiscountPercentage = offer.Request.DiscountPercentage,
                EndingPoint = offer.Request.EndingPoint,
                EndingPointAdress = offer.Request.EndingPointAdress,
                EndingPointTitle = offer.Request.EndingPointTitle,
                Notes = offer.Request.Notes,
                PaymentWay = offer.Request.PaymentWay,
                StartingPoint = offer.Request.StartingPoint,
                Status = offer.Request.Status,
                StratingPointAdress = offer.Request.StratingPointAdress,
                StratingPointTitle = offer.Request.StratingPointTitle,
                SubcategoryId = offer.Request.SubcategoryId
            };
        }



        //تم القراءة لاشعار عرض السعر
        public RequestWOfferPriceDto ClientMarkOfferPriceRead(long offerPriceId)
        {
            var userId = AbpSession.GetUserId();
            var offer = _repository.GetAll().Include(o => o.Request)
                .Where(o => o.Id == offerPriceId && o.Request.UserRequsetId == userId)
                .FirstOrDefault();

            if (offer == null)
                throw new Exception("not found");

            offer.IsRead = true;

            _repository.Update(offer);
            CurrentUnitOfWork.SaveChanges();

            return new RequestWOfferPriceDto()
            {
                IsDriverRead = offer.IsDriverRead,
                ClientName = offer.ClientName,
                IsClientRated = offer.IsClientRated,
                ClientRate = offer.ClientRate,
                IsRead = offer.IsRead,
                IsRated = offer.IsRated,
                Rate = offer.Rate,
                OfferStatus = offer.OfferStatus,
                DriverName = offer.DriverName,
                AwayFrom = offer.AwayFrom,
                DeliveryCost = (double)offer.DeliveryCost,
                DeliveryThroughDays = offer.DeliveryThroughDays,
                DeliveryThroughHours = offer.DeliveryThroughHours,
                DeliveryThroughMinutes = offer.DeliveryThroughMinutes,
                DeliveryThroughSeconds = offer.DeliveryThroughSeconds,
                DriverId = offer.DriverId,
                OfferPriceId = offer.Id,
                RequestId = offer.RequestId,
                ArrivalDateTime = offer.Request.ArrivalDateTime,
                DiscountPercentage = offer.Request.DiscountPercentage,
                EndingPoint = offer.Request.EndingPoint,
                EndingPointAdress = offer.Request.EndingPointAdress,
                EndingPointTitle = offer.Request.EndingPointTitle,
                Notes = offer.Request.Notes,
                PaymentWay = offer.Request.PaymentWay,
                StartingPoint = offer.Request.StartingPoint,
                Status = offer.Request.Status,
                StratingPointAdress = offer.Request.StratingPointAdress,
                StratingPointTitle = offer.Request.StratingPointTitle,
                SubcategoryId = offer.Request.SubcategoryId
            };
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="offerPriceId"></param>
        /// <returns></returns>
        //تم القراءة لاشعار عرض السعر للسائق
        public void DriverMarkOfferPriceRead(long NotId)
        {
            var userId = AbpSession.GetUserId();
            var notif = _driverNotification.GetAll()
                .Where(o => o.DriverId == userId && o.Id == NotId)
                .FirstOrDefault();

            if (notif == null)
                throw new Exception("not found");

            notif.IsRead = true;

            _driverNotification.Update(notif);
            CurrentUnitOfWork.SaveChanges();

        }

        //قبول عرض سعر
        public async Task<RequestWOfferPriceDto> AcceptClientOfferPrice(long offerPriceId)
        {
            var userId = AbpSession.GetUserId();
            var offer = _repository.GetAll().Include(o => o.Request)
                .Where(o => o.Id == offerPriceId && o.Request.UserRequsetId == userId)
                .FirstOrDefault();

            if (offer == null)
                throw new Exception("not found");

            offer.Request.Status = 3;
            offer.Request.DeliveryCost = offer.DeliveryCost.ToString();
            offer.DeliveryCost = offer.DeliveryCost;
            offer.IsAccepted = true;
            offer.OfferStatus = 2;

            _repository.Update(offer);
            CurrentUnitOfWork.SaveChanges();





            var userToken = _notificationTokenRepository.GetAll()
                .Where(n => n.UserId == offer.DriverId)
                .Select(n => n.Token).FirstOrDefault();


            string title = "موافقة على طلب السعر";
            string body = "لقد تم الموافقة على عرض سعر الذى قدمتة";


            var not = _driverNotification.Insert(new DriverNotification()
            {
                DriverId = offer.DriverId,
                IsRead = false,
                Title = title,
                Body = body,
                Type = 4
            });


            await _notificationCenter.SendPushNotification(new string[] { userToken }, title, body, new
            {
                NotId = not.Id,
                Type = 4,
                Id = offer.Id
            });


            CurrentUnitOfWork.SaveChanges();


            return new RequestWOfferPriceDto()
            {
                IsDriverRead = offer.IsDriverRead,
                IsRead = offer.IsRead,
                ClientName = offer.ClientName,
                IsClientRated = offer.IsClientRated,
                ClientRate = offer.ClientRate,
                IsRated = offer.IsRated,
                Rate = offer.Rate,
                OfferStatus = offer.OfferStatus,
                DriverName = offer.DriverName,
                AwayFrom = offer.AwayFrom,
                DeliveryCost = (double)offer.DeliveryCost,
                DeliveryThroughDays = offer.DeliveryThroughDays,
                DeliveryThroughHours = offer.DeliveryThroughHours,
                DeliveryThroughMinutes = offer.DeliveryThroughMinutes,
                DeliveryThroughSeconds = offer.DeliveryThroughSeconds,
                DriverId = offer.DriverId,
                OfferPriceId = offer.Id,
                RequestId = offer.RequestId,
                ArrivalDateTime = offer.Request.ArrivalDateTime,
                DiscountPercentage = offer.Request.DiscountPercentage,
                EndingPoint = offer.Request.EndingPoint,
                EndingPointAdress = offer.Request.EndingPointAdress,
                EndingPointTitle = offer.Request.EndingPointTitle,
                Notes = offer.Request.Notes,
                PaymentWay = offer.Request.PaymentWay,
                StartingPoint = offer.Request.StartingPoint,
                Status = offer.Request.Status,
                StratingPointAdress = offer.Request.StratingPointAdress,
                StratingPointTitle = offer.Request.StratingPointTitle,
                SubcategoryId = offer.Request.SubcategoryId
            };
        }

        //تققيم السائق
        public async Task<RequestWOfferPriceDto> RateDriverByOffer(RateDriverByOfferDto input)
        {
            if (input.rate < 0 || input.rate > 5)
                throw new Exception("rate must between 0 to 5");

            var userId = AbpSession.GetUserId();

            var rquestWOfferPrice = _repository.GetAll()
                .Include(o => o.Request)
                .Where(o => o.Id == input.offerPriceId && o.Request.UserRequsetId == userId &&
                (o.Request.Status == 3 || o.Request.Status == 4 || o.Request.Status == 5)
                && o.IsAccepted == true).FirstOrDefault();

            if (rquestWOfferPrice == null)
                throw new Exception("not found");

            else
            {
                rquestWOfferPrice.Rate = input.rate;
                rquestWOfferPrice.IsRated = true;
                rquestWOfferPrice.Request.Rate = input.rate;
                rquestWOfferPrice.Request.IsRated = true;
                _repository.Update(rquestWOfferPrice);





                var userToken = _notificationTokenRepository.GetAll()
                    .Where(n => n.UserId == rquestWOfferPrice.DriverId)
                    .Select(n => n.Token).FirstOrDefault();


                string title = "تقييم جديد";
                string body = "لقد تم تقيمك من قبل العميل";




                var not = _driverNotification.Insert(new DriverNotification()
                {
                    DriverId = rquestWOfferPrice.DriverId,
                    IsRead = false,
                    Title = title,
                    Body = body,
                    Type = 5
                });


                await _notificationCenter.SendPushNotification(new string[] { userToken }, title, body, new
                {
                    NotId = not.Id,
                    Type = 5,
                });


                CurrentUnitOfWork.SaveChanges();


                return new RequestWOfferPriceDto()
                {
                    IsRead = rquestWOfferPrice.IsRead,
                    IsDriverRead = rquestWOfferPrice.IsDriverRead,
                    ClientName = rquestWOfferPrice.ClientName,
                    IsClientRated = rquestWOfferPrice.IsClientRated,
                    ClientRate = rquestWOfferPrice.ClientRate,
                    IsRated = true,
                    Rate = input.rate,
                    OfferStatus = rquestWOfferPrice.OfferStatus,
                    DriverName = rquestWOfferPrice.DriverName,
                    AwayFrom = rquestWOfferPrice.AwayFrom,
                    DeliveryCost = (double)rquestWOfferPrice.DeliveryCost,
                    DeliveryThroughDays = rquestWOfferPrice.DeliveryThroughDays,
                    DeliveryThroughHours = rquestWOfferPrice.DeliveryThroughHours,
                    DeliveryThroughMinutes = rquestWOfferPrice.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = rquestWOfferPrice.DeliveryThroughSeconds,
                    DriverId = rquestWOfferPrice.DriverId,
                    OfferPriceId = rquestWOfferPrice.Id,
                    RequestId = rquestWOfferPrice.RequestId,
                    ArrivalDateTime = rquestWOfferPrice.Request.ArrivalDateTime,
                    DiscountPercentage = rquestWOfferPrice.Request.DiscountPercentage,
                    EndingPoint = rquestWOfferPrice.Request.EndingPoint,
                    EndingPointAdress = rquestWOfferPrice.Request.EndingPointAdress,
                    EndingPointTitle = rquestWOfferPrice.Request.EndingPointTitle,
                    Notes = rquestWOfferPrice.Request.Notes,
                    PaymentWay = rquestWOfferPrice.Request.PaymentWay,
                    StartingPoint = rquestWOfferPrice.Request.StartingPoint,
                    Status = rquestWOfferPrice.Request.Status,
                    StratingPointAdress = rquestWOfferPrice.Request.StratingPointAdress,
                    StratingPointTitle = rquestWOfferPrice.Request.StratingPointTitle,
                    SubcategoryId = rquestWOfferPrice.Request.SubcategoryId
                };
            }
        }

        //تققيم السائق
        public async Task<RequestWOfferPriceDto> RateDriverByRequest(RateDriverByRequestDto input)
        {
            if (input.rate < 0 || input.rate > 5)
                throw new Exception("rate must between 0 to 5");

            var userId = AbpSession.GetUserId();

            var rquestWOfferPrice = _repository.GetAll()
                .Include(o => o.Request)
                .Where(o => o.RequestId == input.requestId && o.Request.UserRequsetId == userId &&
                (o.Request.Status == 3 || o.Request.Status == 4 || o.Request.Status == 5)
                && o.IsAccepted == true).FirstOrDefault();

            if (rquestWOfferPrice == null)
                throw new Exception("not found");

            else
            {
                rquestWOfferPrice.Rate = input.rate;
                rquestWOfferPrice.IsRated = true;
                rquestWOfferPrice.Request.Rate = input.rate;
                rquestWOfferPrice.Request.IsRated = true;
                _repository.Update(rquestWOfferPrice);




                var userToken = _notificationTokenRepository.GetAll()
                    .Where(n => n.UserId == rquestWOfferPrice.DriverId)
                    .Select(n => n.Token).FirstOrDefault();


                string title = "تقييم جديد";
                string body = "لقد تم تقيمك من قبل العميل";




                var not = _driverNotification.Insert(new DriverNotification()
                {
                    DriverId = rquestWOfferPrice.DriverId,
                    IsRead = false,
                    Title = title,
                    Body = body,
                    Type = 5
                });


                await _notificationCenter.SendPushNotification(new string[] { userToken }, title, body, new
                {
                    NotId = not.Id,
                    Type = 5,
                });


                CurrentUnitOfWork.SaveChanges();


                return new RequestWOfferPriceDto()
                {
                    IsDriverRead = rquestWOfferPrice.IsDriverRead,
                    IsRead = rquestWOfferPrice.IsRead,
                    ClientName = rquestWOfferPrice.ClientName,
                    IsClientRated = rquestWOfferPrice.IsClientRated,
                    ClientRate = rquestWOfferPrice.ClientRate,
                    IsRated = true,
                    Rate = input.rate,
                    OfferStatus = rquestWOfferPrice.OfferStatus,
                    DriverName = rquestWOfferPrice.DriverName,
                    AwayFrom = rquestWOfferPrice.AwayFrom,
                    DeliveryCost = (double)rquestWOfferPrice.DeliveryCost,
                    DeliveryThroughDays = rquestWOfferPrice.DeliveryThroughDays,
                    DeliveryThroughHours = rquestWOfferPrice.DeliveryThroughHours,
                    DeliveryThroughMinutes = rquestWOfferPrice.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = rquestWOfferPrice.DeliveryThroughSeconds,
                    DriverId = rquestWOfferPrice.DriverId,
                    OfferPriceId = rquestWOfferPrice.Id,
                    RequestId = rquestWOfferPrice.RequestId,
                    ArrivalDateTime = rquestWOfferPrice.Request.ArrivalDateTime,
                    DiscountPercentage = rquestWOfferPrice.Request.DiscountPercentage,
                    EndingPoint = rquestWOfferPrice.Request.EndingPoint,
                    EndingPointAdress = rquestWOfferPrice.Request.EndingPointAdress,
                    EndingPointTitle = rquestWOfferPrice.Request.EndingPointTitle,
                    Notes = rquestWOfferPrice.Request.Notes,
                    PaymentWay = rquestWOfferPrice.Request.PaymentWay,
                    StartingPoint = rquestWOfferPrice.Request.StartingPoint,
                    Status = rquestWOfferPrice.Request.Status,
                    StratingPointAdress = rquestWOfferPrice.Request.StratingPointAdress,
                    StratingPointTitle = rquestWOfferPrice.Request.StratingPointTitle,
                    SubcategoryId = rquestWOfferPrice.Request.SubcategoryId
                };
            }
        }

        //تقديم عرض سعر من السائق
        public RequestWOfferPriceDto DriverOfferNewOfferPrice(CreateOfferPriceDto input)
        {
            var userId = AbpSession.GetUserId();
            var driver = GetDriverByUserId(userId);
            if (driver == null)
                throw new Exception("driver not found");

            var request = _rquestRepository.GetAll().Where(r => r.Id == input.RequestId).FirstOrDefault();
            var requestClient = _clientRepository.GetAll().Where(c => c.UserId == request.UserRequsetId).FirstOrDefault();

            if (request == null)
                throw new Exception("request not found");

            if (request.Status == 3 || request.Status == 4)
                throw new Exception("request not avilable now");

            var newOffer = ObjectMapper.Map<OfferPrice>(input);
            newOffer.DriverId = userId;
            newOffer.DriverName = driver.FullName;
            newOffer.VAT = 15;
            newOffer.ClientName = requestClient.FirstName + " " + requestClient.LastName;
            newOffer.IsRead = false;
            var newOfferId = _repository.InsertAndGetId(newOffer);
            CurrentUnitOfWork.SaveChanges();

            var offer = _repository.GetAll().Include(o => o.Request)
                .Where(o => o.Id == newOfferId)
                .FirstOrDefault();

            if (offer == null)
                throw new Exception("not found");

            offer.Request.Status = 2;
            offer.OfferStatus = 2;

            _rquestRepository.Update(offer.Request);
            CurrentUnitOfWork.SaveChanges();


            var userToken = _notificationTokenRepository.GetAll()
                .Where(n => n.UserId == offer.Request.UserRequsetId)
                .Select(n => n.Token).FirstOrDefault();


            string title = "عروض الاسعار";
            string body = "لقد تم تقديم عرض سعر جديد لطلبك";

            _notificationCenter.SendPushNotification(new string[] { userToken }, title, body, new
            {
                Type = 1,
                Id = offer.Id
            });

            _notificationLogRepository.Insert(new NotificationLog()
            {
                UserId = offer.Request.UserRequsetId,
                NotificationTitle = title,
                NotificationBody = body
            });
            //log noti




            return new RequestWOfferPriceDto()
            {
                IsRead = offer.IsRead,
                IsDriverRead = offer.IsDriverRead,
                ClientName = offer.ClientName,
                IsClientRated = offer.IsClientRated,
                ClientRate = offer.ClientRate,
                IsRated = offer.IsRated,
                Rate = offer.Rate,
                OfferStatus = offer.OfferStatus,
                DriverName = offer.DriverName,
                AwayFrom = offer.AwayFrom,
                DeliveryCost = (double)offer.DeliveryCost,
                DeliveryThroughDays = offer.DeliveryThroughDays,
                DeliveryThroughHours = offer.DeliveryThroughHours,
                DeliveryThroughMinutes = offer.DeliveryThroughMinutes,
                DeliveryThroughSeconds = offer.DeliveryThroughSeconds,
                DriverId = offer.DriverId,
                OfferPriceId = offer.Id,
                RequestId = offer.RequestId,
                ArrivalDateTime = offer.Request.ArrivalDateTime,
                DiscountPercentage = offer.Request.DiscountPercentage,
                EndingPoint = offer.Request.EndingPoint,
                EndingPointAdress = offer.Request.EndingPointAdress,
                EndingPointTitle = offer.Request.EndingPointTitle,
                Notes = offer.Request.Notes,
                PaymentWay = offer.Request.PaymentWay,
                StartingPoint = offer.Request.StartingPoint,
                Status = offer.Request.Status,
                StratingPointAdress = offer.Request.StratingPointAdress,
                StratingPointTitle = offer.Request.StratingPointTitle,
                SubcategoryId = offer.Request.SubcategoryId
            };
        }

        //تقيمات السائق السابقة
        public PagedResultDto<RequestWOfferPriceDto> GetDrverRates(PagedNotifOfferPriceResultRequestDto input)
        {
            var userId = AbpSession.GetUserId();
            var driver = GetDriverByUserId(userId);
            if (driver == null)
                throw new Exception("driver not found");

            var query = _repository.GetAll().Include(o => o.Request)
                .Where(o => o.DriverId == userId &&
                (o.Request.Status == 3 || o.Request.Status == 4 || o.Request.Status == 5)
                && o.IsAccepted == true);

            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var offers = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll().Include(o => o.Request)
                .Where(o => o.DriverId == userId &&
                (o.Request.Status == 3 || o.Request.Status == 4 || o.Request.Status == 5)
                && o.IsAccepted == true);


            var count = querycount.Count();

            return new PagedResultDto<RequestWOfferPriceDto>()
            {
                Items = offers.Select(r => new RequestWOfferPriceDto()
                {
                    IsDriverRead = r.IsDriverRead,
                    ClientName = r.ClientName,
                    IsClientRated = r.IsClientRated,
                    ClientRate = r.ClientRate,
                    IsRead = r.IsRead,
                    IsRated = r.IsRated,
                    Rate = r.Rate,
                    OfferStatus = r.OfferStatus,
                    DriverName = r.DriverName,
                    AwayFrom = r.AwayFrom,
                    DeliveryCost = (double)r.DeliveryCost,
                    DeliveryThroughDays = r.DeliveryThroughDays,
                    DeliveryThroughHours = r.DeliveryThroughHours,
                    DeliveryThroughMinutes = r.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = r.DeliveryThroughSeconds,
                    DriverId = r.DriverId,
                    OfferPriceId = r.Id,
                    RequestId = r.RequestId,
                    ArrivalDateTime = r.Request.ArrivalDateTime,
                    DiscountPercentage = r.Request.DiscountPercentage,
                    EndingPoint = r.Request.EndingPoint,
                    EndingPointAdress = r.Request.EndingPointAdress,
                    EndingPointTitle = r.Request.EndingPointTitle,
                    Notes = r.Request.Notes,
                    PaymentWay = r.Request.PaymentWay,
                    StartingPoint = r.Request.StartingPoint,
                    Status = r.Request.Status,
                    StratingPointAdress = r.Request.StratingPointAdress,
                    StratingPointTitle = r.Request.StratingPointTitle,
                    SubcategoryId = r.Request.SubcategoryId
                }).ToList(),
                TotalCount = count
            };
        }


        //الطلبات الحالية للسائق
        public PagedResultDto<RequestWOfferPriceDto> GetDriverCurrentRequests(PagedNotifOfferPriceResultRequestDto input)
        {
            var userId = AbpSession.GetUserId();

            var driver = GetDriverByUserId(userId);

            if (driver == null)
                throw new Exception("no driver found");

            var query = _repository.GetAll().Include(o => o.Request)
                .Where(r => r.DriverId == userId && r.IsAccepted == true &&
                r.Request.Status == 3 && (r.OfferStatus == 2 || r.OfferStatus == 3));

            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var offers = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll()
                .Where(r => r.DriverId == userId && r.IsAccepted == true &&
                r.Request.Status == 3 && (r.OfferStatus == 2 || r.OfferStatus == 3));


            var count = querycount.Count();

            return new PagedResultDto<RequestWOfferPriceDto>()
            {
                Items = offers.Select(r => new RequestWOfferPriceDto()
                {
                    IsDriverRead = r.IsDriverRead,
                    ClientName = r.ClientName,
                    IsClientRated = r.IsClientRated,
                    ClientRate = r.ClientRate,
                    IsRead = r.IsRead,
                    IsRated = r.IsRated,
                    Rate = r.Rate,
                    OfferStatus = r.OfferStatus,
                    DriverName = r.DriverName,
                    AwayFrom = r.AwayFrom,
                    DeliveryCost = (double)r.DeliveryCost,
                    DeliveryThroughDays = r.DeliveryThroughDays,
                    DeliveryThroughHours = r.DeliveryThroughHours,
                    DeliveryThroughMinutes = r.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = r.DeliveryThroughSeconds,
                    DriverId = r.DriverId,
                    OfferPriceId = r.Id,
                    RequestId = r.RequestId,
                    ArrivalDateTime = r.Request.ArrivalDateTime,
                    DiscountPercentage = r.Request.DiscountPercentage,
                    EndingPoint = r.Request.EndingPoint,
                    EndingPointAdress = r.Request.EndingPointAdress,
                    EndingPointTitle = r.Request.EndingPointTitle,
                    Notes = r.Request.Notes,
                    PaymentWay = r.Request.PaymentWay,
                    StartingPoint = r.Request.StartingPoint,
                    Status = r.Request.Status,
                    StratingPointAdress = r.Request.StratingPointAdress,
                    StratingPointTitle = r.Request.StratingPointTitle,
                    SubcategoryId = r.Request.SubcategoryId
                }).ToList(),
                TotalCount = count
            };
        }


        //الطلبات على حسب الحالة للسائق
        public PagedResultDto<RequestWOfferPriceDto> GetDriverRequests(PagedDriverOfferPriceByStatusResultRequestDto input)
        {
            var userId = AbpSession.GetUserId();

            var driver = GetDriverByUserId(userId);

            if (driver == null)
                throw new Exception("no driver found");

            var query = _repository.GetAll().Include(o => o.Request)
                .Where(r => r.DriverId == userId);

            if (input.OfferStatus.HasValue)
            {
                if (input.OfferStatus == 1)
                {
                    query = query.Where(r => r.OfferStatus == 2 && r.Request.Status == 2);
                }
                else if (input.OfferStatus == 2)
                {
                    query = query.Where(r => (r.OfferStatus == 2 || r.OfferStatus == 3) && r.Request.Status == 3 && r.IsAccepted == true);
                }
                else if (input.OfferStatus == 3)
                {
                    query = query.Where(r => (r.OfferStatus == 4) && r.Request.Status == 4 && r.IsAccepted == true);
                }
            }

            query = query.OrderByDescending(r => r.CreationTime);

            if (input.SkipCount == 0)
                input.SkipCount = 1;

            var offers = query.Skip(input.MaxResultCount * (input.SkipCount - 1)).Take(input.MaxResultCount)
                .ToList();

            var querycount = _repository.GetAll()
                .Where(r => r.DriverId == userId);

            if (input.OfferStatus.HasValue)
            {
                if (input.OfferStatus == 1)
                {
                    querycount = querycount.Where(r => r.OfferStatus == 2 && r.Request.Status == 2);
                }
                else if (input.OfferStatus == 2)
                {
                    querycount = querycount.Where(r => (r.OfferStatus == 2 || r.OfferStatus == 3) && r.Request.Status == 3 && r.IsAccepted == true);
                }
                else if (input.OfferStatus == 3)
                {
                    querycount = querycount.Where(r => (r.OfferStatus == 4) && r.Request.Status == 4 && r.IsAccepted == true);
                }
            }


            var count = querycount.Count();

            return new PagedResultDto<RequestWOfferPriceDto>()
            {
                Items = offers.Select(r => new RequestWOfferPriceDto()
                {
                    IsDriverRead = r.IsDriverRead,
                    ClientName = r.ClientName,
                    IsClientRated = r.IsClientRated,
                    ClientRate = r.ClientRate,
                    IsRead = r.IsRead,
                    IsRated = r.IsRated,
                    Rate = r.Rate,
                    OfferStatus = r.OfferStatus,
                    DriverName = r.DriverName,
                    AwayFrom = r.AwayFrom,
                    DeliveryCost = (double)r.DeliveryCost,
                    DeliveryThroughDays = r.DeliveryThroughDays,
                    DeliveryThroughHours = r.DeliveryThroughHours,
                    DeliveryThroughMinutes = r.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = r.DeliveryThroughSeconds,
                    DriverId = r.DriverId,
                    OfferPriceId = r.Id,
                    RequestId = r.RequestId,
                    ArrivalDateTime = r.Request.ArrivalDateTime,
                    DiscountPercentage = r.Request.DiscountPercentage,
                    EndingPoint = r.Request.EndingPoint,
                    EndingPointAdress = r.Request.EndingPointAdress,
                    EndingPointTitle = r.Request.EndingPointTitle,
                    Notes = r.Request.Notes,
                    PaymentWay = r.Request.PaymentWay,
                    StartingPoint = r.Request.StartingPoint,
                    Status = r.Request.Status,
                    StratingPointAdress = r.Request.StratingPointAdress,
                    StratingPointTitle = r.Request.StratingPointTitle,
                    SubcategoryId = r.Request.SubcategoryId
                }).ToList(),
                TotalCount = count
            };
        }

        //تغيير حالة عرض السعر
        public RequestWOfferPriceDto DriverChangeOfferPriceStatus(ChangeOfferPriceStatus input)
        {
            var userId = AbpSession.GetUserId();

            var driver = GetDriverByUserId(userId);

            if (driver == null)
                throw new Exception("no driver found");

            if (input.Status <= 1 || input.Status > 4)
                throw new Exception("bad request");

            var offer = _repository.GetAll().Include(o => o.Request)
                .Where(o => o.Id == input.OfferPriceId && o.DriverId == userId && o.IsAccepted == true &&
                o.Request.Status > 2)
                .FirstOrDefault();

            if (offer == null)
                throw new Exception("not found");

            offer.OfferStatus = input.Status;

            switch (input.Status)
            {
                case 2:
                    offer.Request.Status = 3;
                    break;
                case 3:
                    offer.Request.Status = 3;
                    break;
                case 4:
                    offer.Request.Status = 4;
                    break;
                default:
                    break;
            }


            _repository.Update(offer);
            CurrentUnitOfWork.SaveChanges();

            return new RequestWOfferPriceDto()
            {
                IsDriverRead = offer.IsDriverRead,
                IsRead = offer.IsRead,
                ClientName = offer.ClientName,
                IsClientRated = offer.IsClientRated,
                ClientRate = offer.ClientRate,
                IsRated = offer.IsRated,
                Rate = offer.Rate,
                OfferStatus = offer.OfferStatus,
                DriverName = offer.DriverName,
                AwayFrom = offer.AwayFrom,
                DeliveryCost = (double)offer.DeliveryCost,
                DeliveryThroughDays = offer.DeliveryThroughDays,
                DeliveryThroughHours = offer.DeliveryThroughHours,
                DeliveryThroughMinutes = offer.DeliveryThroughMinutes,
                DeliveryThroughSeconds = offer.DeliveryThroughSeconds,
                DriverId = offer.DriverId,
                OfferPriceId = offer.Id,
                RequestId = offer.RequestId,
                ArrivalDateTime = offer.Request.ArrivalDateTime,
                DiscountPercentage = offer.Request.DiscountPercentage,
                EndingPoint = offer.Request.EndingPoint,
                EndingPointAdress = offer.Request.EndingPointAdress,
                EndingPointTitle = offer.Request.EndingPointTitle,
                Notes = offer.Request.Notes,
                PaymentWay = offer.Request.PaymentWay,
                StartingPoint = offer.Request.StartingPoint,
                Status = offer.Request.Status,
                StratingPointAdress = offer.Request.StratingPointAdress,
                StratingPointTitle = offer.Request.StratingPointTitle,
                SubcategoryId = offer.Request.SubcategoryId
            };
        }

        //تقييم ا لسائق للعميل
        public RequestWOfferPriceDto RateClientByOffer(RateDriverByOfferDto input)
        {
            if (input.rate < 0 || input.rate > 5)
                throw new Exception("rate must between 0 to 5");

            var userId = AbpSession.GetUserId();

            var driver = GetDriverByUserId(userId);

            if (driver == null)
                throw new Exception("no driver found");

            var rquestWOfferPrice = _repository.GetAll()
                .Include(o => o.Request)
                .Where(o => o.Id == input.offerPriceId && o.DriverId == userId &&
                (o.Request.Status == 3 || o.Request.Status == 4 || o.Request.Status == 5)
                && o.IsAccepted == true).FirstOrDefault();

            if (rquestWOfferPrice == null)
                throw new Exception("not found");

            else
            {
                rquestWOfferPrice.ClientRate = input.rate;
                rquestWOfferPrice.IsClientRated = true;
                rquestWOfferPrice.Request.ClientRate = input.rate;
                rquestWOfferPrice.Request.IsClientRated = true;


                _repository.Update(rquestWOfferPrice);
                CurrentUnitOfWork.SaveChanges();


                return new RequestWOfferPriceDto()
                {
                    IsRead = rquestWOfferPrice.IsRead,
                    IsDriverRead = rquestWOfferPrice.IsDriverRead,
                    ClientName = rquestWOfferPrice.ClientName,
                    IsClientRated = true,
                    ClientRate = input.rate,
                    IsRated = rquestWOfferPrice.IsRated,
                    Rate = rquestWOfferPrice.Rate,
                    OfferStatus = rquestWOfferPrice.OfferStatus,
                    DriverName = rquestWOfferPrice.DriverName,
                    AwayFrom = rquestWOfferPrice.AwayFrom,
                    DeliveryCost = (double)rquestWOfferPrice.DeliveryCost,
                    DeliveryThroughDays = rquestWOfferPrice.DeliveryThroughDays,
                    DeliveryThroughHours = rquestWOfferPrice.DeliveryThroughHours,
                    DeliveryThroughMinutes = rquestWOfferPrice.DeliveryThroughMinutes,
                    DeliveryThroughSeconds = rquestWOfferPrice.DeliveryThroughSeconds,
                    DriverId = rquestWOfferPrice.DriverId,
                    OfferPriceId = rquestWOfferPrice.Id,
                    RequestId = rquestWOfferPrice.RequestId,
                    ArrivalDateTime = rquestWOfferPrice.Request.ArrivalDateTime,
                    DiscountPercentage = rquestWOfferPrice.Request.DiscountPercentage,
                    EndingPoint = rquestWOfferPrice.Request.EndingPoint,
                    EndingPointAdress = rquestWOfferPrice.Request.EndingPointAdress,
                    EndingPointTitle = rquestWOfferPrice.Request.EndingPointTitle,
                    Notes = rquestWOfferPrice.Request.Notes,
                    PaymentWay = rquestWOfferPrice.Request.PaymentWay,
                    StartingPoint = rquestWOfferPrice.Request.StartingPoint,
                    Status = rquestWOfferPrice.Request.Status,
                    StratingPointAdress = rquestWOfferPrice.Request.StratingPointAdress,
                    StratingPointTitle = rquestWOfferPrice.Request.StratingPointTitle,
                    SubcategoryId = rquestWOfferPrice.Request.SubcategoryId
                };
            }
        }

        private Driver GetDriverByUserId(long userId)
        {
            return _driverRepository.GetAll().Where(d => d.UserId == userId).FirstOrDefault();
        }

        public OfferPriceDto OfferPriceByRequestId(GetOfferPriceDto input)
        {
           

     
            var Offer = _repository.FirstOrDefault(x => x.RequestId == input.RequestId);
            if (Offer == null)  
                return new OfferPriceDto
                {
                    DriverId = 0,
                    UserId=0
                   
                };
            var driver = _driverRepository.FirstOrDefault(q => q.UserId == Offer.DriverId);
            var driverId = driver !=null?driver.Id:0;

            return new OfferPriceDto
            {
                DriverId = driverId,
                UserId=driver.UserId

            };
        }
    }
}
