using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Coupons;

namespace Wajeeh.Coupons.Dto
{
    [AutoMap(typeof(Coupon))]
    public class CreateCouponDto
    {
        public string Code { get; set; }
        public CouponTypes CouponType { get; set; }
        public float Value { get; set; }
        public bool Active { get; set; }
    }
}
