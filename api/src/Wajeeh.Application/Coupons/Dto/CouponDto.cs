using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Coupons;

namespace Wajeeh.Coupons.Dto
{
    [AutoMap(typeof(Coupon))]
    public class CouponDto : EntityDto<long>
    {
        public string Code { get; set; }
        public CouponTypes CouponType { get; set; }
        public float Value { get; set; }
        public bool Active { get; set; }
    }
}
