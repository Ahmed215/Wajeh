using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories.Dto;
using Abp.Linq.Extensions;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Abp.Extensions;
using Wajeeh.Coupons.Dto;

namespace Wajeeh.Coupons
{
    public class CouponAppService : AsyncCrudAppService<Coupon, CouponDto, long, PagedCouponResultRequestDto, CreateCouponDto, UpdateCouponDto>, ICouponAppService
    {
       

 
        public CouponAppService(IRepository<Coupon, long> repository) : base(repository)
        {
           
        }

        protected override IQueryable<Coupon> CreateFilteredQuery(PagedCouponResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);
            query = query.WhereIf(input.CouponType.HasValue && input.CouponType > 0, t => t.CouponType == input.CouponType);
            query = query.WhereIf(input.Value.HasValue && input.Value > 0, t => t.Value == input.Value);

            if (!input.Keyword.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.Keyword);
                    string Code = filter_query["Code"];
                    query = query.WhereIf(!Code.IsNullOrEmpty(), t => t.Code.Contains(Code));

                    string CreationTime = filter_query["CreationTime"];
                    if (!CreationTime.IsNullOrEmpty())
                    {
                        DateTime creationTime = DateTime.ParseExact(CreationTime, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                        query = query.Where(t =>
                        t.CreationTime.Year == creationTime.Year &&
                        t.CreationTime.Month == creationTime.Month &&
                        t.CreationTime.Day == creationTime.Day);
                    }

                }
                catch (Exception)
                {
                    query = query.Where(t => t.Code == input.Keyword );
                }
            }

            return query;
        }
        protected override CouponDto MapToEntityDto(Coupon entity)
        {
            var CouponDto = base.MapToEntityDto(entity);
          

            return CouponDto;
        }
    }
}
