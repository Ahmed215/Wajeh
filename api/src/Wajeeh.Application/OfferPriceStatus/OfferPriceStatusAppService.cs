using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Wajeeh.OfferPriceStatus.Dto;

namespace Wajeeh.OfferPriceStatus
{
    public class OfferPriceStatusAppService : AsyncCrudAppService<OfferPriceState, OfferPriceStatusDto, long, PagedOfferPriceStatusDto, CreateOfferPriceStatusDto, UpdateOfferPriceStatusDto>, IOfferPriceStatusAppService
    {
        private readonly IRepository<OfferPriceState, long> repository;
        private readonly IHttpContextAccessor httpContext;

        public OfferPriceStatusAppService(IRepository<OfferPriceState, long> repository, IHttpContextAccessor httpContext) : base(repository)
        {
            this.repository = repository;
            this.httpContext = httpContext;
        }

        public List<OfferPriceStatusDto> GetAllOfferPriceStatus()
        {
            var offerPriceStatus = repository.GetAll().Select(q => new { Id = q.Id, Name = q.Name }).ToList();
            var offerPriceStatusDto = new List<OfferPriceStatusDto>();
            var Headers = httpContext.HttpContext.Request.Headers;
            if (Headers.ContainsKey("Content-Language"))
            {
                var lang = Headers["Content-Language"].ToString();
                if (lang.ToLower() == "ar-EG")
                {
                    offerPriceStatus = repository.GetAll().Select(q => new { Id = q.Id, Name = q.NameAr }).ToList();
                }
            }
            offerPriceStatusDto = offerPriceStatus.Select(s => MapToEntityDto(new OfferPriceState { Id = s.Id, Name = s.Name })).ToList();
            return offerPriceStatusDto;
        }
    }
}
